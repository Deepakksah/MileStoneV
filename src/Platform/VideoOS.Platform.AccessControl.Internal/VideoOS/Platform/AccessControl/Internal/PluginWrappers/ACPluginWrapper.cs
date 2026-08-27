using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Internal.PluginWrappers;

public class ACPluginWrapper
{
	private enum TraceLevels
	{
		Disabled,
		Simple,
		Detailed
	}

	private ACPlugin _acPlugin;

	private static TraceLevels TraceLevel { get; set; }

	public Guid Id => TryCallPlugin(() => _acPlugin.Id, _acPlugin.GetType(), "Id (Get)", Guid.Empty);

	public string Name => TryCallPlugin(() => _acPlugin.Name, _acPlugin.GetType(), "Name (Get)", string.Empty);

	public Version Version => TryCallPlugin(() => _acPlugin.Version, _acPlugin.GetType(), "Version (Get)", new Version());

	public string VersionText => TryCallPlugin(() => _acPlugin.VersionText, _acPlugin.GetType(), "VersionText (Get)", string.Empty);

	public Image Icon => TryCallPlugin(() => _acPlugin.Icon, _acPlugin.GetType(), "Icon (Get)", null);

	static ACPluginWrapper()
	{
		TraceLevel = TraceLevels.Disabled;
		string text = ConfigurationManager.AppSettings.Get("AccessControlTraceLevel");
		if (!string.IsNullOrWhiteSpace(text))
		{
			text.Trim();
			if (text.Equals("Simple", StringComparison.InvariantCultureIgnoreCase))
			{
				TraceLevel = TraceLevels.Simple;
			}
			else if (text.Equals("Detailed", StringComparison.InvariantCultureIgnoreCase))
			{
				TraceLevel = TraceLevels.Detailed;
			}
		}
	}

	public ACPluginWrapper(ACPlugin acPlugin)
	{
		_acPlugin = acPlugin;
	}

	public void Init()
	{
		TryCallPlugin(delegate
		{
			_acPlugin.Init();
		}, _acPlugin.GetType(), "Init()");
	}

	public void Close()
	{
		TryCallPlugin(delegate
		{
			_acPlugin.Close();
		}, _acPlugin.GetType(), "Close()");
	}

	public IEnumerable<ACIconInfo> GetIcons()
	{
		IEnumerable<ACIconInfo> enumerable = TryCallPlugin(() => _acPlugin.GetIcons(), _acPlugin.GetType(), "GetIcons()", new ACIconInfo[0]);
		return new ReadOnlyCollection<ACIconInfo>((enumerable != null) ? enumerable.ToList() : new List<ACIconInfo>());
	}

	public IEnumerable<ACCategoryInfo> GetCategories()
	{
		IEnumerable<ACCategoryInfo> enumerable = TryCallPlugin(() => _acPlugin.GetCategories(), _acPlugin.GetType(), "GetCategories()", new ACCategoryInfo[0]);
		return new ReadOnlyCollection<ACCategoryInfo>((enumerable != null) ? enumerable.ToList() : new List<ACCategoryInfo>());
	}

	public IEnumerable<ACPropertyDefinition> GetPropertyDefinitions()
	{
		IEnumerable<ACPropertyDefinition> enumerable = TryCallPlugin(() => _acPlugin.GetPropertyDefinitions(), _acPlugin.GetType(), "GetPropertyDefinitions()", new ACPropertyDefinition[0]);
		return new ReadOnlyCollection<ACPropertyDefinition>((enumerable != null) ? enumerable.ToList() : new List<ACPropertyDefinition>());
	}

	public ACSystemWrapper GenerateAccessControlSystem()
	{
		return TryCallPlugin(() => new ACSystemWrapper(_acPlugin.GenerateAccessControlSystem()), _acPlugin.GetType(), "ValidateProperties()", null);
	}

	public ACPropertyValidationResult ValidateProperties(IEnumerable<ACProperty> properties)
	{
		properties = new ReadOnlyCollection<ACProperty>((properties != null) ? properties.ToList() : new List<ACProperty>());
		return TryCallPlugin(() => _acPlugin.ValidateProperties(properties), _acPlugin.GetType(), "ValidateProperties()", null);
	}

	public static TResult TryCallPlugin<TResult>(Func<TResult> call, Type callObject, string callMethod, TResult defaultValue)
	{
		try
		{
			if (TraceLevel != TraceLevels.Disabled)
			{
				EnvironmentManager.Instance.Log(error: false, "MIPPluginTrace", string.Format(CultureInfo.InvariantCulture, "Calling {0}.{1}", callObject.FullName, callMethod));
			}
			TResult result = call();
			if (TraceLevel != TraceLevels.Disabled)
			{
				EnvironmentManager.Instance.Log(error: false, "MIPPluginTrace", string.Format(CultureInfo.InvariantCulture, "Call to {0}.{1} complete", callObject.FullName, callMethod));
			}
			return result;
		}
		catch (Exception ex)
		{
			string text = string.Format(CultureInfo.InvariantCulture, "Error during call to {0}.{1}. Exception message: {2}", callObject.FullName, callMethod, ex.Message);
			EnvironmentManager.Instance.Log(error: true, "MIPPluginTrace", text);
			return defaultValue;
		}
	}

	public static void TryCallPlugin(Action call, Type callObject, string callMethod)
	{
		try
		{
			if (TraceLevel != TraceLevels.Disabled)
			{
				EnvironmentManager.Instance.Log(error: false, "MIPPluginTrace", string.Format(CultureInfo.InvariantCulture, "Calling {0}.{1}", callObject.FullName, callMethod));
			}
			call();
			if (TraceLevel != TraceLevels.Disabled)
			{
				EnvironmentManager.Instance.Log(error: false, "MIPPluginTrace", string.Format(CultureInfo.InvariantCulture, "Call to {0}.{1} complete", callObject.FullName, callMethod));
			}
		}
		catch (Exception ex)
		{
			string text = string.Format(CultureInfo.InvariantCulture, "Error during call to {0}.{1}. Exception message: {2}", callObject.FullName, callMethod, ex.Message);
			EnvironmentManager.Instance.Log(error: true, "MIPPluginTrace", text);
		}
	}

	public static void TryHandleEvent(Action handle, Type eventSourceType, string eventName)
	{
		try
		{
			if (TraceLevel != TraceLevels.Disabled)
			{
				EnvironmentManager.Instance.Log(error: false, "MIPPluginTrace", string.Format(CultureInfo.InvariantCulture, "Event {0}.{1} signalled", eventSourceType.FullName, eventName));
			}
			handle();
			if (TraceLevel != TraceLevels.Disabled)
			{
				EnvironmentManager.Instance.Log(error: false, "MIPPluginTrace", string.Format(CultureInfo.InvariantCulture, "Event {0}.{1} handled", eventSourceType.FullName, eventName));
			}
		}
		catch (Exception ex)
		{
			string text = string.Format(CultureInfo.InvariantCulture, "Error during handling of {0}.{1}. Exception message: {2}", eventSourceType.FullName, eventName, ex.Message);
			EnvironmentManager.Instance.Log(error: true, "MIPPluginTrace", text);
		}
	}
}
