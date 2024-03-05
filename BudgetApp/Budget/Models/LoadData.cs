using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Budget.Models
{
    public static class LoadData
    {
        // here will be logic to load data from local file or from Googlr Drive - depends on User's choice
        // to the time save to local file onle. Saving to Google Drive will be implemented after basing app functionality

        public static string EnsureDirectoryAndSaveFile()
        {
            string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string budgetDiaryFolderPath = Path.Combine(localAppDataPath, "BudgetDiary");
            string budgetFilePath = Path.Combine(budgetDiaryFolderPath, "budget.json");

            if (!Directory.Exists(budgetDiaryFolderPath))
            {
                Directory.CreateDirectory(budgetDiaryFolderPath);
            }
            return budgetFilePath;
        }

        public static ObservableCollection<BudgetItem> LoadDataFromLocalFile(string filePath)
        {
            ObservableCollection<BudgetItem> BudgetItemsTable = new ObservableCollection<BudgetItem>();
            if (File.Exists(filePath))
            {
                try
                {
                    string fileContent = File.ReadAllText(filePath);
                    BudgetItemsTable = JsonConvert.DeserializeObject<ObservableCollection<BudgetItem>>(fileContent);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                MessageBox.Show("Saved budget data not found.");
            }

            return BudgetItemsTable;
        }

        public static void SaveDataToLocalFile(string filePath, ObservableCollection<BudgetItem> BudgetItemsTable)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(BudgetItemsTable, Formatting.Indented);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show("Error saving budget data.");
            }
        }
    }
    /*
     JSON example
    [
      {
        "Date": "2024-03-15",
        "ProductNames": ["Product1", "Product2"],
        "Price": [9.99, 15.99]
      },
      {
        "Date": "2024-03-16",
        "ProductNames": ["Product3", "Product4"],
        "Price": [5.99, 7.49]
      }
    ]
     */
}
