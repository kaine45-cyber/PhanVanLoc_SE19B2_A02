using System;
using System.Linq;
using System.Windows;
using PhanVanLocBLL;
using PhanVanLocModels;

namespace PhanVanLocWPF
{
    public partial class BookingsWindow : Window
    {
        private readonly BookingService bookingService = new BookingService();
        private readonly CustomerService customerService = new CustomerService();

        public BookingsWindow()
        {
            InitializeComponent();
            LoadBookings();
        }

        private void LoadBookings()
        {
            try
            {
                var bookings = bookingService.GetAll()
                    .OrderByDescending(b => b.BookingDate)
                    .ToList();
                
                dgBookings.ItemsSource = bookings;
                txtTotalBookings.Text = $"Total bookings: {bookings.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lá»—i khi táº£i dá»¯ liá»‡u: {ex.Message}", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var bookingEditWindow = new BookingEditWindow();
                if (bookingEditWindow.ShowDialog() == true)
                {
                    LoadBookings();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lá»—i khi thÃªm Ä‘áº·t phÃ²ng: {ex.Message}", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as System.Windows.Controls.Button;
                var selectedBooking = button?.Tag as BookingReservation;
                if (selectedBooking == null)
                {
                    MessageBox.Show("Vui lÃ²ng chá»n má»™t Ä‘áº·t phÃ²ng Ä‘á»ƒ xem chi tiáº¿t.", "ThÃ´ng bÃ¡o", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var detailsWindow = new BookingDetailsWindow(selectedBooking);
                detailsWindow.Owner = this;
                detailsWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lá»—i khi xem chi tiáº¿t: {ex.Message}", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as System.Windows.Controls.Button;
                var selectedBooking = button?.Tag as BookingReservation;
                if (selectedBooking == null)
                {
                    MessageBox.Show("Vui lÃ²ng chá»n má»™t Ä‘áº·t phÃ²ng Ä‘á»ƒ xÃ³a.", "ThÃ´ng bÃ¡o", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var result = MessageBox.Show($"Báº¡n cÃ³ cháº¯c cháº¯n muá»‘n xÃ³a Ä‘áº·t phÃ²ng #{selectedBooking.BookingReservationID}?", 
                                           "XÃ¡c nháº­n xÃ³a", 
                                           MessageBoxButton.YesNo, 
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    bool success = await bookingService.DeleteAsync(selectedBooking.BookingReservationID);
                    if (success)
                    {
                        LoadBookings();
                        MessageBox.Show("XÃ³a Ä‘áº·t phÃ²ng thÃ nh cÃ´ng.", "ThÃ nh cÃ´ng", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("KhÃ´ng thá»ƒ xÃ³a Ä‘áº·t phÃ²ng.", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lá»—i khi xÃ³a Ä‘áº·t phÃ²ng: {ex.Message}", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadBookings();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

