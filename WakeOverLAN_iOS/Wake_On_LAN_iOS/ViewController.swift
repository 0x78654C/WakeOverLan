//
//  ViewController.swift
//  Wake_On_LAN_iOS
//
 /* 
    Author: 0x78654C

    Description: This a simple Wake Over Lan app that that came from the idea of code transparency
    and not closed source where you some times don't know what the app really dose even if is free or not..
    Usage: Is made for 1 machine only. You add the IP/Hostname(internal or external ), MAC address, and WOL port(Depending on your motherboard, the
    default port is 9 or can be changed by desire).
    Requirements: iOS 11+
	Awake library from: https://github.com/jesper-lindberg/Awake   (thank you very much)
	
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

import UIKit

class ViewController: UIViewController {
    
    //Declare controllers
    @IBOutlet weak var ipTXT: UITextField!
    @IBOutlet weak var macTXT: UITextField!
    @IBOutlet weak var portTXT: UITextField!
    @IBOutlet weak var logTXT: UILabel!
    @IBOutlet weak var wakeBTN: UIButton!
    //----------------
    
    //Declare variables
    var IP = " "
    var MAC = " "
    var wPort = " "
    //----------------

    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
        
        logTXT.text = " "; //clear logs
        
        //Design the wake button
        wakeBTN.layer.cornerRadius=10
        wakeBTN.clipsToBounds=true
        //------------------
        
        //Load saved UserDefaults
        ipTXT.text = UserDefaults.standard.string(forKey: "ipKey")
        macTXT.text = UserDefaults.standard.string(forKey: "macKey")
        portTXT.text = UserDefaults.standard.string(forKey: "portKey")
        //------------------
        
        //Hide keyboard function call
        let tapGesture = UITapGestureRecognizer(target: self, action: #selector(self.dismissKeyboard (_:)))
        self.view.addGestureRecognizer(tapGesture)
        //-----------------
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    //hide keyboard function
    @objc func dismissKeyboard (_ sender: UITapGestureRecognizer) {
        portTXT.resignFirstResponder()
        ipTXT.resignFirstResponder()
        macTXT.resignFirstResponder()
    }
    
    @IBAction func wake(_ sender: Any) {
        //assign params to defined global variables
        IP = ipTXT.text!
        MAC = macTXT.text!
        wPort=portTXT.text!
        //--------
        
        if IP != "" && MAC != "" && wPort != ""{
            
            if wPort.isInt{
            let a:UInt16? = UInt16(wPort)
            
            logTXT.textColor = UIColor.purple
            logTXT.text = "Wake packet sent!"

            let computer = Awake.Device(MAC: MAC, BroadcastAddr: IP, Port: a!)
            Awake.target(device: computer)

            }else{
                logTXT.textColor = UIColor.red
                logTXT.text = "Port field should be number!"

            }
        }else{
            logTXT.textColor = UIColor.red
            logTXT.text = "Please fill all boxes!"
        }
        
        
        //Storing params from text fields in UserDefaults
        UserDefaults.standard.set(IP, forKey: "ipKey")
        UserDefaults.standard.set(MAC, forKey: "macKey")
        UserDefaults.standard.set(wPort, forKey: "portKey")
        //--------------
        
    }
    
}

