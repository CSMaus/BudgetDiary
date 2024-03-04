using Budget.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Budget.ViewModels
{
    // Answer the qiuestions:
    // what do I want to disply?
    // how it will be easy understandable for the user to fast check his spends?
    // how should look proper structure if user want to see, on what exactly he spend amount for certain category?
    // add posibility to display each day as circle chart? Each category, each day/month/year?
    public class SpendingsInfoViewModel
    {
        public BudgetItem BudgetItemInput { get; set; }
        public ObservableCollection<BudgetTable> BudgetTableRows { get; set; }

        public ICommand AddItemCommand { get; set; }

        public SpendingsInfoViewModel()
        {
            BudgetItemInput = new BudgetItem();
            BudgetTableRows = new ObservableCollection<BudgetTable>();

            // TODO:
            // Read current date by accesinbg date from the system
            // Read current table from the temp file
            // write the current table to the temp file (for testing)
            // maybe choose json format for table? it's the best to work with tables and it can be used in SQL
            AddItemCommand = new RelayCommand(AddItem);
        }

        public void AddItem()
        {
            BudgetTableRows.Add(new BudgetTable
            {
                DateMoney = new Date
                {
                    Day = DateTime.Now.Day.ToString(),
                    Month = DateTime.Now.Month.ToString(),
                    Year = DateTime.Now.Year.ToString()
                },
                // BudgetItem = BudgetItemInput need to calculate summ by categories
                // also, need to rethink archintechture of displayed spendings
            });
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
