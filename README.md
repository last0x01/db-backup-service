# Database Backup Service

![.NET](https://img.shields.io/badge/.NET%20Framework-4.7%2B-blue)
![C#](https://img.shields.io/badge/language-C%23-178600?logo=csharp&logoColor=white)
![Windows Service](https://img.shields.io/badge/Windows%20Service-Ready-green)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)

An automated **Windows Service** that performs **scheduled full backups** of a SQL Server database and writes them to a configurable folder, with timestamped file naming and built-in logging.

---

## Overview

- Runs as a native Windows Service (or in **console mode** for debugging)
- Takes a **full database backup** using the stored procedure `SP_FullBackupDatabase`
- Backup runs on a **configurable time interval** (minutes)
- Creates the backup folder automatically if it does not exist
- Backup files are named `Backup_yyyyMMdd_HHmmss.bak`
- Logs every operation to `Logs.txt` in a configurable log folder
- Fully configurable through `App.config`

---

## Project Structure

```
DatabaseBackUpService.sln
├── Program.cs                      # Service entry point (console vs. service mode)
├── DatabaseBackUpService.cs        # Core backup logic + timer scheduling
├── ProjectInstaller.cs             # Windows Service installer metadata
├── StoredProcedure/
│   └── SP_FullBackupDatabase.sql   # Backup stored procedure (deploy to target DB)
├── App.config                      # Connection string + folder/interval settings
└── Properties/
```

---

## Requirements

- Windows 10 / 11 or Windows Server
- .NET Framework 4.7+
- SQL Server (local or remote, with backup permissions)
- Admin privileges to create/manage the Windows Service
- The service account needs **write access** to the backup and log folders

---

## Configuration (`App.config`)

| Key                     | Description                                              | Example                                  |
| ----------------------- | -------------------------------------------------------- | ---------------------------------------- |
| `ConnectionString`      | SQL Server connection string for the target database     | `Server=YOUR_SERVER;Database=YOUR_DB;Trusted_Connection=True;` |
| `BackupFolder`          | Where `.bak` files are written                           | `C:\DatabaseBackups\`                    |
| `LogFolder`             | Where `Logs.txt` is written                              | `C:\DatabaseBackups\Logs\`               |
| `BackupIntervalMinutes` | Backup frequency in minutes (defaults to `60`)           | `1`                                      |

```xml
<appSettings>
  <add key="ConnectionString" value="Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;" />
  <add key="BackupFolder" value="C:\DatabaseBackups\" />
  <add key="LogFolder" value="C:\DatabaseBackups\Logs\" />
  <add key="BackupIntervalMinutes" value="1" />
</appSettings>
```

---

## Setup

### 1. Deploy the stored procedure

Run `StoredProcedure/SP_FullBackupDatabase.sql` on the target database **before** starting the service. The service invokes this stored procedure on every scheduled run.

### 2. Build & install the service

Build the solution in Visual Studio, then register it as a Windows Service:

```cmd
sc create DatabaseBackUpService binPath= "C:\Path\To\DatabaseBackUpService.exe" start= auto
```

### 3. Manage the service

```cmd
sc start   DatabaseBackUpService   // Start
sc stop    DatabaseBackUpService   // Stop
sc delete  DatabaseBackUpService   // Remove
```

### 4. Run in console mode (debugging)

Running the `.exe` directly starts it interactively — useful for testing without installer/admin setup:

```cmd
DatabaseBackUpService.exe
```

---

## Logging

Sample output written to `Logs.txt`:

```text
[2026-01-16 16:00:00] Service Started.
[2026-01-16 16:00:01] Backup schedule Initiated: every 1 minute(s).
[2026-01-16 16:01:01] Database Backup Created Successfully C:\DatabaseBackups\Backup_20260116_160101.bak
[2026-01-16 16:02:01] Database Backup Created Successfully C:\DatabaseBackups\Backup_20260116_160201.bak
[2026-01-16 16:05:00] Service Stopped.
```

Backup file naming convention: `Backup_yyyyMMdd_HHmmss.bak`