using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using DBApp.Models;
using DBApp.Services;
using System.Threading.Tasks;

namespace DBApp.ViewModels
{
    // #region clientName
    public class ClientViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Client> Clients { get; set; }
        public ICommand AddClientCommand { get; set; }

        private string _clientName;
        public string ClientName
        {
            get => _clientName;
            set
            {
                _clientName = value;
                OnPropertyChanged(nameof(ClientName));
            }
        }
    }
    // #endregion clientName
    // #region clientSurname
    public class ClientViewModel : INotifyPropertyChanged
    {
        private string _clientSurname;
        public string ClientSurname
        {
            get => _clientSurname;
            set
            {
                _clientSurname = value;
                OnPropertyChanged(nameof(ClientSurname));
            }
        }
    }
    // #endregion clientSurname
    // #region constructor
    public class ClientViewModel : INotifyPropertyChanged
    {
        private readonly ClientService _clientService;
        
        public ClientViewModel()
        {
            Clients = new ObservableCollection<Client>();
            AddClientCommand = new Command(async () => await AddClient());

            _clientService = new ClientService();
            LoadClients();
        }
    }
    // #endregion constructor
    // #region loadClient
    public class ClientViewModel : INotifyPropertyChanged
    {
        private async void LoadClients()
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
    // #region addClient
    public class ClientViewModel : INotifyPropertyChanged
    {
        private async Task AddClient()
        {
            if (!string.IsNullOrWhiteSpace(ClientName) && !string.IsNullOrWhiteSpace(ClientSurname))
            {
                var newClient = new Client { Name = ClientName, Surname = ClientSurname };
                await _clientService.SaveClientAsync(newClient);
                Clients.Add(newClient);
                ClientName = string.Empty; // Reset input
                ClientSurname = string.Empty; // Reset input
            }
            else
            {
                // Optionally, show an alert if the input is invalid
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter both name and surname.", "OK");
            }
        }
    }
    // #endregion addClient
    // #region onPropertyChanged
    public class ClientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    // #endregion onPropertyChanged
}