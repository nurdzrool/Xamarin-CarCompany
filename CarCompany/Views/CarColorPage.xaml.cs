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

        private const byte daysInWeek = 7;
        private const byte weekendDays = 2;
        private const byte SaturdayIndex = 5;
        private const byte SundayIndex = 6;

        #endregion

        #region Private Global Variables

        // Int
        private Int16 holidays = 0;

        private Int16 daysToAdd = 0;
        private Int16 totalDays = 0;

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

        // detect changes made to the textAddDays field
        void textAddDays_TextChanged(object sender, TextChangedEventArgs e)
        {
            isChangesMade = true;
        }

        // textAddDays losses focus
        void textAddDays_Completed(object sender, EventArgs e)
        {
            var text = ((Entry)sender).Text; 
            updateDaysToAdd(text);
            AddDays_Clicked(btnAddDays, null);
        }

        #endregion

        #region Event Handlers - Button

        // TODO : mmiller : disable select click
        // TODO : mmiller : dismissing keyboard doesn't proc?
        void AddDays_Clicked(object sender, EventArgs e)
        {
            // Removes the chance of updating twice but also 
            // prevents a missed call if keyboard does not dismiss properly
            if (isChangesMade)
            {
                updateDaysToAdd(textDaysToAdd.Text);
            }
            //updateDaysToAdd(text, true);
            if (isError)
            {
                // Skip once
                isError = false;
                textDaysToAdd.Text = String.Empty;
            }
            else
            {
                totalDays = (Int16)(totalDays + daysToAdd);

                updateDisplay();

                Console.WriteLine($"color_of_the_day({totalDays})-> ‘{color_of_the_day(totalDays)}’");

                // reset total days 
                // TODO : mmiller : when we have persistant days added, have conditional
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
            isChangesMade = true;
            pickStartDay.SelectedIndex = 0;

            if (results != null)
            {
                results.Clear();
            }
            else
            {
                results = new ObservableCollection<Result>();
            }

            results.Add(new Result()
            {
                title = StringConstant.intialDay,
                description = StringConstant.Formatter.formatWeekdayResultLabelString(days[pickStartDay.SelectedIndex],
                                                                                        colors[pickStartDay.SelectedIndex])
            });

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

        #region Calculation

        private void updateDaysToAdd(String text)
        {
            isChangesMade = false;
			// TODO : mmiller : can we assume daysToAdd <= int32 max
			Int16 output;
			Result result;

			if (ErrorChecker.isValidDaysAddedInput(text, out output, out result))
			{
				daysToAdd = output;
			}
			else if (result != null)
			{
				daysToAdd = output;
				addAndScrollTo(result);
				isError = true;
                isChangesMade = true;
			}

			// TODO : mmiller : default error?
		}

		private void updateDisplay()
		{
			byte index = (byte)(totalDays % daysInWeek);

			if (index == SaturdayIndex || index == SundayIndex)
			{
				var dayString = $"{daysToAdd} Days Added to Original Monday";

				addAndScrollTo(new Result()
				{
					title = dayString,
					description = StringConstant.Formatter.formatWeekendResultLabelString(days[index], Color.Black)
				});
			}
			else
			{
				Int16 daysToSkip = 0;

				// TODO : mmiller : refactor for starting at different days
				if (totalDays >= daysInWeek)
				{
					daysToSkip += (Int16)((weekendDays * (totalDays / daysInWeek)) + holidays);
				}

				byte colorIndex = (byte)((totalDays - daysToSkip) % daysInWeek);

				// Should never happen as daysToSkip should always be less than totalDays
				// daysTooSkip = (totalDays * (2/7)
				// Doesn't hurt to check
				if (colorIndex < 0)
				{
					colorIndex += (byte)colors.Count;
				}

				addAndScrollTo(new Result()
				{
					title = $"{daysToAdd} Days Added to Original Monday",
					description = StringConstant.Formatter.formatWeekdayResultLabelString(days[index], colors[colorIndex])
				});
			}
		}

        // Condensed version to fill the original requirement of the project statement statement
        // Not using currently because i am doing more in the conditionals above
        private String color_of_the_day(Int16 daysInTheFuture)
        {
			byte index = (byte)(daysInTheFuture % daysInWeek);
			
            if (index == SaturdayIndex || index == SundayIndex)
            {
                return "No Color";
            }
            else
            {
                Int16 daysToSkip = 0;
                Int16 numberOfWeeks = (Int16)(totalDays / daysInWeek);

                if (totalDays >= daysInWeek)
                {
                    daysToSkip += (Int16)((weekendDays * numberOfWeeks) + holidays);
                }

                byte colorIndex = (byte)((totalDays - daysToSkip) % daysInWeek);

                // Should never happen as daysToSkip should always be less than totalDays
                // daysTooSkip = (totalDays * (2/7)
                // Doesn't hurt to check
                // Will be useful in the future when looking up past days
                if (colorIndex < 0)
                {
                    colorIndex += (byte)colors.Count;
                }

                return colors[colorIndex].name;
            }
        }

        #endregion
    }
}