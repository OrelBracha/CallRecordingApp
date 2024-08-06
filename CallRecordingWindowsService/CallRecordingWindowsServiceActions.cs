using System;
using System.ServiceModel;
using System.ServiceProcess;
using CallRecordingService;

namespace CallRecordingWindowsService
{
    public partial class CallRecordingWindowsServiceActions : ServiceBase
    {
        private ServiceHost _serviceHost = null;

        public CallRecordingWindowsServiceActions()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (_serviceHost != null)
            {
                _serviceHost.Close();
            }

            _serviceHost = new ServiceHost(typeof(CallRecordingService.CallRecordingService));
            _serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (_serviceHost != null)
            {
                _serviceHost.Close();
                _serviceHost = null;
            }
        }
    }
}
