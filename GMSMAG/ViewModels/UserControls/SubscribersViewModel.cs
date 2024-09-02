using GMSMAG.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMSMAG.Data;
using Mysqlx.Crud;
using System.Windows.Input;
using Wpf.Ui.Controls;
using GMSMAG.Views.Windows;
using GMSMAG.Views.UserControls;

namespace GMSMAG.ViewModels.UserControls
{
    public class SubscribersViewModel : INotifyPropertyChanged
    {
        #region Fields
        private MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;
        private IDataHelper<Subscriber> dataHelper;
        private IDataHelper<SubscriptionsTypes> dataHelperForSTs;
        private Subscriber subscriber;
        private Subscriber selectedSubscriber;
        private ObservableCollection<Subscriber> subscribers;
        #endregion

        #region Properties
        public int AdminId
        {
            get
            {
                return subscriber.AdminId;
            }
            set
            {
                if (subscriber.AdminId != value)
                {
                    subscriber.AdminId = value == 0?1:value;
                    OnPropertyChanged(nameof(AdminId));
                }
            }
        }

        public string FirstName
        {
            get
            {
                return subscriber.FirstName;
            }
            set
            {
                if (subscriber.FirstName != value)
                {
                    subscriber.FirstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get
            {
                return subscriber.LastName;
            }
            set
            {
                if (subscriber.LastName != value)
                {
                    subscriber.LastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public string PhoneNumber
        {
            get
            {
                return subscriber.PhoneNumber;
            }
            set
            {
                if (subscriber.PhoneNumber != value)
                {
                    subscriber.PhoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public string HomePhoneNumber
        {
            get
            {
                return subscriber.HomePhoneNumber;
            }
            set
            {
                if (subscriber.HomePhoneNumber != value)
                {
                    subscriber.HomePhoneNumber = value;
                    OnPropertyChanged(nameof(HomePhoneNumber));
                }
            }
        }

        public DateTime Birthday
        {
            get
            {
                return subscriber.Birthday;
            }
            set
            {
                if (subscriber.Birthday != value)
                {
                    subscriber.Birthday = value;
                    OnPropertyChanged(nameof(Birthday));
                }
            }
        }

        public string Address
        {
            get
            {
                return subscriber.Address;
            }
            set
            {
                if (subscriber.Address != value)
                {
                    subscriber.Address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public ICollection<Subscription> Subscriptions
        {
            get
            {
                return subscriber.Subscriptions;
            }
            set
            {
                if (subscriber.Subscriptions != value)
                {
                    subscriber.Subscriptions = value;
                    OnPropertyChanged(nameof(Subscriptions));
                }
            }
        }

        public DateTime? UpdatedAt
        {
            get
            {
                return subscriber.UpdatedAt;
            }
            set
            {
                if (subscriber.UpdatedAt != value)
                {
                    subscriber.UpdatedAt = value;
                    OnPropertyChanged(nameof(UpdatedAt));
                }
            }
        }

        public Subscriber SelectedSubscriber
        {
            get
            {
                return selectedSubscriber;
            }
            set
            {
                if (selectedSubscriber != value)
                {
                    selectedSubscriber = value;
                    OnPropertyChanged(nameof(SelectedSubscriber));
                }
            }
        }



        public ObservableCollection<Subscriber> Subscribers
        {
            get
            {
                return subscribers;
            }
            set
            {
                if (subscribers != value)
                {
                    subscribers = value;
                    OnPropertyChanged(nameof(subscribers));
                }
            }
        }
        #endregion

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand PrintCommand { get; }

        public SubscribersViewModel()
        {
            subscriber = new Subscriber();
            subscribers = new ObservableCollection<Subscriber>();
            dataHelper = App.GetService<IDataHelper<Subscriber>>();
            dataHelperForSTs = App.GetService<IDataHelper<SubscriptionsTypes>>();
            // LoadData
            LoadData();
            AddCommand = new RelayCommand(AddData);
            EditCommand = new RelayCommand(EditData);
            SaveCommand = new RelayCommand(SaveData);
            DeleteCommand = new RelayCommand(DeleteData);
        }

        private async void DeleteData()
        {
            if(selectedSubscriber == null)
            {
                var errorMessageBox = new Wpf.Ui.Controls.MessageBox
                {
                    Title = "Error",
                    Content = "Please select a subscriber to delete.",
                };
                await errorMessageBox.ShowDialogAsync();
                return;
            }

            var messageBox = new Wpf.Ui.Controls.MessageBox
            {
                Title = "Delete Subscriber",
                Content = "Are you sure you want to delete this subscriber?",
                CloseButtonText = "No",
                PrimaryButtonText = "Yes",
                PrimaryButtonAppearance = ControlAppearance.Danger,
            };

            var res = await messageBox.ShowDialogAsync();

            if (res == Wpf.Ui.Controls.MessageBoxResult.Primary)
            {
                if (selectedSubscriber != null)
                {
                    await dataHelper.DeleteAsync(selectedSubscriber.Id);
                    LoadData();
                }
            }
        }

        private void SaveData()
        {
            if(selectedSubscriber != null)
            {
                Subscriber _subscriber = new Subscriber
                {
                    Id = selectedSubscriber.Id,
                    AdminId = selectedSubscriber.AdminId,
                    FirstName = FirstName,
                    LastName = LastName,
                    PhoneNumber = PhoneNumber,
                    HomePhoneNumber = HomePhoneNumber,
                    Birthday = Birthday,
                    Address = Address,
                    Subscriptions = Subscriptions,
                    UpdatedAt = UpdatedAt
                };
                dataHelper.EditAsync(_subscriber);
            }
            else
            {

                Subscriber _subscriber = new Subscriber
                {
                    AdminId = AdminId,
                    FirstName = FirstName,
                    LastName = LastName,
                    PhoneNumber = PhoneNumber,
                    HomePhoneNumber = HomePhoneNumber,
                    Birthday = Birthday,
                    Address = Address,
                    Subscriptions = Subscriptions,
                    UpdatedAt = null
                };
                dataHelper.AddAsync(_subscriber);
            }
            LoadData();
        }

        private async void EditData()
        {
            if (selectedSubscriber != null)
            {
                SetData();
                var subscriptionsTypes = await dataHelperForSTs.GetAllAsync();

                var dialog = new ContentDialog
                {
                    DialogHost = _mainWindow.RootContentDialog,
                    DataContext = this,
                    Title = "Add Subscriber",
                    Content = new AddSubscriber(this, subscriptionsTypes),
                    PrimaryButtonText = "Save",
                    DefaultButton = ContentDialogButton.Primary,
                    CloseButtonText = "Close",
                };

                var res = await dialog.ShowAsync();
                if (res == ContentDialogResult.Primary)
                {
                    SaveData();
                }
            }
            else
            {
               var messageBox = new Wpf.Ui.Controls.MessageBox
               {
                   Title = "Error",
                   Content = "Please select a subscriber to edit.",
               };
               await messageBox.ShowDialogAsync();
            }
            
        }

        private void SetData()
        {
            FirstName = selectedSubscriber.FirstName;
            LastName = selectedSubscriber.LastName;
            PhoneNumber = selectedSubscriber.PhoneNumber;
            HomePhoneNumber = selectedSubscriber.HomePhoneNumber;
            Birthday = selectedSubscriber.Birthday;
            Address = selectedSubscriber.Address;
            Subscriptions = selectedSubscriber.Subscriptions;
            UpdatedAt = selectedSubscriber.UpdatedAt;
        }

        private async void AddData()
        {
            ClearData();
            var subscriptionsTypes = await dataHelperForSTs.GetAllAsync();

            var dialog = new ContentDialog
            {
                DialogHost = _mainWindow.RootContentDialog,
                DataContext = this,
                Title = "Add Subscriber",
                Content = new AddSubscriber(this,subscriptionsTypes),
                PrimaryButtonText = "Add",
                DefaultButton = ContentDialogButton.Primary,
                CloseButtonText = "Close",
            };

            var res =  await dialog.ShowAsync();
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
            Subscriptions = new ObservableCollection<Subscription>();
            UpdatedAt = null;
        }

        private async void LoadData(int Page = 1,int Limit = 50)
        {
            subscribers.Clear();
            foreach (Subscriber item in (await dataHelper.GetAllAsync(Page,Limit)))
            {
                subscribers.Add(item);
            }
        }

        // PropertyChanged event
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
