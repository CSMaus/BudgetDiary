using Budget.Models;
using Budget.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Budget.Views
{
    public partial class SpendingsInfoView : UserControl
    {
        public SpendingsInfoView()
        {
            InitializeComponent();
            this.DataContext = new SpendingsInfoViewModel();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var viewModel = DataContext as SpendingsInfoViewModel;
                viewModel?.AddItemCommand.Execute(null);
            }
        }
    }
}
