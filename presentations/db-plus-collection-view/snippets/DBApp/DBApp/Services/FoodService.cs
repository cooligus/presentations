using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBApp.Models;

namespace DBApp.Services
{
    // #region constructor
    public class FoodService : DB
    {
        public FoodService()
        {
            _database.CreateTableAsync<Food>().Wait();
        }
    }
    // #endregion constructor
    // #region get
    public class FoodService : DB
    {
        // Get all foods
        public Task<List<Food>> GetFoodsAsync()
        {
            return _database.Table<Food>().ToListAsync();
        }
    }
    // #endregion get
    // #region save
    public class FoodService : DB
    {
        // Add a new food
        public Task<int> SaveFoodAsync(Food food)
        {
            return _database.InsertAsync(food);
        }
    }
    // #endregion save
    // #region update
    public class FoodService : DB
    {
        // Update an existing food
        public Task<int> UpdateFoodAsync(Food food)
        {
            return _database.UpdateAsync(food);
        }
    }
    // #endregion update
    // #region delete
    public class FoodService : DB
    {
        // Delete a food
        public Task<int> DeleteFoodAsync(Food food)
        {
            return _database.DeleteAsync(food);
        }
    }
    // #endregion delete
}