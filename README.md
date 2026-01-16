# 🗄️ Database Backup Service

[![.NET](https://img.shields.io/badge/.NET-4.7+-blue)](https://dotnet.microsoft.com/)
[![Windows Service](https://img.shields.io/badge/Windows%20Service-Ready-green)]()

Automated Windows Service for performing scheduled full backups of a SQL Server database.

---

## 📌 Overview
- Performs **full database backups** using SQL Server  
- Executes backup via **stored procedure**  
- Runs on a **configurable time interval**  
- Saves backup files to a **backup folder**  
- Logs all operations to a **log folder**  
- Supports **console mode** for debugging  
- Configurable via `App.config`  

---

## 📝 Requirements
- Windows 10 / 11 or Server editions  
- .NET Framework 4.7+  
- SQL Server (Local or Remote)  
- Admin privileges to create/manage Windows Service  
- Service account with **write access** to backup and log folders  

---

## ⚙ Configuration (`App.config`)
```xml
<appSettings>
  <add key="ConnectionString" value="Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;" />
  <add key="BackupFolder" value="C:\DatabaseBackups\" />
  <add key="LogFolder" value="C:\DatabaseBackups\Logs\" />
  <add key="BackupIntervalMinutes" value="1" />
</appSettings>
```

## 🚀 Installation & Commands

### Create Service
```cmd
sc create DatabaseBackUpService binPath= "C:\Path\To\DatabaseBackUpService.exe" start= auto
```

### Start Service
```cmd
sc start DatabaseBackUpService
```

### Stop Service
```cmd
sc stop DatabaseBackUpService
```

### Delete Service
```cmd
sc delete DatabaseBackUpService
```

## 📄 Logging

### Sample output:
```text
[2026-01-16 16:00:00] Service Started.
[2026-01-16 16:00:01] Backup schedule Initiated: every 1 minute(s).
[2026-01-16 16:01:01] Database Backup Created Successfully C:\DatabaseBackups\Backup_20260116_160101.bak
[2026-01-16 16:02:01] Database Backup Created Successfully C:\DatabaseBackups\Backup_20260116_160201.bak
[2026-01-16 16:03:01] Database Backup Created Successfully C:\DatabaseBackups\Backup_20260116_160301.bak
[2026-01-16 16:04:01] Database Backup Created Successfully C:\DatabaseBackups\Backup_20260116_160401.bak
[2026-01-16 16:05:00] Service Stopped.

```

## 🗃️ Backup Details

### File naming format:
```text
Backup_yyyyMMdd_HHmmss.bak
```
