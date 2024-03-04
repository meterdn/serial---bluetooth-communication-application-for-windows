
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

namespace FixedComDeviceConnection.ViewModels
{
    public class DeviceSelectionViewModel : BindableBase
    {



        private string _selectedCOMPort;

        public string SelectedCOMPort
        {
            get { return _selectedCOMPort; }
            set { SetProperty(ref _selectedCOMPort, value); }
        }



        private List<string> _availableCOMPorts;

        public List<string> AvailableCOMPorts
        {
            get { return _availableCOMPorts; }
            set { SetProperty(ref _availableCOMPorts, value); }
        }

        public DeviceSelectionViewModel()
        {
            AvailableCOMPorts = LoadComs().ToList();

        }

        private IEnumerable<string> LoadComs()
        {
            string[] ports = SerialPort.GetPortNames();
            return ports;
        }
    }
}
