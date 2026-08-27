using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIPatrollingProfileFolder")]
[EntityName("ConfigAPIPatrollingProfile")]
public class PatrollingProfileConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		requestContext.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 25);
		item.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 26);
		path.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 27);
		parentPath.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 28);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		return ValidateOrUpdateItemInternal(requestContext, update: true, item, path, parentPath);
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		requestContext.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 38);
		item.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 39);
		path.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 40);
		parentPath.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 41);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		return ValidateOrUpdateItemInternal(requestContext, update: false, item, path, parentPath);
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 51);
		path.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 52);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			Device device = Device.QueryDeviceById(requestContext, path.Id);
			if (device == null)
			{
				throw new PathNotFoundException();
			}
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			Dictionary<string, string> presetList = GetPresetList(requestContext, path.Id);
			PatrollingProfile[] array = PatrollingProfile.QueryPatrollingProfiles(requestContext, path.Id);
			foreach (PatrollingProfile patrollingProfile in array)
			{
				PatrollingSchema patrollingSchema = new PatrollingSchema(new Guid(patrollingProfile.Id));
				patrollingSchema.DeviceId = new Guid(device.Id);
				patrollingSchema.LoadXml(patrollingProfile.DefinitionXml);
				list.Add(ConstructPatrollingItem(path, patrollingProfile, patrollingSchema, presetList));
			}
			return list.ToArray();
		}
		throw new NotSupportedException();
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 81);
		path.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 82);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath("Camera", path.Id);
			return GroupConfigurationItemFactory.CreateFolderItem("PatrollingProfileFolder", parentPath);
		}
		PatrollingProfile patrollingProfile = PatrollingProfile.QueryPatrollingProfiles(requestContext, new string[1] { path.Id })?.FirstOrDefault();
		if (patrollingProfile == null)
		{
			throw new PathNotFoundException();
		}
		PatrollingSchema patrollingSchema = new PatrollingSchema(new Guid(patrollingProfile.Id));
		patrollingSchema.DeviceId = new Guid(patrollingProfile.CameraDeviceId);
		patrollingSchema.LoadXml(patrollingProfile.DefinitionXml);
		Dictionary<string, string> presetList = GetPresetList(requestContext, patrollingProfile.CameraDeviceId);
		return ConstructPatrollingItem(ConfigurationItemPath.CreateFolderPath("Camera", patrollingProfile.CameraDeviceId, "PatrollingProfileFolder"), patrollingProfile, patrollingSchema, presetList);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		requestContext.NullCheck("", "InvokeMethod", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 107);
		item.NullCheck("", "InvokeMethod", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 108);
		path.NullCheck("", "InvokeMethod", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 109);
		parentPath.NullCheck("", "InvokeMethod", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 110);
		method.NullCheck("", "InvokeMethod", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PatrollingProfile\\PatrollingProfileConfigurationApi.cs", 111);
		ThrowIfPathInvalid(path);
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (method)
		{
		case "AddPatrollingProfile":
			if (!flag)
			{
				return CreateInvokeInfoAddPatrollingProfile(requestContext, item, path, method);
			}
			return AddPatrollingProfile(requestContext, item, path);
		case "RemovePatrollingProfile":
			if (!flag)
			{
				return CreateInvokeInfoRemovePatrollingProfile(requestContext, path, item, method);
			}
			return RemovePatrollingProfile(requestContext, item, path);
		case "AddPatrollingEntry":
			if (!flag)
			{
				return CreateInvokeInfoAddPatrollingEntry(requestContext, item, path, method);
			}
			return AddPatrollingEntry(requestContext, item);
		case "RemovePatrollingEntry":
			if (!flag)
			{
				return CreateInvokeInfoRemovePatrollingEntry(requestContext, item, path, method);
			}
			return RemovePatrollingEntry(requestContext, item);
		default:
			throw new ArgumentException("Unknown methodId:" + method);
		}
	}

	private static ValidateResult ValidateOrUpdateItemInternal(IGeminiRequestContext requestContext, bool update, ConfigurationItem profileItem, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		ThrowIfPathInvalid(path);
		string id = parentPath.Id;
		try
		{
			GetProfileProperties(profileItem, out var name, out var description, out var speed, out var transition, out var customizeTransitions, out var _, out var endPresetId, out var endSpeed, out var endTransition);
			if (string.IsNullOrEmpty(name))
			{
				throw new PropertyInvalidException("Blank name not allowed", "Name");
			}
			Dictionary<string, string> presetList = GetPresetList(requestContext, id);
			PatrollingSchema patrollingSchema = ConstructSchema(new Guid(path.Id), speed, transition, customizeTransitions, endPresetId, endSpeed, endTransition);
			ValidateAndSavePositionsInSchema(patrollingSchema, profileItem, presetList);
			PatrollingProfile patrollingProfile = PatrollingProfile.QueryPatrollingProfiles(requestContext, new string[1] { path.Id })?.FirstOrDefault();
			if (patrollingProfile == null)
			{
				throw new PathInvalidException();
			}
			patrollingProfile.Name = name;
			patrollingProfile.Description = description;
			patrollingProfile.DefinitionXml = patrollingSchema.ToXml();
			patrollingProfile.CameraDeviceId = id;
			if (update)
			{
				PatrollingProfile profile = PatrollingProfile.UpdatePatrollingProfile(requestContext, patrollingProfile);
				ConfigurationItem resultItem = ConstructPatrollingItem(parentPath, profile, patrollingSchema, presetList);
				return new ValidateResult
				{
					ValidatedOk = true,
					ResultItem = resultItem
				};
			}
			return new ValidateResult
			{
				ValidatedOk = true
			};
		}
		catch (UnauthorizedAccessException)
		{
			throw;
		}
		catch (DeviceNotFoundException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, profileItem.Properties.Where((Property p) => string.Equals(p.Key, "Id", StringComparison.Ordinal)));
			if (validateResult != null)
			{
				return validateResult;
			}
		}
		catch (PatrollingProfilePtzNotSupportedException)
		{
			throw;
		}
		catch (ArgumentException ex3)
		{
			ArgumentException ex4 = ex3;
			ArgumentException ex5 = ex4;
			ValidateResult validateResult2 = ErrorHelper.CreateValidateResult(ex5, profileItem.Properties.Where((Property p) => string.Equals(p.Key.ToUpperInvariant(), ex5.ParamName.ToUpperInvariant(), StringComparison.Ordinal)));
			if (validateResult2 == null && profileItem.Children != null && profileItem.Children.Length != 0)
			{
				validateResult2 = ErrorHelper.CreateValidateResult(ex5, profileItem.Children[0].Properties.Where((Property p) => string.Equals(p.Key.ToUpperInvariant(), ex5.ParamName.ToUpperInvariant(), StringComparison.Ordinal)));
			}
			if (validateResult2 != null)
			{
				return validateResult2;
			}
			throw;
		}
		catch (PropertyInvalidException ex6)
		{
			PropertyInvalidException ex7 = ex6;
			PropertyInvalidException ex8 = ex7;
			ValidateResult validateResult3 = ErrorHelper.CreateValidateResult(ex8, profileItem.Properties.Where((Property p) => string.Equals(p.Key.ToUpperInvariant(), ex8.PropertyName.ToUpperInvariant(), StringComparison.Ordinal)));
			if (validateResult3 == null && profileItem.Children != null && profileItem.Children.Length != 0)
			{
				validateResult3 = ErrorHelper.CreateValidateResult(ex8, profileItem.Children[0].Properties.Where((Property p) => string.Equals(p.Key.ToUpperInvariant(), ex8.PropertyName.ToUpperInvariant(), StringComparison.Ordinal)));
			}
			if (validateResult3 != null)
			{
				return validateResult3;
			}
			throw;
		}
		throw new NotSupportedException();
	}

	private static void ValidateAndSavePositionsInSchema(PatrollingSchema schema, ConfigurationItem item, Dictionary<string, string> presetList)
	{
		if (item.Children == null)
		{
			return;
		}
		SortedDictionary<int, PatrollingEdge> sortedDictionary = new SortedDictionary<int, PatrollingEdge>();
		ConfigurationItem[] children = item.Children;
		foreach (ConfigurationItem positionItem in children)
		{
			double? speed = null;
			int? transition = null;
			GetPositionProperties(positionItem, out var presetId, out var order, out var waitTime, out speed, out transition, presetList);
			PatrollingEdge patrollingEdge = new PatrollingEdge
			{
				Order = order
			};
			patrollingEdge.StartPosition.Preset.Key = presetId;
			patrollingEdge.StartPosition.WaitTime = TimeSpan.FromSeconds(waitTime);
			if (!speed.HasValue)
			{
				speed = 1.0;
			}
			if (!transition.HasValue)
			{
				transition = 3;
			}
			patrollingEdge.Transition.PanSpeed = speed.Value;
			patrollingEdge.Transition.TiltSpeed = speed.Value;
			patrollingEdge.Transition.ZoomSpeed = speed.Value;
			patrollingEdge.Transition.ExpectedTransitionTime = TimeSpan.FromSeconds(transition.Value);
			if (sortedDictionary.ContainsKey(order))
			{
				throw new PropertyInvalidException("Duplicate order", "Order");
			}
			sortedDictionary[order] = patrollingEdge;
		}
		schema.Edges = new ArrayList(sortedDictionary.Values);
	}

	private static void GetPositionProperties(ConfigurationItem positionItem, out string presetId, out int order, out int waitTime, out double? speed, out int? transition, Dictionary<string, string> presetList)
	{
		presetId = "";
		order = 0;
		speed = null;
		transition = null;
		waitTime = 3;
		if (positionItem.Properties == null)
		{
			return;
		}
		((IEnumerable<Property>)positionItem.Properties).TryGetPropertyValue("PresetId", out presetId);
		presetId = presetId.ToLowerInvariant();
		if (!presetList.ContainsKey(presetId))
		{
			throw new PropertyInvalidException("Preset not found", "PresetId");
		}
		if (((IEnumerable<Property>)positionItem.Properties).TryGetPropertyValue("Order", out string value) && int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out order) && order < 0)
		{
			throw new PropertyInvalidException("Order must be >= 0", "Order");
		}
		if (((IEnumerable<Property>)positionItem.Properties).TryGetPropertyValue("WaitTime", out string value2) && int.TryParse(value2, NumberStyles.Integer, CultureInfo.InvariantCulture, out waitTime) && waitTime < 0)
		{
			throw new PropertyInvalidException("wait time must be >= 0", "WaitTime");
		}
		if (((IEnumerable<Property>)positionItem.Properties).TryGetPropertyValue("Speed", out string value3) && double.TryParse(value3, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var result))
		{
			if (result < 0.0 || result > 1.0)
			{
				throw new PropertyInvalidException("speed", "Speed");
			}
			speed = result;
		}
		if (((IEnumerable<Property>)positionItem.Properties).TryGetPropertyValue("TransitionTime", out string value4) && int.TryParse(value4, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result2))
		{
			transition = result2;
		}
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder && !string.Equals(path.ParentItemType, "Camera", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static ConfigurationItem CreateInvokeInfoAddPatrollingProfile(IGeminiRequestContext requestContext, ConfigurationItem folderItem, ConfigurationItemPath folderPath, string methodId)
	{
		string id = folderPath.Id;
		if (!folderPath.IsFolder)
		{
			throw new NotSupportedException();
		}
		if (!Device.DeviceExists(requestContext, id))
		{
			throw new PathNotFoundException();
		}
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodIdAddPatrollingProfile;
		configurationItem.ParentPath = folderItem.ParentPath;
		configurationItem.Path = folderItem.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[9]
		{
			GeneralPropertyDefinitions.GetNameProperty(ConfigurationApiResources.PatrollingProfileDefaultName),
			GeneralPropertyDefinitions.GetDescriptionProperty(""),
			GeneralPropertyDefinitions.GetIdPropertyGuid(Guid.NewGuid()),
			PatrollingProfileDefinition.CustomizeTransitionsProperty(customizeTransitions: false),
			PatrollingProfileDefinition.InitSpeedProperty(1.0),
			PatrollingProfileDefinition.InitTransitionTimeProperty(3),
			PatrollingProfileDefinition.EndPresetIdProperty("", GetPresetList(requestContext, id)),
			PatrollingProfileDefinition.EndSpeedProperty(1.0),
			PatrollingProfileDefinition.EndTransitionTimeProperty(3)
		};
		configurationItem.Children = new ConfigurationItem[0];
		configurationItem.ChildrenFilled = true;
		return configurationItem;
	}

	private static ConfigurationItem AddPatrollingProfile(IGeminiRequestContext requestContext, ConfigurationItem invokeInfoFolderItem, ConfigurationItemPath path)
	{
		Device device = Device.QueryDeviceById(requestContext, path.Id);
		if (device == null)
		{
			throw new PathNotFoundException();
		}
		GetProfileProperties(invokeInfoFolderItem, out var name, out var description, out var speed, out var transition, out var customizeTransitions, out var id, out var endPresetId, out var endSpeed, out var endTransition);
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("Blank name not allowed");
		}
		using (IPatrollingProfileComponent patrollingProfileComponent = requestContext.CreatePatrollingProfileComponent())
		{
			PatrollingProfile[] array = patrollingProfileComponent.QueryPatrollingProfiles(device.Id);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Name.Equals(name))
				{
					throw new ArgumentException("Duplicate name");
				}
			}
		}
		PatrollingSchema patrollingSchema = ConstructSchema(id, speed, transition, customizeTransitions, endPresetId, endSpeed, endTransition);
		PatrollingProfile newPatrollingProfile = new PatrollingProfile
		{
			Name = name,
			Description = description,
			CameraDeviceId = device.Id,
			Id = id.ToString(),
			DefinitionXml = patrollingSchema.ToXml()
		};
		PatrollingProfile profile = PatrollingProfile.CreatePatrollingProfile(requestContext, newPatrollingProfile);
		Dictionary<string, string> presetList = GetPresetList(requestContext, path.Id);
		ConfigurationItem configurationItem = ConstructPatrollingItem(path, profile, patrollingSchema, presetList);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}

	private static ConfigurationItem CreateInvokeInfoAddPatrollingEntry(IGeminiRequestContext requestContext, ConfigurationItem profileItem, ConfigurationItemPath profilePath, string methodId)
	{
		if (profilePath.IsFolder || profilePath.ItemType != "PatrollingProfile")
		{
			throw new NotSupportedException();
		}
		PrepareData(requestContext, profileItem, out var parentPath, out var _, out var schema);
		int num = -1;
		foreach (PatrollingEdge edge in schema.Edges)
		{
			if (edge.Order > num)
			{
				num = edge.Order;
			}
		}
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodIdAddPatrollingEntry;
		configurationItem.ParentPath = profileItem.ParentPath;
		configurationItem.Path = profileItem.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Children = null;
		configurationItem.ChildrenFilled = true;
		List<Property> list = new List<Property>
		{
			PatrollingProfileDefinition.OrderProperty(num + 1),
			PatrollingProfileDefinition.PresetIdProperty("", GetPresetList(requestContext, parentPath.Id)),
			PatrollingProfileDefinition.WaitTimeProperty(3)
		};
		if (schema.ShowCustomizeTransitions)
		{
			list.Add(PatrollingProfileDefinition.SpeedProperty(1.0));
			list.Add(PatrollingProfileDefinition.TransitionTimeProperty(3));
		}
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem AddPatrollingEntry(IGeminiRequestContext requestContext, ConfigurationItem invokeInfoPatrollingItem)
	{
		PrepareData(requestContext, invokeInfoPatrollingItem, out var parentPath, out var profile, out var schema);
		Dictionary<string, string> presetList = GetPresetList(requestContext, schema.DeviceId.ToString());
		double? speed = null;
		int? transition = null;
		GetPositionProperties(invokeInfoPatrollingItem, out var presetId, out var order, out var waitTime, out speed, out transition, presetList);
		PatrollingEdge patrollingEdge = new PatrollingEdge
		{
			Order = order
		};
		patrollingEdge.StartPosition.Preset.Key = presetId;
		patrollingEdge.StartPosition.WaitTime = TimeSpan.FromSeconds(waitTime);
		if (!speed.HasValue)
		{
			speed = 1.0;
		}
		if (!transition.HasValue)
		{
			transition = 3;
		}
		patrollingEdge.Transition.PanSpeed = speed.Value;
		patrollingEdge.Transition.TiltSpeed = speed.Value;
		patrollingEdge.Transition.ZoomSpeed = speed.Value;
		patrollingEdge.Transition.ExpectedTransitionTime = TimeSpan.FromSeconds(transition.Value);
		List<PatrollingEdge> list = new List<PatrollingEdge>();
		foreach (PatrollingEdge edge in schema.Edges)
		{
			if (edge.Order == order)
			{
				throw new PropertyInvalidException("Duplicate order", "Order");
			}
			list.Add(edge);
		}
		list.Add(patrollingEdge);
		schema.Edges = new ArrayList(list);
		profile.DefinitionXml = schema.ToXml();
		PatrollingProfile.UpdatePatrollingProfile(requestContext, profile);
		ConfigurationItem configurationItem = ConstructPatrollingItem(parentPath, profile, schema, presetList);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}

	private static void PrepareData(IGeminiRequestContext requestContext, ConfigurationItem patrollingItem, out ConfigurationItemPath parentPath, out PatrollingProfile profile, out PatrollingSchema schema)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(patrollingItem.Path);
		parentPath = ConfigurationItemPath.CreatePath(patrollingItem.ParentPath);
		Device device = Device.QueryDeviceById(requestContext, parentPath.Id);
		if (device == null)
		{
			throw new PathNotFoundException();
		}
		profile = PatrollingProfile.QueryPatrollingProfiles(requestContext, new string[1] { configurationItemPath.Id })?.FirstOrDefault();
		if (profile == null)
		{
			throw new PathNotFoundException();
		}
		schema = new PatrollingSchema(new Guid(profile.Id));
		schema.DeviceId = new Guid(device.Id);
		schema.LoadXml(profile.DefinitionXml);
	}

	private static PatrollingSchema ConstructSchema(Guid id, double speed, int transition, bool customizeTransitions, string endPresetId, double endSpeed, int endTransition)
	{
		PatrollingSchema patrollingSchema = new PatrollingSchema(id);
		patrollingSchema.InitialTransition = new PatrollingTransition
		{
			ExpectedTransitionTime = TimeSpan.FromSeconds(transition),
			PanSpeed = speed,
			TiltSpeed = speed,
			ZoomSpeed = speed
		};
		patrollingSchema.ShowCustomizeTransitions = customizeTransitions;
		patrollingSchema.Edges = new ArrayList();
		if (!string.IsNullOrEmpty(endPresetId))
		{
			patrollingSchema.EndPosition = new PatrollingEdge
			{
				Order = 0
			};
			patrollingSchema.EndPosition.StartPosition.Preset.Key = endPresetId;
			patrollingSchema.EndPosition.StartPosition.WaitTime = TimeSpan.FromSeconds(0.0);
			patrollingSchema.EndPosition.Transition.PanSpeed = endSpeed;
			patrollingSchema.EndPosition.Transition.TiltSpeed = endSpeed;
			patrollingSchema.EndPosition.Transition.ZoomSpeed = endSpeed;
			patrollingSchema.EndPosition.Transition.ExpectedTransitionTime = TimeSpan.FromSeconds(endTransition);
		}
		else
		{
			patrollingSchema.EndPosition = null;
		}
		return patrollingSchema;
	}

	private static void GetProfileProperties(ConfigurationItem item, out string name, out string description, out double speed, out int transition, out bool customizeTransitions, out Guid id, out string endPresetId, out double endSpeed, out int endTransition)
	{
		string value = "1";
		string value2 = "3";
		string value3 = "1";
		string value4 = "3";
		if (item.Properties == null)
		{
			throw new ArgumentException("No properties provided");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name))
		{
			throw new PropertyInvalidException("Name property missing", "Name");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Id", out string value5) || !Guid.TryParse(value5, out id))
		{
			throw new PropertyInvalidException("Id property missing or invalid", "Id");
		}
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("InitSpeed", out value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("InitTransitionTime", out value2);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CustomizeTransitions", out string value6);
		if (!double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out speed) || speed <= 0.0 || speed > 1.0)
		{
			throw new PropertyInvalidException("Not a double, or invalid range", "InitSpeed");
		}
		if (!int.TryParse(value2, NumberStyles.Integer, CultureInfo.InvariantCulture, out transition) || transition <= 0)
		{
			throw new PropertyInvalidException("Not an valid transition value", "InitTransitionTime");
		}
		if (!bool.TryParse(value6, out customizeTransitions))
		{
			throw new PropertyInvalidException("Not a boolean value", "CustomizeTransitions");
		}
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EndPresetId", out endPresetId);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EndSpeed", out value3);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EndTransitionTime", out value4);
		if (!string.IsNullOrEmpty(endPresetId))
		{
			if (!double.TryParse(value3, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out endSpeed) || endSpeed <= 0.0 || endSpeed > 1.0)
			{
				throw new PropertyInvalidException("Not a double, or invalid range", "EndSpeed");
			}
			if (!int.TryParse(value4, NumberStyles.Integer, CultureInfo.InvariantCulture, out endTransition) || endTransition <= 0)
			{
				throw new PropertyInvalidException("Not an valid transition value", "EndTransitionTime");
			}
		}
		else
		{
			endSpeed = 1.0;
			endTransition = 3;
		}
	}

	private static ConfigurationItem CreateInvokeInfoRemovePatrollingProfile(IGeminiRequestContext requestContext, ConfigurationItemPath folderPath, ConfigurationItem folderItem, string methodId)
	{
		string id = folderPath.Id;
		if (!folderPath.IsFolder)
		{
			throw new NotSupportedException();
		}
		if (!Device.DeviceExists(requestContext, id))
		{
			throw new PathNotFoundException();
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		PatrollingProfile[] array = PatrollingProfile.QueryPatrollingProfiles(requestContext, id);
		foreach (PatrollingProfile patrollingProfile in array)
		{
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("PatrollingProfile", patrollingProfile.Id);
			dictionary.Add(configurationItemPath.Text, patrollingProfile.Name);
		}
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemovePatrollingProfile, "PropertyRemovePatrollingProfile", ConfigurationApiResources.PropertyRemovePatrollingProfile, folderItem, dictionary);
	}

	private static ConfigurationItem RemovePatrollingProfile(IGeminiRequestContext requestContext, ConfigurationItem invokeInfoFolderItem, ConfigurationItemPath path)
	{
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(invokeInfoFolderItem);
		try
		{
			PatrollingProfile.DeletePatrollingProfile(requestContext, invokeMemberItemSelectionPath.Id);
		}
		catch (RemoveNonExistingPatrollingProfileException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(invokeInfoFolderItem);
	}

	private static ConfigurationItem CreateInvokeInfoRemovePatrollingEntry(IGeminiRequestContext requestContext, ConfigurationItem profileItem, ConfigurationItemPath profilePath, string methodId)
	{
		if (profilePath.IsFolder || profilePath.ItemType != "PatrollingProfile")
		{
			throw new NotSupportedException();
		}
		PrepareData(requestContext, profileItem, out var _, out var _, out var schema);
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (PatrollingEdge edge in schema.Edges)
		{
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("PatrollingEntry", edge.Order.ToString(CultureInfo.InvariantCulture));
			dictionary.Add(configurationItemPath.Text, string.Format(CultureInfo.InvariantCulture, "Order - {0}", edge.Order));
		}
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemovePatrollingEntry, "WizardRemovePatrollingEntry", ConfigurationApiResources.PropertyRemovePatrollingEntry, profileItem, dictionary);
	}

	private static ConfigurationItem RemovePatrollingEntry(IGeminiRequestContext requestContext, ConfigurationItem invokeInfoProfileItem)
	{
		PrepareData(requestContext, invokeInfoProfileItem, out var _, out var profile, out var schema);
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(invokeInfoProfileItem);
		if (!int.TryParse(invokeMemberItemSelectionPath.Id, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			throw new PathNotFoundException();
		}
		bool flag = false;
		List<PatrollingEdge> list = new List<PatrollingEdge>();
		int num = 0;
		foreach (PatrollingEdge edge in schema.Edges)
		{
			if (edge.Order == result)
			{
				flag = true;
				continue;
			}
			edge.Order = num++;
			list.Add(edge);
		}
		if (!flag)
		{
			throw new PathNotFoundException();
		}
		schema.Edges = new ArrayList(list);
		profile.DefinitionXml = schema.ToXml();
		try
		{
			PatrollingProfile.UpdatePatrollingProfile(requestContext, profile);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		return MethodTypesHelper.CreateInvokeResult(invokeInfoProfileItem, invokeInfoProfileItem.Path, "Result", "Success", false);
	}

	private static ConfigurationItem ConstructPatrollingItem(ConfigurationItemPath parentPath, PatrollingProfile profile, PatrollingSchema schema, Dictionary<string, string> presetList)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("PatrollingProfile", schema.Id.ToString());
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.ItemType = "PatrollingProfile";
		configurationItem.Path = configurationItemPath.Text;
		configurationItem.DisplayName = profile.Name;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.MethodIds = new string[2] { "AddPatrollingEntry", "RemovePatrollingEntry" };
		configurationItem.ChildrenFilled = true;
		ConfigurationItem configurationItem2 = configurationItem;
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetNameProperty(profile.Name));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(profile.Description));
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(profile.Id));
		list.Add(PatrollingProfileDefinition.CustomizeTransitionsProperty(schema.ShowCustomizeTransitions));
		list.Add(PatrollingProfileDefinition.InitSpeedProperty(schema.InitialTransition.PanSpeed));
		list.Add(PatrollingProfileDefinition.InitTransitionTimeProperty((int)schema.InitialTransition.ExpectedTransitionTime.TotalSeconds));
		list.Add(PatrollingProfileDefinition.EndPresetIdProperty(schema.EndPosition?.StartPosition?.Preset.Key, presetList));
		list.Add(PatrollingProfileDefinition.EndSpeedProperty(schema.EndPosition.Transition.PanSpeed));
		list.Add(PatrollingProfileDefinition.EndTransitionTimeProperty((int)schema.EndPosition.Transition.ExpectedTransitionTime.TotalSeconds));
		configurationItem2.Properties = list.ToArray();
		List<ConfigurationItem> list2 = new List<ConfigurationItem>();
		foreach (PatrollingEdge edge in schema.Edges)
		{
			list2.Add(ConvertToPatrollingEntryConfigurationItem(configurationItemPath, edge.Order, edge.StartPosition, schema.ShowCustomizeTransitions ? edge.Transition : null, presetList));
		}
		configurationItem2.Children = list2.ToArray();
		return configurationItem2;
	}

	private static ConfigurationItem ConvertToPatrollingEntryConfigurationItem(ConfigurationItemPath parentPath, int order, PatrollingPosition patrollingPosition, PatrollingTransition patrollingTransition, Dictionary<string, string> presetList)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("PatrollingEntry", order.ToString(CultureInfo.InvariantCulture));
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemCategory = ItemCategories.ChildItem,
			ItemType = "PatrollingEntry",
			ParentPath = parentPath.Text,
			Path = configurationItemPath.Text,
			DisplayName = ConfigurationApiResources.ItemTypePatrollingEntry,
			UIHint = "DirectionHorizontal",
			MethodIds = new string[0],
			ChildrenFilled = true,
			Children = null
		};
		List<Property> list = new List<Property>
		{
			PatrollingProfileDefinition.OrderProperty(order),
			PatrollingProfileDefinition.PresetIdProperty(patrollingPosition?.Preset.Key, presetList),
			PatrollingProfileDefinition.WaitTimeProperty((int)(patrollingPosition?.WaitTime.TotalSeconds ?? 3.0))
		};
		if (patrollingTransition != null)
		{
			list.Add(PatrollingProfileDefinition.SpeedProperty(patrollingTransition.PanSpeed));
			list.Add(PatrollingProfileDefinition.TransitionTimeProperty((int)patrollingTransition.ExpectedTransitionTime.TotalSeconds));
		}
		obj.Properties = list.ToArray();
		return obj;
	}

	private static Dictionary<string, string> GetPresetList(IGeminiRequestContext requestContext, string cameraId)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("", ConfigurationApiResources.PatrollingProfileNoPresetSelected);
		foreach (PtzPreset item in DevicePtzPresets.QueryPtzPresets(requestContext, cameraId))
		{
			dictionary.Add(item.Id.ToString(), item.Name);
		}
		return dictionary;
	}
}
