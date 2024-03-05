using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Models
{
    public class BudgetItem
    {
        public string Date { get; set; }
        public ObservableCollection<string> ProductNames { get; set; }
        public ObservableCollection<float> Price { get; set; }
        // public string Name { get; set; }
        // public string Category { get; set; }
        // public float Amount { get; set; }
    }
}
