using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MethodIdAddOrEditRegistrationNumbersInfoServerTask : ServerTask
{
	public ICollection<ICollection<string>> RegistrationNumbersWithCustomFields
	{
		get
		{
			return LprMatchList.CollectionToListOfLists(RegistrationNumbers);
		}
		set
		{
			RegistrationNumbers = LprMatchList.ListOfListsToCollection(value);
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

	public MethodIdAddOrEditRegistrationNumbersInfoServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("MethodIdAddOrEditRegistrationNumbersInfo"));
	}
}
