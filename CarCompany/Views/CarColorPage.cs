using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CarCompany
{
    public class CarColorPage : ContentPage
    {
        #region UI Components
        // Pickers
        private Picker pickStartDay;

        // Entries
        private Entry txtAddDays;

        // Buttons
        // TODO : mmiller : move to FAB for Android
        // TODO : mmiller : move to menu button for iOS?
        private Button btnAddDays;

        #endregion

        #region Private Constants

        // TODO : mmiller : merge color/colorName into single object
        private List<Color> colors = new List<Color>()
        {
            Color.Red,
            Color.Orange,
            Color.Yellow,
            Color.Green,
            Color.Blue,
            Color.Purple,
            Color.Black
        };

		private List<String> colorNames = new List<String>()
		{
			"Red",
			"Orange",
			"Yellow",
			"Green",
			"Blue",
			"Purple",
			"Black"
		};


        private List<DaysOfTheWeek> days = new List<DaysOfTheWeek>()
        {
                            DaysOfTheWeek.Monday,
                            DaysOfTheWeek.Tuesday,
                            DaysOfTheWeek.Wednesday,
                            DaysOfTheWeek.Thursday,
                            DaysOfTheWeek.Friday,
                            DaysOfTheWeek.Saturday,
                            DaysOfTheWeek.Sunday
        };

        #endregion

        #region Private Global Variables

        // Int
        private int holidays = 0;
        private int weekendDays = 2;
        private int daysToAdd = 0;

        //  Bool 
        private bool isChangesMade = false;

        #endregion

        #region Constructors

        public CarColorPage()
        {
            initLabels();
            initPickers();
            initEntries();
            initButtons();

            Content = new StackLayout
            {
                Children = {
                    pickStartDay,
                    txtAddDays,
                    btnAddDays
                }
            };
        }

        #endregion

        #region Init

        private void initLabels()
        {
            Title = "Car Color Calculator";
        }

        private void initPickers()
        {
            pickStartDay = new Picker()
            {
                Title = "StartDay",
                ItemsSource = days
            };

            pickStartDay.SelectedIndex = 0;
        }

        private void initEntries()
        {
            txtAddDays = new Entry()
            {
                Text = "0",
                Keyboard = Keyboard.Numeric,
                Placeholder = "Number of Days to Add"
            };

            txtAddDays.TextChanged += txtAddDays_TextChanged;
            txtAddDays.Completed += txtAddDays_Completed; ;
        }

        private void initButtons()
        {
            // TODO : mmiller : Localize String
            btnAddDays = new Button() { Text = "Add Days" };

            btnAddDays.Clicked += btnAddDays_Clicked;
        }

		#endregion

		#region Event Handlers - Entry

		void txtAddDays_TextChanged(object sender, TextChangedEventArgs e)
		{
            isChangesMade = true;
		}

        void txtAddDays_Completed(object sender, EventArgs e)
        {
            var text = ((Entry)sender).Text; //cast sender to access the properties of the Entry

            // TODO : mmiller : can we assume daysToAdd <= int32 max
            Int32 output;

            if (Int32.TryParse(text, out output))
            {
                Console.WriteLine($"Parse success : {output}");
                daysToAdd = output;
            }
            else
            {
                Console.WriteLine("NAN: resetting to 0");
                daysToAdd = 0;
            }
            isChangesMade = false;
        }

        #endregion

        #region Event Handlers - Button

        void btnAddDays_Clicked(object sender, EventArgs e)
        {
            // Prevents unneeded calls but insures that the number is up to day
            // Even if the keyboard is still open
            if (isChangesMade) {
				((IEntryController)txtAddDays).SendCompleted();
				Console.WriteLine($"Days to Add : {daysToAdd}");
			}

			var startIndex = (pickStartDay.SelectedIndex);
            var moveNum = (daysToAdd % 7);

            var totalDays = startIndex + daysToAdd;

            // TODO : mmiller : verify this will not produce decimal
            var daysToSkip = holidays;

            if (totalDays >= 7)
            {
                daysToSkip += weekendDays * (totalDays / 7);
            }

            var finalIndex = startIndex + moveNum - daysToSkip;

            if(totalDays != 0 && (moveNum == 5 || moveNum == 6)) 
            {
				Console.WriteLine("==========================");
				Console.WriteLine($"Day : {days[totalDays % 7]}");
				Console.WriteLine($"Color : No Color");
				Console.WriteLine("==========================");
			}
            else 
            {
				if (finalIndex < 0)
				{
					finalIndex += colors.Count;
				}

				Console.WriteLine("==========================");
				Console.WriteLine($"Day : {days[totalDays % 7]}");
				Console.WriteLine($"Color : {colorNames[finalIndex]}");
				Console.WriteLine("==========================");
            }
        }
        #endregion
    }
}

