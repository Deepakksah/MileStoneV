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
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2;

[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[WebServiceBinding(Name = "RecorderStatusService2Soap", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
[XmlInclude(typeof(MediaStreamStatisticsBase))]
[XmlInclude(typeof(DeviceStatisticsBase))]
[XmlInclude(typeof(StatusBase))]
public class RecorderStatusService2 : SoapHttpClientProtocol
{
	private SendOrPostCallback GetVersionOperationCompleted;

	private SendOrPostCallback StartStatusSessionOperationCompleted;

	private SendOrPostCallback StopStatusSessionOperationCompleted;

	private SendOrPostCallback SubscribeEventStatusOperationCompleted;

	private SendOrPostCallback SubscribeDeviceStatusOperationCompleted;

	private SendOrPostCallback SubscribeHardwareStatusOperationCompleted;

	private SendOrPostCallback SubscribeConfigurationStatusOperationCompleted;

	private SendOrPostCallback GetStatusOperationCompleted;

	private SendOrPostCallback GetCurrentDeviceStatusOperationCompleted;

	private SendOrPostCallback GetCurrentHardwareStatusOperationCompleted;

	private SendOrPostCallback GetRecorderStatusOperationCompleted;

	private SendOrPostCallback GetRecordingStorageStatusOperationCompleted;

	private SendOrPostCallback GetArchiveStorageStatusOperationCompleted;

	private SendOrPostCallback GetVideoDeviceStatisticsOperationCompleted;

	private SendOrPostCallback GetAudioDeviceStatisticsOperationCompleted;

	private SendOrPostCallback GetMetadataDeviceStatisticsOperationCompleted;

	public event GetVersionCompletedEventHandler GetVersionCompleted;

	public event StartStatusSessionCompletedEventHandler StartStatusSessionCompleted;

	public event StopStatusSessionCompletedEventHandler StopStatusSessionCompleted;

	public event SubscribeEventStatusCompletedEventHandler SubscribeEventStatusCompleted;

	public event SubscribeDeviceStatusCompletedEventHandler SubscribeDeviceStatusCompleted;

	public event SubscribeHardwareStatusCompletedEventHandler SubscribeHardwareStatusCompleted;

	public event SubscribeConfigurationStatusCompletedEventHandler SubscribeConfigurationStatusCompleted;

	public event GetStatusCompletedEventHandler GetStatusCompleted;

	public event GetCurrentDeviceStatusCompletedEventHandler GetCurrentDeviceStatusCompleted;

	public event GetCurrentHardwareStatusCompletedEventHandler GetCurrentHardwareStatusCompleted;

	public event GetRecorderStatusCompletedEventHandler GetRecorderStatusCompleted;

	public event GetRecordingStorageStatusCompletedEventHandler GetRecordingStorageStatusCompleted;

	public event GetArchiveStorageStatusCompletedEventHandler GetArchiveStorageStatusCompleted;

	public event GetVideoDeviceStatisticsCompletedEventHandler GetVideoDeviceStatisticsCompleted;

	public event GetAudioDeviceStatisticsCompletedEventHandler GetAudioDeviceStatisticsCompleted;

	public event GetMetadataDeviceStatisticsCompletedEventHandler GetMetadataDeviceStatisticsCompleted;

	public RecorderStatusService2(Uri hostUri)
	{
		base.SoapVersion = SoapProtocolVersion.Soap12;
		base.Url = new UriBuilder(hostUri)
		{
			Path = "/recorderstatusservice/recorderstatusservice2.asmx"
		}.ToString();
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/GetVersion", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
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

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/StartStatusSession", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public Guid StartStatusSession(string token)
	{
		return (Guid)Invoke("StartStatusSession", new object[1] { token })[0];
	}

	public IAsyncResult BeginStartStatusSession(string token, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("StartStatusSession", new object[1] { token }, callback, asyncState);
	}

	public Guid EndStartStatusSession(IAsyncResult asyncResult)
	{
		return (Guid)EndInvoke(asyncResult)[0];
	}

	public void StartStatusSessionAsync(string token)
	{
		StartStatusSessionAsync(token, null);
	}

	public void StartStatusSessionAsync(string token, object userState)
	{
		if (StartStatusSessionOperationCompleted == null)
		{
			StartStatusSessionOperationCompleted = OnStartStatusSessionOperationCompleted;
		}
		InvokeAsync("StartStatusSession", new object[1] { token }, StartStatusSessionOperationCompleted, userState);
	}

	private void OnStartStatusSessionOperationCompleted(object arg)
	{
		if (this.StartStatusSessionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.StartStatusSessionCompleted(this, new StartStatusSessionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/StopStatusSession", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void StopStatusSession(string token, Guid statusSessionId)
	{
		Invoke("StopStatusSession", new object[2] { token, statusSessionId });
	}

	public IAsyncResult BeginStopStatusSession(string token, Guid statusSessionId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("StopStatusSession", new object[2] { token, statusSessionId }, callback, asyncState);
	}

	public void EndStopStatusSession(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void StopStatusSessionAsync(string token, Guid statusSessionId)
	{
		StopStatusSessionAsync(token, statusSessionId, null);
	}

	public void StopStatusSessionAsync(string token, Guid statusSessionId, object userState)
	{
		if (StopStatusSessionOperationCompleted == null)
		{
			StopStatusSessionOperationCompleted = OnStopStatusSessionOperationCompleted;
		}
		InvokeAsync("StopStatusSession", new object[2] { token, statusSessionId }, StopStatusSessionOperationCompleted, userState);
	}

	private void OnStopStatusSessionOperationCompleted(object arg)
	{
		if (this.StopStatusSessionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.StopStatusSessionCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/SubscribeEventStatus", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void SubscribeEventStatus(string token, Guid statusSessionId, Guid[] eventIds)
	{
		Invoke("SubscribeEventStatus", new object[3] { token, statusSessionId, eventIds });
	}

	public IAsyncResult BeginSubscribeEventStatus(string token, Guid statusSessionId, Guid[] eventIds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SubscribeEventStatus", new object[3] { token, statusSessionId, eventIds }, callback, asyncState);
	}

	public void EndSubscribeEventStatus(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void SubscribeEventStatusAsync(string token, Guid statusSessionId, Guid[] eventIds)
	{
		SubscribeEventStatusAsync(token, statusSessionId, eventIds, null);
	}

	public void SubscribeEventStatusAsync(string token, Guid statusSessionId, Guid[] eventIds, object userState)
	{
		if (SubscribeEventStatusOperationCompleted == null)
		{
			SubscribeEventStatusOperationCompleted = OnSubscribeEventStatusOperationCompleted;
		}
		InvokeAsync("SubscribeEventStatus", new object[3] { token, statusSessionId, eventIds }, SubscribeEventStatusOperationCompleted, userState);
	}

	private void OnSubscribeEventStatusOperationCompleted(object arg)
	{
		if (this.SubscribeEventStatusCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SubscribeEventStatusCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/SubscribeDeviceStatus", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void SubscribeDeviceStatus(string token, Guid statusSessionId, Guid[] deviceIds)
	{
		Invoke("SubscribeDeviceStatus", new object[3] { token, statusSessionId, deviceIds });
	}

	public IAsyncResult BeginSubscribeDeviceStatus(string token, Guid statusSessionId, Guid[] deviceIds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SubscribeDeviceStatus", new object[3] { token, statusSessionId, deviceIds }, callback, asyncState);
	}

	public void EndSubscribeDeviceStatus(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void SubscribeDeviceStatusAsync(string token, Guid statusSessionId, Guid[] deviceIds)
	{
		SubscribeDeviceStatusAsync(token, statusSessionId, deviceIds, null);
	}

	public void SubscribeDeviceStatusAsync(string token, Guid statusSessionId, Guid[] deviceIds, object userState)
	{
		if (SubscribeDeviceStatusOperationCompleted == null)
		{
			SubscribeDeviceStatusOperationCompleted = OnSubscribeDeviceStatusOperationCompleted;
		}
		InvokeAsync("SubscribeDeviceStatus", new object[3] { token, statusSessionId, deviceIds }, SubscribeDeviceStatusOperationCompleted, userState);
	}

	private void OnSubscribeDeviceStatusOperationCompleted(object arg)
	{
		if (this.SubscribeDeviceStatusCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SubscribeDeviceStatusCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/SubscribeHardwareStatus", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void SubscribeHardwareStatus(string token, Guid statusSessionId, Guid[] hardwareIds)
	{
		Invoke("SubscribeHardwareStatus", new object[3] { token, statusSessionId, hardwareIds });
	}

	public IAsyncResult BeginSubscribeHardwareStatus(string token, Guid statusSessionId, Guid[] hardwareIds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SubscribeHardwareStatus", new object[3] { token, statusSessionId, hardwareIds }, callback, asyncState);
	}

	public void EndSubscribeHardwareStatus(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void SubscribeHardwareStatusAsync(string token, Guid statusSessionId, Guid[] hardwareIds)
	{
		SubscribeHardwareStatusAsync(token, statusSessionId, hardwareIds, null);
	}

	public void SubscribeHardwareStatusAsync(string token, Guid statusSessionId, Guid[] hardwareIds, object userState)
	{
		if (SubscribeHardwareStatusOperationCompleted == null)
		{
			SubscribeHardwareStatusOperationCompleted = OnSubscribeHardwareStatusOperationCompleted;
		}
		InvokeAsync("SubscribeHardwareStatus", new object[3] { token, statusSessionId, hardwareIds }, SubscribeHardwareStatusOperationCompleted, userState);
	}

	private void OnSubscribeHardwareStatusOperationCompleted(object arg)
	{
		if (this.SubscribeHardwareStatusCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SubscribeHardwareStatusCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/SubscribeConfigurationStatus", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void SubscribeConfigurationStatus(string token, Guid statusSessionId, bool subscribe)
	{
		Invoke("SubscribeConfigurationStatus", new object[3] { token, statusSessionId, subscribe });
	}

	public IAsyncResult BeginSubscribeConfigurationStatus(string token, Guid statusSessionId, bool subscribe, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SubscribeConfigurationStatus", new object[3] { token, statusSessionId, subscribe }, callback, asyncState);
	}

	public void EndSubscribeConfigurationStatus(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void SubscribeConfigurationStatusAsync(string token, Guid statusSessionId, bool subscribe)
	{
		SubscribeConfigurationStatusAsync(token, statusSessionId, subscribe, null);
	}

	public void SubscribeConfigurationStatusAsync(string token, Guid statusSessionId, bool subscribe, object userState)
	{
		if (SubscribeConfigurationStatusOperationCompleted == null)
		{
			SubscribeConfigurationStatusOperationCompleted = OnSubscribeConfigurationStatusOperationCompleted;
		}
		InvokeAsync("SubscribeConfigurationStatus", new object[3] { token, statusSessionId, subscribe }, SubscribeConfigurationStatusOperationCompleted, userState);
	}

	private void OnSubscribeConfigurationStatusOperationCompleted(object arg)
	{
		if (this.SubscribeConfigurationStatusCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SubscribeConfigurationStatusCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/GetStatus", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public Status GetStatus(string token, Guid statusSessionId, int millisecondsTimeout)
	{
		return (Status)Invoke("GetStatus", new object[3] { token, statusSessionId, millisecondsTimeout })[0];
	}

	public IAsyncResult BeginGetStatus(string token, Guid statusSessionId, int millisecondsTimeout, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetStatus", new object[3] { token, statusSessionId, millisecondsTimeout }, callback, asyncState);
	}

	public Status EndGetStatus(IAsyncResult asyncResult)
	{
		return (Status)EndInvoke(asyncResult)[0];
	}

	public void GetStatusAsync(string token, Guid statusSessionId, int millisecondsTimeout)
	{
		GetStatusAsync(token, statusSessionId, millisecondsTimeout, null);
	}

	public void GetStatusAsync(string token, Guid statusSessionId, int millisecondsTimeout, object userState)
	{
		if (GetStatusOperationCompleted == null)
		{
			GetStatusOperationCompleted = OnGetStatusOperationCompleted;
		}
		InvokeAsync("GetStatus", new object[3] { token, statusSessionId, millisecondsTimeout }, GetStatusOperationCompleted, userState);
	}

	private void OnGetStatusOperationCompleted(object arg)
	{
		if (this.GetStatusCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetStatusCompleted(this, new GetStatusCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/GetCurrentDeviceStatus", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public Status GetCurrentDeviceStatus(string token, Guid[] deviceIds)
	{
		return (Status)Invoke("GetCurrentDeviceStatus", new object[2] { token, deviceIds })[0];
	}

	public IAsyncResult BeginGetCurrentDeviceStatus(string token, Guid[] deviceIds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetCurrentDeviceStatus", new object[2] { token, deviceIds }, callback, asyncState);
	}

	public Status EndGetCurrentDeviceStatus(IAsyncResult asyncResult)
	{
		return (Status)EndInvoke(asyncResult)[0];
	}

	public void GetCurrentDeviceStatusAsync(string token, Guid[] deviceIds)
	{
		GetCurrentDeviceStatusAsync(token, deviceIds, null);
	}

	public void GetCurrentDeviceStatusAsync(string token, Guid[] deviceIds, object userState)
	{
		if (GetCurrentDeviceStatusOperationCompleted == null)
		{
			GetCurrentDeviceStatusOperationCompleted = OnGetCurrentDeviceStatusOperationCompleted;
		}
		InvokeAsync("GetCurrentDeviceStatus", new object[2] { token, deviceIds }, GetCurrentDeviceStatusOperationCompleted, userState);
	}

	private void OnGetCurrentDeviceStatusOperationCompleted(object arg)
	{
		if (this.GetCurrentDeviceStatusCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetCurrentDeviceStatusCompleted(this, new GetCurrentDeviceStatusCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/GetCurrentHardwareStatus", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public Status GetCurrentHardwareStatus(string token, Guid[] hardwareIds)
	{
		return (Status)Invoke("GetCurrentHardwareStatus", new object[2] { token, hardwareIds })[0];
	}

	public IAsyncResult BeginGetCurrentHardwareStatus(string token, Guid[] hardwareIds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetCurrentHardwareStatus", new object[2] { token, hardwareIds }, callback, asyncState);
	}

	public Status EndGetCurrentHardwareStatus(IAsyncResult asyncResult)
	{
		return (Status)EndInvoke(asyncResult)[0];
	}

	public void GetCurrentHardwareStatusAsync(string token, Guid[] hardwareIds)
	{
		GetCurrentHardwareStatusAsync(token, hardwareIds, null);
	}

	public void GetCurrentHardwareStatusAsync(string token, Guid[] hardwareIds, object userState)
	{
		if (GetCurrentHardwareStatusOperationCompleted == null)
		{
			GetCurrentHardwareStatusOperationCompleted = OnGetCurrentHardwareStatusOperationCompleted;
		}
		InvokeAsync("GetCurrentHardwareStatus", new object[2] { token, hardwareIds }, GetCurrentHardwareStatusOperationCompleted, userState);
	}

	private void OnGetCurrentHardwareStatusOperationCompleted(object arg)
	{
		if (this.GetCurrentHardwareStatusCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetCurrentHardwareStatusCompleted(this, new GetCurrentHardwareStatusCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/GetRecorderStatus", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public AttachAndConnectionState GetRecorderStatus(string token)
	{
		return (AttachAndConnectionState)Invoke("GetRecorderStatus", new object[1] { token })[0];
	}

	public IAsyncResult BeginGetRecorderStatus(string token, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetRecorderStatus", new object[1] { token }, callback, asyncState);
	}

	public AttachAndConnectionState EndGetRecorderStatus(IAsyncResult asyncResult)
	{
		return (AttachAndConnectionState)EndInvoke(asyncResult)[0];
	}

	public void GetRecorderStatusAsync(string token)
	{
		GetRecorderStatusAsync(token, null);
	}

	public void GetRecorderStatusAsync(string token, object userState)
	{
		if (GetRecorderStatusOperationCompleted == null)
		{
			GetRecorderStatusOperationCompleted = OnGetRecorderStatusOperationCompleted;
		}
		InvokeAsync("GetRecorderStatus", new object[1] { token }, GetRecorderStatusOperationCompleted, userState);
	}

	private void OnGetRecorderStatusOperationCompleted(object arg)
	{
		if (this.GetRecorderStatusCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetRecorderStatusCompleted(this, new GetRecorderStatusCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/GetRecordingStorageStatus", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public StorageStatus[] GetRecordingStorageStatus(string token)
	{
		return (StorageStatus[])Invoke("GetRecordingStorageStatus", new object[1] { token })[0];
	}

	public IAsyncResult BeginGetRecordingStorageStatus(string token, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetRecordingStorageStatus", new object[1] { token }, callback, asyncState);
	}

	public StorageStatus[] EndGetRecordingStorageStatus(IAsyncResult asyncResult)
	{
		return (StorageStatus[])EndInvoke(asyncResult)[0];
	}

	public void GetRecordingStorageStatusAsync(string token)
	{
		GetRecordingStorageStatusAsync(token, null);
	}

	public void GetRecordingStorageStatusAsync(string token, object userState)
	{
		if (GetRecordingStorageStatusOperationCompleted == null)
		{
			GetRecordingStorageStatusOperationCompleted = OnGetRecordingStorageStatusOperationCompleted;
		}
		InvokeAsync("GetRecordingStorageStatus", new object[1] { token }, GetRecordingStorageStatusOperationCompleted, userState);
	}

	private void OnGetRecordingStorageStatusOperationCompleted(object arg)
	{
		if (this.GetRecordingStorageStatusCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetRecordingStorageStatusCompleted(this, new GetRecordingStorageStatusCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/GetArchiveStorageStatus", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public StorageStatus[] GetArchiveStorageStatus(string token)
	{
		return (StorageStatus[])Invoke("GetArchiveStorageStatus", new object[1] { token })[0];
	}

	public IAsyncResult BeginGetArchiveStorageStatus(string token, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetArchiveStorageStatus", new object[1] { token }, callback, asyncState);
	}

	public StorageStatus[] EndGetArchiveStorageStatus(IAsyncResult asyncResult)
	{
		return (StorageStatus[])EndInvoke(asyncResult)[0];
	}

	public void GetArchiveStorageStatusAsync(string token)
	{
		GetArchiveStorageStatusAsync(token, null);
	}

	public void GetArchiveStorageStatusAsync(string token, object userState)
	{
		if (GetArchiveStorageStatusOperationCompleted == null)
		{
			GetArchiveStorageStatusOperationCompleted = OnGetArchiveStorageStatusOperationCompleted;
		}
		InvokeAsync("GetArchiveStorageStatus", new object[1] { token }, GetArchiveStorageStatusOperationCompleted, userState);
	}

	private void OnGetArchiveStorageStatusOperationCompleted(object arg)
	{
		if (this.GetArchiveStorageStatusCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetArchiveStorageStatusCompleted(this, new GetArchiveStorageStatusCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/GetVideoDeviceStatistics", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public VideoDeviceStatistics[] GetVideoDeviceStatistics(string token, Guid[] deviceIds)
	{
		return (VideoDeviceStatistics[])Invoke("GetVideoDeviceStatistics", new object[2] { token, deviceIds })[0];
	}

	public IAsyncResult BeginGetVideoDeviceStatistics(string token, Guid[] deviceIds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetVideoDeviceStatistics", new object[2] { token, deviceIds }, callback, asyncState);
	}

	public VideoDeviceStatistics[] EndGetVideoDeviceStatistics(IAsyncResult asyncResult)
	{
		return (VideoDeviceStatistics[])EndInvoke(asyncResult)[0];
	}

	public void GetVideoDeviceStatisticsAsync(string token, Guid[] deviceIds)
	{
		GetVideoDeviceStatisticsAsync(token, deviceIds, null);
	}

	public void GetVideoDeviceStatisticsAsync(string token, Guid[] deviceIds, object userState)
	{
		if (GetVideoDeviceStatisticsOperationCompleted == null)
		{
			GetVideoDeviceStatisticsOperationCompleted = OnGetVideoDeviceStatisticsOperationCompleted;
		}
		InvokeAsync("GetVideoDeviceStatistics", new object[2] { token, deviceIds }, GetVideoDeviceStatisticsOperationCompleted, userState);
	}

	private void OnGetVideoDeviceStatisticsOperationCompleted(object arg)
	{
		if (this.GetVideoDeviceStatisticsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetVideoDeviceStatisticsCompleted(this, new GetVideoDeviceStatisticsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/GetAudioDeviceStatistics", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public AudioDeviceStatistics[] GetAudioDeviceStatistics(string token, Guid[] deviceIds)
	{
		return (AudioDeviceStatistics[])Invoke("GetAudioDeviceStatistics", new object[2] { token, deviceIds })[0];
	}

	public IAsyncResult BeginGetAudioDeviceStatistics(string token, Guid[] deviceIds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAudioDeviceStatistics", new object[2] { token, deviceIds }, callback, asyncState);
	}

	public AudioDeviceStatistics[] EndGetAudioDeviceStatistics(IAsyncResult asyncResult)
	{
		return (AudioDeviceStatistics[])EndInvoke(asyncResult)[0];
	}

	public void GetAudioDeviceStatisticsAsync(string token, Guid[] deviceIds)
	{
		GetAudioDeviceStatisticsAsync(token, deviceIds, null);
	}

	public void GetAudioDeviceStatisticsAsync(string token, Guid[] deviceIds, object userState)
	{
		if (GetAudioDeviceStatisticsOperationCompleted == null)
		{
			GetAudioDeviceStatisticsOperationCompleted = OnGetAudioDeviceStatisticsOperationCompleted;
		}
		InvokeAsync("GetAudioDeviceStatistics", new object[2] { token, deviceIds }, GetAudioDeviceStatisticsOperationCompleted, userState);
	}

	private void OnGetAudioDeviceStatisticsOperationCompleted(object arg)
	{
		if (this.GetAudioDeviceStatisticsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAudioDeviceStatisticsCompleted(this, new GetAudioDeviceStatisticsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus2/GetMetadataDeviceStatistics", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public MetadataDeviceStatistics[] GetMetadataDeviceStatistics(string token, Guid[] deviceIds)
	{
		return (MetadataDeviceStatistics[])Invoke("GetMetadataDeviceStatistics", new object[2] { token, deviceIds })[0];
	}

	public IAsyncResult BeginGetMetadataDeviceStatistics(string token, Guid[] deviceIds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetMetadataDeviceStatistics", new object[2] { token, deviceIds }, callback, asyncState);
	}

	public MetadataDeviceStatistics[] EndGetMetadataDeviceStatistics(IAsyncResult asyncResult)
	{
		return (MetadataDeviceStatistics[])EndInvoke(asyncResult)[0];
	}

	public void GetMetadataDeviceStatisticsAsync(string token, Guid[] deviceIds)
	{
		GetMetadataDeviceStatisticsAsync(token, deviceIds, null);
	}

	public void GetMetadataDeviceStatisticsAsync(string token, Guid[] deviceIds, object userState)
	{
		if (GetMetadataDeviceStatisticsOperationCompleted == null)
		{
			GetMetadataDeviceStatisticsOperationCompleted = OnGetMetadataDeviceStatisticsOperationCompleted;
		}
		InvokeAsync("GetMetadataDeviceStatistics", new object[2] { token, deviceIds }, GetMetadataDeviceStatisticsOperationCompleted, userState);
	}

	private void OnGetMetadataDeviceStatisticsOperationCompleted(object arg)
	{
		if (this.GetMetadataDeviceStatisticsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetMetadataDeviceStatisticsCompleted(this, new GetMetadataDeviceStatisticsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
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
