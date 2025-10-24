using System;
using System.Windows;
using PhanVanLocBLL;
using PhanVanLocModels;

namespace PhanVanLocWPF
{
    public partial class CustomerBookingDetailsWindow : Window
    {
        private readonly BookingService bookingService = new BookingService();
        private Customer currentCustomer;
        private RoomInformation selectedRoom;

        public CustomerBookingDetailsWindow(Customer customer, RoomInformation room)
        {
            InitializeComponent();
            currentCustomer = customer;
            selectedRoom = room;
            InitializeBookingForm();
        }

        private void InitializeBookingForm()
        {
            txtRoomInfo.Text = $"{selectedRoom.RoomNumber} - {selectedRoom.RoomType?.RoomTypeName}";
            txtPricePerDay.Text = selectedRoom.RoomPricePerDay?.ToString("C0") ?? "N/A";
            
            dpCheckIn.SelectedDate = DateTime.Today;
            dpCheckOut.SelectedDate = DateTime.Today.AddDays(1);
            
            dpCheckIn.SelectedDateChanged += OnDateChanged;
            dpCheckOut.SelectedDateChanged += OnDateChanged;
            
            CalculateTotalPrice();
        }

        private void OnDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CalculateTotalPrice();
        }

        private void CalculateTotalPrice()
        {
            if (dpCheckIn.SelectedDate.HasValue && dpCheckOut.SelectedDate.HasValue)
            {
                var checkIn = dpCheckIn.SelectedDate.Value;
                var checkOut = dpCheckOut.SelectedDate.Value;
                
                if (checkOut > checkIn)
                {
                    var numberOfDays = (checkOut - checkIn).Days;
                    txtNumberOfDays.Text = numberOfDays.ToString();
                    
                    if (selectedRoom.RoomPricePerDay.HasValue)
                    {
                        var totalPrice = numberOfDays * selectedRoom.RoomPricePerDay.Value;
                        txtTotalPrice.Text = totalPrice.ToString("C0");
                    }
                    else
                    {
                        txtTotalPrice.Text = "N/A";
                    }
                }
                else
                {
                    txtNumberOfDays.Text = "0";
                    txtTotalPrice.Text = "Invalid dates";
                }
            }
        }

        private async void ConfirmBookingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                var checkIn = dpCheckIn.SelectedDate.Value;
                var checkOut = dpCheckOut.SelectedDate.Value;
                var numberOfDays = (checkOut - checkIn).Days;
                var totalPrice = numberOfDays * (selectedRoom.RoomPricePerDay ?? 0);

                // Generate new booking ID
                var maxId = bookingService.GetAll().Max(b => (int?)b.BookingReservationID) ?? 0;
                var newBookingId = maxId + 1;

                // Create booking reservation
                var booking = new BookingReservation
                {
                    BookingReservationID = newBookingId,
                    CustomerID = currentCustomer.CustomerID,
                    BookingDate = DateTime.Now,
                    TotalPrice = totalPrice,
                    BookingStatus = 1 // Confirmed
                };

                // Create booking detail
                var bookingDetail = new BookingDetail
                {
                    BookingReservationID = newBookingId,
                    RoomID = selectedRoom.RoomID,
                    StartDate = checkIn,
                    EndDate = checkOut,
                    ActualPrice = selectedRoom.RoomPricePerDay
                };

                booking.BookingDetails.Add(bookingDetail);

                bool success = await bookingService.AddAsync(booking);
                if (success)
                {
                    MessageBox.Show("Booking confirmed successfully!", "Success", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Failed to confirm booking. Please try again.", "Error", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error confirming booking: {ex.Message}", "Error", 
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateInput()
        {
            if (!dpCheckIn.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select check-in date.", "Validation Error", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                dpCheckIn.Focus();
                return false;
            }

            if (!dpCheckOut.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select check-out date.", "Validation Error", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                dpCheckOut.Focus();
                return false;
            }

            if (dpCheckOut.SelectedDate <= dpCheckIn.SelectedDate)
            {
                MessageBox.Show("Check-out date must be after check-in date.", "Validation Error", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                dpCheckOut.Focus();
                return false;
            }

            if (dpCheckIn.SelectedDate < DateTime.Today)
            {
                MessageBox.Show("Check-in date cannot be in the past.", "Validation Error", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                dpCheckIn.Focus();
                return false;
            }

            return true;
        }
    }
}

