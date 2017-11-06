using ForeignExchange.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ForeignExchange.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes

        bool _isRunnig;
        string _result;

        #endregion

        #region Properties

        public string Amount { get; set; }

        public ObservableCollection<Rate> Rates { get; set; }

        public Rate SourceRate { get; set; }

        public Rate TargetRate { get; set; }

        public bool IsRunnning
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
                        new PropertyChangedEventArgs(nameof(IsRunnning)));
                }
            }
        }

        public bool IsEnabled { get; set; }

        public string Result { get; set; }

        #endregion

        #region Commands

        public ICommand ConvertCommand
        {
            get
            {
                return new RelayCommand(Convert);
            }
        }

        

        void Convert()
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Constructors
        public MainViewModel()
        {
            LoadRates();
        }
        #endregion

        void LoadRates()
        {
            IsRunning = true;
            Result = "Loading rates...";

        }
    }
}
