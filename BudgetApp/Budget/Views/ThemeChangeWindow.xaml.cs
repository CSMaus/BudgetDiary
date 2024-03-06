using Budget.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Budget.Views
{
    public partial class ThemeChangeWindow : Window
    {
        // TODO: setup via rgb. connection RGB and Hex
        // TODO: define color by pointer (at any place, even outside window)
        private string[] ColorItems = new string[]
        {
            "Foreground",
            "BordersColor",
            "MainBackground",
            "Background",
            "OnPressed",
            "OnSelected",
            "Gradient1",
            "Gradient2"
        };
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

        private void LoadCurrentThemeColors()
        {
            if (Application.Current.Resources.Contains("Foreground"))
            {
                var brush = Application.Current.Resources["Foreground"] as SolidColorBrush;
                colorForegroundHex.Text = brush.Color.ToString();
            }
            if (Application.Current.Resources.Contains("MainBackground"))
            {
                var brush = Application.Current.Resources["MainBackground"] as SolidColorBrush;
                colorMainBackgroundHex.Text = brush.Color.ToString();
            }
            if(Application.Current.Resources.Contains("Background"))
            {
                var brush = Application.Current.Resources["Background"] as SolidColorBrush;
                colorBackgroundHex.Text = brush.Color.ToString();
            }
            if(Application.Current.Resources.Contains("BordersColor"))
            {
                var brush = Application.Current.Resources["BordersColor"] as SolidColorBrush;
                colorBordersColorHex.Text = brush.Color.ToString();
            }
            if(Application.Current.Resources.Contains("OnPressed"))
            {
                var brush = Application.Current.Resources["OnPressed"] as SolidColorBrush;
                colorOnPressedHex.Text = brush.Color.ToString();
            }
            if(Application.Current.Resources.Contains("OnSelected"))
            {
                var brush = Application.Current.Resources["OnSelected"] as SolidColorBrush;
                colorOnSelectedHex.Text = brush.Color.ToString();
            }
            if(Application.Current.Resources.Contains("Gradient1"))
            {
                var color = (Color)Application.Current.Resources["Gradient1"];
                colorGradient1Hex.Text = color.ToString();
            }
            if(Application.Current.Resources.Contains("Gradient2"))
            {
                var color = (Color)Application.Current.Resources["Gradient1"];
                colorGradient1Hex.Text = color.ToString();
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
                    // MessageBox.Show("Invalid color format");
                }
            }
            else
            {
                ShowToolTip(sender, "Invalid color Format");
                //MessageBox.Show("Invalid color format");
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
                if (Application.Current.Resources[itemName] is SolidColorBrush brush)
                {
                    colorValues.AppendLine($"{itemName}:{brush.Color}");
                }
                else if (Application.Current.Resources[itemName] is Color color)
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
