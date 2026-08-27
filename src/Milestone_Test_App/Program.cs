using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Milestone_Test_App
{
    public class App : Application
    {
        [STAThread]
        public static void Main()
        {
            App app = new App();
            LycheeMainWindow mainWin = new LycheeMainWindow("lychee-cluster.local", "Administrator");
            app.Run(mainWin);
        }
    }

    #region --- LYCHEE VMS DATA MODELS & STORE ---

    public class LycheeConfigStore
    {
        public string SiteName { get; set; } = "LycheeVMS Global Site — Cluster 01";
        public string License { get; set; } = "LycheeVMS Enterprise 2025 (Unlimited Video Channels)";
        public List<LycheeServerNode> Servers { get; set; } = new List<LycheeServerNode>();

        public static string ConfigPath => System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            "LycheeVMS_Config.json");

        public static LycheeConfigStore Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    string json = File.ReadAllText(ConfigPath);
                    var cfg = JsonSerializer.Deserialize<LycheeConfigStore>(json);
                    if (cfg != null && cfg.Servers.Count > 0) return cfg;
                }
            }
            catch { }

            var defaultStore = new LycheeConfigStore();
            var srv = new LycheeServerNode { Name = "LYCHEE-REC-SRV-01", IpAddress = "192.168.1.100", Port = "7563", Status = "Online (Active)" };
            
            var cam1 = new LycheeCameraNode
            {
                Name = "LYCHEE MAIN ENTRANCE CAM 01",
                IpAddress = "192.168.1.201",
                Driver = "HikVisionGeneric",
                Codec = "H.265",
                Resolution = "1920x1080 (FHD)",
                Fps = 25,
                Bitrate = 4096,
                RtspUrl = "rtsp://192.168.1.201:554/live/ch0"
            };

            var cam2 = new LycheeCameraNode
            {
                Name = "LYCHEE PERIMETER PTZ CAM 02",
                IpAddress = "192.168.1.202",
                Driver = "ONVIF Generic Profile S/G/T",
                Codec = "H.265+",
                Resolution = "3840x2160 (4K UHD)",
                Fps = 30,
                Bitrate = 8192,
                RtspUrl = "rtsp://192.168.1.202:554/stream1"
            };

            srv.Cameras.Add(cam1);
            srv.Cameras.Add(cam2);
            defaultStore.Servers.Add(srv);
            return defaultStore;
        }

        public void Save()
        {
            try
            {
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigPath, json);
            }
            catch { }
        }
    }

    public class LycheeServerNode
    {
        public string Name { get; set; } = "LYCHEE RECORDING SERVER";
        public string IpAddress { get; set; } = "127.0.0.1";
        public string Port { get; set; } = "7563";
        public string Status { get; set; } = "Online";
        public List<LycheeCameraNode> Cameras { get; set; } = new List<LycheeCameraNode>();
    }

    public class LycheeCameraNode
    {
        public string Name { get; set; } = "New Lychee Camera";
        public string IpAddress { get; set; } = "192.168.1.10";
        public string Driver { get; set; } = "ONVIF Generic";
        public string Codec { get; set; } = "H.265";
        public string Resolution { get; set; } = "1920x1080";
        public int Fps { get; set; } = 25;
        public int Bitrate { get; set; } = 4096;
        public string RtspUrl { get; set; } = "rtsp://192.168.1.10:554/live";
        public int Brightness { get; set; } = 50;
        public int Contrast { get; set; } = 50;
        public int Saturation { get; set; } = 50;
        public int Sharpness { get; set; } = 50;
        public bool MotionDetection { get; set; } = true;
    }

    #endregion

    #region --- MAIN LYCHEE VMS MANAGEMENT CLIENT WORKSPACE ---

    public class LycheeMainWindow : Window
    {
        // Lychee Theme Color Palette (Crimson Red, Rose Coral, Deep Wine & Soft Pulp)
        private static readonly Color LycheePrimary = Color.FromRgb(225, 29, 72);    // #E11D48 (Vibrant Lychee Red)
        private static readonly Color LycheeDark = Color.FromRgb(159, 18, 57);       // #9F1239 (Deep Lychee Shell)
        private static readonly Color LycheeRose = Color.FromRgb(244, 63, 94);       // #F43F5E (Bright Lychee Rose)
        private static readonly Color LycheeSoftPink = Color.FromRgb(255, 228, 230); // #FFE4E6 (Lychee Pulp Pink)
        private static readonly Color DarkSlate = Color.FromRgb(15, 23, 42);         // #0F172A (Deep Slate BG)
        private static readonly Color PanelSlate = Color.FromRgb(30, 41, 59);        // #1E293B (Panel BG)

        private readonly LycheeConfigStore _store;
        private readonly string _server;
        private readonly string _user;

        private TreeView _deviceTree = new TreeView();
        private StackPanel _propertiesPanel = new StackPanel();
        private TextBlock _cameraTitleLabel = new TextBlock();
        private TextBlock _streamFpsLabel = new TextBlock();
        private Canvas _videoCanvas = new Canvas();
        private Rectangle _motionBox = new Rectangle();
        private DispatcherTimer _videoSimTimer = new DispatcherTimer();
        private double _motionX = 120, _motionY = 90, _dx = 4.0, _dy = 3.0;

        private LycheeCameraNode _selectedCamera;

        public LycheeMainWindow(string server, string user)
        {
            _server = server;
            _user = user;
            _store = LycheeConfigStore.Load();

            Title = $"LycheeVMS — Intelligent Video Management Client 2025 • [{server}]";
            Width = 1500;
            Height = 940;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Background = new SolidColorBrush(Color.FromRgb(248, 250, 252));

            DockPanel mainDock = new DockPanel();

            // Top Header & Menu
            UIElement headerBar = CreateLycheeHeader();
            DockPanel.SetDock(headerBar, Dock.Top);
            mainDock.Children.Add(headerBar);

            ToolBarTray toolTray = CreateLycheeToolBar();
            DockPanel.SetDock(toolTray, Dock.Top);
            mainDock.Children.Add(toolTray);

            // Bottom Status Bar
            StatusBar statusBar = CreateLycheeStatusBar();
            DockPanel.SetDock(statusBar, Dock.Bottom);
            mainDock.Children.Add(statusBar);

            // Main 3-Column Split
            Grid mainWorkspace = new Grid { Margin = new Thickness(6) };
            mainWorkspace.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(270), MinWidth = 220 }); // Left Site Nav
            mainWorkspace.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5) });
            mainWorkspace.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // 1. Site Nav
            UIElement siteNav = CreateSiteNavigationPane();
            Grid.SetColumn(siteNav, 0);
            mainWorkspace.Children.Add(siteNav);

            GridSplitter split1 = new GridSplitter { Width = 5, HorizontalAlignment = HorizontalAlignment.Center, Background = new SolidColorBrush(Color.FromRgb(226, 232, 240)) };
            Grid.SetColumn(split1, 1);
            mainWorkspace.Children.Add(split1);

            // 2. Right Content Area (Top: Devices + Properties, Bottom: Video Preview)
            Grid contentArea = new Grid();
            contentArea.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3.2, GridUnitType.Star) });
            contentArea.RowDefinitions.Add(new RowDefinition { Height = new GridLength(5) });
            contentArea.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2.2, GridUnitType.Star) });

            Grid topGrid = new Grid();
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.15, GridUnitType.Star) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            UIElement deviceExplorer = CreateDeviceExplorerPane();
            Grid.SetColumn(deviceExplorer, 0);
            topGrid.Children.Add(deviceExplorer);

            GridSplitter splitTop = new GridSplitter { Width = 5, HorizontalAlignment = HorizontalAlignment.Center, Background = new SolidColorBrush(Color.FromRgb(226, 232, 240)) };
            Grid.SetColumn(splitTop, 1);
            topGrid.Children.Add(splitTop);

            UIElement propPane = CreatePropertiesPane();
            Grid.SetColumn(propPane, 2);
            topGrid.Children.Add(propPane);

            Grid.SetRow(topGrid, 0);
            contentArea.Children.Add(topGrid);

            GridSplitter split2 = new GridSplitter { Height = 5, HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Center, Background = new SolidColorBrush(Color.FromRgb(226, 232, 240)) };
            Grid.SetRow(split2, 1);
            contentArea.Children.Add(split2);

            UIElement previewPane = CreateVideoPreviewPane();
            Grid.SetRow(previewPane, 2);
            contentArea.Children.Add(previewPane);

            Grid.SetColumn(contentArea, 2);
            mainWorkspace.Children.Add(contentArea);

            mainDock.Children.Add(mainWorkspace);
            Content = mainDock;

            InitVideoSimulation();
        }

        #region --- LYCHEE HEADER, MENUS & TOOLBAR ---

        private UIElement CreateLycheeHeader()
        {
            Border header = new Border
            {
                Background = new LinearGradientBrush(LycheeDark, LycheePrimary, new Point(0, 0), new Point(1, 0)),
                Padding = new Thickness(16, 8, 16, 8)
            };

            DockPanel dp = new DockPanel();

            // Brand Logo + Title
            StackPanel brandPanel = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center };
            Border logoBadge = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(8, 4, 8, 4),
                Margin = new Thickness(0, 0, 12, 0)
            };
            TextBlock logoText = new TextBlock { Text = "🍒 LycheeVMS", FontSize = 16, FontWeight = FontWeights.ExtraBold, Foreground = new SolidColorBrush(LycheePrimary) };
            logoBadge.Child = logoText;
            brandPanel.Children.Add(logoBadge);

            TextBlock appSub = new TextBlock { Text = "Intelligent VMS Management Client 2025 R3", FontSize = 13, FontWeight = FontWeights.SemiBold, Foreground = new SolidColorBrush(Color.FromRgb(255, 228, 230)), VerticalAlignment = VerticalAlignment.Center };
            brandPanel.Children.Add(appSub);
            dp.Children.Add(brandPanel);

            // Right Info
            StackPanel rightInfo = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center };
            Border statusBadge = new Border
            {
                Background = new SolidColorBrush(Color.FromArgb(40, 255, 255, 255)),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(10, 4, 10, 4)
            };
            statusBadge.Child = new TextBlock { Text = "● Enterprise Cluster Online", FontSize = 11.5, FontWeight = FontWeights.Bold, Foreground = Brushes.White };
            rightInfo.Children.Add(statusBadge);
            dp.Children.Add(rightInfo);

            header.Child = dp;
            return header;
        }

        private ToolBarTray CreateLycheeToolBar()
        {
            ToolBarTray tray = new ToolBarTray { Background = new SolidColorBrush(Color.FromRgb(255, 241, 242)) };
            ToolBar bar = new ToolBar { Band = 0, BandIndex = 0, Background = Brushes.Transparent };

            Button saveBtn = CreateLycheeBtn("💾 Save Configuration", true);
            saveBtn.Click += (s, e) => SaveAndNotify();
            bar.Items.Add(saveBtn);

            Button refreshBtn = CreateLycheeBtn("🔄 Refresh Hardware", false);
            refreshBtn.Click += (s, e) => RefreshDeviceTree();
            bar.Items.Add(refreshBtn);

            bar.Items.Add(new Separator());

            Button addCamBtn = CreateLycheeBtn("➕ Add Lychee Camera", true);
            addCamBtn.Click += (s, e) => OpenAddCameraDialog();
            bar.Items.Add(addCamBtn);

            Button addSrvBtn = CreateLycheeBtn("🖥️ Add Recording Server", false);
            addSrvBtn.Click += (s, e) => OpenAddServerDialog();
            bar.Items.Add(addSrvBtn);

            tray.ToolBars.Add(bar);
            return tray;
        }

        private Button CreateLycheeBtn(string text, bool isHighlight)
        {
            Button btn = new Button
            {
                Content = text,
                FontSize = 11.5,
                FontWeight = isHighlight ? FontWeights.Bold : FontWeights.Normal,
                Margin = new Thickness(3, 2, 3, 2),
                Padding = new Thickness(10, 4, 10, 4),
                Background = isHighlight ? new SolidColorBrush(LycheePrimary) : Brushes.White,
                Foreground = isHighlight ? Brushes.White : new SolidColorBrush(LycheeDark),
                BorderBrush = new SolidColorBrush(Color.FromRgb(254, 205, 211)),
                BorderThickness = new Thickness(1),
                Cursor = Cursors.Hand
            };
            return btn;
        }

        private StatusBar CreateLycheeStatusBar()
        {
            StatusBar bar = new StatusBar { Height = 26, Background = new SolidColorBrush(Color.FromRgb(255, 228, 230)) };
            bar.Items.Add(new StatusBarItem { Content = $"🍒 LycheeVMS Active: {_server}" });
            bar.Items.Add(new Separator());
            bar.Items.Add(new StatusBarItem { Content = $"👤 {_user}" });
            bar.Items.Add(new Separator());
            bar.Items.Add(new StatusBarItem { Content = $"📹 Active Cameras: {_store.Servers.Sum(s => s.Cameras.Count)}" });
            bar.Items.Add(new Separator());
            bar.Items.Add(new StatusBarItem { Content = "Real-time AI Video Analytics Pipeline: [Running]" });
            return bar;
        }

        #endregion

        #region --- 1. SITE NAVIGATION PANE ---

        private UIElement CreateSiteNavigationPane()
        {
            Border border = new Border { Background = Brushes.White, BorderBrush = new SolidColorBrush(Color.FromRgb(254, 205, 211)), BorderThickness = new Thickness(1), CornerRadius = new CornerRadius(6) };
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(32) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            Border header = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 241, 242)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(254, 205, 211)),
                BorderThickness = new Thickness(0, 0, 0, 1),
                Padding = new Thickness(10, 6, 10, 6)
            };
            header.Child = new TextBlock { Text = "Site Navigation", FontWeight = FontWeights.Bold, FontSize = 12, Foreground = new SolidColorBrush(LycheeDark) };
            Grid.SetRow(header, 0);
            grid.Children.Add(header);

            TreeView navTree = new TreeView { BorderThickness = new Thickness(0), Padding = new Thickness(6) };

            TreeViewItem rootNode = CreateNavNode($"🌐 {_store.SiteName}", true);

            TreeViewItem basics = CreateNavNode("📁 Basics & Licenses", true);
            basics.Items.Add(CreateNavNode("📜 License Information"));
            basics.Items.Add(CreateNavNode("🏢 Site Info & Coordinates"));
            rootNode.Items.Add(basics);

            TreeViewItem servers = CreateNavNode("🖥️ Lychee Servers", true);
            TreeViewItem recServers = CreateNavNode("📹 Recording Servers", true);
            recServers.IsSelected = true;
            servers.Items.Add(recServers);
            servers.Items.Add(CreateNavNode("🛡️ Failover Hot-Standby"));
            servers.Items.Add(CreateNavNode("📱 Lychee Mobile Gateway"));
            rootNode.Items.Add(servers);

            TreeViewItem devices = CreateNavNode("🔌 Video & Audio Devices", true);
            devices.Items.Add(CreateNavNode("🎥 IP Cameras & Streams"));
            devices.Items.Add(CreateNavNode("🎙️ Audio Microphones"));
            devices.Items.Add(CreateNavNode("🔊 Intercom Speakers"));
            devices.Items.Add(CreateNavNode("🏷️ AI Object & Face Metadata"));
            rootNode.Items.Add(devices);

            TreeViewItem rules = CreateNavNode("⚡ Rules, Events & Alarms", false);
            rules.Items.Add(CreateNavNode("📋 AI Motion Recording Rule"));
            rules.Items.Add(CreateNavNode("⏰ 24x7 Continuous Archive"));
            rules.Items.Add(CreateNavNode("🔔 Push Alarm Alerts"));
            rootNode.Items.Add(rules);

            TreeViewItem sec = CreateNavNode("🔒 Security & Roles", false);
            sec.Items.Add(CreateNavNode("👥 Super Administrators"));
            sec.Items.Add(CreateNavNode("👤 Security Operators"));
            rootNode.Items.Add(sec);

            navTree.Items.Add(rootNode);
            Grid.SetRow(navTree, 1);
            grid.Children.Add(navTree);

            border.Child = grid;
            return border;
        }

        private TreeViewItem CreateNavNode(string text, bool isExpanded = false)
        {
            return new TreeViewItem { Header = text, FontSize = 11.5, IsExpanded = isExpanded, Padding = new Thickness(2) };
        }

        #endregion

        #region --- 2. RECORDING SERVER & DEVICE EXPLORER ---

        private UIElement CreateDeviceExplorerPane()
        {
            Border border = new Border { Background = Brushes.White, BorderBrush = new SolidColorBrush(Color.FromRgb(254, 205, 211)), BorderThickness = new Thickness(1), CornerRadius = new CornerRadius(6) };
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(32) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(72) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            Border header = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 241, 242)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(254, 205, 211)),
                BorderThickness = new Thickness(0, 0, 0, 1),
                Padding = new Thickness(10, 6, 10, 6)
            };
            header.Child = new TextBlock { Text = "Recording Servers & Cameras Explorer", FontWeight = FontWeights.Bold, FontSize = 12, Foreground = new SolidColorBrush(LycheeDark) };
            Grid.SetRow(header, 0);
            grid.Children.Add(header);

            StackPanel filterPanel = new StackPanel { Margin = new Thickness(8, 6, 8, 6) };
            StackPanel btnRow = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 6) };

            Button addCamQuick = CreateLycheeBtn("+ Add Camera", true);
            addCamQuick.Click += (s, e) => OpenAddCameraDialog();
            btnRow.Children.Add(addCamQuick);

            Button addSrvQuick = CreateLycheeBtn("+ Add Server", false);
            addSrvQuick.Click += (s, e) => OpenAddServerDialog();
            btnRow.Children.Add(addSrvQuick);

            Button deleteQuick = new Button
            {
                Content = "🗑️ Delete",
                FontSize = 11,
                Padding = new Thickness(8, 2, 8, 2),
                Background = Brushes.White,
                Foreground = Brushes.DarkRed,
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1),
                Cursor = Cursors.Hand
            };
            deleteQuick.Click += (s, e) => DeleteSelectedNode();
            btnRow.Children.Add(deleteQuick);

            filterPanel.Children.Add(btnRow);

            TextBox searchBox = new TextBox { Height = 24, Text = "Search cameras or IP...", Foreground = Brushes.Gray };
            searchBox.GotFocus += (s, e) => { if (searchBox.Text == "Search cameras or IP...") { searchBox.Text = ""; searchBox.Foreground = Brushes.Black; } };
            searchBox.TextChanged += (s, e) => FilterTree(searchBox.Text);
            filterPanel.Children.Add(searchBox);

            Grid.SetRow(filterPanel, 1);
            grid.Children.Add(filterPanel);

            _deviceTree = new TreeView { BorderThickness = new Thickness(0), Margin = new Thickness(6) };
            _deviceTree.SelectedItemChanged += (s, e) =>
            {
                if (_deviceTree.SelectedItem is TreeViewItem item && item.Tag is LycheeCameraNode cam)
                {
                    _selectedCamera = cam;
                    PopulateProperties(cam);
                    UpdatePreview(cam);
                }
            };

            RefreshDeviceTree();

            Grid.SetRow(_deviceTree, 2);
            grid.Children.Add(_deviceTree);

            border.Child = grid;
            return border;
        }

        private void RefreshDeviceTree()
        {
            _deviceTree.Items.Clear();
            TreeViewItem root = new TreeViewItem { Header = "📼 Lychee Recording Cluster", IsExpanded = true, FontWeight = FontWeights.Bold, FontSize = 12 };

            foreach (var srv in _store.Servers)
            {
                TreeViewItem srvItem = new TreeViewItem { Header = $"🖥️ {srv.Name} ({srv.IpAddress})", IsExpanded = true, FontWeight = FontWeights.Normal, FontSize = 11.5, Tag = srv };
                
                foreach (var cam in srv.Cameras)
                {
                    TreeViewItem camItem = new TreeViewItem
                    {
                        Header = $"🎥 {cam.Name} [{cam.IpAddress}]",
                        FontSize = 11.5,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(LycheeDark),
                        Tag = cam
                    };
                    srvItem.Items.Add(camItem);
                }
                root.Items.Add(srvItem);
            }

            _deviceTree.Items.Add(root);

            if (_selectedCamera == null && _store.Servers.Count > 0 && _store.Servers[0].Cameras.Count > 0)
            {
                _selectedCamera = _store.Servers[0].Cameras[0];
                PopulateProperties(_selectedCamera);
                UpdatePreview(_selectedCamera);
            }
        }

        private void FilterTree(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query == "Search cameras or IP...")
            {
                RefreshDeviceTree();
                return;
            }

            query = query.ToLower();
            _deviceTree.Items.Clear();
            TreeViewItem root = new TreeViewItem { Header = "📼 Recording Cluster (Filtered)", IsExpanded = true, FontWeight = FontWeights.Bold, FontSize = 12 };

            foreach (var srv in _store.Servers)
            {
                var matchingCams = srv.Cameras.Where(c => c.Name.ToLower().Contains(query) || c.IpAddress.Contains(query)).ToList();
                if (matchingCams.Count > 0 || srv.Name.ToLower().Contains(query))
                {
                    TreeViewItem srvItem = new TreeViewItem { Header = $"🖥️ {srv.Name} ({srv.IpAddress})", IsExpanded = true, Tag = srv };
                    foreach (var cam in matchingCams)
                    {
                        srvItem.Items.Add(new TreeViewItem { Header = $"🎥 {cam.Name} [{cam.IpAddress}]", Tag = cam, IsSelected = true });
                    }
                    root.Items.Add(srvItem);
                }
            }
            _deviceTree.Items.Add(root);
        }

        private void DeleteSelectedNode()
        {
            if (_deviceTree.SelectedItem is TreeViewItem item)
            {
                if (item.Tag is LycheeCameraNode cam)
                {
                    foreach (var srv in _store.Servers)
                    {
                        if (srv.Cameras.Remove(cam))
                        {
                            SaveAndNotify($"Camera '{cam.Name}' deleted!");
                            RefreshDeviceTree();
                            break;
                        }
                    }
                }
                else if (item.Tag is LycheeServerNode srv)
                {
                    _store.Servers.Remove(srv);
                    SaveAndNotify($"Recording Server '{srv.Name}' deleted!");
                    RefreshDeviceTree();
                }
            }
        }

        #endregion

        #region --- 3. LIVE PROPERTIES EDITOR PANE ---

        private UIElement CreatePropertiesPane()
        {
            Border border = new Border { Background = Brushes.White, BorderBrush = new SolidColorBrush(Color.FromRgb(254, 205, 211)), BorderThickness = new Thickness(1), CornerRadius = new CornerRadius(6) };
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(32) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(28) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(44) });

            Border header = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 241, 242)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(254, 205, 211)),
                BorderThickness = new Thickness(0, 0, 0, 1),
                Padding = new Thickness(10, 6, 10, 6)
            };
            header.Child = new TextBlock { Text = "Lychee Camera Properties & Encoding", FontWeight = FontWeights.Bold, FontSize = 12, Foreground = new SolidColorBrush(LycheeDark) };
            Grid.SetRow(header, 0);
            grid.Children.Add(header);

            Border subHeader = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 245, 245)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(254, 205, 211)),
                BorderThickness = new Thickness(0, 0, 0, 1),
                Padding = new Thickness(8, 4, 8, 4)
            };
            subHeader.Child = new TextBlock { Text = "Live Parameter Configurations", FontWeight = FontWeights.SemiBold, FontSize = 11, Foreground = Brushes.DimGray };
            Grid.SetRow(subHeader, 1);
            grid.Children.Add(subHeader);

            ScrollViewer scroll = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            _propertiesPanel = new StackPanel { Margin = new Thickness(8) };
            scroll.Content = _propertiesPanel;
            Grid.SetRow(scroll, 2);
            grid.Children.Add(scroll);

            Border footer = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 241, 242)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(254, 205, 211)),
                BorderThickness = new Thickness(0, 1, 0, 0),
                Padding = new Thickness(8, 6, 8, 6)
            };
            Button applyBtn = new Button
            {
                Content = "💾 Save & Apply Lychee Camera Settings",
                FontWeight = FontWeights.Bold,
                Background = new SolidColorBrush(LycheePrimary),
                Foreground = Brushes.White,
                Height = 30,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand
            };
            applyBtn.Click += (s, e) => SaveAndNotify("Lychee Camera properties applied successfully!");
            footer.Child = applyBtn;
            Grid.SetRow(footer, 3);
            grid.Children.Add(footer);

            border.Child = grid;
            return border;
        }

        private void PopulateProperties(LycheeCameraNode cam)
        {
            _propertiesPanel.Children.Clear();
            if (cam == null) return;

            _propertiesPanel.Children.Add(CreatePropertyGroupHeader("▼ General Device Info"));
            _propertiesPanel.Children.Add(CreateEditableRow("Camera Name", cam.Name, v => { cam.Name = v; UpdatePreview(cam); }));
            _propertiesPanel.Children.Add(CreateEditableRow("IP Address", cam.IpAddress, v => cam.IpAddress = v));
            _propertiesPanel.Children.Add(CreateEditableRow("Driver / Protocol", cam.Driver, v => cam.Driver = v));
            _propertiesPanel.Children.Add(CreateEditableRow("RTSP Stream URL", cam.RtspUrl, v => cam.RtspUrl = v));

            _propertiesPanel.Children.Add(CreatePropertyGroupHeader("▼ Video Encoding & Stream"));
            _propertiesPanel.Children.Add(CreateEditableRow("Codec", cam.Codec, v => cam.Codec = v));
            _propertiesPanel.Children.Add(CreateEditableRow("Resolution", cam.Resolution, v => cam.Resolution = v));
            _propertiesPanel.Children.Add(CreateEditableRow("Frames per Second (FPS)", cam.Fps.ToString(), v => { if (int.TryParse(v, out int n)) cam.Fps = n; }));
            _propertiesPanel.Children.Add(CreateEditableRow("Max Bitrate (kbit/s)", cam.Bitrate.ToString(), v => { if (int.TryParse(v, out int n)) cam.Bitrate = n; }));

            _propertiesPanel.Children.Add(CreatePropertyGroupHeader("▼ Image Tuning & Quality"));
            _propertiesPanel.Children.Add(CreateSliderRow("Brightness", cam.Brightness, v => cam.Brightness = (int)v));
            _propertiesPanel.Children.Add(CreateSliderRow("Contrast", cam.Contrast, v => cam.Contrast = (int)v));
            _propertiesPanel.Children.Add(CreateSliderRow("Saturation", cam.Saturation, v => cam.Saturation = (int)v));
            _propertiesPanel.Children.Add(CreateSliderRow("Sharpness", cam.Sharpness, v => cam.Sharpness = (int)v));
        }

        private Border CreatePropertyGroupHeader(string name)
        {
            Border b = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 228, 230)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(254, 205, 211)),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(6, 3, 6, 3),
                Margin = new Thickness(0, 8, 0, 4),
                CornerRadius = new CornerRadius(4)
            };
            b.Child = new TextBlock { Text = name, FontWeight = FontWeights.Bold, FontSize = 11, Foreground = new SolidColorBrush(LycheeDark) };
            return b;
        }

        private Grid CreateEditableRow(string label, string val, Action<string> onUpdate)
        {
            Grid g = new Grid { Margin = new Thickness(4, 2, 4, 2) };
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(170) });
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock lbl = new TextBlock { Text = label, FontSize = 11, VerticalAlignment = VerticalAlignment.Center, Foreground = Brushes.Black };
            TextBox box = new TextBox { Text = val, FontSize = 11, Height = 22, Background = Brushes.White };
            box.TextChanged += (s, e) => onUpdate(box.Text);

            Grid.SetColumn(lbl, 0);
            Grid.SetColumn(box, 1);
            g.Children.Add(lbl);
            g.Children.Add(box);
            return g;
        }

        private Grid CreateSliderRow(string label, int val, Action<double> onUpdate)
        {
            Grid g = new Grid { Margin = new Thickness(4, 2, 4, 2) };
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });

            TextBlock lbl = new TextBlock { Text = label, FontSize = 11, VerticalAlignment = VerticalAlignment.Center };
            TextBlock valLbl = new TextBlock { Text = val.ToString(), FontSize = 11, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center };
            
            Slider slider = new Slider { Minimum = 0, Maximum = 100, Value = val, Margin = new Thickness(4, 0, 4, 0), VerticalAlignment = VerticalAlignment.Center };
            slider.ValueChanged += (s, e) =>
            {
                valLbl.Text = ((int)slider.Value).ToString();
                onUpdate(slider.Value);
            };

            Grid.SetColumn(lbl, 0);
            Grid.SetColumn(slider, 1);
            Grid.SetColumn(valLbl, 2);
            g.Children.Add(lbl);
            g.Children.Add(slider);
            g.Children.Add(valLbl);
            return g;
        }

        #endregion

        #region --- 4. VIDEO PREVIEW PANE WITH LYCHEE THEME ---

        private UIElement CreateVideoPreviewPane()
        {
            Border border = new Border { Background = new SolidColorBrush(Color.FromRgb(255, 241, 242)), BorderBrush = new SolidColorBrush(Color.FromRgb(254, 205, 211)), BorderThickness = new Thickness(1), CornerRadius = new CornerRadius(6) };
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(32) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });

            Border header = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 241, 242)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(254, 205, 211)),
                BorderThickness = new Thickness(0, 0, 0, 1),
                Padding = new Thickness(10, 4, 10, 4)
            };
            DockPanel headerDock = new DockPanel();
            headerDock.Children.Add(new TextBlock { Text = "🍒 Lychee Live Stream & PTZ Control", FontWeight = FontWeights.Bold, FontSize = 11.5, Foreground = new SolidColorBrush(LycheeDark), VerticalAlignment = VerticalAlignment.Center });

            StackPanel ptzBar = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            ptzBar.Children.Add(CreatePtzBtn("◀ Pan Left"));
            ptzBar.Children.Add(CreatePtzBtn("▲ Tilt Up"));
            ptzBar.Children.Add(CreatePtzBtn("▼ Tilt Down"));
            ptzBar.Children.Add(CreatePtzBtn("▶ Pan Right"));
            ptzBar.Children.Add(CreatePtzBtn("🔍 Zoom +"));
            ptzBar.Children.Add(CreatePtzBtn("🔍 Zoom -"));
            headerDock.Children.Add(ptzBar);

            header.Child = headerDock;
            Grid.SetRow(header, 0);
            grid.Children.Add(header);

            // Black Screen with Live AI Motion Tracker
            Border blackScreen = new Border { Background = Brushes.Black, Margin = new Thickness(8, 4, 8, 2), ClipToBounds = true, CornerRadius = new CornerRadius(4) };
            _videoCanvas = new Canvas { Background = new SolidColorBrush(Color.FromRgb(15, 20, 30)) };

            // Grid Lines in Rose Tint
            for (int i = 50; i < 800; i += 60)
            {
                Line gridLine = new Line { X1 = i, Y1 = 0, X2 = i, Y2 = 600, Stroke = new SolidColorBrush(Color.FromArgb(20, 244, 63, 94)), StrokeThickness = 1 };
                _videoCanvas.Children.Add(gridLine);
            }

            // Moving Target AI Box (Lychee Rose Target Box)
            _motionBox = new Rectangle
            {
                Width = 120,
                Height = 75,
                Stroke = new SolidColorBrush(LycheeRose),
                StrokeThickness = 2,
                Fill = new SolidColorBrush(Color.FromArgb(40, 225, 29, 72))
            };
            Canvas.SetLeft(_motionBox, _motionX);
            Canvas.SetTop(_motionBox, _motionY);
            _videoCanvas.Children.Add(_motionBox);

            StackPanel overlay = new StackPanel { Margin = new Thickness(16) };
            _streamFpsLabel = new TextBlock { Text = "● LYCHEE LIVE [25.0 FPS] | BITRATE: 4096 kbps | H.265", Foreground = new SolidColorBrush(LycheeRose), FontSize = 12, FontWeight = FontWeights.Bold };
            TextBlock timeLabel = new TextBlock { Text = $"TIMESTAMP: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}", Foreground = Brushes.White, FontSize = 11, Margin = new Thickness(0, 4, 0, 0) };
            overlay.Children.Add(_streamFpsLabel);
            overlay.Children.Add(timeLabel);
            _videoCanvas.Children.Add(overlay);

            blackScreen.Child = _videoCanvas;
            Grid.SetRow(blackScreen, 1);
            grid.Children.Add(blackScreen);

            Border footer = new Border { Background = Brushes.White, Padding = new Thickness(10, 4, 10, 4) };
            _cameraTitleLabel = new TextBlock
            {
                Text = "SELECTED CAMERA: LYCHEE MAIN ENTRANCE CAM 01",
                FontSize = 11,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(LycheeDark),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            footer.Child = _cameraTitleLabel;
            Grid.SetRow(footer, 2);
            grid.Children.Add(footer);

            border.Child = grid;
            return border;
        }

        private Button CreatePtzBtn(string label)
        {
            Button btn = new Button
            {
                Content = label,
                FontSize = 10,
                Margin = new Thickness(2, 0, 2, 0),
                Padding = new Thickness(6, 2, 6, 2),
                Background = Brushes.White,
                Foreground = new SolidColorBrush(LycheeDark),
                BorderBrush = new SolidColorBrush(Color.FromRgb(254, 205, 211)),
                Cursor = Cursors.Hand
            };
            btn.Click += (s, e) => SaveAndNotify($"LycheeVMS: PTZ Command [{label}] sent to camera!");
            return btn;
        }

        private void InitVideoSimulation()
        {
            _videoSimTimer.Interval = TimeSpan.FromMilliseconds(40);
            _videoSimTimer.Tick += (s, e) =>
            {
                _motionX += _dx;
                _motionY += _dy;

                if (_motionX <= 20 || _motionX >= 550) _dx = -_dx;
                if (_motionY <= 20 || _motionY >= 200) _dy = -_dy;

                Canvas.SetLeft(_motionBox, _motionX);
                Canvas.SetTop(_motionBox, _motionY);
            };
            _videoSimTimer.Start();
        }

        private void UpdatePreview(LycheeCameraNode cam)
        {
            if (cam != null)
            {
                _cameraTitleLabel.Text = $"SELECTED CAMERA: {cam.Name} • [{cam.IpAddress}] • PROTOCOL: {cam.Driver}";
                _streamFpsLabel.Text = $"● LYCHEE LIVE [{cam.Fps} FPS] | BITRATE: {cam.Bitrate} kbps | CODEC: {cam.Codec} | {cam.Resolution}";
            }
        }

        #endregion

        #region --- DIALOGS: ADD CAMERA & ADD SERVER ---

        private void OpenAddCameraDialog()
        {
            Window dlg = new Window
            {
                Title = "Add New IP Camera to LycheeVMS",
                Width = 480,
                Height = 440,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize,
                Background = Brushes.White
            };

            StackPanel p = new StackPanel { Margin = new Thickness(20) };
            p.Children.Add(new TextBlock { Text = "🍒 Add Lychee IP Camera", FontSize = 16, FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(LycheeDark), Margin = new Thickness(0, 0, 0, 14) });

            p.Children.Add(new TextBlock { Text = "Target Recording Server:", FontSize = 11.5 });
            ComboBox srvCombo = new ComboBox { Height = 26, Margin = new Thickness(0, 2, 0, 10) };
            foreach (var s in _store.Servers) srvCombo.Items.Add(s.Name);
            if (srvCombo.Items.Count > 0) srvCombo.SelectedIndex = 0;
            p.Children.Add(srvCombo);

            p.Children.Add(new TextBlock { Text = "Camera Display Name:", FontSize = 11.5 });
            TextBox nameBox = new TextBox { Text = "LYCHEE LOBBY DOME CAM 03", Height = 24, Margin = new Thickness(0, 2, 0, 10) };
            p.Children.Add(nameBox);

            p.Children.Add(new TextBlock { Text = "IP Address:", FontSize = 11.5 });
            TextBox ipBox = new TextBox { Text = "192.168.1.205", Height = 24, Margin = new Thickness(0, 2, 0, 10) };
            p.Children.Add(ipBox);

            p.Children.Add(new TextBlock { Text = "Driver / Manufacturer:", FontSize = 11.5 });
            ComboBox drvCombo = new ComboBox { Height = 26, Margin = new Thickness(0, 2, 0, 10) };
            drvCombo.Items.Add("ONVIF Generic Profile S/G/T");
            drvCombo.Items.Add("HikVisionGeneric");
            drvCombo.Items.Add("Dahua Generic");
            drvCombo.Items.Add("Axis Network Camera");
            drvCombo.SelectedIndex = 0;
            p.Children.Add(drvCombo);

            p.Children.Add(new TextBlock { Text = "RTSP Stream URL:", FontSize = 11.5 });
            TextBox rtspBox = new TextBox { Text = "rtsp://192.168.1.205:554/live", Height = 24, Margin = new Thickness(0, 2, 0, 16) };
            p.Children.Add(rtspBox);

            StackPanel btnRow = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            Button okBtn = new Button { Content = "Add Camera", Width = 100, Height = 28, Background = new SolidColorBrush(LycheePrimary), Foreground = Brushes.White, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 8, 0), Cursor = Cursors.Hand };
            okBtn.Click += (s, e) =>
            {
                var targetServer = _store.Servers.FirstOrDefault(x => x.Name == srvCombo.Text) ?? _store.Servers.FirstOrDefault();
                if (targetServer != null)
                {
                    targetServer.Cameras.Add(new LycheeCameraNode
                    {
                        Name = nameBox.Text,
                        IpAddress = ipBox.Text,
                        Driver = drvCombo.Text,
                        RtspUrl = rtspBox.Text
                    });
                    SaveAndNotify($"Camera '{nameBox.Text}' added to LycheeVMS!");
                    RefreshDeviceTree();
                    dlg.Close();
                }
            };

            Button cancelBtn = new Button { Content = "Cancel", Width = 80, Height = 28, Cursor = Cursors.Hand };
            cancelBtn.Click += (s, e) => dlg.Close();

            btnRow.Children.Add(okBtn);
            btnRow.Children.Add(cancelBtn);
            p.Children.Add(btnRow);

            dlg.Content = p;
            dlg.ShowDialog();
        }

        private void OpenAddServerDialog()
        {
            Window dlg = new Window
            {
                Title = "Add New Recording Server to LycheeVMS",
                Width = 440,
                Height = 320,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize,
                Background = Brushes.White
            };

            StackPanel p = new StackPanel { Margin = new Thickness(20) };
            p.Children.Add(new TextBlock { Text = "🍒 Add Lychee Recording Server", FontSize = 16, FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(LycheeDark), Margin = new Thickness(0, 0, 0, 14) });

            p.Children.Add(new TextBlock { Text = "Server Name / Hostname:", FontSize = 11.5 });
            TextBox nameBox = new TextBox { Text = "LYCHEE-REC-SRV-SOUTH-02", Height = 24, Margin = new Thickness(0, 2, 0, 10) };
            p.Children.Add(nameBox);

            p.Children.Add(new TextBlock { Text = "IP Address / FQDN:", FontSize = 11.5 });
            TextBox ipBox = new TextBox { Text = "192.168.1.105", Height = 24, Margin = new Thickness(0, 2, 0, 10) };
            p.Children.Add(ipBox);

            p.Children.Add(new TextBlock { Text = "Port:", FontSize = 11.5 });
            TextBox portBox = new TextBox { Text = "7563", Height = 24, Margin = new Thickness(0, 2, 0, 16) };
            p.Children.Add(portBox);

            StackPanel btnRow = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            Button okBtn = new Button { Content = "Add Server", Width = 100, Height = 28, Background = new SolidColorBrush(LycheePrimary), Foreground = Brushes.White, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 8, 0), Cursor = Cursors.Hand };
            okBtn.Click += (s, e) =>
            {
                _store.Servers.Add(new LycheeServerNode
                {
                    Name = nameBox.Text,
                    IpAddress = ipBox.Text,
                    Port = portBox.Text
                });
                SaveAndNotify($"Recording Server '{nameBox.Text}' added to LycheeVMS!");
                RefreshDeviceTree();
                dlg.Close();
            };

            Button cancelBtn = new Button { Content = "Cancel", Width = 80, Height = 28, Cursor = Cursors.Hand };
            cancelBtn.Click += (s, e) => dlg.Close();

            btnRow.Children.Add(okBtn);
            btnRow.Children.Add(cancelBtn);
            p.Children.Add(btnRow);

            dlg.Content = p;
            dlg.ShowDialog();
        }

        private void SaveAndNotify(string msg = "LycheeVMS configuration saved successfully!")
        {
            _store.Save();
            MessageBox.Show(msg, "LycheeVMS Management Client", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion
    }

    #endregion
}
