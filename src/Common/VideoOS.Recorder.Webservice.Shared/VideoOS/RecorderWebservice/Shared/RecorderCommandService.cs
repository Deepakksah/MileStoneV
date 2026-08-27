using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace VideoOS.RecorderWebservice.Shared;

[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[WebServiceBinding(Name = "RecorderCommandServiceSoap", Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class RecorderCommandService : SoapHttpClientProtocol
{
	private SendOrPostCallback GetVersionOperationCompleted;

	private SendOrPostCallback PTZMoveAbsoluteOperationCompleted;

	private SendOrPostCallback PTZMoveRelativeOperationCompleted;

	private SendOrPostCallback PTZMoveHomeOperationCompleted;

	private SendOrPostCallback PTZMoveStartOperationCompleted;

	private SendOrPostCallback PTZMoveStopOperationCompleted;

	private SendOrPostCallback PTZCenterOperationCompleted;

	private SendOrPostCallback PTZRectangleOperationCompleted;

	private SendOrPostCallback PTZGetAbsolutePositionOperationCompleted;

	private SendOrPostCallback PTZExtendedManualSessionOperationCompleted;

	private SendOrPostCallback PTZGetSessionInfoOperationCompleted;

	private SendOrPostCallback PTZReserveSessionOperationCompleted;

	private SendOrPostCallback PTZReleaseSessionOperationCompleted;

	private SendOrPostCallback PTZActivatePatrollingSchemaOperationCompleted;

	private SendOrPostCallback PTZDeactivatePatrollingSchemaOperationCompleted;

	private SendOrPostCallback PresetGetListOperationCompleted;

	private SendOrPostCallback PresetMoveToOperationCompleted;

	private SendOrPostCallback PresetSetOperationCompleted;

	private SendOrPostCallback OutputActivateOperationCompleted;

	private SendOrPostCallback OutputDeactivateOperationCompleted;

	private SendOrPostCallback OutputTriggerOperationCompleted;

	private SendOrPostCallback OutputGetStateOperationCompleted;

	private SendOrPostCallback SequencesGetTypesOperationCompleted;

	private SendOrPostCallback SequencesGetOperationCompleted;

	private SendOrPostCallback SequencesGetAroundOperationCompleted;

	private SendOrPostCallback SequencesGetAroundWithSpanOperationCompleted;

	private SendOrPostCallback GetRetentionTimeLimitOperationCompleted;

	private SendOrPostCallback TimeLineInformationGetOperationCompleted;

	private SendOrPostCallback SmartSearchStartOperationCompleted;

	private SendOrPostCallback SmartSearchCancelOperationCompleted;

	private SendOrPostCallback SmartSearchGetStatusOperationCompleted;

	private SendOrPostCallback SmartSearchGetResultOperationCompleted;

	private SendOrPostCallback JPEGGetAtOperationCompleted;

	private SendOrPostCallback JPEGGetAtOrBeforeOperationCompleted;

	private SendOrPostCallback JPEGGetAtOrAfterOperationCompleted;

	private SendOrPostCallback JPEGGetLiveOperationCompleted;

	private SendOrPostCallback InputGetStateOperationCompleted;

	private SendOrPostCallback StartRecordingOperationCompleted;

	private SendOrPostCallback SendDriverCommandOperationCompleted;

	private SendOrPostCallback SendDriverCommandWithResponseOperationCompleted;

	private SendOrPostCallback MulticastStartOperationCompleted;

	private SendOrPostCallback MulticastContinueOperationCompleted;

	private SendOrPostCallback MulticastStopOperationCompleted;

	private SendOrPostCallback IsManualRecordingOperationCompleted;

	private SendOrPostCallback StartManualRecordingOperationCompleted;

	private SendOrPostCallback StopManualRecordingOperationCompleted;

	public event GetVersionCompletedEventHandler GetVersionCompleted;

	public event PTZMoveAbsoluteCompletedEventHandler PTZMoveAbsoluteCompleted;

	public event PTZMoveRelativeCompletedEventHandler PTZMoveRelativeCompleted;

	public event PTZMoveHomeCompletedEventHandler PTZMoveHomeCompleted;

	public event PTZMoveStartCompletedEventHandler PTZMoveStartCompleted;

	public event PTZMoveStopCompletedEventHandler PTZMoveStopCompleted;

	public event PTZCenterCompletedEventHandler PTZCenterCompleted;

	public event PTZRectangleCompletedEventHandler PTZRectangleCompleted;

	public event PTZGetAbsolutePositionCompletedEventHandler PTZGetAbsolutePositionCompleted;

	public event PTZExtendedManualSessionCompletedEventHandler PTZExtendedManualSessionCompleted;

	public event PTZGetSessionInfoCompletedEventHandler PTZGetSessionInfoCompleted;

	public event PTZReserveSessionCompletedEventHandler PTZReserveSessionCompleted;

	public event PTZReleaseSessionCompletedEventHandler PTZReleaseSessionCompleted;

	public event PTZActivatePatrollingSchemaCompletedEventHandler PTZActivatePatrollingSchemaCompleted;

	public event PTZDeactivatePatrollingSchemaCompletedEventHandler PTZDeactivatePatrollingSchemaCompleted;

	public event PresetGetListCompletedEventHandler PresetGetListCompleted;

	public event PresetMoveToCompletedEventHandler PresetMoveToCompleted;

	public event PresetSetCompletedEventHandler PresetSetCompleted;

	public event OutputActivateCompletedEventHandler OutputActivateCompleted;

	public event OutputDeactivateCompletedEventHandler OutputDeactivateCompleted;

	public event OutputTriggerCompletedEventHandler OutputTriggerCompleted;

	public event OutputGetStateCompletedEventHandler OutputGetStateCompleted;

	public event SequencesGetTypesCompletedEventHandler SequencesGetTypesCompleted;

	public event SequencesGetCompletedEventHandler SequencesGetCompleted;

	public event SequencesGetAroundCompletedEventHandler SequencesGetAroundCompleted;

	public event SequencesGetAroundWithSpanCompletedEventHandler SequencesGetAroundWithSpanCompleted;

	public event GetRetentionTimeLimitCompletedEventHandler GetRetentionTimeLimitCompleted;

	public event TimeLineInformationGetCompletedEventHandler TimeLineInformationGetCompleted;

	public event SmartSearchStartCompletedEventHandler SmartSearchStartCompleted;

	public event SmartSearchCancelCompletedEventHandler SmartSearchCancelCompleted;

	public event SmartSearchGetStatusCompletedEventHandler SmartSearchGetStatusCompleted;

	public event SmartSearchGetResultCompletedEventHandler SmartSearchGetResultCompleted;

	public event JPEGGetAtCompletedEventHandler JPEGGetAtCompleted;

	public event JPEGGetAtOrBeforeCompletedEventHandler JPEGGetAtOrBeforeCompleted;

	public event JPEGGetAtOrAfterCompletedEventHandler JPEGGetAtOrAfterCompleted;

	public event JPEGGetLiveCompletedEventHandler JPEGGetLiveCompleted;

	public event InputGetStateCompletedEventHandler InputGetStateCompleted;

	public event StartRecordingCompletedEventHandler StartRecordingCompleted;

	public event SendDriverCommandCompletedEventHandler SendDriverCommandCompleted;

	public event SendDriverCommandWithResponseCompletedEventHandler SendDriverCommandWithResponseCompleted;

	public event MulticastStartCompletedEventHandler MulticastStartCompleted;

	public event MulticastContinueCompletedEventHandler MulticastContinueCompleted;

	public event MulticastStopCompletedEventHandler MulticastStopCompleted;

	public event IsManualRecordingCompletedEventHandler IsManualRecordingCompleted;

	public event StartManualRecordingCompletedEventHandler StartManualRecordingCompleted;

	public event StopManualRecordingCompletedEventHandler StopManualRecordingCompleted;

	public RecorderCommandService()
	{
		base.Url = "http://localhost/RecorderAPIGenerator/RecorderCommandService.asmx";
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/GetVersion", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int GetVersion()
	{
		return (int)Invoke("GetVersion", new object[0])[0];
	}

	public IAsyncResult BeginGetVersion(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetVersion", new object[0], callback, asyncState);
	}

	public int EndGetVersion(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void GetVersionAsync()
	{
		GetVersionAsync(null);
	}

	public void GetVersionAsync(object userState)
	{
		if (GetVersionOperationCompleted == null)
		{
			GetVersionOperationCompleted = OnGetVersionOperationCompleted;
		}
		InvokeAsync("GetVersion", new object[0], GetVersionOperationCompleted, userState);
	}

	private void OnGetVersionOperationCompleted(object arg)
	{
		if (this.GetVersionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetVersionCompleted(this, new GetVersionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PTZMoveAbsolute", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void PTZMoveAbsolute(string token, Guid deviceId, PTZArgs ptzArgs)
	{
		Invoke("PTZMoveAbsolute", new object[3] { token, deviceId, ptzArgs });
	}

	public IAsyncResult BeginPTZMoveAbsolute(string token, Guid deviceId, PTZArgs ptzArgs, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PTZMoveAbsolute", new object[3] { token, deviceId, ptzArgs }, callback, asyncState);
	}

	public void EndPTZMoveAbsolute(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void PTZMoveAbsoluteAsync(string token, Guid deviceId, PTZArgs ptzArgs)
	{
		PTZMoveAbsoluteAsync(token, deviceId, ptzArgs, null);
	}

	public void PTZMoveAbsoluteAsync(string token, Guid deviceId, PTZArgs ptzArgs, object userState)
	{
		if (PTZMoveAbsoluteOperationCompleted == null)
		{
			PTZMoveAbsoluteOperationCompleted = OnPTZMoveAbsoluteOperationCompleted;
		}
		InvokeAsync("PTZMoveAbsolute", new object[3] { token, deviceId, ptzArgs }, PTZMoveAbsoluteOperationCompleted, userState);
	}

	private void OnPTZMoveAbsoluteOperationCompleted(object arg)
	{
		if (this.PTZMoveAbsoluteCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PTZMoveAbsoluteCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PTZMoveRelative", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void PTZMoveRelative(string token, Guid deviceId, PTZArgs ptzArgs)
	{
		Invoke("PTZMoveRelative", new object[3] { token, deviceId, ptzArgs });
	}

	public IAsyncResult BeginPTZMoveRelative(string token, Guid deviceId, PTZArgs ptzArgs, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PTZMoveRelative", new object[3] { token, deviceId, ptzArgs }, callback, asyncState);
	}

	public void EndPTZMoveRelative(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void PTZMoveRelativeAsync(string token, Guid deviceId, PTZArgs ptzArgs)
	{
		PTZMoveRelativeAsync(token, deviceId, ptzArgs, null);
	}

	public void PTZMoveRelativeAsync(string token, Guid deviceId, PTZArgs ptzArgs, object userState)
	{
		if (PTZMoveRelativeOperationCompleted == null)
		{
			PTZMoveRelativeOperationCompleted = OnPTZMoveRelativeOperationCompleted;
		}
		InvokeAsync("PTZMoveRelative", new object[3] { token, deviceId, ptzArgs }, PTZMoveRelativeOperationCompleted, userState);
	}

	private void OnPTZMoveRelativeOperationCompleted(object arg)
	{
		if (this.PTZMoveRelativeCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PTZMoveRelativeCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PTZMoveHome", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void PTZMoveHome(string token, Guid deviceId)
	{
		Invoke("PTZMoveHome", new object[2] { token, deviceId });
	}

	public IAsyncResult BeginPTZMoveHome(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PTZMoveHome", new object[2] { token, deviceId }, callback, asyncState);
	}

	public void EndPTZMoveHome(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void PTZMoveHomeAsync(string token, Guid deviceId)
	{
		PTZMoveHomeAsync(token, deviceId, null);
	}

	public void PTZMoveHomeAsync(string token, Guid deviceId, object userState)
	{
		if (PTZMoveHomeOperationCompleted == null)
		{
			PTZMoveHomeOperationCompleted = OnPTZMoveHomeOperationCompleted;
		}
		InvokeAsync("PTZMoveHome", new object[2] { token, deviceId }, PTZMoveHomeOperationCompleted, userState);
	}

	private void OnPTZMoveHomeOperationCompleted(object arg)
	{
		if (this.PTZMoveHomeCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PTZMoveHomeCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PTZMoveStart", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void PTZMoveStart(string token, Guid deviceId, PTZArgs ptzArgs)
	{
		Invoke("PTZMoveStart", new object[3] { token, deviceId, ptzArgs });
	}

	public IAsyncResult BeginPTZMoveStart(string token, Guid deviceId, PTZArgs ptzArgs, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PTZMoveStart", new object[3] { token, deviceId, ptzArgs }, callback, asyncState);
	}

	public void EndPTZMoveStart(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void PTZMoveStartAsync(string token, Guid deviceId, PTZArgs ptzArgs)
	{
		PTZMoveStartAsync(token, deviceId, ptzArgs, null);
	}

	public void PTZMoveStartAsync(string token, Guid deviceId, PTZArgs ptzArgs, object userState)
	{
		if (PTZMoveStartOperationCompleted == null)
		{
			PTZMoveStartOperationCompleted = OnPTZMoveStartOperationCompleted;
		}
		InvokeAsync("PTZMoveStart", new object[3] { token, deviceId, ptzArgs }, PTZMoveStartOperationCompleted, userState);
	}

	private void OnPTZMoveStartOperationCompleted(object arg)
	{
		if (this.PTZMoveStartCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PTZMoveStartCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PTZMoveStop", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void PTZMoveStop(string token, Guid deviceId)
	{
		Invoke("PTZMoveStop", new object[2] { token, deviceId });
	}

	public IAsyncResult BeginPTZMoveStop(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PTZMoveStop", new object[2] { token, deviceId }, callback, asyncState);
	}

	public void EndPTZMoveStop(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void PTZMoveStopAsync(string token, Guid deviceId)
	{
		PTZMoveStopAsync(token, deviceId, null);
	}

	public void PTZMoveStopAsync(string token, Guid deviceId, object userState)
	{
		if (PTZMoveStopOperationCompleted == null)
		{
			PTZMoveStopOperationCompleted = OnPTZMoveStopOperationCompleted;
		}
		InvokeAsync("PTZMoveStop", new object[2] { token, deviceId }, PTZMoveStopOperationCompleted, userState);
	}

	private void OnPTZMoveStopOperationCompleted(object arg)
	{
		if (this.PTZMoveStopCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PTZMoveStopCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PTZCenter", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void PTZCenter(string token, Guid deviceId, double refWidth, double refHeight, double centerX, double centerY, double zoom)
	{
		Invoke("PTZCenter", new object[7] { token, deviceId, refWidth, refHeight, centerX, centerY, zoom });
	}

	public IAsyncResult BeginPTZCenter(string token, Guid deviceId, double refWidth, double refHeight, double centerX, double centerY, double zoom, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PTZCenter", new object[7] { token, deviceId, refWidth, refHeight, centerX, centerY, zoom }, callback, asyncState);
	}

	public void EndPTZCenter(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void PTZCenterAsync(string token, Guid deviceId, double refWidth, double refHeight, double centerX, double centerY, double zoom)
	{
		PTZCenterAsync(token, deviceId, refWidth, refHeight, centerX, centerY, zoom, null);
	}

	public void PTZCenterAsync(string token, Guid deviceId, double refWidth, double refHeight, double centerX, double centerY, double zoom, object userState)
	{
		if (PTZCenterOperationCompleted == null)
		{
			PTZCenterOperationCompleted = OnPTZCenterOperationCompleted;
		}
		InvokeAsync("PTZCenter", new object[7] { token, deviceId, refWidth, refHeight, centerX, centerY, zoom }, PTZCenterOperationCompleted, userState);
	}

	private void OnPTZCenterOperationCompleted(object arg)
	{
		if (this.PTZCenterCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PTZCenterCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PTZRectangle", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void PTZRectangle(string token, Guid deviceId, double refWidth, double refHeight, double left, double top, double bottom, double right)
	{
		Invoke("PTZRectangle", new object[8] { token, deviceId, refWidth, refHeight, left, top, bottom, right });
	}

	public IAsyncResult BeginPTZRectangle(string token, Guid deviceId, double refWidth, double refHeight, double left, double top, double bottom, double right, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PTZRectangle", new object[8] { token, deviceId, refWidth, refHeight, left, top, bottom, right }, callback, asyncState);
	}

	public void EndPTZRectangle(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void PTZRectangleAsync(string token, Guid deviceId, double refWidth, double refHeight, double left, double top, double bottom, double right)
	{
		PTZRectangleAsync(token, deviceId, refWidth, refHeight, left, top, bottom, right, null);
	}

	public void PTZRectangleAsync(string token, Guid deviceId, double refWidth, double refHeight, double left, double top, double bottom, double right, object userState)
	{
		if (PTZRectangleOperationCompleted == null)
		{
			PTZRectangleOperationCompleted = OnPTZRectangleOperationCompleted;
		}
		InvokeAsync("PTZRectangle", new object[8] { token, deviceId, refWidth, refHeight, left, top, bottom, right }, PTZRectangleOperationCompleted, userState);
	}

	private void OnPTZRectangleOperationCompleted(object arg)
	{
		if (this.PTZRectangleCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PTZRectangleCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PTZGetAbsolutePosition", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public PTZArgs PTZGetAbsolutePosition(string token, Guid deviceId)
	{
		return (PTZArgs)Invoke("PTZGetAbsolutePosition", new object[2] { token, deviceId })[0];
	}

	public IAsyncResult BeginPTZGetAbsolutePosition(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PTZGetAbsolutePosition", new object[2] { token, deviceId }, callback, asyncState);
	}

	public PTZArgs EndPTZGetAbsolutePosition(IAsyncResult asyncResult)
	{
		return (PTZArgs)EndInvoke(asyncResult)[0];
	}

	public void PTZGetAbsolutePositionAsync(string token, Guid deviceId)
	{
		PTZGetAbsolutePositionAsync(token, deviceId, null);
	}

	public void PTZGetAbsolutePositionAsync(string token, Guid deviceId, object userState)
	{
		if (PTZGetAbsolutePositionOperationCompleted == null)
		{
			PTZGetAbsolutePositionOperationCompleted = OnPTZGetAbsolutePositionOperationCompleted;
		}
		InvokeAsync("PTZGetAbsolutePosition", new object[2] { token, deviceId }, PTZGetAbsolutePositionOperationCompleted, userState);
	}

	private void OnPTZGetAbsolutePositionOperationCompleted(object arg)
	{
		if (this.PTZGetAbsolutePositionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PTZGetAbsolutePositionCompleted(this, new PTZGetAbsolutePositionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PTZExtendedManualSession", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void PTZExtendedManualSession(string token, Guid deviceId)
	{
		Invoke("PTZExtendedManualSession", new object[2] { token, deviceId });
	}

	public IAsyncResult BeginPTZExtendedManualSession(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PTZExtendedManualSession", new object[2] { token, deviceId }, callback, asyncState);
	}

	public void EndPTZExtendedManualSession(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void PTZExtendedManualSessionAsync(string token, Guid deviceId)
	{
		PTZExtendedManualSessionAsync(token, deviceId, null);
	}

	public void PTZExtendedManualSessionAsync(string token, Guid deviceId, object userState)
	{
		if (PTZExtendedManualSessionOperationCompleted == null)
		{
			PTZExtendedManualSessionOperationCompleted = OnPTZExtendedManualSessionOperationCompleted;
		}
		InvokeAsync("PTZExtendedManualSession", new object[2] { token, deviceId }, PTZExtendedManualSessionOperationCompleted, userState);
	}

	private void OnPTZExtendedManualSessionOperationCompleted(object arg)
	{
		if (this.PTZExtendedManualSessionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PTZExtendedManualSessionCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PTZGetSessionInfo", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public PtzSessionInfo PTZGetSessionInfo(string token, Guid deviceId)
	{
		return (PtzSessionInfo)Invoke("PTZGetSessionInfo", new object[2] { token, deviceId })[0];
	}

	public IAsyncResult BeginPTZGetSessionInfo(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PTZGetSessionInfo", new object[2] { token, deviceId }, callback, asyncState);
	}

	public PtzSessionInfo EndPTZGetSessionInfo(IAsyncResult asyncResult)
	{
		return (PtzSessionInfo)EndInvoke(asyncResult)[0];
	}

	public void PTZGetSessionInfoAsync(string token, Guid deviceId)
	{
		PTZGetSessionInfoAsync(token, deviceId, null);
	}

	public void PTZGetSessionInfoAsync(string token, Guid deviceId, object userState)
	{
		if (PTZGetSessionInfoOperationCompleted == null)
		{
			PTZGetSessionInfoOperationCompleted = OnPTZGetSessionInfoOperationCompleted;
		}
		InvokeAsync("PTZGetSessionInfo", new object[2] { token, deviceId }, PTZGetSessionInfoOperationCompleted, userState);
	}

	private void OnPTZGetSessionInfoOperationCompleted(object arg)
	{
		if (this.PTZGetSessionInfoCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PTZGetSessionInfoCompleted(this, new PTZGetSessionInfoCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PTZReserveSession", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void PTZReserveSession(string token, Guid deviceId)
	{
		Invoke("PTZReserveSession", new object[2] { token, deviceId });
	}

	public IAsyncResult BeginPTZReserveSession(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PTZReserveSession", new object[2] { token, deviceId }, callback, asyncState);
	}

	public void EndPTZReserveSession(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void PTZReserveSessionAsync(string token, Guid deviceId)
	{
		PTZReserveSessionAsync(token, deviceId, null);
	}

	public void PTZReserveSessionAsync(string token, Guid deviceId, object userState)
	{
		if (PTZReserveSessionOperationCompleted == null)
		{
			PTZReserveSessionOperationCompleted = OnPTZReserveSessionOperationCompleted;
		}
		InvokeAsync("PTZReserveSession", new object[2] { token, deviceId }, PTZReserveSessionOperationCompleted, userState);
	}

	private void OnPTZReserveSessionOperationCompleted(object arg)
	{
		if (this.PTZReserveSessionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PTZReserveSessionCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PTZReleaseSession", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void PTZReleaseSession(string token, Guid deviceId)
	{
		Invoke("PTZReleaseSession", new object[2] { token, deviceId });
	}

	public IAsyncResult BeginPTZReleaseSession(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PTZReleaseSession", new object[2] { token, deviceId }, callback, asyncState);
	}

	public void EndPTZReleaseSession(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void PTZReleaseSessionAsync(string token, Guid deviceId)
	{
		PTZReleaseSessionAsync(token, deviceId, null);
	}

	public void PTZReleaseSessionAsync(string token, Guid deviceId, object userState)
	{
		if (PTZReleaseSessionOperationCompleted == null)
		{
			PTZReleaseSessionOperationCompleted = OnPTZReleaseSessionOperationCompleted;
		}
		InvokeAsync("PTZReleaseSession", new object[2] { token, deviceId }, PTZReleaseSessionOperationCompleted, userState);
	}

	private void OnPTZReleaseSessionOperationCompleted(object arg)
	{
		if (this.PTZReleaseSessionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PTZReleaseSessionCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PTZActivatePatrollingSchema", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void PTZActivatePatrollingSchema(string token, Guid deviceId, Guid patrollingSchemaId)
	{
		Invoke("PTZActivatePatrollingSchema", new object[3] { token, deviceId, patrollingSchemaId });
	}

	public IAsyncResult BeginPTZActivatePatrollingSchema(string token, Guid deviceId, Guid patrollingSchemaId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PTZActivatePatrollingSchema", new object[3] { token, deviceId, patrollingSchemaId }, callback, asyncState);
	}

	public void EndPTZActivatePatrollingSchema(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void PTZActivatePatrollingSchemaAsync(string token, Guid deviceId, Guid patrollingSchemaId)
	{
		PTZActivatePatrollingSchemaAsync(token, deviceId, patrollingSchemaId, null);
	}

	public void PTZActivatePatrollingSchemaAsync(string token, Guid deviceId, Guid patrollingSchemaId, object userState)
	{
		if (PTZActivatePatrollingSchemaOperationCompleted == null)
		{
			PTZActivatePatrollingSchemaOperationCompleted = OnPTZActivatePatrollingSchemaOperationCompleted;
		}
		InvokeAsync("PTZActivatePatrollingSchema", new object[3] { token, deviceId, patrollingSchemaId }, PTZActivatePatrollingSchemaOperationCompleted, userState);
	}

	private void OnPTZActivatePatrollingSchemaOperationCompleted(object arg)
	{
		if (this.PTZActivatePatrollingSchemaCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PTZActivatePatrollingSchemaCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PTZDeactivatePatrollingSchema", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void PTZDeactivatePatrollingSchema(string token, Guid deviceId, Guid patrollingSchemaId)
	{
		Invoke("PTZDeactivatePatrollingSchema", new object[3] { token, deviceId, patrollingSchemaId });
	}

	public IAsyncResult BeginPTZDeactivatePatrollingSchema(string token, Guid deviceId, Guid patrollingSchemaId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PTZDeactivatePatrollingSchema", new object[3] { token, deviceId, patrollingSchemaId }, callback, asyncState);
	}

	public void EndPTZDeactivatePatrollingSchema(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void PTZDeactivatePatrollingSchemaAsync(string token, Guid deviceId, Guid patrollingSchemaId)
	{
		PTZDeactivatePatrollingSchemaAsync(token, deviceId, patrollingSchemaId, null);
	}

	public void PTZDeactivatePatrollingSchemaAsync(string token, Guid deviceId, Guid patrollingSchemaId, object userState)
	{
		if (PTZDeactivatePatrollingSchemaOperationCompleted == null)
		{
			PTZDeactivatePatrollingSchemaOperationCompleted = OnPTZDeactivatePatrollingSchemaOperationCompleted;
		}
		InvokeAsync("PTZDeactivatePatrollingSchema", new object[3] { token, deviceId, patrollingSchemaId }, PTZDeactivatePatrollingSchemaOperationCompleted, userState);
	}

	private void OnPTZDeactivatePatrollingSchemaOperationCompleted(object arg)
	{
		if (this.PTZDeactivatePatrollingSchemaCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PTZDeactivatePatrollingSchemaCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PresetGetList", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string[] PresetGetList(string token, Guid deviceId)
	{
		return (string[])Invoke("PresetGetList", new object[2] { token, deviceId })[0];
	}

	public IAsyncResult BeginPresetGetList(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PresetGetList", new object[2] { token, deviceId }, callback, asyncState);
	}

	public string[] EndPresetGetList(IAsyncResult asyncResult)
	{
		return (string[])EndInvoke(asyncResult)[0];
	}

	public void PresetGetListAsync(string token, Guid deviceId)
	{
		PresetGetListAsync(token, deviceId, null);
	}

	public void PresetGetListAsync(string token, Guid deviceId, object userState)
	{
		if (PresetGetListOperationCompleted == null)
		{
			PresetGetListOperationCompleted = OnPresetGetListOperationCompleted;
		}
		InvokeAsync("PresetGetList", new object[2] { token, deviceId }, PresetGetListOperationCompleted, userState);
	}

	private void OnPresetGetListOperationCompleted(object arg)
	{
		if (this.PresetGetListCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PresetGetListCompleted(this, new PresetGetListCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PresetMoveTo", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void PresetMoveTo(string token, Guid deviceId, string presetName)
	{
		Invoke("PresetMoveTo", new object[3] { token, deviceId, presetName });
	}

	public IAsyncResult BeginPresetMoveTo(string token, Guid deviceId, string presetName, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PresetMoveTo", new object[3] { token, deviceId, presetName }, callback, asyncState);
	}

	public void EndPresetMoveTo(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void PresetMoveToAsync(string token, Guid deviceId, string presetName)
	{
		PresetMoveToAsync(token, deviceId, presetName, null);
	}

	public void PresetMoveToAsync(string token, Guid deviceId, string presetName, object userState)
	{
		if (PresetMoveToOperationCompleted == null)
		{
			PresetMoveToOperationCompleted = OnPresetMoveToOperationCompleted;
		}
		InvokeAsync("PresetMoveTo", new object[3] { token, deviceId, presetName }, PresetMoveToOperationCompleted, userState);
	}

	private void OnPresetMoveToOperationCompleted(object arg)
	{
		if (this.PresetMoveToCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PresetMoveToCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PresetSet", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void PresetSet(string token, Guid deviceId, string presetName)
	{
		Invoke("PresetSet", new object[3] { token, deviceId, presetName });
	}

	public IAsyncResult BeginPresetSet(string token, Guid deviceId, string presetName, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PresetSet", new object[3] { token, deviceId, presetName }, callback, asyncState);
	}

	public void EndPresetSet(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void PresetSetAsync(string token, Guid deviceId, string presetName)
	{
		PresetSetAsync(token, deviceId, presetName, null);
	}

	public void PresetSetAsync(string token, Guid deviceId, string presetName, object userState)
	{
		if (PresetSetOperationCompleted == null)
		{
			PresetSetOperationCompleted = OnPresetSetOperationCompleted;
		}
		InvokeAsync("PresetSet", new object[3] { token, deviceId, presetName }, PresetSetOperationCompleted, userState);
	}

	private void OnPresetSetOperationCompleted(object arg)
	{
		if (this.PresetSetCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.PresetSetCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/OutputActivate", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void OutputActivate(string token, Guid deviceId)
	{
		Invoke("OutputActivate", new object[2] { token, deviceId });
	}

	public IAsyncResult BeginOutputActivate(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("OutputActivate", new object[2] { token, deviceId }, callback, asyncState);
	}

	public void EndOutputActivate(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void OutputActivateAsync(string token, Guid deviceId)
	{
		OutputActivateAsync(token, deviceId, null);
	}

	public void OutputActivateAsync(string token, Guid deviceId, object userState)
	{
		if (OutputActivateOperationCompleted == null)
		{
			OutputActivateOperationCompleted = OnOutputActivateOperationCompleted;
		}
		InvokeAsync("OutputActivate", new object[2] { token, deviceId }, OutputActivateOperationCompleted, userState);
	}

	private void OnOutputActivateOperationCompleted(object arg)
	{
		if (this.OutputActivateCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.OutputActivateCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/OutputDeactivate", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void OutputDeactivate(string token, Guid deviceId)
	{
		Invoke("OutputDeactivate", new object[2] { token, deviceId });
	}

	public IAsyncResult BeginOutputDeactivate(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("OutputDeactivate", new object[2] { token, deviceId }, callback, asyncState);
	}

	public void EndOutputDeactivate(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void OutputDeactivateAsync(string token, Guid deviceId)
	{
		OutputDeactivateAsync(token, deviceId, null);
	}

	public void OutputDeactivateAsync(string token, Guid deviceId, object userState)
	{
		if (OutputDeactivateOperationCompleted == null)
		{
			OutputDeactivateOperationCompleted = OnOutputDeactivateOperationCompleted;
		}
		InvokeAsync("OutputDeactivate", new object[2] { token, deviceId }, OutputDeactivateOperationCompleted, userState);
	}

	private void OnOutputDeactivateOperationCompleted(object arg)
	{
		if (this.OutputDeactivateCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.OutputDeactivateCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/OutputTrigger", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void OutputTrigger(string token, Guid deviceId, TimeDuration duration)
	{
		Invoke("OutputTrigger", new object[3] { token, deviceId, duration });
	}

	public IAsyncResult BeginOutputTrigger(string token, Guid deviceId, TimeDuration duration, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("OutputTrigger", new object[3] { token, deviceId, duration }, callback, asyncState);
	}

	public void EndOutputTrigger(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void OutputTriggerAsync(string token, Guid deviceId, TimeDuration duration)
	{
		OutputTriggerAsync(token, deviceId, duration, null);
	}

	public void OutputTriggerAsync(string token, Guid deviceId, TimeDuration duration, object userState)
	{
		if (OutputTriggerOperationCompleted == null)
		{
			OutputTriggerOperationCompleted = OnOutputTriggerOperationCompleted;
		}
		InvokeAsync("OutputTrigger", new object[3] { token, deviceId, duration }, OutputTriggerOperationCompleted, userState);
	}

	private void OnOutputTriggerOperationCompleted(object arg)
	{
		if (this.OutputTriggerCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.OutputTriggerCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/OutputGetState", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public IOState OutputGetState(string token, Guid deviceId)
	{
		return (IOState)Invoke("OutputGetState", new object[2] { token, deviceId })[0];
	}

	public IAsyncResult BeginOutputGetState(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("OutputGetState", new object[2] { token, deviceId }, callback, asyncState);
	}

	public IOState EndOutputGetState(IAsyncResult asyncResult)
	{
		return (IOState)EndInvoke(asyncResult)[0];
	}

	public void OutputGetStateAsync(string token, Guid deviceId)
	{
		OutputGetStateAsync(token, deviceId, null);
	}

	public void OutputGetStateAsync(string token, Guid deviceId, object userState)
	{
		if (OutputGetStateOperationCompleted == null)
		{
			OutputGetStateOperationCompleted = OnOutputGetStateOperationCompleted;
		}
		InvokeAsync("OutputGetState", new object[2] { token, deviceId }, OutputGetStateOperationCompleted, userState);
	}

	private void OnOutputGetStateOperationCompleted(object arg)
	{
		if (this.OutputGetStateCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.OutputGetStateCompleted(this, new OutputGetStateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/SequencesGetTypes", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public SequenceType[] SequencesGetTypes(string token, Guid deviceId)
	{
		return (SequenceType[])Invoke("SequencesGetTypes", new object[2] { token, deviceId })[0];
	}

	public IAsyncResult BeginSequencesGetTypes(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SequencesGetTypes", new object[2] { token, deviceId }, callback, asyncState);
	}

	public SequenceType[] EndSequencesGetTypes(IAsyncResult asyncResult)
	{
		return (SequenceType[])EndInvoke(asyncResult)[0];
	}

	public void SequencesGetTypesAsync(string token, Guid deviceId)
	{
		SequencesGetTypesAsync(token, deviceId, null);
	}

	public void SequencesGetTypesAsync(string token, Guid deviceId, object userState)
	{
		if (SequencesGetTypesOperationCompleted == null)
		{
			SequencesGetTypesOperationCompleted = OnSequencesGetTypesOperationCompleted;
		}
		InvokeAsync("SequencesGetTypes", new object[2] { token, deviceId }, SequencesGetTypesOperationCompleted, userState);
	}

	private void OnSequencesGetTypesOperationCompleted(object arg)
	{
		if (this.SequencesGetTypesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SequencesGetTypesCompleted(this, new SequencesGetTypesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/SequencesGet", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public SequenceEntry[] SequencesGet(string token, Guid deviceId, Guid sequenceType, DateTime minTime, DateTime maxTime, int maxCount)
	{
		return (SequenceEntry[])Invoke("SequencesGet", new object[6] { token, deviceId, sequenceType, minTime, maxTime, maxCount })[0];
	}

	public IAsyncResult BeginSequencesGet(string token, Guid deviceId, Guid sequenceType, DateTime minTime, DateTime maxTime, int maxCount, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SequencesGet", new object[6] { token, deviceId, sequenceType, minTime, maxTime, maxCount }, callback, asyncState);
	}

	public SequenceEntry[] EndSequencesGet(IAsyncResult asyncResult)
	{
		return (SequenceEntry[])EndInvoke(asyncResult)[0];
	}

	public void SequencesGetAsync(string token, Guid deviceId, Guid sequenceType, DateTime minTime, DateTime maxTime, int maxCount)
	{
		SequencesGetAsync(token, deviceId, sequenceType, minTime, maxTime, maxCount, null);
	}

	public void SequencesGetAsync(string token, Guid deviceId, Guid sequenceType, DateTime minTime, DateTime maxTime, int maxCount, object userState)
	{
		if (SequencesGetOperationCompleted == null)
		{
			SequencesGetOperationCompleted = OnSequencesGetOperationCompleted;
		}
		InvokeAsync("SequencesGet", new object[6] { token, deviceId, sequenceType, minTime, maxTime, maxCount }, SequencesGetOperationCompleted, userState);
	}

	private void OnSequencesGetOperationCompleted(object arg)
	{
		if (this.SequencesGetCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SequencesGetCompleted(this, new SequencesGetCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/SequencesGetAround", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public SequenceEntry[] SequencesGetAround(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, int maxCountBefore, int maxCountAfter)
	{
		return (SequenceEntry[])Invoke("SequencesGetAround", new object[6] { token, deviceId, sequenceType, centerTime, maxCountBefore, maxCountAfter })[0];
	}

	public IAsyncResult BeginSequencesGetAround(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, int maxCountBefore, int maxCountAfter, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SequencesGetAround", new object[6] { token, deviceId, sequenceType, centerTime, maxCountBefore, maxCountAfter }, callback, asyncState);
	}

	public SequenceEntry[] EndSequencesGetAround(IAsyncResult asyncResult)
	{
		return (SequenceEntry[])EndInvoke(asyncResult)[0];
	}

	public void SequencesGetAroundAsync(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, int maxCountBefore, int maxCountAfter)
	{
		SequencesGetAroundAsync(token, deviceId, sequenceType, centerTime, maxCountBefore, maxCountAfter, null);
	}

	public void SequencesGetAroundAsync(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, int maxCountBefore, int maxCountAfter, object userState)
	{
		if (SequencesGetAroundOperationCompleted == null)
		{
			SequencesGetAroundOperationCompleted = OnSequencesGetAroundOperationCompleted;
		}
		InvokeAsync("SequencesGetAround", new object[6] { token, deviceId, sequenceType, centerTime, maxCountBefore, maxCountAfter }, SequencesGetAroundOperationCompleted, userState);
	}

	private void OnSequencesGetAroundOperationCompleted(object arg)
	{
		if (this.SequencesGetAroundCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SequencesGetAroundCompleted(this, new SequencesGetAroundCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/SequencesGetAroundWithSpan", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public SequenceEntry[] SequencesGetAroundWithSpan(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter)
	{
		return (SequenceEntry[])Invoke("SequencesGetAroundWithSpan", new object[8] { token, deviceId, sequenceType, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter })[0];
	}

	public IAsyncResult BeginSequencesGetAroundWithSpan(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SequencesGetAroundWithSpan", new object[8] { token, deviceId, sequenceType, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter }, callback, asyncState);
	}

	public SequenceEntry[] EndSequencesGetAroundWithSpan(IAsyncResult asyncResult)
	{
		return (SequenceEntry[])EndInvoke(asyncResult)[0];
	}

	public void SequencesGetAroundWithSpanAsync(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter)
	{
		SequencesGetAroundWithSpanAsync(token, deviceId, sequenceType, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, null);
	}

	public void SequencesGetAroundWithSpanAsync(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter, object userState)
	{
		if (SequencesGetAroundWithSpanOperationCompleted == null)
		{
			SequencesGetAroundWithSpanOperationCompleted = OnSequencesGetAroundWithSpanOperationCompleted;
		}
		InvokeAsync("SequencesGetAroundWithSpan", new object[8] { token, deviceId, sequenceType, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter }, SequencesGetAroundWithSpanOperationCompleted, userState);
	}

	private void OnSequencesGetAroundWithSpanOperationCompleted(object arg)
	{
		if (this.SequencesGetAroundWithSpanCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SequencesGetAroundWithSpanCompleted(this, new SequencesGetAroundWithSpanCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/GetRetentionTimeLimit", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DateTime GetRetentionTimeLimit(string token, Guid deviceId)
	{
		return (DateTime)Invoke("GetRetentionTimeLimit", new object[2] { token, deviceId })[0];
	}

	public IAsyncResult BeginGetRetentionTimeLimit(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetRetentionTimeLimit", new object[2] { token, deviceId }, callback, asyncState);
	}

	public DateTime EndGetRetentionTimeLimit(IAsyncResult asyncResult)
	{
		return (DateTime)EndInvoke(asyncResult)[0];
	}

	public void GetRetentionTimeLimitAsync(string token, Guid deviceId)
	{
		GetRetentionTimeLimitAsync(token, deviceId, null);
	}

	public void GetRetentionTimeLimitAsync(string token, Guid deviceId, object userState)
	{
		if (GetRetentionTimeLimitOperationCompleted == null)
		{
			GetRetentionTimeLimitOperationCompleted = OnGetRetentionTimeLimitOperationCompleted;
		}
		InvokeAsync("GetRetentionTimeLimit", new object[2] { token, deviceId }, GetRetentionTimeLimitOperationCompleted, userState);
	}

	private void OnGetRetentionTimeLimitOperationCompleted(object arg)
	{
		if (this.GetRetentionTimeLimitCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetRetentionTimeLimitCompleted(this, new GetRetentionTimeLimitCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/TimeLineInformationGet", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public TimeLineInformationData[] TimeLineInformationGet(string token, Guid deviceId, Guid[] timeLineInformationTypes, DateTime timeLineInformationBeginTime, TimeDuration timeLineInformationInterval, int timeLineInformationCount)
	{
		return (TimeLineInformationData[])Invoke("TimeLineInformationGet", new object[6] { token, deviceId, timeLineInformationTypes, timeLineInformationBeginTime, timeLineInformationInterval, timeLineInformationCount })[0];
	}

	public IAsyncResult BeginTimeLineInformationGet(string token, Guid deviceId, Guid[] timeLineInformationTypes, DateTime timeLineInformationBeginTime, TimeDuration timeLineInformationInterval, int timeLineInformationCount, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("TimeLineInformationGet", new object[6] { token, deviceId, timeLineInformationTypes, timeLineInformationBeginTime, timeLineInformationInterval, timeLineInformationCount }, callback, asyncState);
	}

	public TimeLineInformationData[] EndTimeLineInformationGet(IAsyncResult asyncResult)
	{
		return (TimeLineInformationData[])EndInvoke(asyncResult)[0];
	}

	public void TimeLineInformationGetAsync(string token, Guid deviceId, Guid[] timeLineInformationTypes, DateTime timeLineInformationBeginTime, TimeDuration timeLineInformationInterval, int timeLineInformationCount)
	{
		TimeLineInformationGetAsync(token, deviceId, timeLineInformationTypes, timeLineInformationBeginTime, timeLineInformationInterval, timeLineInformationCount, null);
	}

	public void TimeLineInformationGetAsync(string token, Guid deviceId, Guid[] timeLineInformationTypes, DateTime timeLineInformationBeginTime, TimeDuration timeLineInformationInterval, int timeLineInformationCount, object userState)
	{
		if (TimeLineInformationGetOperationCompleted == null)
		{
			TimeLineInformationGetOperationCompleted = OnTimeLineInformationGetOperationCompleted;
		}
		InvokeAsync("TimeLineInformationGet", new object[6] { token, deviceId, timeLineInformationTypes, timeLineInformationBeginTime, timeLineInformationInterval, timeLineInformationCount }, TimeLineInformationGetOperationCompleted, userState);
	}

	private void OnTimeLineInformationGetOperationCompleted(object arg)
	{
		if (this.TimeLineInformationGetCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.TimeLineInformationGetCompleted(this, new TimeLineInformationGetCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/SmartSearchStart", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public Guid SmartSearchStart(string token, Guid deviceId, DateTime searchBeginTime, DateTime searchEndTime, int sensitivity, TimeDuration searchInterval, ImageMask searchMask, bool ignoreInitialMotion, Size resultMotionMaskSize)
	{
		return (Guid)Invoke("SmartSearchStart", new object[9] { token, deviceId, searchBeginTime, searchEndTime, sensitivity, searchInterval, searchMask, ignoreInitialMotion, resultMotionMaskSize })[0];
	}

	public IAsyncResult BeginSmartSearchStart(string token, Guid deviceId, DateTime searchBeginTime, DateTime searchEndTime, int sensitivity, TimeDuration searchInterval, ImageMask searchMask, bool ignoreInitialMotion, Size resultMotionMaskSize, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SmartSearchStart", new object[9] { token, deviceId, searchBeginTime, searchEndTime, sensitivity, searchInterval, searchMask, ignoreInitialMotion, resultMotionMaskSize }, callback, asyncState);
	}

	public Guid EndSmartSearchStart(IAsyncResult asyncResult)
	{
		return (Guid)EndInvoke(asyncResult)[0];
	}

	public void SmartSearchStartAsync(string token, Guid deviceId, DateTime searchBeginTime, DateTime searchEndTime, int sensitivity, TimeDuration searchInterval, ImageMask searchMask, bool ignoreInitialMotion, Size resultMotionMaskSize)
	{
		SmartSearchStartAsync(token, deviceId, searchBeginTime, searchEndTime, sensitivity, searchInterval, searchMask, ignoreInitialMotion, resultMotionMaskSize, null);
	}

	public void SmartSearchStartAsync(string token, Guid deviceId, DateTime searchBeginTime, DateTime searchEndTime, int sensitivity, TimeDuration searchInterval, ImageMask searchMask, bool ignoreInitialMotion, Size resultMotionMaskSize, object userState)
	{
		if (SmartSearchStartOperationCompleted == null)
		{
			SmartSearchStartOperationCompleted = OnSmartSearchStartOperationCompleted;
		}
		InvokeAsync("SmartSearchStart", new object[9] { token, deviceId, searchBeginTime, searchEndTime, sensitivity, searchInterval, searchMask, ignoreInitialMotion, resultMotionMaskSize }, SmartSearchStartOperationCompleted, userState);
	}

	private void OnSmartSearchStartOperationCompleted(object arg)
	{
		if (this.SmartSearchStartCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SmartSearchStartCompleted(this, new SmartSearchStartCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/SmartSearchCancel", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void SmartSearchCancel(string token, Guid smartSearchId)
	{
		Invoke("SmartSearchCancel", new object[2] { token, smartSearchId });
	}

	public IAsyncResult BeginSmartSearchCancel(string token, Guid smartSearchId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SmartSearchCancel", new object[2] { token, smartSearchId }, callback, asyncState);
	}

	public void EndSmartSearchCancel(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void SmartSearchCancelAsync(string token, Guid smartSearchId)
	{
		SmartSearchCancelAsync(token, smartSearchId, null);
	}

	public void SmartSearchCancelAsync(string token, Guid smartSearchId, object userState)
	{
		if (SmartSearchCancelOperationCompleted == null)
		{
			SmartSearchCancelOperationCompleted = OnSmartSearchCancelOperationCompleted;
		}
		InvokeAsync("SmartSearchCancel", new object[2] { token, smartSearchId }, SmartSearchCancelOperationCompleted, userState);
	}

	private void OnSmartSearchCancelOperationCompleted(object arg)
	{
		if (this.SmartSearchCancelCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SmartSearchCancelCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/SmartSearchGetStatus", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public SmartSearchStatus SmartSearchGetStatus(string token, Guid smartSearchId)
	{
		return (SmartSearchStatus)Invoke("SmartSearchGetStatus", new object[2] { token, smartSearchId })[0];
	}

	public IAsyncResult BeginSmartSearchGetStatus(string token, Guid smartSearchId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SmartSearchGetStatus", new object[2] { token, smartSearchId }, callback, asyncState);
	}

	public SmartSearchStatus EndSmartSearchGetStatus(IAsyncResult asyncResult)
	{
		return (SmartSearchStatus)EndInvoke(asyncResult)[0];
	}

	public void SmartSearchGetStatusAsync(string token, Guid smartSearchId)
	{
		SmartSearchGetStatusAsync(token, smartSearchId, null);
	}

	public void SmartSearchGetStatusAsync(string token, Guid smartSearchId, object userState)
	{
		if (SmartSearchGetStatusOperationCompleted == null)
		{
			SmartSearchGetStatusOperationCompleted = OnSmartSearchGetStatusOperationCompleted;
		}
		InvokeAsync("SmartSearchGetStatus", new object[2] { token, smartSearchId }, SmartSearchGetStatusOperationCompleted, userState);
	}

	private void OnSmartSearchGetStatusOperationCompleted(object arg)
	{
		if (this.SmartSearchGetStatusCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SmartSearchGetStatusCompleted(this, new SmartSearchGetStatusCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/SmartSearchGetResult", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public SmartSearchResult SmartSearchGetResult(string token, Guid smartSearchId, bool continueSearch)
	{
		return (SmartSearchResult)Invoke("SmartSearchGetResult", new object[3] { token, smartSearchId, continueSearch })[0];
	}

	public IAsyncResult BeginSmartSearchGetResult(string token, Guid smartSearchId, bool continueSearch, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SmartSearchGetResult", new object[3] { token, smartSearchId, continueSearch }, callback, asyncState);
	}

	public SmartSearchResult EndSmartSearchGetResult(IAsyncResult asyncResult)
	{
		return (SmartSearchResult)EndInvoke(asyncResult)[0];
	}

	public void SmartSearchGetResultAsync(string token, Guid smartSearchId, bool continueSearch)
	{
		SmartSearchGetResultAsync(token, smartSearchId, continueSearch, null);
	}

	public void SmartSearchGetResultAsync(string token, Guid smartSearchId, bool continueSearch, object userState)
	{
		if (SmartSearchGetResultOperationCompleted == null)
		{
			SmartSearchGetResultOperationCompleted = OnSmartSearchGetResultOperationCompleted;
		}
		InvokeAsync("SmartSearchGetResult", new object[3] { token, smartSearchId, continueSearch }, SmartSearchGetResultOperationCompleted, userState);
	}

	private void OnSmartSearchGetResultOperationCompleted(object arg)
	{
		if (this.SmartSearchGetResultCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SmartSearchGetResultCompleted(this, new SmartSearchGetResultCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/JPEGGetAt", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public JPEGData JPEGGetAt(string token, Guid deviceId, DateTime time)
	{
		return (JPEGData)Invoke("JPEGGetAt", new object[3] { token, deviceId, time })[0];
	}

	public IAsyncResult BeginJPEGGetAt(string token, Guid deviceId, DateTime time, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("JPEGGetAt", new object[3] { token, deviceId, time }, callback, asyncState);
	}

	public JPEGData EndJPEGGetAt(IAsyncResult asyncResult)
	{
		return (JPEGData)EndInvoke(asyncResult)[0];
	}

	public void JPEGGetAtAsync(string token, Guid deviceId, DateTime time)
	{
		JPEGGetAtAsync(token, deviceId, time, null);
	}

	public void JPEGGetAtAsync(string token, Guid deviceId, DateTime time, object userState)
	{
		if (JPEGGetAtOperationCompleted == null)
		{
			JPEGGetAtOperationCompleted = OnJPEGGetAtOperationCompleted;
		}
		InvokeAsync("JPEGGetAt", new object[3] { token, deviceId, time }, JPEGGetAtOperationCompleted, userState);
	}

	private void OnJPEGGetAtOperationCompleted(object arg)
	{
		if (this.JPEGGetAtCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.JPEGGetAtCompleted(this, new JPEGGetAtCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/JPEGGetAtOrBefore", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public JPEGData JPEGGetAtOrBefore(string token, Guid deviceId, DateTime time)
	{
		return (JPEGData)Invoke("JPEGGetAtOrBefore", new object[3] { token, deviceId, time })[0];
	}

	public IAsyncResult BeginJPEGGetAtOrBefore(string token, Guid deviceId, DateTime time, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("JPEGGetAtOrBefore", new object[3] { token, deviceId, time }, callback, asyncState);
	}

	public JPEGData EndJPEGGetAtOrBefore(IAsyncResult asyncResult)
	{
		return (JPEGData)EndInvoke(asyncResult)[0];
	}

	public void JPEGGetAtOrBeforeAsync(string token, Guid deviceId, DateTime time)
	{
		JPEGGetAtOrBeforeAsync(token, deviceId, time, null);
	}

	public void JPEGGetAtOrBeforeAsync(string token, Guid deviceId, DateTime time, object userState)
	{
		if (JPEGGetAtOrBeforeOperationCompleted == null)
		{
			JPEGGetAtOrBeforeOperationCompleted = OnJPEGGetAtOrBeforeOperationCompleted;
		}
		InvokeAsync("JPEGGetAtOrBefore", new object[3] { token, deviceId, time }, JPEGGetAtOrBeforeOperationCompleted, userState);
	}

	private void OnJPEGGetAtOrBeforeOperationCompleted(object arg)
	{
		if (this.JPEGGetAtOrBeforeCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.JPEGGetAtOrBeforeCompleted(this, new JPEGGetAtOrBeforeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/JPEGGetAtOrAfter", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public JPEGData JPEGGetAtOrAfter(string token, Guid deviceId, DateTime time)
	{
		return (JPEGData)Invoke("JPEGGetAtOrAfter", new object[3] { token, deviceId, time })[0];
	}

	public IAsyncResult BeginJPEGGetAtOrAfter(string token, Guid deviceId, DateTime time, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("JPEGGetAtOrAfter", new object[3] { token, deviceId, time }, callback, asyncState);
	}

	public JPEGData EndJPEGGetAtOrAfter(IAsyncResult asyncResult)
	{
		return (JPEGData)EndInvoke(asyncResult)[0];
	}

	public void JPEGGetAtOrAfterAsync(string token, Guid deviceId, DateTime time)
	{
		JPEGGetAtOrAfterAsync(token, deviceId, time, null);
	}

	public void JPEGGetAtOrAfterAsync(string token, Guid deviceId, DateTime time, object userState)
	{
		if (JPEGGetAtOrAfterOperationCompleted == null)
		{
			JPEGGetAtOrAfterOperationCompleted = OnJPEGGetAtOrAfterOperationCompleted;
		}
		InvokeAsync("JPEGGetAtOrAfter", new object[3] { token, deviceId, time }, JPEGGetAtOrAfterOperationCompleted, userState);
	}

	private void OnJPEGGetAtOrAfterOperationCompleted(object arg)
	{
		if (this.JPEGGetAtOrAfterCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.JPEGGetAtOrAfterCompleted(this, new JPEGGetAtOrAfterCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/JPEGGetLive", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public JPEGData JPEGGetLive(string token, Guid deviceId, int maxWidth, int maxHeight)
	{
		return (JPEGData)Invoke("JPEGGetLive", new object[4] { token, deviceId, maxWidth, maxHeight })[0];
	}

	public IAsyncResult BeginJPEGGetLive(string token, Guid deviceId, int maxWidth, int maxHeight, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("JPEGGetLive", new object[4] { token, deviceId, maxWidth, maxHeight }, callback, asyncState);
	}

	public JPEGData EndJPEGGetLive(IAsyncResult asyncResult)
	{
		return (JPEGData)EndInvoke(asyncResult)[0];
	}

	public void JPEGGetLiveAsync(string token, Guid deviceId, int maxWidth, int maxHeight)
	{
		JPEGGetLiveAsync(token, deviceId, maxWidth, maxHeight, null);
	}

	public void JPEGGetLiveAsync(string token, Guid deviceId, int maxWidth, int maxHeight, object userState)
	{
		if (JPEGGetLiveOperationCompleted == null)
		{
			JPEGGetLiveOperationCompleted = OnJPEGGetLiveOperationCompleted;
		}
		InvokeAsync("JPEGGetLive", new object[4] { token, deviceId, maxWidth, maxHeight }, JPEGGetLiveOperationCompleted, userState);
	}

	private void OnJPEGGetLiveOperationCompleted(object arg)
	{
		if (this.JPEGGetLiveCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.JPEGGetLiveCompleted(this, new JPEGGetLiveCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/InputGetState", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public IOState InputGetState(string token, Guid deviceId)
	{
		return (IOState)Invoke("InputGetState", new object[2] { token, deviceId })[0];
	}

	public IAsyncResult BeginInputGetState(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("InputGetState", new object[2] { token, deviceId }, callback, asyncState);
	}

	public IOState EndInputGetState(IAsyncResult asyncResult)
	{
		return (IOState)EndInvoke(asyncResult)[0];
	}

	public void InputGetStateAsync(string token, Guid deviceId)
	{
		InputGetStateAsync(token, deviceId, null);
	}

	public void InputGetStateAsync(string token, Guid deviceId, object userState)
	{
		if (InputGetStateOperationCompleted == null)
		{
			InputGetStateOperationCompleted = OnInputGetStateOperationCompleted;
		}
		InvokeAsync("InputGetState", new object[2] { token, deviceId }, InputGetStateOperationCompleted, userState);
	}

	private void OnInputGetStateOperationCompleted(object arg)
	{
		if (this.InputGetStateCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.InputGetStateCompleted(this, new InputGetStateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/StartRecording", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public bool StartRecording(string token, Guid deviceId, long recordingTimeMicroSeconds)
	{
		return (bool)Invoke("StartRecording", new object[3] { token, deviceId, recordingTimeMicroSeconds })[0];
	}

	public IAsyncResult BeginStartRecording(string token, Guid deviceId, long recordingTimeMicroSeconds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("StartRecording", new object[3] { token, deviceId, recordingTimeMicroSeconds }, callback, asyncState);
	}

	public bool EndStartRecording(IAsyncResult asyncResult)
	{
		return (bool)EndInvoke(asyncResult)[0];
	}

	public void StartRecordingAsync(string token, Guid deviceId, long recordingTimeMicroSeconds)
	{
		StartRecordingAsync(token, deviceId, recordingTimeMicroSeconds, null);
	}

	public void StartRecordingAsync(string token, Guid deviceId, long recordingTimeMicroSeconds, object userState)
	{
		if (StartRecordingOperationCompleted == null)
		{
			StartRecordingOperationCompleted = OnStartRecordingOperationCompleted;
		}
		InvokeAsync("StartRecording", new object[3] { token, deviceId, recordingTimeMicroSeconds }, StartRecordingOperationCompleted, userState);
	}

	private void OnStartRecordingOperationCompleted(object arg)
	{
		if (this.StartRecordingCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.StartRecordingCompleted(this, new StartRecordingCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/SendDriverCommand", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void SendDriverCommand(string token, Guid deviceId, string command)
	{
		Invoke("SendDriverCommand", new object[3] { token, deviceId, command });
	}

	public IAsyncResult BeginSendDriverCommand(string token, Guid deviceId, string command, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SendDriverCommand", new object[3] { token, deviceId, command }, callback, asyncState);
	}

	public void EndSendDriverCommand(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void SendDriverCommandAsync(string token, Guid deviceId, string command)
	{
		SendDriverCommandAsync(token, deviceId, command, null);
	}

	public void SendDriverCommandAsync(string token, Guid deviceId, string command, object userState)
	{
		if (SendDriverCommandOperationCompleted == null)
		{
			SendDriverCommandOperationCompleted = OnSendDriverCommandOperationCompleted;
		}
		InvokeAsync("SendDriverCommand", new object[3] { token, deviceId, command }, SendDriverCommandOperationCompleted, userState);
	}

	private void OnSendDriverCommandOperationCompleted(object arg)
	{
		if (this.SendDriverCommandCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SendDriverCommandCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/SendDriverCommandWithResponse", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string SendDriverCommandWithResponse(string token, Guid deviceId, string command, string parameter)
	{
		return (string)Invoke("SendDriverCommandWithResponse", new object[4] { token, deviceId, command, parameter })[0];
	}

	public IAsyncResult BeginSendDriverCommandWithResponse(string token, Guid deviceId, string command, string parameter, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SendDriverCommandWithResponse", new object[4] { token, deviceId, command, parameter }, callback, asyncState);
	}

	public string EndSendDriverCommandWithResponse(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void SendDriverCommandWithResponseAsync(string token, Guid deviceId, string command, string parameter)
	{
		SendDriverCommandWithResponseAsync(token, deviceId, command, parameter, null);
	}

	public void SendDriverCommandWithResponseAsync(string token, Guid deviceId, string command, string parameter, object userState)
	{
		if (SendDriverCommandWithResponseOperationCompleted == null)
		{
			SendDriverCommandWithResponseOperationCompleted = OnSendDriverCommandWithResponseOperationCompleted;
		}
		InvokeAsync("SendDriverCommandWithResponse", new object[4] { token, deviceId, command, parameter }, SendDriverCommandWithResponseOperationCompleted, userState);
	}

	private void OnSendDriverCommandWithResponseOperationCompleted(object arg)
	{
		if (this.SendDriverCommandWithResponseCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SendDriverCommandWithResponseCompleted(this, new SendDriverCommandWithResponseCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/MulticastStart", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public MulticastInfo[] MulticastStart(string token, Guid[] deviceIds, ClientCapabilities clientCapabilities)
	{
		return (MulticastInfo[])Invoke("MulticastStart", new object[3] { token, deviceIds, clientCapabilities })[0];
	}

	public IAsyncResult BeginMulticastStart(string token, Guid[] deviceIds, ClientCapabilities clientCapabilities, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("MulticastStart", new object[3] { token, deviceIds, clientCapabilities }, callback, asyncState);
	}

	public MulticastInfo[] EndMulticastStart(IAsyncResult asyncResult)
	{
		return (MulticastInfo[])EndInvoke(asyncResult)[0];
	}

	public void MulticastStartAsync(string token, Guid[] deviceIds, ClientCapabilities clientCapabilities)
	{
		MulticastStartAsync(token, deviceIds, clientCapabilities, null);
	}

	public void MulticastStartAsync(string token, Guid[] deviceIds, ClientCapabilities clientCapabilities, object userState)
	{
		if (MulticastStartOperationCompleted == null)
		{
			MulticastStartOperationCompleted = OnMulticastStartOperationCompleted;
		}
		InvokeAsync("MulticastStart", new object[3] { token, deviceIds, clientCapabilities }, MulticastStartOperationCompleted, userState);
	}

	private void OnMulticastStartOperationCompleted(object arg)
	{
		if (this.MulticastStartCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.MulticastStartCompleted(this, new MulticastStartCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/MulticastContinue", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void MulticastContinue(string token, Guid[] multicastSessionIds)
	{
		Invoke("MulticastContinue", new object[2] { token, multicastSessionIds });
	}

	public IAsyncResult BeginMulticastContinue(string token, Guid[] multicastSessionIds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("MulticastContinue", new object[2] { token, multicastSessionIds }, callback, asyncState);
	}

	public void EndMulticastContinue(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void MulticastContinueAsync(string token, Guid[] multicastSessionIds)
	{
		MulticastContinueAsync(token, multicastSessionIds, null);
	}

	public void MulticastContinueAsync(string token, Guid[] multicastSessionIds, object userState)
	{
		if (MulticastContinueOperationCompleted == null)
		{
			MulticastContinueOperationCompleted = OnMulticastContinueOperationCompleted;
		}
		InvokeAsync("MulticastContinue", new object[2] { token, multicastSessionIds }, MulticastContinueOperationCompleted, userState);
	}

	private void OnMulticastContinueOperationCompleted(object arg)
	{
		if (this.MulticastContinueCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.MulticastContinueCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/MulticastStop", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void MulticastStop(string token, Guid[] multicastSessionIds)
	{
		Invoke("MulticastStop", new object[2] { token, multicastSessionIds });
	}

	public IAsyncResult BeginMulticastStop(string token, Guid[] multicastSessionIds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("MulticastStop", new object[2] { token, multicastSessionIds }, callback, asyncState);
	}

	public void EndMulticastStop(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void MulticastStopAsync(string token, Guid[] multicastSessionIds)
	{
		MulticastStopAsync(token, multicastSessionIds, null);
	}

	public void MulticastStopAsync(string token, Guid[] multicastSessionIds, object userState)
	{
		if (MulticastStopOperationCompleted == null)
		{
			MulticastStopOperationCompleted = OnMulticastStopOperationCompleted;
		}
		InvokeAsync("MulticastStop", new object[2] { token, multicastSessionIds }, MulticastStopOperationCompleted, userState);
	}

	private void OnMulticastStopOperationCompleted(object arg)
	{
		if (this.MulticastStopCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.MulticastStopCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/IsManualRecording", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public ManualRecordingInfo[] IsManualRecording(string token, Guid[] deviceIds)
	{
		return (ManualRecordingInfo[])Invoke("IsManualRecording", new object[2] { token, deviceIds })[0];
	}

	public IAsyncResult BeginIsManualRecording(string token, Guid[] deviceIds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("IsManualRecording", new object[2] { token, deviceIds }, callback, asyncState);
	}

	public ManualRecordingInfo[] EndIsManualRecording(IAsyncResult asyncResult)
	{
		return (ManualRecordingInfo[])EndInvoke(asyncResult)[0];
	}

	public void IsManualRecordingAsync(string token, Guid[] deviceIds)
	{
		IsManualRecordingAsync(token, deviceIds, null);
	}

	public void IsManualRecordingAsync(string token, Guid[] deviceIds, object userState)
	{
		if (IsManualRecordingOperationCompleted == null)
		{
			IsManualRecordingOperationCompleted = OnIsManualRecordingOperationCompleted;
		}
		InvokeAsync("IsManualRecording", new object[2] { token, deviceIds }, IsManualRecordingOperationCompleted, userState);
	}

	private void OnIsManualRecordingOperationCompleted(object arg)
	{
		if (this.IsManualRecordingCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.IsManualRecordingCompleted(this, new IsManualRecordingCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/StartManualRecording", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public ManualRecordingResult[] StartManualRecording(string token, Guid[] deviceIds)
	{
		return (ManualRecordingResult[])Invoke("StartManualRecording", new object[2] { token, deviceIds })[0];
	}

	public IAsyncResult BeginStartManualRecording(string token, Guid[] deviceIds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("StartManualRecording", new object[2] { token, deviceIds }, callback, asyncState);
	}

	public ManualRecordingResult[] EndStartManualRecording(IAsyncResult asyncResult)
	{
		return (ManualRecordingResult[])EndInvoke(asyncResult)[0];
	}

	public void StartManualRecordingAsync(string token, Guid[] deviceIds)
	{
		StartManualRecordingAsync(token, deviceIds, null);
	}

	public void StartManualRecordingAsync(string token, Guid[] deviceIds, object userState)
	{
		if (StartManualRecordingOperationCompleted == null)
		{
			StartManualRecordingOperationCompleted = OnStartManualRecordingOperationCompleted;
		}
		InvokeAsync("StartManualRecording", new object[2] { token, deviceIds }, StartManualRecordingOperationCompleted, userState);
	}

	private void OnStartManualRecordingOperationCompleted(object arg)
	{
		if (this.StartManualRecordingCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.StartManualRecordingCompleted(this, new StartManualRecordingCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/StopManualRecording", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public ManualRecordingResult[] StopManualRecording(string token, Guid[] deviceIds)
	{
		return (ManualRecordingResult[])Invoke("StopManualRecording", new object[2] { token, deviceIds })[0];
	}

	public IAsyncResult BeginStopManualRecording(string token, Guid[] deviceIds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("StopManualRecording", new object[2] { token, deviceIds }, callback, asyncState);
	}

	public ManualRecordingResult[] EndStopManualRecording(IAsyncResult asyncResult)
	{
		return (ManualRecordingResult[])EndInvoke(asyncResult)[0];
	}

	public void StopManualRecordingAsync(string token, Guid[] deviceIds)
	{
		StopManualRecordingAsync(token, deviceIds, null);
	}

	public void StopManualRecordingAsync(string token, Guid[] deviceIds, object userState)
	{
		if (StopManualRecordingOperationCompleted == null)
		{
			StopManualRecordingOperationCompleted = OnStopManualRecordingOperationCompleted;
		}
		InvokeAsync("StopManualRecording", new object[2] { token, deviceIds }, StopManualRecordingOperationCompleted, userState);
	}

	private void OnStopManualRecordingOperationCompleted(object arg)
	{
		if (this.StopManualRecordingCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.StopManualRecordingCompleted(this, new StopManualRecordingCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public new void CancelAsync(object userState)
	{
		base.CancelAsync(userState);
	}

	protected override WebRequest GetWebRequest(Uri uri)
	{
		WebRequest webRequest = base.GetWebRequest(uri);
		if (webRequest is HttpWebRequest httpWebRequest)
		{
			httpWebRequest.ServerCertificateValidationCallback = MyOwnValidationCallback;
		}
		return webRequest;
	}

	private bool MyOwnValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
	{
		return sslpolicyerrors == SslPolicyErrors.None;
	}
}
