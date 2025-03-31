using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBApp.Models;
using System.IO;
using System;
using System.Security.Cryptography.X509Certificates;

namespace DBApp.Services
{
    public class ClientService : DB
    {
        public ClientService()
        {
            _database.CreateTableAsync<Client>().Wait();
        }

     
        public Task<List<Client>> GetClientsAsync()
        {
            return _database.Table<Client>().ToListAsync();
        }

     
        public Task<int> SaveClientAsync(Client client)
        {
            return _database.InsertAsync(client);
        }

        public Task<int> UpdateClientAsync(Client client)
        {
            return _database.UpdateAsync(client);
        }

 
        public Task<int> DeleteClientAsync(Client client)
        {
            return _database.DeleteAsync(client);
        }
    }
}