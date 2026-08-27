using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace VideoOS.Platform;

[Serializable]
[DataContract]
public class ServerId
{
	[DataMember(Name = "Type", Order = 1, EmitDefaultValue = false)]
	public string ServerType;

	[DataMember(Name = "Scheme", Order = 5, EmitDefaultValue = false)]
	public string ServerScheme;

	[DataMember(Name = "Hostname", Order = 2, EmitDefaultValue = false)]
	public string ServerHostname;

	[DataMember(Name = "Port", Order = 3, EmitDefaultValue = false)]
	public int Serverport;

	[DataMember(Name = "Id", Order = 4)]
	public Guid Id;

	[NonSerialized]
	private UserContext _userContext;

	public const string DefaultServerType = "XP";

	[Obsolete("This server type is deprecated")]
	public const string EnterpriseServerType = "XPE";

	public const string CorporateManagementServerType = "XPCO";

	public const string CorporateRecordingServerType = "XPCORS";

	public const string LprServerType = "LPR Server";

	[Obsolete("This server type is deprecated")]
	public const string ArcusServerType = "ARCUS";

	[Obsolete("This server type is deprecated")]
	public const string MobileServerType = "MOBILE";

	public const string BlkExportType = "BLK";

	public const string PqzExportType = "PQZ";

	public const string IniExportType = "INI";

	[XmlIgnore]
	[IgnoreDataMember]
	public UserContext UserContext
	{
		get
		{
			return _userContext;
		}
		set
		{
			_userContext = value;
		}
	}

	public Uri Uri => ((ServerScheme == Uri.UriSchemeFile) ? new UriBuilder
	{
		Scheme = Uri.UriSchemeFile,
		Host = "",
		Path = ServerHostname
	} : new UriBuilder(ServerScheme, ServerHostname, Serverport)).Uri;

	public bool IsExportType
	{
		get
		{
			if (!(ServerType == "BLK") && !(ServerType == "PQZ"))
			{
				return ServerType == "INI";
			}
			return true;
		}
	}

	public ServerId()
	{
		ServerScheme = Uri.UriSchemeHttp;
	}

	public ServerId(string type, string hostname, int port, Guid id)
	{
		ServerType = type;
		ServerHostname = hostname;
		Serverport = port;
		ServerScheme = Uri.UriSchemeHttp;
		SetupDefaultPortHttpScheme();
		Id = id;
	}

	public ServerId(string type, string hostname, string port, Guid id)
	{
		ServerType = type;
		ServerHostname = hostname;
		ServerScheme = Uri.UriSchemeHttp;
		int.TryParse(port, out Serverport);
		SetupDefaultPortHttpScheme();
		Id = id;
	}

	public ServerId(string type, string scheme, string hostname, int port, Guid id)
	{
		ServerType = type;
		ServerScheme = scheme;
		ServerHostname = hostname;
		Serverport = port;
		SetupDefaultPortHttpScheme();
		Id = id;
	}

	public ServerId(string type, string scheme, string hostname, string port, Guid id)
	{
		ServerType = type;
		ServerScheme = scheme;
		ServerHostname = hostname;
		if (!int.TryParse(port, out Serverport))
		{
			Serverport = 0;
		}
		SetupDefaultPortHttpScheme();
		Id = id;
	}

	[OnDeserializing]
	private void OnDeserializing(StreamingContext context)
	{
		ServerScheme = Uri.UriSchemeHttp;
		SetupDefaultPortHttpScheme();
	}

	public ServerId Clone()
	{
		return new ServerId(ServerType, ServerScheme, ServerHostname, Serverport, Id);
	}

	private void SetupDefaultPortHttpScheme()
	{
		if (Serverport == 443)
		{
			ServerScheme = Uri.UriSchemeHttps;
		}
		else if (Serverport == 80)
		{
			ServerScheme = Uri.UriSchemeHttp;
		}
	}

	public override int GetHashCode()
	{
		return ServerHostname.GetHashCode() * Id.GetHashCode();
	}

	public override bool Equals(object otherObject)
	{
		ServerId serverId = otherObject as ServerId;
		if (serverId == null)
		{
			return false;
		}
		if (Id != serverId.Id)
		{
			return false;
		}
		if (Serverport != serverId.Serverport && Serverport != 0 && serverId.Serverport != 0)
		{
			return false;
		}
		if (Serverport != serverId.Serverport && ((Serverport == 0 && serverId.Serverport != 80) || (Serverport != 80 && serverId.Serverport == 0)))
		{
			return false;
		}
		if (NormalizedHost(ServerHostname) != NormalizedHost(serverId.ServerHostname))
		{
			return false;
		}
		return true;
	}

	public static bool operator ==(ServerId obj1, ServerId obj2)
	{
		if ((object)obj1 == obj2)
		{
			return true;
		}
		if ((object)obj1 == null)
		{
			return false;
		}
		if ((object)obj2 == null)
		{
			return false;
		}
		return obj1.Equals(obj2);
	}

	public static bool operator !=(ServerId obj1, ServerId obj2)
	{
		return !(obj1 == obj2);
	}

	public bool Contains(string match)
	{
		if (match == null)
		{
			return false;
		}
		if (!ServerHostname.Contains(match) && !ServerType.Contains(match))
		{
			return match.Contains(Id.ToString());
		}
		return true;
	}

	private static string NormalizedHost(string hostName)
	{
		if (hostName == null)
		{
			return string.Empty;
		}
		string text = "";
		try
		{
			text = new UriBuilder(hostName).Host;
		}
		catch (Exception)
		{
			EnvironmentManager.Instance.Log(error: false, "ServerId.NormalizedHost", "Unable to UriBuilder for:" + hostName + " (Using default construction)");
			string text2 = hostName.ToLower(CultureInfo.InvariantCulture);
			if (text2.StartsWith("http://"))
			{
				text2 = text2.Substring("http://".Length);
			}
			if (text2.StartsWith("https://"))
			{
				text2 = text2.Substring("https://".Length);
			}
			int num = text2.IndexOf(":", StringComparison.InvariantCulture);
			if (num > 0)
			{
				text2 = text2.Substring(0, num);
			}
			text = text2;
		}
		if (text == "0.0.0.0" || text == "127.0.0.1")
		{
			return "localhost";
		}
		return text;
	}

	public override string ToString()
	{
		return FormattableString.Invariant($"Id: {Id} Uri: {ServerScheme}://{ServerHostname}.{Serverport}");
	}
}
