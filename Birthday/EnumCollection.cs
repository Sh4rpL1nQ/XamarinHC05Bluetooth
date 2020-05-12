using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Birthday
{
    public enum LedType
    {
        Aus = 14, 
        An = 15, 
        Nacheinander = 16,
        Blinken = 17,
        Snake = 18, 
        Farbe = 19,
        Zufällig = 20
    }

    //public enum LedPin
    //{
    //    Y1_13 = 13,
    //    R1_12 = 12,
    //    G1_11 = 11,
    //    Y2_10 = 10,
    //    R2_9 = 9,
    //    G2_8 = 8,
    //    Y3_7 = 7,
    //    R3_6 = 6,
    //    G3_5 = 5,
    //    Y4_4 = 4,
    //    R4_3 = 3
    //}
}