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
using Android.Bluetooth;
using Android.Content.PM;
using Android;
using System.Threading.Tasks;

namespace Birthday
{
    public class ConnectFragment : Fragment
    {
        public ImageButton BtnConnect { get; private set; }
        public ProgressBar ProgressBar { get; private set; }

        private BluetoothAdapter adapter;        

        private BluetoothReceiver bluetoothReceiver;
        private DeviceReceiver deviceReceiver;
        private PairingReceiver pairingReceiver;

        private IntentFilter bluetoothFilter, deviceFilter, pairingFilter;

        public ConnectFragment(BluetoothAdapter adapter)
        {
            this.adapter = adapter;

            bluetoothFilter = new IntentFilter(BluetoothAdapter.ActionStateChanged);
            deviceFilter = new IntentFilter(BluetoothDevice.ActionFound);
            pairingFilter = new IntentFilter(BluetoothDevice.ActionBondStateChanged);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);  
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_connection, container, false);

            BtnConnect = view.FindViewById<ImageButton>(Resource.Id.btn_device_search);
            BtnConnect.Click += BtnConnect_Click;
            BtnConnect.Enabled = adapter.IsEnabled;

            ProgressBar = view.FindViewById<ProgressBar>(Resource.Id.progress_bar);

            bluetoothReceiver = new BluetoothReceiver();
            bluetoothReceiver.BluetoothStateChanged += BluetoothReceiver_BluetoothStateChanged;            

            deviceReceiver = new DeviceReceiver();
            deviceReceiver.DeviceFound += DeviceReceiver_DeviceFound;            

            pairingReceiver = new PairingReceiver();
            pairingReceiver.DeviceConnected += PairingReceiver_DeviceConnected;          

            return view;
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            if (adapter == null)
            {
                Toast.MakeText(Context, Resource.String.error_no_bluetooth, ToastLength.Long).Show();
                return;
            }

            if (!adapter.IsEnabled)
                return;

            BtnConnect.Enabled = false;

            ProgressBar.Visibility = ViewStates.Visible;

            if (adapter.IsDiscovering)
                adapter.CancelDiscovery();

            CheckBTPermission();

            adapter.StartDiscovery();            
        }

        private void DeviceReceiver_DeviceFound(object sender, EventArgs e)
        {
            var device = sender as BluetoothDevice;
            if (device.Name != null && device.Name.Equals("HC-06"))
            {
                if (device.BondState == Bond.Bonded)
                    EstablishConnection(device);
                else
                    device.CreateBond();
            }
        }

        private void BluetoothReceiver_BluetoothStateChanged(object sender, EventArgs e)
        {
            if ((State)sender == State.Off)
                BtnConnect.Enabled = false;
            else
                BtnConnect.Enabled = true;
        }

        private void PairingReceiver_DeviceConnected(object sender, EventArgs e)
        {
            var device = sender as BluetoothDevice;
            EstablishConnection(device);
        }

        private async void EstablishConnection(BluetoothDevice device)
        {
            await Task.Run(() =>
            {
                adapter.CancelDiscovery();
            });

            InitiateConnection?.Invoke(device, new EventArgs());
        }

        public event EventHandler InitiateConnection;

        #region Rest
        public override void OnPause()
        {
            base.OnDestroy();
            Activity.UnregisterReceiver(bluetoothReceiver);
            Activity.UnregisterReceiver(deviceReceiver);
            Activity.UnregisterReceiver(pairingReceiver);
        }

        public override void OnResume()
        {
            base.OnResume();
            Activity.RegisterReceiver(bluetoothReceiver, bluetoothFilter);
            Activity.RegisterReceiver(deviceReceiver, deviceFilter);
            Activity.RegisterReceiver(pairingReceiver, pairingFilter);
        }

        private void CheckBTPermission()
        {
            if (Build.VERSION.SdkInt > BuildVersionCodes.Lollipop)
            {
                var permissionCheck1 = Activity.CheckSelfPermission("Manifest.Permission.AccessFineLocation");
                var permissionCheck2 = Activity.CheckSelfPermission("Manifest.Permission.AccessCoarseLocation");
                if (permissionCheck1 == Permission.Denied)
                    RequestPermissions(new string[] { Manifest.Permission.AccessFineLocation }, 1001);
                if (permissionCheck2 == Permission.Granted)
                    RequestPermissions(new string[] { Manifest.Permission.AccessCoarseLocation }, 1002);
            }
        }
        #endregion
    }

    #region Broadcast Receiver
    public class BluetoothReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string action = intent.Action;
            if (action.Equals(BluetoothAdapter.ActionStateChanged))
            {
                var state = intent.GetIntExtra(BluetoothAdapter.ExtraState, BluetoothAdapter.Error);
                if (state == (int)State.Off)
                    BluetoothStateChanged?.Invoke(State.Off, new EventArgs());
                else if (state == (int)State.On)
                    BluetoothStateChanged?.Invoke(State.On, new EventArgs());
            }
        }

        public event EventHandler BluetoothStateChanged;
    }

    public class DeviceReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string action = intent.Action;
            if (action.Equals(BluetoothDevice.ActionFound))
            {
                var device = intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                DeviceFound?.Invoke(device, new EventArgs());
            }
        }

        public event EventHandler DeviceFound;
    }

    public class PairingReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string action = intent.Action;
            if (action.Equals(BluetoothDevice.ActionBondStateChanged))
            {
                var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                switch (device.BondState)
                {
                    case Bond.Bonded:
                        Toast.MakeText(context, "Gerät gekoppelt", ToastLength.Long).Show();
                        DeviceConnected?.Invoke(device, new EventArgs());
                        break;
                    case Bond.Bonding:
                        Toast.MakeText(context, "Gerät wird gekoppelt", ToastLength.Long).Show();
                        break;
                    case Bond.None:

                        break;
                    default: break;
                }
            }
        }

        public event EventHandler DeviceConnected;
    }
    #endregion
}