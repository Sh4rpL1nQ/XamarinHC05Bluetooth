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
        private List<LedType> list;

        #region LedButtons
        private ToggleButton btnThirteen;
        private ToggleButton btnTwelfe;
        private ToggleButton btnEleven;
        private ToggleButton btnTen;
        private ToggleButton btnNine;
        private ToggleButton btnEight;
        private ToggleButton btnSeven;
        private ToggleButton btnSix;
        private ToggleButton btnFive;
        private ToggleButton btnFour;
        private ToggleButton btnThree;
        #endregion

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_led, container, false);

            spinner = view.FindViewById<Spinner>(Resource.Id.spinner);
            seekbar = view.FindViewById<SeekBar>(Resource.Id.seek_bar);

            #region LedButtons
            btnThirteen = view.FindViewById<ToggleButton>(Resource.Id.led_thirteen);
            btnTwelfe = view.FindViewById<ToggleButton>(Resource.Id.led_twelve);
            btnEleven = view.FindViewById<ToggleButton>(Resource.Id.led_eleven);
            btnTen = view.FindViewById<ToggleButton>(Resource.Id.led_ten);
            btnNine = view.FindViewById<ToggleButton>(Resource.Id.led_nine);
            btnEight = view.FindViewById<ToggleButton>(Resource.Id.led_eight);
            btnSeven = view.FindViewById<ToggleButton>(Resource.Id.led_seven);
            btnSix = view.FindViewById<ToggleButton>(Resource.Id.led_six);
            btnFive = view.FindViewById<ToggleButton>(Resource.Id.led_five);
            btnFour = view.FindViewById<ToggleButton>(Resource.Id.led_four);
            btnThree = view.FindViewById<ToggleButton>(Resource.Id.led_three);

            btnThirteen.Click += BtnSingleLed_Click;
            btnTwelfe.Click += BtnSingleLed_Click;
            btnEleven.Click += BtnSingleLed_Click;
            btnTen.Click += BtnSingleLed_Click;
            btnNine.Click += BtnSingleLed_Click;
            btnEight.Click += BtnSingleLed_Click;
            btnSeven.Click += BtnSingleLed_Click;
            btnSix.Click += BtnSingleLed_Click;
            btnFive.Click += BtnSingleLed_Click;
            btnFour.Click += BtnSingleLed_Click;
            btnThree.Click += BtnSingleLed_Click;
            #endregion

            list = new List<LedType>();
            foreach (LedType ledType in Enum.GetValues(typeof(LedType)))
                list.Add(ledType);

            spinner.Adapter = new ArrayAdapter(Context, Resource.Layout.support_simple_spinner_dropdown_item, list);
            spinner.ItemSelected += Spinner_ItemSelected;

            return view;
        }

        private void BtnSingleLed_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            SingleLedSelection?.Invoke(b.Text, new EventArgs());
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            LedSelection?.Invoke(spinner.GetItemAtPosition(e.Position), new EventArgs());
        }

        public void CheckUncheckLeds(bool val)
        {
            btnThirteen.Checked = val;
            btnTwelfe.Checked = val;
            btnEleven.Checked = val;
            btnTen.Checked = val;
            btnNine.Checked = val;
            btnEight.Checked = val;
            btnSeven.Checked = val;
            btnSix.Checked = val;
            btnFive.Checked = val;
            btnFour.Checked = val;
            btnThree.Checked = val;
        }

        public void EnableDisableLeds(bool val)
        {
            btnThirteen.Enabled = val;
            btnTwelfe.Enabled = val;
            btnEleven.Enabled = val;
            btnTen.Enabled = val;
            btnNine.Enabled = val;
            btnEight.Enabled = val;
            btnSeven.Enabled = val;
            btnSix.Enabled = val;
            btnFive.Enabled = val;
            btnFour.Enabled = val;
            btnThree.Enabled = val;
        }

        public event EventHandler LedSelection;

        public event EventHandler SingleLedSelection;
    }
}