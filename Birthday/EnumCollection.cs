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

    public enum Delay
    {
        Epilepsie = 21,
        Schnell = 50,
        Mittel = 90,
        Langsam = 140
    }

    public enum Music
    {
        Cantina = 22,
        Imperial = 23,
        Mario = 24,
        Hedwig = 25,
        Birthday = 26
    }
}