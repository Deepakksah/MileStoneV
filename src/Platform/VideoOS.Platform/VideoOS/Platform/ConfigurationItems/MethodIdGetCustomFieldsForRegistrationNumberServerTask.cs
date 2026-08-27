using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MethodIdGetCustomFieldsForRegistrationNumberServerTask : ServerTask
{
	public string RegistrationNumber
	{
		get
		{
			return base.Wrapper.GetPropertyValue("RegistrationNumber");
		}
		set
		{
			base.Wrapper.SetPropertyValue("RegistrationNumber", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public MethodIdGetCustomFieldsForRegistrationNumberResult ExecuteWithResult()
	{
		return new MethodIdGetCustomFieldsForRegistrationNumberResult(Execute());
	}

	public MethodIdGetCustomFieldsForRegistrationNumberServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("MethodIdGetCustomFieldsForRegistrationNumber"));
	}
}
