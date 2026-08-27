using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

public sealed class MethodIdGetCustomFieldsForRegistrationNumberResult : ServerTaskResult
{
	public ICollection<string> CustomFields { get; private set; }

	public MethodIdGetCustomFieldsForRegistrationNumberResult(ServerTask serverTask)
		: base(serverTask)
	{
		if (base.Success)
		{
			string property = base.ServerTask.GetProperty("CustomFields");
			if (property != null)
			{
				Collection<string> source = ConfigurationApiUtil.SeparatorStringToCollection(property);
				CustomFields = source.Select(ConfigurationApiUtil.Unescape).ToList();
			}
		}
	}
}
