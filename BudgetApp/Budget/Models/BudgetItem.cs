using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Models
{
    public class BudgetItem
    {
        // think, that for each item need to create property change event
        // and also apdate collection of this items when property changed
        private string date;
        public string Date
        {
            get { return date; }
            set 
            { 
                date = value;
                OnPropertyChanged("Date");
            }
        }
        
        private string weekDay;
        public string WeekDay
        {
            get { return weekDay; }
            set
            {
                weekDay = value;
                OnPropertyChanged("WeekDay");
            }
        }
        private string category;
        public string Category
        {
            get { return category;}
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }
        private string incomeType;
        public string IncomeType
        {
            get { return incomeType; }
            set
            {
                incomeType = value;
                OnPropertyChanged("IncomeType");
            }
        }
        private string productName;
        public string ProductName
        {
            get { return productName; }
            set
            {
                productName = value;
                OnPropertyChanged("ProductName");
            }
        }
        private float productPrice;
        public float ProductPrice
        {
            get { return productPrice; }
            set
            {
                productPrice = value;
                OnPropertyChanged("ProductPrice");
            }
        }
        //public ObservableCollection<CategoryType> Categories { get; set; } // this is the names of CATEGORIES
        //public ObservableCollection<float> Prices { get; set; }  // and this is summ prices of all items for each category

        // actually, it should looks like this. hmm, no. better another approach.
        // {"date1": {"Category 1": [(float)TotallPrice, {"ProductName1": (float)price1, "ProductName2": (float)price2, ...}], "Category2":...}, "date2":...}


        // I want to be able to take a look at the totall spend to each category in different days, so, the category have contain items
        // and! better each item to have a price, date, and category parameter.
        // and in table I'll add the result of them
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
