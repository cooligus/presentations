using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using DBApp.Models;
using DBApp.Services;
using System.Threading.Tasks;

namespace DBApp.ViewModels
{
    // #region selectedFood
    public class FavouriteFoodViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<FavouriteFood> FavouriteFoods { get; set; }
        public ObservableCollection<Food> Foods { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public ICommand AddFavouriteFoodCommand { get; set; }
        public ICommand LoadDataCommand { get; }

        private Food _selectedFood;
        public Food SelectedFood
        {
            get => _selectedFood;
            set
            {
                _selectedFood = value;
                OnPropertyChanged(nameof(SelectedFood));
            }
        }
    }
    // #endregion selectedFood
    // #region selectedClient
    public class FavouriteFoodViewModel : INotifyPropertyChanged
    {
        private Client _selectedClient;
        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged(nameof(SelectedClient));
            }
        }
    }
    // #endregion selectedClient
    // #region constructor
    public class FavouriteFoodViewModel : INotifyPropertyChanged
    {
        private readonly FavouriteFoodService _favouriteFoodService;
        private readonly FoodService _foodService;
        private readonly ClientService _clientService;

        public FavouriteFoodViewModel()
        {
            FavouriteFoods = new ObservableCollection<FavouriteFood>();
            Foods = new ObservableCollection<Food>();
            Clients = new ObservableCollection<Client>();
            AddFavouriteFoodCommand = new Command(async () => await AddFavouriteFood());
            LoadDataCommand = new Command(() => LoadData());

            _favouriteFoodService = new FavouriteFoodService();
            _foodService = new FoodService();
            _clientService = new ClientService();

            LoadData();
        }
    }
    // #endregion constructor
    // #region loadData
    public class FavouriteFoodViewModel : INotifyPropertyChanged
    {
        private async void LoadData()
        {
            await LoadFoods();
            await LoadClients();
            await LoadFavouriteFoods();
        }
    }
    // #endregion loadData
    // #region loadFood
    public class FavouriteFoodViewModel : INotifyPropertyChanged
    {
        private async Task LoadFoods()
        {
            var foods = await _foodService.GetFoodsAsync();
            Foods.Clear();
            foreach (var food in foods)
            {
                Foods.Add(food);
            }
        }
    }
    // #endregion loadFood
    // #region loadClient
    public class FavouriteFoodViewModel : INotifyPropertyChanged
    {
        private async Task LoadClients()
        {
            var clients = await _clientService.GetClientsAsync();
            Clients.Clear();
            foreach (var client in clients)
            {
                Clients.Add(client);
            }
        }
    }
    // #endregion loadClient
    // #region loadFavFood
    public class FavouriteFoodViewModel : INotifyPropertyChanged
    {
        private async Task LoadFavouriteFoods()
        {
            var favouriteFoods = await _favouriteFoodService.GetFavouriteFoodsAsync();
            FavouriteFoods.Clear();
            foreach (var favouriteFood in favouriteFoods)
            {
                FavouriteFoods.Add(favouriteFood);
            }
        }
    }
    // #endregion loadFavFood
    // #region addFavFood
    public class FavouriteFoodViewModel : INotifyPropertyChanged
    {
        private async Task AddFavouriteFood()
        {
            if (SelectedFood != null && SelectedClient != null)
            {
                var newFavouriteFood = new FavouriteFood { Food = SelectedFood, Client = SelectedClient };
                await _favouriteFoodService.SaveFavouriteFoodAsync(newFavouriteFood);
                FavouriteFoods.Add(newFavouriteFood);
                SelectedFood = null; // Reset selection
                SelectedClient = null; // Reset selection
            }
        }
    }
    // #endregion addFavFood
    // #region onPropertyChanged
    public class FavouriteFoodViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    // #endregion onPropertyChanged
}