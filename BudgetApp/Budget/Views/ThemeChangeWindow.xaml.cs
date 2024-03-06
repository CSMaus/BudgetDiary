using Budget.Utilities;
using System;
using System.Collections.Generic;
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
                CheckAndUpdateColor(objectName, colorHex);
            }
        }
        private void CheckAndUpdateColor(string objectName, string colorHex)
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
                    MessageBox.Show("Invalid color format");
                }
            }
            else
            {
                MessageBox.Show("Invalid color format");
            }
        }
    }
}
