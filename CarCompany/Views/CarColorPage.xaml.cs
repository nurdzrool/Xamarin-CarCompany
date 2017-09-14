using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;

using Xamarin.Forms;

using CarCompany.Models;

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

			if (Int16.TryParse(text, out output))
			{
				Console.WriteLine($"Parse success : {output}");
				daysToAdd = output;

				if (daysToAdd < 0)
				{
					daysToAdd = 0;

					var fs = new FormattedString();
					fs.Spans.Add(new Span() { Text = "Please enter a positive whole number." });
					var dayString = "Must Be a Future Day";

					addAndScrollTo(new Result() { title = dayString, description = fs });

					isError = true;
				}
			}
			else
			{
                Int64 bigInt;
                Int32 mediumInt;

				if (Int64.TryParse(text, out bigInt))
				{
					Console.WriteLine("Number Too Big");
					daysToAdd = 0;

					var fs = new FormattedString();
					fs.Spans.Add(new Span() { Text = "Number must be less than 32,768" });
					var dayString = "Number Too Big";

					addAndScrollTo(new Result() { title = dayString, description = fs });

					isError = true;
				}
				else if(Int32.TryParse(text, out mediumInt))
				{
					Console.WriteLine("Number Too Big");
					daysToAdd = 0;

					var fs = new FormattedString();
					fs.Spans.Add(new Span() { Text = "Number must be less than 32,768" });
					var dayString = "Number Too Big";

					addAndScrollTo(new Result() { title = dayString, description = fs });

					isError = true;
				}
                else 
                {
					Console.WriteLine("NAN: resetting to 0");
					daysToAdd = 0;

					var fs = new FormattedString();
					fs.Spans.Add(new Span() { Text = "Please enter a positive whole number." });
					var dayString = "Error";

					addAndScrollTo(new Result() { title = dayString, description = fs });

					isError = true;   
                }
			}
			isChangesMade = false;
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
				Console.WriteLine($"Days to Add : {daysToAdd}");
			}

            if(isError)
            {
                // Skip once
                isError = false;
            }
            else 
            {
				totalDays = totalDays + daysToAdd;

				var index = totalDays % 7;

				if (index < 0)
				{
					index += 7;
				}

				if (index == 5 || index == 6)
				{
					var str = formatResultLabelString(days[index], "No Color", Color.Black, true);

					var dayString = $"{daysToAdd} Days Added to Original Monday";

					addAndScrollTo(new Result() { title = dayString, description = str });

					// for testing
					Console.WriteLine("==========================");
					Console.WriteLine($"Day : {days[index]}");
					Console.WriteLine($"Color : No Color");
					Console.WriteLine("==========================");
				}
				else
				{
					// TODO : mmiller : verify this will not produce decimal
					var daysToSkip = 0;

					// TODO : mmiller : refactor for starting at different days
					if (totalDays >= 7)
					{
						daysToSkip += ((weekendDays * (totalDays / 7)) + holidays);
					}

					// Positive totalDays
					var colorIndex = (totalDays - daysToSkip) % 7;

					if (colorIndex < 0)
					{
						colorIndex += colors.Count;
					}

                    var str = formatResultLabelString(days[index], colors[colorIndex].name, colors[colorIndex].content, false);

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
			pickStartDay.SelectedIndex = totalDays % 7;

            var initialString = formatResultLabelString(days[pickStartDay.SelectedIndex], colors[pickStartDay.SelectedIndex].name, colors[pickStartDay.SelectedIndex].content, false);
            if (results != null) 
            {
                results.Clear();
            }
            else 
            {
                results = new ObservableCollection<Result>();
            }

            results.Add(new Result() { title = "Intial Day Set to Monday", description = initialString });

            textDaysToAdd.Text = "";
            textDaysToAdd.Focus();
        }

        #endregion

        #region Calculation

        #endregion

        #region Helper Functions

        private FormattedString formatResultLabelString(String day, String colorName, Color color, bool isWeekend)
		{
			var fs = new FormattedString();
			if (isWeekend)
			{
				fs.Spans.Add(new Span() { Text = $"No cars produced on this {day}" });
			}
			else
			{
				fs.Spans.Add(new Span { Text = $"Cars on this {day} will be : ", ForegroundColor = Color.Black });
				fs.Spans.Add(new Span { Text = colorName, ForegroundColor = color });
			}

			return fs;
		}

        private void addAndScrollTo(Result result)
        {
            results.Add(result);
            resultsListView.ScrollTo(results, ScrollToPosition.End, true);
        }

		#endregion
	}
}