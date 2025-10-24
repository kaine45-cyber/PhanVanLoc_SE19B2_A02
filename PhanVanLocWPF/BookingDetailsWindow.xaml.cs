using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using PhanVanLocBLL;
using PhanVanLocModels;

namespace PhanVanLocWPF
{
    public partial class BookingDetailsWindow : Window
    {
        private readonly BookingService bookingService = new BookingService();
        private readonly BookingReservation booking;

        public BookingDetailsWindow(BookingReservation booking)
        {
            InitializeComponent();
            this.booking = booking;
            LoadBookingDetails();
        }

        private void LoadBookingDetails()
        {
            try
            {
                // Load booking info
                txtBookingID.Text = booking.BookingReservationID.ToString();
                txtCustomer.Text = booking.Customer?.CustomerFullName ?? "N/A";
                txtBookingDate.Text = booking.BookingDate?.ToString("dd/MM/yyyy") ?? "N/A";
                txtTotalPrice.Text = booking.TotalPrice?.ToString("C0") ?? "N/A";

                // Load booking details - use the booking's details directly
                var details = booking.BookingDetails?.ToList() ?? new List<BookingDetail>();
                
                // Ensure RoomInformation and RoomType are loaded
                foreach (var detail in details)
                {
                    if (detail.RoomInformation != null && detail.RoomInformation.RoomType == null)
                    {
                        // Load RoomType if not already loaded
                        var roomService = new RoomService();
                        var room = roomService.GetById(detail.RoomInformation.RoomID);
                        if (room != null)
                        {
                            detail.RoomInformation = room;
                        }
                    }
                }

                dgBookingDetails.ItemsSource = details;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error viewing booking details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

