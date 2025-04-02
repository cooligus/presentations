using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBApp.Models;
using System.IO;
using System;
using System.Security.Cryptography.X509Certificates;

namespace DBApp.Services
{
    // #region constructor
    public class ClientService : DB
    {
        public ClientService()
        {
            _database.CreateTableAsync<Client>().Wait();
        }
    }
    // #endregion constructor
    // #region get
    public class ClientService : DB
    {
        public Task<List<Client>> GetClientsAsync()
        {
            return _database.Table<Client>().ToListAsync();
        }
    }
    // #endregion get
    // #region save
    public class ClientService : DB
    {
        public Task<int> SaveClientAsync(Client client)
        {
            return _database.InsertAsync(client);
        }
    }
    // #endregion save
    // #region update
    public class ClientService : DB
    {
        public Task<int> UpdateClientAsync(Client client)
        {
            return _database.UpdateAsync(client);
        }
    }
    // #endregion update
    // #region delete
    public class ClientService : DB
    {
        public Task<int> DeleteClientAsync(Client client)
        {
            return _database.DeleteAsync(client);
        }
    }
    // #endregion delete
}