# 🚀 LycheeVMS — Native High-Performance Milestone XProtect VMS (2025 R3) in Pure Rust

LycheeVMS is the native, pure Rust re-architecture of the entire **Milestone XProtect VMS 2025 R3** platform (211 C# / C++ projects decompiled and analyzed from source).

---

## 🏗️ Deep Architectural Breakdown (From Decompiled Source & Core Binaries)

Based on exhaustive analysis of `Decompiled_Source/`, `src/`, and `Milestone_Core_Binaries/`, Milestone XProtect VMS is organized into 15 foundational domains:

```
src/ / Decompiled_Source/
├── 🎥 Platform/             (VideoOS.Platform, VideoOS.Platform.SDK, VideoOS.Toolkit)
├── ⚙️ Management/           (VideoOS.ConfigurationAPI, VideoOS.Management.Server, VideoOS.Management.VmoClient)
├── 🚨 EventServer/          (VideoOS.EventServer.Communication, VideoOS.ServiceHost.EventServer)
├── 🗄️ Database/             (VideoOS.Database, VideoOS.DatabaseUtility.MediaStorage)
├── 🔐 Identity/             (VideoOS.IdentityServer, VideoOS.CertificateRegistration.Common)
├── 📱 Mobile/               (VideoOS.Mobile, VideoOS.Mobile.SDK.Portable, VideoOS.Mobile.MP4Muxing)
├── 🏷️ IncidentManager/      (VideoOS.IM.Server.WebApi, VideoOS.IM.Server.Models)
├── 🚗 LPR/                  (VideoOS.LPR.LogService, VideoOS.LPR.EventMatching)
├── 📜 RulesEngine/          (VideoOS.Rules, VideoOS.Rules.Adapters)
├── 📊 Reporting/            (VideoOS.Reporting.Service, VideoOS.Reporting.Common)
├── 🖥️ ServerCore/           (VideoOS.Server, VideoOS.Server.Service.TrayController)
├── 🎛️ SCH/                  (VideoOS.SCH.ConfigurationHost, VideoOS.SCH.DataAccess)
├── 🎨 UI/                   (VideoOS.UI.Common.WPF, VideoOS.Common.Controls)
├── 🛠️ Installer/            (Milestone.SystemInstaller, VideoOS.Installer.* plugins - 57 projects)
└── 📦 Common/               (VideoOS.Common, VideoOS.Vms.Types, Telemetry & Crypto - 48 projects)
```

---

## 🧩 Key Subsystems Re-Engineered into Pure Rust

### 1. 🎥 Core Platform & MIP SDK (`lychee_platform`)
- **Decompiled Origin**: `src/Platform/VideoOS.Platform` & `VideoOS.Platform.SDK`
- **FQID Identity System**: Re-implemented `FQID` (Fully Qualified ID: `ObjectId`, `Kind`, `ServerId`, `ParentId`) with 100% binary compatibility for Milestone GUIDs.
- **Kind Hierarchy**: Standardized 30+ GUID definitions (`ManagementServer`, `RecordingServer`, `Hardware`, `Camera`, `Microphone`, `Speaker`, `Metadata`, `Input`, `Output`, `SmartWall`, `Matrix`, `ViewGroup`, `Rule`, `Alarms`, `AccessControlUnits`).
- **Environment & License Engine**: Native implementation of `EnvironmentManager`, `SystemLicense`, `GracePeriod`, and MIP setting token store.

### 2. ⚙️ Management Server & Configuration API (`lychee_management`)
- **Decompiled Origin**: `src/Management/VideoOS.ConfigurationAPI` & `VideoOS.Management.VmoClient`
- **Configuration Service**: Native implementation of `IConfigurationService` (`GetItem`, `GetItems`, `SetItem`, `ConstructItem`, `ConstructChildItem`, `ValidateItem`, `DeleteItem`, `InvokeMethod`).
- **VMO Client & Repositories**: Object-Relational Model for `ManagementServer`, `RecordingServer`, `Hardware`, `CameraDevice`, `DeviceGroup`, `HardwareDriver`, `TimeProfile`, `UserDefinedEvent`, `PatrollingProfile`, `Role`, `BasicUser`.
- **Hardware Drivers & Streaming Profiles**: Universal ONVIF & `HikVisionGeneric` driver profiles (`General`, `Video stream 1`, `Video stream 2`, RTSP/RTP/TCP).

### 3. 🚨 Event Server & Alarm Engine (`lychee_events`)
- **Decompiled Origin**: `src/EventServer/VideoOS.EventServer.Communication` & `VideoOS.ServiceHost.EventServer`
- **Real-Time Pub/Sub**: High-performance multi-threaded event bus managing Alarm states (New, InProgress, Acknowledged, Closed), Priority (1–5), and automated rule triggers.

### 4. 🔐 Identity & Token Security (`lychee_identity`)
- **Decompiled Origin**: `src/Identity/VideoOS.IdentityServer`
- **OAuth 2.0 / OpenID Connect**: Native token exchange, JWT bearer authentication, Claims principal validation, and Windows Negotiate / Basic User credential verification.

### 5. 📱 Mobile Gateway & Streaming Bridge (`lychee_mobile`)
- **Decompiled Origin**: `src/Mobile/VideoOS.Mobile` & `VideoOS.Mobile.SDK.Portable`
- **WebRTC & HLS Transcoder**: Real-time H.264/H.265 stream repackaging and low-latency mobile streaming.

### 6. 🚗 AI Analytics, LPR & Metadata (`lychee_analytics`)
- **Decompiled Origin**: `src/LPR/` & `src/IncidentManager/`
- **ANPR Engine**: High-speed OCR and license plate matching pipeline with confidence scoring and vehicle classification.

---

## 📦 Binary Compilation & Running

The standalone desktop executable is fully built with Link-Time Optimization (LTO):

```powershell
# Build optimized release executable
cargo build --release --manifest-path ".\LycheeVMS\Desktop\Cargo.toml"

# Run Standalone Management Client
.\LycheeVMS\MilestoneXProtect-ManagementClient.exe
```
