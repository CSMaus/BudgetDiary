using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Models
{
    public class BudgetItem
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public float Amount { get; set; }
    }
}
