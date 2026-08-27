using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIMonitor")]
[EntityName("ConfigAPIMonitorFolder")]
public class MonitorConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	private const string CameraPathsKey = "CameraPaths";

	private const string CameraGroupPathsKey = "CameraGroupPaths";

	private const string BeginPositionKey = "BeginPosition";

	private const string LayoutPathKey = "LayoutPath";

	private const string ViewItemsXmlKey = "ViewItemsXml";

	private const string TextMessageKey = "TextMessage";

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		ValidateResult validateResult = ValidateItemInternal(item, path, out var name, out var description, out var aspectRatio, out var emptyViewItems, out var insertionMethod, out var locationX, out var locationY, out var monitorSize, out var noLayout);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		try
		{
			Monitor monitor = Monitor.QueryMonitor(requestContext, path.Id);
			if (name != null)
			{
				monitor.Name = name;
			}
			if (description != null)
			{
				monitor.Description = description;
			}
			if (aspectRatio.HasValue)
			{
				monitor.AspectRatio = aspectRatio.Value;
			}
			if (emptyViewItems.HasValue)
			{
				monitor.EmptyViewItems = emptyViewItems.Value;
			}
			if (insertionMethod.HasValue)
			{
				monitor.InsertionMethod = insertionMethod.Value;
			}
			if (locationX.HasValue)
			{
				monitor.LocationX = locationX.Value;
			}
			if (locationY.HasValue)
			{
				monitor.LocationY = locationY.Value;
			}
			if (monitorSize.HasValue)
			{
				monitor.MonitorSize = (float)monitorSize.Value;
			}
			if (noLayout.HasValue)
			{
				monitor.NoLayout = noLayout.Value;
			}
			monitor = Monitor.UpdateMonitor(requestContext, monitor);
			ConfigurationItem resultItem = ConvertToMonitorConfigurationItem(parentPath, monitor);
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = resultItem;
			return validateResult;
		}
		catch (DeviceUpdateException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			string[] monitorIds = Monitor.QueryMonitorIds(requestContext, path.Id);
			Monitor[] array = Monitor.QueryMonitorsByIds(requestContext, monitorIds);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			Monitor[] array2 = array;
			foreach (Monitor monitor in array2)
			{
				list.Add(ConvertToMonitorConfigurationItem(path, monitor));
			}
			return list.ToArray();
		}
		ConfigurationItem configurationItem = GroupConfigurationItemFactory.CreateFolderItem("MonitorPresetFolder", path);
		return new ConfigurationItem[1] { configurationItem };
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("MonitorFolder", ConfigurationItemPath.CreateItemPath("VideoWall", path.Id));
		}
		try
		{
			Monitor monitor = Monitor.QueryMonitor(requestContext, path.Id);
			if (monitor == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			return ConvertToMonitorConfigurationItem(ConfigurationItemPath.CreateFolderPath("VideoWall", monitor.VideoWallId, "MonitorFolder"), monitor);
		}
		catch (MonitorNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!item.MethodIds.Contains(method))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", method, item.ItemType), "method");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (method)
		{
		case "AddMonitor":
			if (!flag)
			{
				return CreateInvokeInfoAddMonitor(item, method);
			}
			return AddMonitor(requestContext, item, path);
		case "RemoveMonitor":
			if (!flag)
			{
				return CreateInvokeInfoRemoveMonitor(item, GetChildItems(requestContext, path), method);
			}
			return RemoveMonitor(requestContext, item, path);
		case "ApplyCameras":
			if (!flag)
			{
				return CreateInvokeInfoApplyCameras(item, method);
			}
			return ApplyCameras(requestContext, item, path);
		case "ApplyCamerasAndLayout":
			if (!flag)
			{
				return CreateInvokeInfoApplyCamerasAndLayout(item, method);
			}
			return ApplyCamerasAndLayout(requestContext, item, path);
		case "RemoveCameras":
			if (!flag)
			{
				return CreateInvokeInfoRemoveCameras(item, method);
			}
			return RemoveCameras(requestContext, item, path);
		case "ApplyViewItemsXml":
			if (!flag)
			{
				return CreateInvokeInfoApplyViewItemsXml(item, method);
			}
			return ApplyViewItemsXml(requestContext, item, path);
		case "ApplyMonitorState":
			if (!flag)
			{
				return CreateInvokeInfoApplyMonitorState(item, method);
			}
			return ApplyMonitorState(requestContext, item, path);
		case "ApplyLayout":
			if (!flag)
			{
				return CreateInvokeInfoApplyLayout(item, method);
			}
			return ApplyLayout(requestContext, item, path);
		case "ApplyTextAndLayout":
			if (!flag)
			{
				return CreateInvokeInfoApplyTextAndLayout(item, method);
			}
			return ApplyTextAndLayout(requestContext, item, path);
		default:
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
		}
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder)
		{
			if (Monitor.QueryMonitor(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			string name;
			string description;
			AspectRatio? aspectRatio;
			PresetLayoutMethod? emptyViewItems;
			ElementInsertionMethod? insertionMethod;
			double? locationX;
			double? locationY;
			double? monitorSize;
			PresetLayoutMethod? noLayout;
			return ValidateItemInternal(item, path, out name, out description, out aspectRatio, out emptyViewItems, out insertionMethod, out locationX, out locationY, out monitorSize, out noLayout);
		}
		throw new NotSupportedException();
	}

	private static ConfigurationItem CreateInvokeInfoAddMonitor(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddMonitor;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[9]
		{
			GeneralPropertyDefinitions.GetNameProperty(""),
			GeneralPropertyDefinitions.GetDescriptionProperty(""),
			MonitorPropertyDefinition.AspectRatioProperty(AspectRatio.Aspect4x3),
			MonitorPropertyDefinition.EmptyViewItemsProperty(PresetLayoutMethod.Preserve),
			MonitorPropertyDefinition.InsertionMethodProperty(ElementInsertionMethod.Independent),
			MonitorPropertyDefinition.LocationXProperty(0.0),
			MonitorPropertyDefinition.LocationYProperty(0.0),
			MonitorPropertyDefinition.MonitorSizeProperty(20.0),
			MonitorPropertyDefinition.NoLayoutProperty(PresetLayoutMethod.Preserve)
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveMonitor(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveMonitor, "PropertyRemoveMonitor", ConfigurationApiResources.PropertyRemoveMonitor, item, members);
	}

	private static ConfigurationItem CreateInvokeInfoApplyCameras(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardApplyCameras;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[3]
		{
			CameraPathsProperty(),
			CameraGroupPathsProperty(),
			BeginPositionProperty()
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoApplyCamerasAndLayout(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardApplyCamerasAndLayout;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[4]
		{
			CameraPathsProperty(),
			CameraGroupPathsProperty(),
			BeginPositionProperty(),
			LayoutPathProperty()
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveCameras(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardRemoveCameras;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[2]
		{
			CameraPathsProperty(),
			CameraGroupPathsProperty()
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoApplyViewItemsXml(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardApplyViewItemsXml;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[2]
		{
			ViewItemsXmlProperty(),
			BeginPositionProperty()
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoApplyMonitorState(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardApplyMonitorState;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[1] { MonitorPropertyDefinition.MonitorStateProperty("", isSettable: true) };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoApplyLayout(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardApplyLayout;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[1] { LayoutPathProperty() };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoApplyTextAndLayout(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardApplyTextAndLayout;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[3]
		{
			TextMessageProperty(),
			BeginPositionProperty(),
			LayoutPathProperty()
		};
		return configurationItem;
	}

	private static ConfigurationItem AddMonitor(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "VideoWall", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string name;
		string description;
		AspectRatio? aspectRatio;
		PresetLayoutMethod? emptyViewItems;
		ElementInsertionMethod? insertionMethod;
		double? locationX;
		double? locationY;
		double? monitorSize;
		PresetLayoutMethod? noLayout;
		ValidateResult validateResult = ValidateItemInternal(item, path, out name, out description, out aspectRatio, out emptyViewItems, out insertionMethod, out locationX, out locationY, out monitorSize, out noLayout, allowFolder: true);
		if (!validateResult.ValidatedOk)
		{
			throw new PropertyInvalidException(validateResult.ErrorResults.First().ErrorText);
		}
		try
		{
			Monitor monitor = Monitor.CreateMonitor(requestContext, path.Id, new Monitor
			{
				Name = name,
				Description = description,
				AspectRatio = (aspectRatio ?? AspectRatio.Aspect4x3),
				EmptyViewItems = emptyViewItems.GetValueOrDefault(),
				InsertionMethod = (insertionMethod ?? ElementInsertionMethod.Independent),
				LocationX = locationX.GetValueOrDefault(),
				LocationY = locationY.GetValueOrDefault(),
				MonitorSize = (float)(monitorSize ?? 20.0),
				NoLayout = noLayout.GetValueOrDefault()
			});
			ConfigurationItem configurationItem = ConvertToMonitorConfigurationItem(path, monitor);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (MonitorCreationException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
	}

	private static ConfigurationItem RemoveMonitor(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "VideoWall", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			Monitor.DeleteMonitor(requestContext, invokeMemberItemSelectionPath.Id);
		}
		catch (RemoveNonExistingMonitorException)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static ConfigurationItem ApplyCameras(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "Monitor", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = null;
		string value2 = null;
		int? value3 = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CameraPaths", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CameraGroupPaths", out value2);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("BeginPosition", out value3);
		}
		if (value == null && value2 == null)
		{
			throw new PropertyInvalidException(string.Format(CultureInfo.InvariantCulture, "{0} and {1} cannot both be null", "CameraPaths", "CameraGroupPaths"));
		}
		value3 = ((!value3.HasValue) ? new int?(0) : (value3 - 1));
		List<ConfigurationItemPath> list = new List<ConfigurationItemPath>();
		List<ConfigurationItemPath> list2 = new List<ConfigurationItemPath>();
		ValidateApplyCamerasArguments(value, value2, list, list2);
		try
		{
			Monitor.ApplyCamerasToMonitorView(requestContext, path.Id, list.Select((ConfigurationItemPath p) => p.Id).ToArray(), list2.Select((ConfigurationItemPath p) => p.Id).ToArray(), value3.Value);
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (MonitorNotFoundException exception)
		{
			throw new PathInvalidException("Monitor not found.", exception);
		}
	}

	private static ConfigurationItem ApplyCamerasAndLayout(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "Monitor", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = null;
		string value2 = null;
		string value3 = null;
		int? value4 = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CameraPaths", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CameraGroupPaths", out value2);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("BeginPosition", out value4);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("LayoutPath", out value3);
		}
		value4 = ((!value4.HasValue) ? new int?(0) : (value4 - 1));
		List<ConfigurationItemPath> list = new List<ConfigurationItemPath>();
		List<ConfigurationItemPath> list2 = new List<ConfigurationItemPath>();
		ValidateApplyCamerasArguments(value, value2, list, list2);
		string layoutId = Guid.Empty.ToString();
		if (!string.IsNullOrEmpty(value3))
		{
			try
			{
				layoutId = ConfigurationItemPath.CreatePath(value3).Id;
			}
			catch (ArgumentException exception)
			{
				throw new PathInvalidException("Layout not found.", exception);
			}
		}
		try
		{
			Monitor.ApplyCamerasAndLayoutToMonitorView(requestContext, path.Id, list.Select((ConfigurationItemPath p) => p.Id).ToArray(), list2.Select((ConfigurationItemPath p) => p.Id).ToArray(), value4.Value, layoutId);
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (MonitorNotFoundException exception2)
		{
			throw new PathInvalidException("Monitor not found.", exception2);
		}
	}

	private static ConfigurationItem RemoveCameras(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "Monitor", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = null;
		string value2 = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CameraPaths", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CameraGroupPaths", out value2);
		}
		if (value == null && value2 == null)
		{
			throw new PropertyInvalidException(string.Format(CultureInfo.InvariantCulture, "{0} and {1} cannot both be null", "CameraPaths", "CameraGroupPaths"));
		}
		List<ConfigurationItemPath> list = new List<ConfigurationItemPath>();
		List<ConfigurationItemPath> list2 = new List<ConfigurationItemPath>();
		ValidateApplyCamerasArguments(value, value2, list, list2);
		try
		{
			Monitor.RemoveCamerasFromMonitorView(requestContext, path.Id, list.Select((ConfigurationItemPath p) => p.Id).ToArray(), list2.Select((ConfigurationItemPath p) => p.Id).ToArray());
		}
		catch (MonitorNotFoundException exception)
		{
			throw new PathInvalidException("Monitor not found.", exception);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static ConfigurationItem ApplyViewItemsXml(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "Monitor", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = null;
		int? value2 = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ViewItemsXml", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("BeginPosition", out value2);
		}
		value2 = ((!value2.HasValue) ? new int?(0) : (value2 - 1));
		if (value == null)
		{
			throw new PropertyInvalidException("Invalid property.", "ViewItemsXml");
		}
		List<string> list = new List<string>();
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(value);
			foreach (XmlNode item2 in xmlDocument.SelectNodes("/viewitems/viewitem"))
			{
				list.Add(item2.OuterXml);
			}
		}
		catch (XmlException inner)
		{
			throw new PropertyInvalidException("Invalid property.", "ViewItemsXml", inner);
		}
		try
		{
			Monitor.ApplyViewItemXmlsToMonitorView(requestContext, path.Id, list.ToArray(), value2.Value);
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (MonitorNotFoundException exception)
		{
			throw new PathInvalidException("Monitor not found.", exception);
		}
	}

	private static ConfigurationItem ApplyMonitorState(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "Monitor", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("MonitorState", out value);
		}
		if (value == null)
		{
			throw new PropertyInvalidException("Invalid property.", "MonitorState");
		}
		try
		{
			Monitor.ApplyMonitorStateToMonitor(requestContext, path.Id, value);
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (MonitorNotFoundException exception)
		{
			throw new PathInvalidException("Monitor not found.", exception);
		}
	}

	private static ConfigurationItem ApplyLayout(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "Monitor", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("LayoutPath", out value);
		}
		ConfigurationItemPath configurationItemPath = null;
		if (value != null)
		{
			try
			{
				configurationItemPath = ConfigurationItemPath.CreatePath(value);
			}
			catch (ArgumentException exception)
			{
				throw new PathInvalidException("Layout not found.", exception);
			}
		}
		if (configurationItemPath == null)
		{
			throw new PropertyInvalidException("Invalid property.", "LayoutPath");
		}
		try
		{
			Monitor.ApplyLayout(requestContext, path.Id, configurationItemPath.Id);
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (MonitorNotFoundException exception2)
		{
			throw new PathInvalidException("Monitor not found.", exception2);
		}
	}

	private static ConfigurationItem ApplyTextAndLayout(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "Monitor", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = null;
		string value2 = null;
		int? value3 = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("TextMessage", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("BeginPosition", out value3);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("LayoutPath", out value2);
		}
		value3 = ((!value3.HasValue) ? new int?(0) : (value3 - 1));
		if (string.IsNullOrEmpty(value))
		{
			throw new PropertyInvalidException("Invalid property.", "TextMessage");
		}
		string layoutId = Guid.Empty.ToString();
		if (!string.IsNullOrEmpty(value2))
		{
			try
			{
				layoutId = ConfigurationItemPath.CreatePath(value2).Id;
			}
			catch (ArgumentException exception)
			{
				throw new PathInvalidException("Layout not found.", exception);
			}
		}
		try
		{
			Monitor.ApplyTextAndLayoutToMonitorView(requestContext, path.Id, value, value3.Value, layoutId);
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (MonitorNotFoundException exception2)
		{
			throw new PathInvalidException("Monitor not found.", exception2);
		}
	}

	private static ValidateResult ValidateItemInternal(ConfigurationItem item, ConfigurationItemPath path, out string name, out string description, out AspectRatio? aspectRatio, out PresetLayoutMethod? emptyViewItems, out ElementInsertionMethod? insertionMethod, out double? locationX, out double? locationY, out double? monitorSize, out PresetLayoutMethod? noLayout, bool allowFolder = false)
	{
		name = null;
		description = null;
		aspectRatio = null;
		emptyViewItems = null;
		insertionMethod = null;
		locationX = null;
		locationY = null;
		monitorSize = null;
		noLayout = null;
		try
		{
			if (!path.IsFolder || allowFolder)
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AspectRatio", out aspectRatio);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EmptyViewItems", out emptyViewItems);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("InsertionMethod", out insertionMethod);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("LocationX", out locationX);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("LocationY", out locationY);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("MonitorSize", out monitorSize);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("NoLayout", out noLayout);
				}
				Validation.CheckName(name, "Name", allowNull: true);
				Validation.CheckDescription(ref description, "Description", allowNull: true);
				if (aspectRatio.HasValue)
				{
					Validation.CheckAspectRatio(aspectRatio.Value, "AspectRatio");
				}
				if (emptyViewItems.HasValue)
				{
					Validation.CheckPresetLayoutMethod(emptyViewItems.Value, "EmptyViewItems");
				}
				if (insertionMethod.HasValue)
				{
					Validation.CheckElementInsertionMethod(insertionMethod.Value, "InsertionMethod");
				}
				ValidateLocation(locationX, locationY);
				if (noLayout.HasValue)
				{
					Validation.CheckPresetLayoutMethod(noLayout.Value, "NoLayout");
				}
				ValidateResult validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = item;
				return validateResult;
			}
			throw new NotSupportedException();
		}
		catch (ArgumentException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	private static void ValidateLocation(double? locationX, double? locationY)
	{
		if (locationX.HasValue || locationY.HasValue)
		{
			if (locationX.HasValue && !locationY.HasValue)
			{
				throw new ArgumentNullException("LocationY", VmoResources.VMO62002);
			}
			if (locationY.HasValue && !locationX.HasValue)
			{
				throw new ArgumentNullException("LocationX", VmoResources.VMO62002);
			}
		}
	}

	private static void ValidateApplyCamerasArguments(string serializedCameraPaths, string serializedCameraGroupPaths, List<ConfigurationItemPath> cameraPaths, List<ConfigurationItemPath> cameraGroupPaths)
	{
		if (!string.IsNullOrEmpty(serializedCameraPaths))
		{
			try
			{
				cameraPaths.AddRange(from s in serializedCameraPaths.Trim().Split(';')
					select ConfigurationItemPath.CreatePath(s));
				ValidatePaths(cameraPaths, "Camera");
			}
			catch (OverflowException)
			{
				throw new PropertyInvalidException("Invalid property.", "CameraPaths");
			}
			catch (FormatException)
			{
				throw new PropertyInvalidException("Invalid property.", "CameraPaths");
			}
			catch (ArgumentException)
			{
				throw new PropertyInvalidException("Invalid property.", "CameraPaths");
			}
		}
		if (string.IsNullOrEmpty(serializedCameraGroupPaths))
		{
			return;
		}
		try
		{
			cameraGroupPaths.AddRange(from s in serializedCameraGroupPaths.Trim().Split(';')
				select ConfigurationItemPath.CreatePath(s));
			ValidatePaths(cameraGroupPaths, "CameraGroup");
		}
		catch (OverflowException)
		{
			throw new PropertyInvalidException("Invalid property.", "CameraGroupPaths");
		}
		catch (FormatException)
		{
			throw new PropertyInvalidException("Invalid property.", "CameraGroupPaths");
		}
		catch (ArgumentException)
		{
			throw new PropertyInvalidException("Invalid property.", "CameraGroupPaths");
		}
	}

	private static void ValidatePaths(IEnumerable<ConfigurationItemPath> paths, string itemType)
	{
		if (!paths.All((ConfigurationItemPath p) => p.ItemType == itemType))
		{
			throw new FormatException();
		}
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "VideoWall", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static ConfigurationItem ConvertToMonitorConfigurationItem(ConfigurationItemPath parentPath, Monitor monitor)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "Monitor";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = monitor.Name;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, monitor.Id).Text;
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(monitor.Id));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(monitor.Name));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(monitor.Description));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(monitor.LastModified));
		list.Add(GeneralPropertyDefinitions.GetCreatedDateProperty(monitor.CreatedDate));
		list.Add(MonitorPropertyDefinition.AspectRatioProperty(monitor.AspectRatio));
		list.Add(MonitorPropertyDefinition.EmptyViewItemsProperty(monitor.EmptyViewItems));
		list.Add(MonitorPropertyDefinition.InsertionMethodProperty(monitor.InsertionMethod));
		list.Add(MonitorPropertyDefinition.LocationXProperty(monitor.LocationX));
		list.Add(MonitorPropertyDefinition.LocationYProperty(monitor.LocationY));
		list.Add(MonitorPropertyDefinition.MonitorSizeProperty(monitor.MonitorSize));
		list.Add(MonitorPropertyDefinition.NoLayoutProperty(monitor.NoLayout));
		list.Add(MonitorPropertyDefinition.MonitorStateProperty(monitor.MonitorState, isSettable: false));
		list.Add(MonitorPropertyDefinition.ViewProperty(ConfigurationItemPath.CreateItemPath("View", monitor.ViewId).Text));
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[7] { "ApplyCameras", "ApplyCamerasAndLayout", "RemoveCameras", "ApplyViewItemsXml", "ApplyMonitorState", "ApplyLayout", "ApplyTextAndLayout" };
		return configurationItem;
	}

	private static Property CameraPathsProperty()
	{
		Property property = new Property("CameraPaths", "");
		property.ValueType = "PathList";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.PropertyCameraPaths;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.ToolTipTranslationId = "PropertyCameraPathsToolTip";
		property.TranslationId = "PropertyCameraPaths";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "Camera"
			}
		};
		return property;
	}

	private static Property CameraGroupPathsProperty()
	{
		Property property = new Property("CameraGroupPaths", "");
		property.ValueType = "PathList";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.PropertyCameraGroupPaths;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.ToolTipTranslationId = "PropertyCameraGroupPathsToolTip";
		property.TranslationId = "PropertyCameraGroupPaths";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "CameraGroup"
			}
		};
		return property;
	}

	private static Property BeginPositionProperty()
	{
		return new Property("BeginPosition", PropertyTypesHelper.GetValue(1))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyBeginPosition,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyBeginPositionToolTip",
			TranslationId = "PropertyBeginPosition",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetIntegerRange(1, 999)
		};
	}

	private static Property LayoutPathProperty()
	{
		Property property = new Property("LayoutPath", "");
		property.ValueType = "Path";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.PropertyViewLayoutPath;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.ToolTipTranslationId = "PropertyViewLayoutPathToolTip";
		property.TranslationId = "PropertyViewLayoutPath";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "Layout"
			}
		};
		return property;
	}

	private static Property ViewItemsXmlProperty()
	{
		return new Property("ViewItemsXml", "")
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyViewItemsXml,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyViewItemsXmlToolTip",
			TranslationId = "PropertyViewItemsXml",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	private static Property TextMessageProperty()
	{
		return new Property("TextMessage", "")
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyTextMessage,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyTextMessageToolTip",
			TranslationId = "PropertyTextMessage",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}
