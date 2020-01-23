using CarBookingAPI.Controllers;
using CarBookingAPI.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarBookingWPF
{
    /// <summary>
    /// Interaction logic for BookCar.xaml
    /// </summary>
    public partial class BookCar : Window
    {
        public DateTime BookingDate { get; set; } = DateTime.Now.Date;
        private static HttpClient client = new HttpClient();

        private readonly Car _car;

        public BookCar(Car car)
        {
            InitializeComponent();

            DataContext = this;

            _car = car;
        }

        /// <summary>
        /// Simply exits the dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Books the specified car for the selected date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BookButton_Click(object sender, RoutedEventArgs e)
        {
            BookingRequest request = new BookingRequest
            {
                BookingDate = BookingDate,
                CarId = _car.CarId
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:5000/api/booking", content);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to book car.");
            }

            Close();
        }
    }
}
