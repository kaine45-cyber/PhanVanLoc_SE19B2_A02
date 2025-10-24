using System;
using System.Linq;
using System.Windows;
using PhanVanLocBLL;
using PhanVanLocModels;

namespace PhanVanLocWPF
{
    public partial class BookingEditWindow : Window
    {
        private readonly BookingService bookingService = new BookingService();
        private readonly CustomerService customerService = new CustomerService();
        private BookingReservation? booking;
        private bool isEdit;

        public BookingEditWindow(BookingReservation? booking = null)
        {
            InitializeComponent();
            this.booking = booking;
            this.isEdit = booking != null;
            
            LoadCustomers();
            LoadBookingData();
        }

        private void LoadCustomers()
        {
            try
            {
                var customers = customerService.GetAll().ToList();
                cbCustomer.ItemsSource = customers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lá»—i khi táº£i danh sÃ¡ch khÃ¡ch hÃ ng: {ex.Message}", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadBookingData()
        {
            if (isEdit && booking != null)
            {
                cbCustomer.SelectedValue = booking.CustomerID;
                dpBookingDate.SelectedDate = booking.BookingDate;
                txtTotalPrice.Text = booking.TotalPrice?.ToString() ?? "";
                cbBookingStatus.SelectedIndex = (booking.BookingStatus ?? 1) - 1;
            }
            else
            {
                dpBookingDate.SelectedDate = DateTime.Now;
                cbBookingStatus.SelectedIndex = 0; // Default to "ÄÃ£ xÃ¡c nháº­n"
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    var bookingToSave = isEdit ? booking : new BookingReservation();
                    
                    if (bookingToSave != null)
                    {
                        bookingToSave.CustomerID = (int)cbCustomer.SelectedValue;
                        bookingToSave.BookingDate = dpBookingDate.SelectedDate;
                        bookingToSave.TotalPrice = decimal.Parse(txtTotalPrice.Text);
                        bookingToSave.BookingStatus = (byte?)((System.Windows.Controls.ComboBoxItem)cbBookingStatus.SelectedItem)?.Tag;

                        bool success;
                        if (isEdit)
                        {
                            success = await bookingService.UpdateAsync(bookingToSave);
                            if (success)
                                MessageBox.Show("Cáº­p nháº­t Ä‘áº·t phÃ²ng thÃ nh cÃ´ng.", "ThÃ nh cÃ´ng", MessageBoxButton.OK, MessageBoxImage.Information);
                            else
                                MessageBox.Show("KhÃ´ng thá»ƒ cáº­p nháº­t Ä‘áº·t phÃ²ng.", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            success = await bookingService.AddAsync(bookingToSave);
                            if (success)
                                MessageBox.Show("ThÃªm Ä‘áº·t phÃ²ng thÃ nh cÃ´ng.", "ThÃ nh cÃ´ng", MessageBoxButton.OK, MessageBoxImage.Information);
                            else
                                MessageBox.Show("KhÃ´ng thá»ƒ thÃªm Ä‘áº·t phÃ²ng.", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        if (success)
                        {
                            DialogResult = true;
                            Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lá»—i: {ex.Message}", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (cbCustomer.SelectedValue == null)
            {
                MessageBox.Show("Vui lÃ²ng chá»n khÃ¡ch hÃ ng.", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (dpBookingDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lÃ²ng chá»n ngÃ y Ä‘áº·t.", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTotalPrice.Text) || !decimal.TryParse(txtTotalPrice.Text, out _))
            {
                MessageBox.Show("Vui lÃ²ng nháº­p tá»•ng tiá»n há»£p lá»‡.", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

