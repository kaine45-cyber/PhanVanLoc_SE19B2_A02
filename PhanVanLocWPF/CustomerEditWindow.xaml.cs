using System;
using System.Windows;
using PhanVanLocBLL;
using PhanVanLocModels;

namespace PhanVanLocWPF
{
    public partial class CustomerEditWindow : Window
    {
        private readonly CustomerService customerService;
        private Customer customer;
        private bool isEdit;

        public CustomerEditWindow()
        {
            InitializeComponent();
            customerService = new CustomerService();
            isEdit = false;
            Loaded += (s, e) => FullNameTextBox.Focus();
        }

        public CustomerEditWindow(Customer customer) : this()
        {
            this.customer = customer;
            isEdit = true;
            HeaderText.Text = "Edit Customer";
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            if (customer != null)
            {
                FullNameTextBox.Text = customer.CustomerFullName;
                EmailTextBox.Text = customer.EmailAddress;
                TelephoneTextBox.Text = customer.Telephone;
                BirthdayDatePicker.SelectedDate = customer.CustomerBirthday;
                PasswordBox.Password = customer.Password;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    var customerToSave = isEdit ? customer : new Customer();
                    
                    if (customerToSave != null)
                    {
                        customerToSave.CustomerFullName = FullNameTextBox.Text.Trim();
                        customerToSave.EmailAddress = EmailTextBox.Text.Trim();
                        customerToSave.Telephone = TelephoneTextBox.Text.Trim();
                        customerToSave.CustomerBirthday = BirthdayDatePicker.SelectedDate ?? DateTime.Now;
                        customerToSave.Password = PasswordBox.Password;

                        bool success;
                        if (isEdit)
                        {
                            success = await customerService.UpdateAsync(customerToSave);
                            if (success)
                                MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            else
                                MessageBox.Show("Failed to update customer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            success = await customerService.AddAsync(customerToSave);
                            if (success)
                                MessageBox.Show("Customer added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            else
                                MessageBox.Show("Failed to add customer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                ShowError(ex.Message);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
            {
                ShowError("Full name is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                ShowError("Email is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(TelephoneTextBox.Text))
            {
                ShowError("Telephone is required.");
                return false;
            }

            if (BirthdayDatePicker.SelectedDate == null)
            {
                ShowError("Birthday is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                ShowError("Password is required.");
                return false;
            }

            return true;
        }

        private void ShowError(string message)
        {
            ErrorMessageText.Text = message;
            ErrorMessageText.Visibility = Visibility.Visible;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

