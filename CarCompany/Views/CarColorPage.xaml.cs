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
        #region ViewModels

        #endregion

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

		//  Bool 
		private bool isChangesMade = false;

		// List
		private ObservableCollection<Result> results = new ObservableCollection<Result>();

		#endregion

		#region Constructors

		public CarColorPage()
		{
            InitializeComponent();

            //BindingContext = viewModel = new ResultsViewModel();
            
			Title = "Car Color Calculator";

            pickStartDay.ItemsSource = days;
            pickStartDay.SelectedIndex = 0;


            var initialString = formatResultLabelString(days[pickStartDay.SelectedIndex], colorNames[pickStartDay.SelectedIndex], colors[pickStartDay.SelectedIndex], false);
            results.Add(new Result() { colorString=initialString, daysAddedString="Intial Day Set to Monday"});

            resultsListView.ItemsSource = results;
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

		void AddDays_Clicked(object sender, EventArgs e)
		{
			// Prevents unneeded calls but insures that the number is up to day
			// Even if the keyboard is still open

			if (isChangesMade)
			{
				((IEntryController)textDaysToAdd).SendCompleted();
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

			if (totalDays != 0 && (moveNum == 5 || moveNum == 6))
			{
				var str = formatResultLabelString(days[totalDays % 7], "No Color", Color.Black, true);

				//TableRow row = LayoutInflater.Inflate(Resource.Layout.OneRow, table, true);
				results.Add(new Result() { colorString = str, daysAddedString = $"Days added = {totalDays}" });

                //resultsListView.reloadData();

				// for testing
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

				var str = formatResultLabelString(days[totalDays % 7], colorNames[finalIndex], colors[finalIndex], false);

				results.Add(new Result() { colorString = str, daysAddedString = $"Days added = {totalDays}" });
				// for testing
				Console.WriteLine("==========================");
				Console.WriteLine($"Day : {days[totalDays % 7]}");
				Console.WriteLine($"Color : {colorNames[finalIndex]}");
				Console.WriteLine("==========================");
			}
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
		#endregion
	}
}

