using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ini;

namespace WOL
{

    /// <summary>
    /* 
    Author: 0x78654C

    Description: This a simple Wake Over Lan app that that came from the idea of code transparency
    and not closed source where you some times don't know what the app really dose even if is free or not..
    This app is distributed under the GNU GPLv3 License.
    Usage: Is made for 1 machine only. You add the IP/Hostname(internal or external ), MAC address, and WOL port(Depending on your motherboard, the
    default port is 9 or can be changed by desire).

    Copyright © 2020 0x78654C. All rights reserved.
    */
    /// </summary>
    public partial class Form1 : Form

    {
        BackgroundWorker worker;

       readonly IniFile file = new IniFile(Directory.GetCurrentDirectory() + @"\settings.ini");

        private static string macAddress = "";
        private static string _IP = "";
        private static string _MAC = "";
        private static string _Port = "";
        private static string sFile = Directory.GetCurrentDirectory() + @"\settings.ini";
        public Form1()
        {
            InitializeComponent();

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
        /// Ping input function
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

        private void Form1_Load(object sender, EventArgs e)
        {
            //Check if settings file exist
            if (!File.Exists(sFile))
            {
                MessageBox.Show("File settings.ini dose not exist!");
                this.Close();
            }
            //------------------------------

            //Loading settings from settings file
            if (File.Exists("settings.ini"))
            {
                _IP= file.IniReadValue("CONNECTION", "IPTarget");
                _MAC= file.IniReadValue("CONNECTION", "MAC");
                _Port= file.IniReadValue("CONNECTION", "Port");

                textBox1.Text = _IP;    //Display ip
                textBox2.Text = _MAC;   //Display MAC
                textBox4.Text = _Port;  //Display Port
                
            }
            //--------------------------------
        }


        /// <summary>
        /// Wake button function 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)

        {
            
            if (_MAC != file.IniReadValue("CONNECTION", "MAC") || _Port != file.IniReadValue("CONNECTION", "Port") || _IP != file.IniReadValue("CONNECTION", "IPTarget"))
            {
                //saving settings from textboxes to ini
  
                file.IniWriteValue("CONNECTION", "Port", textBox4.Text);
                file.IniWriteValue("CONNECTION", "MAC", textBox2.Text);
                file.IniWriteValue("CONNECTION", "IPTarget", textBox1.Text);
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
                            textBox3.Text = returnValue + " bytes sent to IP Mask/MAC: " + _IP + "/" + _MAC;
                        }

                    }
                    else
                    {
                        textBox3.Text = "Remote client could not be set in broadcast mode!";
                    }
                }
                catch(Exception)
                {
                                     
                
                    textBox3.Text = "Something went wrong ..check settings!";
                
            }

            }
            else { textBox3.Text = "All text boxes must be filled!"; }
        }
    

       
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            //Background woker for host check

            worker = new BackgroundWorker();
            worker.DoWork += (obj, ea) => pping();
            worker.RunWorkerAsync();
        }

   

      
       //Check activity of host with ping function
        private void pping()
        {

                if (PingHost(_IP) == true)
                {
                    label4.ForeColor = Color.Green;
                    label4.Text = "ONLINE";

                }
                else
                {
                    label4.ForeColor = Color.Red;
                    label4.Text = "OFFLINE";

                }

        }
        //---------------------------------
     
    }
}
