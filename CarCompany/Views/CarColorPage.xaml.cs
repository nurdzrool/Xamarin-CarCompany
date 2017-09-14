using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;
using System.Linq;

using Xamarin.Forms;

using CarCompany.Models;
using CarCompany.Constants;
using CarCompany.Utility;

namespace CarCompany
{
    // TODO : mmiller : localize strings
    public partial class CarColorPage : ContentPage
    {
        #region Private Constants

        private readonly List<ColorObj> colors = new List<ColorObj>() {
            new ColorObj() { content = Color.Red, name = "Red" },
            new ColorObj() { content = Color.Orange, name = "Orange" },
            new ColorObj() { content = Color.Yellow, name = "Yellow" },
            new ColorObj() { content = Color.Green, name = "Green" },
            new ColorObj() { content = Color.Blue, name = "Blue" },
            new ColorObj() { content = Color.Purple, name = "Purple" },
            new ColorObj() { content = Color.Black, name = "Black" }
        };

        private readonly List<String> days = new List<String>
        {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"               
        };

        private const int daysInWeek = 7;
        private const int weekendDays = 2;
        private const int SaturdayIndex = 5;
        private const int SundayIndex = 6;

		#endregion

		#region Private Global Variables

		// Int
		private int holidays = 0;

		private int daysToAdd = 0;
        private int totalDays = 0;

		//  Bool 
		private bool isChangesMade = false;
        private bool isError = false;

		// List
		private ObservableCollection<Result> results;

		#endregion

		#region Constructors

		public CarColorPage()
		{
            InitializeComponent();
            
			Title = "Car Color Calculator";

            // Set picker data source, and default index 
            // future version may allow changing start day
            pickStartDay.ItemsSource = days;
            pickStartDay.SelectedIndex = 0;
            pickStartDay.IsEnabled = false;

            // use reset to initialize variables
            Reset_Clicked();

            // Set tableview data to the results array
            resultsListView.ItemsSource = results;

            // Open Keyboard
            textDaysToAdd.Focus();
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
            // TODO : mmiller : add result for bad input
			Int16 output;
            Result result;

            if (ErrorChecker.isValidDaysAddedInput(text, out output, out result))
            {
                daysToAdd = output;
                isChangesMade = false;
            }
            else if (result != null)
            {
                daysToAdd = output;
                addAndScrollTo(result);
                isError = true;
            }
            // TODO : mmiller : default error?
		}

		#endregion

		#region Event Handlers - Button

		void AddDays_Clicked(object sender, EventArgs e)
		{
			// Prevents unneeded calls but insures that the number is up to day
			// Even if the keyboard is still open
			if (isChangesMade)
			{
				((IEntryController)textDaysToAdd).SendCompleted();
			}

            if(isError)
            {
                // Skip once
                isError = false;
                textDaysToAdd.Text = String.Empty;
            }
            else 
            {
				totalDays = totalDays + daysToAdd;

                var index = totalDays % daysInWeek;

				if (index < 0)
				{
                    index += daysInWeek;
				}

				if (index == SaturdayIndex || index == SundayIndex)
				{
					var dayString = $"{daysToAdd} Days Added to Original Monday";

					addAndScrollTo(new Result() { title = dayString, 
                        description = StringConstant.Formatter.formatWeekendResultLabelString(days[index], Color.Black)});
				}
				else
				{
					// TODO : mmiller : verify this will not produce decimal
					var daysToSkip = 0;

					// TODO : mmiller : refactor for starting at different days
                    if (totalDays >= daysInWeek)
					{
						daysToSkip += ((weekendDays * (totalDays / daysInWeek)) + holidays);
					}

					// Positive totalDays
					var colorIndex = (totalDays - daysToSkip) % daysInWeek;

					if (colorIndex < 0)
					{
						colorIndex += colors.Count;
					}

                    var str = StringConstant.Formatter.formatWeekdayResultLabelString(days[index], colors[colorIndex]);

					var dayString = $"{daysToAdd} Days Added to Original Monday";

					addAndScrollTo(new Result() { title = dayString, description = str });

				}
				totalDays = 0;
            }
		}

        void Reset_Clicked(object sender, EventArgs e)
        {
            Reset_Clicked();
        }

        void Reset_Clicked() 
        {
			totalDays = 0;
			pickStartDay.SelectedIndex = 0;

            if (results != null) 
            {
                results.Clear();
            }
            else 
            {
                results = new ObservableCollection<Result>();
            }

            results.Add(new Result() { title = StringConstant.intialDay,
				description = StringConstant.Formatter.formatWeekdayResultLabelString(days[pickStartDay.SelectedIndex],
																						colors[pickStartDay.SelectedIndex]) });

            textDaysToAdd.Text = String.Empty;
            textDaysToAdd.Focus();
        }

        #endregion


        #region Helper Functions

        private void addAndScrollTo(Result result)
        {
            results.Add(result);
            resultsListView.ScrollTo(results.Last(), ScrollToPosition.End, true);
        }

		#endregion
	}
}