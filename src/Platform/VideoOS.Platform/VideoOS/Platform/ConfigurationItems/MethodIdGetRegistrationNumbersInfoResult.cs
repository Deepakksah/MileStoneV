using System.Collections.Generic;
using System.Collections.ObjectModel;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

public sealed class MethodIdGetRegistrationNumbersInfoResult : ServerTaskResult
{
	public ICollection<ICollection<string>> RegistrationNumbersWithCustomFields { get; private set; }

	public MethodIdGetRegistrationNumbersInfoResult(ServerTask serverTask)
		: base(serverTask)
	{
		if (base.Success)
		{
			string property = base.ServerTask.GetProperty("RegistrationNumbers");
			if (property != null)
			{
				Collection<string> collection = ConfigurationApiUtil.SeparatorStringToCollection(property);
				RegistrationNumbersWithCustomFields = LprMatchList.CollectionToListOfLists(collection);
			}
		}
	}
}
