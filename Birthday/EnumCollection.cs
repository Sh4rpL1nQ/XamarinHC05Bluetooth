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
        Off = 14, 
        On = 15, 
        ReverseSnake = 16,
        Blink = 17,
        Snake = 18, 
        Color = 19,
        Random = 20
    }

    public enum Delay
    {
        Epilepsy = 21,
        Fast = 50,
        Medium = 90,
        Slow = 140
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