using GMSMAG.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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

        private readonly MainWindow _mainWindow = App.GetService<MainWindow>();
        private readonly IDataHelper<Subscriber> _dataHelper;
        private readonly IDataHelper<SubscriptionType> _dataHelperForSTs;
        private readonly IDataHelper<Subscription> _dataHelperForSubscriptions;
        private Subscriber _subscriber;
        private Subscriber _selectedSubscriber;
        private List<Subscriber> _subscribers;
        private string _searchText;
        private string _colName = "Id"; // Default column name for search
        private bool _isLoading;
        private string _subscriptionId;

        #endregion

        #region Properties

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

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
            get => _subscriber.Subscriptions?.ToList() ?? new List<Subscription>();
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
                    _colName = value;
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

        public string SubscriptionId
        {
            get => _subscriptionId;
            set
            {
                if (_subscriptionId != value)
                {
                    _subscriptionId = value;
                    OnPropertyChanged(nameof(SubscriptionId));
                }
            }
        }

        #endregion

        #region Commands

        public ICommand AddDataCommand { get; }
        public ICommand EditDataCommand { get; }
        public ICommand DeleteDataCommand { get; }
        public ICommand SaveDataCommand { get; }
        public ICommand SearchDataCommand { get; }
        public ICommand LoadDataCommand { get; }

        #endregion

        public SubscribersViewModel(
            IDataHelper<Subscriber> dataHelper,
            IDataHelper<SubscriptionType> dataHelperForSTs,
            IDataHelper<Subscription> dataHelperForSubscriptions)
        {
            _subscriber = new Subscriber();
            _subscribers = new List<Subscriber>();
            _dataHelper = dataHelper;
            _dataHelperForSTs = dataHelperForSTs;
            _dataHelperForSubscriptions = dataHelperForSubscriptions;

            AddDataCommand = new RelayCommand(async () => await AddData());
            EditDataCommand = new RelayCommand(async () => await EditData());
            SaveDataCommand = new RelayCommand(async () => await SaveData());
            DeleteDataCommand = new RelayCommand(async () => await DeleteData());
            SearchDataCommand = new RelayCommand(async () => await SearchData());
            LoadDataCommand = new RelayCommand(async () => await LoadData(1, 50));
        }

        private async Task DeleteData()
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
                await LoadData();
            }
        }

        private async Task SaveData()
        {
            if (!string.IsNullOrEmpty(_subscriptionId))
            {
                var st = await _dataHelperForSTs.FindAsync(int.Parse(_subscriptionId));
                _subscriber.Subscriptions.Add(new Subscription
                {
                    AdminId = 1,
                    SubscriberId = _subscriber.Id,
                    SubscriptionTypeId = int.Parse(_subscriptionId),
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(st.DurationInDays),
                });
            }

            if (_selectedSubscriber != null)
            {
                await _dataHelper.EditAsync(_subscriber);
            }
            else
            {
                await _dataHelper.AddAsync(_subscriber);
            }

            await LoadData();
        }

        private async Task EditData()
        {
            if (_selectedSubscriber == null)
            {
                await ShowMessageBoxAsync("Error", "Please select a subscriber to edit.");
                return;
            }

            var subscriptionTypes = await _dataHelperForSTs.GetAllAsync();

            var dialog = new ContentDialog
            {
                DialogHost = _mainWindow.RootContentDialog,
                DataContext = this,
                Title = "Edit Subscriber",
                Content = new AddSubscriber(this, subscriptionTypes),
                PrimaryButtonText = "Save",
                CloseButtonText = "Close",
                DefaultButton = ContentDialogButton.Primary,
            };

            var res = await dialog.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                await SaveData();
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

        public async Task AddData()
        {
            ClearData();
            var subscriptionTypes = await _dataHelperForSTs.GetAllAsync();

            var dialog = new ContentDialog
            {
                DialogHost = _mainWindow.RootContentDialog,
                DataContext = this,
                Title = "Add Subscriber",
                Content = new AddSubscriber(this, subscriptionTypes),
                PrimaryButtonText = "Add",
                CloseButtonText = "Close",
                DefaultButton = ContentDialogButton.Primary,
            };

            var res = await dialog.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                await SaveData();
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
        }

        public async Task SearchData()
        {
            IsLoading = true;
            try
            {
                var result = await _dataHelper.SearchAsync(SearchText, ColName);
                Subscribers = result.ToList();
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task LoadData(int pageNumber = 1, int pageSize = 50)
        {
            IsLoading = true;
            try
            {
                Subscribers = (await _dataHelper.GetAllAsync(pageNumber, pageSize)).ToList();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private Task ShowMessageBoxAsync(string title, string message)
        {
            var messageBox = new Wpf.Ui.Controls.MessageBox
            {
                Title = title,
                Content=message
            };
            return messageBox.ShowDialogAsync();
        }

        private Task<ContentDialogResult> ShowConfirmationDialogAsync(string title, string message)
        {
            var dialog = new ContentDialog
            {
                DialogHost = _mainWindow.RootContentDialog,
                Title = title,
                Content = message,
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };
            return dialog.ShowAsync();
        }

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
