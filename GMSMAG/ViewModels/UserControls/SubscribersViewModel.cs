using GMSMAG.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui.Controls;
using GMSMAG.Views.UserControls;
using GMSMAG.Data;
using GMSMAG.Views.Windows;

namespace GMSMAG.ViewModels.UserControls
{
    public class SubscribersViewModel : INotifyPropertyChanged
    {
        #region Fields
        private readonly MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;
        private readonly IDataHelper<Subscriber> _dataHelper;
        private readonly IDataHelper<SubscriptionsTypes> _dataHelperForSTs;
        private Subscriber _subscriber;
        private Subscriber _selectedSubscriber;
        private List<Subscriber> _subscribers;
        private string _searchText;
        private string _colName = "0";
        #endregion

        #region Properties
        public int AdminId
        {
            get => _subscriber.AdminId;
            set
            {
                if (_subscriber.AdminId != value)
                {
                    _subscriber.AdminId = value == 0 ? 1 : value;
                    OnPropertyChanged(nameof(AdminId));
                }
            }
        }

        public string FirstName
        {
            get => _subscriber.FirstName;
            set
            {
                if (_subscriber.FirstName != value)
                {
                    _subscriber.FirstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get => _subscriber.LastName;
            set
            {
                if (_subscriber.LastName != value)
                {
                    _subscriber.LastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public string PhoneNumber
        {
            get => _subscriber.PhoneNumber;
            set
            {
                if (_subscriber.PhoneNumber != value)
                {
                    _subscriber.PhoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public string HomePhoneNumber
        {
            get => _subscriber.HomePhoneNumber;
            set
            {
                if (_subscriber.HomePhoneNumber != value)
                {
                    _subscriber.HomePhoneNumber = value;
                    OnPropertyChanged(nameof(HomePhoneNumber));
                }
            }
        }

        public DateTime Birthday
        {
            get => _subscriber.Birthday;
            set
            {
                if (_subscriber.Birthday != value)
                {
                    _subscriber.Birthday = value;
                    OnPropertyChanged(nameof(Birthday));
                }
            }
        }

        public string Address
        {
            get => _subscriber.Address;
            set
            {
                if (_subscriber.Address != value)
                {
                    _subscriber.Address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public List<Subscription> Subscriptions
        {
            get => _subscriber.Subscriptions.ToList();
            set
            {
                if (_subscriber.Subscriptions != value)
                {
                    _subscriber.Subscriptions = value;
                    OnPropertyChanged(nameof(Subscriptions));
                }
            }
        }

        public DateTime? UpdatedAt
        {
            get => _subscriber.UpdatedAt;
            set
            {
                if (_subscriber.UpdatedAt != value)
                {
                    _subscriber.UpdatedAt = value;
                    OnPropertyChanged(nameof(UpdatedAt));
                }
            }
        }

        public Subscriber SelectedSubscriber
        {
            get => _selectedSubscriber;
            set
            {
                if (_selectedSubscriber != value)
                {
                    _selectedSubscriber = value;
                    OnPropertyChanged(nameof(SelectedSubscriber));
                    if (_selectedSubscriber != null) SetData();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        public string ColName
        {
            get => _colName;
            set
            {
                if (_colName != value)
                {
                    _colName = value ;
                    OnPropertyChanged(nameof(ColName));
                }
            }
        }

        public List<Subscriber> Subscribers
        {
            get => _subscribers;
            set
            {
                if (_subscribers != value)
                {
                    _subscribers = value;
                    OnPropertyChanged(nameof(Subscribers));
                }
            }
        }
        #endregion

        #region Commands
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand LoadCommand { get; }
        #endregion

        public SubscribersViewModel()
        {
            _subscriber = new Subscriber();
            _subscribers = new List<Subscriber>();
            _dataHelper = App.GetService<IDataHelper<Subscriber>>();
            _dataHelperForSTs = App.GetService<IDataHelper<SubscriptionsTypes>>();

            AddCommand = new RelayCommand(AddData);
            EditCommand = new RelayCommand(EditData);
            SaveCommand = new RelayCommand(SaveData);
            DeleteCommand = new RelayCommand(DeleteData);
            SearchCommand = new RelayCommand(SearchData);
            LoadCommand = new RelayCommand(() => LoadData(1,50));

            LoadData();
        }

        private async void DeleteData()
        {
            if (_selectedSubscriber == null)
            {
                await ShowMessageBoxAsync("Error", "Please select a subscriber to delete.");
                return;
            }

            var res = await ShowConfirmationDialogAsync("Delete Subscriber", "Are you sure you want to delete this subscriber?");
            if (res == ContentDialogResult.Primary)
            {
                await _dataHelper.DeleteAsync(_selectedSubscriber.Id);
                LoadData();
            }
        }

        private async void SaveData()
        {
            if (_selectedSubscriber != null)
            {
                await _dataHelper.EditAsync(_subscriber);
            }
            else
            {
                await _dataHelper.AddAsync(_subscriber);
            }
            LoadData();
        }

        private async void EditData()
        {
            if (_selectedSubscriber == null)
            {
                await ShowMessageBoxAsync("Error", "Please select a subscriber to edit.");
                return;
            }

            var subscriptionsTypes = await _dataHelperForSTs.GetAllAsync();

            var dialog = new ContentDialog
            {
                DialogHost = _mainWindow.RootContentDialog,
                DataContext = this,
                Title = "Edit Subscriber",
                Content = new AddSubscriber(this, subscriptionsTypes),
                PrimaryButtonText = "Save",
                CloseButtonText = "Close",
                DefaultButton = ContentDialogButton.Primary,
            };

            var res = await dialog.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                SaveData();
            }
        }

        private void SetData()
        {
            AdminId = _selectedSubscriber.AdminId;
            FirstName = _selectedSubscriber.FirstName;
            LastName = _selectedSubscriber.LastName;
            PhoneNumber = _selectedSubscriber.PhoneNumber;
            HomePhoneNumber = _selectedSubscriber.HomePhoneNumber;
            Birthday = _selectedSubscriber.Birthday;
            Address = _selectedSubscriber.Address;
            Subscriptions = new List<Subscription>(_selectedSubscriber.Subscriptions);
            UpdatedAt = _selectedSubscriber.UpdatedAt;
        }

        private async void AddData()
        {
            ClearData();
            var subscriptionsTypes = await _dataHelperForSTs.GetAllAsync();

            var dialog = new ContentDialog
            {
                DialogHost = _mainWindow.RootContentDialog,
                DataContext = this,
                Title = "Add Subscriber",
                Content = new AddSubscriber(this, subscriptionsTypes),
                PrimaryButtonText = "Add",
                CloseButtonText = "Close",
                DefaultButton = ContentDialogButton.Primary,
            };

            var res = await dialog.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                SaveData();
            }
        }

        private void ClearData()
        {
            AdminId = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            HomePhoneNumber = string.Empty;
            Birthday = DateTime.Now;
            Address = string.Empty;
            Subscriptions = new List<Subscription>();
            UpdatedAt = null;
        }

        private async void LoadData(int page = 1, int limit = 50)
        {
            var items = await _dataHelper.GetAllAsync(page, limit);
            Subscribers = new List<Subscriber>(items);
        }

        private async void SearchData()
        {
            var items = await _dataHelper.SearchAsync(_searchText, _colName);
            Subscribers = new List<Subscriber>(items);
        }

        private async Task ShowMessageBoxAsync(string title, string content)
        {
            var messageBox = new Wpf.Ui.Controls.MessageBox
            {
                Title = title,
                Content = content,
            };
            await messageBox.ShowDialogAsync();
        }

        private async Task<ContentDialogResult> ShowConfirmationDialogAsync(string title, string content)
        {
            var messageBox = new Wpf.Ui.Controls.MessageBox
            {
                Title = title,
                Content = content,
                CloseButtonText = "No",
                PrimaryButtonText = "Yes",
                PrimaryButtonAppearance = ControlAppearance.Danger,
            };

            return (ContentDialogResult)await messageBox.ShowDialogAsync();
        }

        // PropertyChanged event
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
