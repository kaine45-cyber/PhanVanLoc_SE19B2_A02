using System;
using System.Linq;
using System.Windows;
using PhanVanLocBLL;
using PhanVanLocModels;

namespace PhanVanLocWPF
{
    public partial class RoomEditWindow : Window
    {
        private readonly RoomService roomService = new RoomService();
        private RoomInformation? room;
        private bool isEdit;

        public RoomEditWindow(RoomInformation? room = null)
        {
            InitializeComponent();
            this.room = room;
            this.isEdit = room != null;
            
            LoadRoomTypes();
            LoadRoomData();
        }

        private void LoadRoomTypes()
        {
            try
            {
                var roomTypes = roomService.GetAllRoomTypes().ToList();
                cbRoomType.ItemsSource = roomTypes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lá»—i khi táº£i danh sÃ¡ch loáº¡i phÃ²ng: {ex.Message}", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadRoomData()
        {
            if (isEdit && room != null)
            {
                txtRoomNumber.Text = room.RoomNumber;
                cbRoomType.SelectedValue = room.RoomTypeID;
                txtRoomDescription.Text = room.RoomDetailDescription ?? "";
                txtMaxCapacity.Text = room.RoomMaxCapacity?.ToString() ?? "";
                txtPricePerDay.Text = room.RoomPricePerDay?.ToString() ?? "";
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    var roomToSave = isEdit ? room : new RoomInformation();
                    
                    if (roomToSave != null)
                    {
                        roomToSave.RoomNumber = txtRoomNumber.Text.Trim();
                        roomToSave.RoomTypeID = (int)cbRoomType.SelectedValue;
                        roomToSave.RoomDetailDescription = txtRoomDescription.Text.Trim();
                        roomToSave.RoomMaxCapacity = int.TryParse(txtMaxCapacity.Text, out int capacity) ? capacity : null;
                        roomToSave.RoomPricePerDay = decimal.TryParse(txtPricePerDay.Text, out decimal price) ? price : null;

                        bool success;
                        if (isEdit)
                        {
                            success = await roomService.UpdateAsync(roomToSave);
                            if (success)
                                MessageBox.Show("Cáº­p nháº­t phÃ²ng thÃ nh cÃ´ng.", "ThÃ nh cÃ´ng", MessageBoxButton.OK, MessageBoxImage.Information);
                            else
                                MessageBox.Show("KhÃ´ng thá»ƒ cáº­p nháº­t phÃ²ng.", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            success = await roomService.AddAsync(roomToSave);
                            if (success)
                                MessageBox.Show("ThÃªm phÃ²ng thÃ nh cÃ´ng.", "ThÃ nh cÃ´ng", MessageBoxButton.OK, MessageBoxImage.Information);
                            else
                                MessageBox.Show("KhÃ´ng thá»ƒ thÃªm phÃ²ng.", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (string.IsNullOrWhiteSpace(txtRoomNumber.Text))
            {
                MessageBox.Show("Vui lÃ²ng nháº­p sá»‘ phÃ²ng.", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (cbRoomType.SelectedValue == null)
            {
                MessageBox.Show("Vui lÃ²ng chá»n loáº¡i phÃ²ng.", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtMaxCapacity.Text) && !int.TryParse(txtMaxCapacity.Text, out _))
            {
                MessageBox.Show("Sá»©c chá»©a pháº£i lÃ  sá»‘ nguyÃªn.", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtPricePerDay.Text) && !decimal.TryParse(txtPricePerDay.Text, out _))
            {
                MessageBox.Show("GiÃ¡ phÃ²ng pháº£i lÃ  sá»‘.", "Lá»—i", MessageBoxButton.OK, MessageBoxImage.Warning);
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

