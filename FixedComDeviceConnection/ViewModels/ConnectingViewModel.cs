
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
    public class ConnectingViewModel : BindableBase
    {

        private string _message;

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                SetProperty(ref _message, value);

            }
        }

        public bool isportopen;

        public DelegateCommand SendCommand { get; set; }

        public DelegateCommand SwitchToDecCommand { get; set; }
        public DelegateCommand SwitchToHexCommand { get; set; }
        public DelegateCommand SendInAsciiCommand { get; set; }
        public DelegateCommand SendInHexCommand { get; set; }
        public DelegateCommand GoBackCommand { get; private set; }

        public bool serialinitiated = false;

        SerialPort portt;


        public ConnectingViewModel()
        {
            isportopen = false;
            SendCommand = new DelegateCommand(SendMessage);
            SerialPortInitiated();


            SwitchToDecCommand = new DelegateCommand(SwitchDec);
            SwitchToHexCommand = new DelegateCommand(SwitchHex);
            SendInAsciiCommand = new DelegateCommand(SendAscii);
            SendInHexCommand = new DelegateCommand(SendHex);

            GoBackCommand = new DelegateCommand(GoBack);


        }
        private void GoBack()
        {
            // Close the serial port if it's open
            if (isportopen)
            {
                portt.Close();
                isportopen = false;
            }

            // Navigate back to the MainWindow
            var mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.DataContext = new MainWindowViewModel();

            // Close the current ConnectingView
            Application.Current.Windows[0].Close();
        }


        public bool ReadInHex = false;
        public bool SendInHex = true;


        private void SendAscii()
        {
            SendInHex = false;

        }
        private void SendHex()
        {
            SendInHex = true;
        }

        private void SwitchDec()
        {
            ReadInHex = false;
            Answer = AnswerAscii;
        }
        private void SwitchHex()
        {
            ReadInHex = true;
            Answer = AnswerHex;
        }

        private void SerialPort_DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort serialPort = (SerialPort)sender;

                if (e.EventType == SerialData.Chars)
                {
                    int bytesToRead = serialPort.BytesToRead;
                    byte[] buffer = new byte[bytesToRead];
                    serialPort.Read(buffer, 0, bytesToRead);

                    byte[] messageBytes = buffer.Take(bytesToRead - 2).ToArray();

                    string receivedMessage = Encoding.ASCII.GetString(messageBytes);
                    AnswerAscii = receivedMessage + Environment.NewLine;

                    string hexString = BitConverter.ToString(messageBytes).Replace("-", "");

                    // Set the Answer property to the hex formatted message
                    AnswerHex = hexString + Environment.NewLine;
                    if (ReadInHex == true)
                    {
                        Answer = AnswerHex;
                    }
                    else
                    {
                        Answer = AnswerAscii;
                    }
                }
            }
            catch
            {
                MessageBox.Show("An error occurred: answer can not be displayed ", "unknown answer", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }



        private void SerialPortInitiated()
        {

            if (serialinitiated == false)
            {
                var mainViewModel = Application.Current.MainWindow.DataContext as MainWindowViewModel;


                if (mainViewModel != null)
                {
                    portt = new SerialPort(mainViewModel.SelectedCOMPort);
                    portt.BaudRate = int.Parse(mainViewModel.SelectedBaudRate);

                    if (portt != null)
                    {
                        if (isportopen == false)
                        {
                            portt.Open();
                            isportopen = true;
                        }

                    }
                    portt.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataRecieved);
                    serialinitiated = true;
                }
            }




        }







        private void SendMessage()
        {


            try
            {
                if (Message == null)
                {
                    Answer = "invalid message";
                    return;
                }
                Thread.Sleep(500);
                Message = Message.Trim();

                if (SendInHex == false)
                {

                    byte[] bytes = Encoding.ASCII.GetBytes(Message); // Convert ASCII string to bytes
                    Message = BitConverter.ToString(bytes).Replace("-", ""); // Convert bytes to hex string
                }

                var len = Message.Length / 2 + 2;
                byte[] newArray = new byte[len];


                for (int indexer = 0, slider = 0; indexer < len - 2; indexer++)
                {


                    if (slider >= Message.Length)
                    {
                        break;
                    }
                    newArray[indexer] = Convert.ToByte(Message.Substring(slider, 2), 16);
                    slider += 2;

                }

                byte[] crc = CRCModbus.CRCModbus.crc16(newArray, len - 2);
                newArray[len - 2] = (byte)crc[0];
                newArray[len - 1] = (byte)crc[1];

                portt.Write(newArray, 0, newArray.Length);




            }
            catch
            {
                MessageBox.Show("error sending the message, check connection and message format", "error", MessageBoxButton.OK, MessageBoxImage.Error);

            }



        }

        private string _answer;

        public string Answer
        {
            get { return _answer; }
            set { SetProperty(ref _answer, value); }
        }
        private string _answerascii;

        public string AnswerAscii
        {
            get { return _answerascii; }
            set { SetProperty(ref _answerascii, value); }
        }
        private string _answerhex;

        public string AnswerHex
        {
            get { return _answerhex; }
            set { SetProperty(ref _answerhex, value); }
        }





    }
}
