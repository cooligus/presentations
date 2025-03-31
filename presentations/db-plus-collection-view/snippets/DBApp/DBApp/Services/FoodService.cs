using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBApp.Models;

namespace DBApp.Services
{
    public class FoodService : DB
    {
        public FoodService()
        {
            _database.CreateTableAsync<Food>().Wait();
        }

        // Get all foods
        public Task<List<Food>> GetFoodsAsync()
        {
            return _database.Table<Food>().ToListAsync();
        }

        // Add a new food
        public Task<int> SaveFoodAsync(Food food)
        {
            return _database.InsertAsync(food);
        }

        // Update an existing food
        public Task<int> UpdateFoodAsync(Food food)
        {
            return _database.UpdateAsync(food);
        }

        // Delete a food
        public Task<int> DeleteFoodAsync(Food food)
        {
            return _database.DeleteAsync(food);
        }
    }
}