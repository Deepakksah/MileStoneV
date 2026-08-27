# 🖥️ LycheeVMS Desktop — Pure Rust Management & Smart Client Studio

> **Native, Hardware-Accelerated Desktop VMS Management Client matching Milestone XProtect 2025 R2**  
> *Built with Pure Rust (`eframe` / `egui`), Zero-Allocation UI Engine, and Direct MS-SQL Integration*

---

## 🏛️ Complete Desktop Architectural Blueprint

The Desktop Client is organized into a clean, modular, multi-file architecture with strict separation of concerns between state management, navigation hierarchy, toolbar actions, and dedicated domain view pages.

```
LycheeVMS/Desktop/
├── Cargo.toml                  # Native Rust package manifest & domain crate dependencies
├── README.md                   # 📑 Complete Desktop Client Architecture & Manual
├── run.bat                     # 🚀 1-Click launcher setting MinGW PATH and starting binary
│
└── src/
    ├── main.rs                 # ⚡ Entry Point (< 60 lines) orchestrating rendering lifecycle
    ├── state.rs                # 🧠 Central Application State, Navigation Enums, and Wizard Handlers
    │
    └── ui/                     # 🎨 UI Subsystem Architecture
        ├── mod.rs              # Re-exports all UI submodules
        ├── menu_bar.rs         # 🗄️ Top Menu Bar (File, View, Action, Maintenance, Tools, Help)
        ├── toolbar.rs          # 💾 Icon Toolbar (Save, Refresh F5, Undo/Redo, Live DB Status Pill)
        ├── tree_nav.rs         # 🌲 Left Site Navigation Tree with 10 Rich Colored Categories
        │
        └── views/              # 📑 Har Feature / Subsystem ka Dedicated Page
            ├── mod.rs          # View Exports
            ├── license_view.rs # 📄 Milestone Care Plus, Product SLC, Seat Allocations (399/400)
            ├── site_view.rs    # 🌐 Site Definition (MXM-PS-01), Port 443, Surveillance DB Status
            ├── servers_view.rs # 🖧 3-Column Split + Bottom Multi-Camera Live Preview Strip
            ├── cameras_view.rs # 📹 RTSP Camera Stream Registry + Add Camera Wizard Modal
            ├── security_view.rs# 👥 Security Roles (Administrators, Operators) & Basic Users
            ├── alarms_view.rs  # 🚨 Real-time Alarm Definitions & Trigger Rules
            ├── logs_view.rs    # 📜 System Audit Trail & Real-time Event Logs
            ├── mobile_view.rs  # 📱 Mobile Gateway & Low-Latency WebRTC Streaming Sessions
            └── analytics_view.rs# 🔍 AI License Plate Recognition (ANPR) & Incident Reports
```

---

## 🔗 Integrated Rust Backend Domain Crates

The Desktop GUI seamlessly interfaces with 8 specialized backend crates located in `../crates/`:

| Backend Crate | Converted Milestone Projects | Responsibilities in Desktop GUI |
|---|---|---|
| **`lychee_database`** | `VideoOS.Database`, `VideoOS.SqlUpdate` | Live queries to MS-SQL `Surveillance` database on `localhost` |
| **`lychee_common`** | `VideoOS.Common`, `VideoOS.Vms.Types` | Core GUID types, Device definitions, Site specifications, `VmsError` |
| **`lychee_identity`** | `VideoOS.IdentityServer`, `VideoOS.Common.Login` | Basic User authentication, PBKDF2 hashing, JWT tokens, Roles |
| **`lychee_management`**| `VideoOS.ConfigurationAPI`, `VideoOS.Management.*`| Hardware discovery, Driver profiles, Camera CRUD, Recording Server nodes |
| **`lychee_platform`**  | `VideoOS.Platform`, `VideoOS.Toolkit` | RTSP Frame Ingest, 90-Day Media Retention, MP4/MKV video recording engine |
| **`lychee_events`**    | `VideoOS.EventServer.*`, `VideoOS.Rules.*` | Event broker, Alarm rule trigger pipeline, Server logs audit trail |
| **`lychee_mobile`**    | `VideoOS.Mobile`, `VideoOS.Mobile.SDK` | Mobile Server Gateway (Port 8081), WebRTC / HLS streaming sessions |
| **`lychee_analytics`** | `VideoOS.LPR.*`, `VideoOS.IM.*` | ANPR / LPR AI Plate Engine (`OD-02-AK-9921`, `DL-10-CA-4028`), Incident Reports |

---

## 🎨 UI Design & Milestone Layout Compatibility

### 1. **Recording Server 3-Pane + Bottom Preview Layout**
- **Middle Pane**: `Recording Server` Hierarchy with search filter (`Filter your devices`), `☑ Hide disabled devices` checkbox, and the expandable camera tree matching live plant cameras (`IPCamera 10.40.56.203`, `AIRPORT HIGHWAY`, `Alcon EXCHANGE_ENTRANCE`, `ATC FRONT PTZ`, etc.).
- **Right Pane**: `Properties` Panel displaying `Name`, `Description`, `Host name`, `Local web server address`, `Time zone`, and bottom navigation tabs (`ℹ Info`, `🗄 Storage`, `🛡️ Failover`, `🖧 Multicast`, `🌐 Network`).
- **Bottom Strip**: `Preview` multi-camera live video tiles with resolution headers (`Live 1920x1080 15fps`), perspective grid lines, red `REC` dots, and camera name tags (*AIRPORT LANDI, DATA CENTER E, MAIN GATE, SERVER ROOM, WEIGH BRIDGE 2*).

### 2. **Interactive Wizards & Dialogs**
- **➕ Add Camera / Hardware Wizard**: Register new ONVIF / RTSP streams with resolution (4K UHD / 1080p / 720p), FPS slider (10–60), and Codec selection (H.265 / H.264).
- **➕ Add Recording Server Wizard**: Connect distributed storage nodes with dedicated TB capacity.

---

## 🛠️ Build & Run Commands

### 🏃 Quick Launch (Debug Mode)
```cmd
run.bat
```
*Or via cargo:*
```powershell
$env:PATH = "C:\Users\ICO-IND-DEEPAKSAH\AppData\Local\Microsoft\WinGet\Packages\BrechtSanders.WinLibs.POSIX.MSVCRT_Microsoft.Winget.Source_8wekyb3d8bbwe\mingw64\bin;$env:PATH"
cargo run --manifest-path ".\LycheeVMS\Desktop\Cargo.toml"
```

### ⚡ Optimized Standalone Production Release Build
```powershell
$env:PATH = "C:\Users\ICO-IND-DEEPAKSAH\AppData\Local\Microsoft\WinGet\Packages\BrechtSanders.WinLibs.POSIX.MSVCRT_Microsoft.Winget.Source_8wekyb3d8bbwe\mingw64\bin;$env:PATH"
cargo build --release --manifest-path ".\LycheeVMS\Desktop\Cargo.toml"
```
**Compiled Binary Output:**
`LycheeVMS/Desktop/target/release/lychee-vms.exe` (Standalone executable, ~8-15 MB, zero .NET dependencies).
