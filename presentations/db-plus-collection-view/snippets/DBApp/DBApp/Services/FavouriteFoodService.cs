using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBApp.Models;
using System.Linq;

namespace DBApp.Services
{
    // #region constructor
    public class FavouriteFoodService : DB
    {
        public FavouriteFoodService()
        {
            _database.CreateTableAsync<FavouriteFood>().Wait();
        }
    }
    // #endregion constructor
    // #region get
    public class FavouriteFoodService : DB
    {
        // Get all favourite foods
        public async Task<List<FavouriteFood>> GetFavouriteFoodsAsync()
        {
            var favouriteFoods = await _database.Table<FavouriteFood>().ToListAsync();
            var foodIds = favouriteFoods.Select(ff => ff.FoodId).Distinct().ToList();
            var clientIds = favouriteFoods.Select(ff => ff.ClientId).Distinct().ToList();

            var foods = await _database.Table<Food>().Where(f => foodIds.Contains(f.Id)).ToListAsync();
            var clients = await _database.Table<Client>().Where(c => clientIds.Contains(c.Id)).ToListAsync();

            foreach (var favouriteFood in favouriteFoods)
            {
                favouriteFood.Food = foods.FirstOrDefault(f => f.Id == favouriteFood.FoodId);
                favouriteFood.Client = clients.FirstOrDefault(c => c.Id == favouriteFood.ClientId);
            }

            return favouriteFoods;
        }
    }
    // #endregion get
    // #region save
    public class FavouriteFoodService : DB
    {
        // Add a new favourite food
        public Task<int> SaveFavouriteFoodAsync(FavouriteFood favouriteFood)
        {
            favouriteFood.ClientId = favouriteFood.Client.Id;
            favouriteFood.FoodId = favouriteFood.Food.Id;
            return _database.InsertAsync(favouriteFood);
        }
    }
    // #endregion save
    // #region update
    public class FavouriteFoodService : DB
    {
        // Update an existing favourite food
        public Task<int> UpdateFavouriteFoodAsync(FavouriteFood favouriteFood)
        {
            return _database.UpdateAsync(favouriteFood);
        }
    }
    // #endregion update
    // #region delete
    public class FavouriteFoodService : DB
    {
        // Delete a favourite food
        public Task<int> DeleteFavouriteFoodAsync(FavouriteFood favouriteFood)
        {
            return _database.DeleteAsync(favouriteFood);
        }
    }
    // #endregion delete
}