using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using License;
using Microsoft.Win32;

namespace License
{
    public class RegisterKeyLicense : License
    {
        // Every license class can be generated only once to prevent multiple set and control fails
        private static RegisterKeyLicense _license;
        public static RegisterKeyLicense GetLicense()
        {
            if (_license == null)
            {
                _license = new RegisterKeyLicense();
            }

            return _license;
        }
        public RegisterKeyLicense()
        {
            // Check license when start
            CheckLicense();

            // todo 
            // Licenses should be check in certain period 
            // Now license controlled only startup
        }

        private void CheckLicense()
        {
            // Open Licensed Saved Register Key
            var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\EA-App");
            var step = 2; 

            if (key == null)
            {
                step = 0;
            }

            while (step < 3)
            {
                switch (step)
                {
                    case 0: // Create Register Key to Save License
                        key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\EA-App");
                        step = 1;
                        break;
                    case 1: // Start 7 days trial if it is first setup
                        var ci = new CultureInfo("tr-tr", false);
                        ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                        ci.DateTimeFormat.ShortTimePattern = "hh:mm:ss";

                        Thread.CurrentThread.CurrentCulture = ci;
                        Thread.CurrentThread.CurrentUICulture = ci;

                        if (key == null) // if key not available create new one
                        {
                            step = 0;
                            break;
                        }

                        key.SetValue("Set1", Transformations.Base64Encode(DateTime.Now.AddDays(7).ToString(ci)));
                        step = 2;
                        break;
                    case 2: // Read License from Register Key and Check Availability 
                        if (key != null)
                        {
                            var value = key.GetValue("Set1").ToString();
                            var licenseKey = value.Substring(48, value.Length - 96);
                            var keyValue = Transformations.Base64Decode(licenseKey);
                            key.Close();

                            if (licenseKey == DEFAULT) // if license is default set license to N/A and exit
                            {
                                base.CheckLicense(new KeyValuePair<bool, string>(true, "N/A"));
                                step = 3;
                                break;
                            }

                            // Check availability of the license and set property then exit
                            var result = DateTime.Compare(Convert.ToDateTime(keyValue), DateTime.Now) > 0;
                            base.CheckLicense(new KeyValuePair<bool, string>(result, keyValue));
                            step = 3;
                        }
                        else // if key not available create new one
                        {
                            step = 0;
                        }
                        break;
                    default:
                        step = 4; // exit
                        break;
                }
            }
        }
        // Set license key with new one and check it's availability
        public override bool SetLicense(string license)
        {

            var newLicense = Transformations.Base64Encode(Guid.NewGuid().ToString()) +
                             Transformations.Base64Encode(license) +
                             Transformations.Base64Encode(Guid.NewGuid().ToString());

            var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\EA-App", true);
            if (key != null)
            {
                key.SetValue("Set1", newLicense);
            }

            CheckLicense();

            return LicenseValuePair.Key;
        }
    }
}
