using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using wclCommon;
using wclBluetooth;

namespace BtDeviceRenamer
{
    public partial class fmMain : Form
    {
        #region Bluetooth API
        private const Byte BLUETOOTH_MAX_NAME_SIZE = 248;

        [StructLayout(LayoutKind.Explicit)]
        private struct BLUETOOTH_ADDRESS
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.I8)]
            public Int64 ullLong;
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.U1)]
            public Byte rgBytes_0;
            [FieldOffset(1)]
            [MarshalAs(UnmanagedType.U1)]
            public Byte rgBytes_1;
            [FieldOffset(2)]
            [MarshalAs(UnmanagedType.U1)]
            public Byte rgBytes_2;
            [FieldOffset(3)]
            [MarshalAs(UnmanagedType.U1)]
            public Byte rgBytes_3;
            [FieldOffset(4)]
            [MarshalAs(UnmanagedType.U1)]
            public Byte rgBytes_4;
            [FieldOffset(5)]
            [MarshalAs(UnmanagedType.U1)]
            public Byte rgBytes_5;
        };

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            [MarshalAs(UnmanagedType.U2)]
            public Int16 wYear;
            [MarshalAs(UnmanagedType.U2)]
            public Int16 wMonth;
            [MarshalAs(UnmanagedType.U2)]
            public Int16 wDayOfWeek;
            [MarshalAs(UnmanagedType.U2)]
            public Int16 wDay;
            [MarshalAs(UnmanagedType.U2)]
            public Int16 wHour;
            [MarshalAs(UnmanagedType.U2)]
            public Int16 wMinute;
            [MarshalAs(UnmanagedType.U2)]
            public Int16 wSecond;
            [MarshalAs(UnmanagedType.U2)]
            public Int16 wMilliseconds;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct BLUETOOTH_DEVICE_INFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwSize;
            public BLUETOOTH_ADDRESS Address;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 ulClassofDevice;
            [MarshalAs(UnmanagedType.Bool)]
            public Boolean fConnected;
            [MarshalAs(UnmanagedType.Bool)]
            public Boolean fRemembered;
            [MarshalAs(UnmanagedType.Bool)]
            public Boolean fAuthenticated;
            public SYSTEMTIME stLastSeen;
            public SYSTEMTIME stLastUsed;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BLUETOOTH_MAX_NAME_SIZE)]
            public String szName;
        };

        [DllImport("BluetoothApis.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern UInt32 BluetoothUpdateDeviceRecord(
            [param: In, Out] ref BLUETOOTH_DEVICE_INFO pbtdi);
        #endregion

        private wclBluetoothManager FManager;
        private wclBluetoothRadio FRadio;

        public fmMain()
        {
            InitializeComponent();
        }

        private void fmMain_Load(object sender, EventArgs e)
        {
            FManager = new wclBluetoothManager();
            FManager.Open();
            if (FManager.GetClassicRadio(out FRadio) != wclErrors.WCL_E_SUCCESS)
                FRadio = null;
        }

        private void fmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            FManager.Close();
        }

        private void btEnumDevices_Click(object sender, EventArgs e)
        {
            lvDevices.Items.Clear();

            if (FRadio == null)
                MessageBox.Show("Bluetooth Hardware Not Found.");
            else
            {
                Int64[] Devices;
                if (FRadio.EnumPairedDevices(out Devices) != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Enumerating installed devices failed.");
                else
                {
                    if (Devices == null || Devices.Length == 0)
                        MessageBox.Show("No Paired devices found.");
                    else
                    {
                        foreach (Int64 Mac in Devices)
                        {
                            ListViewItem Item = lvDevices.Items.Add(Mac.ToString("X12"));
                            
                            String Name;
                            if (FRadio.GetRemoteName(Mac, out Name) != wclErrors.WCL_E_SUCCESS)
                                Name = "<" + Mac.ToString("x12") + ">";
                            Item.SubItems.Add(Name);
                        }
                    }
                }
            }
        }

        private void btSetName_Click(object sender, EventArgs e)
        {
            if (FRadio == null)
                MessageBox.Show("Bluetooth Hardware Not Found.");
            {
                if (lvDevices.SelectedItems.Count == 0)
                    MessageBox.Show("Select device");
                else
                {
                    if (edNewName.Text == "")
                        MessageBox.Show("Name can not be empty");
                    else
                    {
                        Int64 Mac = Convert.ToInt64(lvDevices.SelectedItems[0].Text, 16);
                        BLUETOOTH_DEVICE_INFO Info = new BLUETOOTH_DEVICE_INFO();
                        Info.dwSize = (UInt32)Marshal.SizeOf(typeof(BLUETOOTH_DEVICE_INFO));
                        Info.Address.ullLong = Mac;
                        Info.szName = edNewName.Text;

                        UInt32 Res = BluetoothUpdateDeviceRecord(ref Info);
                        if (Res != 0)
                            MessageBox.Show("Update device name failed");
                        else
                        {
                            String Name;
                            if (FRadio.GetRemoteName(Mac, out Name) != wclErrors.WCL_E_SUCCESS)
                                Name = "<" + Mac.ToString("x12") + ">";
                            lvDevices.SelectedItems[0].SubItems[1].Text = Name;

                            // We need to re-enable Bluetooth so name will be refreshed.
                            if (FRadio.TurnOff() == wclErrors.WCL_E_SUCCESS)
                            {
                                if (FRadio.TurnOn() != wclErrors.WCL_E_SUCCESS)
                                    MessageBox.Show("Enable Bluetooth.");
                            }

                            MessageBox.Show("Name changed!");
                        }
                    }
                }
            }
        }
    }
}
