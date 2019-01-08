using System.ComponentModel;
using System.Threading;

namespace McProtocolApp.Service
{
    public class TimerService
    {
        #region Events
        public event TickEventHandler Tick;
        public delegate void TickEventHandler(object sender, int tick);
        #endregion

        #region Properties

        private int _sleepTime = 1000;
        public int SleepTime
        {
            get
            {
                return _sleepTime;
            }

            set
            {
                if (_sleepTime != value)
                {
                    _sleepTime = value;
                }
            }
        }

        private string _serviceName;
        public string ServiceName
        {
            get
            {
                return _serviceName;
            }
            set
            {
                if (value != _serviceName)
                {
                    _serviceName = value;
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return (_worker != null) ? _worker.IsBusy : false;
            }
        }
        #endregion

        #region Public Methods
        public void RunServiceAsync()
        {
            if (_worker != null)
            {
                _worker.RunWorkerAsync(SleepTime);
            }
        }
        public void CancelServiceAsync()
        {
            _worker.CancelAsync();
        }
        #endregion

        #region Constructor
        public TimerService()
        {
            InitializeWorkers();
        }

        #region InitializeWorkers
        private void InitializeWorkers()
        {
            _worker = new BackgroundWorker();
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
        }
        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int sleepTime = (int)e.Argument;

            while (!_worker.CancellationPending)
            {
                RaiseTickEvent();
                Thread.Sleep(sleepTime);
            }
        }
        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
        #endregion

        #endregion

        #region Members
        private BackgroundWorker _worker;
        private int _ticks = 0;
        #endregion

        #region Private Methods
        private void RaiseTickEvent()
        {
            var copy = Tick;
            if (Tick != null)
            {
                copy(this, _ticks++);
            }
        }
        #endregion
    }
}