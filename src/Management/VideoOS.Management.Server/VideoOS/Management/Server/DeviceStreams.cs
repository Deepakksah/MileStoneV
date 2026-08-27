using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public class DeviceStreams
{
	private class UpdateStreamsXmlResult
	{
		public DeviceStreamsContract DeviceStreamsContract { get; set; }

		public Guid NewStreamReferenceId { get; set; }

		public UpdateStreamsXmlResult(DeviceStreamsContract deviceStreamsContract)
		{
			DeviceStreamsContract = deviceStreamsContract;
		}
	}

	public static DeviceStreamsContract QueryStreams(IGeminiRequestContext requestContext, string deviceId)
	{
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		Guid namespaceId;
		try
		{
			namespaceId = requestContext.DeviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, deviceId);
		}
		catch (UnknownDeviceException)
		{
			throw new DeviceNotFoundException();
		}
		catch (DatabaseConnectionException)
		{
			throw;
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, namespaceId, "GENERIC_READ", deviceId);
		using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
		using IHardwareDefinitionComponent hardwareDefinitionComponent = requestContext.CreateHardwareDefinitionComponent();
		DeviceStreamsContract deviceStreamsContract = deviceComponent.QueryDeviceStreams(deviceId);
		if (deviceStreamsContract == null)
		{
			throw new DeviceNotFoundException();
		}
		MainDefinition mainDefinition = MainDefinition.LoadXml(hardwareDefinitionComponent.QueryDefinition(deviceStreamsContract.HardwareDefinitionDigest));
		DeviceHwid key = new DeviceHwid(deviceStreamsContract.DeviceType.ToDeviceTypeEnum(), deviceStreamsContract.DeviceIndex);
		if (!mainDefinition.HardwareDefinition.DeviceDefinitions[key].StreamCombinations.Supported)
		{
			throw new DeviceStreamsNotSupportedException();
		}
		return deviceStreamsContract;
	}

	public static DeviceStreamsContract ValidateStreamsUpdate(IGeminiRequestContext requestContext, string deviceId, IEnumerable<StreamInfo> streamUpdates, IEnumerable<DeviceTrackContract> trackUpdates)
	{
		return ValidateStreamsAndUpdate(requestContext, deviceId, streamUpdates, trackUpdates);
	}

	public static void AddStream(IGeminiRequestContext requestContext, string deviceId, out DeviceStreamsContract deviceStreamsContract, out Guid newStreamReferenceId)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		UpdateStreamsXmlResult updateStreamsXmlResult = UpdateStreamsXmlWithRetry(updateStreamsFunc: delegate
		{
			ValidateStreamAndAdd(requestContext, deviceId, out var deviceStreamsContract2, out var newStreamReferenceId2);
			return new UpdateStreamsXmlResult(deviceStreamsContract2)
			{
				NewStreamReferenceId = newStreamReferenceId2
			};
		}, requestContext: requestContext);
		deviceStreamsContract = updateStreamsXmlResult.DeviceStreamsContract;
		newStreamReferenceId = updateStreamsXmlResult.NewStreamReferenceId;
	}

	private static void ValidateStreamAndAdd(IGeminiRequestContext requestContext, string deviceId, out DeviceStreamsContract deviceStreamsContract, out Guid newStreamReferenceId)
	{
		Validation.CheckRequestContext(requestContext);
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		bool flag = requestContext.FeatureCacheManager.IsFeatureLicensed(requestContext, "LiveMultiStreaming");
		deviceStreamsContract = ValidateStreamCore(requestContext, deviceId, out var deviceDefinition);
		StreamsConfiguration streamsConfiguration = StreamsConfiguration.FromXml(deviceStreamsContract.StreamsXml);
		if (!flag && streamsConfiguration.Count > 1)
		{
			throw new DeviceStreamMaxCountReachedException();
		}
		StreamDefinition streamDefinition = deviceDefinition.Streams.FirstOrDefault((StreamDefinition sd) => !streamsConfiguration.Any((StreamInfo s) => s.StreamReferenceId == sd.ReferenceId));
		if (streamDefinition == null)
		{
			throw new DeviceStreamMaxCountReachedException();
		}
		string name = streamDefinition.DisplayNameResolved;
		int num = 2;
		while (streamsConfiguration.Any((StreamInfo s) => string.Equals(s.Name, name, StringComparison.Ordinal)))
		{
			name = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", streamDefinition.DisplayNameResolved, num++);
		}
		if (flag)
		{
			StreamInfo stream = new StreamInfo(streamDefinition.ReferenceId, streamDefinition.Hwid, name, liveFeed: true, liveDefault: false, liveRunAlways: false, null);
			streamsConfiguration.Add(stream);
		}
		else
		{
			StreamInfo streamInfo = streamsConfiguration.Single();
			streamInfo.LiveFeed = false;
			streamInfo.LiveDefault = false;
			streamInfo.LiveRunAlways = false;
			streamInfo.TrackDefinition = WellKnownDeviceTrackDefinition.Primary;
			StreamInfo stream2 = new StreamInfo(streamDefinition.ReferenceId, streamDefinition.Hwid, name, liveFeed: true, liveDefault: true, liveRunAlways: false, null);
			streamsConfiguration.Add(stream2);
		}
		IEnumerable<TrackConfiguration> tracks = deviceStreamsContract.DeviceTracks.Select((DeviceTrackContract t) => new TrackConfiguration(t.TrackDefinitionId, t.DefaultPlayback, t.MergeEdge));
		if (streamsConfiguration.ValidateConfiguration(deviceDefinition, tracks).HasError)
		{
			throw new InternalServerErrorException(VmoResources.VMO65002 + "\nThe streams configuration for the device is not valid after adding a new stream.");
		}
		deviceStreamsContract.StreamsXml = streamsConfiguration.SaveXml();
		newStreamReferenceId = streamDefinition.ReferenceId;
	}

	public static DeviceStreamsContract UpdateStreams(IGeminiRequestContext requestContext, string deviceId, IEnumerable<StreamInfo> streamUpdates, IEnumerable<DeviceTrackContract> trackUpdates)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		return UpdateStreamsXmlWithRetry(updateStreamsFunc: delegate
		{
			DeviceStreamsContract deviceStreamsContract = ValidateStreamsAndUpdate(requestContext, deviceId, streamUpdates, trackUpdates);
			Integration.FireConfigurationChanged(requestContext, "Camera", deviceId, EventAction.Updated, string.Empty);
			DeviceTrackContract[] array = deviceStreamsContract.DeviceTracks.Where((DeviceTrackContract t) => t.TrackId == Guid.Empty).ToArray();
			if (array.Any())
			{
				using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
				DeviceTrackContract[] array2 = array;
				foreach (DeviceTrackContract deviceTrackContract in array2)
				{
					deviceTrackContract.TrackId = Guid.NewGuid();
					deviceComponent.CreateDeviceTrack(deviceTrackContract);
					deviceStreamsContract.DeviceTracks.Add(deviceTrackContract);
				}
			}
			return new UpdateStreamsXmlResult(deviceStreamsContract);
		}, requestContext: requestContext).DeviceStreamsContract;
	}

	private static DeviceStreamsContract ValidateStreamsAndUpdate(IGeminiRequestContext requestContext, string deviceId, IEnumerable<StreamInfo> streamUpdates, IEnumerable<DeviceTrackContract> trackUpdates)
	{
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		DeviceDefinition deviceDefinition;
		DeviceStreamsContract deviceStreamsContract = ValidateStreamCore(requestContext, deviceId, out deviceDefinition);
		StreamsConfiguration streamsConfiguration = StreamsConfiguration.FromXml(deviceStreamsContract.StreamsXml);
		if (streamsConfiguration.Count != streamUpdates.Count())
		{
			throw new DeviceStreamConfigurationException(VmoResources.VMO60241);
		}
		streamsConfiguration = new StreamsConfiguration();
		foreach (StreamInfo streamUpdate in streamUpdates)
		{
			streamsConfiguration.Add(streamUpdate);
		}
		IEnumerable<TrackConfiguration> tracks = trackUpdates.Select((DeviceTrackContract t) => new TrackConfiguration(t.TrackDefinitionId, t.DefaultPlayback, t.MergeEdge));
		ThrowIfValidationFailed(streamsConfiguration.ValidateConfiguration(deviceDefinition, tracks));
		ValidateStreamsAllowedByFeatureFlags();
		SetCorrectHwidforAllNewStreams();
		UpdateDeviceStreamsContract();
		return deviceStreamsContract;
		void SetCorrectHwidforAllNewStreams()
		{
			foreach (StreamInfo stream in streamsConfiguration)
			{
				StreamDefinition streamDefinition = deviceDefinition.Streams.FirstOrDefault((StreamDefinition s) => s.ReferenceId == stream.StreamReferenceId);
				if (streamDefinition == null)
				{
					throw VmoExceptionFactory.VMO60240_DeviceStreamConfigurationException();
				}
				stream.Hwid = streamDefinition.Hwid;
			}
		}
		void UpdateDeviceStreamsContract()
		{
			deviceStreamsContract.StreamsXml = streamsConfiguration.SaveXml();
			foreach (DeviceTrackContract item in trackUpdates.Where((DeviceTrackContract t1) => !deviceStreamsContract.DeviceTracks.Any((DeviceTrackContract deviceTrackContract2) => t1.TrackDefinitionId == deviceTrackContract2.TrackDefinitionId)))
			{
				deviceStreamsContract.DeviceTracks.Add(item);
			}
			DeviceTrackContract[] array = deviceStreamsContract.DeviceTracks.ToArray();
			foreach (DeviceTrackContract track in array)
			{
				if (track.TrackDefinitionId == WellKnownDeviceTrackDefinition.Edge)
				{
					track.OnlyUseMediaAfter = null;
				}
				else
				{
					DeviceTrackContract deviceTrackContract = trackUpdates.FirstOrDefault((DeviceTrackContract t) => t.TrackDefinitionId == track.TrackDefinitionId);
					if (deviceTrackContract == null)
					{
						track.DefaultPlayback = false;
						track.MergeEdge = false;
						track.InUse = false;
						track.OnlyUseMediaAfter = null;
					}
					else
					{
						track.DefaultPlayback = deviceTrackContract.DefaultPlayback;
						track.MergeEdge = deviceTrackContract.MergeEdge;
						if (track.TrackDefinitionId == WellKnownDeviceTrackDefinition.Primary || track.InUse)
						{
							track.OnlyUseMediaAfter = null;
						}
						track.InUse = true;
					}
				}
			}
		}
		void ValidateStreamsAllowedByFeatureFlags()
		{
			if (!requestContext.FeatureCacheManager.IsFeatureLicensed(requestContext, "LiveMultiStreaming") && streamsConfiguration.Count((StreamInfo s) => s.LiveFeed) > 1)
			{
				throw new LiveMultistreamingNotAllowedException();
			}
			if (!requestContext.FeatureCacheManager.IsFeatureLicensed(requestContext, "MultistreamRecording") && streamsConfiguration.Count((StreamInfo s) => s.TrackDefinition.HasValue) > 1)
			{
				throw VmoExceptionFactory.VMO60395_DeviceStreamConfigurationException();
			}
		}
	}

	public static void ValidateStreamParameters(string name, string streamReferenceId, StreamLiveMode? liveMode, bool? liveDefault, string recordTo, bool? useEdge, bool? defaultPlayback, bool? legacyRecord, bool useLegacyStreamProperties)
	{
		Validation.CheckObjectId(ref streamReferenceId, "StreamReferenceId", allowNullOrEmpty: false);
		Validation.CheckName(name, "Name", allowNull: true);
		if (!liveMode.HasValue)
		{
			throw new ArgumentNullException("LiveMode");
		}
		if (!liveDefault.HasValue)
		{
			throw new ArgumentNullException("LiveDefault");
		}
		if (useLegacyStreamProperties)
		{
			if (!legacyRecord.HasValue)
			{
				throw new ArgumentException(VmoResources.VMO62001, "Record");
			}
			return;
		}
		Validation.CheckStringNotNull(recordTo, "RecordTo");
		Validation.CheckObjectId(recordTo, "RecordTo", allowNullOrEmpty: true);
		if (!useEdge.HasValue)
		{
			throw new ArgumentNullException("UseEdge");
		}
		if (!defaultPlayback.HasValue)
		{
			throw new ArgumentNullException("DefaultPlayback");
		}
	}

	private static void ThrowIfValidationFailed(StreamValidationResult validationResult)
	{
		if (validationResult.HasError)
		{
			if (validationResult.SameStreamsError)
			{
				throw VmoExceptionFactory.VMO60230_DeviceStreamConfigurationException();
			}
			if (validationResult.SameStreamNamesError)
			{
				throw VmoExceptionFactory.VMO60231_DeviceStreamConfigurationException();
			}
			if (validationResult.EmptyStreamNameError)
			{
				throw VmoExceptionFactory.VMO60232_DeviceStreamConfigurationException();
			}
			if (validationResult.EmptySelectionError)
			{
				throw VmoExceptionFactory.VMO60233_DeviceStreamConfigurationException();
			}
			if (validationResult.InvalidSelectionError)
			{
				throw VmoExceptionFactory.VMO60394_DeviceStreamConfigurationException();
			}
			if (validationResult.StreamCombinationError)
			{
				throw VmoExceptionFactory.VMO60235_DeviceStreamConfigurationException();
			}
			if (validationResult.OnlyOneStreamAllowedError)
			{
				throw VmoExceptionFactory.VMO60236_DeviceStreamConfigurationException();
			}
			if (validationResult.OnlyOneDefaultLiveStreamError)
			{
				throw VmoExceptionFactory.VMO60386_DeviceStreamConfigurationException();
			}
			if (validationResult.MustHaveOneDefaultLiveStreamError)
			{
				throw VmoExceptionFactory.VMO60387_DeviceStreamConfigurationException();
			}
			if (validationResult.OnlyOnePrimaryTrackStreamError)
			{
				throw VmoExceptionFactory.VMO60390_DeviceStreamConfigurationException();
			}
			if (validationResult.MustHaveOnePrimaryTrackStreamError)
			{
				throw VmoExceptionFactory.VMO60391_DeviceStreamConfigurationException();
			}
			if (validationResult.TwoOrMoreStreamsWithSameTrackStreamError)
			{
				throw VmoExceptionFactory.VMO60392_DeviceStreamConfigurationException();
			}
			if (validationResult.UnknownTrackStreamError)
			{
				throw VmoExceptionFactory.VMO60393_DeviceStreamConfigurationException();
			}
			if (validationResult.UnknownStreamError)
			{
				throw VmoExceptionFactory.VMO60240_DeviceStreamConfigurationException();
			}
			if (validationResult.OnlyOneDefaultPlaybackStreamError)
			{
				throw VmoExceptionFactory.VMO60388_DeviceStreamConfigurationException();
			}
			if (validationResult.MustHaveOneDefaultPlaybackStreamError)
			{
				throw VmoExceptionFactory.VMO60389_DeviceStreamConfigurationException();
			}
			throw new DeviceStreamConfigurationException(VmoResources.VMO60229);
		}
	}

	public static DeviceStreamsContract RemoveStream(IGeminiRequestContext requestContext, string deviceId, string streamReferenceId)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		return UpdateStreamsXmlWithRetry(updateStreamsFunc: () => new UpdateStreamsXmlResult(ValidateStreamAndRemove(requestContext, deviceId, streamReferenceId)), requestContext: requestContext).DeviceStreamsContract;
	}

	private static void HandlePrimaryRecordingStreamRemoval(StreamsConfiguration streamsConfiguration, DeviceStreamsContract deviceStreamsContract)
	{
		bool flag = !streamsConfiguration.Any((StreamInfo s) => s.TrackDefinition == WellKnownDeviceTrackDefinition.Primary);
		streamsConfiguration.EnsureLiveAndRecordSelections();
		foreach (DeviceTrackContract item in deviceStreamsContract.DeviceTracks.Where((DeviceTrackContract t) => t.TrackDefinitionId != WellKnownDeviceTrackDefinition.Edge && !streamsConfiguration.Any((StreamInfo s) => s.TrackDefinition == t.TrackDefinitionId)))
		{
			item.DefaultPlayback = false;
			item.MergeEdge = false;
			item.InUse = false;
		}
		bool flag2 = deviceStreamsContract.DeviceTracks.Any((DeviceTrackContract t) => t.DefaultPlayback);
		if (flag || !flag2)
		{
			DeviceTrackContract deviceTrackContract = deviceStreamsContract.DeviceTracks.First((DeviceTrackContract t) => t.TrackDefinitionId == WellKnownDeviceTrackDefinition.Primary);
			if (!flag2)
			{
				deviceTrackContract.DefaultPlayback = true;
			}
			if (flag)
			{
				deviceTrackContract.MergeEdge = true;
			}
		}
	}

	private static DeviceStreamsContract ValidateStreamAndRemove(IGeminiRequestContext requestContext, string deviceId, string streamReferenceId)
	{
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref streamReferenceId, "streamReferenceId", allowNullOrEmpty: false);
		DeviceDefinition deviceDefinition;
		DeviceStreamsContract deviceStreamsContract = ValidateStreamCore(requestContext, deviceId, out deviceDefinition);
		StreamsConfiguration streamsConfiguration = StreamsConfiguration.FromXml(deviceStreamsContract.StreamsXml);
		StreamInfo streamInfo = streamsConfiguration.FirstOrDefault((StreamInfo s) => s.StreamReferenceId == new Guid(streamReferenceId));
		if (streamInfo == null)
		{
			throw new DeviceStreamRemoveNotFoundException();
		}
		streamsConfiguration.Remove(streamInfo);
		if (streamsConfiguration.Count == 0)
		{
			throw new DeviceStreamRemoveLastStreamException();
		}
		HandlePrimaryRecordingStreamRemoval(streamsConfiguration, deviceStreamsContract);
		IEnumerable<TrackConfiguration> tracks = deviceStreamsContract.DeviceTracks.Select((DeviceTrackContract t) => new TrackConfiguration(t.TrackDefinitionId, t.DefaultPlayback, t.MergeEdge));
		if (streamsConfiguration.ValidateConfiguration(deviceDefinition, tracks).HasError)
		{
			throw new InternalServerErrorException(VmoResources.VMO65002 + "\nThe streams configuration for the device is not valid after removing a stream.");
		}
		deviceStreamsContract.StreamsXml = streamsConfiguration.SaveXml();
		return deviceStreamsContract;
	}

	private static DeviceStreamsContract ValidateStreamCore(IGeminiRequestContext requestContext, string deviceId, out DeviceDefinition deviceDefinition)
	{
		Guid namespaceId;
		try
		{
			namespaceId = requestContext.DeviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, deviceId);
		}
		catch (UnknownDeviceException)
		{
			throw new DeviceNotFoundException();
		}
		catch (DatabaseConnectionException)
		{
			throw;
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", deviceId);
		using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
		using IHardwareDefinitionComponent hardwareDefinitionComponent = requestContext.CreateHardwareDefinitionComponent();
		DeviceStreamsContract deviceStreamsContract = deviceComponent.QueryDeviceStreams(deviceId);
		if (deviceStreamsContract == null)
		{
			throw new DeviceNotFoundException();
		}
		MainDefinition mainDefinition = MainDefinition.LoadXml(hardwareDefinitionComponent.QueryDefinition(deviceStreamsContract.HardwareDefinitionDigest));
		DeviceHwid key = new DeviceHwid(deviceStreamsContract.DeviceType.ToDeviceTypeEnum(), deviceStreamsContract.DeviceIndex);
		deviceDefinition = mainDefinition.HardwareDefinition.DeviceDefinitions[key];
		if (!deviceDefinition.StreamCombinations.Supported)
		{
			throw new DeviceStreamsNotSupportedException();
		}
		return deviceStreamsContract;
	}

	private static UpdateStreamsXmlResult UpdateStreamsXmlWithRetry(IGeminiRequestContext requestContext, Func<UpdateStreamsXmlResult> updateStreamsFunc)
	{
		Guid sessionId = Guid.NewGuid();
		int num = 5;
		bool flag;
		UpdateStreamsXmlResult updateResult;
		do
		{
			flag = false;
			using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
			updateResult = updateStreamsFunc();
			try
			{
				deviceComponent.UpdateStreams(updateResult.DeviceStreamsContract, sessionId);
			}
			catch (DeviceNotRecentVersionException)
			{
				flag = true;
				num--;
				if (num == 0)
				{
					throw;
				}
				Thread.Sleep(50);
			}
		}
		while (flag);
		requestContext.GetIntegrationService().TryNotifyServerAsync();
		using (ISqlLogProcessor sqlLogProcessor = requestContext.CreateLogManager())
		{
			sqlLogProcessor.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => r.DeviceType == updateResult.DeviceStreamsContract.DeviceType);
		Integration.FireConfigurationChanged(requestContext, deviceTypeRelation.ItemType, updateResult.DeviceStreamsContract.Id, EventAction.Updated, string.Empty);
		return updateResult;
	}
}
