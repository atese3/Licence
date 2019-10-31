using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License
{
    public class DongleLicense : License
    {
        public DongleLicense()
        {
            // todo
            // Dongle Type License should also be checked
        }

        private void CheckLicense()
        {
        }

        public override bool SetLicense(string license)
        {
            return LicenseValuePair.Key;
        }
    }
}
