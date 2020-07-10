using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Ini;


namespace WOL
{
    /// <summary>
    /* 
    Author: 0x78654C

    Description: This a simple Wake Over Lan app that that came from the idea of code transparency
    and not closed source where you some times don't know what the app really dose even if is free or not..
    Usage: Is made for 1 machine only. You add the IP/Hostname(internal or external ), MAC address, and WOL port(Depending on your motherboard, the
    default port is 9 or can be changed by desire).
    Requirements: .NET Framework 4.5 +
    This project uses following librabry for watermark on textboxes: https://github.com/GuOrg/Gu.Wpf.Adorners

    This app is distributed under the MIT License.
    Copyright © 2020 0x78654C. All rights reserved.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
    */
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker worker;

        readonly IniFile file = new IniFile(Directory.GetCurrentDirectory() + @"\settings.ini");

        private static string macAddress = ""; //define the MAC string for replacement in wake function
        private static string _IP = "";        //define the IP string for imput
        private static string _MAC = "";       //define the MAC string for imput
        private static string _Port = "";      //define the WOL Port string for imput
        private static string sFile = Directory.GetCurrentDirectory() + @"\settings.ini";
        readonly static string sData = @"[Connection]
IPTarget=
MAC=
Port=
";  //Default ini settings for create the settings.ini file if not exist.
        public MainWindow()
        {
            InitializeComponent();

            //Timer setup for check target status
            DispatcherTimer chkPingTimrt = new DispatcherTimer();
            chkPingTimrt.Tick += chkPingTimer_Tick;
            chkPingTimrt.Interval = new TimeSpan(0, 0, 1);
            chkPingTimrt.Start();
            //-----------------------------


            //Check if settings file exist and if not it creates autmaticly
            if (!File.Exists(sFile))
            {
                File.WriteAllText(sFile, sData);
            }
            //------------------------------

            //Loading settings from settings file
            if (File.Exists("settings.ini"))
            {
                _IP = file.IniReadValue("CONNECTION", "IPTarget");
                _MAC = file.IniReadValue("CONNECTION", "MAC");
                _Port = file.IniReadValue("CONNECTION", "Port");

                TbIPTarget.Text = _IP;    //Display ip
                TbMAC.Text = _MAC;   //Display MAC
                TbPort.Text = _Port;  //Display Port

            }
            //--------------------------------
        }
        /// <summary>
        /// Drag windows form with mouse event function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        
        /// <summary>
        /// Close button function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public class WOLUdpClient : UdpClient
        {
            // *********************************************************************
            /// <summary>
            /// Initializes a new instance of <see cref="WOLUdpClient"/>.
            /// </summary>
            public WOLUdpClient() : base()
            {
            }

            // *********************************************************************
            /// <summary>
            /// Sets up the UDP client to broadcast packets.
            /// </summary>
            /// <returns><see langword="true"/> if the UDP client is set in
            /// broadcast mode.</returns>
            public bool IsClientInBrodcastMode()
            {
                bool broadcast = false;
                if (this.Active)
                {
                    try
                    {
                        this.Client.SetSocketOption(SocketOptionLevel.Socket,
                             SocketOptionName.Broadcast, 0);
                        broadcast = true;
                    }
                    catch
                    {
                        broadcast = false;
                    }
                }
                return broadcast;
            }
        }


        /// <summary>
        /// Ping IP/Hostename input function
        /// </summary>
        /// <param name="nameOrAddress"></param>
        /// <returns></returns>
        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;

            }
            catch (PingException)
            {

            }
            return pingable;
        }
        //------------------------------------

        /// <summary>
        /// Get first octet from ip function
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public uint ReturnFirtsOctet(string ipAddress)
        {
            System.Net.IPAddress iPAddress = System.Net.IPAddress.Parse(ipAddress);
            byte[] byteIP = iPAddress.GetAddressBytes();
            uint ipInUint = (uint)byteIP[0];
            return ipInUint;
        }

        /// <summary>
        /// Return subnet mask from IP.
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        public string ReturnSubnetmask(String ipaddress)
        {

            uint firstOctet = ReturnFirtsOctet(ipaddress);
            if (firstOctet >= 0 && firstOctet <= 127)
                return "255.0.0.0";
            else if (firstOctet >= 128 && firstOctet <= 191)
                return "255.255.0.0";
            else if (firstOctet >= 192 && firstOctet <= 223)
                return "255.255.255.0";
            else return "0.0.0.0";

        }

        /// <summary>
        /// Wake button function 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWake_Click(object sender, RoutedEventArgs e)
        {
            _IP= TbIPTarget.Text;    //loading new imput ip
            _MAC= TbMAC.Text;        //loading new imput MAC
            _Port= TbPort.Text;      //loading new imput Port

            if (_MAC != file.IniReadValue("CONNECTION", "MAC") || _Port != file.IniReadValue("CONNECTION", "Port") || _IP != file.IniReadValue("CONNECTION", "IPTarget"))
            {
                //Saving settings from textboxes to ini

                file.IniWriteValue("CONNECTION", "Port", TbPort.Text);
                file.IniWriteValue("CONNECTION", "MAC", TbMAC.Text);
                file.IniWriteValue("CONNECTION", "IPTarget", TbIPTarget.Text);
            }


            if (_MAC.Length > 0 && _Port.Length > 0 && _IP.Length > 0)//check texboxes for null 
            {
                try
                {
                    IPAddress ip2 = IPAddress.Parse(_IP);
                    string port = _Port;
                    int p = Convert.ToInt32(port);
                    if (_MAC.Length == 17)
                    {
                        macAddress = _MAC.Replace(":", "");
                        WOLUdpClient client = new WOLUdpClient();
                        client.Connect(ip2,    //255.255.255.255  i.e broadcast
                        p); // port = 12287
                        if (client.IsClientInBrodcastMode())
                        {


                            int byteCount = 0;
                            byte[] bytes = new byte[102];
                            for (int trailer = 0; trailer < 6; trailer++)
                            {
                                bytes[byteCount++] = 0xFF;
                            }
                            for (int macPackets = 0; macPackets < 16; macPackets++)
                            {
                                int i = 0;
                                for (int macBytes = 0; macBytes < 6; macBytes++)
                                {
                                    bytes[byteCount++] =
                                    byte.Parse(macAddress.Substring(i, 2),
                                    NumberStyles.HexNumber);
                                    i += 2;
                                }
                            }

                            int returnValue = client.Send(bytes, byteCount);
                            TbLog.Text = "Magic packet of " + returnValue + " bytes is sent to: " +Environment.NewLine + _IP + " / " + _MAC;
                        }

                    }
                    else
                    {
                        TbLog.Text = "Remote client could not" +Environment.NewLine+" be set in broadcast mode. Please check your settings!";
                    }
                }
                catch (Exception)
                {


                    TbLog.Text = Environment.NewLine + "Something went wrong ..check settings!";

                }

            }
            else { TbLog.Text = "All text boxes must be filled!"; }
        }
        /// <summary>
        /// Check if target ip is online timer function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPingTimer_Tick(object sender, EventArgs e)
        {

            //Background woker for host check

            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(pping);
            worker.RunWorkerAsync();
        
        }

        /// <summary>
        /// Check activity of host with ping function
        /// </summary>
        private void pping(object sender, DoWorkEventArgs e)
        {
             Application.Current.Dispatcher.Invoke(
             new Action(() =>
              {
             _IP = TbIPTarget.Text;    //loading new imput ip
              }
              ));

            if (PingHost(_IP) == true)
            {

                Application.Current.Dispatcher.Invoke(
               new Action(() =>
               {
                   lbDStatus.Foreground = Brushes.Green;
                   lbDStatus.Content = "ONLINE"; //Displaying ONLINE status in green lable
               }
               ));

            }
            else
            {
                Application.Current.Dispatcher.Invoke(
               new Action(() =>
               {
                   lbDStatus.Foreground = Brushes.Red;
                   lbDStatus.Content = "OFFLINE"; //Displaying OFFLINE status in red label 
               }
               ));

            }

        }
        //---------------------------------
    }
}
