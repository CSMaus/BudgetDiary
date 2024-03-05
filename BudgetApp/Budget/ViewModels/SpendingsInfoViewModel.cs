using Budget.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


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
            budgetJsonFilePath = LoadData.EnsureDirectoryAndSaveFile();
            BudgetItems = LoadData.LoadDataFromLocalFile(budgetJsonFilePath);

            // TODO:
            // Read current date by accesinbg date from the system
            // Read current table from the temp file
            // write the current table to the temp file (for testing)
            // maybe choose json format for table? it's the best to work with tables and it can be used in SQL
            AddItemCommand = new RelayCommand(() => AddItem());
        }

        public void AddItem()
        {
            BudgetItems.Add(new BudgetItem 
            { 
                Date = Date.Day + "." + Date.Month + "." + Date.Year,
                WeekDay = Date.DayOfWeek.ToString(),
                Category = DefineCategory(ProductName),
                ProductName = ProductName,
                ProductPrice = ProductPrice
            });
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
