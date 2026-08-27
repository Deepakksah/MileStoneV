using System.Security.Principal;
using System.ServiceProcess;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Common.ServiceAccountHelper;

public class ServiceAccountItem
{
	private string _serviceAccountString;

	private ServiceAccount _serviceAccount;

	public ServiceAccount ServiceAccount => _serviceAccount;

	public string AccountName
	{
		get
		{
			WellKnownSidType sidType;
			switch (_serviceAccount)
			{
			case ServiceAccount.LocalService:
				sidType = WellKnownSidType.LocalServiceSid;
				break;
			case ServiceAccount.LocalSystem:
				sidType = WellKnownSidType.LocalSystemSid;
				break;
			case ServiceAccount.NetworkService:
				sidType = WellKnownSidType.NetworkServiceSid;
				break;
			default:
				return _serviceAccountString;
			}
			NTAccount nTAccount = (NTAccount)new SecurityIdentifier(sidType, null).Translate(typeof(NTAccount));
			LogManager.Write("Selected account was translated from " + _serviceAccount.ToString() + " to " + nTAccount.Value);
			return nTAccount.Value;
		}
	}

	public string NonLocalisedAccountName => _serviceAccount switch
	{
		ServiceAccount.LocalService => "NT AUTHORITY\\LocalService", 
		ServiceAccount.LocalSystem => ".\\LocalSystem", 
		ServiceAccount.NetworkService => "NT AUTHORITY\\NetworkService", 
		_ => _serviceAccountString, 
	};

	public override string ToString()
	{
		return _serviceAccountString;
	}

	public ServiceAccountItem(string text, ServiceAccount serviceAccount)
	{
		_serviceAccountString = text;
		_serviceAccount = serviceAccount;
	}

	public ServiceAccountItem(string name, out bool canBeLoc)
	{
		_serviceAccountString = name;
		canBeLoc = true;
		if (name.ToLower().Contains("Network Service".ToLower()))
		{
			_serviceAccount = ServiceAccount.NetworkService;
		}
		else if (name.ToLower().Contains("Local Service".ToLower()))
		{
			_serviceAccount = ServiceAccount.LocalService;
		}
		else if (name.ToLower().Contains("Local System".ToLower()))
		{
			_serviceAccount = ServiceAccount.LocalSystem;
		}
		else
		{
			canBeLoc = false;
		}
	}
}
