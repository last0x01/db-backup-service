# Database BackUp Service

Automated Windows Service for performing SQL Server database backups at scheduled intervals.

---

## 📌 Overview

`DatabaseBackUpService` is a Windows Service written in C# that automatically performs **full backups** of a SQL Server database. The service supports:

- Scheduled backups at configurable intervals
- Logging of backup activities to a local folder
- Output of backup file path via stored procedure
- Windows Service with dependencies on SQL Server, RPC, and Event Log

---

## ⚙ Features

- Full database backup using SQL Server stored procedure (`SP_FullBackupDatabase`)
- Configurable backup folder and interval via `App.config`
- Automatic logging to `Logs.txt`
- Output parameter `@SourceFilePath` contains full backup path
- Dependencies: `MSSQLSERVER`, `RpcSs`, `EventLog`

---

## 📝 Requirements

- Windows 10 / 11 or Server editions
- .NET Framework 4.7+
- SQL Server instance (local or network)
- Service account with write access to backup folder
- Admin privileges to create Windows Service

---

## ⚙ Installation Using SC

```cmd
sc create DatabaseBackUpService binPath= "C:\Path\To\Build\DatabaseBackUpService.exe" start= auto 
sc start DatabaseBackUpService
