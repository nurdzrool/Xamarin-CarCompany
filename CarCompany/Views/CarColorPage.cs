using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CarCompany
{
    public class CarColorPage : ContentPage
    {
        #region UI Components

        private Picker pickStartDay;

        #endregion

        #region Private Constants

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

        #endregion

        #region Constructors

        public CarColorPage()
        {
            
            initLabels();
            initPickers();

            Content = new StackLayout
            {
                Children = {
                    pickStartDay
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
                ItemsSource = new List<DaysOfTheWeek>
                        {
                            DaysOfTheWeek.Monday,
                            DaysOfTheWeek.Tuesday,
                            DaysOfTheWeek.Wednesday,
                            DaysOfTheWeek.Thursday,
                            DaysOfTheWeek.Friday,
                            DaysOfTheWeek.Saturday,
                            DaysOfTheWeek.Sunday
                        }
            };

            pickStartDay.SelectedItem = DaysOfTheWeek.Monday;
        }

        #endregion

    }
}

