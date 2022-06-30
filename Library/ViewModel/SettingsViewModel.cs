using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Configuration;

namespace Library.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        string _name, _connectionString, _provider;
        IEnumerable<string> _connections;
        string _selectedConnection;



        public IEnumerable<string> Connections
        {
            get => _connections;
            set
            {
                _connections = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Connections)));
            }
        }

        public string SelectedConnection
        {
            get => _selectedConnection;
            set
            {
                _selectedConnection = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedConnection)));
            }
        }
        public string ConnectionName
        {
            get => _name;
            set
            {
                _name = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ConnectionName)));
            }
        }
        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                _connectionString = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ConnectionString)));
            }
        }
        public string Provider
        {
            get => _provider;
            set
            {
                _provider = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Provider)));
            }
        }

        public SettingsViewModel()
        {
            Connections = (from item in Enumerable.Range(0, ConfigurationManager.ConnectionStrings.Count)
                           select ConfigurationManager.ConnectionStrings[item].Name);
        }

        RelayCommand _selectConnection;
        public RelayCommand SelectConnectionCommand
        {
            get => _selectConnection = _selectConnection ?? new RelayCommand(SelectConnection);
        }

        RelayCommand _addConnection;
        public RelayCommand AddConnectionCommand
        {
            get => _addConnection = _addConnection ?? new RelayCommand(AddConnection);
        }

        void SelectConnection()
        {
            if (SelectedConnection == null) return;

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (config.AppSettings.Settings["currentConnection"] == null)
            {
                config.AppSettings.Settings.Add(new KeyValueConfigurationElement("currentConnection", SelectedConnection));
            }
            else
                config.AppSettings.Settings["currentConnection"].Value = SelectedConnection;
            config.Save(ConfigurationSaveMode.Modified);
            
        }

        void AddConnection()
        {
            if (ConnectionName == null || ConnectionString == null || Provider == null) return;

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            //config.AppSettings.CurrentConfiguration.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(ConnectionName, ConnectionString, Provider));
            config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(ConnectionName, ConnectionString, Provider));
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
