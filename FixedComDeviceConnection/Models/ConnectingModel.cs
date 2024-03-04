
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Documents;
using System.Windows;
using System.Text;
using System.Windows.Controls;

namespace FixedComDeviceConnection.Models
{
    public class ConnectingModel : BindableBase
    {
        //public ConnectingModel() { AvailableCOMPorts = new List<string>(); }
        private string _COM;
        public string COM
        {
            get { return _COM; }
            set { SetProperty(ref _COM, value); }
        }
        private List<string> _availableCOMPorts;
        public List<string> AvailableCOMPorts
        {
            get { return _availableCOMPorts; }
            set { SetProperty(ref _availableCOMPorts, value); }
        }

    }
}
