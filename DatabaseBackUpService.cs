using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace DatabaseBackUpService
{
    public partial class DatabaseBackUpService : ServiceBase
    {
        string ConnectionString;
        string BackupFolder;
        string LogFolder;
        int BackupIntervalMinutes;
        Timer BackupTimer;
        public DatabaseBackUpService()
        {
            InitializeComponent();

            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            BackupFolder = ConfigurationManager.AppSettings["BackupFolder"];
            LogFolder = ConfigurationManager.AppSettings["LogFolder"];
            BackupIntervalMinutes = int.TryParse(ConfigurationManager.AppSettings["BackupIntervalMinutes"], out int Interval) ? Interval : 60;
        }

        private void SetNewDatabaseBackup(object state)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_FullBackupDatabase", connection))
                    {

                        if (!Directory.Exists(BackupFolder))
                            Directory.CreateDirectory(BackupFolder);

                        command.Parameters.AddWithValue("@BackupLocation", BackupFolder);

                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter outputParam = new SqlParameter("@SourceFilePath", SqlDbType.VarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        connection.Open();

                        command.ExecuteNonQuery();

                        string backupFilePath = outputParam.Value.ToString();

                        WriteLog($"Database Backup Created Successfully {backupFilePath}");
                    }

                }
            }
            catch (Exception ex)
            {
                WriteLog("Error occurred during backup: " + ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            WriteLog("Started Service.");

            BackupTimer = new Timer(
                state => SetNewDatabaseBackup(state),
                null,
                0,
                BackupIntervalMinutes * 60 * 1000
            );

            WriteLog($"Backup schedule Initiated: every {BackupIntervalMinutes} minute(s).");
        }

        protected override void OnStop()
        {
            BackupTimer?.Dispose();
            WriteLog("Stopped Service.");
        }

        private void WriteLog(string Message)
        {

            if (!Directory.Exists(LogFolder))
                Directory.CreateDirectory(LogFolder);

            string LogMessage = $"[{DateTime.Now:yyyy-MM-dd HH-mm-ss}] {Message}\n";

            File.AppendAllText(Path.Combine(LogFolder, "Logs.txt"), LogMessage);

            if (Environment.UserInteractive)
            {
                Console.WriteLine(LogMessage);
            }

        }

        public void StartInConsole()
        {
            Console.WriteLine("Start-Pending...");
            OnStart(null);
            Console.ReadLine();

            Console.WriteLine("Stop-Pending...");
            OnStop();
            Console.ReadKey();
        }
    }
}
