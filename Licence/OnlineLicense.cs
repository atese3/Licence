using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License
{
    public class OnlineLicense : License
    {
        public OnlineLicense()
        {
            // todo
            // Online Licenses should also be checked
        }

        public void CheckLicense()
        {
        }

        public override bool SetLicense(string license)
        {
            return LicenseValuePair.Key;
        }
    }
}
