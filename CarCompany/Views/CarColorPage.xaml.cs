using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace CarCompany
{
	// TODO : mmiller : localize strings
	public partial class CarColorPage : ContentPage
	{ 
		#region Private Constants

		// TODO : mmiller : merge color/colorName into single object
        // TODO : mmiller : make constant?
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

        private List<String> days = new List<String>
        {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"               
        };

		#endregion

		#region Private Global Variables

		// Int
		private int holidays = 0;
		private int weekendDays = 2;
		private int daysToAdd = 0;
        private int totalDays = 0;

		//  Bool 
		private bool isChangesMade = false;

		// List
		private ObservableCollection<Result> results;

		#endregion

		#region Constructors

		public CarColorPage()
		{
            InitializeComponent();

            //BindingContext = viewModel = new ResultsViewModel();
            
			Title = "Car Color Calculator";

            pickStartDay.ItemsSource = days;
            pickStartDay.SelectedIndex = 0;
            pickStartDay.IsEnabled = false;

            Reset_Clicked();

            resultsListView.ItemsSource = results;
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

		void AddDays_Clicked(object sender, EventArgs e)
		{
			// Prevents unneeded calls but insures that the number is up to day
			// Even if the keyboard is still open
			if (isChangesMade)
			{
				((IEntryController)textDaysToAdd).SendCompleted();
				Console.WriteLine($"Days to Add : {daysToAdd}");
			}
			
			totalDays = totalDays + daysToAdd;

			// TODO : mmiller : verify this will not produce decimal
			var daysToSkip = 0;

            // TODO : mmiller : refactor for starting at different days
            if (totalDays >= 7)
			{
                daysToSkip += -1*((weekendDays * (totalDays / 7)) + holidays);
			}
            
            var index = totalDays % 7;

            if(index < 0){
                index += 7;
            }

            var moveNum = (daysToAdd % 7);

            if (index == 5 || index == 6)
			{
				var str = formatResultLabelString(days[index], "No Color", Color.Black, true);

				var dayString = $"{daysToAdd} Days Added to Original Monday";

                addAndScrollTo(new Result() { colorString = str, daysAddedString = dayString });

				// for testing
				Console.WriteLine("==========================");
				Console.WriteLine($"Day : {days[index]}");
				Console.WriteLine($"Color : No Color");
				Console.WriteLine("==========================");
			}
			else
			{
                // Positive totalDays
                var colorIndex = (totalDays + daysToSkip) % 7;

				if (colorIndex < 0)
				{
					colorIndex += colors.Count;
				}

                // Negative totalDays
                if(totalDays < 0) 
                {
                    colorIndex = (totalDays - daysToSkip) % 7;

					if (colorIndex < 0)
					{
						colorIndex += colors.Count;
					}
                }

				var str = formatResultLabelString(days[index], colorNames[colorIndex], colors[colorIndex], false);

                var dayString = $"{daysToAdd} Days Added to Original Monday";

                addAndScrollTo(new Result() { colorString = str, daysAddedString = dayString});
				// for testing
				Console.WriteLine("==========================");
				Console.WriteLine($"Day : {days[index]}");
				Console.WriteLine($"Color : {colorNames[colorIndex]}");
				Console.WriteLine("==========================");
			}
            totalDays = 0;
		}

        void Reset_Clicked(object sender, EventArgs e)
        {
            Reset_Clicked();
        }

        void Reset_Clicked() 
        {
			totalDays = 0;
			pickStartDay.SelectedIndex = totalDays % 7;

            var initialString = formatResultLabelString(days[pickStartDay.SelectedIndex], colorNames[pickStartDay.SelectedIndex], colors[pickStartDay.SelectedIndex], false);
            if (results != null) {
                results.Clear();
            }
            else {
                results = new ObservableCollection<Result>();
            }
            results.Add(new Result() { colorString=initialString, daysAddedString="Intial Day Set to Monday"});

            textDaysToAdd.Text = "";
            textDaysToAdd.Focus();
        }

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