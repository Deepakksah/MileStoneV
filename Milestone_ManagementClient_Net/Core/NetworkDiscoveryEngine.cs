using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using MilestoneManagementClient.Models;

namespace MilestoneManagementClient.Core
{
    public class DiscoveredHardwareItem
    {
        public string DeviceName { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string DriverName { get; set; }
        public string DriverVersion { get; set; }
        public string Resolution { get; set; }
        public string MacAddress { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class SubnetInfo
    {
        public string AdapterName { get; set; }
        public IPAddress LocalIp { get; set; }
        public IPAddress SubnetMask { get; set; }
        public string NetworkCidr { get; set; }
        public string BaseIpPrefix { get; set; }
        public int StartHost { get; set; }
        public int EndHost { get; set; }
    }

    public static class NetworkDiscoveryEngine
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

        private static SubnetInfo _cachedSubnetInfo;

        public static SubnetInfo GetActiveSubnetInfo()
        {
            if (_cachedSubnetInfo != null) return _cachedSubnetInfo;

            try
            {
                string localIpStr = "";
                try
                {
                    using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                    {
                        socket.Connect("8.8.8.8", 65530);
                        var endPoint = socket.LocalEndPoint as IPEndPoint;
                        if (endPoint != null)
                        {
                            localIpStr = endPoint.Address.ToString();
                        }
                    }
                }
                catch { }

                if (string.IsNullOrEmpty(localIpStr))
                {
                    foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        if (ni.OperationalStatus == OperationalStatus.Up && 
                            ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                        {
                            var ipProps = ni.GetIPProperties();
                            foreach (var u in ipProps.UnicastAddresses)
                            {
                                if (u.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(u.Address))
                                {
                                    localIpStr = u.Address.ToString();
                                    break;
                                }
                            }
                            if (!string.IsNullOrEmpty(localIpStr)) break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(localIpStr)) localIpStr = "10.192.0.100";

                int lastDot = localIpStr.LastIndexOf('.');
                string prefix = (lastDot > 0) ? localIpStr.Substring(0, lastDot + 1) : "10.192.0.";

                _cachedSubnetInfo = new SubnetInfo
                {
                    AdapterName = "Active LAN / Wi-Fi Adapter",
                    LocalIp = IPAddress.Parse(localIpStr),
                    SubnetMask = IPAddress.Parse("255.255.255.0"),
                    NetworkCidr = prefix + "0/24",
                    BaseIpPrefix = prefix,
                    StartHost = 1,
                    EndHost = 254
                };
            }
            catch
            {
                _cachedSubnetInfo = new SubnetInfo
                {
                    AdapterName = "Default LAN Adapter",
                    LocalIp = IPAddress.Parse("10.192.0.100"),
                    SubnetMask = IPAddress.Parse("255.255.255.0"),
                    NetworkCidr = "10.192.0.0/24",
                    BaseIpPrefix = "10.192.0.",
                    StartHost = 1,
                    EndHost = 254
                };
            }

            return _cachedSubnetInfo;
        }

        public static void ScanNetworkAsync(
            string basePrefix,
            int startHost,
            int endHost,
            int[] probePorts,
            Action<int, int, string> onProgress,
            Action<DiscoveredHardwareItem> onDeviceFound,
            Action<List<DiscoveredHardwareItem>> onComplete,
            string defaultUser = "admin",
            string defaultPass = "admin123")
        {
            var discovered = new List<DiscoveredHardwareItem>();
            var lockObj = new object();
            var discoveredIps = new HashSet<string>();

            Logger.Info("DISCOVERY", string.Format("Starting Non-blocking Scan Thread: Prefix={0}, Range={1}-{2}", basePrefix, startHost, endHost));

            var thread = new Thread(() =>
            {
                try
                {
                    // 1. Quick ONVIF WS-Discovery probe (1000ms max)
                    try
                    {
                        var onvifDevs = DiscoverOnvifLive(1000, defaultUser, defaultPass);
                        foreach (var dev in onvifDevs)
                        {
                            lock (lockObj)
                            {
                                if (!discoveredIps.Contains(dev.IpAddress))
                                {
                                    discoveredIps.Add(dev.IpAddress);
                                    discovered.Add(dev);
                                    Logger.Info("DISCOVERY", string.Format("[ONVIF MATCH] Discovered {0} at {1}:{2}", dev.DeviceName, dev.IpAddress, dev.Port));
                                    if (onDeviceFound != null)
                                    {
                                        try { onDeviceFound(dev); } catch { }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Debug("DISCOVERY", "ONVIF discovery note: " + ex.Message);
                    }

                    // 2. Controlled Subnet Sweeping (Batches of 16 to keep CPU & message pump free)
                    int totalIps = Math.Max(1, (endHost - startHost + 1));
                    int completedCount = 0;

                    if (probePorts == null || probePorts.Length == 0)
                    {
                        probePorts = new int[] { 80, 554, 8000, 37777, 8080, 8899, 7563, 443 };
                    }

                    int batchSize = 16;
                    for (int h = startHost; h <= endHost; h += batchSize)
                    {
                        int currentBatchEnd = Math.Min(endHost, h + batchSize - 1);
                        var threadsInBatch = new List<Thread>();

                        for (int currentH = h; currentH <= currentBatchEnd; currentH++)
                        {
                            string targetIp = basePrefix + currentH;
                            var t = new Thread(ipObj =>
                            {
                                string ip = (string)ipObj;
                                try
                                {
                                    bool alreadyFound = false;
                                    lock (lockObj) { alreadyFound = discoveredIps.Contains(ip); }

                                    if (!alreadyFound)
                                    {
                                        var found = ProbeIpForCamera(ip, probePorts, 250, defaultUser, defaultPass);
                                        if (found != null)
                                        {
                                            lock (lockObj)
                                            {
                                                if (!discoveredIps.Contains(ip))
                                                {
                                                    discoveredIps.Add(ip);
                                                    discovered.Add(found);
                                                    Logger.Info("DISCOVERY", string.Format("[PORT MATCH] Discovered {0} at {1}", found.DeviceName, found.IpAddress));
                                                    if (onDeviceFound != null)
                                                    {
                                                        try { onDeviceFound(found); } catch { }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch { }
                                finally
                                {
                                    int done = Interlocked.Increment(ref completedCount);
                                    if (onProgress != null)
                                    {
                                        try { onProgress(done, totalIps, ip); } catch { }
                                    }
                                }
                            });
                            t.IsBackground = true;
                            threadsInBatch.Add(t);
                            t.Start(targetIp);
                        }

                        // Wait for current small batch to finish before launching next batch
                        foreach (var t in threadsInBatch)
                        {
                            t.Join(350);
                        }

                        Thread.Sleep(10); // Yield to OS
                    }

                    Logger.Info("DISCOVERY", string.Format("Scan Complete. Discovered {0} devices.", discovered.Count));
                }
                catch (Exception ex)
                {
                    Logger.Error("DISCOVERY", "Scan thread exception", ex);
                }
                finally
                {
                    if (onComplete != null)
                    {
                        try { onComplete(discovered); } catch { }
                    }
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        private static DiscoveredHardwareItem ProbeIpForCamera(string ip, int[] ports, int timeoutMs, string defaultUser = "admin", string defaultPass = "admin123")
        {
            // 1. Ignore Gateways, Broadcast, Host machine, and common router IPs
            if (string.IsNullOrEmpty(ip) || ip.EndsWith(".1") || ip.EndsWith(".254") || ip.EndsWith(".255") || ip == "127.0.0.1")
            {
                return null;
            }

            foreach (var port in ports)
            {
                TcpClient client = null;
                try
                {
                    client = new TcpClient();
                    var asyncResult = client.BeginConnect(ip, port, null, null);
                    bool isConnected = asyncResult.AsyncWaitHandle.WaitOne(timeoutMs, false);
                    if (isConnected && client.Connected)
                    {
                        try { client.EndConnect(asyncResult); } catch { }

                        string banner = "";
                        if (port == 80 || port == 8080 || port == 8899 || port == 8000)
                        {
                            banner = GrabHttpBanner(ip, port, 150);
                        }

                        // 2. Reject Routers, Gateways, Web Servers, and non-camera systems
                        string lowerBanner = banner.ToLower();
                        if (lowerBanner.Contains("router") || lowerBanner.Contains("gateway") || lowerBanner.Contains("mikrotik") ||
                            lowerBanner.Contains("openwrt") || lowerBanner.Contains("jiofiber") || lowerBanner.Contains("airtel") ||
                            lowerBanner.Contains("apache") || lowerBanner.Contains("nginx") || lowerBanner.Contains("iis") ||
                            lowerBanner.Contains("microsoft-httpapi") || lowerBanner.Contains("printer") || lowerBanner.Contains("cups"))
                        {
                            return null;
                        }

                        string mac = ResolveMac(ip);
                        string brand = OnvifDeviceProber.ResolveBrandFromMac(mac);
                        string model = "IP Network Camera";
                        string driver = HardwareDriverRegistry.GetDriverByBrand(brand).DriverName;
                        bool isVerifiedCamera = (port == 554 || port == 8554);

                        // Deep ONVIF SOAP metadata probing
                        var onvifInfo = OnvifDeviceProber.ProbeCamera(ip, port, defaultUser, defaultPass);
                        if (onvifInfo != null && onvifInfo.Manufacturer != "Universal ONVIF")
                        {
                            brand = onvifInfo.Manufacturer;
                            model = onvifInfo.Model;
                            driver = onvifInfo.DriverName;
                            isVerifiedCamera = true;
                        }
                        else if (brand != "Universal ONVIF")
                        {
                            isVerifiedCamera = true;
                        }
                        else
                        {
                            brand = IdentifyBrand(port, banner);
                            if (brand != "Universal ONVIF" && brand != "Generic RTSP Streamer")
                            {
                                isVerifiedCamera = true;
                                model = IdentifyModel(brand, port);
                                driver = IdentifyDriver(brand);
                            }
                        }

                        // Only return if verified as camera device (Skip non-camera servers/routers)
                        if (!isVerifiedCamera && port != 554 && port != 8000 && port != 37777 && port != 8899)
                        {
                            return null;
                        }

                        string safeBrand = brand.Replace(" ", "-").ToUpper();
                        string devName = string.Format("{0}-CAM-{1}", safeBrand, ip.Replace(".", "_"));

                        // Smart credential assignment for instant Live Video playback
                        string autoUser = defaultUser;
                        string autoPass = defaultPass;
                        if (brand.IndexOf("Axis", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            autoUser = "root";
                            autoPass = "pass";
                        }
                        else if (brand.IndexOf("Hikvision", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            autoUser = "admin";
                            autoPass = !string.IsNullOrEmpty(defaultPass) ? defaultPass : "admin123";
                        }

                        return new DiscoveredHardwareItem
                        {
                            DeviceName = devName,
                            IpAddress = ip,
                            Port = port,
                            Manufacturer = brand,
                            Model = model,
                            DriverName = driver,
                            DriverVersion = "v2025.3 MIP / ONVIF",
                            Resolution = "1920x1080 / 4K UHD",
                            MacAddress = mac,
                            Status = "Live Online",
                            UserName = autoUser,
                            Password = autoPass
                        };
                    }
                }
                catch { }
                finally
                {
                    if (client != null)
                    {
                        try { client.Close(); } catch { }
                    }
                }
            }
            return null;
        }

        private static List<DiscoveredHardwareItem> DiscoverOnvifLive(int timeoutMs, string defaultUser = "admin", string defaultPass = "admin123")
        {
            var results = new List<DiscoveredHardwareItem>();
            UdpClient udp = null;
            try
            {
                string probeXml =
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<Envelope xmlns:dn=\"http://www.onvif.org/ver10/network/wsdl\" xmlns=\"http://www.w3.org/2003/05/soap-envelope\">" +
                    "<Header><wsa:MessageID xmlns:wsa=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\">uuid:" + Guid.NewGuid() + "</wsa:MessageID>" +
                    "<wsa:To xmlns:wsa=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\">urn:schemas-xmlsoap-org:ws:2005:04:discovery</wsa:To>" +
                    "<wsa:Action xmlns:wsa=\"http://schemas.xmlsoap.org/ws/2004/08/addressing\">http://schemas.xmlsoap.org/ws/2005/04/discovery/Probe</wsa:Action>" +
                    "</Header><Body><Probe xmlns=\"http://schemas.xmlsoap.org/ws/2005/04/discovery\"><Types>dn:NetworkVideoTransmitter</Types></Probe></Body></Envelope>";

                byte[] bytes = Encoding.UTF8.GetBytes(probeXml);

                udp = new UdpClient();
                udp.EnableBroadcast = true;
                udp.Client.ReceiveTimeout = timeoutMs;
                var ep = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 3702);
                udp.Send(bytes, bytes.Length, ep);

                DateTime start = DateTime.Now;
                while ((DateTime.Now - start).TotalMilliseconds < timeoutMs)
                {
                    try
                    {
                        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                        byte[] respBytes = udp.Receive(ref sender);
                        string resp = Encoding.UTF8.GetString(respBytes);

                        if (!string.IsNullOrEmpty(resp) && resp.Contains("ProbeMatches"))
                        {
                            string ip = sender.Address.ToString();
                            string brand = ParseBrandFromSoap(resp);

                            string autoUser = defaultUser;
                            string autoPass = defaultPass;
                            if (brand.IndexOf("Axis", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                autoUser = "root";
                                autoPass = "pass";
                            }
                            else if (brand.IndexOf("Hikvision", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                autoUser = "admin";
                                autoPass = !string.IsNullOrEmpty(defaultPass) ? defaultPass : "admin123";
                            }

                            var item = new DiscoveredHardwareItem
                            {
                                DeviceName = brand.ToUpper() + "-ONVIF-" + ip.Replace(".", "_"),
                                IpAddress = ip,
                                Port = sender.Port > 0 ? sender.Port : 80,
                                Manufacturer = brand,
                                Model = "ONVIF Profile S/G/T/M Camera",
                                DriverName = IdentifyDriver(brand),
                                DriverVersion = "v2025.3 Conformance Driver",
                                Resolution = "1920x1080 / 4K UHD",
                                MacAddress = ResolveMac(ip),
                                Status = "Live ONVIF",
                                UserName = autoUser,
                                Password = autoPass
                            };
                            results.Add(item);
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
            }
            catch { }
            finally
            {
                if (udp != null)
                {
                    try { udp.Close(); } catch { }
                }
            }

            return results;
        }

        private static string GrabHttpBanner(string ip, int port, int timeoutMs)
        {
            TcpClient tcp = null;
            try
            {
                tcp = new TcpClient();
                var res = tcp.BeginConnect(ip, port, null, null);
                if (res.AsyncWaitHandle.WaitOne(timeoutMs, false) && tcp.Connected)
                {
                    try { tcp.EndConnect(res); } catch { }
                    using (var stream = tcp.GetStream())
                    {
                        stream.ReadTimeout = timeoutMs;
                        byte[] req = Encoding.ASCII.GetBytes("GET / HTTP/1.1\r\nHost: " + ip + "\r\nConnection: close\r\n\r\n");
                        stream.Write(req, 0, req.Length);

                        byte[] buf = new byte[1024];
                        int read = stream.Read(buf, 0, buf.Length);
                        if (read > 0)
                        {
                            return Encoding.ASCII.GetString(buf, 0, read);
                        }
                    }
                }
            }
            catch { }
            finally
            {
                if (tcp != null) { try { tcp.Close(); } catch { } }
            }
            return "";
        }

        private static string IdentifyBrand(int port, string banner)
        {
            string lower = banner.ToLower();
            if (lower.Contains("hikvision") || lower.Contains("hik-web") || port == 8000) return "Hikvision";
            if (lower.Contains("axis") || lower.Contains("vapix")) return "Axis Communications";
            if (lower.Contains("dahua") || lower.Contains("webcam") || port == 37777) return "Dahua Technology";
            if (lower.Contains("wisenet") || lower.Contains("hanwha")) return "Hanwha Vision";
            if (lower.Contains("bosch")) return "Bosch Security";
            if (port == 7563) return "Milestone Interconnect";
            if (port == 554) return "Generic RTSP Streamer";
            return "Universal ONVIF";
        }

        private static string ParseBrandFromSoap(string soap)
        {
            string lower = soap.ToLower();
            if (lower.Contains("hikvision")) return "Hikvision";
            if (lower.Contains("axis")) return "Axis Communications";
            if (lower.Contains("dahua")) return "Dahua Technology";
            if (lower.Contains("hanwha") || lower.Contains("wisenet")) return "Hanwha Vision";
            if (lower.Contains("bosch")) return "Bosch Security";
            if (lower.Contains("sony")) return "Sony Security";
            if (lower.Contains("panasonic")) return "Panasonic i-PRO";
            if (lower.Contains("uniview")) return "Uniview (UNV)";
            if (lower.Contains("flir")) return "FLIR Systems";
            return "Universal ONVIF";
        }

        private static string IdentifyModel(string brand, int port)
        {
            if (brand == "Hikvision") return "Hikvision IP Camera (ISAPI / Port " + port + ")";
            if (brand == "Axis Communications") return "Axis Network Camera (VAPIX)";
            if (brand == "Dahua Technology") return "Dahua Network Camera (DH-IP)";
            if (brand == "Hanwha Vision") return "Hanwha Wisenet AI Dome";
            if (brand == "Bosch Security") return "Bosch AUTODOME / Dinion";
            if (brand == "Milestone Interconnect") return "Milestone XProtect Recorder Node";
            return "ONVIF Profile S/G/T Camera";
        }

        private static string IdentifyDriver(string brand)
        {
            if (brand == "Hikvision") return "HikVision Generic IP Device Driver";
            if (brand == "Axis Communications") return "Axis Network Camera Driver Framework";
            if (brand == "Dahua Technology") return "Dahua Technology Universal ONVIF Driver";
            if (brand == "Hanwha Vision") return "Hanwha Vision / Wisenet Driver Framework";
            if (brand == "Bosch Security") return "Bosch Dinion & Autodome Driver Framework";
            if (brand == "Milestone Interconnect") return "Milestone Direct XProtect Recorder Node";
            return "Universal ONVIF Conformance Driver";
        }

        private static string ResolveMac(string ipStr)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(ipStr);
                byte[] mac = new byte[6];
                uint len = (uint)mac.Length;
                int destIp = BitConverter.ToInt32(ip.GetAddressBytes(), 0);
                if (SendARP(destIp, 0, mac, ref len) == 0)
                {
                    return string.Format("{0:X2}:{1:X2}:{2:X2}:{3:X2}:{4:X2}:{5:X2}", mac[0], mac[1], mac[2], mac[3], mac[4], mac[5]);
                }
            }
            catch { }
            return "00:00:00:00:00:00";
        }
    }
}
