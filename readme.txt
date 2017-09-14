The most obvious solution to this problem is the brute force approach.  As we all know,
if brute force isn’t working then you aren’t using enough of it. While easiest, it 
definitely is not the most efficient.  Instead I decided to calculate the date using mod (%)
to get the day of the week and the number of weeks to get the color offset given weekends.

While I do not use this function to display the content on screen, it condenses down to 

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


This application was written with several assumptions in mind:
	- Not inputting a number should be considered an error instead of assuming it to be zero
	- I limited the number of days that can be added to a small int as this would still allow		32,767 days which translates to 89.7 years. (if my boss is still working at that 		time may God have mercy on his soul)
	- seven days in a week
	- two day weekends
	- no holidays
	- number of colors will be below 255 allowing me to minimize memory usage.

Other limitations in the program based on the prompt
	- “days in the future” implies only positive numbers should be considered valid input
	- always starts on “current” monday 
	- always starts with red as the current color

Tested on
	- iPhone 7 10.3.1 (emulator)
	- Nexus 6p

I will be updating the “dev” branch but will leave the master branch alone for your consideration.

https://github.com/nurdzrool/Xamarin-CarCompany