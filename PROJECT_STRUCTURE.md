# Milestone XProtect VMS 2025 R3 — Folder-Wise Project Structure

All source code has been structured **folder-wise** into the `./src/` directory. Each subsystem resides in its dedicated domain solution folder, and all C# types are organized into **nested namespace folders** (matching the original source tree).

---

## 📁 Solution Architecture (`./src/`)

```
src/
├── Platform/            # Core MIP SDK, Live Streaming, Decoders & Interops (15 Projects)
├── Management/          # Configuration API, VMO Client, Management Server (6 Projects)
├── EventServer/         # Event Server, Alarm Management & REST APIs (7 Projects)
├── Database/            # Media Storage, SQL Schemas & Update Handlers (5 Projects)
├── Identity/            # Identity Server, OAuth 2.0 & Certificate Management (18 Projects)
├── Mobile/              # Mobile Server, MP4 Muxer, Streaming Transcoder (12 Projects)
├── IncidentManager/     # Incident Manager WebAPI, Models & Repositories (13 Projects)
├── LPR/                 # License Plate Recognition / ANPR Subsystems (3 Projects)
├── RulesEngine/         # Milestone Automation & Event Rules Engine (3 Projects)
├── Reporting/           # Web Reporting Services & Plugins (2 Projects)
├── ServerCore/          # Milestone Core Services, Tray Controller & Backup (8 Projects)
├── SCH/                 # Service Configuration Host & Data Access (8 Projects)
├── UI/                  # WPF Shared Controls, Themes & Custom Views (6 Projects)
├── Installer/           # Setup Bootstrapper UI & Role Plugins (57 Projects)
└── Common/              # Shared Utilities, Telemetry, Crypto & WebSockets (48 Projects)
```

---

## 📑 Domain Project Breakdown

### 1. 🎥 `src/Platform/` (Core Platform & MIP SDK)
- [`VideoOS.Platform`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Platform/VideoOS.Platform) — Core MIP SDK, Environment, Configuration, and Item models.
- [`VideoOS.Platform.SDK`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Platform/VideoOS.Platform.SDK) — SDK Client proxy and high-level wrappers.
- [`VideoOS.Toolkit`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Platform/VideoOS.Toolkit) — Media toolkit, DirectX/GPU hardware acceleration wrappers.
- [`VideoOS.Platform.MediaProcessing`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Platform/VideoOS.Platform.MediaProcessing) — Image and video frame transformation pipeline.
- [`VideoOS.Platform.SDK.Export`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Platform/VideoOS.Platform.SDK.Export) — AVI, MKV, MP4, and Milestone Native export engines.

### 2. ⚙️ `src/Management/` (Management & Configuration)
- [`VideoOS.ConfigurationAPI`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Management/VideoOS.ConfigurationAPI) — Strong-typed WCF configuration contract clients.
- [`VideoOS.Management.Server`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Management/VideoOS.Management.Server) — Management Server business services and routing.
- [`VideoOS.Management.VmoClient`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Management/VideoOS.Management.VmoClient) — Video Management Object (VMO) client communications.

### 3. 🚨 `src/EventServer/` (Event & Alarm Services)
- [`VideoOS.EventServer.Communication`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/EventServer/VideoOS.EventServer.Communication) — Real-time event subscription and notification engine.
- [`VideoOS.ServiceHost.EventServer`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/EventServer/VideoOS.ServiceHost.EventServer) — Standalone Event Server Windows service host.
- [`VideoOS.EventServer.Common.RestApi.Client`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/EventServer/VideoOS.EventServer.Common.RestApi.Client) — RESTful client for external integrations.

### 4. 🗄️ `src/Database/` (Database & Storage Management)
- [`VideoOS.Database`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Database/VideoOS.Database) — SQL Server data access layer, schemas, and migrations.
- [`VideoOS.DatabaseUtility.MediaStorage`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Database/VideoOS.DatabaseUtility.MediaStorage) — Archive tiering, grooming, and storage management.

### 5. 🔐 `src/Identity/` (Identity & Security)
- [`VideoOS.IdentityServer`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Identity/VideoOS.IdentityServer) — OpenID Connect / OAuth 2.0 authentication service.
- [`VideoOS.CertificateRegistration.Common`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Identity/VideoOS.CertificateRegistration.Common) — TLS/SSL certificate trust and server binding.

### 6. 📱 `src/Mobile/` (Mobile Server & Transcoding)
- [`VideoOS.Mobile`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Mobile/VideoOS.Mobile) — Mobile Server streaming gateway and WebRTC bridge.
- [`VideoOS.Mobile.SDK.Portable`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Mobile/VideoOS.Mobile.SDK.Portable) — Cross-platform client SDK for iOS/Android/Web.

### 7. 🏷️ `src/IncidentManager/` & `src/LPR/` (Analytics & Management)
- [`VideoOS.IM.Server.WebApi`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/IncidentManager/VideoOS.IM.Server.WebApi) — Incident logging and project export API.
- [`VideoOS.LPR.LogService`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/LPR/VideoOS.LPR.LogService) — License Plate Recognition log and event matching engine.

### 8. 🛠️ `src/Installer/` (Installer Framework & Plugins)
- [`Milestone.SystemInstaller`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Installer/Milestone.SystemInstaller) — Main WPF installer bootstrapper and command line executor.
- [`VideoOS.Installer.RecordingServerPlugin`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Installer/VideoOS.Installer.RecordingServerPlugin) — Recording Server setup engine.
- [`VideoOS.Installer.ManagementServerPlugin`](file:///c:/Program%20Files%20%28x86%29/i-Comply/Milestone/Milestone%20XProtect%20VMS%20Products%202025%20R3%20System%20Installer/src/Installer/VideoOS.Installer.ManagementServerPlugin) — Management Server setup engine.
