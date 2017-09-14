using System;
using Xamarin.Forms;
using CarCompany.Models;

namespace CarCompany.Constants
{
    // TODO : mmiller : localize
    public class StringConstant
    {
        public static String intialDay = "Intial Day Set to Monday";

        public class Error
        {
            public static String futureDayTitle = "Must Be A Future Day";
            public static String futureDayDescription = "Please enter a positive whole number.";

            public static String numberTooBigTitle = "Number Too Big";
            public static String numberTooBigDescription = "Number must be less than 32,768";

            public static String notANumberTitle = "Error";
            public static String notANumberDescription = "Please enter a positive whole number.";
        }

        public class Formatter
        {
			public static FormattedString formatWeekdayResultLabelString(String day, ColorObj color)
			{
				var fs = new FormattedString();
				fs.Spans.Add(new Span { Text = $"Cars on this {day} will be : ", ForegroundColor = Color.Black });
                fs.Spans.Add(new Span { Text = color.name, ForegroundColor = color.content });		

				return fs;
			}

            public static FormattedString formatWeekendResultLabelString(String day, Color color)
            {
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = $"No cars produced on this {day}" });

                return fs;
            }
        }
    }
}
