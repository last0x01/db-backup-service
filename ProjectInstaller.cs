using System.ComponentModel;
using System.ServiceProcess;

namespace DatabaseBackUpService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller _ServiceProcessInstaller;
        private ServiceInstaller _ServiceInstaller;

        public ProjectInstaller()
        {
            InitializeComponent();


            _ServiceProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };


            _ServiceInstaller = new ServiceInstaller
            {
                ServiceName = "DatabaseBackUpService",
                DisplayName = "Database BackUp Service",
                Description = "Service to perform automated SQL Server database backups at scheduled intervals.",
                StartType = ServiceStartMode.Automatic,
                ServicesDependedOn = new[]
                {
                    "MSSQLSERVER",
                    "RpcSs",
                    "EventLog"
                }
            };


            Installers.Add(_ServiceProcessInstaller);
            Installers.Add(_ServiceInstaller);
        }
    }
}
