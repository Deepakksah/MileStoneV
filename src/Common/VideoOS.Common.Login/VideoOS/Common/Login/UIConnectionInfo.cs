using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Xml;

namespace VideoOS.Common.Login;

public sealed class UIConnectionInfo
{
	private const string LocalHost = "localhost";

	private readonly AsyncLoginCallback<IOnlineConnectionInfo> _asyncLoginCallback;

	private readonly List<ServerAddressHistoryEntry> _serverAddressHistory = new List<ServerAddressHistoryEntry>();

	private readonly List<AllowUnsecureUserOptionEntry> _allowUnsecureUserOptions = new List<AllowUnsecureUserOptionEntry>();

	private readonly Dictionary<string, string> _userNameHistory = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	private Guid _serverTypeId;

	private string _settingsFile;

	private Uri _serverAddress;

	private string _userName;

	public static Func<XmlDocument> XmlDocumentFactory { get; } = () => new XmlDocument();

	public static Func<Stream, XmlWriterSettings, XmlWriter> XmlWriterFactory { get; } = XmlWriter.Create;

	public static Func<Stream, XmlReader> XmlReaderFactory { get; } = XmlReader.Create;

	public AsyncLoginCallback<IOnlineConnectionInfo> AsyncLoginCallback => _asyncLoginCallback;

	public string HistoryFile
	{
		get
		{
			return _settingsFile;
		}
		set
		{
			_settingsFile = value;
			LoadFromFile(_settingsFile);
		}
	}

	public Collection<ServerType> ServerTypeCollection { get; set; }

	public Guid ServerTypeId
	{
		get
		{
			return _serverTypeId;
		}
		set
		{
			_serverTypeId = value;
		}
	}

	public ReadOnlyCollection<string> ServerAddressHistory => new ReadOnlyCollection<string>(_serverAddressHistory.ConvertAll<string>(ServerAddressHistoryToStringAddress));

	public Uri ServerAddress
	{
		get
		{
			return _serverAddress;
		}
		set
		{
			if (_serverAddress != value)
			{
				_serverAddress = value;
				UpdateAllowUnsecureConnectionPreference();
			}
		}
	}

	public AuthenticationType AuthenticationType { get; set; }

	public string LoginProviderId { get; set; }

	public bool AllowUnsecureConnection { get; set; }

	public Dictionary<string, string> UserNameHistory => _userNameHistory;

	public string UserName
	{
		get
		{
			return _userName;
		}
		set
		{
			if (_userName != value)
			{
				_userName = value;
				UpdateAllowUnsecureConnectionPreference();
			}
		}
	}

	public string EncryptedPassword { get; private set; }

	public string Password
	{
		get
		{
			if (string.IsNullOrEmpty(EncryptedPassword))
			{
				return null;
			}
			try
			{
				return DataProtection.Unprotect(EncryptedPassword);
			}
			catch
			{
				return string.Empty;
			}
		}
		set
		{
			try
			{
				if (!string.IsNullOrEmpty(value))
				{
					EncryptedPassword = DataProtection.Protect(value);
				}
				else
				{
					EncryptedPassword = null;
				}
			}
			catch
			{
			}
		}
	}

	public bool PersistPassword { get; set; }

	public bool AutoLogin { get; set; }

	public bool DisableAutoLogin { get; set; }

	public bool ShowRememberPassword { get; set; }

	public bool ShowAutoLogin { get; set; }

	public UIConnectionInfo(AsyncLoginCallback<IOnlineConnectionInfo> asyncLoginCallback)
	{
		_asyncLoginCallback = asyncLoginCallback;
		ServerType[] list = new ServerType[1]
		{
			new UnknownServerType()
		};
		ServerTypeCollection = new Collection<ServerType>(list);
		SetDefaults();
	}

	public UIConnectionInfo(AsyncLoginCallback<IOnlineConnectionInfo> asyncLoginCallback, ServerType[] serverTypes)
	{
		if (serverTypes == null)
		{
			throw new ArgumentNullException("serverTypes");
		}
		ServerTypeCollection = new Collection<ServerType>(serverTypes);
		_asyncLoginCallback = asyncLoginCallback;
		SetDefaults();
	}

	private static string ServerAddressHistoryToStringAddress(ServerAddressHistoryEntry serverAddressHistoryEntry)
	{
		return serverAddressHistoryEntry.ServerAddress;
	}

	public string TryFindPassword(string userName, AuthenticationType currentAuthenticationType)
	{
		string value = string.Empty;
		if (currentAuthenticationType != AuthenticationType.WindowsDefault)
		{
			if (_userNameHistory != null && !string.IsNullOrEmpty(userName) && _userNameHistory.TryGetValue(userName, out value))
			{
				EncryptedPassword = value;
				return Password;
			}
			return string.Empty;
		}
		return string.Empty;
	}

	private void LoadFromFile(string fileName)
	{
		_settingsFile = fileName;
		if (File.Exists(_settingsFile))
		{
			using (FileStream arg = new FileStream(_settingsFile, FileMode.Open, FileAccess.Read))
			{
				XmlReader reader = XmlReaderFactory(arg);
				LoadFromStream(reader);
			}
		}
	}

	private void LoadFromStream(XmlReader reader)
	{
		XmlDocument xmlDocument = XmlDocumentFactory();
		try
		{
			xmlDocument.Load(reader);
		}
		catch
		{
			SetDefaults();
		}
		try
		{
			XmlNode xmlNode = xmlDocument.SelectSingleNode("/login/lastlogin/servertype");
			if (xmlNode != null)
			{
				try
				{
					_serverTypeId = new Guid(xmlNode.InnerText);
				}
				catch
				{
					_serverTypeId = Guid.Empty;
				}
			}
			XmlNode xmlNode2 = xmlDocument.SelectSingleNode("/login/lastlogin/serveraddress");
			if (xmlNode2 != null)
			{
				string innerText = xmlNode2.InnerText;
				ServerAddress = new Uri(innerText);
			}
			XmlNode xmlNode3 = xmlDocument.SelectSingleNode("/login/lastlogin/authenticationtype");
			if (xmlNode3 != null)
			{
				string innerText2 = xmlNode3.InnerText;
				if (Enum.IsDefined(typeof(AuthenticationType), innerText2))
				{
					AuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), innerText2);
				}
				else
				{
					AuthenticationType = AuthenticationType.WindowsDefault;
				}
			}
			if (AuthenticationType == AuthenticationType.WindowsDefault)
			{
				UserName = WindowsIdentity.GetCurrent().Name;
				EncryptedPassword = string.Empty;
			}
			else
			{
				XmlNode xmlNode4 = xmlDocument.SelectSingleNode("/login/lastlogin/username");
				if (xmlNode4 != null)
				{
					UserName = xmlNode4.InnerText;
				}
			}
			XmlNode xmlNode5 = xmlDocument.SelectSingleNode("/login/lastlogin/rememberpassword");
			if (xmlNode5 != null)
			{
				PersistPassword = Convert.ToBoolean(xmlNode5.InnerText, CultureInfo.InvariantCulture);
			}
			XmlNode xmlNode6 = xmlDocument.SelectSingleNode("/login/lastlogin/autologin");
			if (xmlNode6 != null)
			{
				AutoLogin = Convert.ToBoolean(xmlNode6.InnerText, CultureInfo.InvariantCulture);
			}
			XmlNode xmlNode7 = xmlDocument.SelectSingleNode("/login/lastlogin/showrememberpassword");
			if (xmlNode7 != null)
			{
				ShowRememberPassword = Convert.ToBoolean(xmlNode7.InnerText, CultureInfo.InvariantCulture);
			}
			XmlNode xmlNode8 = xmlDocument.SelectSingleNode("/login/lastlogin/showautologin");
			if (xmlNode8 != null)
			{
				ShowAutoLogin = Convert.ToBoolean(xmlNode8.InnerText, CultureInfo.InvariantCulture);
			}
		}
		catch (Exception)
		{
			SetDefaults();
		}
		try
		{
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("login/allowunsecureconnections/user");
			if (xmlNodeList != null && xmlNodeList.Count > 0)
			{
				foreach (XmlNode item in xmlNodeList)
				{
					XmlAttribute xmlAttribute = item.Attributes["username"];
					XmlAttribute xmlAttribute2 = item.Attributes["allowunsecure"];
					if (xmlAttribute != null && xmlAttribute2 != null && bool.TryParse(xmlAttribute2.Value, out var result))
					{
						string value = xmlAttribute.Value;
						if (value.Equals(UserName, StringComparison.InvariantCultureIgnoreCase))
						{
							AllowUnsecureConnection = result;
						}
						_allowUnsecureUserOptions.Add(new AllowUnsecureUserOptionEntry(value, result));
					}
				}
			}
		}
		catch
		{
		}
		try
		{
			XmlNodeList xmlNodeList2 = xmlDocument.SelectNodes("login/historyaddress/entry");
			if (xmlNodeList2 != null && xmlNodeList2.Count > 0)
			{
				foreach (XmlNode item2 in xmlNodeList2)
				{
					_serverAddressHistory.Add(new ServerAddressHistoryEntry(item2.InnerText));
				}
			}
		}
		catch
		{
		}
		try
		{
			XmlNodeList xmlNodeList3 = xmlDocument.SelectNodes("login/historyusername/entry");
			if (xmlNodeList3.Count > 0)
			{
				foreach (XmlNode item3 in xmlNodeList3)
				{
					XmlAttribute xmlAttribute3 = item3.Attributes["username"];
					if (xmlAttribute3 == null)
					{
						continue;
					}
					string innerText3 = xmlAttribute3.InnerText;
					string text = null;
					XmlAttribute xmlAttribute4 = item3.Attributes["password2"];
					if (xmlAttribute4 == null)
					{
						xmlAttribute4 = item3.Attributes["password"];
						if (xmlAttribute4 == null)
						{
							continue;
						}
						text = ((!string.IsNullOrWhiteSpace(xmlAttribute4.InnerText)) ? DataProtection.ProtectFromLegacy(xmlAttribute4.InnerText) : string.Empty);
					}
					else
					{
						text = xmlAttribute4.InnerText;
					}
					if (!string.IsNullOrEmpty(innerText3) && text != null)
					{
						_userNameHistory.Add(innerText3, text);
					}
				}
			}
		}
		catch
		{
		}
		if (PersistPassword)
		{
			TryFindPassword(UserName, AuthenticationType);
		}
		else
		{
			EncryptedPassword = string.Empty;
		}
	}

	public static void CreateDirectoryIfItDoesNotExist(string fullPath)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
		}
	}

	public void SaveToFile()
	{
		int num = 0;
		while (true)
		{
			try
			{
				if (!string.IsNullOrEmpty(_settingsFile))
				{
					string text = _settingsFile + ".tmp";
					if (File.Exists(text))
					{
						File.Delete(text);
					}
					CreateDirectoryIfItDoesNotExist(Path.GetDirectoryName(text));
					using (FileStream arg = new FileStream(text, FileMode.Create, FileAccess.ReadWrite))
					{
						XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
						xmlWriterSettings.Indent = true;
						XmlWriter writer = XmlWriterFactory(arg, xmlWriterSettings);
						SaveToStream(writer);
					}
					if (File.Exists(_settingsFile))
					{
						File.Delete(_settingsFile);
					}
					File.Move(text, _settingsFile);
				}
				break;
			}
			catch
			{
				if (num < 6)
				{
					Thread.Sleep(50);
					num++;
					continue;
				}
				throw;
			}
		}
	}

	public void SaveToStream(XmlWriter writer)
	{
		string text = ((AuthenticationType == AuthenticationType.WindowsDefault) ? string.Empty : UserName);
		string text2 = ((AuthenticationType == AuthenticationType.WindowsDefault) ? string.Empty : EncryptedPassword);
		writer.WriteStartElement("login");
		writer.WriteStartElement("lastlogin");
		writer.WriteElementString("servertype", _serverTypeId.ToString());
		writer.WriteElementString("serveraddress", ServerAddress.ToString());
		writer.WriteElementString("authenticationtype", Enum.GetName(typeof(AuthenticationType), AuthenticationType));
		writer.WriteElementString("username", text);
		writer.WriteElementString("rememberpassword", PersistPassword.ToString());
		writer.WriteElementString("autologin", AutoLogin.ToString());
		writer.WriteElementString("showrememberpassword", ShowRememberPassword.ToString());
		writer.WriteElementString("showautologin", ShowAutoLogin.ToString());
		writer.WriteEndElement();
		AllowUnsecureUserOptionEntry allowUnsecureUserOptionEntry = _allowUnsecureUserOptions.Find((AllowUnsecureUserOptionEntry x) => x.Username.Equals(UserName, StringComparison.InvariantCultureIgnoreCase));
		if (allowUnsecureUserOptionEntry == null)
		{
			_allowUnsecureUserOptions.Add(new AllowUnsecureUserOptionEntry(UserName, AllowUnsecureConnection));
		}
		else
		{
			allowUnsecureUserOptionEntry.AllowUnsecureConnection = AllowUnsecureConnection;
		}
		if (_allowUnsecureUserOptions.Any((AllowUnsecureUserOptionEntry x) => x.AllowUnsecureConnection))
		{
			writer.WriteStartElement("allowunsecureconnections");
			foreach (AllowUnsecureUserOptionEntry allowUnsecureUserOption in _allowUnsecureUserOptions)
			{
				if (allowUnsecureUserOption.AllowUnsecureConnection)
				{
					writer.WriteStartElement("user");
					writer.WriteAttributeString("username", allowUnsecureUserOption.Username);
					writer.WriteAttributeString("allowunsecure", allowUnsecureUserOption.AllowUnsecureConnection.ToString(CultureInfo.InvariantCulture));
					writer.WriteEndElement();
				}
			}
			writer.WriteEndElement();
		}
		writer.WriteStartElement("historyaddress");
		ServerAddressHistoryEntry serverAddressHistoryEntry = _serverAddressHistory.Find((ServerAddressHistoryEntry s) => s.ServerAddress.Equals(ServerAddress.ToString(), StringComparison.InvariantCultureIgnoreCase));
		if (serverAddressHistoryEntry == null)
		{
			serverAddressHistoryEntry = new ServerAddressHistoryEntry(ServerAddress.ToString());
			_serverAddressHistory.Add(serverAddressHistoryEntry);
		}
		foreach (ServerAddressHistoryEntry item in _serverAddressHistory)
		{
			writer.WriteStartElement("entry");
			writer.WriteString(item.ServerAddress);
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
		writer.WriteStartElement("historyusername");
		if (AuthenticationType != AuthenticationType.WindowsDefault && !_userNameHistory.ContainsKey(text))
		{
			_userNameHistory.Add(text, text2);
		}
		foreach (string key in _userNameHistory.Keys)
		{
			writer.WriteStartElement("entry");
			writer.WriteAttributeString("username", key);
			string value = string.Empty;
			if (key.Equals(text))
			{
				if (PersistPassword)
				{
					value = text2;
				}
			}
			else
			{
				_userNameHistory.TryGetValue(key, out value);
			}
			if (AuthenticationType == AuthenticationType.WindowsDefault)
			{
				_userNameHistory.TryGetValue(key, out value);
			}
			writer.WriteAttributeString("password", string.Empty);
			writer.WriteAttributeString("password2", value);
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
		writer.WriteEndElement();
		writer.WriteEndDocument();
		writer.Flush();
		writer.Close();
	}

	private void UpdateAllowUnsecureConnectionPreference()
	{
		AllowUnsecureConnection = false;
		AllowUnsecureUserOptionEntry allowUnsecureUserOptionEntry = _allowUnsecureUserOptions.Find((AllowUnsecureUserOptionEntry x) => x.Username.Equals(UserName, StringComparison.InvariantCultureIgnoreCase));
		if (allowUnsecureUserOptionEntry != null)
		{
			AllowUnsecureConnection = allowUnsecureUserOptionEntry.AllowUnsecureConnection;
		}
	}

	private void SetDefaults()
	{
		_serverTypeId = Guid.Empty;
		ServerAddress = GetHostNameForLocalHost();
		AuthenticationType = AuthenticationType.WindowsDefault;
		AllowUnsecureConnection = false;
		try
		{
			UserName = WindowsIdentity.GetCurrent().Name;
		}
		catch (SecurityException)
		{
			UserName = string.Empty;
		}
		ShowRememberPassword = true;
		ShowAutoLogin = true;
		EncryptedPassword = string.Empty;
	}

	private static Uri GetHostNameForLocalHost()
	{
		string hostName;
		try
		{
			hostName = Dns.GetHostEntry("localhost").HostName;
		}
		catch (SocketException)
		{
			try
			{
				hostName = Environment.MachineName;
			}
			catch (InvalidOperationException)
			{
				hostName = "localhost";
			}
		}
		return new UriBuilder(Uri.UriSchemeHttp, hostName).Uri;
	}

	public DialogLoginControls ValidateAndCollect()
	{
		if (ServerAddress == null)
		{
			return DialogLoginControls.ServerAddress;
		}
		if (AuthenticationType != AuthenticationType.WindowsDefault)
		{
			if (UserName == null)
			{
				return DialogLoginControls.UserName;
			}
			if (EncryptedPassword == null)
			{
				return DialogLoginControls.Password;
			}
		}
		else
		{
			UserName = WindowsIdentity.GetCurrent().Name;
			Password = string.Empty;
		}
		return DialogLoginControls.None;
	}

	public static bool IsUnsecureConnectionAllowedForUser(string historyFile, string username)
	{
		bool result = false;
		if (File.Exists(historyFile))
		{
			try
			{
				using FileStream arg = new FileStream(historyFile, FileMode.Open, FileAccess.Read);
				XmlReader reader = XmlReaderFactory(arg);
				XmlDocument xmlDocument = XmlDocumentFactory();
				xmlDocument.Load(reader);
				XmlNode userNode = GetUserNode(xmlDocument, username, createIfNeeded: false);
				if (userNode != null)
				{
					XmlAttribute xmlAttribute = userNode.Attributes["allowunsecure"];
					if (xmlAttribute != null && bool.TryParse(xmlAttribute.Value, out var result2) && result2)
					{
						result = true;
					}
				}
			}
			catch
			{
			}
		}
		return result;
	}

	public static void ClearUnsecureConnectionAllowedForUser(string historyFile, string username)
	{
		if (!File.Exists(historyFile))
		{
			return;
		}
		try
		{
			XmlDocument xmlDocument = XmlDocumentFactory();
			using (FileStream arg = new FileStream(historyFile, FileMode.Open, FileAccess.Read))
			{
				using XmlReader reader = XmlReaderFactory(arg);
				xmlDocument.Load(reader);
			}
			XmlNode userNode = GetUserNode(xmlDocument, username, createIfNeeded: false);
			if (userNode == null)
			{
				return;
			}
			XmlNode parentNode = userNode.ParentNode;
			parentNode.RemoveChild(userNode);
			if (!parentNode.HasChildNodes)
			{
				parentNode.ParentNode.RemoveChild(parentNode);
			}
			using FileStream arg2 = new FileStream(historyFile, FileMode.Create, FileAccess.ReadWrite);
			using XmlWriter w = XmlWriterFactory(arg2, new XmlWriterSettings
			{
				Indent = true
			});
			xmlDocument.Save(w);
		}
		catch
		{
		}
	}

	public static void SetUnsecureConnectionAllowedForUser(string historyFile, string username)
	{
		try
		{
			XmlDocument xmlDocument = XmlDocumentFactory();
			if (File.Exists(historyFile))
			{
				using FileStream arg = new FileStream(historyFile, FileMode.Open, FileAccess.Read);
				using XmlReader reader = XmlReaderFactory(arg);
				xmlDocument.Load(reader);
			}
			XmlNode userNode = GetUserNode(xmlDocument, username, createIfNeeded: true);
			if (userNode == null)
			{
				return;
			}
			XmlAttribute xmlAttribute = userNode.Attributes["allowunsecure"];
			if (xmlAttribute == null)
			{
				xmlAttribute = xmlDocument.CreateAttribute("allowunsecure");
				userNode.Attributes.Append(xmlAttribute);
			}
			if (!(xmlAttribute.Value != true.ToString(CultureInfo.InvariantCulture)))
			{
				return;
			}
			xmlAttribute.Value = true.ToString(CultureInfo.InvariantCulture);
			using FileStream arg2 = new FileStream(historyFile, FileMode.Create, FileAccess.ReadWrite);
			using XmlWriter w = XmlWriterFactory(arg2, new XmlWriterSettings
			{
				Indent = true
			});
			xmlDocument.Save(w);
		}
		catch
		{
		}
	}

	private static XmlNode GetUserNode(XmlDocument xmlDocument, string username, bool createIfNeeded)
	{
		XmlNode xmlNode = xmlDocument.SelectSingleNode("login");
		if (xmlNode == null)
		{
			if (!createIfNeeded)
			{
				return null;
			}
			xmlNode = xmlDocument.CreateElement("login");
			xmlDocument.AppendChild(xmlNode);
		}
		XmlNode xmlNode2 = xmlNode.SelectSingleNode("allowunsecureconnections");
		if (xmlNode2 == null)
		{
			if (!createIfNeeded)
			{
				return null;
			}
			xmlNode2 = xmlDocument.CreateElement("allowunsecureconnections");
			xmlNode.AppendChild(xmlNode2);
		}
		XmlNode xmlNode3 = null;
		XmlNodeList xmlNodeList = xmlNode2.SelectNodes("user");
		if (xmlNodeList != null)
		{
			foreach (XmlNode item in xmlNodeList)
			{
				XmlAttribute xmlAttribute = item.Attributes["username"];
				if (xmlAttribute != null && xmlAttribute.Value.Equals(username, StringComparison.InvariantCultureIgnoreCase))
				{
					xmlNode3 = item;
					break;
				}
			}
		}
		if (xmlNode3 == null)
		{
			if (!createIfNeeded)
			{
				return null;
			}
			xmlNode3 = xmlDocument.CreateElement("user");
			XmlAttribute xmlAttribute2 = xmlDocument.CreateAttribute("username");
			xmlAttribute2.Value = username;
			xmlNode3.Attributes.Append(xmlAttribute2);
			xmlNode2.AppendChild(xmlNode3);
		}
		return xmlNode3;
	}
}
