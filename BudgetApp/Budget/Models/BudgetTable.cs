using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Models
{
    public class BudgetTable
    {
        public Date DateMoney { get; set; }
        public ObservableCollection<string> Categories { get; set; }


    }

    
}
