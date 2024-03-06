using Budget.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace Budget.Views
{
    public partial class ThemeChangeWindow : Window
    {
        // NUGet: Extended.Wpf.Toolkit
        // TODO: setup via rgb. connection RGB and Hex
        // TODO: define color via pipette (at any place, even outside window)
        private string[] ColorItems = new string[]
        {
            "Foreground",
            "MainBackground",
            "Background",
            "BordersColor",
            "OnPressed",
            "OnSelected",
            "Gradient1",
            "Gradient2"
        };

        private string FocusedTextBox = "";

        /// ////////////////////////////////  PIPETTE  //////////////////////////////////////////
        private bool _isColorPickingMode = false;

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        public ThemeChangeWindow()
        {
            InitializeComponent();
            LoadCurrentThemeColors();
        }

        private void ResetTheme_Click(object sender, RoutedEventArgs e)
        {
            ChangeAppTheme.ResetColors();
            LoadCurrentThemeColors();
        }

        private void PickColor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cursorUri = "pack://application:,,,/Budget;component/Views/Pen.cur";

                this.Cursor = new Cursor(System.Windows.Application.GetResourceStream(new Uri(cursorUri)).Stream);
                FocusedTextBox = (sender as Button).Tag.ToString();
                _isColorPickingMode = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Failed to load the Pen cursor: {ex.Message}");
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_isColorPickingMode && e.LeftButton == MouseButtonState.Pressed)
            {
                GetCursorPos(out POINT cursorPos);
                IntPtr screenDC = GetDC(IntPtr.Zero);

                uint colorRef = GetPixel(screenDC, cursorPos.X, cursorPos.Y);
                ReleaseDC(IntPtr.Zero, screenDC);

                byte r = (byte)(colorRef & 0xFF);
                byte g = (byte)((colorRef >> 8) & 0xFF);
                byte b = (byte)((colorRef >> 16) & 0xFF);

                Color color = Color.FromRgb(r, g, b);
                var colorHex = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
                CheckAndUpdateColor(FocusedTextBox, colorHex, sender);

                TextBox textBox = FindName($"color{FocusedTextBox}Hex") as TextBox;
                textBox.Text = colorHex;

                this.Cursor = Cursors.Arrow;
                _isColorPickingMode = false;
            }
        }

        private void LoadCurrentThemeColors()
        {
            var appResources = System.Windows.Application.Current.Resources;
            if (appResources.Contains("Foreground"))
            {
                var brush = appResources["Foreground"] as SolidColorBrush;
                colorForegroundHex.Text = brush.Color.ToString();
            }
            if (appResources.Contains("MainBackground"))
            {
                var brush = appResources["MainBackground"] as SolidColorBrush;
                colorMainBackgroundHex.Text = brush.Color.ToString();
            }
            if(appResources.Contains("Background"))
            {
                var brush = appResources["Background"] as SolidColorBrush;
                colorBackgroundHex.Text = brush.Color.ToString();
            }
            if(appResources.Contains("BordersColor"))
            {
                var brush = appResources["BordersColor"] as SolidColorBrush;
                colorBordersColorHex.Text = brush.Color.ToString();
            }
            if(appResources.Contains("OnPressed"))
            {
                var brush = appResources["OnPressed"] as SolidColorBrush;
                colorOnPressedHex.Text = brush.Color.ToString();
            }
            if(appResources.Contains("OnSelected"))
            {
                var brush = appResources["OnSelected"] as SolidColorBrush;
                colorOnSelectedHex.Text = brush.Color.ToString();
            }
            if(appResources.Contains("Gradient1"))
            {
                var color = (Color)appResources["Gradient1"];
                colorGradient1Hex.Text = color.ToString();
            }
            if(appResources.Contains("Gradient2"))
            {
                var color = (Color)appResources["Gradient2"];
                colorGradient2Hex.Text = color.ToString();
            }
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            // taken by https://github.com/xceedsoftware/wpftoolkit
            var colorPicker = sender as Xceed.Wpf.Toolkit.ColorPicker;
            if (colorPicker.SelectedColor.HasValue && colorPicker.Tag is string objectName)
            {
                Color newColor = colorPicker.SelectedColor.Value;
                string hexValue = $"#{newColor.R:X2}{newColor.G:X2}{newColor.B:X2}";

                // var define textBox with same tag
                TextBox textBox = FindName($"{objectName}") as TextBox;
                textBox.Text = hexValue.ToString();
                CheckAndUpdateColor(objectName, hexValue, sender);
            }
        }

        private void ColorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Tag is string objectName)
            {
                string colorHex = textBox.Text;
                CheckAndUpdateColor(objectName, colorHex, sender);
            }
        }
        private void CheckAndUpdateColor(string objectName, string colorHex, object sender)
        {
            if (colorHex.Length == 7 && colorHex[0] == '#' || colorHex.Length == 9 && colorHex[0] == '#')
            {
                try
                {
                    Color newColor = (Color)ColorConverter.ConvertFromString(colorHex);
                    ChangeAppTheme.ChangeColor(objectName, newColor);
                }
                catch (FormatException)
                {
                    ShowToolTip(sender, "Invalid color Format");
                }
            }
            else
            {
                ShowToolTip(sender, "Invalid color Format");
            }
        }

        private void ShowToolTip(object sender, string message)
        {
            if (sender is TextBox textBox)
            {
                ToolTip tooltip = new ToolTip
                {
                    Content = message,
                    IsOpen = true,
                    StaysOpen = false,
                };
                tooltip.PlacementTarget = textBox;
                tooltip.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                textBox.ToolTip = tooltip;

                var timer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
                timer.Tick += (s, args) =>
                {
                    tooltip.IsOpen = false;
                    timer.Stop();
                };
                timer.Start();
            }
        }


        private void OK_Click(object sender, RoutedEventArgs e)
        {
            var colorValues = new StringBuilder();

            foreach (var itemName in ColorItems)
            {
                if (System.Windows.Application.Current.Resources[itemName] is SolidColorBrush brush)
                {
                    colorValues.AppendLine($"{itemName}:{brush.Color}");
                }
                else if (System.Windows.Application.Current.Resources[itemName] is Color color)
                {
                    colorValues.AppendLine($"{itemName}:{color}");
                }
            }

            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = System.IO.Path.Combine(folderPath, "BudgetDiary");
            string fileName = "ThemeColors.txt";
            string fullPath = System.IO.Path.Combine(appFolder, fileName);
            Directory.CreateDirectory(appFolder);

            File.WriteAllText(fullPath, colorValues.ToString());

            MessageBox.Show($"Theme colors saved to {fullPath}");

            this.Close();
        }

    }
}
