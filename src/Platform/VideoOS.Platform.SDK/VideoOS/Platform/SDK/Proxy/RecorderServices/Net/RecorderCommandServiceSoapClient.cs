using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
public class RecorderCommandServiceSoapClient : ClientBase<RecorderCommandServiceSoap>, RecorderCommandServiceSoap
{
	public enum EndpointConfiguration
	{
		RecorderCommandServiceSoap,
		RecorderCommandServiceSoap12
	}

	public RecorderCommandServiceSoapClient(EndpointConfiguration endpointConfiguration)
		: base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
	{
		base.Endpoint.Name = endpointConfiguration.ToString();
	}

	public RecorderCommandServiceSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
	{
		base.Endpoint.Name = endpointConfiguration.ToString();
	}

	public RecorderCommandServiceSoapClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
	{
		base.Endpoint.Name = endpointConfiguration.ToString();
	}

	public RecorderCommandServiceSoapClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public int GetVersion()
	{
		return base.Channel.GetVersion();
	}

	public Task<int> GetVersionAsync()
	{
		return base.Channel.GetVersionAsync();
	}

	public void PTZMoveAbsolute(string token, Guid deviceId, PTZArgs ptzArgs)
	{
		base.Channel.PTZMoveAbsolute(token, deviceId, ptzArgs);
	}

	public Task PTZMoveAbsoluteAsync(string token, Guid deviceId, PTZArgs ptzArgs)
	{
		return base.Channel.PTZMoveAbsoluteAsync(token, deviceId, ptzArgs);
	}

	public void PTZMoveRelative(string token, Guid deviceId, PTZArgs ptzArgs)
	{
		base.Channel.PTZMoveRelative(token, deviceId, ptzArgs);
	}

	public Task PTZMoveRelativeAsync(string token, Guid deviceId, PTZArgs ptzArgs)
	{
		return base.Channel.PTZMoveRelativeAsync(token, deviceId, ptzArgs);
	}

	public void PTZMoveHome(string token, Guid deviceId)
	{
		base.Channel.PTZMoveHome(token, deviceId);
	}

	public Task PTZMoveHomeAsync(string token, Guid deviceId)
	{
		return base.Channel.PTZMoveHomeAsync(token, deviceId);
	}

	public void PTZMoveStart(string token, Guid deviceId, PTZArgs ptzArgs)
	{
		base.Channel.PTZMoveStart(token, deviceId, ptzArgs);
	}

	public Task PTZMoveStartAsync(string token, Guid deviceId, PTZArgs ptzArgs)
	{
		return base.Channel.PTZMoveStartAsync(token, deviceId, ptzArgs);
	}

	public void PTZMoveStop(string token, Guid deviceId)
	{
		base.Channel.PTZMoveStop(token, deviceId);
	}

	public Task PTZMoveStopAsync(string token, Guid deviceId)
	{
		return base.Channel.PTZMoveStopAsync(token, deviceId);
	}

	public void PTZCenter(string token, Guid deviceId, double refWidth, double refHeight, double centerX, double centerY, double zoom)
	{
		base.Channel.PTZCenter(token, deviceId, refWidth, refHeight, centerX, centerY, zoom);
	}

	public Task PTZCenterAsync(string token, Guid deviceId, double refWidth, double refHeight, double centerX, double centerY, double zoom)
	{
		return base.Channel.PTZCenterAsync(token, deviceId, refWidth, refHeight, centerX, centerY, zoom);
	}

	public void PTZRectangle(string token, Guid deviceId, double refWidth, double refHeight, double left, double top, double bottom, double right)
	{
		base.Channel.PTZRectangle(token, deviceId, refWidth, refHeight, left, top, bottom, right);
	}

	public Task PTZRectangleAsync(string token, Guid deviceId, double refWidth, double refHeight, double left, double top, double bottom, double right)
	{
		return base.Channel.PTZRectangleAsync(token, deviceId, refWidth, refHeight, left, top, bottom, right);
	}

	public PTZArgs PTZGetAbsolutePosition(string token, Guid deviceId)
	{
		return base.Channel.PTZGetAbsolutePosition(token, deviceId);
	}

	public Task<PTZArgs> PTZGetAbsolutePositionAsync(string token, Guid deviceId)
	{
		return base.Channel.PTZGetAbsolutePositionAsync(token, deviceId);
	}

	public void PTZExtendedManualSession(string token, Guid deviceId)
	{
		base.Channel.PTZExtendedManualSession(token, deviceId);
	}

	public Task PTZExtendedManualSessionAsync(string token, Guid deviceId)
	{
		return base.Channel.PTZExtendedManualSessionAsync(token, deviceId);
	}

	public PtzSessionInfo PTZGetSessionInfo(string token, Guid deviceId)
	{
		return base.Channel.PTZGetSessionInfo(token, deviceId);
	}

	public Task<PtzSessionInfo> PTZGetSessionInfoAsync(string token, Guid deviceId)
	{
		return base.Channel.PTZGetSessionInfoAsync(token, deviceId);
	}

	public void PTZReserveSession(string token, Guid deviceId)
	{
		base.Channel.PTZReserveSession(token, deviceId);
	}

	public Task PTZReserveSessionAsync(string token, Guid deviceId)
	{
		return base.Channel.PTZReserveSessionAsync(token, deviceId);
	}

	public void PTZReleaseSession(string token, Guid deviceId)
	{
		base.Channel.PTZReleaseSession(token, deviceId);
	}

	public Task PTZReleaseSessionAsync(string token, Guid deviceId)
	{
		return base.Channel.PTZReleaseSessionAsync(token, deviceId);
	}

	public void PTZActivatePatrollingSchema(string token, Guid deviceId, Guid patrollingSchemaId)
	{
		base.Channel.PTZActivatePatrollingSchema(token, deviceId, patrollingSchemaId);
	}

	public Task PTZActivatePatrollingSchemaAsync(string token, Guid deviceId, Guid patrollingSchemaId)
	{
		return base.Channel.PTZActivatePatrollingSchemaAsync(token, deviceId, patrollingSchemaId);
	}

	public void PTZDeactivatePatrollingSchema(string token, Guid deviceId, Guid patrollingSchemaId)
	{
		base.Channel.PTZDeactivatePatrollingSchema(token, deviceId, patrollingSchemaId);
	}

	public Task PTZDeactivatePatrollingSchemaAsync(string token, Guid deviceId, Guid patrollingSchemaId)
	{
		return base.Channel.PTZDeactivatePatrollingSchemaAsync(token, deviceId, patrollingSchemaId);
	}

	public string[] PresetGetList(string token, Guid deviceId)
	{
		return base.Channel.PresetGetList(token, deviceId);
	}

	public Task<string[]> PresetGetListAsync(string token, Guid deviceId)
	{
		return base.Channel.PresetGetListAsync(token, deviceId);
	}

	public void PresetMoveTo(string token, Guid deviceId, string presetName)
	{
		base.Channel.PresetMoveTo(token, deviceId, presetName);
	}

	public Task PresetMoveToAsync(string token, Guid deviceId, string presetName)
	{
		return base.Channel.PresetMoveToAsync(token, deviceId, presetName);
	}

	public void PresetSet(string token, Guid deviceId, string presetName)
	{
		base.Channel.PresetSet(token, deviceId, presetName);
	}

	public Task PresetSetAsync(string token, Guid deviceId, string presetName)
	{
		return base.Channel.PresetSetAsync(token, deviceId, presetName);
	}

	public void OutputActivate(string token, Guid deviceId)
	{
		base.Channel.OutputActivate(token, deviceId);
	}

	public Task OutputActivateAsync(string token, Guid deviceId)
	{
		return base.Channel.OutputActivateAsync(token, deviceId);
	}

	public void OutputDeactivate(string token, Guid deviceId)
	{
		base.Channel.OutputDeactivate(token, deviceId);
	}

	public Task OutputDeactivateAsync(string token, Guid deviceId)
	{
		return base.Channel.OutputDeactivateAsync(token, deviceId);
	}

	public void OutputTrigger(string token, Guid deviceId, TimeDuration duration)
	{
		base.Channel.OutputTrigger(token, deviceId, duration);
	}

	public Task OutputTriggerAsync(string token, Guid deviceId, TimeDuration duration)
	{
		return base.Channel.OutputTriggerAsync(token, deviceId, duration);
	}

	public IOState OutputGetState(string token, Guid deviceId)
	{
		return base.Channel.OutputGetState(token, deviceId);
	}

	public Task<IOState> OutputGetStateAsync(string token, Guid deviceId)
	{
		return base.Channel.OutputGetStateAsync(token, deviceId);
	}

	public SequenceType[] SequencesGetTypes(string token, Guid deviceId)
	{
		return base.Channel.SequencesGetTypes(token, deviceId);
	}

	public Task<SequenceType[]> SequencesGetTypesAsync(string token, Guid deviceId)
	{
		return base.Channel.SequencesGetTypesAsync(token, deviceId);
	}

	public SequenceEntry[] SequencesGet(string token, Guid deviceId, Guid sequenceType, DateTime minTime, DateTime maxTime, int maxCount)
	{
		return base.Channel.SequencesGet(token, deviceId, sequenceType, minTime, maxTime, maxCount);
	}

	public Task<SequenceEntry[]> SequencesGetAsync(string token, Guid deviceId, Guid sequenceType, DateTime minTime, DateTime maxTime, int maxCount)
	{
		return base.Channel.SequencesGetAsync(token, deviceId, sequenceType, minTime, maxTime, maxCount);
	}

	public SequenceEntry[] SequencesGetAround(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, int maxCountBefore, int maxCountAfter)
	{
		return base.Channel.SequencesGetAround(token, deviceId, sequenceType, centerTime, maxCountBefore, maxCountAfter);
	}

	public Task<SequenceEntry[]> SequencesGetAroundAsync(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, int maxCountBefore, int maxCountAfter)
	{
		return base.Channel.SequencesGetAroundAsync(token, deviceId, sequenceType, centerTime, maxCountBefore, maxCountAfter);
	}

	public SequenceEntry[] SequencesGetAroundWithSpan(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter)
	{
		return base.Channel.SequencesGetAroundWithSpan(token, deviceId, sequenceType, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter);
	}

	public Task<SequenceEntry[]> SequencesGetAroundWithSpanAsync(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter)
	{
		return base.Channel.SequencesGetAroundWithSpanAsync(token, deviceId, sequenceType, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter);
	}

	public DateTime GetRetentionTimeLimit(string token, Guid deviceId)
	{
		return base.Channel.GetRetentionTimeLimit(token, deviceId);
	}

	public Task<DateTime> GetRetentionTimeLimitAsync(string token, Guid deviceId)
	{
		return base.Channel.GetRetentionTimeLimitAsync(token, deviceId);
	}

	public TimeLineInformationData[] TimeLineInformationGet(string token, Guid deviceId, Guid[] timeLineInformationTypes, DateTime timeLineInformationBeginTime, TimeDuration timeLineInformationInterval, int timeLineInformationCount)
	{
		return base.Channel.TimeLineInformationGet(token, deviceId, timeLineInformationTypes, timeLineInformationBeginTime, timeLineInformationInterval, timeLineInformationCount);
	}

	public Task<TimeLineInformationData[]> TimeLineInformationGetAsync(string token, Guid deviceId, Guid[] timeLineInformationTypes, DateTime timeLineInformationBeginTime, TimeDuration timeLineInformationInterval, int timeLineInformationCount)
	{
		return base.Channel.TimeLineInformationGetAsync(token, deviceId, timeLineInformationTypes, timeLineInformationBeginTime, timeLineInformationInterval, timeLineInformationCount);
	}

	public Guid SmartSearchStart(string token, Guid deviceId, DateTime searchBeginTime, DateTime searchEndTime, int sensitivity, TimeDuration searchInterval, ImageMask searchMask, bool ignoreInitialMotion, Size resultMotionMaskSize)
	{
		return base.Channel.SmartSearchStart(token, deviceId, searchBeginTime, searchEndTime, sensitivity, searchInterval, searchMask, ignoreInitialMotion, resultMotionMaskSize);
	}

	public Task<Guid> SmartSearchStartAsync(string token, Guid deviceId, DateTime searchBeginTime, DateTime searchEndTime, int sensitivity, TimeDuration searchInterval, ImageMask searchMask, bool ignoreInitialMotion, Size resultMotionMaskSize)
	{
		return base.Channel.SmartSearchStartAsync(token, deviceId, searchBeginTime, searchEndTime, sensitivity, searchInterval, searchMask, ignoreInitialMotion, resultMotionMaskSize);
	}

	public void SmartSearchCancel(string token, Guid smartSearchId)
	{
		base.Channel.SmartSearchCancel(token, smartSearchId);
	}

	public Task SmartSearchCancelAsync(string token, Guid smartSearchId)
	{
		return base.Channel.SmartSearchCancelAsync(token, smartSearchId);
	}

	public SmartSearchStatus SmartSearchGetStatus(string token, Guid smartSearchId)
	{
		return base.Channel.SmartSearchGetStatus(token, smartSearchId);
	}

	public Task<SmartSearchStatus> SmartSearchGetStatusAsync(string token, Guid smartSearchId)
	{
		return base.Channel.SmartSearchGetStatusAsync(token, smartSearchId);
	}

	public SmartSearchResult SmartSearchGetResult(string token, Guid smartSearchId, bool continueSearch)
	{
		return base.Channel.SmartSearchGetResult(token, smartSearchId, continueSearch);
	}

	public Task<SmartSearchResult> SmartSearchGetResultAsync(string token, Guid smartSearchId, bool continueSearch)
	{
		return base.Channel.SmartSearchGetResultAsync(token, smartSearchId, continueSearch);
	}

	public JPEGData JPEGGetAt(string token, Guid deviceId, DateTime time)
	{
		return base.Channel.JPEGGetAt(token, deviceId, time);
	}

	public Task<JPEGData> JPEGGetAtAsync(string token, Guid deviceId, DateTime time)
	{
		return base.Channel.JPEGGetAtAsync(token, deviceId, time);
	}

	public JPEGData JPEGGetAtOrBefore(string token, Guid deviceId, DateTime time)
	{
		return base.Channel.JPEGGetAtOrBefore(token, deviceId, time);
	}

	public Task<JPEGData> JPEGGetAtOrBeforeAsync(string token, Guid deviceId, DateTime time)
	{
		return base.Channel.JPEGGetAtOrBeforeAsync(token, deviceId, time);
	}

	public JPEGData JPEGGetAtOrAfter(string token, Guid deviceId, DateTime time)
	{
		return base.Channel.JPEGGetAtOrAfter(token, deviceId, time);
	}

	public Task<JPEGData> JPEGGetAtOrAfterAsync(string token, Guid deviceId, DateTime time)
	{
		return base.Channel.JPEGGetAtOrAfterAsync(token, deviceId, time);
	}

	public JPEGData JPEGGetLive(string token, Guid deviceId, int maxWidth, int maxHeight)
	{
		return base.Channel.JPEGGetLive(token, deviceId, maxWidth, maxHeight);
	}

	public Task<JPEGData> JPEGGetLiveAsync(string token, Guid deviceId, int maxWidth, int maxHeight)
	{
		return base.Channel.JPEGGetLiveAsync(token, deviceId, maxWidth, maxHeight);
	}

	public IOState InputGetState(string token, Guid deviceId)
	{
		return base.Channel.InputGetState(token, deviceId);
	}

	public Task<IOState> InputGetStateAsync(string token, Guid deviceId)
	{
		return base.Channel.InputGetStateAsync(token, deviceId);
	}

	public bool StartRecording(string token, Guid deviceId, long recordingTimeMicroSeconds)
	{
		return base.Channel.StartRecording(token, deviceId, recordingTimeMicroSeconds);
	}

	public Task<bool> StartRecordingAsync(string token, Guid deviceId, long recordingTimeMicroSeconds)
	{
		return base.Channel.StartRecordingAsync(token, deviceId, recordingTimeMicroSeconds);
	}

	public void SendDriverCommand(string token, Guid deviceId, string command)
	{
		base.Channel.SendDriverCommand(token, deviceId, command);
	}

	public Task SendDriverCommandAsync(string token, Guid deviceId, string command)
	{
		return base.Channel.SendDriverCommandAsync(token, deviceId, command);
	}

	public string SendDriverCommandWithResponse(string token, Guid deviceId, string command, string parameter)
	{
		return base.Channel.SendDriverCommandWithResponse(token, deviceId, command, parameter);
	}

	public Task<string> SendDriverCommandWithResponseAsync(string token, Guid deviceId, string command, string parameter)
	{
		return base.Channel.SendDriverCommandWithResponseAsync(token, deviceId, command, parameter);
	}

	public MulticastInfo[] MulticastStart(string token, Guid[] deviceIds, ClientCapabilities clientCapabilities)
	{
		return base.Channel.MulticastStart(token, deviceIds, clientCapabilities);
	}

	public Task<MulticastInfo[]> MulticastStartAsync(string token, Guid[] deviceIds, ClientCapabilities clientCapabilities)
	{
		return base.Channel.MulticastStartAsync(token, deviceIds, clientCapabilities);
	}

	public void MulticastContinue(string token, Guid[] multicastSessionIds)
	{
		base.Channel.MulticastContinue(token, multicastSessionIds);
	}

	public Task MulticastContinueAsync(string token, Guid[] multicastSessionIds)
	{
		return base.Channel.MulticastContinueAsync(token, multicastSessionIds);
	}

	public void MulticastStop(string token, Guid[] multicastSessionIds)
	{
		base.Channel.MulticastStop(token, multicastSessionIds);
	}

	public Task MulticastStopAsync(string token, Guid[] multicastSessionIds)
	{
		return base.Channel.MulticastStopAsync(token, multicastSessionIds);
	}

	public ManualRecordingInfo[] IsManualRecording(string token, Guid[] deviceIds)
	{
		return base.Channel.IsManualRecording(token, deviceIds);
	}

	public Task<ManualRecordingInfo[]> IsManualRecordingAsync(string token, Guid[] deviceIds)
	{
		return base.Channel.IsManualRecordingAsync(token, deviceIds);
	}

	public ManualRecordingResult[] StartManualRecording(string token, Guid[] deviceIds)
	{
		return base.Channel.StartManualRecording(token, deviceIds);
	}

	public Task<ManualRecordingResult[]> StartManualRecordingAsync(string token, Guid[] deviceIds)
	{
		return base.Channel.StartManualRecordingAsync(token, deviceIds);
	}

	public ManualRecordingResult[] StopManualRecording(string token, Guid[] deviceIds)
	{
		return base.Channel.StopManualRecording(token, deviceIds);
	}

	public Task<ManualRecordingResult[]> StopManualRecordingAsync(string token, Guid[] deviceIds)
	{
		return base.Channel.StopManualRecordingAsync(token, deviceIds);
	}

	public virtual Task OpenAsync()
	{
		return Task.Factory.FromAsync(((ICommunicationObject)this).BeginOpen((AsyncCallback)null, (object)null), ((ICommunicationObject)this).EndOpen);
	}

	private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
	{
		switch (endpointConfiguration)
		{
		case EndpointConfiguration.RecorderCommandServiceSoap:
			return new BasicHttpBinding
			{
				MaxBufferSize = int.MaxValue,
				ReaderQuotas = XmlDictionaryReaderQuotas.Max,
				MaxReceivedMessageSize = 2147483647L,
				AllowCookies = true
			};
		case EndpointConfiguration.RecorderCommandServiceSoap12:
		{
			CustomBinding customBinding = new CustomBinding();
			TextMessageEncodingBindingElement item = new TextMessageEncodingBindingElement
			{
				MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None)
			};
			customBinding.Elements.Add(item);
			HttpTransportBindingElement item2 = new HttpTransportBindingElement
			{
				AllowCookies = true,
				MaxBufferSize = int.MaxValue,
				MaxReceivedMessageSize = 2147483647L
			};
			customBinding.Elements.Add(item2);
			return customBinding;
		}
		default:
			throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
		}
	}

	private static EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
	{
		return endpointConfiguration switch
		{
			EndpointConfiguration.RecorderCommandServiceSoap => new EndpointAddress("http://dkws-apimipsdk1.milestone.dk:7563/recordercommandservice/recordercommandservice.asmx"), 
			EndpointConfiguration.RecorderCommandServiceSoap12 => new EndpointAddress("http://dkws-apimipsdk1.milestone.dk:7563/recordercommandservice/recordercommandservice.asmx"), 
			_ => throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'."), 
		};
	}
}
