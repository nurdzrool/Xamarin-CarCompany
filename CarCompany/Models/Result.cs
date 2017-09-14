using System;

using Xamarin.Forms;

namespace CarCompany.Models
{
    public class Result
    {
        public String title { get; set; }
        public FormattedString description { get; set; }

        public Result(String title, String description)
        {
            this.title = title;

			var fs = new FormattedString();
            fs.Spans.Add(new Span() { Text =  description});

            this.description = fs;
        }

        public Result()
        {
        }
    }
}
