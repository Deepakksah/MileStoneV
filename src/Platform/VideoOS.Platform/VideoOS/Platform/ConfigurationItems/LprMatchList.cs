using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LprMatchList : IConfigurationItem
{
	public string Id => base.Wrapper.GetPropertyValue("Id");

	public string TriggerEventList
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TriggerEventList");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TriggerEventList", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Collection<string> CustomFields
	{
		get
		{
			return ConfigurationApiUtil.SeparatorStringToCollection(base.Wrapper.GetPropertyValue("CustomFields"));
		}
		set
		{
			base.Wrapper.SetPropertyValue("CustomFields", ConfigurationApiUtil.ToSeparatorString(value));
			base.Wrapper.IsDirty = true;
		}
	}

	public Collection<string> RegistrationNumbers => ConfigurationApiUtil.SeparatorStringToCollection(base.Wrapper.GetPropertyValue("RegistrationNumbers"));

	public ICollection<string> CustomFieldsList
	{
		get
		{
			return CustomFields.Select(ConfigurationApiUtil.Unescape).ToList();
		}
		set
		{
			CustomFields = new Collection<string>(value.Select(ConfigurationApiUtil.Escape).ToList());
		}
	}

	public ICollection<string> RegistrationNumbersList => RegistrationNumbers.Select(ConfigurationApiUtil.Unescape).ToList();

	public LprMatchList(ServerId serverId, ConfigurationItem lprMatchList)
		: base(serverId, lprMatchList)
	{
	}

	public LprMatchList(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LprMatchList(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask MethodIdGetCustomFieldsForRegistrationNumber(string registrationNumber)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("MethodIdGetCustomFieldsForRegistrationNumber");
			configurationApiItemWrapper.SetProperty("RegistrationNumber", registrationNumber);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("MethodIdGetCustomFieldsForRegistrationNumber"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LprMatchList", "Unable to call method 'MethodIdGetCustomFieldsForRegistrationNumber':" + ex.Message);
		}
		return null;
	}

	public MethodIdGetCustomFieldsForRegistrationNumberServerTask MethodIdGetCustomFieldsForRegistrationNumber()
	{
		try
		{
			return new MethodIdGetCustomFieldsForRegistrationNumberServerTask(base.Wrapper.ExecuteMethod("MethodIdGetCustomFieldsForRegistrationNumber"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LprMatchList", "Unable to call method 'MethodIdGetCustomFieldsForRegistrationNumber':" + ex.Message);
		}
		return null;
	}

	public ServerTask MethodIdGetRegistrationNumbersInfo()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("MethodIdGetRegistrationNumbersInfo"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MethodIdGetRegistrationNumbersInfo", "Unable to call method 'MethodIdGetRegistrationNumbersInfo':" + ex.Message);
			return null;
		}
	}

	public ServerTask MethodIdAddOrEditRegistrationNumbersInfo(string registrationNumbers)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("MethodIdAddOrEditRegistrationNumbersInfo");
			configurationApiItemWrapper.SetProperty("RegistrationNumbers", registrationNumbers);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("MethodIdAddOrEditRegistrationNumbersInfo"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LprMatchList", "Unable to call method 'MethodIdAddOrEditRegistrationNumbersInfo':" + ex.Message);
		}
		return null;
	}

	public MethodIdAddOrEditRegistrationNumbersInfoServerTask MethodIdAddOrEditRegistrationNumbersInfo()
	{
		try
		{
			return new MethodIdAddOrEditRegistrationNumbersInfoServerTask(base.Wrapper.ExecuteMethod("MethodIdAddOrEditRegistrationNumbersInfo"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LprMatchList", "Unable to call method 'MethodIdAddOrEditRegistrationNumbersInfo':" + ex.Message);
		}
		return null;
	}

	public ServerTask MethodIdDeleteRegistrationNumbers(string registrationNumbers)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("MethodIdDeleteRegistrationNumbers");
			configurationApiItemWrapper.SetProperty("RegistrationNumbers", registrationNumbers);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("MethodIdDeleteRegistrationNumbers"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LprMatchList", "Unable to call method 'MethodIdDeleteRegistrationNumbers':" + ex.Message);
		}
		return null;
	}

	public MethodIdDeleteRegistrationNumbersServerTask MethodIdDeleteRegistrationNumbers()
	{
		try
		{
			return new MethodIdDeleteRegistrationNumbersServerTask(base.Wrapper.ExecuteMethod("MethodIdDeleteRegistrationNumbers"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LprMatchList", "Unable to call method 'MethodIdDeleteRegistrationNumbers':" + ex.Message);
		}
		return null;
	}

	public ServerTask MethodIdDeleteAllRegistrationNumbers()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("MethodIdDeleteAllRegistrationNumbers"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MethodIdDeleteAllRegistrationNumbers", "Unable to call method 'MethodIdDeleteAllRegistrationNumbers':" + ex.Message);
			return null;
		}
	}

	public MethodIdGetRegistrationNumbersInfoResult MethodIdGetRegistrationNumbersInfoWithResult()
	{
		return new MethodIdGetRegistrationNumbersInfoResult(MethodIdGetRegistrationNumbersInfo());
	}

	public MethodIdGetCustomFieldsForRegistrationNumberResult MethodIdGetCustomFieldsForRegistrationNumberWithResult(string registrationNumber)
	{
		return new MethodIdGetCustomFieldsForRegistrationNumberResult(MethodIdGetCustomFieldsForRegistrationNumber(registrationNumber));
	}

	public ServerTask MethodIdAddOrEditRegistrationNumbersInfo(ICollection<ICollection<string>> registrationNumbers)
	{
		string registrationNumbers2 = ConfigurationApiUtil.ToSeparatorString(ListOfListsToCollection(registrationNumbers));
		return MethodIdAddOrEditRegistrationNumbersInfo(registrationNumbers2);
	}

	public ServerTask MethodIdDeleteRegistrationNumbers(ICollection<string> registrationNumbers)
	{
		string registrationNumbers2 = ConfigurationApiUtil.ToSeparatorString(registrationNumbers);
		return MethodIdDeleteRegistrationNumbers(registrationNumbers2);
	}

	public static ICollection<ICollection<string>> CollectionToListOfLists(Collection<string> collection)
	{
		return ((IEnumerable<string>)collection).Select((Func<string, ICollection<string>>)((string s) => ConfigurationApiUtil.SeparatorStringToList(s, ',').Select(ConfigurationApiUtil.Unescape).ToList())).ToList();
	}

	public static Collection<string> ListOfListsToCollection(ICollection<ICollection<string>> list)
	{
		return new Collection<string>(list.Select((ICollection<string> s) => ConfigurationApiUtil.ToSeparatorString(s.Select(ConfigurationApiUtil.Escape), ',')).ToList());
	}

	[Obsolete("This method is no longer supported. Use the corresponding method on LprMatchListFolder instead.")]
	public ServerTask MethodIdAddLprMatchList(string name)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("MethodIdAddLprMatchList");
			configurationApiItemWrapper.SetProperty("Name", name);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("MethodIdAddLprMatchList"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LprMatchList", "Unable to call method 'MethodIdAddLprMatchList':" + ex.Message);
		}
		return null;
	}

	[Obsolete("This method is no longer supported. Use the corresponding method on LprMatchListFolder instead.")]
	public MethodIdAddLprMatchListServerTask MethodIdAddLprMatchList()
	{
		try
		{
			return new MethodIdAddLprMatchListServerTask(base.Wrapper.ExecuteMethod("MethodIdAddLprMatchList"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LprMatchList", "Unable to call method 'MethodIdAddLprMatchList':" + ex.Message);
		}
		return null;
	}

	[Obsolete("This method is no longer supported. Use the corresponding method on LprMatchListFolder instead.")]
	public ServerTask MethodIdRemoveLprMatchList(string id)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("MethodIdRemoveLprMatchList");
			configurationApiItemWrapper.SetProperty("Id", id);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("MethodIdRemoveLprMatchList"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LprMatchList", "Unable to call method 'MethodIdRemoveLprMatchList':" + ex.Message);
		}
		return null;
	}

	[Obsolete("This method is no longer supported. Use the corresponding method on LprMatchListFolder instead.")]
	public MethodIdRemoveLprMatchListServerTask MethodIdRemoveLprMatchList()
	{
		try
		{
			return new MethodIdRemoveLprMatchListServerTask(base.Wrapper.ExecuteMethod("MethodIdRemoveLprMatchList"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LprMatchList", "Unable to call method 'MethodIdRemoveLprMatchList':" + ex.Message);
		}
		return null;
	}
}
