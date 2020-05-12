using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace Birthday
{
    public class BluetoothConnectionService
    {
        private BluetoothSocket socket;
        private Thread listenThread;
        private BluetoothDevice hc06;
        private BluetoothAdapter adapter;
        private Context context;

        public BluetoothConnectionService(Context context, BluetoothAdapter adapter, BluetoothDevice hc06)
        {
            this.context = context;
            this.adapter = adapter;
            this.hc06 = hc06;
        }

        public void Initiate()
        {
            listenThread = new Thread(Listener);
            listenThread.Start();

            socket = hc06.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));

            try
            {
                socket.Connect();
                Toast.MakeText(context, "Socketverbindung erfolgreich", ToastLength.Long).Show();
                ConnectionSuccessful?.Invoke(this, new EventArgs());
            }
            catch (SocketException e)
            {
                socket.Close();                
                Toast.MakeText(context, "Socketverbindung fehlgeschlagen", ToastLength.Long).Show();
                ConnectionFailed?.Invoke(this, new EventArgs());
            }            
        }

        public event EventHandler ConnectionFailed;

        public event EventHandler ConnectionSuccessful;

        private void Listener()
        {
            //byte[] read = new byte[1];
            //while (true)
            //{

            //    //thisTime = DateTime.Now;


            //    try
            //    {

            //        socket.InputStream.Read(read, 0, 1);
            //        socket.InputStream.Close();

            //        SignalReceived(read, new EventArgs());
            //    }
            //    catch { }
            //    finally
            //    {
            //        socket.InputStream.Close();
            //    }

            //}
        }

        public event EventHandler SignalReceived;

        public void WriteToSocket(int number)
        {
            if (!socket.IsConnected)
                return;

            
            socket.OutputStream.WriteByte(Convert.ToByte(number));
            Thread.Sleep(10);
        }
    }
}