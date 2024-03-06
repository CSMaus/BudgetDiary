using Budget.Models;
using Budget.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


// NuGet Google.Apis.Drive.v3
// NuGet Newtonsoft.Json

namespace Budget.ViewModels
{
    // Answer the qiuestions:
    // what do I want to disply?
    // how it will be easy understandable for the user to fast check his spends?
    // how should look proper structure if user want to see, on what exactly he spend amount for certain category?
    // add posibility to display each day as circle chart? Each category, each day/month/year?

    public class SpendingsInfoViewModel
    {
        // TODO: Add income/outcome
        // add autosave when app is closed
        // remove time from date
        private string budgetJsonFilePath;
        public ObservableCollection<BudgetItem> BudgetItems { get; set; }

        public DateTime Date { get; set; }

        public string ProductName { get; set; }
        public float ProductPrice { get; set; }

        public ICommand AddItemCommand { get; set; }

        public SpendingsInfoViewModel()
        {
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                Console.WriteLine(fontFamily.Source);
            }
            LoadTheme();

            budgetJsonFilePath = LoadData.EnsureDirectoryAndSaveFile();
            BudgetItems = LoadData.LoadDataFromLocalFile(budgetJsonFilePath);
            Date = DateTime.Now;
            // TODO:
            // Read current date by accesinbg date from the system
            // Read current table from the temp file
            // write the current table to the temp file (for testing)
            // maybe choose json format for table? it's the best to work with tables and it can be used in SQL
            AddItemCommand = new RelayCommand(() => AddItem());
        }

        // for now I'll place theme loading here
        public void LoadTheme()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = System.IO.Path.Combine(folderPath, "BudgetDiary");
            string fileName = "ThemeColors.txt";
            string fullPath = System.IO.Path.Combine(appFolder, fileName);

            if (File.Exists(fullPath))
            {
                string[] lines = File.ReadAllLines(fullPath);

                foreach (var line in lines)
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        string itemName = parts[0];
                        string colorValue = parts[1];
                        Color newColor = (Color)ColorConverter.ConvertFromString(colorValue);
                        ChangeAppTheme.ChangeColor(itemName, newColor);
                    }
                }

                // MessageBox.Show("Theme colors loaded successfully.");
            }
            else
            {
                // MessageBox.Show("Theme color file not found.");
            }
        }

        public void AddItem()
        {
            BudgetItems.Add(new BudgetItem 
            { 
                Date = Date.Day + "." + Date.Month + "." + Date.Year,
                WeekDay = Date.DayOfWeek.ToString().Substring(0, 3),
                Category = DefineCategory(ProductName),
                ProductName = ProductName,
                ProductPrice = ProductPrice
            });
            ProductName = "";
            ProductPrice = 0;
        }

        public string DefineCategory(string productName)
        {
            string category = "SomeCategiry"; // here will use NN to define product category
            return category;
        }

        private void BudgetItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (BudgetItem item in e.NewItems)
                {
                    item.PropertyChanged += BudgetItems_PropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (BudgetItem item in e.OldItems)
                {
                    item.PropertyChanged -= BudgetItems_PropertyChanged;
                }
            }
        }
        private void BudgetItems_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "Date")
            {
                if (sender is BudgetItem item)
                {
                    int ichan = BudgetItems.IndexOf(item);
                    // update the connected functions (they will be in analysis)
                }
            }

            if (e.PropertyName == "ProductPrice")
            {
                if (sender is BudgetItem item)
                {
                    int ichan = BudgetItems.IndexOf(item);
                    // update the connected functions (they will be in analysis)
                }
            }
            if (e.PropertyName == "ProductName")
            {
                if (sender is BudgetItem item)
                {
                    int ichan = BudgetItems.IndexOf(item);
                    // update the connected functions (they will be in analysis)
                }
            }
            if (e.PropertyName == "Category")
            {
                if (sender is BudgetItem item)
                {
                    int ichan = BudgetItems.IndexOf(item);
                    // update the connected functions (they will be in analysis)
                }
            }
        }


        /// <summary>
        /// ////////////////////////// HELPERS //////////////////////////
        /// </summary>
        public class RelayCommand : ICommand
        {
            private readonly Action _execute;
            public RelayCommand(Action execute)
            {
                _execute = execute;
            }
            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public void Execute(object parameter)
            {
                _execute();
            }
        }

        // implement norify property changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
