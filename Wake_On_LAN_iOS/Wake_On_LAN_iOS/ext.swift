//
//  ext.swift
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
	
import Foundation

//Extending string for check for numbers and return boolean
extension String {
    var isInt: Bool {
        return Int(self) != nil
    }
}
//------------------------------
