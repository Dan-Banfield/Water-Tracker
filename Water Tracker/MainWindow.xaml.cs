using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Water_Tracker
{
    public partial class MainWindow : Window
    {
        //Int to keep track of water drink count
        private int waterDrinkCount = 0;
        //String to path of save file
        private string saveFilePath = @"Resources\watercount.txt";
        //Stores current date
        DateTime currentDate = DateTime.Now.Date;
        //Gets file modified time
        DateTime fileModifiedTime;

        public MainWindow()
        {
            InitializeComponent();
            //Checks for a save file
            CheckForSaveFile();
        }

        private async void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Save();
            //Wait for material design button animation to be played before performing action
            await Task.Delay(300);
            //Exit the application
            Environment.Exit(0);
        }

        private async void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            //Wait for material design button animation to be played before performing action
            await Task.Delay(300);
            //Minimize window
            WindowState = WindowState.Minimized;
        }

        private void drinkButton_Click(object sender, RoutedEventArgs e)
        {
            IncreaseValue();
        }

        private void IncreaseValue()
        {
            //Increase the waterDrinkCount variable + 1
            waterDrinkCount += 1;
            //If the waterDrinkCount variable is less than or equal to 8, execute code
            if (waterDrinkCount <= 8)
            {
                //Updates the cups drinken label to the waterDrinkCount converted to string
                cupsAmountLabel.Content = waterDrinkCount.ToString();
                //Increases the progress bar value by 12.5
                drinkProgressBar.Value += 12.5;
            }
            //When count is completed
            if (waterDrinkCount == 8)
            {
                WaterCountCompleted();
            }
        }

        private void WaterCountCompleted()
        {
            //Shows completion message
            ShowSnackbar();
            //Updates the count to a completed value 
            SaveCompletedCount();
        }

        private void SaveCompletedCount()
        {
            Save();
        }

        private void Save()
        {
            //If no file exists in the save file directory, create a new save file and write save data to it
            if (!File.Exists(saveFilePath))
            {
                //Creates the file to write saves to
                MakeSaveFile();
                //Writes save data
                WriteSaveData();
            }
            else
            {
                WriteSaveData();
            }
        }

        private void CheckForSaveFile()
        {
            //If no file exists in the save file directory, create one
            if (!File.Exists(saveFilePath))
            {
                MakeSaveFile();
            }
            else
            {
                //Creates time reference variable
                fileModifiedTime = File.GetLastWriteTime(saveFilePath).AddHours(12);
                //Checks if it has been 12 hours since the file was modified. If it has been more than 24 hours, the save data will be reset
                if (currentDate.Date > fileModifiedTime)
                {
                    ResetSaveData();
                }
                else
                {
                    LoadSaveFile();
                }
            }
        }

        private void ResetSaveData()
        {
            //Clear saved data
            File.WriteAllText(saveFilePath, string.Empty);
        }

        private void LoadSaveFile()
        {
            //If the file's text is empty, don't do anything
            if (File.ReadAllText(saveFilePath) == string.Empty)
            {
                
            }
            else
            {
                //Updates water drink count
                waterDrinkCount = int.Parse(File.ReadAllText(saveFilePath));
                //Updates cups amount label
                cupsAmountLabel.Content = waterDrinkCount;
                //Updates progress bar value
                switch (waterDrinkCount)
                {
                    case 0:
                        drinkProgressBar.Value = 0;
                        break;
                    case 1:
                        drinkProgressBar.Value = 12.5;
                        break;
                    case 2:
                        drinkProgressBar.Value = 25;
                        break;
                    case 3:
                        drinkProgressBar.Value = 37.5;
                        break;
                    case 4:
                        drinkProgressBar.Value = 50;
                        break;
                    case 5:
                        drinkProgressBar.Value = 62.5;
                        break;
                    case 6:
                        drinkProgressBar.Value = 75;
                        break;
                    case 7:
                        drinkProgressBar.Value = 87.5;
                        break;
                    case 8:
                        drinkProgressBar.Value = 100;
                        break;
                }
            }
        }

        private void MakeSaveFile()
        {
            //Creates a save file in the save file path
            File.Create(saveFilePath).Dispose();
        }

        private void WriteSaveData()
        {
            //Clears previous save data
            File.WriteAllText(saveFilePath, string.Empty);
            using (StreamWriter file = new StreamWriter(saveFilePath, true))
            {
                //Write waterDrinkCount to save file
                file.WriteLine(waterDrinkCount.ToString());
                //Dispose of stream writer
                file.Dispose();
            }
        }

        private async void ShowSnackbar()
        {
            //Sets the snackbar active
            SnackbarThree.IsActive = true;
            //Wait for 3000 mili secs before executing next task
            await Task.Delay(3000);
            //Sets the snackbar false
            SnackbarThree.IsActive = false;
        }

        private void RestartApplication()
        {
            //Open the application
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            //Shutdown the currecnt application
            Application.Current.Shutdown();
        }

        private void Background_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //Checks if the left mouse button is being held down
            switch (e.LeftButton)
            {
                case System.Windows.Input.MouseButtonState.Pressed:
                    this.DragMove();
                    break;
            }
        }

        private void Image_OnMouseDownClickCount(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //If the Water Tracker logo is clicked three consecutive times, the save data will be reset and the application will restart
            if (e.ClickCount == 3)
            {
                ResetSaveData();
                RestartApplication();
            }
        }
    }
}
