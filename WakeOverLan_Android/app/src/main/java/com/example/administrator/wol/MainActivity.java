package com.example.administrator.wol;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Bundle;
import android.os.Handler;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;

public class MainActivity extends AppCompatActivity {

    //This a simple Wake Over Lan app that came from the desire of knowing what actually dose for sure a app
    //and use closed source where you some times don't know what the app really dose.

    SharedPreferences myPrefs;
    private static MainActivity ins;
    private static final String MAC_REGEX = "([0-9a-fA-F]{2}[-:]){5}[0-9a-fA-F]{2}";
    public Handler handler = null; //Define handler for _conCheck function
    public static Runnable runnable = null; //Define runnable for the handler on _conCheck function
    public Handler m_handler;//Define the handler for information messages on button's action


    @SuppressLint({"SetTextI18n", "ResourceType"})
    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate (savedInstanceState);
        setContentView (R.layout.activity_main);
        TextView _stat = findViewById (R.id.textView2);
        ImageView _img1 = findViewById (R.id.imageView);
        _img1.setImageResource(R.raw.wifi_ico_grey_24);
        ImageView _img2 = findViewById (R.id.imageView2);
        _img2.setImageResource(R.raw.net_grey_data);

        //Display wifi icon if is up or down
        if(_wifi ()){
            _img1.setImageResource(R.raw.wifi_ico_24);
        }else{
            _img1.setImageResource(R.raw.wifi_ico_grey_24);
        }
        //------------------------
        //Display mobile data icon if is up or down
        if(_mobile ()){
            _img2.setImageResource(R.raw.net_green_data);
        }else{
            _img2.setImageResource(R.raw.net_grey_data);
        }
        //--------------------------------

        _conCheck (); //Run check on connection stats

        //Set and get preferences settings on textBoxes
        myPrefs = getSharedPreferences ("prefID", Context.MODE_PRIVATE);
        String _ip = myPrefs.getString ("nameKey", "");
        String _mac = myPrefs.getString ("nameKey1", "");
        String _port = myPrefs.getString ("nameKey2", "");
        EditText iptext = findViewById (R.id.editText);
        EditText mactext = findViewById (R.id.editText2);
        EditText ptext = findViewById (R.id.editText3);
        iptext.setText (_ip);
        mactext.setText (_mac);
        ptext.setText (_port);
        //-----------------------------------------------

        //Wake button for main function.
        Button button1 = findViewById (R.id.button);
        button1.setOnClickListener (new View.OnClickListener () {
            @SuppressLint("SetTextI18n")
            @Override
            public void onClick(View _view) {

                m_handler = new Handler();
                TextView _stat = findViewById (R.id.textView2);
                EditText iptext = findViewById (R.id.editText);
                EditText mactext = findViewById (R.id.editText2);
                String broadcastIP;
                broadcastIP = String.valueOf (iptext.getText ());
                String mac = String.valueOf (mactext.getText ());
                Log.d ("Read mac= ", mac);
                Log.d ("Read ip=", broadcastIP);
                wakeup (broadcastIP, mac);

                if (isInternetAvailable ()) {
                    _stat.setTextColor (Color.MAGENTA);
                    _stat.setText ("Wake packets sent!");
                    m_handler.postDelayed(new Runnable()
                    {
                        public void run()
                        {
                            TextView _stat = findViewById (R.id.textView2);
                            _stat.setTextColor (Color.MAGENTA);
                            _stat.setText ("");
                        }
                    }, 2000);

                } else {

                    _stat.setTextColor (Color.MAGENTA);
                    _stat.setText ("No internet connection but wake packets are sent!");
                    m_handler.postDelayed(new Runnable()
                    {
                        public void run()
                        {
                            TextView _stat = findViewById (R.id.textView2);
                            _stat.setTextColor (Color.MAGENTA);
                            _stat.setText ("");
                        }
                    }, 2000);

                }

            }

        });
        //-----------------------------------------

        //Ping function button.
        Button button2 = findViewById (R.id.button2);
        button2.setOnClickListener (new View.OnClickListener () {
            @SuppressLint("SetTextI18n")
            @Override
            public void onClick(View _view) {
                m_handler = new Handler();
                 TextView _stat = findViewById (R.id.textView2);
                 EditText iptext = findViewById (R.id.editText);
                if (pingHost (iptext.getText ().toString ())) {
                    _stat.setTextColor (Color.GREEN);
                    _stat.setText ("Online");
                    m_handler.postDelayed(new Runnable()
                    {
                        public void run()
                        {
                            TextView _stat = findViewById (R.id.textView2);
                            _stat.setTextColor (Color.MAGENTA);
                            _stat.setText ("");
                        }
                    }, 2000);
                } else {
                    _stat.setTextColor (Color.RED);
                    _stat.setText ("Offline");
                    m_handler.postDelayed(new Runnable()
                    {
                        public void run()
                        {
                            TextView _stat = findViewById (R.id.textView2);
                            _stat.setTextColor (Color.MAGENTA);
                            _stat.setText ("");
                        }
                    }, 2000);
                }

            }
        });
        //-----------------------------------------

        //Save last pc instance function button.
        Button button3 = findViewById (R.id.button3);
        button3.setOnClickListener (new View.OnClickListener () {
            @SuppressLint("SetTextI18n")
            @Override
            public void onClick(View _view) {
                m_handler = new Handler();
                final EditText iptext = findViewById (R.id.editText);
                final EditText mactext = findViewById (R.id.editText2);
                final EditText ptext = findViewById (R.id.editText3);
                myPrefs = getSharedPreferences ("prefID", Context.MODE_PRIVATE);
                SharedPreferences.Editor editor = myPrefs.edit ();
                editor.putString ("nameKey", iptext.getText ().toString ());
                editor.putString ("nameKey1", mactext.getText ().toString ());
                editor.putString ("nameKey2", ptext.getText ().toString ());
                editor.apply ();
                TextView _stat = findViewById (R.id.textView2);
                _stat.setTextColor (Color.YELLOW);
                _stat.setText ("IP/MAC/Port saved!");
                m_handler.postDelayed(new Runnable()
                {
                    public void run()
                    {
                        TextView _stat = findViewById (R.id.textView2);
                        _stat.setTextColor (Color.MAGENTA);
                        _stat.setText ("");
                    }
                }, 2000);

            }
        });
    }
    //-----------------------------------------

    //Check wifi network connection

    public  boolean _wifi() {

        ConnectivityManager connectivityManager = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
        if ( connectivityManager.getNetworkInfo(ConnectivityManager.TYPE_WIFI).getState() == NetworkInfo.State.CONNECTED) {
            return true;
        } else {
            return false;
        }

    }
    //-------------------------

    //Check mobile network connection
    public boolean _mobile() {


        ConnectivityManager connectivityManager = (ConnectivityManager) getSystemService (Context.CONNECTIVITY_SERVICE);
        if (connectivityManager.getNetworkInfo (ConnectivityManager.TYPE_MOBILE).getState () == NetworkInfo.State.CONNECTED) {
            //we are connected to a networL
            return true;
        } else {
            return false;
        }
    }
    //------------------------


    //Check wifi and mobile data connection and display the necessary icons

    @SuppressLint("ResourceType")
    public void _conCheck() {

        //
        handler = new Handler ();
        runnable = new Runnable () {
            public void run() {
                //  MainActivity.getInstace()._conCheck ();
                ImageView _img1 = findViewById (R.id.imageView);
                _img1.setImageResource (R.raw.wifi_ico_grey_24);
                ImageView _img2 = findViewById (R.id.imageView2);
                _img2.setImageResource (R.raw.net_grey_data);

                //Display wifi icon if is up or down
                if (_wifi ()) {
                    _img1.setImageResource (R.raw.wifi_ico_24);
                } else {
                    _img1.setImageResource (R.raw.wifi_ico_grey_24);
                }
                //------------------------
                //Display mobile data icon if is up or down
                if (_mobile ()) {
                    _img2.setImageResource (R.raw.net_green_data);
                } else {
                    _img2.setImageResource (R.raw.net_grey_data);
                }
                handler.postDelayed (runnable, 2000);
            }
        };
        handler.postDelayed (runnable, 3000);
    }
    //--------------------------------------------

    //Check internet connection via ping one time on google.

    public boolean isInternetAvailable() {
        try {
            EditText iptext = findViewById (R.id.editText);
            Process p1 = java.lang.Runtime.getRuntime ().exec ("ping -c 1 www.google.com");
            int returnVal = p1.waitFor ();
            boolean reachable = (returnVal == 0);
            return reachable;
        } catch (Exception e) {
            // TODO Auto-generated catch block
            e.printStackTrace ();
        }
        return false;


    }
    //---------------------------------------------

    //Ping function with one beacon
    public boolean pingHost(String ip) {
        try {

            Process p1 = java.lang.Runtime.getRuntime ().exec ("ping -c 1 " + ip);
            int returnVal = p1.waitFor ();
            boolean reachable = (returnVal == 0);
            return reachable;
        } catch (Exception e) {
            // TODO Auto-generated catch block
            e.printStackTrace ();
        }
        return false;

    }
    //---------------------------------

    //-----------------------------------------------
    //---------------Wake over lan functions---------

        private static byte[] getMacBytes(String mac) throws IllegalArgumentException {
        if (!mac.matches (MAC_REGEX)) {
            throw new IllegalArgumentException ("Invalid MAC address");
        }

        byte[] bytes = new byte[6];
        String[] hex = mac.split ("(:|\\-)");

        try {
            for (int i = 0; i < 6; i++) {
                bytes[i] = (byte) Integer.parseInt (hex[i], 16);
            }
        } catch (NumberFormatException e) {
            // Should not happen, the regex forbids it, but it doesn't compile otherwise.
            throw new IllegalArgumentException ("Invalid hex digit in MAC address.");
        }
        return bytes;
    }

    public void wakeup(String broadcastIP, String mac) {
        Log.d ("wakeup", "method started");
        if (mac == null) {
            Log.d ("Mac error at wakeup", "mac = null");
            return;
        }

        try {
            byte[] macBytes = getMacBytes (mac);
            Log.d ("wakeup (bytes)", new String (macBytes));
            byte[] bytes = new byte[6 + 16 * macBytes.length];
            for (int i = 0; i < 6; i++) {
                bytes[i] = (byte) 0xff;
            }
            for (int i = 6; i < bytes.length; i += macBytes.length) {
                System.arraycopy (macBytes, 0, bytes, i, macBytes.length);
            }

            Log.d ("wakeup", "calculating completed, sending...");
            EditText ptext1 = findViewById (R.id.editText3);
            int in1 = Integer.valueOf (ptext1.getText ().toString ());
            InetAddress address = InetAddress.getByName (broadcastIP);
            DatagramPacket packet = new DatagramPacket (bytes, bytes.length, address, in1);
            DatagramSocket socket = new DatagramSocket ();
            socket.send (packet);
            socket.close ();
            Log.d ("wakeup", "Magic Packet sent");

        } catch (Exception e) {
            Log.e ("wakeup", "error");
        }

    }
    //---------------------------------------------------

}

