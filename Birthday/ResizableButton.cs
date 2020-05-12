using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Birthday
{
    public class ResizableButtonWidth : ToggleButton
    {

        public ResizableButtonWidth(Context context, IAttributeSet attrs) : base(context, attrs)
        {

        }


        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int height = MeasureSpec.GetSize(heightMeasureSpec);
            SetMeasuredDimension(height, height);
        }
    }

    public class ResizableButtonHeight : ToggleButton
    {

        public ResizableButtonHeight(Context context, IAttributeSet attrs) : base(context, attrs)
        {

        }


        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int width = MeasureSpec.GetSize(widthMeasureSpec);
            SetMeasuredDimension(width, width);
        }
    }
}