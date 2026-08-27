using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class SaveSearchesFolder : IConfigurationItem
{
	private const string MethodIdAddSaveSearch = "AddSaveSearch";

	private const string MethodIdUpdateSaveSearch = "UpdateSaveSearch";

	private const string MethodIdRemoveSaveSearch = "RemoveSaveSearch";

	private const string MethodIdGetSaveSearch = "GetSaveSearch";

	private const string MethodIdFindSaveSearches = "FindSaveSearches";

	public SaveSearchesFolder(ServerId serverId, ConfigurationItem item)
		: base(serverId, item)
	{
	}

	public SaveSearchesFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public SaveSearchesFolder(FQID fqid)
		: base(fqid)
	{
	}

	public SaveSearchesFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/SaveSearchesFolder")
	{
	}

	public AddSaveSearchServerTask AddSaveSearch(string name, string description, string searchQuery, SaveSearches.SaveSearchAvailability availability, bool includesScopeItems)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddSaveSearch");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("SearchQuery", searchQuery);
			configurationApiItemWrapper.SetProperty("Availability", availability.ToString());
			configurationApiItemWrapper.SetProperty("IncludesScopeItems", includesScopeItems.ToString());
			return new AddSaveSearchServerTask(configurationApiItemWrapper.ExecuteMethod("AddSaveSearch"));
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, "SaveSearchesFolder", string.Format(CultureInfo.InvariantCulture, "Unable to call method '{0}': {1}", "AddSaveSearch", ex.Message));
			throw;
		}
	}

	public UpdateSaveSearchServerTask UpdateSaveSearch(Guid id, string name, string description, string searchQuery, SaveSearches.SaveSearchAvailability availability, bool includesScopeItems)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("UpdateSaveSearch");
			configurationApiItemWrapper.SetProperty("Id", id.ToString());
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("SearchQuery", searchQuery);
			configurationApiItemWrapper.SetProperty("Availability", availability.ToString());
			configurationApiItemWrapper.SetProperty("IncludesScopeItems", includesScopeItems.ToString());
			return new UpdateSaveSearchServerTask(configurationApiItemWrapper.ExecuteMethod("UpdateSaveSearch"));
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, "SaveSearchesFolder", string.Format(CultureInfo.InvariantCulture, "Unable to call method '{0}': {1}", "UpdateSaveSearch", ex.Message));
			throw;
		}
	}

	public ServerTask RemoveSaveSearch(Guid id)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveSaveSearch");
			configurationApiItemWrapper.SetProperty("Id", id.ToString());
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveSaveSearch"));
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, "SaveSearchesFolder", string.Format(CultureInfo.InvariantCulture, "Unable to call method '{0}': {1}", "RemoveSaveSearch", ex.Message));
			throw;
		}
	}

	public GetSaveSearchServerTask GetSaveSearch(Guid id)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("GetSaveSearch");
			configurationApiItemWrapper.SetProperty("Id", id.ToString());
			return new GetSaveSearchServerTask(configurationApiItemWrapper.ExecuteMethod("GetSaveSearch"));
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, "SaveSearchesFolder", string.Format(CultureInfo.InvariantCulture, "Unable to call method '{0}': {1}", "GetSaveSearch", ex.Message));
			throw;
		}
	}

	public FindSaveSearchesServerTask FindSaveSearches(int count, int startIndex, string keywordFilter, bool privateOnly)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("FindSaveSearches");
			configurationApiItemWrapper.SetProperty("Count", count.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("StartIndex", startIndex.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("KeywordFilter", keywordFilter);
			configurationApiItemWrapper.SetProperty("PrivateOnly", privateOnly.ToString());
			return new FindSaveSearchesServerTask(configurationApiItemWrapper.ExecuteMethod("FindSaveSearches"));
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, "SearchTemplateFolder", string.Format(CultureInfo.InvariantCulture, "Unable to call method '{0}': {1}", "FindSaveSearches", ex.Message));
			throw;
		}
	}
}
