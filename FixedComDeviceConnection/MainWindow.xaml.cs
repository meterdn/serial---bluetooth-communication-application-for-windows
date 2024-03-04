using FixedComDeviceConnection.ViewModels;
using FixedComDeviceConnection.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FixedComDeviceConnection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
        private void GoToConnectingView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var connectingView = new ConnectingView();
                connectingView.DataContext = new ConnectingViewModel();
                Content = connectingView;


            }
            catch
            {
                MessageBox.Show("An error occurred: no connection", "connection lost", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel != null)
            {
                viewModel.PopulateComboBoxItems();
            }
        }
    }
}