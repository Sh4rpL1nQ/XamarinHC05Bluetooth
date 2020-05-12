using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Support = Android.Support.V4.App;
using Android.Widget;
using Android.Bluetooth;
using System;
using Android.Support.Design.Internal;

namespace Birthday
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private BluetoothConnectionService connectionService;
        private BottomNavigationView navigation;
        private ConnectFragment connect;
        private LedFragment led;
        private MusicFragment music;
        private BluetoothAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            adapter = BluetoothAdapter.DefaultAdapter;

            connect = new ConnectFragment(adapter);
            connect.InitiateConnection += Connect_InitiateConnection;

            led = new LedFragment();
            led.LedSelection += Led_LedSelection;
            led.SingleLedSelection += Led_SingleLedSelection;
            led.DelaySelected += Led_DelaySelected;

            music = new MusicFragment();
            music.MusicSelected += Music_MusicSelected;

            navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.NavigationItemSelected += Navigation_NavigationItemSelected;
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, connect)
                .Commit();
        }

        private void Led_DelaySelected(object sender, EventArgs e)
        {
            Enum.TryParse(sender.ToString(), out Delay val);
            connectionService?.WriteToSocket((int)val);
        }

        private void Music_MusicSelected(object sender, EventArgs e)
        {
            Enum.TryParse(sender.ToString(), out Music val);
            connectionService?.WriteToSocket((int)val);
        }

        private void Led_SingleLedSelection(object sender, EventArgs e)
        {
            int.TryParse(sender.ToString(), out int val);
            connectionService?.WriteToSocket(val);
        }

        private void Led_LedSelection(object sender, EventArgs e)
        {
            Enum.TryParse(sender.ToString(), out LedType ledType);
            connectionService?.WriteToSocket((int)ledType);

            if (ledType == LedType.Aus)
            {
                led.CheckUncheckLeds(false);
                led.EnableDisableLeds(true);
            }               
            else if (ledType == LedType.An)
            {
                led.CheckUncheckLeds(true);
                led.EnableDisableLeds(true);
            }              
            else
            {
                led.EnableDisableLeds(false);
            }
        }

        private void Connect_InitiateConnection(object sender, System.EventArgs e)
        {
            connectionService = new BluetoothConnectionService(this, adapter, sender as BluetoothDevice);
            connectionService.ConnectionFailed += ConnectionService_ConnectionFailed;
            connectionService.ConnectionSuccessful += ConnectionService_ConnectionSuccessful;
            connectionService.SignalReceived += ConnectionService_SignalReceived;
            connectionService.Initiate();
        }

        private void ConnectionService_SignalReceived(object sender, EventArgs e)
        {
            
        }

        private void ConnectionService_ConnectionSuccessful(object sender, EventArgs e)
        {
            connect.ProgressBar.Visibility = ViewStates.Invisible;
            var menuItem = navigation.Menu.FindItem(Resource.Id.navigation_connection_tab);
            menuItem.SetIcon(Resources.GetDrawable(Resource.Drawable.bluetooth_connected_white));
            menuItem.SetEnabled(false);            

            navigation.Menu.FindItem(Resource.Id.navigation_led_tab).SetEnabled(true);
            navigation.Menu.FindItem(Resource.Id.navigation_music_tab).SetEnabled(true);

            navigation.SelectedItemId = Resource.Id.navigation_led_tab;
        }

        private void ConnectionService_ConnectionFailed(object sender, EventArgs e)
        {
            connect.BtnConnect.Enabled = true;
        }

        private void Navigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            Support.Fragment fragment = null;
            switch (e.Item.ItemId)
            {
                case Resource.Id.navigation_connection_tab:
                    fragment = connect;
                    break;
                case Resource.Id.navigation_led_tab:
                    fragment = led;
                    break;
                case Resource.Id.navigation_music_tab:
                    fragment = music;
                    break;
            }

            if (fragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

