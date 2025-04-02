using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DBApp.Models;
using SQLite;

namespace DBApp.Services
{
    // #region snippet
    public class DB
    {
        protected SQLiteAsyncConnection _database { get; }
        protected string _dbName { get; set; } = "FavouriteFood.db";
        public DB()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _dbName);
            _database = new SQLiteAsyncConnection(path);
        }
    }
    // #endregion snippet
}
