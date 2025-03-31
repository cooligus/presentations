using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using DBApp.Models;
using DBApp.Services;

namespace DBApp.ViewModels
{
    public class FoodViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Food> Foods { get; set; }
        public ICommand AddFoodCommand { get; set; }

        private string _foodName;
        public string FoodName
        {
            get => _foodName;
            set
            {
                _foodName = value;
                OnPropertyChanged(nameof(FoodName));
            }
        }

        private int _inStock;
        public int InStock
        {
            get => _inStock;
            set
            {
                _inStock = value;
                OnPropertyChanged(nameof(InStock));
            }
        }

        private FoodType _foodType;
        public FoodType FoodType
        {
            get => _foodType;
            set
            {
                _foodType = value;
                OnPropertyChanged(nameof(FoodType));
            }
        }

        public ObservableCollection<FoodType> FoodTypes { get; set; } 

        private readonly FoodService _foodService;

        public FoodViewModel()
        {
            Foods = new ObservableCollection<Food>();
            AddFoodCommand = new Command(async () => await AddFood());

            _foodService = new FoodService();
            LoadFoods();

     
            FoodTypes = new ObservableCollection<FoodType>((FoodType[])System.Enum.GetValues(typeof(FoodType)));
            FoodType = FoodTypes[0];
        }

        private async void LoadFoods()
        {
            var foods = await _foodService.GetFoodsAsync();
            Foods.Clear();
            foreach (var food in foods)
            {
                Foods.Add(food);
            }
        }

        private async Task AddFood()
        {
            if (!string.IsNullOrWhiteSpace(FoodName))
            {
                var newFood = new Food { Name = FoodName, InStock = InStock, Type = FoodType };
                await _foodService.SaveFoodAsync(newFood);
                Foods.Add(newFood);
                FoodName = string.Empty;
                InStock = 0;
                FoodType = FoodTypes[0];
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a food name.", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}