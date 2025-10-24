using System.Windows;

namespace PhanVanLocWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CustomerButton_Click(object sender, RoutedEventArgs e)
        {
            var customerWindow = new CustomersWindow();
            customerWindow.ShowDialog();
        }

        private void RoomButton_Click(object sender, RoutedEventArgs e)
        {
            var roomWindow = new RoomsWindow();
            roomWindow.ShowDialog();
        }

        private void BookingButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement booking management window
            var bookingWindow = new BookingsWindow();
            bookingWindow.Owner = this;
            bookingWindow.ShowDialog();
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            var reportWindow = new ReportWindow();
            reportWindow.Owner = this;
            reportWindow.ShowDialog();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}

