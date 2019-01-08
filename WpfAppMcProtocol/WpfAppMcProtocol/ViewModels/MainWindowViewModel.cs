using System;
using System.Windows.Input;
using System.Diagnostics;
using McProtocolApp.Service;
using McProtocolApp.Helpers;
using GalaSoft.MvvmLight;
using SimpleHmi.PlcService;
using System.Threading.Tasks;

namespace McProtocolApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string IpAddress
        {
            get { return _ipAddress; }
            set { Set(ref _ipAddress, value); }
        }
        private string _ipAddress;

        public bool HighLimit
        {
            get { return _highLimit; }
            set { Set(ref _highLimit, value); }
        }
        private bool _highLimit;

        public bool LowLimit
        {
            get { return _lowLimit; }
            set { Set(ref _lowLimit, value); }
        }
        private bool _lowLimit;

        public bool PumpState
        {
            get { return _pumpState; }
            set { Set(ref _pumpState, value); }
        }
        private bool _pumpState;

        public int TankLevel
        {
            get { return _tankLevel; }
            set { Set(ref _tankLevel, value); }
        }
        private int _tankLevel;

        public ConnectionStates ConnectionState
        {
            get { return _connectionState; }
            set { Set(ref _connectionState, value); }
        }
        private ConnectionStates _connectionState;

        public TimeSpan ScanTime
        {
            get { return _scanTime; }
            set { Set(ref _scanTime, value); }
        }
        private TimeSpan _scanTime;

        public ICommand ConnectCommand { get; private set; }

        public ICommand DisconnectCommand { get; private set; }

        public ICommand StartCommand { get; private set; }

        public ICommand StopCommand { get; private set; }

        S7PlcService _plcService;

        public MainWindowViewModel()
        {
            InitializeServices();
            _timer.RunServiceAsync(); //tod on command
            _clickCommand = new DelegateCommand(Click, CanExecuteClick);
            _isClickButtonEnable = true;

            //Siemens
            _plcService = new S7PlcService();
            ConnectCommand = new DelegateCommand(Connect);
            DisconnectCommand = new DelegateCommand(Disconnect);
            StartCommand = new DelegateCommand(async () => { await Start(); });
            StopCommand = new DelegateCommand(async () => { await Stop(); });

            IpAddress = "127.0.0.1";

            OnPlcServiceValuesRefreshed(null, null);
            _plcService.ValuesRefreshed += OnPlcServiceValuesRefreshed;
        }

        private void OnPlcServiceValuesRefreshed(object sender, EventArgs e)
        {
            ConnectionState = _plcService.ConnectionState;
            PumpState = _plcService.PumpState;
            HighLimit = _plcService.HighLimit;
            LowLimit = _plcService.LowLimit;
            TankLevel = _plcService.TankLevel;
            ScanTime = _plcService.ScanTime;
        }

        private void Connect()
        {
            _plcService.Connect(IpAddress, 0, 1);
        }

        private void Disconnect()
        {
            _plcService.Disconnect();
        }

        private async Task Start()
        {
            await _plcService.WriteStart();
        }

        private async Task Stop()
        {
            await _plcService.WriteStop();
        }

        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                if (_message != value)
                {
                    _message = value;
                    RaisePropertyChanged(() => Message);
                }
            }
        }

        #region InitializeServices

        public bool IsClickButtonEnable
        {
            get
            {
                return _isClickButtonEnable;
            }
            set
            {
                if (_isClickButtonEnable != value)
                {
                    _isClickButtonEnable = value;
                    RaisePropertyChanged(() => IsClickButtonEnable);
                }
            }
        }

        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand;
            }

            private set { }
        }

        private void Click()
        {
            _count++;
            Message = string.Format("Click #{0}", _count);
        }

        private bool CanExecuteClick()
        {
            Debug.WriteLine("called CanExecuteClick: {0}", DateTime.Now);
            return IsClickButtonEnable;
        }

        private void InitializeServices()
        {
            _timer = new TimerService();
            _timer.SleepTime = 1000; //1s
            _timer.Tick += _timer_Tick;
        }

        void _timer_Tick(object sender, int tick)
        {
            Message = string.Format("Tick #{0}", tick);
        }
        #endregion

        #region Members
        private TimerService _timer;
        private bool _isClickButtonEnable;
        private ICommand _clickCommand;
        private int _count = 0;
        #endregion
    }
}