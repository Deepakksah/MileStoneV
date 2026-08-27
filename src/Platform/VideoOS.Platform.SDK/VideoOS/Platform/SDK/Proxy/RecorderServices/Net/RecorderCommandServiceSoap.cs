using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[ServiceContract(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand", ConfigurationName = "VideoOS.Platform.SDK.Proxy.RecorderServices.Net.RecorderCommandServiceSoap")]
public interface RecorderCommandServiceSoap
{
	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/GetVersion", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	int GetVersion();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/GetVersion", ReplyAction = "*")]
	Task<int> GetVersionAsync();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZMoveAbsolute", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void PTZMoveAbsolute(string token, Guid deviceId, PTZArgs ptzArgs);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZMoveAbsolute", ReplyAction = "*")]
	Task PTZMoveAbsoluteAsync(string token, Guid deviceId, PTZArgs ptzArgs);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZMoveRelative", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void PTZMoveRelative(string token, Guid deviceId, PTZArgs ptzArgs);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZMoveRelative", ReplyAction = "*")]
	Task PTZMoveRelativeAsync(string token, Guid deviceId, PTZArgs ptzArgs);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZMoveHome", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void PTZMoveHome(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZMoveHome", ReplyAction = "*")]
	Task PTZMoveHomeAsync(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZMoveStart", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void PTZMoveStart(string token, Guid deviceId, PTZArgs ptzArgs);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZMoveStart", ReplyAction = "*")]
	Task PTZMoveStartAsync(string token, Guid deviceId, PTZArgs ptzArgs);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZMoveStop", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void PTZMoveStop(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZMoveStop", ReplyAction = "*")]
	Task PTZMoveStopAsync(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZCenter", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void PTZCenter(string token, Guid deviceId, double refWidth, double refHeight, double centerX, double centerY, double zoom);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZCenter", ReplyAction = "*")]
	Task PTZCenterAsync(string token, Guid deviceId, double refWidth, double refHeight, double centerX, double centerY, double zoom);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZRectangle", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void PTZRectangle(string token, Guid deviceId, double refWidth, double refHeight, double left, double top, double bottom, double right);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZRectangle", ReplyAction = "*")]
	Task PTZRectangleAsync(string token, Guid deviceId, double refWidth, double refHeight, double left, double top, double bottom, double right);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZGetAbsolutePosition", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	PTZArgs PTZGetAbsolutePosition(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZGetAbsolutePosition", ReplyAction = "*")]
	Task<PTZArgs> PTZGetAbsolutePositionAsync(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZExtendedManualSession", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void PTZExtendedManualSession(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZExtendedManualSession", ReplyAction = "*")]
	Task PTZExtendedManualSessionAsync(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZGetSessionInfo", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	PtzSessionInfo PTZGetSessionInfo(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZGetSessionInfo", ReplyAction = "*")]
	Task<PtzSessionInfo> PTZGetSessionInfoAsync(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZReserveSession", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void PTZReserveSession(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZReserveSession", ReplyAction = "*")]
	Task PTZReserveSessionAsync(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZReleaseSession", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void PTZReleaseSession(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZReleaseSession", ReplyAction = "*")]
	Task PTZReleaseSessionAsync(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZActivatePatrollingSchema", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void PTZActivatePatrollingSchema(string token, Guid deviceId, Guid patrollingSchemaId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZActivatePatrollingSchema", ReplyAction = "*")]
	Task PTZActivatePatrollingSchemaAsync(string token, Guid deviceId, Guid patrollingSchemaId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZDeactivatePatrollingSchema", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void PTZDeactivatePatrollingSchema(string token, Guid deviceId, Guid patrollingSchemaId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PTZDeactivatePatrollingSchema", ReplyAction = "*")]
	Task PTZDeactivatePatrollingSchemaAsync(string token, Guid deviceId, Guid patrollingSchemaId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PresetGetList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	string[] PresetGetList(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PresetGetList", ReplyAction = "*")]
	Task<string[]> PresetGetListAsync(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PresetMoveTo", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void PresetMoveTo(string token, Guid deviceId, string presetName);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PresetMoveTo", ReplyAction = "*")]
	Task PresetMoveToAsync(string token, Guid deviceId, string presetName);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PresetSet", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void PresetSet(string token, Guid deviceId, string presetName);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/PresetSet", ReplyAction = "*")]
	Task PresetSetAsync(string token, Guid deviceId, string presetName);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/OutputActivate", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void OutputActivate(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/OutputActivate", ReplyAction = "*")]
	Task OutputActivateAsync(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/OutputDeactivate", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void OutputDeactivate(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/OutputDeactivate", ReplyAction = "*")]
	Task OutputDeactivateAsync(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/OutputTrigger", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void OutputTrigger(string token, Guid deviceId, TimeDuration duration);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/OutputTrigger", ReplyAction = "*")]
	Task OutputTriggerAsync(string token, Guid deviceId, TimeDuration duration);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/OutputGetState", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	IOState OutputGetState(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/OutputGetState", ReplyAction = "*")]
	Task<IOState> OutputGetStateAsync(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SequencesGetTypes", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	SequenceType[] SequencesGetTypes(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SequencesGetTypes", ReplyAction = "*")]
	Task<SequenceType[]> SequencesGetTypesAsync(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SequencesGet", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	SequenceEntry[] SequencesGet(string token, Guid deviceId, Guid sequenceType, DateTime minTime, DateTime maxTime, int maxCount);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SequencesGet", ReplyAction = "*")]
	Task<SequenceEntry[]> SequencesGetAsync(string token, Guid deviceId, Guid sequenceType, DateTime minTime, DateTime maxTime, int maxCount);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SequencesGetAround", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	SequenceEntry[] SequencesGetAround(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, int maxCountBefore, int maxCountAfter);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SequencesGetAround", ReplyAction = "*")]
	Task<SequenceEntry[]> SequencesGetAroundAsync(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, int maxCountBefore, int maxCountAfter);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SequencesGetAroundWithSpan", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	SequenceEntry[] SequencesGetAroundWithSpan(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SequencesGetAroundWithSpan", ReplyAction = "*")]
	Task<SequenceEntry[]> SequencesGetAroundWithSpanAsync(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/GetRetentionTimeLimit", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	DateTime GetRetentionTimeLimit(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/GetRetentionTimeLimit", ReplyAction = "*")]
	Task<DateTime> GetRetentionTimeLimitAsync(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/TimeLineInformationGet", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	TimeLineInformationData[] TimeLineInformationGet(string token, Guid deviceId, Guid[] timeLineInformationTypes, DateTime timeLineInformationBeginTime, TimeDuration timeLineInformationInterval, int timeLineInformationCount);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/TimeLineInformationGet", ReplyAction = "*")]
	Task<TimeLineInformationData[]> TimeLineInformationGetAsync(string token, Guid deviceId, Guid[] timeLineInformationTypes, DateTime timeLineInformationBeginTime, TimeDuration timeLineInformationInterval, int timeLineInformationCount);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SmartSearchStart", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Guid SmartSearchStart(string token, Guid deviceId, DateTime searchBeginTime, DateTime searchEndTime, int sensitivity, TimeDuration searchInterval, ImageMask searchMask, bool ignoreInitialMotion, Size resultMotionMaskSize);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SmartSearchStart", ReplyAction = "*")]
	Task<Guid> SmartSearchStartAsync(string token, Guid deviceId, DateTime searchBeginTime, DateTime searchEndTime, int sensitivity, TimeDuration searchInterval, ImageMask searchMask, bool ignoreInitialMotion, Size resultMotionMaskSize);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SmartSearchCancel", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void SmartSearchCancel(string token, Guid smartSearchId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SmartSearchCancel", ReplyAction = "*")]
	Task SmartSearchCancelAsync(string token, Guid smartSearchId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SmartSearchGetStatus", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	SmartSearchStatus SmartSearchGetStatus(string token, Guid smartSearchId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SmartSearchGetStatus", ReplyAction = "*")]
	Task<SmartSearchStatus> SmartSearchGetStatusAsync(string token, Guid smartSearchId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SmartSearchGetResult", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	SmartSearchResult SmartSearchGetResult(string token, Guid smartSearchId, bool continueSearch);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SmartSearchGetResult", ReplyAction = "*")]
	Task<SmartSearchResult> SmartSearchGetResultAsync(string token, Guid smartSearchId, bool continueSearch);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/JPEGGetAt", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	JPEGData JPEGGetAt(string token, Guid deviceId, DateTime time);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/JPEGGetAt", ReplyAction = "*")]
	Task<JPEGData> JPEGGetAtAsync(string token, Guid deviceId, DateTime time);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/JPEGGetAtOrBefore", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	JPEGData JPEGGetAtOrBefore(string token, Guid deviceId, DateTime time);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/JPEGGetAtOrBefore", ReplyAction = "*")]
	Task<JPEGData> JPEGGetAtOrBeforeAsync(string token, Guid deviceId, DateTime time);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/JPEGGetAtOrAfter", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	JPEGData JPEGGetAtOrAfter(string token, Guid deviceId, DateTime time);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/JPEGGetAtOrAfter", ReplyAction = "*")]
	Task<JPEGData> JPEGGetAtOrAfterAsync(string token, Guid deviceId, DateTime time);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/JPEGGetLive", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	JPEGData JPEGGetLive(string token, Guid deviceId, int maxWidth, int maxHeight);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/JPEGGetLive", ReplyAction = "*")]
	Task<JPEGData> JPEGGetLiveAsync(string token, Guid deviceId, int maxWidth, int maxHeight);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/InputGetState", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	IOState InputGetState(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/InputGetState", ReplyAction = "*")]
	Task<IOState> InputGetStateAsync(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/StartRecording", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	bool StartRecording(string token, Guid deviceId, long recordingTimeMicroSeconds);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/StartRecording", ReplyAction = "*")]
	Task<bool> StartRecordingAsync(string token, Guid deviceId, long recordingTimeMicroSeconds);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SendDriverCommand", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void SendDriverCommand(string token, Guid deviceId, string command);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SendDriverCommand", ReplyAction = "*")]
	Task SendDriverCommandAsync(string token, Guid deviceId, string command);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SendDriverCommandWithResponse", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	string SendDriverCommandWithResponse(string token, Guid deviceId, string command, string parameter);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/SendDriverCommandWithResponse", ReplyAction = "*")]
	Task<string> SendDriverCommandWithResponseAsync(string token, Guid deviceId, string command, string parameter);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/MulticastStart", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	MulticastInfo[] MulticastStart(string token, Guid[] deviceIds, ClientCapabilities clientCapabilities);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/MulticastStart", ReplyAction = "*")]
	Task<MulticastInfo[]> MulticastStartAsync(string token, Guid[] deviceIds, ClientCapabilities clientCapabilities);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/MulticastContinue", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void MulticastContinue(string token, Guid[] multicastSessionIds);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/MulticastContinue", ReplyAction = "*")]
	Task MulticastContinueAsync(string token, Guid[] multicastSessionIds);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/MulticastStop", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	void MulticastStop(string token, Guid[] multicastSessionIds);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/MulticastStop", ReplyAction = "*")]
	Task MulticastStopAsync(string token, Guid[] multicastSessionIds);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/IsManualRecording", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ManualRecordingInfo[] IsManualRecording(string token, Guid[] deviceIds);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/IsManualRecording", ReplyAction = "*")]
	Task<ManualRecordingInfo[]> IsManualRecordingAsync(string token, Guid[] deviceIds);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/StartManualRecording", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ManualRecordingResult[] StartManualRecording(string token, Guid[] deviceIds);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/StartManualRecording", ReplyAction = "*")]
	Task<ManualRecordingResult[]> StartManualRecordingAsync(string token, Guid[] deviceIds);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/StopManualRecording", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ManualRecordingResult[] StopManualRecording(string token, Guid[] deviceIds);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderCommand/StopManualRecording", ReplyAction = "*")]
	Task<ManualRecordingResult[]> StopManualRecordingAsync(string token, Guid[] deviceIds);
}
