using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MethodIdDeleteRegistrationNumbersServerTask : ServerTask
{
	public ICollection<string> RegistrationNumbersList
	{
		get
		{
			return RegistrationNumbers.Select(ConfigurationApiUtil.Unescape).ToList();
		}
		set
		{
			RegistrationNumbers = new Collection<string>(value.Select(ConfigurationApiUtil.Escape).ToList());
		}
	}

	public Collection<string> RegistrationNumbers
	{
		get
		{
			return ConfigurationApiUtil.SeparatorStringToCollection(base.Wrapper.GetPropertyValue("RegistrationNumbers"));
		}
		set
		{
			base.Wrapper.SetPropertyValue("RegistrationNumbers", ConfigurationApiUtil.ToSeparatorString(value));
			base.Wrapper.IsDirty = true;
		}
	}

	public MethodIdDeleteRegistrationNumbersServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("MethodIdDeleteRegistrationNumbers"));
	}
}
