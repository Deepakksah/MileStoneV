using System;
using Microsoft.Deployment.WindowsInstaller;
using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.Utilities;

public class SessionVariables
{
	private const string DeleteLogsSessionVarName = "DELETELOGS";

	public Session Session { get; private set; }

	public string Path
	{
		get
		{
			return Session.CustomActionData["INSTALLDIR"].SanitizeFolder();
		}
		set
		{
			Session.CustomActionData["INSTALLDIR"] = value;
		}
	}

	public string UriString
	{
		get
		{
			return Session.CustomActionData["URI"].SanitizeUri();
		}
		set
		{
			Session.CustomActionData["URI"] = value;
		}
	}

	public bool DeleteLogs
	{
		get
		{
			return string.Equals(bool.TrueString, Session.CustomActionData["DELETELOGS"], StringComparison.InvariantCultureIgnoreCase);
		}
		set
		{
			Session.CustomActionData["DELETELOGS"] = value.ToString();
		}
	}

	public Guid ServerId
	{
		get
		{
			Guid result = Guid.Empty;
			try
			{
				result = new Guid(Session.CustomActionData["SERVERID"]);
			}
			catch
			{
			}
			return result;
		}
		set
		{
			Session.CustomActionData["SERVERID"] = value.ToString();
		}
	}

	public Guid ServerIdS
	{
		get
		{
			Guid result = Guid.Empty;
			try
			{
				result = new Guid(Session.CustomActionData["SERVERIDS"]);
			}
			catch
			{
			}
			return result;
		}
		set
		{
			Session.CustomActionData["SERVERIDS"] = value.ToString();
		}
	}

	public Guid ServerIdT
	{
		get
		{
			Guid result = Guid.Empty;
			try
			{
				result = new Guid(Session.CustomActionData["SERVERIDT"]);
			}
			catch
			{
			}
			return result;
		}
		set
		{
			Session.CustomActionData["SERVERIDT"] = value.ToString();
		}
	}

	public SessionVariables(Session session)
	{
		Session = session;
	}

	public SessionVariables()
	{
	}
}
