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
    /// This is a simple wake on lan project for windows 
    /// </summary>
    public partial class Form1 : Form

    {
        BackgroundWorker worker;

        IniFile file = new IniFile(Directory.GetCurrentDirectory() + @"\settings.ini");

        static string macAddress = "";
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
        public uint ReturnFirtsOctet(string ipAddress)
        {
            System.Net.IPAddress iPAddress = System.Net.IPAddress.Parse(ipAddress);
            byte[] byteIP = iPAddress.GetAddressBytes();
            uint ipInUint = (uint)byteIP[0];
            return ipInUint;
        }
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

            if (File.Exists("settings.ini"))
            {
                textBox1.Text = file.IniReadValue("CONNECTION", "IPTarget");
                textBox2.Text = file.IniReadValue("CONNECTION", "MAC");
                textBox4.Text = file.IniReadValue("CONNECTION", "Port");
            }
        }



        private void button1_Click(object sender, EventArgs e)

        {
            
            if (textBox2.Text != file.IniReadValue("CONNECTION", "MAC") || textBox4.Text != file.IniReadValue("CONNECTION", "Port") || textBox1.Text != file.IniReadValue("CONNECTION", "IPMask"))
            {
  
                file.IniWriteValue("CONNECTION", "Port", textBox4.Text);
                file.IniWriteValue("CONNECTION", "MAC", textBox2.Text);
                file.IniWriteValue("CONNECTION", "IPMask", textBox1.Text);
            }
            if (textBox2.Text.Length > 0 && textBox4.Text.Length > 0 && textBox1.Text.Length > 0)
            {
                try
                {
                    IPAddress ip2 = IPAddress.Parse(textBox1.Text);
                    string port = textBox4.Text;
                    int p = Convert.ToInt32(port);
                    if (textBox2.Text.Length == 17)
                    {
                        macAddress = textBox2.Text.Replace(":", "");
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
                            textBox3.Text = returnValue + " bytes sent to IP Mask/MAC: " + textBox1.Text + "/" + textBox2.Text;
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
            


            worker = new BackgroundWorker();
            worker.DoWork += (obj, ea) => pping();
            worker.RunWorkerAsync();
        }

   

      
       
        private void pping()
        {

                if (PingHost(textBox1.Text) == true)
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
     
    }
}
