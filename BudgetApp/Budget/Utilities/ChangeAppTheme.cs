using System;
using System.Windows;
using System.Windows.Media;

namespace Budget.Utilities
{
    public static class ChangeAppTheme
    {
        // Add few more standart themes (dark, light, nature (green), gray, night (dark, blue, space), etc.)
        public static void ResetColors()
        {
            ChangeColor("Foreground",     (Color)ColorConverter.ConvertFromString("#7D5A5A"));  // FromRgb(125, 90, 90));
            ChangeColor("BordersColor",   (Color)ColorConverter.ConvertFromString("#F3E1E1"));
            ChangeColor("MainBackground", (Color)ColorConverter.ConvertFromString("#FAF2F2"));
            ChangeColor("Background",     (Color)ColorConverter.ConvertFromString("#F1D1D1"));
            ChangeColor("OnPressed",      (Color)ColorConverter.ConvertFromString("#F3E1E1"));
            ChangeColor("OnSelected",     (Color)ColorConverter.ConvertFromString("#FFE4E7"));
            ChangeColor("Gradient1",      (Color)ColorConverter.ConvertFromString("#E6CCD0"));
            ChangeColor("Gradient2",      (Color)ColorConverter.ConvertFromString("#FFE6E7"));
        }
        public static void ChangeColor(string itemName, Color newColor)
        {
            var appResources = Application.Current.Resources;
            if (appResources.Contains(itemName))
            {
                var item = appResources[itemName];
                if (item is SolidColorBrush)
                {
                    var newBrush = new SolidColorBrush(newColor);
                    newBrush.Freeze();
                    appResources[itemName] = newBrush;
                }

                else if (item is Color)
                {
                    appResources[itemName] = newColor;
                }
                /*
                var appResources = Application.Current.Resources;
                if (appResources.Contains(itemName))
                {
                    ((SolidColorBrush)appResources[itemName]).Color = newColor;
                }*/
            }
        }
    }
}
