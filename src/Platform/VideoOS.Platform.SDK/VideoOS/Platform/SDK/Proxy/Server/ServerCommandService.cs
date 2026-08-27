using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Server;

[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[WebServiceBinding(Name = "ServerCommandServiceSoap", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[XmlInclude(typeof(GroupInfo))]
[XmlInclude(typeof(DeviceInfo))]
[XmlInclude(typeof(DictionaryEntry[]))]
[ExcludeFromCodeCoverage]
public class ServerCommandService : SoapHttpClientProtocol
{
	private SendOrPostCallback GetVersionOperationCompleted;

	private SendOrPostCallback GetServerVersionOperationCompleted;

	private SendOrPostCallback LoginOperationCompleted;

	private SendOrPostCallback LogoutOperationCompleted;

	private SendOrPostCallback GetVmoServicesListOperationCompleted;

	private SendOrPostCallback GetSmartClientVersionOperationCompleted;

	private SendOrPostCallback CheckSmartClientVersionOperationCompleted;

	private SendOrPostCallback GetProductInfoOperationCompleted;

	private SendOrPostCallback GetSystemLicenseInfoOperationCompleted;

	private SendOrPostCallback GetUserInfoOperationCompleted;

	private SendOrPostCallback GetViewGroupsOperationCompleted;

	private SendOrPostCallback GetViewGroupDataOperationCompleted;

	private SendOrPostCallback SetViewGroupDataOperationCompleted;

	private SendOrPostCallback GetCustomSettingDataGlobalOperationCompleted;

	private SendOrPostCallback GetCustomSettingDataUserOperationCompleted;

	private SendOrPostCallback SetCustomSettingDataGlobalOperationCompleted;

	private SendOrPostCallback SetCustomSettingDataUserOperationCompleted;

	private SendOrPostCallback EventTriggerOperationCompleted;

	private SendOrPostCallback EventTriggerWithMetadataOperationCompleted;

	private SendOrPostCallback AlertsGetAroundWithSpanOperationCompleted;

	private SendOrPostCallback MatrixMonitorActivateCameraOperationCompleted;

	private SendOrPostCallback MatrixMonitorActivateScriptOperationCompleted;

	private SendOrPostCallback QueryChangesOperationCompleted;

	private SendOrPostCallback QueryRecorderInfoOperationCompleted;

	private SendOrPostCallback GetConfigurationOperationCompleted;

	private SendOrPostCallback BookmarkGetNewReferenceOperationCompleted;

	private SendOrPostCallback BookmarkCreateOperationCompleted;

	private SendOrPostCallback BookmarkUpdateOperationCompleted;

	private SendOrPostCallback BookmarkDeleteOperationCompleted;

	private SendOrPostCallback BookmarkGetOperationCompleted;

	private SendOrPostCallback BookmarkSearchTimeOperationCompleted;

	private SendOrPostCallback BookmarkSearchFromBookmarkOperationCompleted;

	private SendOrPostCallback AuthorizeOperationCompleted;

	private SendOrPostCallback MarkedDataGetNewReferenceOperationCompleted;

	private SendOrPostCallback MarkedDataCreateOperationCompleted;

	private SendOrPostCallback MarkedDataUpdateOperationCompleted;

	private SendOrPostCallback MarkedDataDeleteOperationCompleted;

	private SendOrPostCallback MarkedDataGetOperationCompleted;

	private SendOrPostCallback MarkedDataSearchOperationCompleted;

	private SendOrPostCallback GetClientProfileOperationCompleted;

	private SendOrPostCallback GetPresetsOperationCompleted;

	private SendOrPostCallback DeletePresetOperationCompleted;

	private SendOrPostCallback UpdatePresetNameOperationCompleted;

	private SendOrPostCallback UpdatePresetPositionOperationCompleted;

	private SendOrPostCallback CreatePresetOperationCompleted;

	private SendOrPostCallback OrderPresetsOperationCompleted;

	public string AuthorizationId { get; set; }

	public event GetVersionCompletedEventHandler GetVersionCompleted;

	public event GetServerVersionCompletedEventHandler GetServerVersionCompleted;

	public event LoginCompletedEventHandler LoginCompleted;

	public event LogoutCompletedEventHandler LogoutCompleted;

	public event GetVmoServicesListCompletedEventHandler GetVmoServicesListCompleted;

	public event GetSmartClientVersionCompletedEventHandler GetSmartClientVersionCompleted;

	public event CheckSmartClientVersionCompletedEventHandler CheckSmartClientVersionCompleted;

	public event GetProductInfoCompletedEventHandler GetProductInfoCompleted;

	public event GetSystemLicenseInfoCompletedEventHandler GetSystemLicenseInfoCompleted;

	public event GetUserInfoCompletedEventHandler GetUserInfoCompleted;

	public event GetViewGroupsCompletedEventHandler GetViewGroupsCompleted;

	public event GetViewGroupDataCompletedEventHandler GetViewGroupDataCompleted;

	public event SetViewGroupDataCompletedEventHandler SetViewGroupDataCompleted;

	public event GetCustomSettingDataGlobalCompletedEventHandler GetCustomSettingDataGlobalCompleted;

	public event GetCustomSettingDataUserCompletedEventHandler GetCustomSettingDataUserCompleted;

	public event SetCustomSettingDataGlobalCompletedEventHandler SetCustomSettingDataGlobalCompleted;

	public event SetCustomSettingDataUserCompletedEventHandler SetCustomSettingDataUserCompleted;

	public event EventTriggerCompletedEventHandler EventTriggerCompleted;

	public event EventTriggerWithMetadataCompletedEventHandler EventTriggerWithMetadataCompleted;

	public event AlertsGetAroundWithSpanCompletedEventHandler AlertsGetAroundWithSpanCompleted;

	public event MatrixMonitorActivateCameraCompletedEventHandler MatrixMonitorActivateCameraCompleted;

	public event MatrixMonitorActivateScriptCompletedEventHandler MatrixMonitorActivateScriptCompleted;

	public event QueryChangesCompletedEventHandler QueryChangesCompleted;

	public event QueryRecorderInfoCompletedEventHandler QueryRecorderInfoCompleted;

	public event GetConfigurationCompletedEventHandler GetConfigurationCompleted;

	public event BookmarkGetNewReferenceCompletedEventHandler BookmarkGetNewReferenceCompleted;

	public event BookmarkCreateCompletedEventHandler BookmarkCreateCompleted;

	public event BookmarkUpdateCompletedEventHandler BookmarkUpdateCompleted;

	public event BookmarkDeleteCompletedEventHandler BookmarkDeleteCompleted;

	public event BookmarkGetCompletedEventHandler BookmarkGetCompleted;

	public event BookmarkSearchTimeCompletedEventHandler BookmarkSearchTimeCompleted;

	public event BookmarkSearchFromBookmarkCompletedEventHandler BookmarkSearchFromBookmarkCompleted;

	public event AuthorizeCompletedEventHandler AuthorizeCompleted;

	public event MarkedDataGetNewReferenceCompletedEventHandler MarkedDataGetNewReferenceCompleted;

	public event MarkedDataCreateCompletedEventHandler MarkedDataCreateCompleted;

	public event MarkedDataUpdateCompletedEventHandler MarkedDataUpdateCompleted;

	public event MarkedDataDeleteCompletedEventHandler MarkedDataDeleteCompleted;

	public event MarkedDataGetCompletedEventHandler MarkedDataGetCompleted;

	public event MarkedDataSearchCompletedEventHandler MarkedDataSearchCompleted;

	public event GetClientProfileCompletedEventHandler GetClientProfileCompleted;

	public event GetPresetsCompletedEventHandler GetPresetsCompleted;

	public event DeletePresetCompletedEventHandler DeletePresetCompleted;

	public event UpdatePresetNameCompletedEventHandler UpdatePresetNameCompleted;

	public event UpdatePresetPositionCompletedEventHandler UpdatePresetPositionCompleted;

	public event CreatePresetCompletedEventHandler CreatePresetCompleted;

	public event OrderPresetsCompletedEventHandler OrderPresetsCompleted;

	public ServerCommandService()
	{
		base.Url = "http://localhost/ServerAPIGenerator/ServerCommandService.asmx";
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetVersion", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
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

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetServerVersion", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetServerVersion()
	{
		return (string)Invoke("GetServerVersion", new object[0])[0];
	}

	public IAsyncResult BeginGetServerVersion(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetServerVersion", new object[0], callback, asyncState);
	}

	public string EndGetServerVersion(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void GetServerVersionAsync()
	{
		GetServerVersionAsync(null);
	}

	public void GetServerVersionAsync(object userState)
	{
		if (GetServerVersionOperationCompleted == null)
		{
			GetServerVersionOperationCompleted = OnGetServerVersionOperationCompleted;
		}
		InvokeAsync("GetServerVersion", new object[0], GetServerVersionOperationCompleted, userState);
	}

	private void OnGetServerVersionOperationCompleted(object arg)
	{
		if (this.GetServerVersionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetServerVersionCompleted(this, new GetServerVersionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/Login", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public LoginInfo Login(Guid instanceId, string currentToken)
	{
		return (LoginInfo)Invoke("Login", new object[2] { instanceId, currentToken })[0];
	}

	public IAsyncResult BeginLogin(Guid instanceId, string currentToken, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("Login", new object[2] { instanceId, currentToken }, callback, asyncState);
	}

	public LoginInfo EndLogin(IAsyncResult asyncResult)
	{
		return (LoginInfo)EndInvoke(asyncResult)[0];
	}

	public void LoginAsync(Guid instanceId, string currentToken)
	{
		LoginAsync(instanceId, currentToken, null);
	}

	public void LoginAsync(Guid instanceId, string currentToken, object userState)
	{
		if (LoginOperationCompleted == null)
		{
			LoginOperationCompleted = OnLoginOperationCompleted;
		}
		InvokeAsync("Login", new object[2] { instanceId, currentToken }, LoginOperationCompleted, userState);
	}

	private void OnLoginOperationCompleted(object arg)
	{
		if (this.LoginCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.LoginCompleted(this, new LoginCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/Logout", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void Logout(Guid instanceId, string currentToken)
	{
		Invoke("Logout", new object[2] { instanceId, currentToken });
	}

	public IAsyncResult BeginLogout(Guid instanceId, string currentToken, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("Logout", new object[2] { instanceId, currentToken }, callback, asyncState);
	}

	public void EndLogout(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void LogoutAsync(Guid instanceId, string currentToken)
	{
		LogoutAsync(instanceId, currentToken, null);
	}

	public void LogoutAsync(Guid instanceId, string currentToken, object userState)
	{
		if (LogoutOperationCompleted == null)
		{
			LogoutOperationCompleted = OnLogoutOperationCompleted;
		}
		InvokeAsync("Logout", new object[2] { instanceId, currentToken }, LogoutOperationCompleted, userState);
	}

	private void OnLogoutOperationCompleted(object arg)
	{
		if (this.LogoutCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.LogoutCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetDevicesDisabled", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlArrayItem(IsNullable = false)]
	public DeviceDisabledInfo[] GetDevicesDisabled()
	{
		return (DeviceDisabledInfo[])Invoke("GetDevicesDisabled", new object[0])[0];
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetVmoServicesList", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlArrayItem(IsNullable = false)]
	public DictionaryEntry[] GetVmoServicesList()
	{
		return (DictionaryEntry[])Invoke("GetVmoServicesList", new object[0])[0];
	}

	public IAsyncResult BeginGetVmoServicesList(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetVmoServicesList", new object[0], callback, asyncState);
	}

	public DictionaryEntry[] EndGetVmoServicesList(IAsyncResult asyncResult)
	{
		return (DictionaryEntry[])EndInvoke(asyncResult)[0];
	}

	public void GetVmoServicesListAsync()
	{
		GetVmoServicesListAsync(null);
	}

	public void GetVmoServicesListAsync(object userState)
	{
		if (GetVmoServicesListOperationCompleted == null)
		{
			GetVmoServicesListOperationCompleted = OnGetVmoServicesListOperationCompleted;
		}
		InvokeAsync("GetVmoServicesList", new object[0], GetVmoServicesListOperationCompleted, userState);
	}

	private void OnGetVmoServicesListOperationCompleted(object arg)
	{
		if (this.GetVmoServicesListCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetVmoServicesListCompleted(this, new GetVmoServicesListCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetSmartClientVersion", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public SmartClientVersion GetSmartClientVersion()
	{
		return (SmartClientVersion)Invoke("GetSmartClientVersion", new object[0])[0];
	}

	public IAsyncResult BeginGetSmartClientVersion(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetSmartClientVersion", new object[0], callback, asyncState);
	}

	public SmartClientVersion EndGetSmartClientVersion(IAsyncResult asyncResult)
	{
		return (SmartClientVersion)EndInvoke(asyncResult)[0];
	}

	public void GetSmartClientVersionAsync()
	{
		GetSmartClientVersionAsync(null);
	}

	public void GetSmartClientVersionAsync(object userState)
	{
		if (GetSmartClientVersionOperationCompleted == null)
		{
			GetSmartClientVersionOperationCompleted = OnGetSmartClientVersionOperationCompleted;
		}
		InvokeAsync("GetSmartClientVersion", new object[0], GetSmartClientVersionOperationCompleted, userState);
	}

	private void OnGetSmartClientVersionOperationCompleted(object arg)
	{
		if (this.GetSmartClientVersionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetSmartClientVersionCompleted(this, new GetSmartClientVersionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/CheckSmartClientVersion", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public SmartClientVersion CheckSmartClientVersion(int major, int minor, string revision)
	{
		return (SmartClientVersion)Invoke("CheckSmartClientVersion", new object[3] { major, minor, revision })[0];
	}

	public IAsyncResult BeginCheckSmartClientVersion(int major, int minor, string revision, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CheckSmartClientVersion", new object[3] { major, minor, revision }, callback, asyncState);
	}

	public SmartClientVersion EndCheckSmartClientVersion(IAsyncResult asyncResult)
	{
		return (SmartClientVersion)EndInvoke(asyncResult)[0];
	}

	public void CheckSmartClientVersionAsync(int major, int minor, string revision)
	{
		CheckSmartClientVersionAsync(major, minor, revision, null);
	}

	public void CheckSmartClientVersionAsync(int major, int minor, string revision, object userState)
	{
		if (CheckSmartClientVersionOperationCompleted == null)
		{
			CheckSmartClientVersionOperationCompleted = OnCheckSmartClientVersionOperationCompleted;
		}
		InvokeAsync("CheckSmartClientVersion", new object[3] { major, minor, revision }, CheckSmartClientVersionOperationCompleted, userState);
	}

	private void OnCheckSmartClientVersionOperationCompleted(object arg)
	{
		if (this.CheckSmartClientVersionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CheckSmartClientVersionCompleted(this, new CheckSmartClientVersionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetProductInfo", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public ProductInfo GetProductInfo()
	{
		return (ProductInfo)Invoke("GetProductInfo", new object[0])[0];
	}

	public IAsyncResult BeginGetProductInfo(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetProductInfo", new object[0], callback, asyncState);
	}

	public ProductInfo EndGetProductInfo(IAsyncResult asyncResult)
	{
		return (ProductInfo)EndInvoke(asyncResult)[0];
	}

	public void GetProductInfoAsync()
	{
		GetProductInfoAsync(null);
	}

	public void GetProductInfoAsync(object userState)
	{
		if (GetProductInfoOperationCompleted == null)
		{
			GetProductInfoOperationCompleted = OnGetProductInfoOperationCompleted;
		}
		InvokeAsync("GetProductInfo", new object[0], GetProductInfoOperationCompleted, userState);
	}

	private void OnGetProductInfoOperationCompleted(object arg)
	{
		if (this.GetProductInfoCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetProductInfoCompleted(this, new GetProductInfoCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/IsOnline", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public bool IsOnline()
	{
		return (bool)Invoke("IsOnline", new object[0])[0];
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetSystemLicenseInfo", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public SystemLicenseInfo GetSystemLicenseInfo()
	{
		return (SystemLicenseInfo)Invoke("GetSystemLicenseInfo", new object[0])[0];
	}

	public IAsyncResult BeginGetSystemLicenseInfo(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetSystemLicenseInfo", new object[0], callback, asyncState);
	}

	public SystemLicenseInfo EndGetSystemLicenseInfo(IAsyncResult asyncResult)
	{
		return (SystemLicenseInfo)EndInvoke(asyncResult)[0];
	}

	public void GetSystemLicenseInfoAsync()
	{
		GetSystemLicenseInfoAsync(null);
	}

	public void GetSystemLicenseInfoAsync(object userState)
	{
		if (GetSystemLicenseInfoOperationCompleted == null)
		{
			GetSystemLicenseInfoOperationCompleted = OnGetSystemLicenseInfoOperationCompleted;
		}
		InvokeAsync("GetSystemLicenseInfo", new object[0], GetSystemLicenseInfoOperationCompleted, userState);
	}

	private void OnGetSystemLicenseInfoOperationCompleted(object arg)
	{
		if (this.GetSystemLicenseInfoCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetSystemLicenseInfoCompleted(this, new GetSystemLicenseInfoCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetUserInfo", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public UserInfo GetUserInfo(string token)
	{
		return (UserInfo)Invoke("GetUserInfo", new object[1] { token })[0];
	}

	public IAsyncResult BeginGetUserInfo(string token, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetUserInfo", new object[1] { token }, callback, asyncState);
	}

	public UserInfo EndGetUserInfo(IAsyncResult asyncResult)
	{
		return (UserInfo)EndInvoke(asyncResult)[0];
	}

	public void GetUserInfoAsync(string token)
	{
		GetUserInfoAsync(token, null);
	}

	public void GetUserInfoAsync(string token, object userState)
	{
		if (GetUserInfoOperationCompleted == null)
		{
			GetUserInfoOperationCompleted = OnGetUserInfoOperationCompleted;
		}
		InvokeAsync("GetUserInfo", new object[1] { token }, GetUserInfoOperationCompleted, userState);
	}

	private void OnGetUserInfoOperationCompleted(object arg)
	{
		if (this.GetUserInfoCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetUserInfoCompleted(this, new GetUserInfoCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetViewGroups", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public ViewGroupInfo[] GetViewGroups(string token)
	{
		return (ViewGroupInfo[])Invoke("GetViewGroups", new object[1] { token })[0];
	}

	public IAsyncResult BeginGetViewGroups(string token, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetViewGroups", new object[1] { token }, callback, asyncState);
	}

	public ViewGroupInfo[] EndGetViewGroups(IAsyncResult asyncResult)
	{
		return (ViewGroupInfo[])EndInvoke(asyncResult)[0];
	}

	public void GetViewGroupsAsync(string token)
	{
		GetViewGroupsAsync(token, null);
	}

	public void GetViewGroupsAsync(string token, object userState)
	{
		if (GetViewGroupsOperationCompleted == null)
		{
			GetViewGroupsOperationCompleted = OnGetViewGroupsOperationCompleted;
		}
		InvokeAsync("GetViewGroups", new object[1] { token }, GetViewGroupsOperationCompleted, userState);
	}

	private void OnGetViewGroupsOperationCompleted(object arg)
	{
		if (this.GetViewGroupsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetViewGroupsCompleted(this, new GetViewGroupsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetViewGroupData", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public ViewGroup GetViewGroupData(string token, Guid viewGroupId)
	{
		return (ViewGroup)Invoke("GetViewGroupData", new object[2] { token, viewGroupId })[0];
	}

	public IAsyncResult BeginGetViewGroupData(string token, Guid viewGroupId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetViewGroupData", new object[2] { token, viewGroupId }, callback, asyncState);
	}

	public ViewGroup EndGetViewGroupData(IAsyncResult asyncResult)
	{
		return (ViewGroup)EndInvoke(asyncResult)[0];
	}

	public void GetViewGroupDataAsync(string token, Guid viewGroupId)
	{
		GetViewGroupDataAsync(token, viewGroupId, null);
	}

	public void GetViewGroupDataAsync(string token, Guid viewGroupId, object userState)
	{
		if (GetViewGroupDataOperationCompleted == null)
		{
			GetViewGroupDataOperationCompleted = OnGetViewGroupDataOperationCompleted;
		}
		InvokeAsync("GetViewGroupData", new object[2] { token, viewGroupId }, GetViewGroupDataOperationCompleted, userState);
	}

	private void OnGetViewGroupDataOperationCompleted(object arg)
	{
		if (this.GetViewGroupDataCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetViewGroupDataCompleted(this, new GetViewGroupDataCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/SetViewGroupData", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public SetViewGroupDataResultType SetViewGroupData(string token, Guid viewGroupId, ViewGroupData viewGroupdata)
	{
		return (SetViewGroupDataResultType)Invoke("SetViewGroupData", new object[3] { token, viewGroupId, viewGroupdata })[0];
	}

	public IAsyncResult BeginSetViewGroupData(string token, Guid viewGroupId, ViewGroupData viewGroupdata, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SetViewGroupData", new object[3] { token, viewGroupId, viewGroupdata }, callback, asyncState);
	}

	public SetViewGroupDataResultType EndSetViewGroupData(IAsyncResult asyncResult)
	{
		return (SetViewGroupDataResultType)EndInvoke(asyncResult)[0];
	}

	public void SetViewGroupDataAsync(string token, Guid viewGroupId, ViewGroupData viewGroupdata)
	{
		SetViewGroupDataAsync(token, viewGroupId, viewGroupdata, null);
	}

	public void SetViewGroupDataAsync(string token, Guid viewGroupId, ViewGroupData viewGroupdata, object userState)
	{
		if (SetViewGroupDataOperationCompleted == null)
		{
			SetViewGroupDataOperationCompleted = OnSetViewGroupDataOperationCompleted;
		}
		InvokeAsync("SetViewGroupData", new object[3] { token, viewGroupId, viewGroupdata }, SetViewGroupDataOperationCompleted, userState);
	}

	private void OnSetViewGroupDataOperationCompleted(object arg)
	{
		if (this.SetViewGroupDataCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SetViewGroupDataCompleted(this, new SetViewGroupDataCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetCustomSettingDataGlobal", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public CustomSetting GetCustomSettingDataGlobal(string token, Guid customSettingId)
	{
		return (CustomSetting)Invoke("GetCustomSettingDataGlobal", new object[2] { token, customSettingId })[0];
	}

	public IAsyncResult BeginGetCustomSettingDataGlobal(string token, Guid customSettingId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetCustomSettingDataGlobal", new object[2] { token, customSettingId }, callback, asyncState);
	}

	public CustomSetting EndGetCustomSettingDataGlobal(IAsyncResult asyncResult)
	{
		return (CustomSetting)EndInvoke(asyncResult)[0];
	}

	public void GetCustomSettingDataGlobalAsync(string token, Guid customSettingId)
	{
		GetCustomSettingDataGlobalAsync(token, customSettingId, null);
	}

	public void GetCustomSettingDataGlobalAsync(string token, Guid customSettingId, object userState)
	{
		if (GetCustomSettingDataGlobalOperationCompleted == null)
		{
			GetCustomSettingDataGlobalOperationCompleted = OnGetCustomSettingDataGlobalOperationCompleted;
		}
		InvokeAsync("GetCustomSettingDataGlobal", new object[2] { token, customSettingId }, GetCustomSettingDataGlobalOperationCompleted, userState);
	}

	private void OnGetCustomSettingDataGlobalOperationCompleted(object arg)
	{
		if (this.GetCustomSettingDataGlobalCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetCustomSettingDataGlobalCompleted(this, new GetCustomSettingDataGlobalCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetCustomSettingDataUser", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public CustomSetting GetCustomSettingDataUser(string token, Guid customSettingId)
	{
		return (CustomSetting)Invoke("GetCustomSettingDataUser", new object[2] { token, customSettingId })[0];
	}

	public IAsyncResult BeginGetCustomSettingDataUser(string token, Guid customSettingId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetCustomSettingDataUser", new object[2] { token, customSettingId }, callback, asyncState);
	}

	public CustomSetting EndGetCustomSettingDataUser(IAsyncResult asyncResult)
	{
		return (CustomSetting)EndInvoke(asyncResult)[0];
	}

	public void GetCustomSettingDataUserAsync(string token, Guid customSettingId)
	{
		GetCustomSettingDataUserAsync(token, customSettingId, null);
	}

	public void GetCustomSettingDataUserAsync(string token, Guid customSettingId, object userState)
	{
		if (GetCustomSettingDataUserOperationCompleted == null)
		{
			GetCustomSettingDataUserOperationCompleted = OnGetCustomSettingDataUserOperationCompleted;
		}
		InvokeAsync("GetCustomSettingDataUser", new object[2] { token, customSettingId }, GetCustomSettingDataUserOperationCompleted, userState);
	}

	private void OnGetCustomSettingDataUserOperationCompleted(object arg)
	{
		if (this.GetCustomSettingDataUserCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetCustomSettingDataUserCompleted(this, new GetCustomSettingDataUserCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/SetCustomSettingDataGlobal", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public SetCustomSettingResultType SetCustomSettingDataGlobal(string token, Guid customSettingId, CustomSettingData customSettingData, bool forceOverride)
	{
		return (SetCustomSettingResultType)Invoke("SetCustomSettingDataGlobal", new object[4] { token, customSettingId, customSettingData, forceOverride })[0];
	}

	public IAsyncResult BeginSetCustomSettingDataGlobal(string token, Guid customSettingId, CustomSettingData customSettingData, bool forceOverride, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SetCustomSettingDataGlobal", new object[4] { token, customSettingId, customSettingData, forceOverride }, callback, asyncState);
	}

	public SetCustomSettingResultType EndSetCustomSettingDataGlobal(IAsyncResult asyncResult)
	{
		return (SetCustomSettingResultType)EndInvoke(asyncResult)[0];
	}

	public void SetCustomSettingDataGlobalAsync(string token, Guid customSettingId, CustomSettingData customSettingData, bool forceOverride)
	{
		SetCustomSettingDataGlobalAsync(token, customSettingId, customSettingData, forceOverride, null);
	}

	public void SetCustomSettingDataGlobalAsync(string token, Guid customSettingId, CustomSettingData customSettingData, bool forceOverride, object userState)
	{
		if (SetCustomSettingDataGlobalOperationCompleted == null)
		{
			SetCustomSettingDataGlobalOperationCompleted = OnSetCustomSettingDataGlobalOperationCompleted;
		}
		InvokeAsync("SetCustomSettingDataGlobal", new object[4] { token, customSettingId, customSettingData, forceOverride }, SetCustomSettingDataGlobalOperationCompleted, userState);
	}

	private void OnSetCustomSettingDataGlobalOperationCompleted(object arg)
	{
		if (this.SetCustomSettingDataGlobalCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SetCustomSettingDataGlobalCompleted(this, new SetCustomSettingDataGlobalCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/SetCustomSettingDataUser", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public SetCustomSettingResultType SetCustomSettingDataUser(string token, Guid customSettingId, CustomSettingData customSettingData, bool forceOverride)
	{
		return (SetCustomSettingResultType)Invoke("SetCustomSettingDataUser", new object[4] { token, customSettingId, customSettingData, forceOverride })[0];
	}

	public IAsyncResult BeginSetCustomSettingDataUser(string token, Guid customSettingId, CustomSettingData customSettingData, bool forceOverride, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SetCustomSettingDataUser", new object[4] { token, customSettingId, customSettingData, forceOverride }, callback, asyncState);
	}

	public SetCustomSettingResultType EndSetCustomSettingDataUser(IAsyncResult asyncResult)
	{
		return (SetCustomSettingResultType)EndInvoke(asyncResult)[0];
	}

	public void SetCustomSettingDataUserAsync(string token, Guid customSettingId, CustomSettingData customSettingData, bool forceOverride)
	{
		SetCustomSettingDataUserAsync(token, customSettingId, customSettingData, forceOverride, null);
	}

	public void SetCustomSettingDataUserAsync(string token, Guid customSettingId, CustomSettingData customSettingData, bool forceOverride, object userState)
	{
		if (SetCustomSettingDataUserOperationCompleted == null)
		{
			SetCustomSettingDataUserOperationCompleted = OnSetCustomSettingDataUserOperationCompleted;
		}
		InvokeAsync("SetCustomSettingDataUser", new object[4] { token, customSettingId, customSettingData, forceOverride }, SetCustomSettingDataUserOperationCompleted, userState);
	}

	private void OnSetCustomSettingDataUserOperationCompleted(object arg)
	{
		if (this.SetCustomSettingDataUserCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SetCustomSettingDataUserCompleted(this, new SetCustomSettingDataUserCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/EventTrigger", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void EventTrigger(string token, Guid eventId)
	{
		Invoke("EventTrigger", new object[2] { token, eventId });
	}

	public IAsyncResult BeginEventTrigger(string token, Guid eventId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("EventTrigger", new object[2] { token, eventId }, callback, asyncState);
	}

	public void EndEventTrigger(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void EventTriggerAsync(string token, Guid eventId)
	{
		EventTriggerAsync(token, eventId, null);
	}

	public void EventTriggerAsync(string token, Guid eventId, object userState)
	{
		if (EventTriggerOperationCompleted == null)
		{
			EventTriggerOperationCompleted = OnEventTriggerOperationCompleted;
		}
		InvokeAsync("EventTrigger", new object[2] { token, eventId }, EventTriggerOperationCompleted, userState);
	}

	private void OnEventTriggerOperationCompleted(object arg)
	{
		if (this.EventTriggerCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.EventTriggerCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/EventTriggerWithMetadata", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void EventTriggerWithMetadata(string token, Guid eventId, KeyValue[] metadata)
	{
		Invoke("EventTriggerWithMetadata", new object[3] { token, eventId, metadata });
	}

	public IAsyncResult BeginEventTriggerWithMetadata(string token, Guid eventId, KeyValue[] metadata, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("EventTriggerWithMetadata", new object[3] { token, eventId, metadata }, callback, asyncState);
	}

	public void EndEventTriggerWithMetadata(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void EventTriggerWithMetadataAsync(string token, Guid eventId, KeyValue[] metadata)
	{
		EventTriggerWithMetadataAsync(token, eventId, metadata, null);
	}

	public void EventTriggerWithMetadataAsync(string token, Guid eventId, KeyValue[] metadata, object userState)
	{
		if (EventTriggerWithMetadataOperationCompleted == null)
		{
			EventTriggerWithMetadataOperationCompleted = OnEventTriggerWithMetadataOperationCompleted;
		}
		InvokeAsync("EventTriggerWithMetadata", new object[3] { token, eventId, metadata }, EventTriggerWithMetadataOperationCompleted, userState);
	}

	private void OnEventTriggerWithMetadataOperationCompleted(object arg)
	{
		if (this.EventTriggerWithMetadataCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.EventTriggerWithMetadataCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/AlertsGetAroundWithSpan", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public AlertOccurrence[] AlertsGetAroundWithSpan(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter)
	{
		return (AlertOccurrence[])Invoke("AlertsGetAroundWithSpan", new object[7] { token, alertIds, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter })[0];
	}

	public IAsyncResult BeginAlertsGetAroundWithSpan(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("AlertsGetAroundWithSpan", new object[7] { token, alertIds, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter }, callback, asyncState);
	}

	public AlertOccurrence[] EndAlertsGetAroundWithSpan(IAsyncResult asyncResult)
	{
		return (AlertOccurrence[])EndInvoke(asyncResult)[0];
	}

	public void AlertsGetAroundWithSpanAsync(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter)
	{
		AlertsGetAroundWithSpanAsync(token, alertIds, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, null);
	}

	public void AlertsGetAroundWithSpanAsync(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter, object userState)
	{
		if (AlertsGetAroundWithSpanOperationCompleted == null)
		{
			AlertsGetAroundWithSpanOperationCompleted = OnAlertsGetAroundWithSpanOperationCompleted;
		}
		InvokeAsync("AlertsGetAroundWithSpan", new object[7] { token, alertIds, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter }, AlertsGetAroundWithSpanOperationCompleted, userState);
	}

	private void OnAlertsGetAroundWithSpanOperationCompleted(object arg)
	{
		if (this.AlertsGetAroundWithSpanCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.AlertsGetAroundWithSpanCompleted(this, new AlertsGetAroundWithSpanCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/MatrixMonitorActivateCamera", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void MatrixMonitorActivateCamera(string token, Guid matrixMonitorId, Guid deviceId)
	{
		Invoke("MatrixMonitorActivateCamera", new object[3] { token, matrixMonitorId, deviceId });
	}

	public IAsyncResult BeginMatrixMonitorActivateCamera(string token, Guid matrixMonitorId, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("MatrixMonitorActivateCamera", new object[3] { token, matrixMonitorId, deviceId }, callback, asyncState);
	}

	public void EndMatrixMonitorActivateCamera(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void MatrixMonitorActivateCameraAsync(string token, Guid matrixMonitorId, Guid deviceId)
	{
		MatrixMonitorActivateCameraAsync(token, matrixMonitorId, deviceId, null);
	}

	public void MatrixMonitorActivateCameraAsync(string token, Guid matrixMonitorId, Guid deviceId, object userState)
	{
		if (MatrixMonitorActivateCameraOperationCompleted == null)
		{
			MatrixMonitorActivateCameraOperationCompleted = OnMatrixMonitorActivateCameraOperationCompleted;
		}
		InvokeAsync("MatrixMonitorActivateCamera", new object[3] { token, matrixMonitorId, deviceId }, MatrixMonitorActivateCameraOperationCompleted, userState);
	}

	private void OnMatrixMonitorActivateCameraOperationCompleted(object arg)
	{
		if (this.MatrixMonitorActivateCameraCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.MatrixMonitorActivateCameraCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/MatrixMonitorActivateScript", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void MatrixMonitorActivateScript(string token, Guid matrixMonitorId, string script)
	{
		Invoke("MatrixMonitorActivateScript", new object[3] { token, matrixMonitorId, script });
	}

	public IAsyncResult BeginMatrixMonitorActivateScript(string token, Guid matrixMonitorId, string script, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("MatrixMonitorActivateScript", new object[3] { token, matrixMonitorId, script }, callback, asyncState);
	}

	public void EndMatrixMonitorActivateScript(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void MatrixMonitorActivateScriptAsync(string token, Guid matrixMonitorId, string script)
	{
		MatrixMonitorActivateScriptAsync(token, matrixMonitorId, script, null);
	}

	public void MatrixMonitorActivateScriptAsync(string token, Guid matrixMonitorId, string script, object userState)
	{
		if (MatrixMonitorActivateScriptOperationCompleted == null)
		{
			MatrixMonitorActivateScriptOperationCompleted = OnMatrixMonitorActivateScriptOperationCompleted;
		}
		InvokeAsync("MatrixMonitorActivateScript", new object[3] { token, matrixMonitorId, script }, MatrixMonitorActivateScriptOperationCompleted, userState);
	}

	private void OnMatrixMonitorActivateScriptOperationCompleted(object arg)
	{
		if (this.MatrixMonitorActivateScriptCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.MatrixMonitorActivateScriptCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/QueryChanges", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public Change[] QueryChanges(string token, DateTime changesFrom)
	{
		return (Change[])Invoke("QueryChanges", new object[2] { token, changesFrom })[0];
	}

	public IAsyncResult BeginQueryChanges(string token, DateTime changesFrom, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("QueryChanges", new object[2] { token, changesFrom }, callback, asyncState);
	}

	public Change[] EndQueryChanges(IAsyncResult asyncResult)
	{
		return (Change[])EndInvoke(asyncResult)[0];
	}

	public void QueryChangesAsync(string token, DateTime changesFrom)
	{
		QueryChangesAsync(token, changesFrom, null);
	}

	public void QueryChangesAsync(string token, DateTime changesFrom, object userState)
	{
		if (QueryChangesOperationCompleted == null)
		{
			QueryChangesOperationCompleted = OnQueryChangesOperationCompleted;
		}
		InvokeAsync("QueryChanges", new object[2] { token, changesFrom }, QueryChangesOperationCompleted, userState);
	}

	private void OnQueryChangesOperationCompleted(object arg)
	{
		if (this.QueryChangesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.QueryChangesCompleted(this, new QueryChangesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/QueryRecorderInfo", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public RecorderInfo QueryRecorderInfo(string token, Guid recorderId)
	{
		return (RecorderInfo)Invoke("QueryRecorderInfo", new object[2] { token, recorderId })[0];
	}

	public IAsyncResult BeginQueryRecorderInfo(string token, Guid recorderId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("QueryRecorderInfo", new object[2] { token, recorderId }, callback, asyncState);
	}

	public RecorderInfo EndQueryRecorderInfo(IAsyncResult asyncResult)
	{
		return (RecorderInfo)EndInvoke(asyncResult)[0];
	}

	public void QueryRecorderInfoAsync(string token, Guid recorderId)
	{
		QueryRecorderInfoAsync(token, recorderId, null);
	}

	public void QueryRecorderInfoAsync(string token, Guid recorderId, object userState)
	{
		if (QueryRecorderInfoOperationCompleted == null)
		{
			QueryRecorderInfoOperationCompleted = OnQueryRecorderInfoOperationCompleted;
		}
		InvokeAsync("QueryRecorderInfo", new object[2] { token, recorderId }, QueryRecorderInfoOperationCompleted, userState);
	}

	private void OnQueryRecorderInfoOperationCompleted(object arg)
	{
		if (this.QueryRecorderInfoCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.QueryRecorderInfoCompleted(this, new QueryRecorderInfoCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetConfiguration", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public ConfigurationInfo GetConfiguration(string token)
	{
		return (ConfigurationInfo)Invoke("GetConfiguration", new object[1] { token })[0];
	}

	public IAsyncResult BeginGetConfiguration(string token, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetConfiguration", new object[1] { token }, callback, asyncState);
	}

	public ConfigurationInfo EndGetConfiguration(IAsyncResult asyncResult)
	{
		return (ConfigurationInfo)EndInvoke(asyncResult)[0];
	}

	public void GetConfigurationAsync(string token)
	{
		GetConfigurationAsync(token, null);
	}

	public void GetConfigurationAsync(string token, object userState)
	{
		if (GetConfigurationOperationCompleted == null)
		{
			GetConfigurationOperationCompleted = OnGetConfigurationOperationCompleted;
		}
		InvokeAsync("GetConfiguration", new object[1] { token }, GetConfigurationOperationCompleted, userState);
	}

	private void OnGetConfigurationOperationCompleted(object arg)
	{
		if (this.GetConfigurationCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetConfigurationCompleted(this, new GetConfigurationCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetConfigurationHardware", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public HardwareInfo[] GetConfigurationHardware(string token, Guid[] hardwareIds)
	{
		return (HardwareInfo[])Invoke("GetConfigurationHardware", new object[2] { token, hardwareIds })[0];
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetConfigurationRecorders", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public RecorderInfo[] GetConfigurationRecorders(string token, Guid[] recorderIds)
	{
		return (RecorderInfo[])Invoke("GetConfigurationRecorders", new object[2] { token, recorderIds })[0];
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/BookmarkGetNewReference", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public BookmarkReference BookmarkGetNewReference(string token, Guid deviceId, bool live)
	{
		return (BookmarkReference)Invoke("BookmarkGetNewReference", new object[3] { token, deviceId, live })[0];
	}

	public IAsyncResult BeginBookmarkGetNewReference(string token, Guid deviceId, bool live, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("BookmarkGetNewReference", new object[3] { token, deviceId, live }, callback, asyncState);
	}

	public BookmarkReference EndBookmarkGetNewReference(IAsyncResult asyncResult)
	{
		return (BookmarkReference)EndInvoke(asyncResult)[0];
	}

	public void BookmarkGetNewReferenceAsync(string token, Guid deviceId, bool live)
	{
		BookmarkGetNewReferenceAsync(token, deviceId, live, null);
	}

	public void BookmarkGetNewReferenceAsync(string token, Guid deviceId, bool live, object userState)
	{
		if (BookmarkGetNewReferenceOperationCompleted == null)
		{
			BookmarkGetNewReferenceOperationCompleted = OnBookmarkGetNewReferenceOperationCompleted;
		}
		InvokeAsync("BookmarkGetNewReference", new object[3] { token, deviceId, live }, BookmarkGetNewReferenceOperationCompleted, userState);
	}

	private void OnBookmarkGetNewReferenceOperationCompleted(object arg)
	{
		if (this.BookmarkGetNewReferenceCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.BookmarkGetNewReferenceCompleted(this, new BookmarkGetNewReferenceCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/BookmarkCreate", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public Bookmark BookmarkCreate(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return (Bookmark)Invoke("BookmarkCreate", new object[8] { token, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description })[0];
	}

	public IAsyncResult BeginBookmarkCreate(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("BookmarkCreate", new object[8] { token, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description }, callback, asyncState);
	}

	public Bookmark EndBookmarkCreate(IAsyncResult asyncResult)
	{
		return (Bookmark)EndInvoke(asyncResult)[0];
	}

	public void BookmarkCreateAsync(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		BookmarkCreateAsync(token, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description, null);
	}

	public void BookmarkCreateAsync(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description, object userState)
	{
		if (BookmarkCreateOperationCompleted == null)
		{
			BookmarkCreateOperationCompleted = OnBookmarkCreateOperationCompleted;
		}
		InvokeAsync("BookmarkCreate", new object[8] { token, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description }, BookmarkCreateOperationCompleted, userState);
	}

	private void OnBookmarkCreateOperationCompleted(object arg)
	{
		if (this.BookmarkCreateCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.BookmarkCreateCompleted(this, new BookmarkCreateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/BookmarkUpdate", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public Bookmark BookmarkUpdate(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return (Bookmark)Invoke("BookmarkUpdate", new object[9] { token, bookmarkId, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description })[0];
	}

	public IAsyncResult BeginBookmarkUpdate(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("BookmarkUpdate", new object[9] { token, bookmarkId, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description }, callback, asyncState);
	}

	public Bookmark EndBookmarkUpdate(IAsyncResult asyncResult)
	{
		return (Bookmark)EndInvoke(asyncResult)[0];
	}

	public void BookmarkUpdateAsync(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		BookmarkUpdateAsync(token, bookmarkId, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description, null);
	}

	public void BookmarkUpdateAsync(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description, object userState)
	{
		if (BookmarkUpdateOperationCompleted == null)
		{
			BookmarkUpdateOperationCompleted = OnBookmarkUpdateOperationCompleted;
		}
		InvokeAsync("BookmarkUpdate", new object[9] { token, bookmarkId, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description }, BookmarkUpdateOperationCompleted, userState);
	}

	private void OnBookmarkUpdateOperationCompleted(object arg)
	{
		if (this.BookmarkUpdateCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.BookmarkUpdateCompleted(this, new BookmarkUpdateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/BookmarkDelete", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void BookmarkDelete(string token, Guid bookmarkId)
	{
		Invoke("BookmarkDelete", new object[2] { token, bookmarkId });
	}

	public IAsyncResult BeginBookmarkDelete(string token, Guid bookmarkId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("BookmarkDelete", new object[2] { token, bookmarkId }, callback, asyncState);
	}

	public void EndBookmarkDelete(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void BookmarkDeleteAsync(string token, Guid bookmarkId)
	{
		BookmarkDeleteAsync(token, bookmarkId, null);
	}

	public void BookmarkDeleteAsync(string token, Guid bookmarkId, object userState)
	{
		if (BookmarkDeleteOperationCompleted == null)
		{
			BookmarkDeleteOperationCompleted = OnBookmarkDeleteOperationCompleted;
		}
		InvokeAsync("BookmarkDelete", new object[2] { token, bookmarkId }, BookmarkDeleteOperationCompleted, userState);
	}

	private void OnBookmarkDeleteOperationCompleted(object arg)
	{
		if (this.BookmarkDeleteCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.BookmarkDeleteCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/BookmarkGet", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public Bookmark BookmarkGet(string token, Guid bookmarkId)
	{
		return (Bookmark)Invoke("BookmarkGet", new object[2] { token, bookmarkId })[0];
	}

	public IAsyncResult BeginBookmarkGet(string token, Guid bookmarkId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("BookmarkGet", new object[2] { token, bookmarkId }, callback, asyncState);
	}

	public Bookmark EndBookmarkGet(IAsyncResult asyncResult)
	{
		return (Bookmark)EndInvoke(asyncResult)[0];
	}

	public void BookmarkGetAsync(string token, Guid bookmarkId)
	{
		BookmarkGetAsync(token, bookmarkId, null);
	}

	public void BookmarkGetAsync(string token, Guid bookmarkId, object userState)
	{
		if (BookmarkGetOperationCompleted == null)
		{
			BookmarkGetOperationCompleted = OnBookmarkGetOperationCompleted;
		}
		InvokeAsync("BookmarkGet", new object[2] { token, bookmarkId }, BookmarkGetOperationCompleted, userState);
	}

	private void OnBookmarkGetOperationCompleted(object arg)
	{
		if (this.BookmarkGetCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.BookmarkGetCompleted(this, new BookmarkGetCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/BookmarkSearchTime", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public Bookmark[] BookmarkSearchTime(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return (Bookmark[])Invoke("BookmarkSearchTime", new object[8] { token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr })[0];
	}

	public IAsyncResult BeginBookmarkSearchTime(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("BookmarkSearchTime", new object[8] { token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr }, callback, asyncState);
	}

	public Bookmark[] EndBookmarkSearchTime(IAsyncResult asyncResult)
	{
		return (Bookmark[])EndInvoke(asyncResult)[0];
	}

	public void BookmarkSearchTimeAsync(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		BookmarkSearchTimeAsync(token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, null);
	}

	public void BookmarkSearchTimeAsync(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, object userState)
	{
		if (BookmarkSearchTimeOperationCompleted == null)
		{
			BookmarkSearchTimeOperationCompleted = OnBookmarkSearchTimeOperationCompleted;
		}
		InvokeAsync("BookmarkSearchTime", new object[8] { token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr }, BookmarkSearchTimeOperationCompleted, userState);
	}

	private void OnBookmarkSearchTimeOperationCompleted(object arg)
	{
		if (this.BookmarkSearchTimeCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.BookmarkSearchTimeCompleted(this, new BookmarkSearchTimeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/BookmarkSearchFromBookmark", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public Bookmark[] BookmarkSearchFromBookmark(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return (Bookmark[])Invoke("BookmarkSearchFromBookmark", new object[8] { token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr })[0];
	}

	public IAsyncResult BeginBookmarkSearchFromBookmark(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("BookmarkSearchFromBookmark", new object[8] { token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr }, callback, asyncState);
	}

	public Bookmark[] EndBookmarkSearchFromBookmark(IAsyncResult asyncResult)
	{
		return (Bookmark[])EndInvoke(asyncResult)[0];
	}

	public void BookmarkSearchFromBookmarkAsync(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		BookmarkSearchFromBookmarkAsync(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, null);
	}

	public void BookmarkSearchFromBookmarkAsync(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, object userState)
	{
		if (BookmarkSearchFromBookmarkOperationCompleted == null)
		{
			BookmarkSearchFromBookmarkOperationCompleted = OnBookmarkSearchFromBookmarkOperationCompleted;
		}
		InvokeAsync("BookmarkSearchFromBookmark", new object[8] { token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr }, BookmarkSearchFromBookmarkOperationCompleted, userState);
	}

	private void OnBookmarkSearchFromBookmarkOperationCompleted(object arg)
	{
		if (this.BookmarkSearchFromBookmarkCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.BookmarkSearchFromBookmarkCompleted(this, new BookmarkSearchFromBookmarkCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/Authorize", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string Authorize(string userDomainName, AuthorizationActionType type)
	{
		return (string)Invoke("Authorize", new object[2] { userDomainName, type })[0];
	}

	public IAsyncResult BeginAuthorize(string userDomainName, AuthorizationActionType type, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("Authorize", new object[2] { userDomainName, type }, callback, asyncState);
	}

	public string EndAuthorize(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void AuthorizeAsync(string userDomainName, AuthorizationActionType type)
	{
		AuthorizeAsync(userDomainName, type, null);
	}

	public void AuthorizeAsync(string userDomainName, AuthorizationActionType type, object userState)
	{
		if (AuthorizeOperationCompleted == null)
		{
			AuthorizeOperationCompleted = OnAuthorizeOperationCompleted;
		}
		InvokeAsync("Authorize", new object[2] { userDomainName, type }, AuthorizeOperationCompleted, userState);
	}

	private void OnAuthorizeOperationCompleted(object arg)
	{
		if (this.AuthorizeCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.AuthorizeCompleted(this, new AuthorizeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/MarkedDataGetNewReference", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public MarkedDataReference MarkedDataGetNewReference(string token, Guid[] deviceIds, bool live)
	{
		return (MarkedDataReference)Invoke("MarkedDataGetNewReference", new object[3] { token, deviceIds, live })[0];
	}

	public IAsyncResult BeginMarkedDataGetNewReference(string token, Guid[] deviceIds, bool live, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("MarkedDataGetNewReference", new object[3] { token, deviceIds, live }, callback, asyncState);
	}

	public MarkedDataReference EndMarkedDataGetNewReference(IAsyncResult asyncResult)
	{
		return (MarkedDataReference)EndInvoke(asyncResult)[0];
	}

	public void MarkedDataGetNewReferenceAsync(string token, Guid[] deviceIds, bool live)
	{
		MarkedDataGetNewReferenceAsync(token, deviceIds, live, null);
	}

	public void MarkedDataGetNewReferenceAsync(string token, Guid[] deviceIds, bool live, object userState)
	{
		if (MarkedDataGetNewReferenceOperationCompleted == null)
		{
			MarkedDataGetNewReferenceOperationCompleted = OnMarkedDataGetNewReferenceOperationCompleted;
		}
		InvokeAsync("MarkedDataGetNewReference", new object[3] { token, deviceIds, live }, MarkedDataGetNewReferenceOperationCompleted, userState);
	}

	private void OnMarkedDataGetNewReferenceOperationCompleted(object arg)
	{
		if (this.MarkedDataGetNewReferenceCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.MarkedDataGetNewReferenceCompleted(this, new MarkedDataGetNewReferenceCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/MarkedDataCreate", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public MarkedDataResult MarkedDataCreate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		return (MarkedDataResult)Invoke("MarkedDataCreate", new object[13]
		{
			token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType,
			useRetention, retentionExpire, retentionOption
		})[0];
	}

	public IAsyncResult BeginMarkedDataCreate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("MarkedDataCreate", new object[13]
		{
			token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType,
			useRetention, retentionExpire, retentionOption
		}, callback, asyncState);
	}

	public MarkedDataResult EndMarkedDataCreate(IAsyncResult asyncResult)
	{
		return (MarkedDataResult)EndInvoke(asyncResult)[0];
	}

	public void MarkedDataCreateAsync(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		MarkedDataCreateAsync(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption, null);
	}

	public void MarkedDataCreateAsync(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption, object userState)
	{
		if (MarkedDataCreateOperationCompleted == null)
		{
			MarkedDataCreateOperationCompleted = OnMarkedDataCreateOperationCompleted;
		}
		InvokeAsync("MarkedDataCreate", new object[13]
		{
			token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType,
			useRetention, retentionExpire, retentionOption
		}, MarkedDataCreateOperationCompleted, userState);
	}

	private void OnMarkedDataCreateOperationCompleted(object arg)
	{
		if (this.MarkedDataCreateCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.MarkedDataCreateCompleted(this, new MarkedDataCreateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/MarkedDataUpdate", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public MarkedDataResult MarkedDataUpdate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		return (MarkedDataResult)Invoke("MarkedDataUpdate", new object[13]
		{
			token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType,
			useRetention, retentionExpire, retentionOption
		})[0];
	}

	public IAsyncResult BeginMarkedDataUpdate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("MarkedDataUpdate", new object[13]
		{
			token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType,
			useRetention, retentionExpire, retentionOption
		}, callback, asyncState);
	}

	public MarkedDataResult EndMarkedDataUpdate(IAsyncResult asyncResult)
	{
		return (MarkedDataResult)EndInvoke(asyncResult)[0];
	}

	public void MarkedDataUpdateAsync(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		MarkedDataUpdateAsync(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption, null);
	}

	public void MarkedDataUpdateAsync(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption, object userState)
	{
		if (MarkedDataUpdateOperationCompleted == null)
		{
			MarkedDataUpdateOperationCompleted = OnMarkedDataUpdateOperationCompleted;
		}
		InvokeAsync("MarkedDataUpdate", new object[13]
		{
			token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType,
			useRetention, retentionExpire, retentionOption
		}, MarkedDataUpdateOperationCompleted, userState);
	}

	private void OnMarkedDataUpdateOperationCompleted(object arg)
	{
		if (this.MarkedDataUpdateCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.MarkedDataUpdateCompleted(this, new MarkedDataUpdateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/MarkedDataDelete", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public MarkedDataResult[] MarkedDataDelete(string token, Guid[] markedDataIds)
	{
		return (MarkedDataResult[])Invoke("MarkedDataDelete", new object[2] { token, markedDataIds })[0];
	}

	public IAsyncResult BeginMarkedDataDelete(string token, Guid[] markedDataIds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("MarkedDataDelete", new object[2] { token, markedDataIds }, callback, asyncState);
	}

	public MarkedDataResult[] EndMarkedDataDelete(IAsyncResult asyncResult)
	{
		return (MarkedDataResult[])EndInvoke(asyncResult)[0];
	}

	public void MarkedDataDeleteAsync(string token, Guid[] markedDataIds)
	{
		MarkedDataDeleteAsync(token, markedDataIds, null);
	}

	public void MarkedDataDeleteAsync(string token, Guid[] markedDataIds, object userState)
	{
		if (MarkedDataDeleteOperationCompleted == null)
		{
			MarkedDataDeleteOperationCompleted = OnMarkedDataDeleteOperationCompleted;
		}
		InvokeAsync("MarkedDataDelete", new object[2] { token, markedDataIds }, MarkedDataDeleteOperationCompleted, userState);
	}

	private void OnMarkedDataDeleteOperationCompleted(object arg)
	{
		if (this.MarkedDataDeleteCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.MarkedDataDeleteCompleted(this, new MarkedDataDeleteCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/MarkedDataGet", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public MarkedData MarkedDataGet(string token, Guid markedDataId)
	{
		return (MarkedData)Invoke("MarkedDataGet", new object[2] { token, markedDataId })[0];
	}

	public IAsyncResult BeginMarkedDataGet(string token, Guid markedDataId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("MarkedDataGet", new object[2] { token, markedDataId }, callback, asyncState);
	}

	public MarkedData EndMarkedDataGet(IAsyncResult asyncResult)
	{
		return (MarkedData)EndInvoke(asyncResult)[0];
	}

	public void MarkedDataGetAsync(string token, Guid markedDataId)
	{
		MarkedDataGetAsync(token, markedDataId, null);
	}

	public void MarkedDataGetAsync(string token, Guid markedDataId, object userState)
	{
		if (MarkedDataGetOperationCompleted == null)
		{
			MarkedDataGetOperationCompleted = OnMarkedDataGetOperationCompleted;
		}
		InvokeAsync("MarkedDataGet", new object[2] { token, markedDataId }, MarkedDataGetOperationCompleted, userState);
	}

	private void OnMarkedDataGetOperationCompleted(object arg)
	{
		if (this.MarkedDataGetCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.MarkedDataGetCompleted(this, new MarkedDataGetCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/MarkedDataSearch", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public MarkedData[] MarkedDataSearch(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		return (MarkedData[])Invoke("MarkedDataSearch", new object[16]
		{
			token, deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime,
			retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending
		})[0];
	}

	public IAsyncResult BeginMarkedDataSearch(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("MarkedDataSearch", new object[16]
		{
			token, deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime,
			retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending
		}, callback, asyncState);
	}

	public MarkedData[] EndMarkedDataSearch(IAsyncResult asyncResult)
	{
		return (MarkedData[])EndInvoke(asyncResult)[0];
	}

	public void MarkedDataSearchAsync(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		MarkedDataSearchAsync(token, deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending, null);
	}

	public void MarkedDataSearchAsync(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending, object userState)
	{
		if (MarkedDataSearchOperationCompleted == null)
		{
			MarkedDataSearchOperationCompleted = OnMarkedDataSearchOperationCompleted;
		}
		InvokeAsync("MarkedDataSearch", new object[16]
		{
			token, deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime,
			retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending
		}, MarkedDataSearchOperationCompleted, userState);
	}

	private void OnMarkedDataSearchOperationCompleted(object arg)
	{
		if (this.MarkedDataSearchCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.MarkedDataSearchCompleted(this, new MarkedDataSearchCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetClientProfile", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public ClientProfile GetClientProfile(string token, bool withSetting)
	{
		return (ClientProfile)Invoke("GetClientProfile", new object[2] { token, withSetting })[0];
	}

	public IAsyncResult BeginGetClientProfile(string token, bool withSetting, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetClientProfile", new object[2] { token, withSetting }, callback, asyncState);
	}

	public ClientProfile EndGetClientProfile(IAsyncResult asyncResult)
	{
		return (ClientProfile)EndInvoke(asyncResult)[0];
	}

	public void GetClientProfileAsync(string token, bool withSetting)
	{
		GetClientProfileAsync(token, withSetting, null);
	}

	public void GetClientProfileAsync(string token, bool withSetting, object userState)
	{
		if (GetClientProfileOperationCompleted == null)
		{
			GetClientProfileOperationCompleted = OnGetClientProfileOperationCompleted;
		}
		InvokeAsync("GetClientProfile", new object[2] { token, withSetting }, GetClientProfileOperationCompleted, userState);
	}

	private void OnGetClientProfileOperationCompleted(object arg)
	{
		if (this.GetClientProfileCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetClientProfileCompleted(this, new GetClientProfileCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/GetPresets", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public PresetInfo[] GetPresets(string token, Guid deviceId)
	{
		return (PresetInfo[])Invoke("GetPresets", new object[2] { token, deviceId })[0];
	}

	public IAsyncResult BeginGetPresets(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetPresets", new object[2] { token, deviceId }, callback, asyncState);
	}

	public PresetInfo[] EndGetPresets(IAsyncResult asyncResult)
	{
		return (PresetInfo[])EndInvoke(asyncResult)[0];
	}

	public void GetPresetsAsync(string token, Guid deviceId)
	{
		GetPresetsAsync(token, deviceId, null);
	}

	public void GetPresetsAsync(string token, Guid deviceId, object userState)
	{
		if (GetPresetsOperationCompleted == null)
		{
			GetPresetsOperationCompleted = OnGetPresetsOperationCompleted;
		}
		InvokeAsync("GetPresets", new object[2] { token, deviceId }, GetPresetsOperationCompleted, userState);
	}

	private void OnGetPresetsOperationCompleted(object arg)
	{
		if (this.GetPresetsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetPresetsCompleted(this, new GetPresetsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/DeletePreset", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void DeletePreset(string token, Guid id, Guid deviceId)
	{
		Invoke("DeletePreset", new object[3] { token, id, deviceId });
	}

	public IAsyncResult BeginDeletePreset(string token, Guid id, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DeletePreset", new object[3] { token, id, deviceId }, callback, asyncState);
	}

	public void EndDeletePreset(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void DeletePresetAsync(string token, Guid id, Guid deviceId)
	{
		DeletePresetAsync(token, id, deviceId, null);
	}

	public void DeletePresetAsync(string token, Guid id, Guid deviceId, object userState)
	{
		if (DeletePresetOperationCompleted == null)
		{
			DeletePresetOperationCompleted = OnDeletePresetOperationCompleted;
		}
		InvokeAsync("DeletePreset", new object[3] { token, id, deviceId }, DeletePresetOperationCompleted, userState);
	}

	private void OnDeletePresetOperationCompleted(object arg)
	{
		if (this.DeletePresetCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DeletePresetCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/UpdatePresetName", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void UpdatePresetName(string token, Guid id, Guid deviceId, string newName)
	{
		Invoke("UpdatePresetName", new object[4] { token, id, deviceId, newName });
	}

	public IAsyncResult BeginUpdatePresetName(string token, Guid id, Guid deviceId, string newName, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("UpdatePresetName", new object[4] { token, id, deviceId, newName }, callback, asyncState);
	}

	public void EndUpdatePresetName(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void UpdatePresetNameAsync(string token, Guid id, Guid deviceId, string newName)
	{
		UpdatePresetNameAsync(token, id, deviceId, newName, null);
	}

	public void UpdatePresetNameAsync(string token, Guid id, Guid deviceId, string newName, object userState)
	{
		if (UpdatePresetNameOperationCompleted == null)
		{
			UpdatePresetNameOperationCompleted = OnUpdatePresetNameOperationCompleted;
		}
		InvokeAsync("UpdatePresetName", new object[4] { token, id, deviceId, newName }, UpdatePresetNameOperationCompleted, userState);
	}

	private void OnUpdatePresetNameOperationCompleted(object arg)
	{
		if (this.UpdatePresetNameCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.UpdatePresetNameCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/UpdatePresetPosition", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void UpdatePresetPosition(string token, Guid id, Guid deviceId)
	{
		Invoke("UpdatePresetPosition", new object[3] { token, id, deviceId });
	}

	public IAsyncResult BeginUpdatePresetPosition(string token, Guid id, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("UpdatePresetPosition", new object[3] { token, id, deviceId }, callback, asyncState);
	}

	public void EndUpdatePresetPosition(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void UpdatePresetPositionAsync(string token, Guid id, Guid deviceId)
	{
		UpdatePresetPositionAsync(token, id, deviceId, null);
	}

	public void UpdatePresetPositionAsync(string token, Guid id, Guid deviceId, object userState)
	{
		if (UpdatePresetPositionOperationCompleted == null)
		{
			UpdatePresetPositionOperationCompleted = OnUpdatePresetPositionOperationCompleted;
		}
		InvokeAsync("UpdatePresetPosition", new object[3] { token, id, deviceId }, UpdatePresetPositionOperationCompleted, userState);
	}

	private void OnUpdatePresetPositionOperationCompleted(object arg)
	{
		if (this.UpdatePresetPositionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.UpdatePresetPositionCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/CreatePreset", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public Guid CreatePreset(string token, Guid deviceId, string name)
	{
		return (Guid)Invoke("CreatePreset", new object[3] { token, deviceId, name })[0];
	}

	public IAsyncResult BeginCreatePreset(string token, Guid deviceId, string name, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CreatePreset", new object[3] { token, deviceId, name }, callback, asyncState);
	}

	public Guid EndCreatePreset(IAsyncResult asyncResult)
	{
		return (Guid)EndInvoke(asyncResult)[0];
	}

	public void CreatePresetAsync(string token, Guid deviceId, string name)
	{
		CreatePresetAsync(token, deviceId, name, null);
	}

	public void CreatePresetAsync(string token, Guid deviceId, string name, object userState)
	{
		if (CreatePresetOperationCompleted == null)
		{
			CreatePresetOperationCompleted = OnCreatePresetOperationCompleted;
		}
		InvokeAsync("CreatePreset", new object[3] { token, deviceId, name }, CreatePresetOperationCompleted, userState);
	}

	private void OnCreatePresetOperationCompleted(object arg)
	{
		if (this.CreatePresetCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CreatePresetCompleted(this, new CreatePresetCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://videoos.net/2/XProtectCSServerCommand/OrderPresets", RequestNamespace = "http://videoos.net/2/XProtectCSServerCommand", ResponseNamespace = "http://videoos.net/2/XProtectCSServerCommand", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void OrderPresets(string token, Guid deviceId, Guid[] ids)
	{
		Invoke("OrderPresets", new object[3] { token, deviceId, ids });
	}

	public IAsyncResult BeginOrderPresets(string token, Guid deviceId, Guid[] ids, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("OrderPresets", new object[3] { token, deviceId, ids }, callback, asyncState);
	}

	public void EndOrderPresets(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void OrderPresetsAsync(string token, Guid deviceId, Guid[] ids)
	{
		OrderPresetsAsync(token, deviceId, ids, null);
	}

	public void OrderPresetsAsync(string token, Guid deviceId, Guid[] ids, object userState)
	{
		if (OrderPresetsOperationCompleted == null)
		{
			OrderPresetsOperationCompleted = OnOrderPresetsOperationCompleted;
		}
		InvokeAsync("OrderPresets", new object[3] { token, deviceId, ids }, OrderPresetsOperationCompleted, userState);
	}

	private void OnOrderPresetsOperationCompleted(object arg)
	{
		if (this.OrderPresetsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.OrderPresetsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public new void CancelAsync(object userState)
	{
		base.CancelAsync(userState);
	}
}
