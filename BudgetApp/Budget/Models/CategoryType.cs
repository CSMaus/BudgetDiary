using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Models
{
    public class CategoryType
    {
        public string CategoryName { get; set; }
        public ObservableCollection<string> Products { get; set; } // it's the names of the separate products
        public ObservableCollection<float> Prices { get; set; } // and this is their prices
    }
}
