using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CarBookingAPI.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;

namespace CarBookingWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum FetchType
    {
        All,
        Available
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Car> Cars { get; set; } = new ObservableCollection<Car>();
        public Car SelectedCar { get; set; }

        private static HttpClient client = new HttpClient();


        private DateTime SelectedDateValue = DateTime.MinValue;
        public DateTime SelectedDate
        {
            get => SelectedDateValue;
            set
            {
                SelectedDateValue = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        private int SelectedFetchTypeValue;
        public int SelectedFetchType
        {
            get => SelectedFetchTypeValue;
            set
            {
                SelectedFetchTypeValue = value;
                OnPropertyChanged(nameof(SelectedFetchType));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        /// <summary>
        /// Fetches the cars from the api and returns them.
        /// </summary>
        /// <param name="fetchType"></param>
        /// <returns></returns>
        private async Task<IEnumerable<Car>> FetchList(string requestUri)
        {
            // Send request
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            // Parse content
            var responseBody = await response.Content.ReadAsStringAsync();
            var cars = JsonSerializer.Deserialize<Car[]>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return cars;
        }

        /// <summary>
        /// Clears and sets the specified elements.
        /// </summary>
        /// <param name="cars">The elements of the list</param>
        private void ReloadList(IEnumerable<Car> cars)
        {
            // Clear list
            Cars.Clear();

            // Add to list
            cars.ToList().ForEach(car => Cars.Add(car));
        }

        /// <summary>
        /// Update car list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedDate = DateTime.MinValue;

            var fetchType = SelectedFetchType switch
            {
                0 => FetchType.All,
                1 => FetchType.Available,
                _ => FetchType.All,
            };

            var requestUri = "http://localhost:5000/api/cars/" + fetchType switch
            {
                FetchType.All => "all",
                FetchType.Available => "available",
                _ => "all"
            };

            // Fetch the list
            var cars = await FetchList(requestUri);

            // Reload list
            ReloadList(cars);
        }

        /// <summary>
        /// Book the specified car (opening the BookCar window for that).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCar is null)
            {
                MessageBox.Show("Please select a car.");
                return;
            }

            // Show the dialog
            var window = new BookCar(SelectedCar);
            window.ShowDialog();

            // Update the list
            ComboBox_SelectionChanged(null, null);
        }

        /// <summary>
        /// Filter by the date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedDate == DateTime.MinValue)
            {
                return;
            }

            var requestUri = "http://localhost:5000/api/cars/available?date=" + SelectedDate.ToString("yyyy-MM-ddTHH:mm:ss.fff");

            // Fetch the list
            var cars = await FetchList(requestUri);

            // Reload list
            ReloadList(cars);
        }
    }
}
