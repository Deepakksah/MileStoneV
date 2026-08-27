using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class Monitor : IConfigurationItem
{
	public MonitorPresetFolder MonitorPresetFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "MonitorPresetFolder")
				{
					return new MonitorPresetFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public DateTime CreatedDate
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("CreatedDate"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public Dictionary<string, string> AspectRatioValues => base.Wrapper.GetEnumValues("AspectRatio");

	public string AspectRatio
	{
		get
		{
			return base.Wrapper.GetPropertyValue("AspectRatio");
		}
		set
		{
			base.Wrapper.SetPropertyValue("AspectRatio", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> EmptyViewItemsValues => base.Wrapper.GetEnumValues("EmptyViewItems");

	public string EmptyViewItems
	{
		get
		{
			return base.Wrapper.GetPropertyValue("EmptyViewItems");
		}
		set
		{
			base.Wrapper.SetPropertyValue("EmptyViewItems", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> InsertionMethodValues => base.Wrapper.GetEnumValues("InsertionMethod");

	public string InsertionMethod
	{
		get
		{
			return base.Wrapper.GetPropertyValue("InsertionMethod");
		}
		set
		{
			base.Wrapper.SetPropertyValue("InsertionMethod", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public double LocationX
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("LocationX");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("LocationX", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double LocationY
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("LocationY");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("LocationY", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double MonitorSize
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("MonitorSize");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("MonitorSize", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> NoLayoutValues => base.Wrapper.GetEnumValues("NoLayout");

	public string NoLayout
	{
		get
		{
			return base.Wrapper.GetPropertyValue("NoLayout");
		}
		set
		{
			base.Wrapper.SetPropertyValue("NoLayout", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string MonitorState => base.Wrapper.GetPropertyValue("MonitorState");

	public string View => base.Wrapper.GetPropertyValue("View");

	public View ViewObject => new View(base.ServerId, View);

	public Monitor(ServerId serverId, ConfigurationItem monitor)
		: base(serverId, monitor)
	{
	}

	public Monitor(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public Monitor(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask ApplyCameras(string cameraPaths, string cameraGroupPaths, int beginPosition)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ApplyCameras");
			configurationApiItemWrapper.SetProperty("CameraPaths", cameraPaths);
			configurationApiItemWrapper.SetProperty("CameraGroupPaths", cameraGroupPaths);
			configurationApiItemWrapper.SetProperty("BeginPosition", beginPosition.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ApplyCameras"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Monitor", "Unable to call method 'ApplyCameras':" + ex.Message);
		}
		return null;
	}

	public ApplyCamerasServerTask ApplyCameras()
	{
		try
		{
			return new ApplyCamerasServerTask(base.Wrapper.ExecuteMethod("ApplyCameras"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Monitor", "Unable to call method 'ApplyCameras':" + ex.Message);
		}
		return null;
	}

	public ServerTask ApplyCamerasAndLayout(string cameraPaths, string cameraGroupPaths, int beginPosition, string layoutPath)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ApplyCamerasAndLayout");
			configurationApiItemWrapper.SetProperty("CameraPaths", cameraPaths);
			configurationApiItemWrapper.SetProperty("CameraGroupPaths", cameraGroupPaths);
			configurationApiItemWrapper.SetProperty("BeginPosition", beginPosition.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("LayoutPath", layoutPath);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ApplyCamerasAndLayout"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Monitor", "Unable to call method 'ApplyCamerasAndLayout':" + ex.Message);
		}
		return null;
	}

	public ApplyCamerasAndLayoutServerTask ApplyCamerasAndLayout()
	{
		try
		{
			return new ApplyCamerasAndLayoutServerTask(base.Wrapper.ExecuteMethod("ApplyCamerasAndLayout"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Monitor", "Unable to call method 'ApplyCamerasAndLayout':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveCameras(string cameraPaths, string cameraGroupPaths)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveCameras");
			configurationApiItemWrapper.SetProperty("CameraPaths", cameraPaths);
			configurationApiItemWrapper.SetProperty("CameraGroupPaths", cameraGroupPaths);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveCameras"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Monitor", "Unable to call method 'RemoveCameras':" + ex.Message);
		}
		return null;
	}

	public RemoveCamerasServerTask RemoveCameras()
	{
		try
		{
			return new RemoveCamerasServerTask(base.Wrapper.ExecuteMethod("RemoveCameras"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Monitor", "Unable to call method 'RemoveCameras':" + ex.Message);
		}
		return null;
	}

	public ServerTask ApplyViewItemsXml(string viewItemsXml, int beginPosition)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ApplyViewItemsXml");
			configurationApiItemWrapper.SetProperty("ViewItemsXml", viewItemsXml);
			configurationApiItemWrapper.SetProperty("BeginPosition", beginPosition.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ApplyViewItemsXml"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Monitor", "Unable to call method 'ApplyViewItemsXml':" + ex.Message);
		}
		return null;
	}

	public ApplyViewItemsXmlServerTask ApplyViewItemsXml()
	{
		try
		{
			return new ApplyViewItemsXmlServerTask(base.Wrapper.ExecuteMethod("ApplyViewItemsXml"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Monitor", "Unable to call method 'ApplyViewItemsXml':" + ex.Message);
		}
		return null;
	}

	public ServerTask ApplyMonitorState(string monitorState)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ApplyMonitorState");
			configurationApiItemWrapper.SetProperty("MonitorState", monitorState);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ApplyMonitorState"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Monitor", "Unable to call method 'ApplyMonitorState':" + ex.Message);
		}
		return null;
	}

	public ApplyMonitorStateServerTask ApplyMonitorState()
	{
		try
		{
			return new ApplyMonitorStateServerTask(base.Wrapper.ExecuteMethod("ApplyMonitorState"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Monitor", "Unable to call method 'ApplyMonitorState':" + ex.Message);
		}
		return null;
	}

	public ServerTask ApplyLayout(string layoutPath)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ApplyLayout");
			configurationApiItemWrapper.SetProperty("LayoutPath", layoutPath);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ApplyLayout"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Monitor", "Unable to call method 'ApplyLayout':" + ex.Message);
		}
		return null;
	}

	public ApplyLayoutServerTask ApplyLayout()
	{
		try
		{
			return new ApplyLayoutServerTask(base.Wrapper.ExecuteMethod("ApplyLayout"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Monitor", "Unable to call method 'ApplyLayout':" + ex.Message);
		}
		return null;
	}

	public ServerTask ApplyTextAndLayout(string textMessage, int beginPosition, string layoutPath)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ApplyTextAndLayout");
			configurationApiItemWrapper.SetProperty("TextMessage", textMessage);
			configurationApiItemWrapper.SetProperty("BeginPosition", beginPosition.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("LayoutPath", layoutPath);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ApplyTextAndLayout"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Monitor", "Unable to call method 'ApplyTextAndLayout':" + ex.Message);
		}
		return null;
	}

	public ApplyTextAndLayoutServerTask ApplyTextAndLayout()
	{
		try
		{
			return new ApplyTextAndLayoutServerTask(base.Wrapper.ExecuteMethod("ApplyTextAndLayout"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Monitor", "Unable to call method 'ApplyTextAndLayout':" + ex.Message);
		}
		return null;
	}
}
