using Budget.Models;
using Budget.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Speech.Recognition;
using System.Diagnostics;
using System.Windows.Controls;


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
        // TODO: Add income/outcome
        // add autosave when app is closed
        // remove time from date
        private string budgetJsonFilePath;
        public ObservableCollection<BudgetItem> BudgetItems { get; set; }

        public DateTime Date { get; set; }

        private string productName = "Shit";
        public string ProductName
        {
            get { return productName; }
            set
            {
                productName = value;
                OnPropertyChanged("ProductName");
            }
        }
        public List<string> Categories { get; set; }

        private List<string> StandartCategories = new List<string>() 
        {
            "Income",
            "Entertainment",
            "Games",
            "Hobbies",
            "Shopping",
            "Subscriptions",
            "Health",
            "Fitness",
            "Food ",
            "Investments",
            "Transport",
            "Fees",
            "Taxes",
            "Travel",
            "Bills",
            "Gifts & Donations",
            "Education",
            "NN"
        };

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        private float productPrice = 5f;
        public float ProductPrice
        {
            get { return productPrice; }
            set
            {
                productPrice = value;
                OnPropertyChanged(nameof(ProductPrice));
            }
        }

        public ICommand AddItemCommand { get; set; }
        public ICommand StartRecCommand { get; set; }


        private SpeechRecognitionEngine recognizer;
        private bool isRecognizing;
        private string _recognizedText;

        public string RecognizedText
        {
            get => _recognizedText;
            set
            {
                _recognizedText = value;
                OnPropertyChanged(nameof(RecognizedText));
            }
        }

        public SpendingsInfoViewModel()
        {
            //foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            //{
                //Console.WriteLine(fontFamily.Source);
            //}
            LoadTheme();
            LoadCategoriesList();

            budgetJsonFilePath = LoadData.EnsureDirectoryAndSaveFile();
            BudgetItems = LoadData.LoadDataFromLocalFile(budgetJsonFilePath);
            Date = DateTime.Now;
            // TODO:
            // Read current date by accesinbg date from the system
            // Read current table from the temp file
            // write the current table to the temp file (for testing)
            // maybe choose json format for table? it's the best to work with tables and it can be used in SQL

            // TODO: speech recognition into text
            // Parse the text and ectrude the data into BudgetItem parts
            // Microsoft.CognitiveServices.Speech
            AddItemCommand = new RelayCommand(() => AddItem());

            // speech 
            StartRecCommand = new RelayCommand(() => StartRec());
            SetupSpeechRecognition();
            isRecognizing = false;
        }
        private void SetupSpeechRecognition()
        {
            // get SpeechRecognitionEngine.InstalledRecognizers() as list of installed recognizers

            var installedRecognizers = SpeechRecognitionEngine.InstalledRecognizers().ToList();
            Console.WriteLine("Installed recognizers:");
            for (int i = 0; i < installedRecognizers.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {installedRecognizers[i].Name} - {installedRecognizers[i].Culture}");
            }

            if (SpeechRecognitionEngine.InstalledRecognizers().Count > 1)
            {
                recognizer = new SpeechRecognitionEngine(SpeechRecognitionEngine.InstalledRecognizers()[1]);
                recognizer.SetInputToDefaultAudioDevice();
            }
            else
            {
                return;
            }

            recognizer.LoadGrammar(new DictationGrammar());
            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
            // 1 Open Settings (Win + I)
            // 2 Time & Language
            // 3 Language & Region
            // 4 Install Language Features - speech recognition
        }

        public void LoadCategoriesList()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = System.IO.Path.Combine(folderPath, "BudgetDiary");
            string fileName = "Categories.txt";
            string fullPath = System.IO.Path.Combine(appFolder, fileName);

            if (!Directory.Exists(appFolder))
                Directory.CreateDirectory(appFolder);

            Categories = File.Exists(fullPath) ? File.ReadAllLines(fullPath).ToList() : new List<string>(StandartCategories);
        }

        public void SaveCategoriesList()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(folderPath, "BudgetDiary");
            string fileName = "Categories.txt";
            string fullPath = Path.Combine(appFolder, fileName);

            File.WriteAllLines(fullPath, Categories);
        }

        // for now I'll place theme loading here
        public void LoadTheme()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = System.IO.Path.Combine(folderPath, "BudgetDiary");
            string fileName = "ThemeColors.txt";
            string fullPath = System.IO.Path.Combine(appFolder, fileName);

            if (File.Exists(fullPath))
            {
                string[] lines = File.ReadAllLines(fullPath);

                foreach (var line in lines)
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        string itemName = parts[0];
                        string colorValue = parts[1];
                        Color newColor = (Color)ColorConverter.ConvertFromString(colorValue);
                        ChangeAppTheme.ChangeColor(itemName, newColor);
                    }
                }
            }
        }

        public void AddItem()
        {
            BudgetItems.Add(new BudgetItem 
            { 
                Date = Date.Day + "." + Date.Month + "." + Date.Year,
                WeekDay = Date.DayOfWeek.ToString().Substring(0, 3),
                Category = SelectedCategory == "NN" ? DefineCategory(ProductName) : SelectedCategory,
                ProductName = ProductName,
                ProductPrice = ProductPrice
            });
        }

        public void StartRec()
        {
            if (!isRecognizing)
            {
                // SetInputToDefaultAudioDevice if a microphone is connected to the system,
                // otherwise use SetInputToWaveFile, SetInputToWaveStream or
                // SetInputToAudioStream to perform speech recognition from pre-recorded audio.'
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
                isRecognizing = true;
            }
            else
            {
                recognizer.RecognizeAsyncStop();
                isRecognizing = false;
            }
        }
        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            RecognizedText += e.Result.Text + " ";
            Console.WriteLine(e.Result.Text);
        }


        public string DefineCategory(string productName)
        {
            string category = "SomeCategiry"; // here will use NN to define product category
            return category;
        }

        private void BudgetItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (BudgetItem item in e.NewItems)
                {
                    item.PropertyChanged += BudgetItems_PropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (BudgetItem item in e.OldItems)
                {
                    item.PropertyChanged -= BudgetItems_PropertyChanged;
                }
            }
        }
        private void BudgetItems_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "Date")
            {
                if (sender is BudgetItem item)
                {
                    int ichan = BudgetItems.IndexOf(item);
                    // update the connected functions (they will be in analysis)
                }
            }

            if (e.PropertyName == "ProductPrice")
            {
                if (sender is BudgetItem item)
                {
                    int ichan = BudgetItems.IndexOf(item);
                    // update the connected functions (they will be in analysis)
                }
            }
            if (e.PropertyName == "ProductName")
            {
                if (sender is BudgetItem item)
                {
                    int ichan = BudgetItems.IndexOf(item);
                    // update the connected functions (they will be in analysis)
                }
            }
            if (e.PropertyName == "Category")
            {
                if (sender is BudgetItem item)
                {
                    int ichan = BudgetItems.IndexOf(item);
                    // update the connected functions (they will be in analysis)
                }
            }
        }


        /// <summary>
        /// ////////////////////////// HELPERS //////////////////////////
        /// </summary>
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
