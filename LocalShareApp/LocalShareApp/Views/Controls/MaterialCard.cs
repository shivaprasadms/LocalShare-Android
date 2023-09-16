using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LocalShareApp.Views.Controls
{
    public class MaterialCard : Frame
    {

        public MaterialCard()
        {
            Padding = 5;
            Margin = 10;
            CornerRadius = 8;
            HasShadow = true;
            BackgroundColor = Color.White; // Set the card's background color
        }
    }
}
