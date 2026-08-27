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

namespace VideoOS.RecorderWebservice.Shared.Status;

[GeneratedCode("wsdl", "2.0.50727.42")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[WebServiceBinding(Name = "RecorderStatusServiceSoap", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus")]
public class RecorderStatusService : SoapHttpClientProtocol
{
	private SendOrPostCallback GetVersionOperationCompleted;

	private SendOrPostCallback StartStatusSessionOperationCompleted;

	private SendOrPostCallback StopStatusSessionOperationCompleted;

	private SendOrPostCallback SubscribeEventStatusOperationCompleted;

	private SendOrPostCallback SubscribeDeviceStatusOperationCompleted;

	private SendOrPostCallback SubscribeConfigurationStatusOperationCompleted;

	private SendOrPostCallback GetStatusOperationCompleted;

	public event GetVersionCompletedEventHandler GetVersionCompleted;

	public event StartStatusSessionCompletedEventHandler StartStatusSessionCompleted;

	public event StopStatusSessionCompletedEventHandler StopStatusSessionCompleted;

	public event SubscribeEventStatusCompletedEventHandler SubscribeEventStatusCompleted;

	public event SubscribeDeviceStatusCompletedEventHandler SubscribeDeviceStatusCompleted;

	public event SubscribeConfigurationStatusCompletedEventHandler SubscribeConfigurationStatusCompleted;

	public event GetStatusCompletedEventHandler GetStatusCompleted;

	public RecorderStatusService()
	{
		base.Url = "http://localhost/RecorderAPIGenerator/RecorderStatusService.asmx";
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus/GetVersion", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
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

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus/StartStatusSession", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
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

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus/StopStatusSession", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
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

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus/SubscribeEventStatus", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
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

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus/SubscribeDeviceStatus", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
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

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus/SubscribeConfigurationStatus", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
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

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSRecorderStatus/GetStatus", RequestNamespace = "http://videoos.net/2/XProtectCSRecorderStatus", ResponseNamespace = "http://videoos.net/2/XProtectCSRecorderStatus", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public StatusBase[] GetStatus(string token, Guid statusSessionId, int millisecondsTimeout)
	{
		return (StatusBase[])Invoke("GetStatus", new object[3] { token, statusSessionId, millisecondsTimeout })[0];
	}

	public IAsyncResult BeginGetStatus(string token, Guid statusSessionId, int millisecondsTimeout, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetStatus", new object[3] { token, statusSessionId, millisecondsTimeout }, callback, asyncState);
	}

	public StatusBase[] EndGetStatus(IAsyncResult asyncResult)
	{
		return (StatusBase[])EndInvoke(asyncResult)[0];
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
