using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security;
using System.Web;
using System.Xml;
using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Resources;

namespace VideoOS.Platform.SDK.Config;

public class DBSystemInfo : ISystemInfo
{
	private enum populated
	{
		notattempted,
		succeeded
	}

	private populated _populated;

	private const bool _invalidated = false;

	private bool _isLoggedIn;

	private readonly string _fileAndPath;

	private DBLoginServer _dbLoginServer;

	private Collection<IDisposable> _toBeDisposed = new Collection<IDisposable>();

	public string UserName { get; set; }

	public SecureString Password { get; set; }

	public AuthenticationType AuthenticationType => AuthenticationType.Basic;

	public HttpStatusCode LastStatusCode { get; set; }

	public string Url { get; set; }

	public Uri Uri => LoginSettings.Uri;

	public string Hostname => LoginSettings.Uri.AbsolutePath;

	public int Port => 0;

	public Exception LastException
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public bool IsConnected { get; }

	public bool Invalidated => false;

	public ServerTypes ServerType => ServerTypes.DATABASE;

	public InternalList<Guid> SlaveIds { get; private set; }

	public bool SlavesPopulated { get; public set; }

	public string MacAddress { get; set; }

	public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();

	public CameraCollection Cameras { get; }

	public RecorderCollection Recorders { get; }

	public MicrophoneCollection Microphones { get; }

	public SpeakerCollection Speakers { get; }

	public UserDefinedEventCollection Events { get; }

	public OutputDeviceCollection OutputDevices { get; }

	public MatrixMonitorCollection MatrixMonitors { get; }

	public IDictionary<Guid, IMetadataDevice> MetadataDevices { get; }

	public string Token => "";

	public UserContext UserContext { get; }

	public NetworkCredential Credentials => LoginSettings.NetworkCredential;

	public bool MasterOnly
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public string Name { get; set; }

	public Guid Id => ServerId.Id;

	public ServerId ServerId { get; }

	public ILoginServer LoginServer => _dbLoginServer;

	public LoginSettings LoginSettings { get; set; }

	public bool Populated => _populated == populated.succeeded;

	public bool PopulatedHasBeenTried => _populated != populated.notattempted;

	public IConfigurationInfo ConfigurationInfo { get; }

	public DateTime LastHardwareUpdate { get; } = DateTime.MinValue;

	public DBSystemInfo(LoginSettings loginSettings, UserContext userContext)
	{
		Cameras = new CameraCollection();
		Microphones = new MicrophoneCollection();
		Speakers = new SpeakerCollection();
		Events = new UserDefinedEventCollection();
		Recorders = new RecorderCollection();
		MatrixMonitors = new MatrixMonitorCollection();
		OutputDevices = new OutputDeviceCollection();
		SlaveIds = new InternalList<Guid>();
		SlavesPopulated = false;
		MetadataDevices = new Dictionary<Guid, IMetadataDevice>();
		MacAddress = "";
		ConfigurationInfo = new ConfigurationInfo();
		IConfigurationInfo configurationInfo = ConfigurationInfo;
		IDeviceMappingInfo[] deviceMappings = Array.Empty<DeviceMappingInfo>();
		configurationInfo.DeviceMappings = deviceMappings;
		LoginSettings = loginSettings;
		Name = LoginSettings.Uri.LocalPath;
		Url = loginSettings.Uri.ToString();
		UserName = loginSettings.NetworkCredential.UserName;
		Password = loginSettings.NetworkCredential.SecurePassword;
		UserContext = userContext;
		LastStatusCode = HttpStatusCode.OK;
		_dbLoginServer = new DBLoginServer();
		_dbLoginServer.UserContext = userContext;
		string originalString = loginSettings.Uri.OriginalString;
		originalString = originalString.Replace("/", "\\");
		string text = ((!originalString.StartsWith("file:\\")) ? originalString : originalString.Substring("file:\\".Length));
		_dbLoginServer.RootPath = text;
		if (text.ToUpper().EndsWith("SCP"))
		{
			if (File.Exists(text))
			{
				ServerId = new ServerId("BLK", text, 0, Guid.NewGuid());
				ServerId.UserContext = userContext;
				loginSettings.Guid = ServerId.Id;
				loginSettings.ServerType = "BLK";
				_fileAndPath = text;
				IsConnected = true;
				_isLoggedIn = true;
				return;
			}
			throw new MIPException("File not found:" + text);
		}
		string text2 = text;
		string text3 = Path.Combine(text, "config.xml");
		if (File.Exists(text3))
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(text3);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("bank/description");
			if (xmlNode != null)
			{
				Name = xmlNode.InnerText;
			}
			xmlNode = xmlDocument.SelectSingleNode("bank/encryption");
		}
		if (!File.Exists(text))
		{
			text2 = Path.Combine(text, "cache.xml");
			if (!File.Exists(text2))
			{
				text2 = Path.Combine(text, "archives_cache.xml");
			}
		}
		if (File.Exists(text2))
		{
			string[] array = text2.Split('\\');
			text = Path.GetDirectoryName(text2);
			string text4 = array[array.Length - 2];
			Guid guid;
			try
			{
				guid = new Guid(text4);
			}
			catch
			{
				guid = Guid.NewGuid();
			}
			if (string.IsNullOrEmpty(Name))
			{
				Name = text4;
			}
			ServerId = new ServerId("BLK", Uri.UriSchemeFile, text, 0, guid);
			ServerId.UserContext = userContext;
			loginSettings.Guid = guid;
			loginSettings.ServerType = "BLK";
			_fileAndPath = text2;
			IsConnected = true;
			return;
		}
		throw new MIPException("File or path not found:" + text);
	}

	public void SetConnectRetry(bool autoConnect)
	{
	}

	public bool ProductSupportOfKind(Guid kind)
	{
		return true;
	}

	public string GetMIPServerType()
	{
		return "";
	}

	public void UpdateServerId(Guid id)
	{
	}

	public void AddSingleCameraConfig(Guid recorderGuid, XmlDocument doc)
	{
	}

	public void AddSingleMicrophoneConfig(Guid recorderGuid, XmlDocument doc)
	{
	}

	public bool Populate()
	{
		if (_populated == populated.notattempted)
		{
			IRecorder recorder = new RecorderInFile(this, "Bank", ServerId.Id, ServerId);
			Recorders.Add(recorder);
			if (ServerId.ServerType == "BLK")
			{
				if (_fileAndPath.ToUpper().EndsWith("SCP"))
				{
					PopulateFromScp(_fileAndPath, recorder);
				}
				else
				{
					PopulateFromBank(_fileAndPath, recorder);
				}
			}
			else if (ServerId.ServerType == "PQZ")
			{
				if (ServerId.ServerHostname.ToUpper().EndsWith("INI"))
				{
					BuildCameraListFromIniFile(ServerId.ServerHostname, recorder);
				}
				else
				{
					string[] array = ServerId.ServerHostname.Split('\\');
					Cameras.Add(new CameraInFile(this, recorder, Guid.NewGuid(), array[array.Length - 1], "", isEncrypted: false)
					{
						ArchivePath = "",
						DatabasePath = ServerId.ServerHostname,
						CameraPath = ""
					});
				}
			}
			_populated = populated.succeeded;
		}
		return true;
	}

	private void PopulateFromScp(string scpPath, IRecorder recorder)
	{
		string path = Path.Combine(Path.GetDirectoryName(scpPath), "Data");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(scpPath);
		foreach (XmlNode item in xmlDocument.SelectNodes("ProjectFile/SystemInformation/Recorders/OfflineRecorderInfo/OfflineSources/OfflineSourceInfo/BankPath"))
		{
			string fullPath = Path.GetFullPath(Path.Combine(path, item.InnerText));
			string text = Path.Combine(fullPath, "cache.xml");
			if (!File.Exists(text))
			{
				text = Path.Combine(fullPath, "archives_cache.xml");
			}
			if (File.Exists(text))
			{
				PopulateFromBank(text, recorder);
			}
		}
		List<DeviceMappingInfo> list = new List<DeviceMappingInfo>();
		foreach (XmlNode item2 in xmlDocument.SelectNodes("ProjectFile/SystemInformation/DeviceMappings/DeviceMappingInfo"))
		{
			XmlNode xmlNode2 = item2.SelectSingleNode("DeviceA");
			XmlNode xmlNode3 = item2.SelectSingleNode("DeviceB");
			if (xmlNode2 != null && xmlNode3 != null)
			{
				try
				{
					Guid deviceA = new Guid(xmlNode2.InnerText);
					Guid deviceB = new Guid(xmlNode3.InnerText);
					list.Add(new DeviceMappingInfo
					{
						DeviceA = deviceA,
						DeviceB = deviceB
					});
				}
				catch (FormatException ex)
				{
					EnvironmentManager.Instance.Log(error: true, "DBSystemInfo", ex.Message);
				}
			}
		}
		IConfigurationInfo configurationInfo = ConfigurationInfo;
		IDeviceMappingInfo[] deviceMappings = list.ToArray();
		configurationInfo.DeviceMappings = deviceMappings;
	}

	private void PopulateFromBank(string cacheXmlPath, IRecorder recorder)
	{
		string directoryName = Path.GetDirectoryName(cacheXmlPath);
		DatabaseBank databaseBank = new DatabaseBank(directoryName);
		bool isEncrypted = databaseBank.IsEncrypted;
		if (isEncrypted && !databaseBank.IsPasswordValid(new NetworkCredential(string.Empty, Password).Password))
		{
			throw new NotAuthorizedMIPException("Provided password not valid for encrypted bank");
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(cacheXmlPath);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("bank/configuration/name");
		if (xmlNode == null)
		{
			xmlNode = xmlDocument.SelectSingleNode("bank/name");
		}
		string text = ((xmlNode == null) ? "" : xmlNode.InnerText);
		foreach (XmlNode item4 in xmlDocument.SelectNodes("bank/table"))
		{
			bool flag = true;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			if (item4.SelectSingleNode("meta_data") != null)
			{
				XmlNode xmlNode3 = item4.SelectSingleNode("meta_data/entry[@key=\"device_type\"]");
				flag = xmlNode3 != null && xmlNode3.InnerText == "Camera";
				flag2 = xmlNode3 != null && xmlNode3.InnerText == "Microphone";
				flag3 = xmlNode3 != null && xmlNode3.InnerText == "Speaker";
				flag4 = xmlNode3 != null && xmlNode3.InnerText == "Metadata";
			}
			string text2 = "";
			XmlNode xmlNode4 = item4.SelectSingleNode("group");
			bool flag5 = false;
			if (xmlNode4 != null)
			{
				text2 = " (" + xmlNode4.InnerText + ")";
				flag5 = true;
			}
			string[] array = directoryName.Split('\\');
			string text3 = array[array.Length - 1];
			XmlNode xmlNode5 = item4.SelectSingleNode("description");
			XmlNode xmlNode6 = item4.SelectSingleNode("name");
			XmlNode xmlNode7 = item4.SelectSingleNode("buffered_millisecs_to_keep");
			string text4 = ((xmlNode5 == null) ? "" : (HttpUtility.HtmlDecode(xmlNode5.InnerText) + text2));
			string text5 = ((xmlNode6 == null) ? "" : xmlNode6.InnerText);
			string dbExportName = text5;
			if (text4 == "")
			{
				text4 = text5;
			}
			if (text5 != text && text5 != "{0}" && text5 != text3 && !flag5 && xmlNode7 == null)
			{
				text4 = string.Format(CultureInfo.InvariantCulture, Strings.EdgeDevice, text4);
			}
			if (flag)
			{
				if (text5.Length < 36)
				{
					text5 = text3;
					if (text5.Length == 36)
					{
						Guid guid = new Guid(text5);
						if (!Cameras.ContainsKey(guid))
						{
							CameraInFile camera = new CameraInFile(this, recorder, guid, text4, dbExportName, isEncrypted)
							{
								DatabasePath = directoryName
							};
							Cameras.Add(camera);
						}
					}
					else
					{
						Guid guid2 = Guid.NewGuid();
						CameraInFile camera2 = new CameraInFile(this, recorder, guid2, text4, dbExportName, isEncrypted)
						{
							DatabasePath = directoryName
						};
						Cameras.Add(camera2);
					}
				}
				else
				{
					string g = text5.Substring(0, 36);
					Guid guid3 = new Guid(g);
					if (!Cameras.ContainsKey(guid3))
					{
						CameraInFile camera3 = new CameraInFile(this, recorder, guid3, text4, "", isEncrypted)
						{
							DatabasePath = directoryName
						};
						Cameras.Add(camera3);
					}
				}
			}
			if (flag2)
			{
				if (text5.Length < 36)
				{
					text5 = text3;
					if (text5.Length == 36)
					{
						Guid guid4 = new Guid(text5);
						if (!Microphones.ContainsKey(guid4))
						{
							MicrophoneInFile item = new MicrophoneInFile(this, recorder, guid4, text4, dbExportName, isEncrypted)
							{
								DatabasePath = directoryName
							};
							Microphones.Add(item);
						}
					}
					else
					{
						Guid guid5 = Guid.NewGuid();
						MicrophoneInFile item2 = new MicrophoneInFile(this, recorder, guid5, text4, dbExportName, isEncrypted)
						{
							DatabasePath = directoryName
						};
						Microphones.Add(item2);
					}
				}
				else
				{
					string g2 = text5.Substring(0, 36);
					Guid guid6 = new Guid(g2);
					if (!Microphones.ContainsKey(guid6))
					{
						MicrophoneInFile item3 = new MicrophoneInFile(this, recorder, guid6, text4, "", isEncrypted)
						{
							DatabasePath = directoryName
						};
						Microphones.Add(item3);
					}
				}
			}
			if (flag3)
			{
				if (text5.Length < 36)
				{
					text5 = text3;
					if (text5.Length == 36)
					{
						Guid guid7 = new Guid(text5);
						if (!Speakers.ContainsKey(guid7))
						{
							SpeakerInFile obj = new SpeakerInFile(this, recorder, guid7, text4, dbExportName, isEncrypted)
							{
								DatabasePath = directoryName
							};
							Speakers.Add(obj);
						}
					}
					else
					{
						Guid guid8 = Guid.NewGuid();
						SpeakerInFile obj2 = new SpeakerInFile(this, recorder, guid8, text4, dbExportName, isEncrypted)
						{
							DatabasePath = directoryName
						};
						Speakers.Add(obj2);
					}
				}
				else
				{
					string g3 = text5.Substring(0, 36);
					Guid guid9 = new Guid(g3);
					if (!Speakers.ContainsKey(guid9))
					{
						SpeakerInFile obj3 = new SpeakerInFile(this, recorder, guid9, text4, "", isEncrypted)
						{
							DatabasePath = directoryName
						};
						Speakers.Add(obj3);
					}
				}
			}
			if (!flag4)
			{
				continue;
			}
			if (text5.Length < 36)
			{
				text5 = text3;
				if (text5.Length == 36)
				{
					Guid guid10 = new Guid(text5);
					if (!MetadataDevices.ContainsKey(guid10))
					{
						MetadataDeviceInFile value = new MetadataDeviceInFile(this, recorder, guid10, text4, text5)
						{
							DatabasePath = directoryName
						};
						MetadataDevices.Add(guid10, value);
					}
				}
				else
				{
					Guid guid11 = Guid.NewGuid();
					MetadataDeviceInFile value2 = new MetadataDeviceInFile(this, recorder, guid11, text4, text5)
					{
						DatabasePath = directoryName
					};
					MetadataDevices.Add(guid11, value2);
				}
			}
			else
			{
				string g4 = text5.Substring(0, 36);
				Guid guid12 = new Guid(g4);
				if (!MetadataDevices.ContainsKey(guid12))
				{
					MetadataDeviceInFile value3 = new MetadataDeviceInFile(this, recorder, guid12, text4, "")
					{
						DatabasePath = directoryName
					};
					MetadataDevices.Add(guid12, value3);
				}
			}
		}
	}

	private void BuildCameraListFromIniFile(string file, IRecorder recorder)
	{
		try
		{
			IniFile iniFile = new IniFile(file);
			string directoryName = Path.GetDirectoryName(file);
			int num = 1;
			bool flag = false;
			Dictionary<Guid, CameraInFile> dictionary = new Dictionary<Guid, CameraInFile>();
			Dictionary<CameraInFile, Guid> dictionary2 = new Dictionary<CameraInFile, Guid>();
			do
			{
				string text = iniFile.IniReadValue("Devices", "Device" + num);
				if (text != null)
				{
					text = text.Trim(' ', '"');
					if (text.Length == 12)
					{
						string s = iniFile.IniReadValue(text, "NumberCams").Trim(' ');
						int result = 0;
						if (int.TryParse(s, out result))
						{
							for (int i = 1; i <= result; i++)
							{
								string g = iniFile.IniReadValue(text, "Guid" + i).Trim(' ');
								string name = iniFile.IniReadValue(text, "Label" + i).Trim(' ', '"');
								string text2 = iniFile.IniReadValue(text, "DatabasePath" + i);
								if (text2 != null && directoryName != null && Path.GetPathRoot(text2) == "")
								{
									text2 = Path.Combine(directoryName, text2);
								}
								string text3 = iniFile.IniReadValue(text, "ArchivePath" + i);
								if (!string.IsNullOrEmpty(text3))
								{
									text3 = Path.Combine(text3, "Archives");
									if (Path.GetPathRoot(text3) == "" && directoryName != null)
									{
										text3 = Path.Combine(directoryName, text3);
									}
								}
								CameraInFile cameraInFile = new CameraInFile(this, recorder, new Guid(g), name, "", isEncrypted: false)
								{
									DatabasePath = text2,
									ArchivePath = text3,
									CameraPath = text + "_" + i
								};
								Cameras.Add(cameraInFile);
								Guid key = new Guid(g);
								if (dictionary.ContainsKey(key))
								{
									EnvironmentManager.Instance.Log(error: true, "DBSystemInfo", "Duplicate DB id ??");
								}
								else
								{
									dictionary.Add(key, cameraInFile);
								}
								string text4 = iniFile.IniReadValue(text, "AudioGuid" + i).Trim(' ');
								if (!string.IsNullOrEmpty(text4) && Guid.TryParse(text4, out var result2))
								{
									dictionary2.Add(cameraInFile, result2);
								}
								flag = true;
							}
						}
					}
					else
					{
						flag = false;
					}
				}
				num++;
			}
			while (flag);
			flag = true;
			int num2 = 1;
			do
			{
				string text5 = iniFile.IniReadValue("AudioDevices", "Audio" + num2);
				if (!string.IsNullOrEmpty(text5))
				{
					if (Guid.TryParse(text5, out var result3))
					{
						string name2 = iniFile.IniReadValue("Audio." + text5, "DisplayName");
						if (Guid.TryParse(iniFile.IniReadValue("Audio." + text5, "DatabaseGuid"), out var result4) && dictionary.TryGetValue(result4, out var value))
						{
							MicrophoneInFile microphoneInFile = new MicrophoneInFile(this, recorder, result3, name2, "", isEncrypted: false)
							{
								DatabasePath = value.DatabasePath,
								ArchivePath = value.ArchivePath,
								CameraPath = value.CameraPath
							};
							Microphones.Add(microphoneInFile);
							foreach (CameraInFile key2 in dictionary2.Keys)
							{
								if (dictionary2[key2] == result3)
								{
									key2.Microphone = microphoneInFile;
								}
							}
						}
					}
					num2++;
				}
				else
				{
					flag = false;
				}
			}
			while (flag);
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "Unpacking Surpro2.ini", ex.Message);
		}
	}

	public bool Login()
	{
		if (ServerId.ServerType == "BLK")
		{
			DatabaseBank databaseBank = new DatabaseBank(Path.GetDirectoryName(_fileAndPath));
			if (databaseBank.IsEncrypted && !databaseBank.IsPasswordValid(new NetworkCredential(string.Empty, Password).Password))
			{
				throw new NotAuthorizedMIPException("Provided password not valid for encrypted bank");
			}
		}
		_isLoggedIn = true;
		return _isLoggedIn;
	}

	public bool IsLoggedIn()
	{
		return _isLoggedIn;
	}

	public bool ServerConnectionCheck()
	{
		return true;
	}

	public bool Logout()
	{
		Disconnect();
		return true;
	}

	public void Disconnect()
	{
		lock (_toBeDisposed)
		{
			foreach (IDisposable item in _toBeDisposed)
			{
				item.Dispose();
			}
			_toBeDisposed.Clear();
		}
	}

	public void AddToDisposableObjects(IDisposable obj)
	{
		lock (_toBeDisposed)
		{
			_toBeDisposed.Add(obj);
		}
	}

	public Collection<IDisposable> DisposableObjects()
	{
		return _toBeDisposed;
	}

	public IChange[] QueryChanges(DateTime changesFrom)
	{
		return null;
	}

	public IRecorderInfo QueryRecorderInfo(Guid recorderId)
	{
		return null;
	}

	public void RebuildDeviceCollections()
	{
	}

	public void RefreshRecorder(FQID recorderFqid)
	{
	}

	public void UpdateLastHardwareUpdateIfNewer(DateTime newLastModified)
	{
	}

	public void RegisterIntegration(string instanceId, Guid integrationId, string integrationName, string version, string manufacturerName)
	{
	}
}
