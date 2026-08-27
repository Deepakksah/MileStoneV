using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.ConfigurationApi.AccessControl;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPISystem")]
[EntityName("ConfigAPISystemAddressFolder")]
[EntityName("ConfigAPISystemAddress")]
public class ManagementServerConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (string.Equals(path.ItemType, "System", StringComparison.Ordinal))
		{
			ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var name, out var description);
			if (!validateResult.ValidatedOk)
			{
				return validateResult;
			}
			ServerInformation serverInformation = ServerInformation.UpdateServerInformation(requestContext, name, description, null, null);
			Site masterSite = null;
			if (!string.IsNullOrEmpty(serverInformation.MasterSite))
			{
				try
				{
					masterSite = Site.QueryMasterSite(requestContext);
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
			ConfigurationItem resultItem = ConvertToConfigurationItem(serverInformation, masterSite);
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = resultItem;
			return validateResult;
		}
		throw new NotSupportedException();
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		string name;
		string description;
		return ValidateItemInternal(requestContext, item, path, out name, out description);
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out string name, out string description)
	{
		name = null;
		description = null;
		try
		{
			if (string.Equals(path.ItemType, "System", StringComparison.Ordinal))
			{
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
				ServerInformation.UpdateServerInformationValidation(requestContext, name, description, null);
				ValidateResult validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = item;
				return validateResult;
			}
			throw new NotSupportedException();
		}
		catch (ArgumentNullException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
		catch (ArgumentException exception2)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception2, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		string itemType = path.ItemType;
		ServerInformation serverInformation = ServerInformation.GetServerInformation(requestContext);
		if (!string.IsNullOrEmpty(path.Id) && itemType == "System" && string.Compare(serverInformation.Id, path.Id, StringComparison.OrdinalIgnoreCase) != 0)
		{
			throw new PathNotFoundException(path.Text);
		}
		if (string.Equals(itemType, "System", StringComparison.Ordinal))
		{
			ConfigurationItem item = GroupConfigurationItemFactory.CreateFolderItem("SystemAddressFolder", path);
			ConfigurationItem item2 = GroupConfigurationItemFactory.CreateFolderItem("RecordingServerFolder", path);
			ConfigurationItem item3 = GroupConfigurationItemFactory.CreateFolderItem("ServerFolder", path);
			ConfigurationItem item4 = GroupConfigurationItemFactory.CreateFolderItem("CameraGroupFolder", path);
			ConfigurationItem item5 = GroupConfigurationItemFactory.CreateFolderItem("MicrophoneGroupFolder", path);
			ConfigurationItem item6 = GroupConfigurationItemFactory.CreateFolderItem("SpeakerGroupFolder", path);
			ConfigurationItem item7 = GroupConfigurationItemFactory.CreateFolderItem("InputEventGroupFolder", path);
			ConfigurationItem item8 = GroupConfigurationItemFactory.CreateFolderItem("OutputGroupFolder", path);
			ConfigurationItem item9 = GroupConfigurationItemFactory.CreateFolderItem("MetadataGroupFolder", path);
			ConfigurationItem item10 = GroupConfigurationItemFactory.CreateFolderItem("LoginProviderFolder", path);
			ConfigurationItem item11 = GroupConfigurationItemFactory.CreateFolderItem("BasicUserFolder", path);
			ConfigurationItem item12 = GroupConfigurationItemFactory.CreateFolderItem("RoleFolder", path);
			ConfigurationItem item13 = GroupConfigurationItemFactory.CreateFolderItem("LayoutGroupFolder", path);
			ConfigurationItem item14 = GroupConfigurationItemFactory.CreateFolderItem("VideoWallFolder", path);
			ConfigurationItem item15 = GroupConfigurationItemFactory.CreateFolderItem("UserDefinedEventFolder", path);
			ConfigurationItem item16 = GroupConfigurationItemFactory.CreateFolderItem("AnalyticsEventFolder", path);
			ConfigurationItem item17 = GroupConfigurationItemFactory.CreateFolderItem("GenericEventFolder", path);
			ConfigurationItem item18 = GroupConfigurationItemFactory.CreateFolderItem("GenericEventDataSourceFolder", path);
			ConfigurationItem item19 = GroupConfigurationItemFactory.CreateFolderItem("GisMapLocationFolder", path);
			ConfigurationItem item20 = GroupConfigurationItemFactory.CreateFolderItem("TimeProfileFolder", path);
			ConfigurationItem item21 = GroupConfigurationItemFactory.CreateFolderItem("AlarmDefinitionFolder", path);
			ConfigurationItem item22 = GroupConfigurationItemFactory.CreateFolderItem("MIPKindFolder", path);
			ConfigurationItem item23 = GroupConfigurationItemFactory.CreateFolderItem("LprMatchListFolder", path);
			ConfigurationItem item24 = GroupConfigurationItemFactory.CreateFolderItem("SaveSearchesFolder", path);
			ConfigurationItem item25 = GroupConfigurationItemFactory.CreateFolderItem("RuleFolder", path);
			ConfigurationItem item26 = VideoOS.Management.Server.ConfigurationApi.AccessControl.ConfigurationApiService.CreateAccessControlSystemFolderItem(path);
			ConfigurationItem item27 = GroupConfigurationItemFactory.CreateFolderItem("SiteFolder", path);
			ConfigurationItem item28 = GroupConfigurationItemFactory.CreateFolderItem("LicenseInformationFolder", path);
			ConfigurationItem item29 = GroupConfigurationItemFactory.CreateFolderItem("BasicOwnerInformationFolder", path);
			ConfigurationItem item30 = GroupConfigurationItemFactory.CreateFolderItem("ToolOptionFolder", path);
			ConfigurationItem item31 = GroupConfigurationItemFactory.CreateFolderItem("ViewGroupFolder", path);
			ConfigurationItem item32 = GroupConfigurationItemFactory.CreateFolderItem("FailoverGroupFolder", path);
			ConfigurationItem item33 = GroupConfigurationItemFactory.CreateFolderItem("ClientProfileFolder", path);
			ConfigurationItem item34 = GroupConfigurationItemFactory.CreateFolderItem("EventTypeGroupFolder", path);
			ConfigurationItem item35 = GroupConfigurationItemFactory.CreateFolderItem("StateGroupFolder", path);
			ConfigurationItem item36 = GroupConfigurationItemFactory.CreateFolderItem("MatrixFolder", path);
			ConfigurationItem item37 = GroupConfigurationItemFactory.CreateFolderItem("EvidenceLockProfileFolder", path);
			ConfigurationItem item38 = GroupConfigurationItemFactory.CreateFolderItem("CompressorFolder", path);
			ConfigurationItem item39 = GroupConfigurationItemFactory.CreateFolderItem("MailNotificationProfileFolder", path);
			ConfigurationItem item40 = GroupConfigurationItemFactory.CreateFolderItem("AccessControlGroupFolder", path);
			List<ConfigurationItem> list = new List<ConfigurationItem>
			{
				item, item2, item3, item4, item5, item6, item7, item8, item9, item10,
				item11, item12, item15, item16, item17, item18, item13, item20, item21, item22,
				item23, item25, item26, item28, item29, item30, item31, item34, item35, item36,
				item38, item39, item40
			};
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartWall"))
			{
				list.Add(item14);
			}
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartMap"))
			{
				list.Add(item19);
			}
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SaveSearches"))
			{
				list.Add(item24);
			}
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "FederatedSites"))
			{
				list.Add(item27);
			}
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "RecordingServerFailover"))
			{
				list.Add(item32);
			}
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartClientProfiles"))
			{
				list.Add(item33);
			}
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EvidenceLock"))
			{
				list.Add(item37);
			}
			return list.ToArray();
		}
		if (string.Equals(itemType, "SystemAddressFolder", StringComparison.Ordinal))
		{
			List<ConfigurationItem> list2 = new List<ConfigurationItem>();
			int num = 0;
			ServerAddress[] addresses = serverInformation.Addresses;
			foreach (ServerAddress addresses2 in addresses)
			{
				num++;
				ConfigurationItem item41 = ConvertToConfigurationItem(path, addresses2, num);
				list2.Add(item41);
			}
			return list2.ToArray();
		}
		if (string.Equals(itemType, "SystemAddress", StringComparison.Ordinal))
		{
			return new ConfigurationItem[0];
		}
		throw new NotSupportedException();
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		string itemType = path.ItemType;
		if (string.Equals(itemType, "System", StringComparison.Ordinal))
		{
			ServerInformation serverInformation = ServerInformation.GetServerInformation(requestContext);
			if (!string.IsNullOrEmpty(path.Id) && string.Compare(serverInformation.Id, path.Id, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new PathNotFoundException(path.Text);
			}
			Site masterSite = null;
			if (!string.IsNullOrEmpty(serverInformation.MasterSite))
			{
				try
				{
					masterSite = Site.QueryMasterSite(requestContext);
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
			return ConvertToConfigurationItem(serverInformation, masterSite);
		}
		if (string.Equals(itemType, "SystemAddressFolder", StringComparison.Ordinal))
		{
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateSystemPath();
			return GroupConfigurationItemFactory.CreateFolderItem("SystemAddressFolder", parentPath);
		}
		throw new NotSupportedException();
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		if (methodId == null)
		{
			throw new ArgumentNullException("methodId");
		}
		if (!item.MethodIds.Contains(methodId))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", methodId, item.ItemType), "methodId");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (methodId)
		{
		case "LoadTasks":
			if (!flag)
			{
				return CreateInvokeInfoItemLoadTasks(item, methodId);
			}
			return LoadTasks(requestContext, item);
		case "ClientLogOnSupported":
			if (!flag)
			{
				return CreateInvokeInfoItemClientLogOn(requestContext, item, methodId);
			}
			return ClientLogOn(requestContext, item);
		case "UploadFileChunk":
			if (!flag)
			{
				return CreateInvokeInfoItemUploadFileChunk(item, methodId);
			}
			return UploadFileChunk(item);
		default:
			throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
		}
	}

	private static ConfigurationItem CreateInvokeInfoItemLoadTasks(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardLoadTasks;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[2]
		{
			ManagementServerPropertyDefinition.IncludeRecordingServerTasksProperty(value: false),
			TaskConfigurationApi.GetTaskTypeFilterProperty(string.Empty)
		};
		return configurationItem;
	}

	private static ConfigurationItem LoadTasks(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		try
		{
			bool result = false;
			if (item.Properties != null && ((IEnumerable<Property>)item.Properties).TryGetPropertyValue("IncludeRecordingServerTasks", out string value) && !bool.TryParse(value, out result))
			{
				result = false;
			}
			string taskType = string.Empty;
			if (item.Properties != null && ((IEnumerable<Property>)item.Properties).TryGetPropertyValue("TaskType", out string value2))
			{
				taskType = value2;
			}
			Task[] array = null;
			if (result)
			{
				string[] taskIds = Task.QueryAllTaskIds(requestContext);
				array = Task.QueryTasks(requestContext, taskIds);
			}
			else
			{
				array = Task.QueryTasksForManagementServer(requestContext);
			}
			if (!string.IsNullOrEmpty(taskType))
			{
				array = array.Where((Task t) => string.Equals(t.Type.ToString(), taskType, StringComparison.Ordinal)).ToArray();
			}
			return TaskHelperFunctions.CreateTasksInvokeResultConfigurationItem(requestContext, array, item);
		}
		catch (Exception ex)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex);
		}
	}

	private static ConfigurationItem CreateInvokeInfoItemClientLogOn(IGeminiRequestContext requestContext, ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardClientLogOnSupported;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		List<Property> list = new List<Property>();
		list.Add(ClientTypeProperty(ClientType.SmartClient));
		list.Add(RecordingServerConfigurationApi.UserNameProperty(requestContext.UserDomainAccountName));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem ClientLogOn(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		try
		{
			if (item.Properties == null)
			{
				throw new ArgumentException("Item properties collection is null");
			}
			if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ClientApplicationType", out ClientType? value))
			{
				throw new PropertyInvalidException("No client application type was provided.", "ClientApplicationType");
			}
			if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("UserName", out string value2))
			{
				throw new PropertyInvalidException("No user name was specified.", "UserName");
			}
			string sidBySearchFactor = IdentityAccessor.GetSidBySearchFactor(requestContext, SearchFactor.AccountName, value2);
			if (sidBySearchFactor == null)
			{
				throw new PropertyInvalidException("User is not part of the system.", "UserName");
			}
			bool allowClientLogOn = Role.AllowClientLogOn(requestContext, sidBySearchFactor, value.Value);
			item.Properties = new Property[1] { AllowClientLogOnProperty(allowClientLogOn) };
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (Exception ex)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex);
		}
	}

	private static ConfigurationItem CreateInvokeInfoItemUploadFileChunk(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodUploadFileChunk;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		List<Property> list = new List<Property>();
		list.Add(FileUploadPropertyDefinition.TransferIdProperty(Guid.NewGuid()));
		list.Add(FileUploadPropertyDefinition.ChunkDataProperty(""));
		list.Add(FileUploadPropertyDefinition.ChunkOffsetProperty(0));
		list.Add(FileUploadPropertyDefinition.FileSizeProperty(0L));
		list.Add(FileUploadPropertyDefinition.FileChecksumProperty(""));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem UploadFileChunk(ConfigurationItem item)
	{
		try
		{
			if (item.Properties == null)
			{
				throw new ArgumentException("Item properties collection is null");
			}
			if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("TransferId", out string value))
			{
				throw new PropertyInvalidException("No Transfer Id property specified.", "TransferId");
			}
			if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ChunkData", out string value2))
			{
				throw new PropertyInvalidException("No Chunk Data property specified.", "ChunkData");
			}
			if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Offset", out string value3))
			{
				throw new PropertyInvalidException("No Offset property specified.", "Offset");
			}
			if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Size", out string value4))
			{
				throw new PropertyInvalidException("No Size property specified.", "Size");
			}
			if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Checksum", out string value5))
			{
				throw new PropertyInvalidException("No Checksum property specified.", "Checksum");
			}
			Guid transferId = new Guid(value);
			byte[] chunkData = Convert.FromBase64String(value2);
			int offset = int.Parse(value3, CultureInfo.InvariantCulture);
			long fileSize = long.Parse(value4, CultureInfo.InvariantCulture);
			byte[] fileChecksum = Convert.FromBase64String(value5);
			FileUpload.FileUploadResult fileUploadResult = FileUpload.UploadFileChunk(transferId, chunkData, offset, fileSize, fileChecksum);
			if (!string.IsNullOrEmpty(fileUploadResult?.StorageId))
			{
				item.Properties = new Property[1] { FileUploadPropertyDefinition.FileStorageIdProperty(fileUploadResult.StorageId) };
			}
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (Exception ex)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex);
		}
	}

	private static ConfigurationItem ConvertToConfigurationItem(ServerInformation serverInformation, Site masterSite)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = serverInformation.Name;
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemType = "System";
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(serverInformation.Id));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(serverInformation.Name));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(serverInformation.Description));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(serverInformation.LastModified));
		list.Add(ManagementServerPropertyDefinition.TimeZoneProperty(serverInformation.TimeZone));
		list.Add(ManagementServerPropertyDefinition.ComputerNameProperty(serverInformation.ComputerName));
		list.Add(ManagementServerPropertyDefinition.DomainNameProperty(serverInformation.DomainName));
		list.Add(ManagementServerPropertyDefinition.LastStatusHandshakeProperty(serverInformation.LastStatusHandshake));
		list.Add(ManagementServerPropertyDefinition.PhysicalMemoryProperty(serverInformation.PhysicalMemory));
		list.Add(ManagementServerPropertyDefinition.PlatformProperty(serverInformation.Platform));
		list.Add(ManagementServerPropertyDefinition.ProcessorsProperty(serverInformation.Processors));
		list.Add(ManagementServerPropertyDefinition.ServiceAccountProperty(serverInformation.ServiceAccount));
		list.Add(ManagementServerPropertyDefinition.SynchronizationStatusProperty(serverInformation.SynchronizationStatus));
		list.Add(ManagementServerPropertyDefinition.MasterSiteAddressPropery((masterSite != null && masterSite.Addresses != null && masterSite.Addresses.Length != 0) ? masterSite.Addresses[0].URL : ""));
		list.Add(ManagementServerPropertyDefinition.VersionProperty(serverInformation.Version));
		configurationItem.Properties = list.ToArray();
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPathBuilder("System", string.Empty).CreatePath();
		configurationItem.Path = configurationItemPath.Text;
		configurationItem.ParentPath = string.Empty;
		configurationItem.MethodIds = new string[3] { "LoadTasks", "ClientLogOnSupported", "UploadFileChunk" };
		return configurationItem;
	}

	private static ConfigurationItem ConvertToConfigurationItem(ConfigurationItemPath parentPath, ServerAddress addresses, int index)
	{
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPathBuilder("SystemAddress", index.ToString(CultureInfo.InvariantCulture)).CreatePath();
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemCategory = ItemCategories.ChildItem,
			ItemType = "SystemAddress",
			UIHint = "DirectionHorizontal"
		};
		List<Property> list = new List<Property>
		{
			ManagementServerPropertyDefinition.ServerAddressUrlProperty(addresses.URL),
			ManagementServerPropertyDefinition.ServerAddressTypeProperty(addresses.Type)
		};
		obj.DisplayName = addresses.URL;
		obj.Properties = list.ToArray();
		obj.Path = configurationItemPath.Text;
		obj.ParentPath = parentPath.Text;
		return obj;
	}

	public static Property ClientTypeProperty(ClientType clientType)
	{
		return new Property("ClientApplicationType", PropertyTypesHelper.GetValue(clientType))
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyClientApplicationType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyClientApplicationType",
			ToolTipTranslationId = "PropertyClientApplicationTypeToolTip",
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(ClientType))
		};
	}

	public static Property AllowClientLogOnProperty(bool allowClientLogOn)
	{
		return new Property("ClientLogOnAllowed", allowClientLogOn.ToString())
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyFunctionAllowClientLogOn,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyFunctionAllowClientLogOn",
			ToolTipTranslationId = "PropertyFunctionAllowClientLogOnToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}
}
