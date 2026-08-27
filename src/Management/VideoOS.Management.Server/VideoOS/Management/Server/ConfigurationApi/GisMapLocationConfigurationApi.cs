#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.GisMapCommunication.Exceptions;
using VideoOS.GisMapCommunication.Item;
using VideoOS.GisMapCommunication.Layer;
using VideoOS.GisMapSharedData.CommandExceptions;
using VideoOS.GisMapSharedData.DataContracts;
using VideoOS.GisMapSharedData.DataContracts.Item;
using VideoOS.GisMapSharedData.DataContracts.Layer;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Platform;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIGisMapLocation")]
[EntityName("ConfigAPIGisMapLocationFolder")]
public class GisMapLocationConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public const string ScaleKey = "Scale";

	public const string PositionXKey = "PositionX";

	public const string PositionYKey = "PositionY";

	public const string ColorKey = "Color";

	private const string DefaultColor = "#FFFF5611";

	private static Guid _locationLayerId = new Guid("CEA7BCDA-09BA-4EEB-AE20-31198F3BCC54");

	private static Guid _mapId = new Guid("CEA7BCDA-59BA-4EEB-AE20-31198F3BCC54");

	private static IItemClient GetClient(IGeminiRequestContext requestContext)
	{
		MIPFactory.CheckAndInitialize(requestContext);
		return new ItemClientManager().GetClient(Configuration.Instance.ServerFQID.ServerId);
	}

	private static ILayerClient GetLayerClient(IGeminiRequestContext requestContext)
	{
		MIPFactory.CheckAndInitialize(requestContext);
		return new LayerClientManager().GetClient(Configuration.Instance.ServerFQID.ServerId);
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		string name = "";
		double scale = 1.0;
		Position position = new Position();
		string color = "#FFFF5611";
		Guid iconId = Guid.Empty;
		ValidateResult validateResult = ValidateItemInternal(item, out name, out scale, out position, out color, out iconId);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		IItemClient client = GetClient(requestContext);
		GetItemResp item2 = client.GetItem(new GetItemParams
		{
			Key = new Guid(path.Id)
		});
		if (item2 == null || item2.Item == null)
		{
			throw new ArgumentException("Current GisMapLocation could not be found", "path");
		}
		LocationMapItemData locationMapItemData = item2.Item as LocationMapItemData;
		locationMapItemData.Color = color;
		locationMapItemData.Scale = scale;
		locationMapItemData.Position = position;
		locationMapItemData.Name = name;
		locationMapItemData.IconId = iconId;
		client.UpdateItem(new UpdateItemParams
		{
			Key = locationMapItemData
		});
		item2 = client.GetItem(new GetItemParams
		{
			Key = new Guid(path.Id)
		});
		validateResult = new ValidateResult();
		validateResult.ResultItem = ConvertToGisMapLocationConfigurationItem(parentPath, item2.Item as LocationMapItemData);
		validateResult.ValidatedOk = true;
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder)
		{
			return ValidateItemInternal(item, path);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(ConfigurationItem item, ConfigurationItemPath path)
	{
		try
		{
			if (!path.IsFolder)
			{
				string name = "";
				double scale = 1.0;
				Position position = new Position();
				string color = "#FFFF5611";
				Guid iconId = Guid.Empty;
				return ValidateItemInternal(item, out name, out scale, out position, out color, out iconId);
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

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			GetItemsByLayerResp getItemsByLayerResp = null;
			try
			{
				getItemsByLayerResp = GetClient(requestContext).GetItemsByLayer(new GetItemsByLayerParams
				{
					Key = new MapLayerBrowseData
					{
						Id = _locationLayerId
					}
				});
			}
			catch (ServiceNotRegisteredException)
			{
			}
			catch (EndpointNotFoundException)
			{
			}
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			if (getItemsByLayerResp != null)
			{
				MapItemData[] items = getItemsByLayerResp.Items;
				for (int i = 0; i < items.Length; i++)
				{
					if (items[i] is LocationMapItemData location)
					{
						list.Add(ConvertToGisMapLocationConfigurationItem(path, location));
					}
				}
			}
			return list.ToArray();
		}
		return new ConfigurationItem[0];
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("GisMapLocationFolder", ConfigurationItemPath.CreateSystemPath());
		}
		if (!Guid.TryParse(path.Id, out var result))
		{
			throw new ArgumentException("Path does not contains a valid GUID", "path");
		}
		GetItemResp item = GetClient(requestContext).GetItem(new GetItemParams
		{
			Key = result
		});
		if (item == null || !(item.Item is LocationMapItemData))
		{
			throw new PathNotFoundException(path.Text);
		}
		return ConvertToGisMapLocationConfigurationItem(new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "GisMapLocationFolder").CreatePath(), (LocationMapItemData)item.Item);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!item.MethodIds.Contains(method))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", method, item.ItemType), "method");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		if (!(method == "AddGisMapLocation"))
		{
			if (method == "RemoveGisMapLocation")
			{
				if (!flag)
				{
					return CreateInvokeInfoRemoveGisMapLocation(item, GetChildItems(requestContext, path), method);
				}
				return RemoveGisMapLocation(requestContext, item, path);
			}
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
		}
		if (!flag)
		{
			return CreateInvokeInfoAddGisMapLocation(item, method);
		}
		return AddGisMapLocation(requestContext, item, path);
	}

	private static ConfigurationItem CreateInvokeInfoAddGisMapLocation(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddGisMapLocation;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[5]
		{
			GeneralPropertyDefinitions.GetNameProperty("", isSettable: true),
			GisMapLocationProperties.ScaleProperty(1.0, isSettable: true),
			GisMapLocationProperties.ColorProperty("#FFFF5611", isSettable: true),
			GisMapLocationProperties.PositionXProperty(0.0, isSettable: true),
			GisMapLocationProperties.PositionYProperty(0.0, isSettable: true)
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveGisMapLocation(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveGisMapLocation, "PropertyRemoveGisMapLocation", ConfigurationApiResources.PropertyRemoveGisMapLocation, item, members);
	}

	private static ConfigurationItem AddGisMapLocation(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string name = "";
		double scale = 1.0;
		Position position = new Position();
		string color = "#FFFF5611";
		Guid iconId = Guid.Empty;
		ValidateResult validateResult = ValidateItemInternal(item, out name, out scale, out position, out color, out iconId);
		if (!validateResult.ValidatedOk)
		{
			string message = "Incorrect properties";
			if (validateResult.ErrorResults != null && validateResult.ErrorResults.Length != 0)
			{
				message = validateResult.ErrorResults[0].ErrorText;
			}
			throw new PropertyInvalidException(message, "item");
		}
		try
		{
			LocationMapItemData locationMapItemData = new LocationMapItemData
			{
				Id = Guid.NewGuid(),
				LayerId = _locationLayerId,
				Name = name,
				Position = position,
				Scale = scale,
				Color = color,
				IconId = iconId
			};
			IItemClient client = GetClient(requestContext);
			AddItemParams parameters = new AddItemParams
			{
				Key = locationMapItemData
			};
			try
			{
				client.AddItem(parameters);
			}
			catch (MapCommandSqlDatabaseException)
			{
				Trace.WriteLine("GisMapLocation: Creating default location layer");
				CreateLocationLayer(requestContext);
				client.AddItem(parameters);
			}
			ConfigurationItem configurationItem = ConvertToGisMapLocationConfigurationItem(path, locationMapItemData);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (Exception ex2)
		{
			throw new PropertyInvalidException(ex2.Message);
		}
	}

	private static void CreateLocationLayer(IGeminiRequestContext requestContext)
	{
		LocationItemsLayerData key = new LocationItemsLayerData
		{
			Id = _locationLayerId,
			MapId = _mapId,
			Name = "Locations",
			IsVisible = true
		};
		AddLayerParams parameters = new AddLayerParams
		{
			Key = key
		};
		GetLayerClient(requestContext).AddLayer(parameters);
	}

	private static ConfigurationItem RemoveGisMapLocation(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		if (!Guid.TryParse(invokeMemberItemSelectionPath.Id, out var result))
		{
			throw new ArgumentException("Current GisMapLocation not found", "item");
		}
		try
		{
			GetClient(requestContext).DeleteItem(new DeleteItemParams
			{
				Key = result
			});
		}
		catch (Exception)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static ValidateResult ValidateItemInternal(ConfigurationItem item, out string name, out double scale, out Position position, out string color, out Guid iconId)
	{
		name = "";
		scale = 1.0;
		position = new Position();
		color = "#FFFF5611";
		iconId = Guid.Empty;
		try
		{
			if (item.ItemType == "GisMapLocation" || item.ItemType == "InvokeInfo")
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
					if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Scale", out string value) && !double.TryParse(value, out scale))
					{
						throw new ArgumentException("Scale not a valid double", "scale");
					}
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Color", out color);
					if (!ValidateColor(color))
					{
						throw new ArgumentException("Color value is not valid", "color");
					}
					string value2 = "0";
					string value3 = "0";
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PositionX", out value2);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PositionY", out value3);
					if (!double.TryParse(value2, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) || !double.TryParse(value3, NumberStyles.Any, CultureInfo.InvariantCulture, out var result2))
					{
						throw new ArgumentException("X or Y is not a valid double", "position");
					}
					position.X = result;
					position.Y = result2;
					position.Z = 0.0;
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

	private static bool ValidateColor(string color)
	{
		if (color == null || color[0] != '#' || color.Length != 9)
		{
			return false;
		}
		string text = color.ToUpper(CultureInfo.InvariantCulture);
		for (int i = 1; i < 9; i++)
		{
			char c = text[i];
			if ((c < '0' || c > '9') && (c < 'A' || c > 'F'))
			{
				return false;
			}
		}
		return true;
	}

	private static ConfigurationItem ConvertToGisMapLocationConfigurationItem(ConfigurationItemPath parentPath, LocationMapItemData location)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "GisMapLocation",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = location.Name,
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, location.Id.ToString()).Text;
		obj.Properties = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(location.Id.ToString()),
			GeneralPropertyDefinitions.GetNameProperty(location.Name, isSettable: true),
			GisMapLocationProperties.ScaleProperty(location.Scale, isSettable: true),
			GisMapLocationProperties.ColorProperty(location.Color, isSettable: true),
			GisMapLocationProperties.PositionXProperty(location.Position.X, isSettable: true),
			GisMapLocationProperties.PositionYProperty(location.Position.Y, isSettable: true)
		}.ToArray();
		List<string> list = new List<string>();
		obj.MethodIds = list.ToArray();
		return obj;
	}
}
