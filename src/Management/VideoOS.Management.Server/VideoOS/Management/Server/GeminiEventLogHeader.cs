using System.Globalization;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public class GeminiEventLogHeader
{
	private string _assemblyName;

	private string _messageBrief;

	public string MessageDetails { get; set; }

	public string AssemblyName
	{
		get
		{
			if (!string.IsNullOrEmpty(_assemblyName))
			{
				return _assemblyName;
			}
			return AssemblyDisplayName(Assembly.GetCallingAssembly());
		}
		set
		{
			_assemblyName = value;
		}
	}

	public string MessageBrief
	{
		get
		{
			if (!string.IsNullOrEmpty(_messageBrief))
			{
				return _messageBrief;
			}
			return Resources.MessageBrief;
		}
		set
		{
			_messageBrief = value;
		}
	}

	public GeminiEventLogHeader(string messageDetails, string assemblyName)
	{
		MessageDetails = messageDetails;
		AssemblyName = assemblyName;
	}

	public static string AssemblyDisplayName(Assembly assembly)
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}; {1}", assembly.FullName, assembly.ImageRuntimeVersion);
	}

	public static string IdentityDisplayName(IIdentity identity)
	{
		if (identity == null)
		{
			return Resources.NotAvailable;
		}
		return identity.Name;
	}

	public static string AuthenticationType(IIdentity identity)
	{
		if (identity == null)
		{
			return Resources.NotAvailable;
		}
		return identity.AuthenticationType;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(MessageBrief);
		stringBuilder.Append(MessageDetails);
		stringBuilder.Append(AssemblyName);
		return stringBuilder.ToString();
	}
}
