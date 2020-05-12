using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Birthday
{
    public class LedFragment : Fragment
    {
        private Spinner spinner;
        private SeekBar seekbar;
        private List<string> list;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_led, container, false);

            spinner = view.FindViewById<Spinner>(Resource.Id.spinner);
            seekbar = view.FindViewById<SeekBar>(Resource.Id.seek_bar);

            list = new List<string>();
            foreach (string ledType in Enum.GetValues(typeof(LedType)))
                list.Add(ledType);

            spinner.Adapter = new ArrayAdapter(Context, Resource.Layout.support_simple_spinner_dropdown_item, list);
            spinner.ItemSelected += Spinner_ItemSelected;

            return view;
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Enum.TryParse(sender.ToString(), out LedType ledType);

            LedSelection?.Invoke(ledType, new EventArgs());
        }

        public event EventHandler LedSelection;
    }
}