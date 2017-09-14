using System;

using CarCompany.Models;
using CarCompany.Constants;

namespace CarCompany.Utility
{
    public class ErrorChecker
    {
        public static bool isNegativeDaysAddedAllowed = false;
        
        public static bool isValidDaysAddedInput(String text, out Int16 daysToAdd, out Result result)
        {
            result = null;

			if (Int16.TryParse(text, out daysToAdd))
			{
                // Negative Number Check
                if (daysToAdd < 0 && !isNegativeDaysAddedAllowed)
				{   
					result = new Result(StringConstant.Error.futureDayTitle,
                                        StringConstant.Error.futureDayDescription);

                    daysToAdd = 0;
                    return false;
				}
                return true;
			}
			else
			{
				if (ErrorChecker.isNumberTooBig(text))
				{
                    result = new Result(StringConstant.Error.numberTooBigTitle,
                                        StringConstant.Error.numberTooBigDescription);
                    daysToAdd = 0;
                    return false;
				}
				else
				{
                    result = new Result(StringConstant.Error.notANumberTitle,
                                        StringConstant.Error.notANumberDescription);

                    daysToAdd = 0;
                    return false;
				}
			}
        }

        private static bool isNumberTooBig(String text)
        {
			Int64 bigInt;
			Int32 mediumInt;

			if (Int64.TryParse(text, out bigInt))
			{
                return true;
			}
			else if (Int32.TryParse(text, out mediumInt))
			{
                return true;
			}
            return false;
        }
    }
}
