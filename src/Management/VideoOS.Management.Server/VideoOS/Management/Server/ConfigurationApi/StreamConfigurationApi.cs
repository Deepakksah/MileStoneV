using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIStream")]
[EntityName("ConfigAPIStreamFolder")]
public class StreamConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	private class StreamData
	{
		public StreamInfo Stream { get; set; }

		public DeviceTrackContract Track { get; set; }

		public Guid EdgeTrackId { get; set; }
	}

	private const string LegacyRecordPropertyName = "Record";

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var deviceStreamsContract);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		StreamsConfiguration streamUpdates = StreamsConfiguration.FromXml(deviceStreamsContract.StreamsXml);
		DeviceStreamsContract deviceStreamsContract2 = DeviceStreams.UpdateStreams(requestContext, path.Id, streamUpdates, deviceStreamsContract.DeviceTracks);
		ValueTypeInfo[] streamReferenceIdTypeInfo = GetStreamReferenceIdTypeInfo(requestContext, deviceStreamsContract2);
		StreamsConfiguration streamsConfiguration = StreamsConfiguration.FromXml(deviceStreamsContract2.StreamsXml);
		List<StreamData> list = new List<StreamData>();
		foreach (StreamInfo stream in streamsConfiguration)
		{
			DeviceTrackContract deviceTrackContract = deviceStreamsContract.DeviceTracks.FirstOrDefault(delegate(DeviceTrackContract t)
			{
				Guid trackDefinitionId = t.TrackDefinitionId;
				Guid? trackDefinition = stream.TrackDefinition;
				return trackDefinitionId == trackDefinition;
			});
			DeviceTrackContract deviceTrackContract2 = null;
			if (deviceTrackContract != null)
			{
				deviceTrackContract2 = deviceStreamsContract.DeviceTracks.FirstOrDefault((DeviceTrackContract t) => t.TrackDefinitionId == WellKnownDeviceTrackDefinition.Edge);
			}
			list.Add(new StreamData
			{
				Stream = stream,
				Track = deviceTrackContract,
				EdgeTrackId = (deviceTrackContract2?.TrackId ?? Guid.Empty)
			});
		}
		validateResult = new ValidateResult();
		validateResult.ValidatedOk = true;
		validateResult.ResultItem = ConvertToStreamConfigurationItem(path, list, streamReferenceIdTypeInfo);
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		DeviceStreamsContract deviceStreamsContract;
		return ValidateItemInternal(requestContext, item, path, out deviceStreamsContract);
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out DeviceStreamsContract deviceStreamsContract)
	{
		deviceStreamsContract = null;
		if (!string.Equals(item.ItemCategory, ItemCategories.Item))
		{
			throw new NotSupportedException("Only the stream item with item category set to 'Item' is supported.");
		}
		if (path.IsFolder)
		{
			throw new NotSupportedException("Only the stream item with item category set to 'Item' is supported.");
		}
		string id = path.Id;
		if (item.Children == null)
		{
			throw new ArgumentException("The children property of the item is null.", "item");
		}
		bool flag = item.Children.Any((ConfigurationItem s) => s.Properties.Any((Property p) => StringComparer.Ordinal.Equals(p.Key, "Record")));
		try
		{
			List<StreamInfo> list = new List<StreamInfo>();
			List<DeviceTrackContract> list2 = new List<DeviceTrackContract>();
			ConfigurationItem[] children = item.Children;
			ValidateResult validateResult;
			foreach (ConfigurationItem configurationItem in children)
			{
				string value;
				string value2;
				bool? value3;
				StreamLiveMode? value4;
				string value5;
				bool? value6;
				bool? value7;
				bool? value8;
				try
				{
					((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("Name", out value);
					((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("StreamReferenceId", out value2);
					((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("LiveDefault", out value3);
					((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("LiveMode", out value4);
					((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("RecordTo", out value5);
					((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("UseEdge", out value6);
					((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("DefaultPlayback", out value7);
					((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("UpdateUseMediaAfter", out value8);
					((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("Record", out bool? value9);
					DeviceStreams.ValidateStreamParameters(value, value2, value4, value3, value5, value6, value7, value9, flag);
					if (flag)
					{
						value6 = (value7 = value9.Value);
						value5 = (value9.Value ? WellKnownDeviceTrackDefinition.Primary.ToString() : "");
					}
				}
				catch (ArgumentException exception)
				{
					validateResult = ErrorHelper.CreateValidateResult(exception, configurationItem.Properties);
					if (validateResult != null)
					{
						return validateResult;
					}
					throw;
				}
				ConvertFromLiveMode(value4.Value, out var liveFeed, out var liveRunAlways);
				StreamInfo item2 = new StreamInfo(new Guid(value2), null, value, liveFeed, value3.Value, liveRunAlways, string.IsNullOrEmpty(value5) ? ((Guid?)null) : new Guid?(value5.ToGuid()));
				list.Add(item2);
				DateTime? onlyUseMediaAfter = null;
				if (value8 == true)
				{
					onlyUseMediaAfter = DateTime.UtcNow;
				}
				if (!string.IsNullOrEmpty(value5))
				{
					list2.Add(new DeviceTrackContract(id, Guid.Empty, value5.ToGuid(), value7.Value, value6.Value, inUse: true, onlyUseMediaAfter));
				}
			}
			deviceStreamsContract = DeviceStreams.ValidateStreamsUpdate(requestContext, id, list, list2);
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = item;
			return validateResult;
		}
		catch (DeviceNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (DeviceStreamsNotSupportedException)
		{
			return CreateErrorValidateResult(ConfigurationApiResources.ErrorDeviceStreamsNotSupported, "ErrorDeviceStreamsNotSupported");
		}
		catch (LiveMultistreamingNotAllowedException)
		{
			return CreateErrorValidateResult(ConfigurationApiResources.ErrorLiveMultistreamingNotAllowed, "ErrorLiveMultistreamingNotAllowed");
		}
		catch (DeviceStreamConfigurationException ex4)
		{
			return ex4.ErrorCode switch
			{
				60230 => CreateErrorValidateResult(ConfigurationApiResources.ErrorSameStreamConfiguredTwice, "ErrorSameStreamConfiguredTwice"), 
				60231 => CreateErrorValidateResult(ConfigurationApiResources.ErrorStreamNameNotUnique, "ErrorStreamNameNotUnique"), 
				60232 => CreateErrorValidateResult(ConfigurationApiResources.ErrorStreamNameEmpty, "ErrorStreamNameEmpty"), 
				60233 => CreateErrorValidateResult(ConfigurationApiResources.ErrorEmptyStreamSelection, "ErrorEmptyStreamSelection"), 
				60234 => CreateErrorValidateResult(ConfigurationApiResources.ErrorInvalidStreamSelection, "ErrorInvalidStreamSelection"), 
				60235 => CreateErrorValidateResult(ConfigurationApiResources.ErrorInvalidStreamCombination, "ErrorInvalidStreamCombination"), 
				60236 => CreateErrorValidateResult(ConfigurationApiResources.ErrorOnlyOneStreamAllowed, "ErrorOnlyOneStreamAllowed"), 
				60386 => CreateErrorValidateResult(ConfigurationApiResources.ErrorOnlyOneDefaultLiveStream, "ErrorOnlyOneDefaultLiveStream"), 
				60387 => CreateErrorValidateResult(ConfigurationApiResources.ErrorMustHaveOneDefaultLiveStream, "ErrorMustHaveOneDefaultLiveStream"), 
				60388 => CreateErrorValidateResult(ConfigurationApiResources.ErrorOnlyOneDefaultPlaybackStream, "ErrorOnlyOneDefaultPlaybackStream"), 
				60389 => CreateErrorValidateResult(ConfigurationApiResources.ErrorMustHaveOneDefaultPlaybackStream, "ErrorMustHaveOneDefaultPlaybackStream"), 
				60390 => CreateErrorValidateResult(ConfigurationApiResources.ErrorOnlyOnePrimaryTrack, "ErrorOnlyOnePrimaryTrack"), 
				60391 => CreateErrorValidateResult(ConfigurationApiResources.ErrorMustHaveOnePrimaryTrack, "ErrorMustHaveOnePrimaryTrack"), 
				60392 => CreateErrorValidateResult(ConfigurationApiResources.ErrorTwoOrMoreStreamsWithSameTrack, "ErrorTwoOrMoreStreamsWithSameTrack"), 
				60393 => CreateErrorValidateResult(ConfigurationApiResources.ErrorUnknownTrack, "ErrorUnknownTrack"), 
				60394 => CreateErrorValidateResult(ConfigurationApiResources.ErrorInvalidStreamSelection, "ErrorInvalidStreamSelection"), 
				60240 => CreateErrorValidateResult(ConfigurationApiResources.ErrorStreamTypeNotFound, "ErrorStreamTypeNotFound"), 
				60395 => CreateErrorValidateResult(ConfigurationApiResources.ErrorMultistreamRecordingNotAllowed, "ErrorMultistreamRecordingNotAllowed"), 
				_ => CreateErrorValidateResult(ConfigurationApiResources.ErrorDeviceStreamConfiguration, "ErrorDeviceStreamConfiguration"), 
			};
		}
		catch (UnauthorizedAccessException)
		{
			throw;
		}
		catch (DatabaseConnectionException)
		{
			throw;
		}
		catch (InternalServerErrorException)
		{
			throw;
		}
	}

	private static ValidateResult CreateErrorValidateResult(string resourceText, string resourceTextId)
	{
		ValidateResult validateResult = new ValidateResult();
		validateResult.ErrorResults = new ErrorResult[1]
		{
			new ErrorResult
			{
				ErrorProperty = "",
				ErrorText = resourceText,
				ErrorTextId = resourceTextId
			}
		};
		validateResult.ValidatedOk = false;
		validateResult.ResultItem = null;
		return validateResult;
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			return new ConfigurationItem[1] { GetStreamItem(requestContext, path) };
		}
		return new ConfigurationItem[0];
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id);
			return GroupConfigurationItemFactory.CreateFolderItem("StreamFolder", parentPath);
		}
		return GetStreamItem(requestContext, path);
	}

	private static ConfigurationItem GetStreamItem(IGeminiRequestContext requestContext, ConfigurationItemPath path, DeviceStreamsContract deviceStreamsContract = null)
	{
		try
		{
			if (deviceStreamsContract == null)
			{
				deviceStreamsContract = DeviceStreams.QueryStreams(requestContext, path.Id);
			}
			StreamsConfiguration streamsConfiguration = StreamsConfiguration.FromXml(deviceStreamsContract.StreamsXml);
			List<StreamData> list = new List<StreamData>();
			foreach (StreamInfo stream in streamsConfiguration)
			{
				DeviceTrackContract deviceTrackContract = deviceStreamsContract.DeviceTracks.FirstOrDefault(delegate(DeviceTrackContract t)
				{
					Guid trackDefinitionId = t.TrackDefinitionId;
					Guid? trackDefinition = stream.TrackDefinition;
					return trackDefinitionId == trackDefinition;
				});
				DeviceTrackContract deviceTrackContract2 = null;
				if (deviceTrackContract != null)
				{
					deviceTrackContract2 = deviceStreamsContract.DeviceTracks.FirstOrDefault((DeviceTrackContract t) => t.TrackDefinitionId == WellKnownDeviceTrackDefinition.Edge);
				}
				list.Add(new StreamData
				{
					Stream = stream,
					Track = deviceTrackContract,
					EdgeTrackId = (deviceTrackContract2?.TrackId ?? Guid.Empty)
				});
			}
			ValueTypeInfo[] streamReferenceIdTypeInfo = GetStreamReferenceIdTypeInfo(requestContext, deviceStreamsContract);
			return ConvertToStreamConfigurationItem(path, list, streamReferenceIdTypeInfo);
		}
		catch (DeviceNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (DeviceStreamsNotSupportedException)
		{
			throw new NotSupportedException("Device streams not supported.");
		}
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		if (!item.MethodIds.Contains(method))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", method, item.ItemType), "method");
		}
		ThrowIfPathInvalid(path);
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		if (!(method == "AddStream"))
		{
			if (method == "RemoveStream")
			{
				if (flag)
				{
					return RemoveStream(requestContext, item, path);
				}
				ConfigurationItem streamItem = GetStreamItem(requestContext, parentPath);
				return CreateInvokeInfoRemoveStream(item, streamItem.Children, method);
			}
			throw new NotSupportedException();
		}
		if (!flag)
		{
			return CreateInvokeInfoAddStream(item, method);
		}
		return AddStream(requestContext, item, path);
	}

	private static ConfigurationItem CreateInvokeInfoAddStream(ConfigurationItem item, string method)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddSteam;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { method };
		configurationItem.Properties = new Property[0];
		return configurationItem;
	}

	private static ConfigurationItem AddStream(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		try
		{
			DeviceStreams.AddStream(requestContext, path.Id, out var deviceStreamsContract, out var newStreamReferenceId);
			ConfigurationItem streamItem = GetStreamItem(requestContext, path, deviceStreamsContract);
			ConfigurationItem configurationItem = MethodTypesHelper.CreateInvokeResult(item, streamItem.Path, "Result", "Success", true);
			configurationItem.Children = streamItem.Children;
			configurationItem.ChildrenFilled = true;
			ValueTypeInfo[] streamReferenceIdTypeInfo = GetStreamReferenceIdTypeInfo(requestContext, deviceStreamsContract);
			configurationItem.Properties = new List<Property>(configurationItem.Properties) { StreamPropertyDefinition.StreamReferenceIdProperty(newStreamReferenceId, streamReferenceIdTypeInfo) }.ToArray();
			return configurationItem;
		}
		catch (DeviceStreamMaxCountReachedException)
		{
			ConfigurationItem configurationItem2 = MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
			configurationItem2.Properties.Where((Property p) => p.Key == "State").First().Value = "Error";
			return configurationItem2;
		}
		catch (DeviceNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (DeviceStreamsNotSupportedException ex3)
		{
			throw new NotSupportedException(ex3.Message, ex3);
		}
	}

	private static ConfigurationItem CreateInvokeInfoRemoveStream(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		Func<ConfigurationItem, string> memberValueFunc = (ConfigurationItem ci) => ci.Properties.First((Property p) => string.Equals(p.Key, "StreamReferenceId", StringComparison.Ordinal)).Value;
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveStream, "PropertySelectStreamToRemove", ConfigurationApiResources.PropertySelectStreamToRemove, item, members, memberValueFunc);
	}

	private static ConfigurationItem RemoveStream(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (!string.Equals(path.ItemType, "Stream", StringComparison.Ordinal))
		{
			throw new PathInvalidException(path.Text);
		}
		string itemSelectionPropertyValue = MethodTypesHelper.GetItemSelectionPropertyValue(item);
		DeviceStreamsContract deviceStreamsContract;
		try
		{
			deviceStreamsContract = DeviceStreams.RemoveStream(requestContext, path.Id, itemSelectionPropertyValue);
		}
		catch (DeviceNotFoundException exception)
		{
			throw new PathNotFoundException(path.Text, exception);
		}
		catch (DeviceStreamsNotSupportedException ex)
		{
			throw new NotSupportedException(ex.Message, ex);
		}
		catch (DeviceStreamRemoveLastStreamException ex2)
		{
			throw new PropertyInvalidException(ex2.Message, ex2);
		}
		catch (DeviceStreamRemoveNotFoundException ex3)
		{
			throw new PropertyInvalidException(ex3.Message, ex3);
		}
		ConfigurationItem streamItem = GetStreamItem(requestContext, path, deviceStreamsContract);
		ConfigurationItem configurationItem = MethodTypesHelper.CreateInvokeResult(item, streamItem.Path, "Result", "Success", true);
		configurationItem.Children = streamItem.Children;
		configurationItem.ChildrenFilled = true;
		return configurationItem;
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

	private static ConfigurationItem ConvertToStreamConfigurationItem(ConfigurationItemPath path, List<StreamData> streams, ValueTypeInfo[] referenceStreamIdTypeInfo)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("Stream", path.Id);
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateItemPath("Camera", path.Id), "StreamFolder");
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.ItemType = "Stream";
		configurationItem.Path = configurationItemPath.Text;
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypeStreamFolder;
		configurationItem.ParentPath = configurationItemPath2.Text;
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.MethodIds = new string[2] { "AddStream", "RemoveStream" };
		configurationItem.ChildrenFilled = true;
		ConfigurationItem configurationItem2 = configurationItem;
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		foreach (StreamData stream in streams)
		{
			list.Add(ConvertToStreamChildConfigurationItem(configurationItemPath2, configurationItemPath, stream, referenceStreamIdTypeInfo));
		}
		configurationItem2.Children = list.ToArray();
		return configurationItem2;
	}

	private static ConfigurationItem ConvertToStreamChildConfigurationItem(ConfigurationItemPath parentPath, ConfigurationItemPath path, StreamData stream, ValueTypeInfo[] referenceStreamIdTypeInfo)
	{
		bool flag = stream.Track?.MergeEdge ?? false;
		Guid edgeTrackId = (flag ? stream.EdgeTrackId : Guid.Empty);
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemCategory = ItemCategories.ChildItem;
		configurationItem.ItemType = "Stream";
		configurationItem.Path = path.Text;
		configurationItem.DisplayName = stream.Stream.Name;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.Properties = new Property[8]
		{
			GeneralPropertyDefinitions.GetNameProperty(stream.Stream.Name),
			StreamPropertyDefinition.StreamReferenceIdProperty(stream.Stream.StreamReferenceId, referenceStreamIdTypeInfo),
			StreamPropertyDefinition.LiveDefaultProperty(stream.Stream.LiveDefault),
			StreamPropertyDefinition.LiveModeProperty(ConvertToLiveMode(stream.Stream.LiveFeed, stream.Stream.LiveRunAlways)),
			StreamPropertyDefinition.RecordToProperty(stream.Stream.TrackDefinition, GetTrackIdTypeInfo()),
			StreamPropertyDefinition.DefaultPlaybackProperty(stream.Track?.DefaultPlayback ?? false),
			StreamPropertyDefinition.UseEdgeProperty(flag),
			StreamPropertyDefinition.EdgeTrackIdProperty(edgeTrackId)
		};
		return configurationItem;
	}

	private static ValueTypeInfo[] GetStreamReferenceIdTypeInfo(IGeminiRequestContext requestContext, DeviceStreamsContract deviceStreamsContract)
	{
		StreamDefinitionCollection streamDefinitionCollection = QueryStreamDefinitions(requestContext, deviceStreamsContract);
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		foreach (StreamDefinition item in streamDefinitionCollection)
		{
			list.Add(new ValueTypeInfo
			{
				Name = item.DisplayNameResolved,
				Value = PropertyTypesHelper.GetValue(item.ReferenceId)
			});
		}
		return list.ToArray();
	}

	private static ValueTypeInfo[] GetTrackIdTypeInfo()
	{
		return new List<ValueTypeInfo>
		{
			new ValueTypeInfo
			{
				Name = "Primary recording",
				Value = WellKnownDeviceTrackDefinition.Primary.ToString()
			},
			new ValueTypeInfo
			{
				Name = "Secondary recording",
				Value = WellKnownDeviceTrackDefinition.Secondary.ToString()
			},
			new ValueTypeInfo
			{
				Name = "No recording",
				Value = string.Empty
			}
		}.ToArray();
	}

	private static StreamDefinitionCollection QueryStreamDefinitions(IGeminiRequestContext requestContext, DeviceStreamsContract deviceStreamsContract)
	{
		using IHardwareDefinitionComponent hardwareDefinitionComponent = requestContext.CreateHardwareDefinitionComponent();
		MainDefinition mainDefinition = MainDefinition.LoadXml(hardwareDefinitionComponent.QueryDefinition(deviceStreamsContract.HardwareDefinitionDigest));
		DeviceHwid key = new DeviceHwid(deviceStreamsContract.DeviceType.ToDeviceTypeEnum(), deviceStreamsContract.DeviceIndex);
		DeviceDefinition deviceDefinition = mainDefinition.HardwareDefinition.DeviceDefinitions[key];
		if (!deviceDefinition.StreamCombinations.Supported)
		{
			throw new DeviceStreamsNotSupportedException();
		}
		return deviceDefinition.Streams;
	}

	private static StreamLiveMode ConvertToLiveMode(bool liveFeed, bool liveRunAlways)
	{
		if (liveFeed && liveRunAlways)
		{
			return StreamLiveMode.Always;
		}
		if (!liveFeed && !liveRunAlways)
		{
			return StreamLiveMode.Never;
		}
		return StreamLiveMode.WhenNeeded;
	}

	private static void ConvertFromLiveMode(StreamLiveMode liveMode, out bool liveFeed, out bool liveRunAlways)
	{
		switch (liveMode)
		{
		case StreamLiveMode.Always:
			liveFeed = true;
			liveRunAlways = true;
			break;
		case StreamLiveMode.Never:
			liveFeed = false;
			liveRunAlways = false;
			break;
		case StreamLiveMode.WhenNeeded:
			liveFeed = true;
			liveRunAlways = false;
			break;
		default:
			throw new NotSupportedException();
		}
	}
}
