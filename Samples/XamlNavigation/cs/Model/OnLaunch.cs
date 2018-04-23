using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml;

namespace AppInstallerFileGenerator.Model
{
    [DataContract(Name = "OnLaunch", Namespace = "")]
    public class OnLaunch
    {
        private bool _isCheckUpdates;

        [DataMember(Name = "HoursBetweenUpdateChecks")]
        private int _hoursBetweenUpdateChecks;


        public OnLaunch()
        {
            _isCheckUpdates = true;
            _hoursBetweenUpdateChecks = 0;
        }

        public OnLaunch(bool isCheckUpdates, int hourseBetweenUpdateChecks)
        {
            _hoursBetweenUpdateChecks = hourseBetweenUpdateChecks;
            _isCheckUpdates = isCheckUpdates;
        }

        public int HoursBetweenUpdateChecks
        {
            get { return _hoursBetweenUpdateChecks; }
            set
            {
                _hoursBetweenUpdateChecks = value;
            }
        }

        public bool IsCheckUpdates
        {
            get { return _isCheckUpdates; }
            set
            {
                _isCheckUpdates = value;
            }
        }
    }
}
