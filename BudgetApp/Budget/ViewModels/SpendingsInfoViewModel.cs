using Budget.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private string budgetJsonFilePath;
        public ObservableCollection<BudgetItem> BudgetItems { get; set; }

        public string Date { get; set; }
        public string ProductName { get; set; }
        public float Price { get; set; }

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
            AddItemCommand = new RelayCommand(AddItem);
        }

        public void AddItem()
        {

        }

        public void ReadCurrentTable()
        {

        }

        public void SaveCurrentTable()
        {

        }

        // need to implement RelayCommand for proper binding
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

    }

}
