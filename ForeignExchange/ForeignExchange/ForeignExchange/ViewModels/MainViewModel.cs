using ForeignExchange.Helpers;
using ForeignExchange.Models;
using ForeignExchange.Services;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace ForeignExchange.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiService;
        DialogService dialogService;
        DataService dataService;
        #endregion

        #region Attributes

        bool _isRunnig;
        bool _isEnabled;
        string _result;
        ObservableCollection<Rate> _rates;
        Rate _sourceRate;
        Rate _targetRate;
        string _status;



        #endregion

        #region Properties

        public string Amount { get; set; }

        public string Status
        {
            get
            {
                return _status;
            }

            set
            {
                if (_status != value)
                {
                    _status = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Status)));
                }
            }
        }

        public ObservableCollection<Rate> Rates
        {
            get
            {
                return _rates;
            }

            set
            {
                if (_rates != value)
                {
                    _rates = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Rates)));
                }
            }
        }

        public Rate SourceRate
        {
            get
            {
                return _sourceRate;
            }

            set
            {
                if (_sourceRate != value)
                {
                    _sourceRate = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(SourceRate)));
                }
            }
        }

        public Rate TargetRate
        {
            get
            {
                return _targetRate;
            }

            set
            {
                if (_targetRate != value)
                {
                    _targetRate = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(TargetRate)));
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunnig;
            }

            set
            {
                if (_isRunnig != value)
                {
                    _isRunnig = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }

        public string Result
        {
            get
            {
                return _result;
            }

            set
            {
                if (_result != value)
                {
                    _result = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }

       

        #endregion

        #region Commands

        public ICommand SwitchCommand
        {
            get
            {
                return new RelayCommand(SwitchCom);
            }
        }

        public ICommand ConvertCommand
        {
            get
            {
                return new RelayCommand(Convert);
            }
        }

        void SwitchCom()
        {
            var aux = SourceRate;
            SourceRate = TargetRate;
            TargetRate = aux;
            Convert();
        }

        async void Convert()
        {
            if (string.IsNullOrEmpty(Amount))
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.AmountValidation);
                return;
            }

            decimal amount = 0;

            if (!decimal.TryParse(Amount, out amount))
            {
                await dialogService.ShowMessage(
                      Lenguages.Error,
                      Lenguages.NumericValue);
                return;
            }

            if (SourceRate == null)
            {
                await dialogService.ShowMessage(
                      Lenguages.Error,
                      Lenguages.SourceRate);
                return;
            }

            if (TargetRate == null)
            {
                await dialogService.ShowMessage(
                      Lenguages.Error,
                      Lenguages.TargetRate);
                return;
            }

            var amountConverted = amount / 
                                  (decimal)SourceRate.TaxRate * 
                                  (decimal)TargetRate.TaxRate;
            Result = string.Format(
               "{0} ${1:N2} = {2} ${3:N2}",
               SourceRate.Code,
               amount,
               TargetRate.Code,
               amountConverted);
        }

        #endregion


        #region Constructors
        public MainViewModel()
        {
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();

            LoadRates();
        }
        #endregion

        #region Methods
        async void LoadRates()
        {
            IsRunning = true;
            Result = Lenguages.Loading ;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSucess)
            {
                IsRunning = false;
                Result = connection.Message;
                return;
            }

            var url = "http://apiexchangerates.azurewebsites.net"; // Application.Current.Resources["URLAPI"].ToString();

            var response = await apiService.GetList<Rate>(
                         url,
                           "/api/Rates");
            if (!response.IsSucess)
            {
                IsRunning = false;
                Result = response.Message;
                return;
            }

            //Gravar os dados localmente

            var rates = (List<Rate>)response.Result;
            dataService.DeleteAll<Rate>();
            dataService.Save(rates);

            Rates = new ObservableCollection<Rate>(rates);

            IsRunning = false;
            IsEnabled = true;
            Result = Lenguages.ResultConvert;
            Status = Lenguages.Status;
        }
        #endregion
    }
}
