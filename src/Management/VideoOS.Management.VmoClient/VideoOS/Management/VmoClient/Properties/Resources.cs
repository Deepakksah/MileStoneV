using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Management.VmoClient.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("VideoOS.Management.VmoClient.Properties.Resources", typeof(Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string ArgumentExceptionMustBeServerVersion => ResourceManager.GetString("ArgumentExceptionMustBeServerVersion", resourceCulture);

	public static string ArgumentOutOfRangeVersion => ResourceManager.GetString("ArgumentOutOfRangeVersion", resourceCulture);

	public static string ConnectionCannotBeChanged => ResourceManager.GetString("ConnectionCannotBeChanged", resourceCulture);

	public static string ConnectionFailureException => ResourceManager.GetString("ConnectionFailureException", resourceCulture);

	public static string DisplayVersionString => ResourceManager.GetString("DisplayVersionString", resourceCulture);

	public static string InvalidArgumentValue => ResourceManager.GetString("InvalidArgumentValue", resourceCulture);

	public static string InvalidPropertyValue => ResourceManager.GetString("InvalidPropertyValue", resourceCulture);

	public static string InvalidPropertyValueReasonIntAboveMax => ResourceManager.GetString("InvalidPropertyValueReasonIntAboveMax", resourceCulture);

	public static string InvalidPropertyValueReasonIntBelowMin => ResourceManager.GetString("InvalidPropertyValueReasonIntBelowMin", resourceCulture);

	public static string InvalidPropertyValueReasonString => ResourceManager.GetString("InvalidPropertyValueReasonString", resourceCulture);

	public static string OperationTimeout => ResourceManager.GetString("OperationTimeout", resourceCulture);

	public static string PropertyReadOnlyException => ResourceManager.GetString("PropertyReadOnlyException", resourceCulture);

	public static string SystemMonitorThresholds => ResourceManager.GetString("SystemMonitorThresholds", resourceCulture);

	public static string UserChangePasswordException => ResourceManager.GetString("UserChangePasswordException", resourceCulture);

	public static string UserDoesNotExist => ResourceManager.GetString("UserDoesNotExist", resourceCulture);

	public static string UserLockoutException => ResourceManager.GetString("UserLockoutException", resourceCulture);

	public static string UsernamePasswordException => ResourceManager.GetString("UsernamePasswordException", resourceCulture);

	public static string ZeroVersionString => ResourceManager.GetString("ZeroVersionString", resourceCulture);

	public Resources()
	{
	}
}
