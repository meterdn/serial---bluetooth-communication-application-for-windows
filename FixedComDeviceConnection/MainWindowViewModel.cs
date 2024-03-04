
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

namespace FixedComDeviceConnection
{
    public class MainWindowViewModel : BindableBase
    {


        private string _selectedBaudRate;

        public string SelectedBaudRate
        {
            get { return _selectedBaudRate; }
            set { SetProperty(ref _selectedBaudRate, value); }
        }



        private List<int> _baudRates;

        public List<int> BaudRates
        {
            get { return _baudRates; }
            set { SetProperty(ref _baudRates, value); }
        }




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

        public MainWindowViewModel()
        {
            BaudRates = GetCommonBaudRates();
            AvailableCOMPorts = LoadComs().ToList();

        }

        public void PopulateComboBoxItems()
        {
            AvailableCOMPorts = LoadComs().ToList();
        }

        private IEnumerable<string> LoadComs()
        {
            string[] ports = SerialPort.GetPortNames();
            return ports;
        }

        public static List<int> GetCommonBaudRates()
        {
            return new List<int>
        {
            300,
            1200,
            2400,
            4800,
            9600,
            19200,
            38400,
            57600,
            115200
        };
        }

    }
}
