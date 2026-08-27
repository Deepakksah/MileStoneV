using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Platform;

namespace VideoOS.Mobile.Management;

public class SurveillanceServer
{
	public delegate UserTypes GetUserType();

	public const int BasicUserManagementFlagVersion = 21;

	public const int ExternalUsersFlagVersionMajor = 22;

	public const int ExternalUsersFlagVersionMinor = 3;

	public const int ShareServiceFlagVersion = 23;

	public static readonly SurveillanceServer Instance = new SurveillanceServer();

	public const string ProductNameKey = "ProductName";

	public const string ServerSubTypeKey = "ServerSubType";

	public const string ServerTypeKey = "ServerType";

	public const string ServerVersionKey = "ServerVersion";

	private readonly Regex _rx = Version.VersionStringFilter;

	private Dictionary<string, SurveillanceType> _mappingsServerSubType;

	private Dictionary<string, SurveillanceType> _mappingsServerType;

	private Dictionary<SurveillanceType, SurveillanceCodeBase> _mappingsCodeBase;

	private Dictionary<SurveillanceType, GetUserType> _mappingsUserTypes;

	public const int FreeEssentialsMinMajorVersion = 10;

	public const int FreeEssentialsMinMinorVersion = 2;

	public string ProductName { get; private set; }

	public string ServerSubType { get; private set; }

	public string VersionString { get; private set; }

	public SurveillanceType Type { get; private set; }

	public SurveillanceCodeBase CodeBase { get; private set; }

	public Version Version { get; private set; }

	public bool IsECodebase => CodeBase == SurveillanceCodeBase.XPE;

	public static void Initialize(Item site)
	{
		Instance.InitializeFromSiteItem(site);
	}

	public static void Initialize(FQID fqid)
	{
		Instance.InitilizeFromServerFqid(fqid);
	}

	private SurveillanceServer()
	{
		Version = new Version();
	}

	public void InitializeFromSiteItem(Item site)
	{
		if (site == null)
		{
			return;
		}
		try
		{
			ProductName = site.Properties["ProductName"];
			MatchCollection matchCollection = _rx.Matches(ProductName);
			if (matchCollection.Count > 0)
			{
				VersionString = matchCollection[0].Value;
			}
		}
		catch
		{
		}
		try
		{
			if (site.Properties.ContainsKey("ServerSubType"))
			{
				ServerSubType = site.Properties["ServerSubType"];
			}
			else if (site.Properties.ContainsKey("ServerType"))
			{
				ServerSubType = site.Properties["ServerType"];
			}
		}
		catch
		{
		}
		try
		{
			VersionString = site.Properties["ServerVersion"];
			MatchCollection matchCollection2 = _rx.Matches(VersionString);
			if (matchCollection2.Count > 0)
			{
				VersionString = matchCollection2[0].Value;
			}
		}
		catch
		{
		}
		try
		{
			Version.InitFromVersionString(VersionString);
		}
		catch
		{
		}
		if (Type == SurveillanceType.Unknown)
		{
			Type = GetTypeFromServerSubType(ServerSubType);
		}
		InitilizeFromServerFqid(site.FQID);
		InitilizeCodeBase(Type);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Server sub type: ").Append(ServerSubType);
		stringBuilder.Append(", Code base: ").Append((int)CodeBase);
		stringBuilder.Append(", Version string: ").Append(VersionString);
		Log.Info("InitializeFromSiteItem", stringBuilder.ToString());
		if ((object)site.FQID?.ServerId != null)
		{
			Log.Debug("InitializeFromSiteItem", "Server inner type: " + site.FQID.ServerId.ServerType);
		}
	}

	public SurveillanceType GetTypeFromMappings(string key, Dictionary<string, SurveillanceType> mappings)
	{
		SurveillanceType result = SurveillanceType.Unknown;
		if (key != null)
		{
			foreach (KeyValuePair<string, SurveillanceType> mapping in mappings)
			{
				if (key.ToLower().Contains(mapping.Key.ToLower()))
				{
					result = mapping.Value;
					break;
				}
			}
		}
		return result;
	}

	public Dictionary<string, SurveillanceType> GetMappingsServerSubType()
	{
		if (_mappingsServerSubType == null)
		{
			_mappingsServerSubType = new Dictionary<string, SurveillanceType>();
			_mappingsServerSubType.Add("100", SurveillanceType.XProtectGo);
			_mappingsServerSubType.Add("190", SurveillanceType.XProtectEssential);
			_mappingsServerSubType.Add("200", SurveillanceType.XProtectEssential);
			_mappingsServerSubType.Add("250", SurveillanceType.XProtectExpress);
			_mappingsServerSubType.Add("300", SurveillanceType.XProtectProfessional);
			_mappingsServerSubType.Add("400", SurveillanceType.XProtectEnterprise);
			_mappingsServerSubType.Add("440", SurveillanceType.XProtectEssentialPlus);
			_mappingsServerSubType.Add("480", SurveillanceType.XProtectProfessionalPlus);
			_mappingsServerSubType.Add("500", SurveillanceType.XProtectExpert);
			_mappingsServerSubType.Add("600", SurveillanceType.XProtectCorporate);
		}
		return _mappingsServerSubType;
	}

	public SurveillanceType GetTypeFromServerSubType(string serverSubType)
	{
		return GetTypeFromMappings(serverSubType, GetMappingsServerSubType());
	}

	public void InitilizeFromServerFqid(FQID fqid)
	{
		if ((object)fqid != null && (object)fqid.ServerId != null && Type == SurveillanceType.Unknown)
		{
			Type = GetTypeFromServerType(fqid.ServerId.ServerType);
		}
	}

	public Dictionary<string, SurveillanceType> GetMappingsServerType()
	{
		if (_mappingsServerType == null)
		{
			_mappingsServerType = new Dictionary<string, SurveillanceType>();
			_mappingsServerType.Add("XPE", SurveillanceType.XProtectEnterprise);
			_mappingsServerType.Add("XPCO", SurveillanceType.XProtectCorporate);
		}
		return _mappingsServerType;
	}

	public SurveillanceType GetTypeFromServerType(string serverType)
	{
		return GetTypeFromMappings(serverType, GetMappingsServerType());
	}

	public Dictionary<SurveillanceType, SurveillanceCodeBase> GetMappingsCodeBase()
	{
		if (_mappingsCodeBase == null)
		{
			_mappingsCodeBase = new Dictionary<SurveillanceType, SurveillanceCodeBase>();
			_mappingsCodeBase.Add(SurveillanceType.XProtectGo, SurveillanceCodeBase.XPE);
			_mappingsCodeBase.Add(SurveillanceType.XProtectBasisPlus, SurveillanceCodeBase.XPE);
			_mappingsCodeBase.Add(SurveillanceType.XProtectEssential, SurveillanceCodeBase.XPE);
			_mappingsCodeBase.Add(SurveillanceType.XProtectNvr, SurveillanceCodeBase.XPE);
			_mappingsCodeBase.Add(SurveillanceType.XProtectExpress, SurveillanceCodeBase.XPE);
			_mappingsCodeBase.Add(SurveillanceType.XProtectProfessional, SurveillanceCodeBase.XPE);
			_mappingsCodeBase.Add(SurveillanceType.XProtectEnterprise, SurveillanceCodeBase.XPE);
			_mappingsCodeBase.Add(SurveillanceType.XProtectEssentialPlus, SurveillanceCodeBase.XPCo);
			_mappingsCodeBase.Add(SurveillanceType.XProtectProfessionalPlus, SurveillanceCodeBase.XPCo);
			_mappingsCodeBase.Add(SurveillanceType.XProtectExpert, SurveillanceCodeBase.XPCo);
			_mappingsCodeBase.Add(SurveillanceType.XProtectCorporate, SurveillanceCodeBase.XPCo);
		}
		return _mappingsCodeBase;
	}

	private void InitilizeCodeBase(SurveillanceType surveillanceType)
	{
		if (CodeBase == SurveillanceCodeBase.Unknown)
		{
			CodeBase = GetCodeBaseFromType(surveillanceType, GetMappingsCodeBase());
		}
	}

	public SurveillanceCodeBase GetCodeBaseFromType(SurveillanceType key, Dictionary<SurveillanceType, SurveillanceCodeBase> mappings)
	{
		SurveillanceCodeBase result = SurveillanceCodeBase.Unknown;
		if (mappings.ContainsKey(key))
		{
			result = mappings[key];
		}
		return result;
	}

	public Dictionary<SurveillanceType, GetUserType> GetMappingsUserTypes()
	{
		if (_mappingsUserTypes == null)
		{
			_mappingsUserTypes = new Dictionary<SurveillanceType, GetUserType>();
			UserTypes basicOnly = new UserTypes().AddUserTypeE(UserType.Basic);
			UserTypes tADOnly = new UserTypes().AddUserTypeE(UserType.ActiveDirectory);
			UserTypes both = new UserTypes().AddUserTypeE(UserType.ActiveDirectory).AddUserTypeE(UserType.Basic);
			_mappingsUserTypes.Add(SurveillanceType.XProtectEnterprise, () => both);
			_mappingsUserTypes.Add(SurveillanceType.XProtectProfessional, () => both);
			_mappingsUserTypes.Add(SurveillanceType.XProtectEssential, () => both);
			_mappingsUserTypes.Add(SurveillanceType.XProtectNvr, () => both);
			_mappingsUserTypes.Add(SurveillanceType.XProtectExpress, () => both);
			_mappingsUserTypes.Add(SurveillanceType.XProtectBasisPlus, () => basicOnly);
			_mappingsUserTypes.Add(SurveillanceType.XProtectGo, () => basicOnly);
			_mappingsUserTypes.Add(SurveillanceType.XProtectCorporate, () => (Version.Major < 6) ? tADOnly : both);
			_mappingsUserTypes.Add(SurveillanceType.XProtectExpert, () => both);
			_mappingsUserTypes.Add(SurveillanceType.XProtectEssentialPlus, () => both);
			_mappingsUserTypes.Add(SurveillanceType.XProtectProfessionalPlus, () => both);
		}
		return _mappingsUserTypes;
	}

	public UserTypes GetUserTypes()
	{
		Dictionary<SurveillanceType, GetUserType> mappingsUserTypes = GetMappingsUserTypes();
		UserTypes result = new UserTypes().AddUserTypeE(UserType.ActiveDirectory);
		try
		{
			result = mappingsUserTypes[Type]();
		}
		catch (Exception ex)
		{
			Log.Error("SurveillanceServer.GetUserTypes()", ex.Message);
		}
		return result;
	}

	public static bool IsFreeSurveillance(SurveillanceServer server)
	{
		if (SurveillanceType.XProtectGo == server.Type)
		{
			return true;
		}
		if ((SurveillanceType.XProtectEssential == server.Type || SurveillanceType.XProtectEssentialPlus == server.Type) && (server.Version.Major > 10 || (server.Version.Major == 10 && server.Version.Minor >= 2)))
		{
			return true;
		}
		return false;
	}

	public string PrintValues()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Product Name: ").AppendLine(ProductName);
		stringBuilder.Append("Server Sub Type: ").AppendLine(ServerSubType);
		stringBuilder.Append("Version String: ").AppendLine(VersionString);
		stringBuilder.AppendLine($"Version: {Version.Major}.{Version.Minor}.{Version.Build}.{Version.Revision}");
		stringBuilder.Append("Server Type: ").AppendLine(Type.ToString());
		return stringBuilder.ToString();
	}
}
