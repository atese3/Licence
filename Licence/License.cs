using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using Microsoft.Win32;

namespace License
{
    /// <summary>
    /// Licenses base class
    /// </summary>
    public abstract class License : INotifyPropertyChanged
    {
        protected License()
        {

        }

        // Default N/A license code can be changed or delete
        public const string DEFAULT = "T3phbiBBYmkgw5Z5bGUgxLBzdGVkaQ==";

        // License validation share and binding property 
        private KeyValuePair<bool, string> _licenseKeyValuePair;
        public KeyValuePair<bool, string> LicenseValuePair
        {
            get { return _licenseKeyValuePair; }
            private set
            {
                _licenseKeyValuePair = value;
                OnPropertyChanged("LicenseValuePair");
            }
        }

        // Property Change function to notify clients
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // License check function to use every license inherited class
        // triggered when change needed for license validation property
        internal virtual void CheckLicense(KeyValuePair<bool, string> license)
        {
            LicenseValuePair = license;
        }

        // License set function to use every license inherited class
        public abstract bool SetLicense(string license);
    }
}
