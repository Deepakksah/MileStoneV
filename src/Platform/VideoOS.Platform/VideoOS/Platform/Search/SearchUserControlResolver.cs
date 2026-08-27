using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Platform.Search.FilterConfigurations;
using VideoOS.Platform.Search.FilterValues;

namespace VideoOS.Platform.Search;

public class SearchUserControlResolver
{
	public static readonly Guid MilestoneSearchAgentPluginId = new Guid("1A5F5C8F-815B-4C77-8F33-7ECABCE8A431");

	private static SearchUserControlResolver _instance;

	private static readonly object _instanceLock = new object();

	private readonly Dictionary<Guid, SearchUserControlsPlugin> _searchResultUserControlTypeMappings = new Dictionary<Guid, SearchUserControlsPlugin>();

	private readonly Dictionary<Type, SearchUserControlsPlugin> _searchFilterConfigurationMappings = new Dictionary<Type, SearchUserControlsPlugin>();

	public static SearchUserControlResolver Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_instanceLock)
				{
					if (_instance == null)
					{
						_instance = new SearchUserControlResolver();
					}
				}
			}
			return _instance;
		}
	}

	private SearchUserControlResolver()
	{
		List<PluginDefinition> allPluginDefinitions = EnvironmentManager.Instance.AllPluginDefinitions;
		PluginDefinition pluginDefinition = allPluginDefinitions.FirstOrDefault((PluginDefinition p) => p.Id == MilestoneSearchAgentPluginId);
		if (pluginDefinition != null)
		{
			InitializeConfigurationMappings(pluginDefinition);
			allPluginDefinitions.Remove(pluginDefinition);
		}
		foreach (PluginDefinition item in allPluginDefinitions)
		{
			InitializeConfigurationMappings(item);
		}
	}

	private void InitializeConfigurationMappings(PluginDefinition platformPlugin)
	{
		IEnumerable<SearchUserControlsPlugin> enumerable = platformPlugin?.SearchUserControlsPlugins;
		if (enumerable == null)
		{
			return;
		}
		foreach (SearchUserControlsPlugin item in enumerable)
		{
			AddSearchUserControlsPlugin(item, overwriteExisting: true);
		}
	}

	private void AddSearchUserControlsPlugin(SearchUserControlsPlugin plugin, bool overwriteExisting)
	{
		if (plugin == null)
		{
			throw new ArgumentNullException("plugin");
		}
		if (plugin.SearchResultUserControlTypes != null)
		{
			foreach (Guid searchResultUserControlType in plugin.SearchResultUserControlTypes)
			{
				if (overwriteExisting || !_searchResultUserControlTypeMappings.ContainsKey(searchResultUserControlType))
				{
					_searchResultUserControlTypeMappings[searchResultUserControlType] = plugin;
				}
			}
		}
		if (plugin.SearchFilterConfigurationTypes == null)
		{
			return;
		}
		foreach (Type searchFilterConfigurationType in plugin.SearchFilterConfigurationTypes)
		{
			if (overwriteExisting || !_searchFilterConfigurationMappings.ContainsKey(searchFilterConfigurationType))
			{
				_searchFilterConfigurationMappings[searchFilterConfigurationType] = plugin;
			}
		}
	}

	public void AddSearchUserControlsPlugin(SearchUserControlsPlugin plugin)
	{
		AddSearchUserControlsPlugin(plugin, overwriteExisting: false);
	}

	public SearchResultUserControl CreateSearchResultUserControl(Guid searchResultUserControlType)
	{
		if (!_searchResultUserControlTypeMappings.TryGetValue(searchResultUserControlType, out var value))
		{
			return null;
		}
		try
		{
			return value.CreateSearchResultUserControl(searchResultUserControlType) ?? throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unable to create search result user control for search result type '{0}' using plugin '{1}'", searchResultUserControlType, value.Name), "searchResultUserControlType");
		}
		catch (Exception ex)
		{
			string text = string.Format(CultureInfo.CurrentCulture, "{0}.{1}", GetType().Name, "SearchUserControlResolver");
			EnvironmentManager instance = EnvironmentManager.Instance;
			object[] args = new string[2] { value.Name, ex.Message };
			instance.Log(error: true, text, "Error initializing search result user control from SearchAgentPlugin=\"{0}\" with exception=\"{1}\"", args);
			return null;
		}
	}

	[Obsolete("Obsolete method. Use overload instead.")]
	public SearchFilterEditControl CreateSearchFilterEditControl(FilterConfigurationBase filterConfiguration, FilterValueBase filterValue, SearchScope searchScope)
	{
		return CreateSearchFilterEditControl(filterConfiguration, null, filterValue, searchScope);
	}

	public SearchFilterEditControl CreateSearchFilterEditControl(FilterConfigurationBase filterConfiguration, SearchFilter filter, FilterValueBase filterValue, SearchScope searchScope)
	{
		if (filterConfiguration == null)
		{
			return null;
		}
		if (!_searchFilterConfigurationMappings.TryGetValue(filterConfiguration.GetType(), out var value))
		{
			return null;
		}
		try
		{
			SearchFilterEditControl searchFilterEditControl = value.CreateSearchFilterEditControl(filterConfiguration);
			if (searchFilterEditControl == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unable to create search filter edit control for filter configuration '{0}' using plugin '{1}'", filterConfiguration.GetType().FullName, value.Name), "filterConfiguration");
			}
			searchFilterEditControl.FilterValue = filterValue;
			searchFilterEditControl.SearchScope = searchScope;
			filterConfiguration = AdjustForObsoleteFlags(filterConfiguration, searchFilterEditControl.SnapToDefaultWidth, filter?.UseInlineEdit ?? false);
			searchFilterEditControl.FilterConfiguration = filterConfiguration;
			return searchFilterEditControl;
		}
		catch (Exception ex)
		{
			string text = string.Format(CultureInfo.CurrentCulture, "{0}.{1}", GetType().Name, "SearchUserControlResolver");
			EnvironmentManager instance = EnvironmentManager.Instance;
			object[] args = new string[2] { value.Name, ex.Message };
			instance.Log(error: true, text, "Error initializing search filter edit control from SearchAgentPlugin=\"{0}\" with exception=\"{1}\"", args);
			return null;
		}
	}

	private static FilterConfigurationBase AdjustForObsoleteFlags(FilterConfigurationBase filterConfiguration, bool ctrlSnapToDefaultWidth, bool searchFilterUseInlineEdit)
	{
		if (!ctrlSnapToDefaultWidth)
		{
			if (filterConfiguration.DisplayMode == EditControlDisplayMode.FixedWidth)
			{
				filterConfiguration.DisplayMode = EditControlDisplayMode.SnapToParentWidth;
			}
			else if (filterConfiguration.DisplayMode == EditControlDisplayMode.InPopupFixedWidth)
			{
				filterConfiguration.DisplayMode = EditControlDisplayMode.InPopupSnapToParentWidth;
			}
		}
		if (searchFilterUseInlineEdit)
		{
			if (filterConfiguration.DisplayMode == EditControlDisplayMode.InPopupFixedWidth)
			{
				filterConfiguration.DisplayMode = EditControlDisplayMode.FixedWidth;
			}
			else if (filterConfiguration.DisplayMode == EditControlDisplayMode.InPopupSnapToParentWidth)
			{
				filterConfiguration.DisplayMode = EditControlDisplayMode.SnapToParentWidth;
			}
		}
		return filterConfiguration;
	}
}
