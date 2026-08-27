namespace VideoOS.Mobile.Portable.MetaChannel;

public class CommunicationCommands
{
	public const string PublicKey = "PublicKey";

	public const string UserName = "Username";

	public const string Password = "Password";

	public const string LoginType = "LoginType";

	public const string VideoID = "VideoId";

	public const string StreamId = "StreamId";

	public const string CameraId = "CameraId";

	public const string DestWidth = "DestWidth";

	public const string DestHeight = "DestHeight";

	public const string FPS = "Fps";

	public const string CompressionLvl = "ComprLevel";

	public const string DestLeft = "DestLeft";

	public const string DestTop = "DestTop";

	public const string DestRight = "DestRight";

	public const string DestBottom = "DestBottom";

	public const string SrcLeft = "SrcLeft";

	public const string SrcTop = "SrcTop";

	public const string SrcRight = "SrcRight";

	public const string SrcBottom = "SrcBottom";

	public const string SrcWidth = "SrcWidth";

	public const string SrcHeight = "SrcHeight";

	public const string SignalType = "SignalType";

	public const string MethodType = "MethodType";

	public const string StreamDataType = "StreamDataType";

	public const string AudioEncoding = "AudioEncoding";

	public const string AudioSamplingRate = "AudioSamplingRate";

	public const string AudioChannelsNumber = "AudioChannelsNumber";

	public const string AudioBitsPerSample = "AudioBitsPerSample";

	public const string CloseConnectionOnError = "CloseConnectionOnError";

	public const string StreamHeaders = "StreamHeaders";

	public const string KeyFramesOnly = "KeyFramesOnly";

	public const string ViewID = "ViewId";

	public const string ConfigurationId = "ConfigurationId";

	public const string Timeout = "Timeout";

	public const string TimeSpan = "TimeSpan";

	public const string Speed = "Speed";

	public const string SeekType = "SeekType";

	public const string Time = "Time";

	public const string TimeRangeBegin = "TimeRangeBegin";

	public const string TimeRangeEnd = "TimeRangeEnd";

	public const string AutoLoop = "AutoLoop";

	public const string Blocking = "Blocking";

	public const string BeforeTime = "BeforeTime";

	public const string BeforeCount = "BeforeCount";

	public const string AfterTime = "AfterTime";

	public const string AfterCount = "AfterCount";

	public const string SequenceType = "SeqType";

	public const string MinTimeBetweenSequences = "MinTimeBetweenSequences";

	public const string PtzMove = "PtzMove";

	public const string GestureXPercent = "GestureXPercent";

	public const string GestureYPercent = "GestureYPercent";

	public const string GestureDuration = "GestureDuration";

	public const string GestureTimeout = "GestureTimeout";

	public const string PtzPreset = "PtzPreset";

	public const string PtzCamera = "PtzCamera";

	public const string ProtocolVersionName = "ProtocolVersion";

	public const string ProtocolVersionVersion = "3.1.0.0";

	public const string ServerVersionName = "ServerVersion";

	public const string ServerProductCode = "ServerProductCode";

	public const string ServerProductCodeValue = "1";

	public const string AuthorizationPtz = "PTZ";

	public const string AuthorizationPreset = "Preset";

	public const string AuthorizationLive = "Live";

	public const string AuthorizationPlayback = "Playback";

	public const string AuthorizationSpeak = "Speak";

	public const string AuthorizationSequences = "Sequences";

	public const string AuthorizationStartRecording = "StartRecording";

	public const string AuthorizationUpstream = "VideoPush";

	public const string AuthorizationUpstreamLocation = "VideoPushLocation";

	public const string AuthorizationUpstreamAudio = "VideoPushAudio";

	public const string AuthorizationBookmarkAdd = "BookmarkAdd";

	public const string AuthorizationBookmarkDelete = "BookmarkDelete";

	public const string AuthorizationBookmarkEdit = "BookmarkEdit";

	public const string AuthorizationBookmarkView = "BookmarkView";

	public const string AuthorizationEvent = "Event";

	public const string AuthorizationOutput = "Output";

	public const string AuthorizationExportAvi = "ExportAvi";

	public const string AuthorizationExportDatabase = "ExportDatabase";

	public const string AuthorizationExportJpeg = "ExportJpeg";

	public const string AuthorizationGetSequences = "GetSequences";

	public const string AuthorizationYes = "Yes";

	public const string AuthorizationNo = "No";

	public const string PositioningLatitude = "Latitude";

	public const string PositioningLongitude = "Longitude";

	public const string PositioningCoverageDirection = "CoverageDirection";

	public const string PositioningCoverageDepth = "CoverageDepth";

	public const string PositioningCoverageFieldOfView = "CoverageFieldOfView";

	public const string SearchCameraIds = "SearchCameraIds";

	public const string Keyword = "Keyword";

	public const string MyBookmarks = "MyBookmarks";

	public const string Bookmark = "Bookmark";

	public const string StartTime = "StartTime";

	public const string EndTime = "EndTime";

	public const string ExportId = "ExportId";

	public const string ExportLink = "ExportLink";

	public const string CodecName = "CodecName";

	public const string ExportName = "Name";

	public const string FileName = "FileName";

	public const string FileSize = "FileSize";

	public const string KeepOriginals = "KeepOriginals";

	public const string ExportType = "Type";

	public const string ExportTypeName = "ExportType";

	public const string State = "State";

	public const string StateName = "StateName";

	public const string States = "States";

	public const string QueuedTime = "QueuedTime";

	public const string CompletedTime = "CompletedTime";

	public const string Size = "Size";

	public const string IncludeAudio = "IncludeAudio";

	public const string AllCamerasView = "AllCamerasView";

	public const string HasOutputsAndEvents = "OutputsAndEvents";

	public const string HasOutputsAndEventsYes = "Yes";

	public const string HasOutputsAndEventsNo = "No";

	public const string TriggerType = "TriggerType";

	public const string ObjectId = "ObjectId";

	public const string Description = "Description";

	public const string Header = "Header";

	public const string Reference = "Reference";

	public const string NativeStreamingAvailable = "NativeStreamingAvailable";

	public const string NativeStreamingRequired = "NativeStreamingRequired";

	public const string TranscodedStreamingAvailable = "TranscodedStreamingAvailable";

	public const string NativeStreamingSuggested = "NativeStreamingSuggested";

	public const string SegmentedStreamingAvailable = "SegmentedStreamingAvailable";

	public const string FakeSegmentedStreamingNotSupported = "FakeSegmentedStreamingNotSupported";

	public const string ID = "Id";

	public const string Name = "Name";

	public const string Level = "Level";

	public const string RequestPrevNextSyncTime = "RequestPrevNextSyncTime";

	public const string RequestMotionAndSequenceId = "RequestMotionAndSequenceId";

	public const string RequsetSignatures = "RequestSignatures";

	public const string RequestSize = "RequestSize";

	public const string EnableOemedClients = "EnableOemedClients";

	public const string ChapSupported = "CHAPSupported";

	public const string JsonSupported = "JsonSupported";

	public const string SecurityEnabled = "SecurityEnabled";

	public const string NumChallenges = "NumChallenges";

	public const string Challenge = "Challenge";

	public const string ChalAnswer = "ChalAnswer";

	public const string PlaybackControllerId = "PlaybackControllerId";

	public const string CloseOldControllers = "CloseOldControllers";

	public const string FragmentDurationMs = "FragmentDurationMs";

	public const string ControllerStreamsCount = "ControllerStreamsCount";

	public const string Priority = "Priority";

	public const string PriorityName = "PriorityName";

	public const string Priorities = "Priorities";

	public const string AssignedTo = "AssignedTo";

	public const string Text = "Text";

	public const string Categories = "Categories";

	public const string Category = "Category";

	public const string CategoryName = "CategoryName";

	public const string MultiCameraPlayback = "MultiCameraPlayback";

	public const string Count = "Count";

	public const string Operator = "Operator";

	public const string Timestamp = "Timestamp";

	public const string Cloud = "Cloud";

	public const string DeviceToken = "X";

	public const string OptionalParamX = "PX";

	public const string OptionalParamY = "PY";

	public const string OptionalParamZ = "PZ";

	public const string Settings = "Settings";

	public const string NotificationMessage = "loc-key";

	public const string NotificationMessageArgument = "loc-args";

	public const string NotificationSourceType = "NotificationSourceType";

	public const string AlarmId = "AlarmId";

	public const string EventId = "EventId";

	public const string MyAlarms = "MyAlarms";

	public const string ValidationSLC = "S";

	public const string NotificationsEnabled = "NotificationsEnabled";

	public const string SyncState = "SyncState";

	public const string DeviceId = "DeviceId";

	public const string DeviceStates = "DeviceStates";

	public const string LocalIp = "LocalIp";

	public const string DeviceName = "DeviceName";

	public const string DeviceEnabled = "Enabled";

	public const string MarkedForDeletion = "MarkedForDeletion";

	public const string DeviceState = "DeviceState";

	public const string NotificationResult = "Result";

	public const string Notification = "Notification";

	public const string Investigation = "Investigation";

	public const string Investigations = "Investigations";

	public const string InvestigationId = "InvestigationId";

	public const string ViewOthersInvestigations = "ViewOthersInvestigations";

	public const string Item = "Item";

	public const string Items = "Items";

	public const string ItemId = "ItemId";

	public const string ItemIds = "ItemIds";

	public const string ItemKind = "ItemKind";

	public const string Hierarchy = "Hierarchy";

	public const string Type = "Type";

	public const string Properties = "Properties";

	public const string Folder = "Folder";

	public const string Carousel = "Carousel";

	public const string Server = "Server";

	public const string Camera = "Camera";

	public const string RelatedCameras = "RelatedCameras";

	public const string IncludeRelatedDevices = "IncludeRelatedDevices";

	public const string View = "View";

	public const string Event = "Event";

	public const string Output = "Output";

	public const string Metadata = "Metadata";

	public const string Input = "Input";

	public const string Alarm = "Alarm";

	public const string FirstEventId = "FirstEventId";

	public const string AlarmDataSetting = "AlarmDataSetting";

	public const string Export = "Export";

	public const string Endpoint = "Endpoint";

	public const string Source = "Source";

	public const string SourceId = "SourceId";

	public const string Message = "Message";

	public const string MessageId = "MessageId";

	public const string Status = "Status";

	public const string StatusProgress = "StatusProgress";

	public const string OriginDbExportId = "OriginDbExportId";

	public const string DbExportId = "DbExportId";

	public const string AviExportId = "AviExportId";

	public const string MkvExportId = "MkvExportId";

	public const string BinaryExportId = "BinaryExportId";

	public const string ReExportFromId = "ReExportFromId";

	public const string CreatedAt = "CreatedAt";

	public const string ModifiedAt = "ModifiedAt";

	public const string ExpirationTime = "ExpirationTime";

	public const string SubExportId = "SubExportId";

	public const string SubExportIds = "SubExportIds";

	public const string CreatedBy = "CreatedBy";

	public const string ModifiedBy = "ModifiedBy";

	public const string ForcePlaybackFromExport = "ForcePlaybackFromExport";

	public const string ForceSequencesFromExport = "ForceSequencesFromExport";

	public const string CarouselCommand = "CarouselCommand";

	public const string Microphone = "Microphone";

	public const string RelatedMicrophone = "RelatedMicrophone";

	public const string Speaker = "Speaker";

	public const string RelatedSpeaker = "RelatedSpeaker";

	public const string Plugin = "Plugin";

	public const string InterconnectedHw = "InterconnectedHw";

	public const string IncludePackages = "IncludePackages";

	public const string ContainsAudio = "ContainsAudio";

	public const string SecondStepVerificationCode = "SecondStepAuthenticationPin";

	public const string PositionX = "PositionX";

	public const string PositionY = "PositionY";

	public const string ZoomLevel = "ZoomLevel";

	public const string ViewZoneFieldOfView = "ViewZoneFieldOfView";

	public const string ViewZoneViewDepth = "ViewZoneViewDepth";

	public const string ViewZoneViewDirection = "ViewZoneViewDirection";

	public const string OsmServerAddress = "OsmServerAddress";

	public const string GisMapLocation = "GisMapLocation";

	public const string GisMapCamera = "GisMapCamera";

	public const string BookmarkId = "BookmarkId";

	public const string IncludeEmptyItems = "IncludeEmptyItems";

	public const string ByteOrder = "ByteOrder";

	public const string StreamType = "StreamType";

	public const string ClientSupportsResampling = "SupportsResampling";

	public const string SupportsExtendedResampling = "SupportsExtendedResamplingFactor";

	public const string SupportsCarousel = "SupportsCarousel";

	public const string IdpClientId = "IdpClientId";

	public const string IdentityToken = "IdentityToken";

	public const string AccessToken = "AccessToken";

	public const string RefreshToken = "RefreshToken";

	public const string SupportsOutgoingAudio = "SupportsOutgoingAudio";

	public const string SupportsOutgoingPTTAudio = "SupportsOutgoingPTTAudio";

	public const string SupportsIncomingAudio = "SupportsIncomingAudio";

	public const string SupportsCommaSeparatedItemIds = "SupportsCommaSeparatedItemIds";

	public const string SupportsAudioIn = "SupportsAudioIn";

	public const string SupportsSpeakerAudioIn = "SupportsSpeakerAudioIn";

	public const string SupportsAudioOut = "SupportsAudioOut";

	public const string ClientRequiresDownsampling = "UserInitiatedDownsampling";

	public const string EulaAccepted = "EulaAccepted";

	public const string ProcessingMessage = "ProcessingMessage";

	public const string WebSocketSupport = "WebSocketSupport";

	public const string ServerId = "ServerId";

	public const string OriginalServerId = "OriginalServerId";

	public const string SupportsAlarms = "SupportsAlarms";

	public const string SupportsBookmarks = "SupportsBookmarks";

	public const string SupportsBookmarkExtendedSearchCameraIds = "SupportsBookmarkExtendedSearchCameraIds";

	public const string AllViewsAndCamerasEnabled = "AllViewsAndCamerasEnabled";

	public const string PredefinedConnectionString = "PredefinedUrl";

	public const string AutomaticConnectionString = "AutomaticUrl";

	public const string ServerAccessibility = "ServerAccessibility";

	public const string InvestigationsChangesStateId = "InvestigationsChangesStateId";

	public const string UserId = "UserId";

	public const string RequireCommandEncryption = "RequireCommandEncryption";

	public const string CommandEncryptionSupported = "CommandEncryptionSupported";

	public const string NoNextPlaylistOnDownload = "NoNextPlaylistOnDownload";

	public const string SupportsTwoStepsVerification = "SupportsTwoStepsVerification";

	public const string IsMapUpToDate = "IsMapUpToDate";

	public const string FootageName = "FootageName";

	public const string SupportsFootages = "Footages";

	public const string SupportsPtzGestures = "PtzGestures";

	public const string EncryptionPadding = "EncryptionPadding";

	public const string PrimeLength = "PrimeLength";

	public const string ClientType = "ClientType";

	public const string CanActivate = "CanActivate";

	public const string Trigger = "Trigger";

	public const string Activate = "Activate";

	public const string DbStartTime = "DbStartTime";

	public const string Slc = "Slc";

	public const string SiteId = "SiteId";

	public const string IsAlive = "IsAlive";

	public const string ServerName = "ServerName";

	public const string Emails = "Emails";

	public const string StoreLink = "StoreLink";

	public const string AppLink = "AppLink";

	public const string SmartConnectEnabled = "SmartConnectEnabled";

	public const string SpecificCulture = "SpecificCulture";

	public const string Heartbeat = "Heartbeat";

	public const string Device = "Device";

	public const string UnrecognizedDevice = "UnrecognizedDevice";

	public const string Configuration = "Configuration";

	public const string HeartbeatResponse = "HeartbeatResponse";

	public const string HeartbeatResponseResult = "HeartbeatResponseResult";

	public const string HeartbeatOnly = "HeartbeatOnly";

	public const string HeartbeatInterval = "HeartbeatInterval";

	public const string CoBrandingPresents = "CoBranding";

	public const string CoBrandingLogoLarge = "CoBrandingLogoLarge";

	public const string CoBrandingLogoSmall = "CoBrandingLogoSmall";

	public const string CoBrandingLogoWeb = "CoBrandingLogoWeb";

	public const string CoBrandingLink = "CoBrandingLink";

	public const string SecondStepVerificationProvider = "SecondStepAuthenticationProvider";

	public const string IsSuperUser = "IsSuperUser";

	public const string ServerDescription = "ServerDescription";

	public const string ServerType = "ServerType";

	public const string BasicUserManagementEnabled = "BasicUserManagementEnabled";

	public const string ChangePasswordRelativePath = "ChangePasswordRelativePath";

	public const string IdpRelativePath = "IdpRelativePath";

	public const string RestApiRelativePath = "RestApiRelativePath";

	public const string ExternalUsersSupported = "ExternalUsersSupported";

	public const string ErrorBadCommandType = "Bad Command Type";

	public const string ErrorBadCommandKind = "Bad Command Name";

	public const string ErrorWrongConnectionID = "Wrong connection ID";

	public const string ErrorMissingInpParam = "Missing mandatory input parameter: ";

	public const string ErrorWrongInpParam = "Wrong input parameter";

	public const string ErrorInternalError = "Internal error";

	public const string ErrorNotFullyImplemented = "Not Fully Implemented";

	public const string ErrorNotImplemented = "Not Implemented";

	public const string ErrorIncorrectUserPass = "Incorrect username/password";

	public const string ErrorUnknownCameraID = "Unknown camera ID or not enought privilegs to see this camera";

	public const string ErrorUnknownItemID = "Unknown item ID - defaulting to root";

	public const string ErrorNoPresetsAvailable = "No presets available";

	public const string ErrorIncorrectPublicKey = "Incorrect public key";

	public const string HttpContentTypeTextHtml = "text/html";

	public const string HttpContentTypeTextXml = "text/xml";

	public const string HttpContentTypeTextJson = "text/json";

	public const string HttpContentTypeTextCss = "text/css";

	public const string HttpContentTypeImageJpeg = "image/jpeg";

	public const string HttpContentTypeBinary = "application/octet-stream";

	public const string HttpMethodGet = "GET";

	public const string HttpMethodPost = "POST";

	public const string AccessControlEvent = "AccessControlEvent";

	public const string AccessControlDoor = "AccessControlDoor";

	public const string AccessControlSystem = "AccessControlSystem";

	public const string AccessControlDoorId = "AccessControlDoorId";

	public const string AccessControlSystemId = "AccessControlSystemId";

	public const string AccessControlSystemName = "AccessControlSystemName";

	public const string AccessControlRuleId = "AccessControlRuleId";

	public const string AccessControlPage = "AccessControlPage";

	public const string AccessControlPageSize = "AccessControlPageSize";

	public const string AccessControlStartTimestamp = "AccessControlStartTimestamp";

	public const string AccessControlEndTimestamp = "AccessControlEndTimestamp";

	public const string AccessControlIncludeEventsCount = "AccessControlIncludeEventsCount";

	public const string AccessControlReason = "AccessControlReason";

	public const string AccessControlCredentialHolders = "AccessControlCredentialHolders";

	public const string AccessControlCommands = "AccessControlCommands";

	public const string AccessControlCommandType = "AccessControlCommandType";

	public const string AccessControlCredentialHolderName = "AccessControlCredentialHolderName";

	public const string AccessControlCredentialHolderPictureData = "AccessControlCredentialHolderPictureData";

	public const string AccessControlCredentialHolderRoles = "AccessControlCredentialHolderRoles";

	public const string AccessControlCredentialHolderProperties = "AccessControlCredentialHolderProperties";

	public const string AccessControlCredentialHolderRole = "AccessControlCredentialHolderRole";

	public const string AccessControlCredentialHolderProperty = "AccessControlCredentialHolderProperty";

	public const string AccessControlCredentialHolderPropertyName = "AccessControlCredentialHolderPropertyName";

	public const string AccessControlCredentialHolderPropertyValue = "AccessControlCredentialHolderPropertyValue";

	public const string AccessControlOperationalInstanceId = "AccessControlOperationalInstanceId";

	public const string AccessControlCommandTypeId = "AccessControlCommandTypeId";

	public const string AccessControlCommandLabel = "AccessControlCommandLabel";

	public const string AccessControlCommandOrder = "AccessControlCommandOrder";

	public const string AccessControlEventId = "AccessControlEventId";

	public const string AccessControlTriggerSourceObjectId = "AccessControlTriggerSourceObjectId";

	public const string AccessControlKindId = "AccessControlKindId";

	public const string AccessControlEventsCount = "AccessControlEventsCount";

	public const string AccessControlSystemsCount = "AccessControlSystemsCount";

	public const string AccessControlDoorName = "AccessControlDoorName";

	public const string AccessControlDoorType = "AccessControlDoorType";

	public const string AccessControlDoorCategories = "AccessControlDoorCategories";

	public const string AccessControlDoorStates = "AccessControlDoorStates";

	public const string AccessControlDoorStateNames = "AccessControlDoorStateNames";

	public const string AccessControlDoorStatesReceivedTime = "AccessControlDoorStatesReceivedTime";

	public const string AccessControlEventType = "AccessControlEventType";

	public const string AccessControlEventTypeName = "AccessControlEventTypeName";

	public const string AccessControlDoorCategory = "AccessControlDoorCategory";

	public const string AccessControlDoorCategoryName = "AccessControlDoorCategoryName";

	public const string AccessControlDoorTypeName = "AccessControlDoorTypeName";

	public const string AccessControlEventCategory = "AccessControlEventCategory";

	public const string AccessControlEventCategoryName = "AccessControlEventCategoryName";

	public const string ExcludedAccessControlSystemIds = "ExcludedAccessControlSystemIds";

	public const string ExcludedAccessControlDoorIds = "ExcludedAccessControlDoorIds";

	public const string ExcludedAccessControlEventCategoryIds = "ExcludedAccessControlEventCategoryIds";

	public const string ExcludedAccessControlDoorCategoryIds = "ExcludedAccessControlDoorCategoryIds";

	public const string ExcludedAccessControlDoorTypeIds = "ExcludedAccessControlDoorTypeIds";

	public const string SupportsAccessControl = "SupportsAccessControl";

	public const string DirectStreamingValidLicense = "DirectStreamingValidLicense";

	public const string SupportsDirectStreamingLive = "DirectStreamingLive";

	public const string SupportsDirectStreamingPlayback = "DirectStreamingPlayback";

	public const string SupportsPlaybackInfoHeader = "SupportsPlaybackInfoHeader";

	public const string SupportsH264InFmp4 = "SupportsH264InFmp4";

	public const string SupportsH265InFmp4 = "SupportsH265InFmp4";

	public const string SupportsPlaybackInTimeRange = "SupportsPlaybackInTimeRange";

	public const string SupportsPlaybackAutoLoop = "SupportsPlaybackAutoLoop";

	public const string SupportsAdaptiveStreaming = "SupportsAdaptiveStreaming";

	public const string SupportsSmartMap = "SupportsSmartMap";

	public const string SupportsShare = "SupportsShare";

	public const string CollectUsageData = "CollectUsageData";

	public const string Exports = "Exports";

	public const string ViewOthersExports = "ViewOthersExports";

	public const string ExportToAvi = "ExportToAvi";

	public const string ExportToDb = "ExportToDb";

	public const string ExportToMkv = "ExportToMkv";

	public const string Email = "Email";

	public const string Authority = "Authority";

	public const string TileServer = "TileServer";

	public const string Address = "Address";

	public const string CompanyTileService = "CompanyTileService";
}
