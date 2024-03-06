using Budget.Models;
using Budget.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Budget.ViewModels.SpendingsInfoViewModel;

namespace Budget
{
    public partial class MainWindow : Window
    {
        private SpendingsInfoView spendingsInfoView;

        // not a good solution, but to the time this:
        public ICommand OpenThemeChangeWindowCommand { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            spendingsInfoView = new SpendingsInfoView();
            BudgetTable.Navigate(spendingsInfoView);

            OpenThemeChangeWindowCommand = new RelayCommand(OpenThemeChangeWindow);
            var changeThemeButton = this.FindName("ChangeThemeButton") as Button;
            if (changeThemeButton != null)
            {
                changeThemeButton.Command = OpenThemeChangeWindowCommand;
            }
        }
        private void OpenThemeChangeWindow()
        {
            ThemeChangeWindow themeChangeWindow = new ThemeChangeWindow();
            themeChangeWindow.ShowDialog(); // ShowDialog() for modal, Show() for non-modal
        }

        public class RelayCommand : ICommand
        {
            private readonly Action _execute;
            private readonly Func<bool> _canExecute;

            public RelayCommand(Action execute, Func<bool> canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute();
            }

            public void Execute(object parameter)
            {
                _execute();
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }
    }
}
