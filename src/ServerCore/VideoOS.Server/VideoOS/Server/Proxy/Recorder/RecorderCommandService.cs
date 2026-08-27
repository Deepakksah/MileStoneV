using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace VideoOS.Server.Proxy.Recorder;

[GeneratedCode("wsdl", "2.0.50727.42")]
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

	private SendOrPostCallback TestOperationCompleted;

	private SendOrPostCallback SequencesGetAroundWithSpanOperationCompleted;

	private SendOrPostCallback TimeLineInformationGetOperationCompleted;

	private SendOrPostCallback SmartSearchStartOperationCompleted;

	private SendOrPostCallback SmartSearchCancelOperationCompleted;

	private SendOrPostCallback SmartSearchGetStatusOperationCompleted;

	private SendOrPostCallback SmartSearchGetResultOperationCompleted;

	private SendOrPostCallback ControlLiveStreamOperationCompleted;

	private SendOrPostCallback JPEGGetAtOperationCompleted;

	private SendOrPostCallback JPEGGetAtOrBeforeOperationCompleted;

	private SendOrPostCallback JPEGGetAtOrAfterOperationCompleted;

	private SendOrPostCallback InputGetStateOperationCompleted;

	public event GetVersionCompletedEventHandler GetVersionCompleted;

	public event PTZMoveAbsoluteCompletedEventHandler PTZMoveAbsoluteCompleted;

	public event PTZMoveRelativeCompletedEventHandler PTZMoveRelativeCompleted;

	public event PTZMoveHomeCompletedEventHandler PTZMoveHomeCompleted;

	public event PTZMoveStartCompletedEventHandler PTZMoveStartCompleted;

	public event PTZMoveStopCompletedEventHandler PTZMoveStopCompleted;

	public event PTZCenterCompletedEventHandler PTZCenterCompleted;

	public event PTZRectangleCompletedEventHandler PTZRectangleCompleted;

	public event PTZGetAbsolutePositionCompletedEventHandler PTZGetAbsolutePositionCompleted;

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

	public event TestCompletedEventHandler TestCompleted;

	public event SequencesGetAroundWithSpanCompletedEventHandler SequencesGetAroundWithSpanCompleted;

	public event TimeLineInformationGetCompletedEventHandler TimeLineInformationGetCompleted;

	public event SmartSearchStartCompletedEventHandler SmartSearchStartCompleted;

	public event SmartSearchCancelCompletedEventHandler SmartSearchCancelCompleted;

	public event SmartSearchGetStatusCompletedEventHandler SmartSearchGetStatusCompleted;

	public event SmartSearchGetResultCompletedEventHandler SmartSearchGetResultCompleted;

	public event ControlLiveStreamCompletedEventHandler ControlLiveStreamCompleted;

	public event JPEGGetAtCompletedEventHandler JPEGGetAtCompleted;

	public event JPEGGetAtOrBeforeCompletedEventHandler JPEGGetAtOrBeforeCompleted;

	public event JPEGGetAtOrAfterCompletedEventHandler JPEGGetAtOrAfterCompleted;

	public event InputGetStateCompletedEventHandler InputGetStateCompleted;

	public RecorderCommandService()
	{
		base.Url = "http://localhost/RecorderAPIGenerator/RecorderApi.asmx";
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/GetVersion", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int GetVersion()
	{
		object[] array = Invoke("GetVersion", new object[0]);
		return (int)array[0];
	}

	public IAsyncResult BeginGetVersion(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetVersion", new object[0], callback, asyncState);
	}

	public int EndGetVersion(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (int)array[0];
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
		object[] array = Invoke("PTZGetAbsolutePosition", new object[2] { token, deviceId });
		return (PTZArgs)array[0];
	}

	public IAsyncResult BeginPTZGetAbsolutePosition(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PTZGetAbsolutePosition", new object[2] { token, deviceId }, callback, asyncState);
	}

	public PTZArgs EndPTZGetAbsolutePosition(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (PTZArgs)array[0];
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

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/PresetGetList", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string[] PresetGetList(string token, Guid deviceId)
	{
		object[] array = Invoke("PresetGetList", new object[2] { token, deviceId });
		return (string[])array[0];
	}

	public IAsyncResult BeginPresetGetList(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("PresetGetList", new object[2] { token, deviceId }, callback, asyncState);
	}

	public string[] EndPresetGetList(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (string[])array[0];
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
		object[] array = Invoke("OutputGetState", new object[2] { token, deviceId });
		return (IOState)array[0];
	}

	public IAsyncResult BeginOutputGetState(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("OutputGetState", new object[2] { token, deviceId }, callback, asyncState);
	}

	public IOState EndOutputGetState(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (IOState)array[0];
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
		object[] array = Invoke("SequencesGetTypes", new object[2] { token, deviceId });
		return (SequenceType[])array[0];
	}

	public IAsyncResult BeginSequencesGetTypes(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SequencesGetTypes", new object[2] { token, deviceId }, callback, asyncState);
	}

	public SequenceType[] EndSequencesGetTypes(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (SequenceType[])array[0];
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
		object[] array = Invoke("SequencesGet", new object[6] { token, deviceId, sequenceType, minTime, maxTime, maxCount });
		return (SequenceEntry[])array[0];
	}

	public IAsyncResult BeginSequencesGet(string token, Guid deviceId, Guid sequenceType, DateTime minTime, DateTime maxTime, int maxCount, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SequencesGet", new object[6] { token, deviceId, sequenceType, minTime, maxTime, maxCount }, callback, asyncState);
	}

	public SequenceEntry[] EndSequencesGet(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (SequenceEntry[])array[0];
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
		object[] array = Invoke("SequencesGetAround", new object[6] { token, deviceId, sequenceType, centerTime, maxCountBefore, maxCountAfter });
		return (SequenceEntry[])array[0];
	}

	public IAsyncResult BeginSequencesGetAround(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, int maxCountBefore, int maxCountAfter, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SequencesGetAround", new object[6] { token, deviceId, sequenceType, centerTime, maxCountBefore, maxCountAfter }, callback, asyncState);
	}

	public SequenceEntry[] EndSequencesGetAround(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (SequenceEntry[])array[0];
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

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/Test", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string Test(string Name)
	{
		object[] array = Invoke("Test", new object[1] { Name });
		return (string)array[0];
	}

	public IAsyncResult BeginTest(string Name, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("Test", new object[1] { Name }, callback, asyncState);
	}

	public string EndTest(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (string)array[0];
	}

	public void TestAsync(string Name)
	{
		TestAsync(Name, null);
	}

	public void TestAsync(string Name, object userState)
	{
		if (TestOperationCompleted == null)
		{
			TestOperationCompleted = OnTestOperationCompleted;
		}
		InvokeAsync("Test", new object[1] { Name }, TestOperationCompleted, userState);
	}

	private void OnTestOperationCompleted(object arg)
	{
		if (this.TestCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.TestCompleted(this, new TestCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/SequencesGetAroundWithSpan", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public SequenceEntry[] SequencesGetAroundWithSpan(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter)
	{
		object[] array = Invoke("SequencesGetAroundWithSpan", new object[8] { token, deviceId, sequenceType, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter });
		return (SequenceEntry[])array[0];
	}

	public IAsyncResult BeginSequencesGetAroundWithSpan(string token, Guid deviceId, Guid sequenceType, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SequencesGetAroundWithSpan", new object[8] { token, deviceId, sequenceType, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter }, callback, asyncState);
	}

	public SequenceEntry[] EndSequencesGetAroundWithSpan(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (SequenceEntry[])array[0];
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

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/TimeLineInformationGet", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public TimeLineInformationData[] TimeLineInformationGet(string token, Guid deviceId, Guid[] timeLineInformationTypes, DateTime timeLineInformationBeginTime, TimeDuration timeLineInformationInterval, int timeLineInformationCount)
	{
		object[] array = Invoke("TimeLineInformationGet", new object[6] { token, deviceId, timeLineInformationTypes, timeLineInformationBeginTime, timeLineInformationInterval, timeLineInformationCount });
		return (TimeLineInformationData[])array[0];
	}

	public IAsyncResult BeginTimeLineInformationGet(string token, Guid deviceId, Guid[] timeLineInformationTypes, DateTime timeLineInformationBeginTime, TimeDuration timeLineInformationInterval, int timeLineInformationCount, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("TimeLineInformationGet", new object[6] { token, deviceId, timeLineInformationTypes, timeLineInformationBeginTime, timeLineInformationInterval, timeLineInformationCount }, callback, asyncState);
	}

	public TimeLineInformationData[] EndTimeLineInformationGet(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (TimeLineInformationData[])array[0];
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
		object[] array = Invoke("SmartSearchStart", new object[9] { token, deviceId, searchBeginTime, searchEndTime, sensitivity, searchInterval, searchMask, ignoreInitialMotion, resultMotionMaskSize });
		return (Guid)array[0];
	}

	public IAsyncResult BeginSmartSearchStart(string token, Guid deviceId, DateTime searchBeginTime, DateTime searchEndTime, int sensitivity, TimeDuration searchInterval, ImageMask searchMask, bool ignoreInitialMotion, Size resultMotionMaskSize, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SmartSearchStart", new object[9] { token, deviceId, searchBeginTime, searchEndTime, sensitivity, searchInterval, searchMask, ignoreInitialMotion, resultMotionMaskSize }, callback, asyncState);
	}

	public Guid EndSmartSearchStart(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (Guid)array[0];
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
		object[] array = Invoke("SmartSearchGetStatus", new object[2] { token, smartSearchId });
		return (SmartSearchStatus)array[0];
	}

	public IAsyncResult BeginSmartSearchGetStatus(string token, Guid smartSearchId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SmartSearchGetStatus", new object[2] { token, smartSearchId }, callback, asyncState);
	}

	public SmartSearchStatus EndSmartSearchGetStatus(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (SmartSearchStatus)array[0];
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
		object[] array = Invoke("SmartSearchGetResult", new object[3] { token, smartSearchId, continueSearch });
		return (SmartSearchResult)array[0];
	}

	public IAsyncResult BeginSmartSearchGetResult(string token, Guid smartSearchId, bool continueSearch, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SmartSearchGetResult", new object[3] { token, smartSearchId, continueSearch }, callback, asyncState);
	}

	public SmartSearchResult EndSmartSearchGetResult(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (SmartSearchResult)array[0];
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

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/ControlLiveStream", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void ControlLiveStream(string token, Guid clientID, Guid controlID, LiveStreamDevice[] devices)
	{
		Invoke("ControlLiveStream", new object[4] { token, clientID, controlID, devices });
	}

	public IAsyncResult BeginControlLiveStream(string token, Guid clientID, Guid controlID, LiveStreamDevice[] devices, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("ControlLiveStream", new object[4] { token, clientID, controlID, devices }, callback, asyncState);
	}

	public void EndControlLiveStream(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void ControlLiveStreamAsync(string token, Guid clientID, Guid controlID, LiveStreamDevice[] devices)
	{
		ControlLiveStreamAsync(token, clientID, controlID, devices, null);
	}

	public void ControlLiveStreamAsync(string token, Guid clientID, Guid controlID, LiveStreamDevice[] devices, object userState)
	{
		if (ControlLiveStreamOperationCompleted == null)
		{
			ControlLiveStreamOperationCompleted = OnControlLiveStreamOperationCompleted;
		}
		InvokeAsync("ControlLiveStream", new object[4] { token, clientID, controlID, devices }, ControlLiveStreamOperationCompleted, userState);
	}

	private void OnControlLiveStreamOperationCompleted(object arg)
	{
		if (this.ControlLiveStreamCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.ControlLiveStreamCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/JPEGGetAt", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public JPEGData JPEGGetAt(string token, Guid deviceId, DateTime time)
	{
		object[] array = Invoke("JPEGGetAt", new object[3] { token, deviceId, time });
		return (JPEGData)array[0];
	}

	public IAsyncResult BeginJPEGGetAt(string token, Guid deviceId, DateTime time, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("JPEGGetAt", new object[3] { token, deviceId, time }, callback, asyncState);
	}

	public JPEGData EndJPEGGetAt(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (JPEGData)array[0];
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
		object[] array = Invoke("JPEGGetAtOrBefore", new object[3] { token, deviceId, time });
		return (JPEGData)array[0];
	}

	public IAsyncResult BeginJPEGGetAtOrBefore(string token, Guid deviceId, DateTime time, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("JPEGGetAtOrBefore", new object[3] { token, deviceId, time }, callback, asyncState);
	}

	public JPEGData EndJPEGGetAtOrBefore(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (JPEGData)array[0];
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
		object[] array = Invoke("JPEGGetAtOrAfter", new object[3] { token, deviceId, time });
		return (JPEGData)array[0];
	}

	public IAsyncResult BeginJPEGGetAtOrAfter(string token, Guid deviceId, DateTime time, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("JPEGGetAtOrAfter", new object[3] { token, deviceId, time }, callback, asyncState);
	}

	public JPEGData EndJPEGGetAtOrAfter(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (JPEGData)array[0];
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

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderCommand/InputGetState", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public IOState InputGetState(string token, Guid deviceId)
	{
		object[] array = Invoke("InputGetState", new object[2] { token, deviceId });
		return (IOState)array[0];
	}

	public IAsyncResult BeginInputGetState(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("InputGetState", new object[2] { token, deviceId }, callback, asyncState);
	}

	public IOState EndInputGetState(IAsyncResult asyncResult)
	{
		object[] array = EndInvoke(asyncResult);
		return (IOState)array[0];
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

	public new void CancelAsync(object userState)
	{
		base.CancelAsync(userState);
	}
}
