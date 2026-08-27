using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ExcludeFromCodeCoverage]
public class ServerCommandServiceClient : ClientBase<IServerCommandService>, IServerCommandService
{
	private BeginOperationDelegate onBeginGetVersionDelegate;

	private EndOperationDelegate onEndGetVersionDelegate;

	private SendOrPostCallback onGetVersionCompletedDelegate;

	private BeginOperationDelegate onBeginGetServerVersionDelegate;

	private EndOperationDelegate onEndGetServerVersionDelegate;

	private SendOrPostCallback onGetServerVersionCompletedDelegate;

	private BeginOperationDelegate onBeginGetProductInfoDelegate;

	private EndOperationDelegate onEndGetProductInfoDelegate;

	private SendOrPostCallback onGetProductInfoCompletedDelegate;

	private BeginOperationDelegate onBeginGetSystemLicenseInfoDelegate;

	private EndOperationDelegate onEndGetSystemLicenseInfoDelegate;

	private SendOrPostCallback onGetSystemLicenseInfoCompletedDelegate;

	private BeginOperationDelegate onBeginGetSmartClientVersionDelegate;

	private EndOperationDelegate onEndGetSmartClientVersionDelegate;

	private SendOrPostCallback onGetSmartClientVersionCompletedDelegate;

	private BeginOperationDelegate onBeginCheckSmartClientVersionDelegate;

	private EndOperationDelegate onEndCheckSmartClientVersionDelegate;

	private SendOrPostCallback onCheckSmartClientVersionCompletedDelegate;

	private BeginOperationDelegate onBeginIsOnlineDelegate;

	private EndOperationDelegate onEndIsOnlineDelegate;

	private SendOrPostCallback onIsOnlineCompletedDelegate;

	private BeginOperationDelegate onBeginLoginDelegate;

	private EndOperationDelegate onEndLoginDelegate;

	private SendOrPostCallback onLoginCompletedDelegate;

	private BeginOperationDelegate onBeginLogoutDelegate;

	private EndOperationDelegate onEndLogoutDelegate;

	private SendOrPostCallback onLogoutCompletedDelegate;

	private BeginOperationDelegate onBeginAuthorizeDelegate;

	private EndOperationDelegate onEndAuthorizeDelegate;

	private SendOrPostCallback onAuthorizeCompletedDelegate;

	private BeginOperationDelegate onBeginGetUserInfoDelegate;

	private EndOperationDelegate onEndGetUserInfoDelegate;

	private SendOrPostCallback onGetUserInfoCompletedDelegate;

	private BeginOperationDelegate onBeginGetConfigurationDelegate;

	private EndOperationDelegate onEndGetConfigurationDelegate;

	private SendOrPostCallback onGetConfigurationCompletedDelegate;

	private BeginOperationDelegate onBeginGetConfigurationRecordersDelegate;

	private EndOperationDelegate onEndGetConfigurationRecordersDelegate;

	private SendOrPostCallback onGetConfigurationRecordersCompletedDelegate;

	private BeginOperationDelegate onBeginGetConfigurationHardwareDelegate;

	private EndOperationDelegate onEndGetConfigurationHardwareDelegate;

	private SendOrPostCallback onGetConfigurationHardwareCompletedDelegate;

	private BeginOperationDelegate onBeginGetDevicesDisabledDelegate;

	private EndOperationDelegate onEndGetDevicesDisabledDelegate;

	private SendOrPostCallback onGetDevicesDisabledCompletedDelegate;

	private BeginOperationDelegate onBeginGetVmoServicesListDelegate;

	private EndOperationDelegate onEndGetVmoServicesListDelegate;

	private SendOrPostCallback onGetVmoServicesListCompletedDelegate;

	private BeginOperationDelegate onBeginGetViewGroupsDelegate;

	private EndOperationDelegate onEndGetViewGroupsDelegate;

	private SendOrPostCallback onGetViewGroupsCompletedDelegate;

	private BeginOperationDelegate onBeginGetViewGroupDataDelegate;

	private EndOperationDelegate onEndGetViewGroupDataDelegate;

	private SendOrPostCallback onGetViewGroupDataCompletedDelegate;

	private BeginOperationDelegate onBeginSetViewGroupDataDelegate;

	private EndOperationDelegate onEndSetViewGroupDataDelegate;

	private SendOrPostCallback onSetViewGroupDataCompletedDelegate;

	private BeginOperationDelegate onBeginGetCustomSettingDataGlobalDelegate;

	private EndOperationDelegate onEndGetCustomSettingDataGlobalDelegate;

	private SendOrPostCallback onGetCustomSettingDataGlobalCompletedDelegate;

	private BeginOperationDelegate onBeginGetCustomSettingDataUserDelegate;

	private EndOperationDelegate onEndGetCustomSettingDataUserDelegate;

	private SendOrPostCallback onGetCustomSettingDataUserCompletedDelegate;

	private BeginOperationDelegate onBeginSetCustomSettingDataGlobalDelegate;

	private EndOperationDelegate onEndSetCustomSettingDataGlobalDelegate;

	private SendOrPostCallback onSetCustomSettingDataGlobalCompletedDelegate;

	private BeginOperationDelegate onBeginSetCustomSettingDataUserDelegate;

	private EndOperationDelegate onEndSetCustomSettingDataUserDelegate;

	private SendOrPostCallback onSetCustomSettingDataUserCompletedDelegate;

	private BeginOperationDelegate onBeginEventTriggerDelegate;

	private EndOperationDelegate onEndEventTriggerDelegate;

	private SendOrPostCallback onEventTriggerCompletedDelegate;

	private BeginOperationDelegate onBeginEventTriggerWithMetadataDelegate;

	private EndOperationDelegate onEndEventTriggerWithMetadataDelegate;

	private SendOrPostCallback onEventTriggerWithMetadataCompletedDelegate;

	private BeginOperationDelegate onBeginAlertsGetAroundWithSpanDelegate;

	private EndOperationDelegate onEndAlertsGetAroundWithSpanDelegate;

	private SendOrPostCallback onAlertsGetAroundWithSpanCompletedDelegate;

	private BeginOperationDelegate onBeginMatrixMonitorActivateCameraDelegate;

	private EndOperationDelegate onEndMatrixMonitorActivateCameraDelegate;

	private SendOrPostCallback onMatrixMonitorActivateCameraCompletedDelegate;

	private BeginOperationDelegate onBeginQueryChangesDelegate;

	private EndOperationDelegate onEndQueryChangesDelegate;

	private SendOrPostCallback onQueryChangesCompletedDelegate;

	private BeginOperationDelegate onBeginQueryRecorderInfoDelegate;

	private EndOperationDelegate onEndQueryRecorderInfoDelegate;

	private SendOrPostCallback onQueryRecorderInfoCompletedDelegate;

	private BeginOperationDelegate onBeginBookmarkGetNewReferenceDelegate;

	private EndOperationDelegate onEndBookmarkGetNewReferenceDelegate;

	private SendOrPostCallback onBookmarkGetNewReferenceCompletedDelegate;

	private BeginOperationDelegate onBeginBookmarkCreateDelegate;

	private EndOperationDelegate onEndBookmarkCreateDelegate;

	private SendOrPostCallback onBookmarkCreateCompletedDelegate;

	private BeginOperationDelegate onBeginBookmarkUpdateDelegate;

	private EndOperationDelegate onEndBookmarkUpdateDelegate;

	private SendOrPostCallback onBookmarkUpdateCompletedDelegate;

	private BeginOperationDelegate onBeginBookmarkDeleteDelegate;

	private EndOperationDelegate onEndBookmarkDeleteDelegate;

	private SendOrPostCallback onBookmarkDeleteCompletedDelegate;

	private BeginOperationDelegate onBeginBookmarkGetDelegate;

	private EndOperationDelegate onEndBookmarkGetDelegate;

	private SendOrPostCallback onBookmarkGetCompletedDelegate;

	private BeginOperationDelegate onBeginBookmarkSearchTimeDelegate;

	private EndOperationDelegate onEndBookmarkSearchTimeDelegate;

	private SendOrPostCallback onBookmarkSearchTimeCompletedDelegate;

	private BeginOperationDelegate onBeginBookmarkSearchTime2Delegate;

	private EndOperationDelegate onEndBookmarkSearchTime2Delegate;

	private SendOrPostCallback onBookmarkSearchTime2CompletedDelegate;

	private BeginOperationDelegate onBeginBookmarkSearchFromBookmarkDelegate;

	private EndOperationDelegate onEndBookmarkSearchFromBookmarkDelegate;

	private SendOrPostCallback onBookmarkSearchFromBookmarkCompletedDelegate;

	private BeginOperationDelegate onBeginBookmarkSearchFromBookmark2Delegate;

	private EndOperationDelegate onEndBookmarkSearchFromBookmark2Delegate;

	private SendOrPostCallback onBookmarkSearchFromBookmark2CompletedDelegate;

	private BeginOperationDelegate onBeginMarkedDataGetNewReferenceDelegate;

	private EndOperationDelegate onEndMarkedDataGetNewReferenceDelegate;

	private SendOrPostCallback onMarkedDataGetNewReferenceCompletedDelegate;

	private BeginOperationDelegate onBeginMarkedDataCreateDelegate;

	private EndOperationDelegate onEndMarkedDataCreateDelegate;

	private SendOrPostCallback onMarkedDataCreateCompletedDelegate;

	private BeginOperationDelegate onBeginMarkedDataUpdateDelegate;

	private EndOperationDelegate onEndMarkedDataUpdateDelegate;

	private SendOrPostCallback onMarkedDataUpdateCompletedDelegate;

	private BeginOperationDelegate onBeginMarkedDataDeleteDelegate;

	private EndOperationDelegate onEndMarkedDataDeleteDelegate;

	private SendOrPostCallback onMarkedDataDeleteCompletedDelegate;

	private BeginOperationDelegate onBeginMarkedDataGetDelegate;

	private EndOperationDelegate onEndMarkedDataGetDelegate;

	private SendOrPostCallback onMarkedDataGetCompletedDelegate;

	private BeginOperationDelegate onBeginMarkedDataSearchDelegate;

	private EndOperationDelegate onEndMarkedDataSearchDelegate;

	private SendOrPostCallback onMarkedDataSearchCompletedDelegate;

	private BeginOperationDelegate onBeginRestrictedMediaCreateDelegate;

	private EndOperationDelegate onEndRestrictedMediaCreateDelegate;

	private SendOrPostCallback onRestrictedMediaCreateCompletedDelegate;

	private BeginOperationDelegate onBeginRestrictedMediaUpdateDelegate;

	private EndOperationDelegate onEndRestrictedMediaUpdateDelegate;

	private SendOrPostCallback onRestrictedMediaUpdateCompletedDelegate;

	private BeginOperationDelegate onBeginRestrictedMediaDeleteDelegate;

	private EndOperationDelegate onEndRestrictedMediaDeleteDelegate;

	private SendOrPostCallback onRestrictedMediaDeleteCompletedDelegate;

	private BeginOperationDelegate onBeginRestrictedMediaQueryAllDelegate;

	private EndOperationDelegate onEndRestrictedMediaQueryAllDelegate;

	private SendOrPostCallback onRestrictedMediaQueryAllCompletedDelegate;

	private BeginOperationDelegate onBeginRestrictedMediaLiveModeEnterDelegate;

	private EndOperationDelegate onEndRestrictedMediaLiveModeEnterDelegate;

	private SendOrPostCallback onRestrictedMediaLiveModeEnterCompletedDelegate;

	private BeginOperationDelegate onBeginRestrictedMediaLiveModeExitDelegate;

	private EndOperationDelegate onEndRestrictedMediaLiveModeExitDelegate;

	private SendOrPostCallback onRestrictedMediaLiveModeExitCompletedDelegate;

	private BeginOperationDelegate onBeginRestrictedMediaLiveUpdateDelegate;

	private EndOperationDelegate onEndRestrictedMediaLiveUpdateDelegate;

	private SendOrPostCallback onRestrictedMediaLiveUpdateCompletedDelegate;

	private BeginOperationDelegate onBeginRestrictedMediaLiveDeleteDelegate;

	private EndOperationDelegate onEndRestrictedMediaLiveDeleteDelegate;

	private SendOrPostCallback onRestrictedMediaLiveDeleteCompletedDelegate;

	private BeginOperationDelegate onBeginRestrictedMediaLiveQueryAllDelegate;

	private EndOperationDelegate onEndRestrictedMediaLiveQueryAllDelegate;

	private SendOrPostCallback onRestrictedMediaLiveQueryAllCompletedDelegate;

	private BeginOperationDelegate onBeginGetClientProfileDelegate;

	private EndOperationDelegate onEndGetClientProfileDelegate;

	private SendOrPostCallback onGetClientProfileCompletedDelegate;

	private BeginOperationDelegate onBeginGetPresetsDelegate;

	private EndOperationDelegate onEndGetPresetsDelegate;

	private SendOrPostCallback onGetPresetsCompletedDelegate;

	private BeginOperationDelegate onBeginDeletePresetDelegate;

	private EndOperationDelegate onEndDeletePresetDelegate;

	private SendOrPostCallback onDeletePresetCompletedDelegate;

	private BeginOperationDelegate onBeginUpdatePresetNameDelegate;

	private EndOperationDelegate onEndUpdatePresetNameDelegate;

	private SendOrPostCallback onUpdatePresetNameCompletedDelegate;

	private BeginOperationDelegate onBeginUpdatePresetPositionDelegate;

	private EndOperationDelegate onEndUpdatePresetPositionDelegate;

	private SendOrPostCallback onUpdatePresetPositionCompletedDelegate;

	private BeginOperationDelegate onBeginCreatePresetDelegate;

	private EndOperationDelegate onEndCreatePresetDelegate;

	private SendOrPostCallback onCreatePresetCompletedDelegate;

	private BeginOperationDelegate onBeginOrderPresetsDelegate;

	private EndOperationDelegate onEndOrderPresetsDelegate;

	private SendOrPostCallback onOrderPresetsCompletedDelegate;

	public event EventHandler<GetVersionCompletedEventArgs> GetVersionCompleted;

	public event EventHandler<GetServerVersionCompletedEventArgs> GetServerVersionCompleted;

	public event EventHandler<GetProductInfoCompletedEventArgs> GetProductInfoCompleted;

	public event EventHandler<GetSystemLicenseInfoCompletedEventArgs> GetSystemLicenseInfoCompleted;

	public event EventHandler<GetSmartClientVersionCompletedEventArgs> GetSmartClientVersionCompleted;

	public event EventHandler<CheckSmartClientVersionCompletedEventArgs> CheckSmartClientVersionCompleted;

	public event EventHandler<IsOnlineCompletedEventArgs> IsOnlineCompleted;

	public event EventHandler<LoginCompletedEventArgs> LoginCompleted;

	public event EventHandler<AsyncCompletedEventArgs> LogoutCompleted;

	public event EventHandler<AuthorizeCompletedEventArgs> AuthorizeCompleted;

	public event EventHandler<GetUserInfoCompletedEventArgs> GetUserInfoCompleted;

	public event EventHandler<GetConfigurationCompletedEventArgs> GetConfigurationCompleted;

	public event EventHandler<GetConfigurationRecordersCompletedEventArgs> GetConfigurationRecordersCompleted;

	public event EventHandler<GetConfigurationHardwareCompletedEventArgs> GetConfigurationHardwareCompleted;

	public event EventHandler<GetDevicesDisabledCompletedEventArgs> GetDevicesDisabledCompleted;

	public event EventHandler<GetVmoServicesListCompletedEventArgs> GetVmoServicesListCompleted;

	public event EventHandler<GetViewGroupsCompletedEventArgs> GetViewGroupsCompleted;

	public event EventHandler<GetViewGroupDataCompletedEventArgs> GetViewGroupDataCompleted;

	public event EventHandler<SetViewGroupDataCompletedEventArgs> SetViewGroupDataCompleted;

	public event EventHandler<GetCustomSettingDataGlobalCompletedEventArgs> GetCustomSettingDataGlobalCompleted;

	public event EventHandler<GetCustomSettingDataUserCompletedEventArgs> GetCustomSettingDataUserCompleted;

	public event EventHandler<SetCustomSettingDataGlobalCompletedEventArgs> SetCustomSettingDataGlobalCompleted;

	public event EventHandler<SetCustomSettingDataUserCompletedEventArgs> SetCustomSettingDataUserCompleted;

	public event EventHandler<AsyncCompletedEventArgs> EventTriggerCompleted;

	public event EventHandler<AsyncCompletedEventArgs> EventTriggerWithMetadataCompleted;

	public event EventHandler<AlertsGetAroundWithSpanCompletedEventArgs> AlertsGetAroundWithSpanCompleted;

	public event EventHandler<AsyncCompletedEventArgs> MatrixMonitorActivateCameraCompleted;

	public event EventHandler<QueryChangesCompletedEventArgs> QueryChangesCompleted;

	public event EventHandler<QueryRecorderInfoCompletedEventArgs> QueryRecorderInfoCompleted;

	public event EventHandler<BookmarkGetNewReferenceCompletedEventArgs> BookmarkGetNewReferenceCompleted;

	public event EventHandler<BookmarkCreateCompletedEventArgs> BookmarkCreateCompleted;

	public event EventHandler<BookmarkUpdateCompletedEventArgs> BookmarkUpdateCompleted;

	public event EventHandler<AsyncCompletedEventArgs> BookmarkDeleteCompleted;

	public event EventHandler<BookmarkGetCompletedEventArgs> BookmarkGetCompleted;

	public event EventHandler<BookmarkSearchTimeCompletedEventArgs> BookmarkSearchTimeCompleted;

	public event EventHandler<BookmarkSearchTime2CompletedEventArgs> BookmarkSearchTime2Completed;

	public event EventHandler<BookmarkSearchFromBookmarkCompletedEventArgs> BookmarkSearchFromBookmarkCompleted;

	public event EventHandler<BookmarkSearchFromBookmark2CompletedEventArgs> BookmarkSearchFromBookmark2Completed;

	public event EventHandler<MarkedDataGetNewReferenceCompletedEventArgs> MarkedDataGetNewReferenceCompleted;

	public event EventHandler<MarkedDataCreateCompletedEventArgs> MarkedDataCreateCompleted;

	public event EventHandler<MarkedDataUpdateCompletedEventArgs> MarkedDataUpdateCompleted;

	public event EventHandler<MarkedDataDeleteCompletedEventArgs> MarkedDataDeleteCompleted;

	public event EventHandler<MarkedDataGetCompletedEventArgs> MarkedDataGetCompleted;

	public event EventHandler<MarkedDataSearchCompletedEventArgs> MarkedDataSearchCompleted;

	public event EventHandler<RestrictedMediaCreateCompletedEventArgs> RestrictedMediaCreateCompleted;

	public event EventHandler<RestrictedMediaUpdateCompletedEventArgs> RestrictedMediaUpdateCompleted;

	public event EventHandler<RestrictedMediaDeleteCompletedEventArgs> RestrictedMediaDeleteCompleted;

	public event EventHandler<RestrictedMediaQueryAllCompletedEventArgs> RestrictedMediaQueryAllCompleted;

	public event EventHandler<RestrictedMediaLiveModeEnterCompletedEventArgs> RestrictedMediaLiveModeEnterCompleted;

	public event EventHandler<RestrictedMediaLiveModeExitCompletedEventArgs> RestrictedMediaLiveModeExitCompleted;

	public event EventHandler<RestrictedMediaLiveUpdateCompletedEventArgs> RestrictedMediaLiveUpdateCompleted;

	public event EventHandler<RestrictedMediaLiveDeleteCompletedEventArgs> RestrictedMediaLiveDeleteCompleted;

	public event EventHandler<RestrictedMediaLiveQueryAllCompletedEventArgs> RestrictedMediaLiveQueryAllCompleted;

	public event EventHandler<GetClientProfileCompletedEventArgs> GetClientProfileCompleted;

	public event EventHandler<GetPresetsCompletedEventArgs> GetPresetsCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DeletePresetCompleted;

	public event EventHandler<AsyncCompletedEventArgs> UpdatePresetNameCompleted;

	public event EventHandler<AsyncCompletedEventArgs> UpdatePresetPositionCompleted;

	public event EventHandler<CreatePresetCompletedEventArgs> CreatePresetCompleted;

	public event EventHandler<AsyncCompletedEventArgs> OrderPresetsCompleted;

	public ServerCommandServiceClient()
	{
	}

	public ServerCommandServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ServerCommandServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ServerCommandServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ServerCommandServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public int GetVersion()
	{
		return base.Channel.GetVersion();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetVersion(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetVersion(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public int EndGetVersion(IAsyncResult result)
	{
		return base.Channel.EndGetVersion(result);
	}

	private IAsyncResult OnBeginGetVersion(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetVersion(callback, asyncState);
	}

	private object[] OnEndGetVersion(IAsyncResult result)
	{
		int num = EndGetVersion(result);
		return new object[1] { num };
	}

	private void OnGetVersionCompleted(object state)
	{
		if (this.GetVersionCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetVersionCompleted(this, new GetVersionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetVersionAsync()
	{
		GetVersionAsync(null);
	}

	public void GetVersionAsync(object userState)
	{
		if (onBeginGetVersionDelegate == null)
		{
			onBeginGetVersionDelegate = OnBeginGetVersion;
		}
		if (onEndGetVersionDelegate == null)
		{
			onEndGetVersionDelegate = OnEndGetVersion;
		}
		if (onGetVersionCompletedDelegate == null)
		{
			onGetVersionCompletedDelegate = OnGetVersionCompleted;
		}
		InvokeAsync(onBeginGetVersionDelegate, null, onEndGetVersionDelegate, onGetVersionCompletedDelegate, userState);
	}

	public string GetServerVersion()
	{
		return base.Channel.GetServerVersion();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetServerVersion(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetServerVersion(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string EndGetServerVersion(IAsyncResult result)
	{
		return base.Channel.EndGetServerVersion(result);
	}

	private IAsyncResult OnBeginGetServerVersion(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetServerVersion(callback, asyncState);
	}

	private object[] OnEndGetServerVersion(IAsyncResult result)
	{
		string text = EndGetServerVersion(result);
		return new object[1] { text };
	}

	private void OnGetServerVersionCompleted(object state)
	{
		if (this.GetServerVersionCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetServerVersionCompleted(this, new GetServerVersionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetServerVersionAsync()
	{
		GetServerVersionAsync(null);
	}

	public void GetServerVersionAsync(object userState)
	{
		if (onBeginGetServerVersionDelegate == null)
		{
			onBeginGetServerVersionDelegate = OnBeginGetServerVersion;
		}
		if (onEndGetServerVersionDelegate == null)
		{
			onEndGetServerVersionDelegate = OnEndGetServerVersion;
		}
		if (onGetServerVersionCompletedDelegate == null)
		{
			onGetServerVersionCompletedDelegate = OnGetServerVersionCompleted;
		}
		InvokeAsync(onBeginGetServerVersionDelegate, null, onEndGetServerVersionDelegate, onGetServerVersionCompletedDelegate, userState);
	}

	public ProductInfo GetProductInfo()
	{
		return base.Channel.GetProductInfo();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetProductInfo(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetProductInfo(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public ProductInfo EndGetProductInfo(IAsyncResult result)
	{
		return base.Channel.EndGetProductInfo(result);
	}

	private IAsyncResult OnBeginGetProductInfo(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetProductInfo(callback, asyncState);
	}

	private object[] OnEndGetProductInfo(IAsyncResult result)
	{
		ProductInfo productInfo = EndGetProductInfo(result);
		return new object[1] { productInfo };
	}

	private void OnGetProductInfoCompleted(object state)
	{
		if (this.GetProductInfoCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetProductInfoCompleted(this, new GetProductInfoCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetProductInfoAsync()
	{
		GetProductInfoAsync(null);
	}

	public void GetProductInfoAsync(object userState)
	{
		if (onBeginGetProductInfoDelegate == null)
		{
			onBeginGetProductInfoDelegate = OnBeginGetProductInfo;
		}
		if (onEndGetProductInfoDelegate == null)
		{
			onEndGetProductInfoDelegate = OnEndGetProductInfo;
		}
		if (onGetProductInfoCompletedDelegate == null)
		{
			onGetProductInfoCompletedDelegate = OnGetProductInfoCompleted;
		}
		InvokeAsync(onBeginGetProductInfoDelegate, null, onEndGetProductInfoDelegate, onGetProductInfoCompletedDelegate, userState);
	}

	public SystemLicenseInfo GetSystemLicenseInfo()
	{
		return base.Channel.GetSystemLicenseInfo();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetSystemLicenseInfo(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetSystemLicenseInfo(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public SystemLicenseInfo EndGetSystemLicenseInfo(IAsyncResult result)
	{
		return base.Channel.EndGetSystemLicenseInfo(result);
	}

	private IAsyncResult OnBeginGetSystemLicenseInfo(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetSystemLicenseInfo(callback, asyncState);
	}

	private object[] OnEndGetSystemLicenseInfo(IAsyncResult result)
	{
		SystemLicenseInfo systemLicenseInfo = EndGetSystemLicenseInfo(result);
		return new object[1] { systemLicenseInfo };
	}

	private void OnGetSystemLicenseInfoCompleted(object state)
	{
		if (this.GetSystemLicenseInfoCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetSystemLicenseInfoCompleted(this, new GetSystemLicenseInfoCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetSystemLicenseInfoAsync()
	{
		GetSystemLicenseInfoAsync(null);
	}

	public void GetSystemLicenseInfoAsync(object userState)
	{
		if (onBeginGetSystemLicenseInfoDelegate == null)
		{
			onBeginGetSystemLicenseInfoDelegate = OnBeginGetSystemLicenseInfo;
		}
		if (onEndGetSystemLicenseInfoDelegate == null)
		{
			onEndGetSystemLicenseInfoDelegate = OnEndGetSystemLicenseInfo;
		}
		if (onGetSystemLicenseInfoCompletedDelegate == null)
		{
			onGetSystemLicenseInfoCompletedDelegate = OnGetSystemLicenseInfoCompleted;
		}
		InvokeAsync(onBeginGetSystemLicenseInfoDelegate, null, onEndGetSystemLicenseInfoDelegate, onGetSystemLicenseInfoCompletedDelegate, userState);
	}

	public SmartClientVersion GetSmartClientVersion()
	{
		return base.Channel.GetSmartClientVersion();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetSmartClientVersion(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetSmartClientVersion(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public SmartClientVersion EndGetSmartClientVersion(IAsyncResult result)
	{
		return base.Channel.EndGetSmartClientVersion(result);
	}

	private IAsyncResult OnBeginGetSmartClientVersion(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetSmartClientVersion(callback, asyncState);
	}

	private object[] OnEndGetSmartClientVersion(IAsyncResult result)
	{
		SmartClientVersion smartClientVersion = EndGetSmartClientVersion(result);
		return new object[1] { smartClientVersion };
	}

	private void OnGetSmartClientVersionCompleted(object state)
	{
		if (this.GetSmartClientVersionCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetSmartClientVersionCompleted(this, new GetSmartClientVersionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetSmartClientVersionAsync()
	{
		GetSmartClientVersionAsync(null);
	}

	public void GetSmartClientVersionAsync(object userState)
	{
		if (onBeginGetSmartClientVersionDelegate == null)
		{
			onBeginGetSmartClientVersionDelegate = OnBeginGetSmartClientVersion;
		}
		if (onEndGetSmartClientVersionDelegate == null)
		{
			onEndGetSmartClientVersionDelegate = OnEndGetSmartClientVersion;
		}
		if (onGetSmartClientVersionCompletedDelegate == null)
		{
			onGetSmartClientVersionCompletedDelegate = OnGetSmartClientVersionCompleted;
		}
		InvokeAsync(onBeginGetSmartClientVersionDelegate, null, onEndGetSmartClientVersionDelegate, onGetSmartClientVersionCompletedDelegate, userState);
	}

	public SmartClientVersion CheckSmartClientVersion(int major, int minor, string revision)
	{
		return base.Channel.CheckSmartClientVersion(major, minor, revision);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginCheckSmartClientVersion(int major, int minor, string revision, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginCheckSmartClientVersion(major, minor, revision, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public SmartClientVersion EndCheckSmartClientVersion(IAsyncResult result)
	{
		return base.Channel.EndCheckSmartClientVersion(result);
	}

	private IAsyncResult OnBeginCheckSmartClientVersion(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int major = (int)inValues[0];
		int minor = (int)inValues[1];
		string revision = (string)inValues[2];
		return BeginCheckSmartClientVersion(major, minor, revision, callback, asyncState);
	}

	private object[] OnEndCheckSmartClientVersion(IAsyncResult result)
	{
		SmartClientVersion smartClientVersion = EndCheckSmartClientVersion(result);
		return new object[1] { smartClientVersion };
	}

	private void OnCheckSmartClientVersionCompleted(object state)
	{
		if (this.CheckSmartClientVersionCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.CheckSmartClientVersionCompleted(this, new CheckSmartClientVersionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void CheckSmartClientVersionAsync(int major, int minor, string revision)
	{
		CheckSmartClientVersionAsync(major, minor, revision, null);
	}

	public void CheckSmartClientVersionAsync(int major, int minor, string revision, object userState)
	{
		if (onBeginCheckSmartClientVersionDelegate == null)
		{
			onBeginCheckSmartClientVersionDelegate = OnBeginCheckSmartClientVersion;
		}
		if (onEndCheckSmartClientVersionDelegate == null)
		{
			onEndCheckSmartClientVersionDelegate = OnEndCheckSmartClientVersion;
		}
		if (onCheckSmartClientVersionCompletedDelegate == null)
		{
			onCheckSmartClientVersionCompletedDelegate = OnCheckSmartClientVersionCompleted;
		}
		InvokeAsync(onBeginCheckSmartClientVersionDelegate, new object[3] { major, minor, revision }, onEndCheckSmartClientVersionDelegate, onCheckSmartClientVersionCompletedDelegate, userState);
	}

	public bool IsOnline()
	{
		return base.Channel.IsOnline();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginIsOnline(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginIsOnline(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndIsOnline(IAsyncResult result)
	{
		return base.Channel.EndIsOnline(result);
	}

	private IAsyncResult OnBeginIsOnline(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginIsOnline(callback, asyncState);
	}

	private object[] OnEndIsOnline(IAsyncResult result)
	{
		bool flag = EndIsOnline(result);
		return new object[1] { flag };
	}

	private void OnIsOnlineCompleted(object state)
	{
		if (this.IsOnlineCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.IsOnlineCompleted(this, new IsOnlineCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void IsOnlineAsync()
	{
		IsOnlineAsync(null);
	}

	public void IsOnlineAsync(object userState)
	{
		if (onBeginIsOnlineDelegate == null)
		{
			onBeginIsOnlineDelegate = OnBeginIsOnline;
		}
		if (onEndIsOnlineDelegate == null)
		{
			onEndIsOnlineDelegate = OnEndIsOnline;
		}
		if (onIsOnlineCompletedDelegate == null)
		{
			onIsOnlineCompletedDelegate = OnIsOnlineCompleted;
		}
		InvokeAsync(onBeginIsOnlineDelegate, null, onEndIsOnlineDelegate, onIsOnlineCompletedDelegate, userState);
	}

	public LoginInfo Login(Guid instanceId, string currentToken)
	{
		return base.Channel.Login(instanceId, currentToken);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginLogin(Guid instanceId, string currentToken, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginLogin(instanceId, currentToken, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public LoginInfo EndLogin(IAsyncResult result)
	{
		return base.Channel.EndLogin(result);
	}

	private IAsyncResult OnBeginLogin(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid instanceId = (Guid)inValues[0];
		string currentToken = (string)inValues[1];
		return BeginLogin(instanceId, currentToken, callback, asyncState);
	}

	private object[] OnEndLogin(IAsyncResult result)
	{
		LoginInfo loginInfo = EndLogin(result);
		return new object[1] { loginInfo };
	}

	private void OnLoginCompleted(object state)
	{
		if (this.LoginCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.LoginCompleted(this, new LoginCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void LoginAsync(Guid instanceId, string currentToken)
	{
		LoginAsync(instanceId, currentToken, null);
	}

	public void LoginAsync(Guid instanceId, string currentToken, object userState)
	{
		if (onBeginLoginDelegate == null)
		{
			onBeginLoginDelegate = OnBeginLogin;
		}
		if (onEndLoginDelegate == null)
		{
			onEndLoginDelegate = OnEndLogin;
		}
		if (onLoginCompletedDelegate == null)
		{
			onLoginCompletedDelegate = OnLoginCompleted;
		}
		InvokeAsync(onBeginLoginDelegate, new object[2] { instanceId, currentToken }, onEndLoginDelegate, onLoginCompletedDelegate, userState);
	}

	public void Logout(Guid instanceId, string currentToken)
	{
		base.Channel.Logout(instanceId, currentToken);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginLogout(Guid instanceId, string currentToken, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginLogout(instanceId, currentToken, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndLogout(IAsyncResult result)
	{
		base.Channel.EndLogout(result);
	}

	private IAsyncResult OnBeginLogout(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid instanceId = (Guid)inValues[0];
		string currentToken = (string)inValues[1];
		return BeginLogout(instanceId, currentToken, callback, asyncState);
	}

	private object[] OnEndLogout(IAsyncResult result)
	{
		EndLogout(result);
		return null;
	}

	private void OnLogoutCompleted(object state)
	{
		if (this.LogoutCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.LogoutCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void LogoutAsync(Guid instanceId, string currentToken)
	{
		LogoutAsync(instanceId, currentToken, null);
	}

	public void LogoutAsync(Guid instanceId, string currentToken, object userState)
	{
		if (onBeginLogoutDelegate == null)
		{
			onBeginLogoutDelegate = OnBeginLogout;
		}
		if (onEndLogoutDelegate == null)
		{
			onEndLogoutDelegate = OnEndLogout;
		}
		if (onLogoutCompletedDelegate == null)
		{
			onLogoutCompletedDelegate = OnLogoutCompleted;
		}
		InvokeAsync(onBeginLogoutDelegate, new object[2] { instanceId, currentToken }, onEndLogoutDelegate, onLogoutCompletedDelegate, userState);
	}

	public string Authorize(string userDomainName, AuthorizationActionType type)
	{
		return base.Channel.Authorize(userDomainName, type);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginAuthorize(string userDomainName, AuthorizationActionType type, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginAuthorize(userDomainName, type, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string EndAuthorize(IAsyncResult result)
	{
		return base.Channel.EndAuthorize(result);
	}

	private IAsyncResult OnBeginAuthorize(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string userDomainName = (string)inValues[0];
		AuthorizationActionType type = (AuthorizationActionType)inValues[1];
		return BeginAuthorize(userDomainName, type, callback, asyncState);
	}

	private object[] OnEndAuthorize(IAsyncResult result)
	{
		string text = EndAuthorize(result);
		return new object[1] { text };
	}

	private void OnAuthorizeCompleted(object state)
	{
		if (this.AuthorizeCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.AuthorizeCompleted(this, new AuthorizeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void AuthorizeAsync(string userDomainName, AuthorizationActionType type)
	{
		AuthorizeAsync(userDomainName, type, null);
	}

	public void AuthorizeAsync(string userDomainName, AuthorizationActionType type, object userState)
	{
		if (onBeginAuthorizeDelegate == null)
		{
			onBeginAuthorizeDelegate = OnBeginAuthorize;
		}
		if (onEndAuthorizeDelegate == null)
		{
			onEndAuthorizeDelegate = OnEndAuthorize;
		}
		if (onAuthorizeCompletedDelegate == null)
		{
			onAuthorizeCompletedDelegate = OnAuthorizeCompleted;
		}
		InvokeAsync(onBeginAuthorizeDelegate, new object[2] { userDomainName, type }, onEndAuthorizeDelegate, onAuthorizeCompletedDelegate, userState);
	}

	public UserInfo GetUserInfo(string token)
	{
		return base.Channel.GetUserInfo(token);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetUserInfo(string token, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetUserInfo(token, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public UserInfo EndGetUserInfo(IAsyncResult result)
	{
		return base.Channel.EndGetUserInfo(result);
	}

	private IAsyncResult OnBeginGetUserInfo(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		return BeginGetUserInfo(token, callback, asyncState);
	}

	private object[] OnEndGetUserInfo(IAsyncResult result)
	{
		UserInfo userInfo = EndGetUserInfo(result);
		return new object[1] { userInfo };
	}

	private void OnGetUserInfoCompleted(object state)
	{
		if (this.GetUserInfoCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetUserInfoCompleted(this, new GetUserInfoCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetUserInfoAsync(string token)
	{
		GetUserInfoAsync(token, null);
	}

	public void GetUserInfoAsync(string token, object userState)
	{
		if (onBeginGetUserInfoDelegate == null)
		{
			onBeginGetUserInfoDelegate = OnBeginGetUserInfo;
		}
		if (onEndGetUserInfoDelegate == null)
		{
			onEndGetUserInfoDelegate = OnEndGetUserInfo;
		}
		if (onGetUserInfoCompletedDelegate == null)
		{
			onGetUserInfoCompletedDelegate = OnGetUserInfoCompleted;
		}
		InvokeAsync(onBeginGetUserInfoDelegate, new object[1] { token }, onEndGetUserInfoDelegate, onGetUserInfoCompletedDelegate, userState);
	}

	public ConfigurationInfo GetConfiguration(string token)
	{
		return base.Channel.GetConfiguration(token);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetConfiguration(string token, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetConfiguration(token, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public ConfigurationInfo EndGetConfiguration(IAsyncResult result)
	{
		return base.Channel.EndGetConfiguration(result);
	}

	private IAsyncResult OnBeginGetConfiguration(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		return BeginGetConfiguration(token, callback, asyncState);
	}

	private object[] OnEndGetConfiguration(IAsyncResult result)
	{
		ConfigurationInfo configurationInfo = EndGetConfiguration(result);
		return new object[1] { configurationInfo };
	}

	private void OnGetConfigurationCompleted(object state)
	{
		if (this.GetConfigurationCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetConfigurationCompleted(this, new GetConfigurationCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetConfigurationAsync(string token)
	{
		GetConfigurationAsync(token, null);
	}

	public void GetConfigurationAsync(string token, object userState)
	{
		if (onBeginGetConfigurationDelegate == null)
		{
			onBeginGetConfigurationDelegate = OnBeginGetConfiguration;
		}
		if (onEndGetConfigurationDelegate == null)
		{
			onEndGetConfigurationDelegate = OnEndGetConfiguration;
		}
		if (onGetConfigurationCompletedDelegate == null)
		{
			onGetConfigurationCompletedDelegate = OnGetConfigurationCompleted;
		}
		InvokeAsync(onBeginGetConfigurationDelegate, new object[1] { token }, onEndGetConfigurationDelegate, onGetConfigurationCompletedDelegate, userState);
	}

	public RecorderInfo[] GetConfigurationRecorders(string token, Guid[] recorderIds)
	{
		return base.Channel.GetConfigurationRecorders(token, recorderIds);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetConfigurationRecorders(string token, Guid[] recorderIds, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetConfigurationRecorders(token, recorderIds, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public RecorderInfo[] EndGetConfigurationRecorders(IAsyncResult result)
	{
		return base.Channel.EndGetConfigurationRecorders(result);
	}

	private IAsyncResult OnBeginGetConfigurationRecorders(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid[] recorderIds = (Guid[])inValues[1];
		return BeginGetConfigurationRecorders(token, recorderIds, callback, asyncState);
	}

	private object[] OnEndGetConfigurationRecorders(IAsyncResult result)
	{
		RecorderInfo[] array = EndGetConfigurationRecorders(result);
		return new object[1] { array };
	}

	private void OnGetConfigurationRecordersCompleted(object state)
	{
		if (this.GetConfigurationRecordersCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetConfigurationRecordersCompleted(this, new GetConfigurationRecordersCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetConfigurationRecordersAsync(string token, Guid[] recorderIds)
	{
		GetConfigurationRecordersAsync(token, recorderIds, null);
	}

	public void GetConfigurationRecordersAsync(string token, Guid[] recorderIds, object userState)
	{
		if (onBeginGetConfigurationRecordersDelegate == null)
		{
			onBeginGetConfigurationRecordersDelegate = OnBeginGetConfigurationRecorders;
		}
		if (onEndGetConfigurationRecordersDelegate == null)
		{
			onEndGetConfigurationRecordersDelegate = OnEndGetConfigurationRecorders;
		}
		if (onGetConfigurationRecordersCompletedDelegate == null)
		{
			onGetConfigurationRecordersCompletedDelegate = OnGetConfigurationRecordersCompleted;
		}
		InvokeAsync(onBeginGetConfigurationRecordersDelegate, new object[2] { token, recorderIds }, onEndGetConfigurationRecordersDelegate, onGetConfigurationRecordersCompletedDelegate, userState);
	}

	public HardwareInfo[] GetConfigurationHardware(string token, Guid[] hardwareIds)
	{
		return base.Channel.GetConfigurationHardware(token, hardwareIds);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetConfigurationHardware(string token, Guid[] hardwareIds, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetConfigurationHardware(token, hardwareIds, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public HardwareInfo[] EndGetConfigurationHardware(IAsyncResult result)
	{
		return base.Channel.EndGetConfigurationHardware(result);
	}

	private IAsyncResult OnBeginGetConfigurationHardware(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid[] hardwareIds = (Guid[])inValues[1];
		return BeginGetConfigurationHardware(token, hardwareIds, callback, asyncState);
	}

	private object[] OnEndGetConfigurationHardware(IAsyncResult result)
	{
		HardwareInfo[] array = EndGetConfigurationHardware(result);
		return new object[1] { array };
	}

	private void OnGetConfigurationHardwareCompleted(object state)
	{
		if (this.GetConfigurationHardwareCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetConfigurationHardwareCompleted(this, new GetConfigurationHardwareCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetConfigurationHardwareAsync(string token, Guid[] hardwareIds)
	{
		GetConfigurationHardwareAsync(token, hardwareIds, null);
	}

	public void GetConfigurationHardwareAsync(string token, Guid[] hardwareIds, object userState)
	{
		if (onBeginGetConfigurationHardwareDelegate == null)
		{
			onBeginGetConfigurationHardwareDelegate = OnBeginGetConfigurationHardware;
		}
		if (onEndGetConfigurationHardwareDelegate == null)
		{
			onEndGetConfigurationHardwareDelegate = OnEndGetConfigurationHardware;
		}
		if (onGetConfigurationHardwareCompletedDelegate == null)
		{
			onGetConfigurationHardwareCompletedDelegate = OnGetConfigurationHardwareCompleted;
		}
		InvokeAsync(onBeginGetConfigurationHardwareDelegate, new object[2] { token, hardwareIds }, onEndGetConfigurationHardwareDelegate, onGetConfigurationHardwareCompletedDelegate, userState);
	}

	public DeviceDisabledInfo[] GetDevicesDisabled(string token)
	{
		return base.Channel.GetDevicesDisabled(token);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetDevicesDisabled(string token, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetDevicesDisabled(token, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public DeviceDisabledInfo[] EndGetDevicesDisabled(IAsyncResult result)
	{
		return base.Channel.EndGetDevicesDisabled(result);
	}

	private IAsyncResult OnBeginGetDevicesDisabled(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		return BeginGetDevicesDisabled(token, callback, asyncState);
	}

	private object[] OnEndGetDevicesDisabled(IAsyncResult result)
	{
		DeviceDisabledInfo[] array = EndGetDevicesDisabled(result);
		return new object[1] { array };
	}

	private void OnGetDevicesDisabledCompleted(object state)
	{
		if (this.GetDevicesDisabledCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetDevicesDisabledCompleted(this, new GetDevicesDisabledCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetDevicesDisabledAsync(string token)
	{
		GetDevicesDisabledAsync(token, null);
	}

	public void GetDevicesDisabledAsync(string token, object userState)
	{
		if (onBeginGetDevicesDisabledDelegate == null)
		{
			onBeginGetDevicesDisabledDelegate = OnBeginGetDevicesDisabled;
		}
		if (onEndGetDevicesDisabledDelegate == null)
		{
			onEndGetDevicesDisabledDelegate = OnEndGetDevicesDisabled;
		}
		if (onGetDevicesDisabledCompletedDelegate == null)
		{
			onGetDevicesDisabledCompletedDelegate = OnGetDevicesDisabledCompleted;
		}
		InvokeAsync(onBeginGetDevicesDisabledDelegate, new object[1] { token }, onEndGetDevicesDisabledDelegate, onGetDevicesDisabledCompletedDelegate, userState);
	}

	public System.Collections.DictionaryEntry[] GetVmoServicesList()
	{
		return base.Channel.GetVmoServicesList();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetVmoServicesList(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetVmoServicesList(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public System.Collections.DictionaryEntry[] EndGetVmoServicesList(IAsyncResult result)
	{
		return base.Channel.EndGetVmoServicesList(result);
	}

	private IAsyncResult OnBeginGetVmoServicesList(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetVmoServicesList(callback, asyncState);
	}

	private object[] OnEndGetVmoServicesList(IAsyncResult result)
	{
		System.Collections.DictionaryEntry[] array = EndGetVmoServicesList(result);
		return new object[1] { array };
	}

	private void OnGetVmoServicesListCompleted(object state)
	{
		if (this.GetVmoServicesListCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetVmoServicesListCompleted(this, new GetVmoServicesListCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetVmoServicesListAsync()
	{
		GetVmoServicesListAsync(null);
	}

	public void GetVmoServicesListAsync(object userState)
	{
		if (onBeginGetVmoServicesListDelegate == null)
		{
			onBeginGetVmoServicesListDelegate = OnBeginGetVmoServicesList;
		}
		if (onEndGetVmoServicesListDelegate == null)
		{
			onEndGetVmoServicesListDelegate = OnEndGetVmoServicesList;
		}
		if (onGetVmoServicesListCompletedDelegate == null)
		{
			onGetVmoServicesListCompletedDelegate = OnGetVmoServicesListCompleted;
		}
		InvokeAsync(onBeginGetVmoServicesListDelegate, null, onEndGetVmoServicesListDelegate, onGetVmoServicesListCompletedDelegate, userState);
	}

	public ViewGroupInfo[] GetViewGroups(string token)
	{
		return base.Channel.GetViewGroups(token);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetViewGroups(string token, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetViewGroups(token, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public ViewGroupInfo[] EndGetViewGroups(IAsyncResult result)
	{
		return base.Channel.EndGetViewGroups(result);
	}

	private IAsyncResult OnBeginGetViewGroups(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		return BeginGetViewGroups(token, callback, asyncState);
	}

	private object[] OnEndGetViewGroups(IAsyncResult result)
	{
		ViewGroupInfo[] array = EndGetViewGroups(result);
		return new object[1] { array };
	}

	private void OnGetViewGroupsCompleted(object state)
	{
		if (this.GetViewGroupsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetViewGroupsCompleted(this, new GetViewGroupsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetViewGroupsAsync(string token)
	{
		GetViewGroupsAsync(token, null);
	}

	public void GetViewGroupsAsync(string token, object userState)
	{
		if (onBeginGetViewGroupsDelegate == null)
		{
			onBeginGetViewGroupsDelegate = OnBeginGetViewGroups;
		}
		if (onEndGetViewGroupsDelegate == null)
		{
			onEndGetViewGroupsDelegate = OnEndGetViewGroups;
		}
		if (onGetViewGroupsCompletedDelegate == null)
		{
			onGetViewGroupsCompletedDelegate = OnGetViewGroupsCompleted;
		}
		InvokeAsync(onBeginGetViewGroupsDelegate, new object[1] { token }, onEndGetViewGroupsDelegate, onGetViewGroupsCompletedDelegate, userState);
	}

	public ViewGroupInternal GetViewGroupData(string token, Guid viewGroupId)
	{
		return base.Channel.GetViewGroupData(token, viewGroupId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetViewGroupData(string token, Guid viewGroupId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetViewGroupData(token, viewGroupId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public ViewGroupInternal EndGetViewGroupData(IAsyncResult result)
	{
		return base.Channel.EndGetViewGroupData(result);
	}

	private IAsyncResult OnBeginGetViewGroupData(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid viewGroupId = (Guid)inValues[1];
		return BeginGetViewGroupData(token, viewGroupId, callback, asyncState);
	}

	private object[] OnEndGetViewGroupData(IAsyncResult result)
	{
		ViewGroupInternal viewGroupInternal = EndGetViewGroupData(result);
		return new object[1] { viewGroupInternal };
	}

	private void OnGetViewGroupDataCompleted(object state)
	{
		if (this.GetViewGroupDataCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetViewGroupDataCompleted(this, new GetViewGroupDataCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetViewGroupDataAsync(string token, Guid viewGroupId)
	{
		GetViewGroupDataAsync(token, viewGroupId, null);
	}

	public void GetViewGroupDataAsync(string token, Guid viewGroupId, object userState)
	{
		if (onBeginGetViewGroupDataDelegate == null)
		{
			onBeginGetViewGroupDataDelegate = OnBeginGetViewGroupData;
		}
		if (onEndGetViewGroupDataDelegate == null)
		{
			onEndGetViewGroupDataDelegate = OnEndGetViewGroupData;
		}
		if (onGetViewGroupDataCompletedDelegate == null)
		{
			onGetViewGroupDataCompletedDelegate = OnGetViewGroupDataCompleted;
		}
		InvokeAsync(onBeginGetViewGroupDataDelegate, new object[2] { token, viewGroupId }, onEndGetViewGroupDataDelegate, onGetViewGroupDataCompletedDelegate, userState);
	}

	public SetViewGroupDataResultType SetViewGroupData(string token, Guid viewGroupId, ViewGroupInternalData viewGroupdata)
	{
		return base.Channel.SetViewGroupData(token, viewGroupId, viewGroupdata);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetViewGroupData(string token, Guid viewGroupId, ViewGroupInternalData viewGroupdata, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetViewGroupData(token, viewGroupId, viewGroupdata, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public SetViewGroupDataResultType EndSetViewGroupData(IAsyncResult result)
	{
		return base.Channel.EndSetViewGroupData(result);
	}

	private IAsyncResult OnBeginSetViewGroupData(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid viewGroupId = (Guid)inValues[1];
		ViewGroupInternalData viewGroupdata = (ViewGroupInternalData)inValues[2];
		return BeginSetViewGroupData(token, viewGroupId, viewGroupdata, callback, asyncState);
	}

	private object[] OnEndSetViewGroupData(IAsyncResult result)
	{
		SetViewGroupDataResultType setViewGroupDataResultType = EndSetViewGroupData(result);
		return new object[1] { setViewGroupDataResultType };
	}

	private void OnSetViewGroupDataCompleted(object state)
	{
		if (this.SetViewGroupDataCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetViewGroupDataCompleted(this, new SetViewGroupDataCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetViewGroupDataAsync(string token, Guid viewGroupId, ViewGroupInternalData viewGroupdata)
	{
		SetViewGroupDataAsync(token, viewGroupId, viewGroupdata, null);
	}

	public void SetViewGroupDataAsync(string token, Guid viewGroupId, ViewGroupInternalData viewGroupdata, object userState)
	{
		if (onBeginSetViewGroupDataDelegate == null)
		{
			onBeginSetViewGroupDataDelegate = OnBeginSetViewGroupData;
		}
		if (onEndSetViewGroupDataDelegate == null)
		{
			onEndSetViewGroupDataDelegate = OnEndSetViewGroupData;
		}
		if (onSetViewGroupDataCompletedDelegate == null)
		{
			onSetViewGroupDataCompletedDelegate = OnSetViewGroupDataCompleted;
		}
		InvokeAsync(onBeginSetViewGroupDataDelegate, new object[3] { token, viewGroupId, viewGroupdata }, onEndSetViewGroupDataDelegate, onSetViewGroupDataCompletedDelegate, userState);
	}

	public CustomSettingInternal GetCustomSettingDataGlobal(string token, Guid customSettingId)
	{
		return base.Channel.GetCustomSettingDataGlobal(token, customSettingId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetCustomSettingDataGlobal(string token, Guid customSettingId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetCustomSettingDataGlobal(token, customSettingId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public CustomSettingInternal EndGetCustomSettingDataGlobal(IAsyncResult result)
	{
		return base.Channel.EndGetCustomSettingDataGlobal(result);
	}

	private IAsyncResult OnBeginGetCustomSettingDataGlobal(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid customSettingId = (Guid)inValues[1];
		return BeginGetCustomSettingDataGlobal(token, customSettingId, callback, asyncState);
	}

	private object[] OnEndGetCustomSettingDataGlobal(IAsyncResult result)
	{
		CustomSettingInternal customSettingInternal = EndGetCustomSettingDataGlobal(result);
		return new object[1] { customSettingInternal };
	}

	private void OnGetCustomSettingDataGlobalCompleted(object state)
	{
		if (this.GetCustomSettingDataGlobalCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetCustomSettingDataGlobalCompleted(this, new GetCustomSettingDataGlobalCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetCustomSettingDataGlobalAsync(string token, Guid customSettingId)
	{
		GetCustomSettingDataGlobalAsync(token, customSettingId, null);
	}

	public void GetCustomSettingDataGlobalAsync(string token, Guid customSettingId, object userState)
	{
		if (onBeginGetCustomSettingDataGlobalDelegate == null)
		{
			onBeginGetCustomSettingDataGlobalDelegate = OnBeginGetCustomSettingDataGlobal;
		}
		if (onEndGetCustomSettingDataGlobalDelegate == null)
		{
			onEndGetCustomSettingDataGlobalDelegate = OnEndGetCustomSettingDataGlobal;
		}
		if (onGetCustomSettingDataGlobalCompletedDelegate == null)
		{
			onGetCustomSettingDataGlobalCompletedDelegate = OnGetCustomSettingDataGlobalCompleted;
		}
		InvokeAsync(onBeginGetCustomSettingDataGlobalDelegate, new object[2] { token, customSettingId }, onEndGetCustomSettingDataGlobalDelegate, onGetCustomSettingDataGlobalCompletedDelegate, userState);
	}

	public CustomSettingInternal GetCustomSettingDataUser(string token, Guid customSettingId)
	{
		return base.Channel.GetCustomSettingDataUser(token, customSettingId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetCustomSettingDataUser(string token, Guid customSettingId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetCustomSettingDataUser(token, customSettingId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public CustomSettingInternal EndGetCustomSettingDataUser(IAsyncResult result)
	{
		return base.Channel.EndGetCustomSettingDataUser(result);
	}

	private IAsyncResult OnBeginGetCustomSettingDataUser(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid customSettingId = (Guid)inValues[1];
		return BeginGetCustomSettingDataUser(token, customSettingId, callback, asyncState);
	}

	private object[] OnEndGetCustomSettingDataUser(IAsyncResult result)
	{
		CustomSettingInternal customSettingInternal = EndGetCustomSettingDataUser(result);
		return new object[1] { customSettingInternal };
	}

	private void OnGetCustomSettingDataUserCompleted(object state)
	{
		if (this.GetCustomSettingDataUserCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetCustomSettingDataUserCompleted(this, new GetCustomSettingDataUserCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetCustomSettingDataUserAsync(string token, Guid customSettingId)
	{
		GetCustomSettingDataUserAsync(token, customSettingId, null);
	}

	public void GetCustomSettingDataUserAsync(string token, Guid customSettingId, object userState)
	{
		if (onBeginGetCustomSettingDataUserDelegate == null)
		{
			onBeginGetCustomSettingDataUserDelegate = OnBeginGetCustomSettingDataUser;
		}
		if (onEndGetCustomSettingDataUserDelegate == null)
		{
			onEndGetCustomSettingDataUserDelegate = OnEndGetCustomSettingDataUser;
		}
		if (onGetCustomSettingDataUserCompletedDelegate == null)
		{
			onGetCustomSettingDataUserCompletedDelegate = OnGetCustomSettingDataUserCompleted;
		}
		InvokeAsync(onBeginGetCustomSettingDataUserDelegate, new object[2] { token, customSettingId }, onEndGetCustomSettingDataUserDelegate, onGetCustomSettingDataUserCompletedDelegate, userState);
	}

	public SetCustomSettingResultType SetCustomSettingDataGlobal(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataGlobal(token, customSettingId, customSettingData, forceOverride);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetCustomSettingDataGlobal(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetCustomSettingDataGlobal(token, customSettingId, customSettingData, forceOverride, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public SetCustomSettingResultType EndSetCustomSettingDataGlobal(IAsyncResult result)
	{
		return base.Channel.EndSetCustomSettingDataGlobal(result);
	}

	private IAsyncResult OnBeginSetCustomSettingDataGlobal(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid customSettingId = (Guid)inValues[1];
		CustomSettingInternalData customSettingData = (CustomSettingInternalData)inValues[2];
		bool forceOverride = (bool)inValues[3];
		return BeginSetCustomSettingDataGlobal(token, customSettingId, customSettingData, forceOverride, callback, asyncState);
	}

	private object[] OnEndSetCustomSettingDataGlobal(IAsyncResult result)
	{
		SetCustomSettingResultType setCustomSettingResultType = EndSetCustomSettingDataGlobal(result);
		return new object[1] { setCustomSettingResultType };
	}

	private void OnSetCustomSettingDataGlobalCompleted(object state)
	{
		if (this.SetCustomSettingDataGlobalCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetCustomSettingDataGlobalCompleted(this, new SetCustomSettingDataGlobalCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetCustomSettingDataGlobalAsync(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		SetCustomSettingDataGlobalAsync(token, customSettingId, customSettingData, forceOverride, null);
	}

	public void SetCustomSettingDataGlobalAsync(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride, object userState)
	{
		if (onBeginSetCustomSettingDataGlobalDelegate == null)
		{
			onBeginSetCustomSettingDataGlobalDelegate = OnBeginSetCustomSettingDataGlobal;
		}
		if (onEndSetCustomSettingDataGlobalDelegate == null)
		{
			onEndSetCustomSettingDataGlobalDelegate = OnEndSetCustomSettingDataGlobal;
		}
		if (onSetCustomSettingDataGlobalCompletedDelegate == null)
		{
			onSetCustomSettingDataGlobalCompletedDelegate = OnSetCustomSettingDataGlobalCompleted;
		}
		InvokeAsync(onBeginSetCustomSettingDataGlobalDelegate, new object[4] { token, customSettingId, customSettingData, forceOverride }, onEndSetCustomSettingDataGlobalDelegate, onSetCustomSettingDataGlobalCompletedDelegate, userState);
	}

	public SetCustomSettingResultType SetCustomSettingDataUser(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataUser(token, customSettingId, customSettingData, forceOverride);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetCustomSettingDataUser(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetCustomSettingDataUser(token, customSettingId, customSettingData, forceOverride, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public SetCustomSettingResultType EndSetCustomSettingDataUser(IAsyncResult result)
	{
		return base.Channel.EndSetCustomSettingDataUser(result);
	}

	private IAsyncResult OnBeginSetCustomSettingDataUser(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid customSettingId = (Guid)inValues[1];
		CustomSettingInternalData customSettingData = (CustomSettingInternalData)inValues[2];
		bool forceOverride = (bool)inValues[3];
		return BeginSetCustomSettingDataUser(token, customSettingId, customSettingData, forceOverride, callback, asyncState);
	}

	private object[] OnEndSetCustomSettingDataUser(IAsyncResult result)
	{
		SetCustomSettingResultType setCustomSettingResultType = EndSetCustomSettingDataUser(result);
		return new object[1] { setCustomSettingResultType };
	}

	private void OnSetCustomSettingDataUserCompleted(object state)
	{
		if (this.SetCustomSettingDataUserCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetCustomSettingDataUserCompleted(this, new SetCustomSettingDataUserCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetCustomSettingDataUserAsync(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		SetCustomSettingDataUserAsync(token, customSettingId, customSettingData, forceOverride, null);
	}

	public void SetCustomSettingDataUserAsync(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride, object userState)
	{
		if (onBeginSetCustomSettingDataUserDelegate == null)
		{
			onBeginSetCustomSettingDataUserDelegate = OnBeginSetCustomSettingDataUser;
		}
		if (onEndSetCustomSettingDataUserDelegate == null)
		{
			onEndSetCustomSettingDataUserDelegate = OnEndSetCustomSettingDataUser;
		}
		if (onSetCustomSettingDataUserCompletedDelegate == null)
		{
			onSetCustomSettingDataUserCompletedDelegate = OnSetCustomSettingDataUserCompleted;
		}
		InvokeAsync(onBeginSetCustomSettingDataUserDelegate, new object[4] { token, customSettingId, customSettingData, forceOverride }, onEndSetCustomSettingDataUserDelegate, onSetCustomSettingDataUserCompletedDelegate, userState);
	}

	public void EventTrigger(string token, Guid eventId)
	{
		base.Channel.EventTrigger(token, eventId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginEventTrigger(string token, Guid eventId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginEventTrigger(token, eventId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndEventTrigger(IAsyncResult result)
	{
		base.Channel.EndEventTrigger(result);
	}

	private IAsyncResult OnBeginEventTrigger(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid eventId = (Guid)inValues[1];
		return BeginEventTrigger(token, eventId, callback, asyncState);
	}

	private object[] OnEndEventTrigger(IAsyncResult result)
	{
		EndEventTrigger(result);
		return null;
	}

	private void OnEventTriggerCompleted(object state)
	{
		if (this.EventTriggerCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.EventTriggerCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void EventTriggerAsync(string token, Guid eventId)
	{
		EventTriggerAsync(token, eventId, null);
	}

	public void EventTriggerAsync(string token, Guid eventId, object userState)
	{
		if (onBeginEventTriggerDelegate == null)
		{
			onBeginEventTriggerDelegate = OnBeginEventTrigger;
		}
		if (onEndEventTriggerDelegate == null)
		{
			onEndEventTriggerDelegate = OnEndEventTrigger;
		}
		if (onEventTriggerCompletedDelegate == null)
		{
			onEventTriggerCompletedDelegate = OnEventTriggerCompleted;
		}
		InvokeAsync(onBeginEventTriggerDelegate, new object[2] { token, eventId }, onEndEventTriggerDelegate, onEventTriggerCompletedDelegate, userState);
	}

	public void EventTriggerWithMetadata(string token, Guid eventId, KeyValue[] metadata)
	{
		base.Channel.EventTriggerWithMetadata(token, eventId, metadata);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginEventTriggerWithMetadata(string token, Guid eventId, KeyValue[] metadata, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginEventTriggerWithMetadata(token, eventId, metadata, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndEventTriggerWithMetadata(IAsyncResult result)
	{
		base.Channel.EndEventTriggerWithMetadata(result);
	}

	private IAsyncResult OnBeginEventTriggerWithMetadata(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid eventId = (Guid)inValues[1];
		KeyValue[] metadata = (KeyValue[])inValues[2];
		return BeginEventTriggerWithMetadata(token, eventId, metadata, callback, asyncState);
	}

	private object[] OnEndEventTriggerWithMetadata(IAsyncResult result)
	{
		EndEventTriggerWithMetadata(result);
		return null;
	}

	private void OnEventTriggerWithMetadataCompleted(object state)
	{
		if (this.EventTriggerWithMetadataCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.EventTriggerWithMetadataCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void EventTriggerWithMetadataAsync(string token, Guid eventId, KeyValue[] metadata)
	{
		EventTriggerWithMetadataAsync(token, eventId, metadata, null);
	}

	public void EventTriggerWithMetadataAsync(string token, Guid eventId, KeyValue[] metadata, object userState)
	{
		if (onBeginEventTriggerWithMetadataDelegate == null)
		{
			onBeginEventTriggerWithMetadataDelegate = OnBeginEventTriggerWithMetadata;
		}
		if (onEndEventTriggerWithMetadataDelegate == null)
		{
			onEndEventTriggerWithMetadataDelegate = OnEndEventTriggerWithMetadata;
		}
		if (onEventTriggerWithMetadataCompletedDelegate == null)
		{
			onEventTriggerWithMetadataCompletedDelegate = OnEventTriggerWithMetadataCompleted;
		}
		InvokeAsync(onBeginEventTriggerWithMetadataDelegate, new object[3] { token, eventId, metadata }, onEndEventTriggerWithMetadataDelegate, onEventTriggerWithMetadataCompletedDelegate, userState);
	}

	public AlertOccurrence[] AlertsGetAroundWithSpan(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter)
	{
		return base.Channel.AlertsGetAroundWithSpan(token, alertIds, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginAlertsGetAroundWithSpan(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginAlertsGetAroundWithSpan(token, alertIds, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public AlertOccurrence[] EndAlertsGetAroundWithSpan(IAsyncResult result)
	{
		return base.Channel.EndAlertsGetAroundWithSpan(result);
	}

	private IAsyncResult OnBeginAlertsGetAroundWithSpan(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid[] alertIds = (Guid[])inValues[1];
		DateTime centerTime = (DateTime)inValues[2];
		TimeDuration maxTimeBefore = (TimeDuration)inValues[3];
		int maxCountBefore = (int)inValues[4];
		TimeDuration maxTimeAfter = (TimeDuration)inValues[5];
		int maxCountAfter = (int)inValues[6];
		return BeginAlertsGetAroundWithSpan(token, alertIds, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, callback, asyncState);
	}

	private object[] OnEndAlertsGetAroundWithSpan(IAsyncResult result)
	{
		AlertOccurrence[] array = EndAlertsGetAroundWithSpan(result);
		return new object[1] { array };
	}

	private void OnAlertsGetAroundWithSpanCompleted(object state)
	{
		if (this.AlertsGetAroundWithSpanCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.AlertsGetAroundWithSpanCompleted(this, new AlertsGetAroundWithSpanCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void AlertsGetAroundWithSpanAsync(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter)
	{
		AlertsGetAroundWithSpanAsync(token, alertIds, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, null);
	}

	public void AlertsGetAroundWithSpanAsync(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter, object userState)
	{
		if (onBeginAlertsGetAroundWithSpanDelegate == null)
		{
			onBeginAlertsGetAroundWithSpanDelegate = OnBeginAlertsGetAroundWithSpan;
		}
		if (onEndAlertsGetAroundWithSpanDelegate == null)
		{
			onEndAlertsGetAroundWithSpanDelegate = OnEndAlertsGetAroundWithSpan;
		}
		if (onAlertsGetAroundWithSpanCompletedDelegate == null)
		{
			onAlertsGetAroundWithSpanCompletedDelegate = OnAlertsGetAroundWithSpanCompleted;
		}
		InvokeAsync(onBeginAlertsGetAroundWithSpanDelegate, new object[7] { token, alertIds, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter }, onEndAlertsGetAroundWithSpanDelegate, onAlertsGetAroundWithSpanCompletedDelegate, userState);
	}

	public void MatrixMonitorActivateCamera(string token, Guid matrixMonitorId, Guid deviceId)
	{
		base.Channel.MatrixMonitorActivateCamera(token, matrixMonitorId, deviceId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginMatrixMonitorActivateCamera(string token, Guid matrixMonitorId, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginMatrixMonitorActivateCamera(token, matrixMonitorId, deviceId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndMatrixMonitorActivateCamera(IAsyncResult result)
	{
		base.Channel.EndMatrixMonitorActivateCamera(result);
	}

	private IAsyncResult OnBeginMatrixMonitorActivateCamera(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid matrixMonitorId = (Guid)inValues[1];
		Guid deviceId = (Guid)inValues[2];
		return BeginMatrixMonitorActivateCamera(token, matrixMonitorId, deviceId, callback, asyncState);
	}

	private object[] OnEndMatrixMonitorActivateCamera(IAsyncResult result)
	{
		EndMatrixMonitorActivateCamera(result);
		return null;
	}

	private void OnMatrixMonitorActivateCameraCompleted(object state)
	{
		if (this.MatrixMonitorActivateCameraCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.MatrixMonitorActivateCameraCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void MatrixMonitorActivateCameraAsync(string token, Guid matrixMonitorId, Guid deviceId)
	{
		MatrixMonitorActivateCameraAsync(token, matrixMonitorId, deviceId, null);
	}

	public void MatrixMonitorActivateCameraAsync(string token, Guid matrixMonitorId, Guid deviceId, object userState)
	{
		if (onBeginMatrixMonitorActivateCameraDelegate == null)
		{
			onBeginMatrixMonitorActivateCameraDelegate = OnBeginMatrixMonitorActivateCamera;
		}
		if (onEndMatrixMonitorActivateCameraDelegate == null)
		{
			onEndMatrixMonitorActivateCameraDelegate = OnEndMatrixMonitorActivateCamera;
		}
		if (onMatrixMonitorActivateCameraCompletedDelegate == null)
		{
			onMatrixMonitorActivateCameraCompletedDelegate = OnMatrixMonitorActivateCameraCompleted;
		}
		InvokeAsync(onBeginMatrixMonitorActivateCameraDelegate, new object[3] { token, matrixMonitorId, deviceId }, onEndMatrixMonitorActivateCameraDelegate, onMatrixMonitorActivateCameraCompletedDelegate, userState);
	}

	public Change[] QueryChanges(string token, DateTime changesFrom)
	{
		return base.Channel.QueryChanges(token, changesFrom);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginQueryChanges(string token, DateTime changesFrom, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginQueryChanges(token, changesFrom, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Change[] EndQueryChanges(IAsyncResult result)
	{
		return base.Channel.EndQueryChanges(result);
	}

	private IAsyncResult OnBeginQueryChanges(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		DateTime changesFrom = (DateTime)inValues[1];
		return BeginQueryChanges(token, changesFrom, callback, asyncState);
	}

	private object[] OnEndQueryChanges(IAsyncResult result)
	{
		Change[] array = EndQueryChanges(result);
		return new object[1] { array };
	}

	private void OnQueryChangesCompleted(object state)
	{
		if (this.QueryChangesCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.QueryChangesCompleted(this, new QueryChangesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void QueryChangesAsync(string token, DateTime changesFrom)
	{
		QueryChangesAsync(token, changesFrom, null);
	}

	public void QueryChangesAsync(string token, DateTime changesFrom, object userState)
	{
		if (onBeginQueryChangesDelegate == null)
		{
			onBeginQueryChangesDelegate = OnBeginQueryChanges;
		}
		if (onEndQueryChangesDelegate == null)
		{
			onEndQueryChangesDelegate = OnEndQueryChanges;
		}
		if (onQueryChangesCompletedDelegate == null)
		{
			onQueryChangesCompletedDelegate = OnQueryChangesCompleted;
		}
		InvokeAsync(onBeginQueryChangesDelegate, new object[2] { token, changesFrom }, onEndQueryChangesDelegate, onQueryChangesCompletedDelegate, userState);
	}

	public RecorderInfo QueryRecorderInfo(string token, Guid recorderId)
	{
		return base.Channel.QueryRecorderInfo(token, recorderId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginQueryRecorderInfo(string token, Guid recorderId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginQueryRecorderInfo(token, recorderId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public RecorderInfo EndQueryRecorderInfo(IAsyncResult result)
	{
		return base.Channel.EndQueryRecorderInfo(result);
	}

	private IAsyncResult OnBeginQueryRecorderInfo(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid recorderId = (Guid)inValues[1];
		return BeginQueryRecorderInfo(token, recorderId, callback, asyncState);
	}

	private object[] OnEndQueryRecorderInfo(IAsyncResult result)
	{
		RecorderInfo recorderInfo = EndQueryRecorderInfo(result);
		return new object[1] { recorderInfo };
	}

	private void OnQueryRecorderInfoCompleted(object state)
	{
		if (this.QueryRecorderInfoCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.QueryRecorderInfoCompleted(this, new QueryRecorderInfoCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void QueryRecorderInfoAsync(string token, Guid recorderId)
	{
		QueryRecorderInfoAsync(token, recorderId, null);
	}

	public void QueryRecorderInfoAsync(string token, Guid recorderId, object userState)
	{
		if (onBeginQueryRecorderInfoDelegate == null)
		{
			onBeginQueryRecorderInfoDelegate = OnBeginQueryRecorderInfo;
		}
		if (onEndQueryRecorderInfoDelegate == null)
		{
			onEndQueryRecorderInfoDelegate = OnEndQueryRecorderInfo;
		}
		if (onQueryRecorderInfoCompletedDelegate == null)
		{
			onQueryRecorderInfoCompletedDelegate = OnQueryRecorderInfoCompleted;
		}
		InvokeAsync(onBeginQueryRecorderInfoDelegate, new object[2] { token, recorderId }, onEndQueryRecorderInfoDelegate, onQueryRecorderInfoCompletedDelegate, userState);
	}

	public BookmarkReference BookmarkGetNewReference(string token, Guid deviceId, bool live)
	{
		return base.Channel.BookmarkGetNewReference(token, deviceId, live);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginBookmarkGetNewReference(string token, Guid deviceId, bool live, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginBookmarkGetNewReference(token, deviceId, live, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public BookmarkReference EndBookmarkGetNewReference(IAsyncResult result)
	{
		return base.Channel.EndBookmarkGetNewReference(result);
	}

	private IAsyncResult OnBeginBookmarkGetNewReference(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid deviceId = (Guid)inValues[1];
		bool live = (bool)inValues[2];
		return BeginBookmarkGetNewReference(token, deviceId, live, callback, asyncState);
	}

	private object[] OnEndBookmarkGetNewReference(IAsyncResult result)
	{
		BookmarkReference bookmarkReference = EndBookmarkGetNewReference(result);
		return new object[1] { bookmarkReference };
	}

	private void OnBookmarkGetNewReferenceCompleted(object state)
	{
		if (this.BookmarkGetNewReferenceCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.BookmarkGetNewReferenceCompleted(this, new BookmarkGetNewReferenceCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void BookmarkGetNewReferenceAsync(string token, Guid deviceId, bool live)
	{
		BookmarkGetNewReferenceAsync(token, deviceId, live, null);
	}

	public void BookmarkGetNewReferenceAsync(string token, Guid deviceId, bool live, object userState)
	{
		if (onBeginBookmarkGetNewReferenceDelegate == null)
		{
			onBeginBookmarkGetNewReferenceDelegate = OnBeginBookmarkGetNewReference;
		}
		if (onEndBookmarkGetNewReferenceDelegate == null)
		{
			onEndBookmarkGetNewReferenceDelegate = OnEndBookmarkGetNewReference;
		}
		if (onBookmarkGetNewReferenceCompletedDelegate == null)
		{
			onBookmarkGetNewReferenceCompletedDelegate = OnBookmarkGetNewReferenceCompleted;
		}
		InvokeAsync(onBeginBookmarkGetNewReferenceDelegate, new object[3] { token, deviceId, live }, onEndBookmarkGetNewReferenceDelegate, onBookmarkGetNewReferenceCompletedDelegate, userState);
	}

	public Bookmark BookmarkCreate(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return base.Channel.BookmarkCreate(token, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginBookmarkCreate(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginBookmarkCreate(token, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Bookmark EndBookmarkCreate(IAsyncResult result)
	{
		return base.Channel.EndBookmarkCreate(result);
	}

	private IAsyncResult OnBeginBookmarkCreate(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid deviceId = (Guid)inValues[1];
		DateTime timeBegin = (DateTime)inValues[2];
		DateTime timeTrigged = (DateTime)inValues[3];
		DateTime timeEnd = (DateTime)inValues[4];
		string reference = (string)inValues[5];
		string header = (string)inValues[6];
		string description = (string)inValues[7];
		return BeginBookmarkCreate(token, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description, callback, asyncState);
	}

	private object[] OnEndBookmarkCreate(IAsyncResult result)
	{
		Bookmark bookmark = EndBookmarkCreate(result);
		return new object[1] { bookmark };
	}

	private void OnBookmarkCreateCompleted(object state)
	{
		if (this.BookmarkCreateCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.BookmarkCreateCompleted(this, new BookmarkCreateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void BookmarkCreateAsync(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		BookmarkCreateAsync(token, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description, null);
	}

	public void BookmarkCreateAsync(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description, object userState)
	{
		if (onBeginBookmarkCreateDelegate == null)
		{
			onBeginBookmarkCreateDelegate = OnBeginBookmarkCreate;
		}
		if (onEndBookmarkCreateDelegate == null)
		{
			onEndBookmarkCreateDelegate = OnEndBookmarkCreate;
		}
		if (onBookmarkCreateCompletedDelegate == null)
		{
			onBookmarkCreateCompletedDelegate = OnBookmarkCreateCompleted;
		}
		InvokeAsync(onBeginBookmarkCreateDelegate, new object[8] { token, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description }, onEndBookmarkCreateDelegate, onBookmarkCreateCompletedDelegate, userState);
	}

	public Bookmark BookmarkUpdate(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return base.Channel.BookmarkUpdate(token, bookmarkId, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginBookmarkUpdate(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginBookmarkUpdate(token, bookmarkId, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Bookmark EndBookmarkUpdate(IAsyncResult result)
	{
		return base.Channel.EndBookmarkUpdate(result);
	}

	private IAsyncResult OnBeginBookmarkUpdate(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid bookmarkId = (Guid)inValues[1];
		Guid deviceId = (Guid)inValues[2];
		DateTime timeBegin = (DateTime)inValues[3];
		DateTime timeTrigged = (DateTime)inValues[4];
		DateTime timeEnd = (DateTime)inValues[5];
		string reference = (string)inValues[6];
		string header = (string)inValues[7];
		string description = (string)inValues[8];
		return BeginBookmarkUpdate(token, bookmarkId, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description, callback, asyncState);
	}

	private object[] OnEndBookmarkUpdate(IAsyncResult result)
	{
		Bookmark bookmark = EndBookmarkUpdate(result);
		return new object[1] { bookmark };
	}

	private void OnBookmarkUpdateCompleted(object state)
	{
		if (this.BookmarkUpdateCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.BookmarkUpdateCompleted(this, new BookmarkUpdateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void BookmarkUpdateAsync(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		BookmarkUpdateAsync(token, bookmarkId, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description, null);
	}

	public void BookmarkUpdateAsync(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description, object userState)
	{
		if (onBeginBookmarkUpdateDelegate == null)
		{
			onBeginBookmarkUpdateDelegate = OnBeginBookmarkUpdate;
		}
		if (onEndBookmarkUpdateDelegate == null)
		{
			onEndBookmarkUpdateDelegate = OnEndBookmarkUpdate;
		}
		if (onBookmarkUpdateCompletedDelegate == null)
		{
			onBookmarkUpdateCompletedDelegate = OnBookmarkUpdateCompleted;
		}
		InvokeAsync(onBeginBookmarkUpdateDelegate, new object[9] { token, bookmarkId, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description }, onEndBookmarkUpdateDelegate, onBookmarkUpdateCompletedDelegate, userState);
	}

	public void BookmarkDelete(string token, Guid bookmarkId)
	{
		base.Channel.BookmarkDelete(token, bookmarkId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginBookmarkDelete(string token, Guid bookmarkId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginBookmarkDelete(token, bookmarkId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndBookmarkDelete(IAsyncResult result)
	{
		base.Channel.EndBookmarkDelete(result);
	}

	private IAsyncResult OnBeginBookmarkDelete(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid bookmarkId = (Guid)inValues[1];
		return BeginBookmarkDelete(token, bookmarkId, callback, asyncState);
	}

	private object[] OnEndBookmarkDelete(IAsyncResult result)
	{
		EndBookmarkDelete(result);
		return null;
	}

	private void OnBookmarkDeleteCompleted(object state)
	{
		if (this.BookmarkDeleteCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.BookmarkDeleteCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void BookmarkDeleteAsync(string token, Guid bookmarkId)
	{
		BookmarkDeleteAsync(token, bookmarkId, null);
	}

	public void BookmarkDeleteAsync(string token, Guid bookmarkId, object userState)
	{
		if (onBeginBookmarkDeleteDelegate == null)
		{
			onBeginBookmarkDeleteDelegate = OnBeginBookmarkDelete;
		}
		if (onEndBookmarkDeleteDelegate == null)
		{
			onEndBookmarkDeleteDelegate = OnEndBookmarkDelete;
		}
		if (onBookmarkDeleteCompletedDelegate == null)
		{
			onBookmarkDeleteCompletedDelegate = OnBookmarkDeleteCompleted;
		}
		InvokeAsync(onBeginBookmarkDeleteDelegate, new object[2] { token, bookmarkId }, onEndBookmarkDeleteDelegate, onBookmarkDeleteCompletedDelegate, userState);
	}

	public Bookmark BookmarkGet(string token, Guid bookmarkId)
	{
		return base.Channel.BookmarkGet(token, bookmarkId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginBookmarkGet(string token, Guid bookmarkId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginBookmarkGet(token, bookmarkId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Bookmark EndBookmarkGet(IAsyncResult result)
	{
		return base.Channel.EndBookmarkGet(result);
	}

	private IAsyncResult OnBeginBookmarkGet(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid bookmarkId = (Guid)inValues[1];
		return BeginBookmarkGet(token, bookmarkId, callback, asyncState);
	}

	private object[] OnEndBookmarkGet(IAsyncResult result)
	{
		Bookmark bookmark = EndBookmarkGet(result);
		return new object[1] { bookmark };
	}

	private void OnBookmarkGetCompleted(object state)
	{
		if (this.BookmarkGetCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.BookmarkGetCompleted(this, new BookmarkGetCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void BookmarkGetAsync(string token, Guid bookmarkId)
	{
		BookmarkGetAsync(token, bookmarkId, null);
	}

	public void BookmarkGetAsync(string token, Guid bookmarkId, object userState)
	{
		if (onBeginBookmarkGetDelegate == null)
		{
			onBeginBookmarkGetDelegate = OnBeginBookmarkGet;
		}
		if (onEndBookmarkGetDelegate == null)
		{
			onEndBookmarkGetDelegate = OnEndBookmarkGet;
		}
		if (onBookmarkGetCompletedDelegate == null)
		{
			onBookmarkGetCompletedDelegate = OnBookmarkGetCompleted;
		}
		InvokeAsync(onBeginBookmarkGetDelegate, new object[2] { token, bookmarkId }, onEndBookmarkGetDelegate, onBookmarkGetCompletedDelegate, userState);
	}

	public Bookmark[] BookmarkSearchTime(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return base.Channel.BookmarkSearchTime(token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginBookmarkSearchTime(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginBookmarkSearchTime(token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Bookmark[] EndBookmarkSearchTime(IAsyncResult result)
	{
		return base.Channel.EndBookmarkSearchTime(result);
	}

	private IAsyncResult OnBeginBookmarkSearchTime(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		DateTime time = (DateTime)inValues[1];
		TimeDuration timeLimit = (TimeDuration)inValues[2];
		int countLimit = (int)inValues[3];
		MediaDeviceType[] deviceTypes = (MediaDeviceType[])inValues[4];
		Guid[] optDeviceIds = (Guid[])inValues[5];
		string[] optUsers = (string[])inValues[6];
		string optSearchStr = (string)inValues[7];
		return BeginBookmarkSearchTime(token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, callback, asyncState);
	}

	private object[] OnEndBookmarkSearchTime(IAsyncResult result)
	{
		Bookmark[] array = EndBookmarkSearchTime(result);
		return new object[1] { array };
	}

	private void OnBookmarkSearchTimeCompleted(object state)
	{
		if (this.BookmarkSearchTimeCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.BookmarkSearchTimeCompleted(this, new BookmarkSearchTimeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void BookmarkSearchTimeAsync(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		BookmarkSearchTimeAsync(token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, null);
	}

	public void BookmarkSearchTimeAsync(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, object userState)
	{
		if (onBeginBookmarkSearchTimeDelegate == null)
		{
			onBeginBookmarkSearchTimeDelegate = OnBeginBookmarkSearchTime;
		}
		if (onEndBookmarkSearchTimeDelegate == null)
		{
			onEndBookmarkSearchTimeDelegate = OnEndBookmarkSearchTime;
		}
		if (onBookmarkSearchTimeCompletedDelegate == null)
		{
			onBookmarkSearchTimeCompletedDelegate = OnBookmarkSearchTimeCompleted;
		}
		InvokeAsync(onBeginBookmarkSearchTimeDelegate, new object[8] { token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr }, onEndBookmarkSearchTimeDelegate, onBookmarkSearchTimeCompletedDelegate, userState);
	}

	public Bookmark[] BookmarkSearchTime2(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return base.Channel.BookmarkSearchTime2(token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginBookmarkSearchTime2(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginBookmarkSearchTime2(token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Bookmark[] EndBookmarkSearchTime2(IAsyncResult result)
	{
		return base.Channel.EndBookmarkSearchTime2(result);
	}

	private IAsyncResult OnBeginBookmarkSearchTime2(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		DateTime time = (DateTime)inValues[1];
		TimeDuration timeLimit = (TimeDuration)inValues[2];
		int countLimit = (int)inValues[3];
		MediaDeviceType[] deviceTypes = (MediaDeviceType[])inValues[4];
		Guid[] optDeviceIds = (Guid[])inValues[5];
		string[] optUsers = (string[])inValues[6];
		string optSearchStr = (string)inValues[7];
		bool searchHeader = (bool)inValues[8];
		bool searchDescription = (bool)inValues[9];
		bool searchReference = (bool)inValues[10];
		bool searchUser = (bool)inValues[11];
		string headerFilter = (string)inValues[12];
		string descriptionFilter = (string)inValues[13];
		return BeginBookmarkSearchTime2(token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, callback, asyncState);
	}

	private object[] OnEndBookmarkSearchTime2(IAsyncResult result)
	{
		Bookmark[] array = EndBookmarkSearchTime2(result);
		return new object[1] { array };
	}

	private void OnBookmarkSearchTime2Completed(object state)
	{
		if (this.BookmarkSearchTime2Completed != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.BookmarkSearchTime2Completed(this, new BookmarkSearchTime2CompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void BookmarkSearchTime2Async(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		BookmarkSearchTime2Async(token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, null);
	}

	public void BookmarkSearchTime2Async(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, object userState)
	{
		if (onBeginBookmarkSearchTime2Delegate == null)
		{
			onBeginBookmarkSearchTime2Delegate = OnBeginBookmarkSearchTime2;
		}
		if (onEndBookmarkSearchTime2Delegate == null)
		{
			onEndBookmarkSearchTime2Delegate = OnEndBookmarkSearchTime2;
		}
		if (onBookmarkSearchTime2CompletedDelegate == null)
		{
			onBookmarkSearchTime2CompletedDelegate = OnBookmarkSearchTime2Completed;
		}
		InvokeAsync(onBeginBookmarkSearchTime2Delegate, new object[14]
		{
			token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription,
			searchReference, searchUser, headerFilter, descriptionFilter
		}, onEndBookmarkSearchTime2Delegate, onBookmarkSearchTime2CompletedDelegate, userState);
	}

	public Bookmark[] BookmarkSearchFromBookmark(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return base.Channel.BookmarkSearchFromBookmark(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginBookmarkSearchFromBookmark(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginBookmarkSearchFromBookmark(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Bookmark[] EndBookmarkSearchFromBookmark(IAsyncResult result)
	{
		return base.Channel.EndBookmarkSearchFromBookmark(result);
	}

	private IAsyncResult OnBeginBookmarkSearchFromBookmark(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid fromBookmarkId = (Guid)inValues[1];
		TimeDuration timeLimit = (TimeDuration)inValues[2];
		int countLimit = (int)inValues[3];
		MediaDeviceType[] deviceTypes = (MediaDeviceType[])inValues[4];
		Guid[] optDeviceIds = (Guid[])inValues[5];
		string[] optUsers = (string[])inValues[6];
		string optSearchStr = (string)inValues[7];
		return BeginBookmarkSearchFromBookmark(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, callback, asyncState);
	}

	private object[] OnEndBookmarkSearchFromBookmark(IAsyncResult result)
	{
		Bookmark[] array = EndBookmarkSearchFromBookmark(result);
		return new object[1] { array };
	}

	private void OnBookmarkSearchFromBookmarkCompleted(object state)
	{
		if (this.BookmarkSearchFromBookmarkCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.BookmarkSearchFromBookmarkCompleted(this, new BookmarkSearchFromBookmarkCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void BookmarkSearchFromBookmarkAsync(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		BookmarkSearchFromBookmarkAsync(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, null);
	}

	public void BookmarkSearchFromBookmarkAsync(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, object userState)
	{
		if (onBeginBookmarkSearchFromBookmarkDelegate == null)
		{
			onBeginBookmarkSearchFromBookmarkDelegate = OnBeginBookmarkSearchFromBookmark;
		}
		if (onEndBookmarkSearchFromBookmarkDelegate == null)
		{
			onEndBookmarkSearchFromBookmarkDelegate = OnEndBookmarkSearchFromBookmark;
		}
		if (onBookmarkSearchFromBookmarkCompletedDelegate == null)
		{
			onBookmarkSearchFromBookmarkCompletedDelegate = OnBookmarkSearchFromBookmarkCompleted;
		}
		InvokeAsync(onBeginBookmarkSearchFromBookmarkDelegate, new object[8] { token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr }, onEndBookmarkSearchFromBookmarkDelegate, onBookmarkSearchFromBookmarkCompletedDelegate, userState);
	}

	public Bookmark[] BookmarkSearchFromBookmark2(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return base.Channel.BookmarkSearchFromBookmark2(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginBookmarkSearchFromBookmark2(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginBookmarkSearchFromBookmark2(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Bookmark[] EndBookmarkSearchFromBookmark2(IAsyncResult result)
	{
		return base.Channel.EndBookmarkSearchFromBookmark2(result);
	}

	private IAsyncResult OnBeginBookmarkSearchFromBookmark2(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid fromBookmarkId = (Guid)inValues[1];
		TimeDuration timeLimit = (TimeDuration)inValues[2];
		int countLimit = (int)inValues[3];
		MediaDeviceType[] deviceTypes = (MediaDeviceType[])inValues[4];
		Guid[] optDeviceIds = (Guid[])inValues[5];
		string[] optUsers = (string[])inValues[6];
		string optSearchStr = (string)inValues[7];
		bool searchHeader = (bool)inValues[8];
		bool searchDescription = (bool)inValues[9];
		bool searchReference = (bool)inValues[10];
		bool searchUser = (bool)inValues[11];
		string headerFilter = (string)inValues[12];
		string descriptionFilter = (string)inValues[13];
		return BeginBookmarkSearchFromBookmark2(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, callback, asyncState);
	}

	private object[] OnEndBookmarkSearchFromBookmark2(IAsyncResult result)
	{
		Bookmark[] array = EndBookmarkSearchFromBookmark2(result);
		return new object[1] { array };
	}

	private void OnBookmarkSearchFromBookmark2Completed(object state)
	{
		if (this.BookmarkSearchFromBookmark2Completed != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.BookmarkSearchFromBookmark2Completed(this, new BookmarkSearchFromBookmark2CompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void BookmarkSearchFromBookmark2Async(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		BookmarkSearchFromBookmark2Async(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, null);
	}

	public void BookmarkSearchFromBookmark2Async(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, object userState)
	{
		if (onBeginBookmarkSearchFromBookmark2Delegate == null)
		{
			onBeginBookmarkSearchFromBookmark2Delegate = OnBeginBookmarkSearchFromBookmark2;
		}
		if (onEndBookmarkSearchFromBookmark2Delegate == null)
		{
			onEndBookmarkSearchFromBookmark2Delegate = OnEndBookmarkSearchFromBookmark2;
		}
		if (onBookmarkSearchFromBookmark2CompletedDelegate == null)
		{
			onBookmarkSearchFromBookmark2CompletedDelegate = OnBookmarkSearchFromBookmark2Completed;
		}
		InvokeAsync(onBeginBookmarkSearchFromBookmark2Delegate, new object[14]
		{
			token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription,
			searchReference, searchUser, headerFilter, descriptionFilter
		}, onEndBookmarkSearchFromBookmark2Delegate, onBookmarkSearchFromBookmark2CompletedDelegate, userState);
	}

	public void RegisterIntegration(string token, string instanceId, Guid integrationId, string integrationVersion, string integrationName, string manufacturerName)
	{
		base.Channel.RegisterIntegration(token, instanceId, integrationId, integrationVersion, integrationName, manufacturerName);
	}

	public Task RegisterIntegrationAsync(string token, string instanceId, Guid integrationId, string integrationVersion, string integrationName, string manufacturerName)
	{
		return base.Channel.RegisterIntegrationAsync(token, instanceId, integrationId, integrationVersion, integrationName, manufacturerName);
	}

	public void RegisterPluginIntegrations(string token, string instanceId, IntegrationType integrationType, PluginIntegration[] pluginIntegrations)
	{
		base.Channel.RegisterPluginIntegrations(token, instanceId, integrationType, pluginIntegrations);
	}

	public Task RegisterPluginIntegrationsAsync(string token, string instanceId, IntegrationType integrationType, PluginIntegration[] pluginIntegrations)
	{
		return base.Channel.RegisterPluginIntegrationsAsync(token, instanceId, integrationType, pluginIntegrations);
	}

	public MarkedDataReference MarkedDataGetNewReference(string token, Guid[] deviceId, bool live)
	{
		return base.Channel.MarkedDataGetNewReference(token, deviceId, live);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginMarkedDataGetNewReference(string token, Guid[] deviceId, bool live, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginMarkedDataGetNewReference(token, deviceId, live, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public MarkedDataReference EndMarkedDataGetNewReference(IAsyncResult result)
	{
		return base.Channel.EndMarkedDataGetNewReference(result);
	}

	private IAsyncResult OnBeginMarkedDataGetNewReference(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid[] deviceId = (Guid[])inValues[1];
		bool live = (bool)inValues[2];
		return BeginMarkedDataGetNewReference(token, deviceId, live, callback, asyncState);
	}

	private object[] OnEndMarkedDataGetNewReference(IAsyncResult result)
	{
		MarkedDataReference markedDataReference = EndMarkedDataGetNewReference(result);
		return new object[1] { markedDataReference };
	}

	private void OnMarkedDataGetNewReferenceCompleted(object state)
	{
		if (this.MarkedDataGetNewReferenceCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.MarkedDataGetNewReferenceCompleted(this, new MarkedDataGetNewReferenceCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void MarkedDataGetNewReferenceAsync(string token, Guid[] deviceId, bool live)
	{
		MarkedDataGetNewReferenceAsync(token, deviceId, live, null);
	}

	public void MarkedDataGetNewReferenceAsync(string token, Guid[] deviceId, bool live, object userState)
	{
		if (onBeginMarkedDataGetNewReferenceDelegate == null)
		{
			onBeginMarkedDataGetNewReferenceDelegate = OnBeginMarkedDataGetNewReference;
		}
		if (onEndMarkedDataGetNewReferenceDelegate == null)
		{
			onEndMarkedDataGetNewReferenceDelegate = OnEndMarkedDataGetNewReference;
		}
		if (onMarkedDataGetNewReferenceCompletedDelegate == null)
		{
			onMarkedDataGetNewReferenceCompletedDelegate = OnMarkedDataGetNewReferenceCompleted;
		}
		InvokeAsync(onBeginMarkedDataGetNewReferenceDelegate, new object[3] { token, deviceId, live }, onEndMarkedDataGetNewReferenceDelegate, onMarkedDataGetNewReferenceCompletedDelegate, userState);
	}

	public MarkedDataResult MarkedDataCreate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		return base.Channel.MarkedDataCreate(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginMarkedDataCreate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginMarkedDataCreate(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public MarkedDataResult EndMarkedDataCreate(IAsyncResult result)
	{
		return base.Channel.EndMarkedDataCreate(result);
	}

	private IAsyncResult OnBeginMarkedDataCreate(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid markedDataId = (Guid)inValues[1];
		Guid[] deviceIds = (Guid[])inValues[2];
		DateTime timeStart = (DateTime)inValues[3];
		DateTime timeTag = (DateTime)inValues[4];
		DateTime timeEnd = (DateTime)inValues[5];
		string reference = (string)inValues[6];
		string header = (string)inValues[7];
		string description = (string)inValues[8];
		int markedDataType = (int)inValues[9];
		bool useRetention = (bool)inValues[10];
		DateTime retentionExpire = (DateTime)inValues[11];
		RetentionOption retentionOption = (RetentionOption)inValues[12];
		return BeginMarkedDataCreate(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption, callback, asyncState);
	}

	private object[] OnEndMarkedDataCreate(IAsyncResult result)
	{
		MarkedDataResult markedDataResult = EndMarkedDataCreate(result);
		return new object[1] { markedDataResult };
	}

	private void OnMarkedDataCreateCompleted(object state)
	{
		if (this.MarkedDataCreateCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.MarkedDataCreateCompleted(this, new MarkedDataCreateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void MarkedDataCreateAsync(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		MarkedDataCreateAsync(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption, null);
	}

	public void MarkedDataCreateAsync(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption, object userState)
	{
		if (onBeginMarkedDataCreateDelegate == null)
		{
			onBeginMarkedDataCreateDelegate = OnBeginMarkedDataCreate;
		}
		if (onEndMarkedDataCreateDelegate == null)
		{
			onEndMarkedDataCreateDelegate = OnEndMarkedDataCreate;
		}
		if (onMarkedDataCreateCompletedDelegate == null)
		{
			onMarkedDataCreateCompletedDelegate = OnMarkedDataCreateCompleted;
		}
		InvokeAsync(onBeginMarkedDataCreateDelegate, new object[13]
		{
			token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType,
			useRetention, retentionExpire, retentionOption
		}, onEndMarkedDataCreateDelegate, onMarkedDataCreateCompletedDelegate, userState);
	}

	public MarkedDataResult MarkedDataUpdate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		return base.Channel.MarkedDataUpdate(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginMarkedDataUpdate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginMarkedDataUpdate(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public MarkedDataResult EndMarkedDataUpdate(IAsyncResult result)
	{
		return base.Channel.EndMarkedDataUpdate(result);
	}

	private IAsyncResult OnBeginMarkedDataUpdate(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid markedDataId = (Guid)inValues[1];
		Guid[] deviceIds = (Guid[])inValues[2];
		DateTime timeStart = (DateTime)inValues[3];
		DateTime timeTag = (DateTime)inValues[4];
		DateTime timeEnd = (DateTime)inValues[5];
		string reference = (string)inValues[6];
		string header = (string)inValues[7];
		string description = (string)inValues[8];
		int markedDataType = (int)inValues[9];
		bool useRetention = (bool)inValues[10];
		DateTime retentionExpire = (DateTime)inValues[11];
		RetentionOption retentionOption = (RetentionOption)inValues[12];
		return BeginMarkedDataUpdate(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption, callback, asyncState);
	}

	private object[] OnEndMarkedDataUpdate(IAsyncResult result)
	{
		MarkedDataResult markedDataResult = EndMarkedDataUpdate(result);
		return new object[1] { markedDataResult };
	}

	private void OnMarkedDataUpdateCompleted(object state)
	{
		if (this.MarkedDataUpdateCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.MarkedDataUpdateCompleted(this, new MarkedDataUpdateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void MarkedDataUpdateAsync(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		MarkedDataUpdateAsync(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption, null);
	}

	public void MarkedDataUpdateAsync(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption, object userState)
	{
		if (onBeginMarkedDataUpdateDelegate == null)
		{
			onBeginMarkedDataUpdateDelegate = OnBeginMarkedDataUpdate;
		}
		if (onEndMarkedDataUpdateDelegate == null)
		{
			onEndMarkedDataUpdateDelegate = OnEndMarkedDataUpdate;
		}
		if (onMarkedDataUpdateCompletedDelegate == null)
		{
			onMarkedDataUpdateCompletedDelegate = OnMarkedDataUpdateCompleted;
		}
		InvokeAsync(onBeginMarkedDataUpdateDelegate, new object[13]
		{
			token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType,
			useRetention, retentionExpire, retentionOption
		}, onEndMarkedDataUpdateDelegate, onMarkedDataUpdateCompletedDelegate, userState);
	}

	public MarkedDataResult[] MarkedDataDelete(string token, Guid[] markedDataIds)
	{
		return base.Channel.MarkedDataDelete(token, markedDataIds);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginMarkedDataDelete(string token, Guid[] markedDataIds, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginMarkedDataDelete(token, markedDataIds, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public MarkedDataResult[] EndMarkedDataDelete(IAsyncResult result)
	{
		return base.Channel.EndMarkedDataDelete(result);
	}

	private IAsyncResult OnBeginMarkedDataDelete(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid[] markedDataIds = (Guid[])inValues[1];
		return BeginMarkedDataDelete(token, markedDataIds, callback, asyncState);
	}

	private object[] OnEndMarkedDataDelete(IAsyncResult result)
	{
		MarkedDataResult[] array = EndMarkedDataDelete(result);
		return new object[1] { array };
	}

	private void OnMarkedDataDeleteCompleted(object state)
	{
		if (this.MarkedDataDeleteCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.MarkedDataDeleteCompleted(this, new MarkedDataDeleteCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void MarkedDataDeleteAsync(string token, Guid[] markedDataIds)
	{
		MarkedDataDeleteAsync(token, markedDataIds, null);
	}

	public void MarkedDataDeleteAsync(string token, Guid[] markedDataIds, object userState)
	{
		if (onBeginMarkedDataDeleteDelegate == null)
		{
			onBeginMarkedDataDeleteDelegate = OnBeginMarkedDataDelete;
		}
		if (onEndMarkedDataDeleteDelegate == null)
		{
			onEndMarkedDataDeleteDelegate = OnEndMarkedDataDelete;
		}
		if (onMarkedDataDeleteCompletedDelegate == null)
		{
			onMarkedDataDeleteCompletedDelegate = OnMarkedDataDeleteCompleted;
		}
		InvokeAsync(onBeginMarkedDataDeleteDelegate, new object[2] { token, markedDataIds }, onEndMarkedDataDeleteDelegate, onMarkedDataDeleteCompletedDelegate, userState);
	}

	public MarkedData MarkedDataGet(string token, Guid markedDataId)
	{
		return base.Channel.MarkedDataGet(token, markedDataId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginMarkedDataGet(string token, Guid markedDataId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginMarkedDataGet(token, markedDataId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public MarkedData EndMarkedDataGet(IAsyncResult result)
	{
		return base.Channel.EndMarkedDataGet(result);
	}

	private IAsyncResult OnBeginMarkedDataGet(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid markedDataId = (Guid)inValues[1];
		return BeginMarkedDataGet(token, markedDataId, callback, asyncState);
	}

	private object[] OnEndMarkedDataGet(IAsyncResult result)
	{
		MarkedData markedData = EndMarkedDataGet(result);
		return new object[1] { markedData };
	}

	private void OnMarkedDataGetCompleted(object state)
	{
		if (this.MarkedDataGetCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.MarkedDataGetCompleted(this, new MarkedDataGetCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void MarkedDataGetAsync(string token, Guid markedDataId)
	{
		MarkedDataGetAsync(token, markedDataId, null);
	}

	public void MarkedDataGetAsync(string token, Guid markedDataId, object userState)
	{
		if (onBeginMarkedDataGetDelegate == null)
		{
			onBeginMarkedDataGetDelegate = OnBeginMarkedDataGet;
		}
		if (onEndMarkedDataGetDelegate == null)
		{
			onEndMarkedDataGetDelegate = OnEndMarkedDataGet;
		}
		if (onMarkedDataGetCompletedDelegate == null)
		{
			onMarkedDataGetCompletedDelegate = OnMarkedDataGetCompleted;
		}
		InvokeAsync(onBeginMarkedDataGetDelegate, new object[2] { token, markedDataId }, onEndMarkedDataGetDelegate, onMarkedDataGetCompletedDelegate, userState);
	}

	public MarkedData[] MarkedDataSearch(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		return base.Channel.MarkedDataSearch(token, deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginMarkedDataSearch(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginMarkedDataSearch(token, deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public MarkedData[] EndMarkedDataSearch(IAsyncResult result)
	{
		return base.Channel.EndMarkedDataSearch(result);
	}

	private IAsyncResult OnBeginMarkedDataSearch(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid[] deviceIds = (Guid[])inValues[1];
		string searchText = (string)inValues[2];
		string[] users = (string[])inValues[3];
		DateTime createdFromTime = (DateTime)inValues[4];
		DateTime createdToTime = (DateTime)inValues[5];
		DateTime markedDataFromTime = (DateTime)inValues[6];
		DateTime markedDataToTime = (DateTime)inValues[7];
		DateTime tagTimeFromTime = (DateTime)inValues[8];
		DateTime tagTimeToTime = (DateTime)inValues[9];
		DateTime retentionExpireFrom = (DateTime)inValues[10];
		DateTime retentionExpireTo = (DateTime)inValues[11];
		int pageIndex = (int)inValues[12];
		int pageSize = (int)inValues[13];
		SortOrderOption sortOrder = (SortOrderOption)inValues[14];
		bool sortAscending = (bool)inValues[15];
		return BeginMarkedDataSearch(token, deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending, callback, asyncState);
	}

	private object[] OnEndMarkedDataSearch(IAsyncResult result)
	{
		MarkedData[] array = EndMarkedDataSearch(result);
		return new object[1] { array };
	}

	private void OnMarkedDataSearchCompleted(object state)
	{
		if (this.MarkedDataSearchCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.MarkedDataSearchCompleted(this, new MarkedDataSearchCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void MarkedDataSearchAsync(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		MarkedDataSearchAsync(token, deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending, null);
	}

	public void MarkedDataSearchAsync(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending, object userState)
	{
		if (onBeginMarkedDataSearchDelegate == null)
		{
			onBeginMarkedDataSearchDelegate = OnBeginMarkedDataSearch;
		}
		if (onEndMarkedDataSearchDelegate == null)
		{
			onEndMarkedDataSearchDelegate = OnEndMarkedDataSearch;
		}
		if (onMarkedDataSearchCompletedDelegate == null)
		{
			onMarkedDataSearchCompletedDelegate = OnMarkedDataSearchCompleted;
		}
		InvokeAsync(onBeginMarkedDataSearchDelegate, new object[16]
		{
			token, deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime,
			retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending
		}, onEndMarkedDataSearchDelegate, onMarkedDataSearchCompletedDelegate, userState);
	}

	public RestrictedMediaResult RestrictedMediaCreate(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return base.Channel.RestrictedMediaCreate(id, deviceIds, header, description, timeStart, timeEnd);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRestrictedMediaCreate(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRestrictedMediaCreate(id, deviceIds, header, description, timeStart, timeEnd, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public RestrictedMediaResult EndRestrictedMediaCreate(IAsyncResult result)
	{
		return base.Channel.EndRestrictedMediaCreate(result);
	}

	private IAsyncResult OnBeginRestrictedMediaCreate(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid id = (Guid)inValues[0];
		Guid[] deviceIds = (Guid[])inValues[1];
		string header = (string)inValues[2];
		string description = (string)inValues[3];
		DateTime timeStart = (DateTime)inValues[4];
		DateTime timeEnd = (DateTime)inValues[5];
		return BeginRestrictedMediaCreate(id, deviceIds, header, description, timeStart, timeEnd, callback, asyncState);
	}

	private object[] OnEndRestrictedMediaCreate(IAsyncResult result)
	{
		RestrictedMediaResult restrictedMediaResult = EndRestrictedMediaCreate(result);
		return new object[1] { restrictedMediaResult };
	}

	private void OnRestrictedMediaCreateCompleted(object state)
	{
		if (this.RestrictedMediaCreateCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RestrictedMediaCreateCompleted(this, new RestrictedMediaCreateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RestrictedMediaCreateAsync(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		RestrictedMediaCreateAsync(id, deviceIds, header, description, timeStart, timeEnd, null);
	}

	public void RestrictedMediaCreateAsync(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd, object userState)
	{
		if (onBeginRestrictedMediaCreateDelegate == null)
		{
			onBeginRestrictedMediaCreateDelegate = OnBeginRestrictedMediaCreate;
		}
		if (onEndRestrictedMediaCreateDelegate == null)
		{
			onEndRestrictedMediaCreateDelegate = OnEndRestrictedMediaCreate;
		}
		if (onRestrictedMediaCreateCompletedDelegate == null)
		{
			onRestrictedMediaCreateCompletedDelegate = OnRestrictedMediaCreateCompleted;
		}
		InvokeAsync(onBeginRestrictedMediaCreateDelegate, new object[6] { id, deviceIds, header, description, timeStart, timeEnd }, onEndRestrictedMediaCreateDelegate, onRestrictedMediaCreateCompletedDelegate, userState);
	}

	public RestrictedMediaResult RestrictedMediaUpdate(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return base.Channel.RestrictedMediaUpdate(id, deviceIdsToAdd, deviceIdsToRemove, header, description, timeStart, timeEnd);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRestrictedMediaUpdate(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRestrictedMediaUpdate(id, deviceIdsToAdd, deviceIdsToRemove, header, description, timeStart, timeEnd, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public RestrictedMediaResult EndRestrictedMediaUpdate(IAsyncResult result)
	{
		return base.Channel.EndRestrictedMediaUpdate(result);
	}

	private IAsyncResult OnBeginRestrictedMediaUpdate(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid id = (Guid)inValues[0];
		Guid[] deviceIdsToAdd = (Guid[])inValues[1];
		Guid[] deviceIdsToRemove = (Guid[])inValues[2];
		string header = (string)inValues[3];
		string description = (string)inValues[4];
		DateTime timeStart = (DateTime)inValues[5];
		DateTime timeEnd = (DateTime)inValues[6];
		return BeginRestrictedMediaUpdate(id, deviceIdsToAdd, deviceIdsToRemove, header, description, timeStart, timeEnd, callback, asyncState);
	}

	private object[] OnEndRestrictedMediaUpdate(IAsyncResult result)
	{
		RestrictedMediaResult restrictedMediaResult = EndRestrictedMediaUpdate(result);
		return new object[1] { restrictedMediaResult };
	}

	private void OnRestrictedMediaUpdateCompleted(object state)
	{
		if (this.RestrictedMediaUpdateCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RestrictedMediaUpdateCompleted(this, new RestrictedMediaUpdateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RestrictedMediaUpdateAsync(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		RestrictedMediaUpdateAsync(id, deviceIdsToAdd, deviceIdsToRemove, header, description, timeStart, timeEnd, null);
	}

	public void RestrictedMediaUpdateAsync(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd, object userState)
	{
		if (onBeginRestrictedMediaUpdateDelegate == null)
		{
			onBeginRestrictedMediaUpdateDelegate = OnBeginRestrictedMediaUpdate;
		}
		if (onEndRestrictedMediaUpdateDelegate == null)
		{
			onEndRestrictedMediaUpdateDelegate = OnEndRestrictedMediaUpdate;
		}
		if (onRestrictedMediaUpdateCompletedDelegate == null)
		{
			onRestrictedMediaUpdateCompletedDelegate = OnRestrictedMediaUpdateCompleted;
		}
		InvokeAsync(onBeginRestrictedMediaUpdateDelegate, new object[7] { id, deviceIdsToAdd, deviceIdsToRemove, header, description, timeStart, timeEnd }, onEndRestrictedMediaUpdateDelegate, onRestrictedMediaUpdateCompletedDelegate, userState);
	}

	public RestrictedMediaResult RestrictedMediaDelete(Guid restrictedMediaId)
	{
		return base.Channel.RestrictedMediaDelete(restrictedMediaId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRestrictedMediaDelete(Guid restrictedMediaId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRestrictedMediaDelete(restrictedMediaId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public RestrictedMediaResult EndRestrictedMediaDelete(IAsyncResult result)
	{
		return base.Channel.EndRestrictedMediaDelete(result);
	}

	private IAsyncResult OnBeginRestrictedMediaDelete(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid restrictedMediaId = (Guid)inValues[0];
		return BeginRestrictedMediaDelete(restrictedMediaId, callback, asyncState);
	}

	private object[] OnEndRestrictedMediaDelete(IAsyncResult result)
	{
		RestrictedMediaResult restrictedMediaResult = EndRestrictedMediaDelete(result);
		return new object[1] { restrictedMediaResult };
	}

	private void OnRestrictedMediaDeleteCompleted(object state)
	{
		if (this.RestrictedMediaDeleteCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RestrictedMediaDeleteCompleted(this, new RestrictedMediaDeleteCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RestrictedMediaDeleteAsync(Guid restrictedMediaId)
	{
		RestrictedMediaDeleteAsync(restrictedMediaId, null);
	}

	public void RestrictedMediaDeleteAsync(Guid restrictedMediaId, object userState)
	{
		if (onBeginRestrictedMediaDeleteDelegate == null)
		{
			onBeginRestrictedMediaDeleteDelegate = OnBeginRestrictedMediaDelete;
		}
		if (onEndRestrictedMediaDeleteDelegate == null)
		{
			onEndRestrictedMediaDeleteDelegate = OnEndRestrictedMediaDelete;
		}
		if (onRestrictedMediaDeleteCompletedDelegate == null)
		{
			onRestrictedMediaDeleteCompletedDelegate = OnRestrictedMediaDeleteCompleted;
		}
		InvokeAsync(onBeginRestrictedMediaDeleteDelegate, new object[1] { restrictedMediaId }, onEndRestrictedMediaDeleteDelegate, onRestrictedMediaDeleteCompletedDelegate, userState);
	}

	public RestrictedMedia[] RestrictedMediaQueryAll()
	{
		return base.Channel.RestrictedMediaQueryAll();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRestrictedMediaQueryAll(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRestrictedMediaQueryAll(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public RestrictedMedia[] EndRestrictedMediaQueryAll(IAsyncResult result)
	{
		return base.Channel.EndRestrictedMediaQueryAll(result);
	}

	private IAsyncResult OnBeginRestrictedMediaQueryAll(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginRestrictedMediaQueryAll(callback, asyncState);
	}

	private object[] OnEndRestrictedMediaQueryAll(IAsyncResult result)
	{
		RestrictedMedia[] array = EndRestrictedMediaQueryAll(result);
		return new object[1] { array };
	}

	private void OnRestrictedMediaQueryAllCompleted(object state)
	{
		if (this.RestrictedMediaQueryAllCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RestrictedMediaQueryAllCompleted(this, new RestrictedMediaQueryAllCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RestrictedMediaQueryAllAsync()
	{
		RestrictedMediaQueryAllAsync(null);
	}

	public void RestrictedMediaQueryAllAsync(object userState)
	{
		if (onBeginRestrictedMediaQueryAllDelegate == null)
		{
			onBeginRestrictedMediaQueryAllDelegate = OnBeginRestrictedMediaQueryAll;
		}
		if (onEndRestrictedMediaQueryAllDelegate == null)
		{
			onEndRestrictedMediaQueryAllDelegate = OnEndRestrictedMediaQueryAll;
		}
		if (onRestrictedMediaQueryAllCompletedDelegate == null)
		{
			onRestrictedMediaQueryAllCompletedDelegate = OnRestrictedMediaQueryAllCompleted;
		}
		InvokeAsync(onBeginRestrictedMediaQueryAllDelegate, null, onEndRestrictedMediaQueryAllDelegate, onRestrictedMediaQueryAllCompletedDelegate, userState);
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime timeStart)
	{
		return base.Channel.RestrictedMediaLiveModeEnter(deviceIds, timeStart);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime timeStart, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRestrictedMediaLiveModeEnter(deviceIds, timeStart, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public RestrictedMediaLiveResult EndRestrictedMediaLiveModeEnter(IAsyncResult result)
	{
		return base.Channel.EndRestrictedMediaLiveModeEnter(result);
	}

	private IAsyncResult OnBeginRestrictedMediaLiveModeEnter(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid[] deviceIds = (Guid[])inValues[0];
		DateTime timeStart = (DateTime)inValues[1];
		return BeginRestrictedMediaLiveModeEnter(deviceIds, timeStart, callback, asyncState);
	}

	private object[] OnEndRestrictedMediaLiveModeEnter(IAsyncResult result)
	{
		RestrictedMediaLiveResult restrictedMediaLiveResult = EndRestrictedMediaLiveModeEnter(result);
		return new object[1] { restrictedMediaLiveResult };
	}

	private void OnRestrictedMediaLiveModeEnterCompleted(object state)
	{
		if (this.RestrictedMediaLiveModeEnterCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RestrictedMediaLiveModeEnterCompleted(this, new RestrictedMediaLiveModeEnterCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RestrictedMediaLiveModeEnterAsync(Guid[] deviceIds, DateTime timeStart)
	{
		RestrictedMediaLiveModeEnterAsync(deviceIds, timeStart, null);
	}

	public void RestrictedMediaLiveModeEnterAsync(Guid[] deviceIds, DateTime timeStart, object userState)
	{
		if (onBeginRestrictedMediaLiveModeEnterDelegate == null)
		{
			onBeginRestrictedMediaLiveModeEnterDelegate = OnBeginRestrictedMediaLiveModeEnter;
		}
		if (onEndRestrictedMediaLiveModeEnterDelegate == null)
		{
			onEndRestrictedMediaLiveModeEnterDelegate = OnEndRestrictedMediaLiveModeEnter;
		}
		if (onRestrictedMediaLiveModeEnterCompletedDelegate == null)
		{
			onRestrictedMediaLiveModeEnterCompletedDelegate = OnRestrictedMediaLiveModeEnterCompleted;
		}
		InvokeAsync(onBeginRestrictedMediaLiveModeEnterDelegate, new object[2] { deviceIds, timeStart }, onEndRestrictedMediaLiveModeEnterDelegate, onRestrictedMediaLiveModeEnterCompletedDelegate, userState);
	}

	public RestrictedMediaResult RestrictedMediaLiveModeExit(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return base.Channel.RestrictedMediaLiveModeExit(id, deviceIds, header, description, timeStart, timeEnd);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRestrictedMediaLiveModeExit(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRestrictedMediaLiveModeExit(id, deviceIds, header, description, timeStart, timeEnd, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public RestrictedMediaResult EndRestrictedMediaLiveModeExit(IAsyncResult result)
	{
		return base.Channel.EndRestrictedMediaLiveModeExit(result);
	}

	private IAsyncResult OnBeginRestrictedMediaLiveModeExit(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid id = (Guid)inValues[0];
		Guid[] deviceIds = (Guid[])inValues[1];
		string header = (string)inValues[2];
		string description = (string)inValues[3];
		DateTime timeStart = (DateTime)inValues[4];
		DateTime timeEnd = (DateTime)inValues[5];
		return BeginRestrictedMediaLiveModeExit(id, deviceIds, header, description, timeStart, timeEnd, callback, asyncState);
	}

	private object[] OnEndRestrictedMediaLiveModeExit(IAsyncResult result)
	{
		RestrictedMediaResult restrictedMediaResult = EndRestrictedMediaLiveModeExit(result);
		return new object[1] { restrictedMediaResult };
	}

	private void OnRestrictedMediaLiveModeExitCompleted(object state)
	{
		if (this.RestrictedMediaLiveModeExitCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RestrictedMediaLiveModeExitCompleted(this, new RestrictedMediaLiveModeExitCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RestrictedMediaLiveModeExitAsync(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		RestrictedMediaLiveModeExitAsync(id, deviceIds, header, description, timeStart, timeEnd, null);
	}

	public void RestrictedMediaLiveModeExitAsync(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd, object userState)
	{
		if (onBeginRestrictedMediaLiveModeExitDelegate == null)
		{
			onBeginRestrictedMediaLiveModeExitDelegate = OnBeginRestrictedMediaLiveModeExit;
		}
		if (onEndRestrictedMediaLiveModeExitDelegate == null)
		{
			onEndRestrictedMediaLiveModeExitDelegate = OnEndRestrictedMediaLiveModeExit;
		}
		if (onRestrictedMediaLiveModeExitCompletedDelegate == null)
		{
			onRestrictedMediaLiveModeExitCompletedDelegate = OnRestrictedMediaLiveModeExitCompleted;
		}
		InvokeAsync(onBeginRestrictedMediaLiveModeExitDelegate, new object[6] { id, deviceIds, header, description, timeStart, timeEnd }, onEndRestrictedMediaLiveModeExitDelegate, onRestrictedMediaLiveModeExitCompletedDelegate, userState);
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime timeStart)
	{
		return base.Channel.RestrictedMediaLiveUpdate(deviceIds, timeStart);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime timeStart, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRestrictedMediaLiveUpdate(deviceIds, timeStart, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public RestrictedMediaLiveResult EndRestrictedMediaLiveUpdate(IAsyncResult result)
	{
		return base.Channel.EndRestrictedMediaLiveUpdate(result);
	}

	private IAsyncResult OnBeginRestrictedMediaLiveUpdate(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid[] deviceIds = (Guid[])inValues[0];
		DateTime timeStart = (DateTime)inValues[1];
		return BeginRestrictedMediaLiveUpdate(deviceIds, timeStart, callback, asyncState);
	}

	private object[] OnEndRestrictedMediaLiveUpdate(IAsyncResult result)
	{
		RestrictedMediaLiveResult restrictedMediaLiveResult = EndRestrictedMediaLiveUpdate(result);
		return new object[1] { restrictedMediaLiveResult };
	}

	private void OnRestrictedMediaLiveUpdateCompleted(object state)
	{
		if (this.RestrictedMediaLiveUpdateCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RestrictedMediaLiveUpdateCompleted(this, new RestrictedMediaLiveUpdateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RestrictedMediaLiveUpdateAsync(Guid[] deviceIds, DateTime timeStart)
	{
		RestrictedMediaLiveUpdateAsync(deviceIds, timeStart, null);
	}

	public void RestrictedMediaLiveUpdateAsync(Guid[] deviceIds, DateTime timeStart, object userState)
	{
		if (onBeginRestrictedMediaLiveUpdateDelegate == null)
		{
			onBeginRestrictedMediaLiveUpdateDelegate = OnBeginRestrictedMediaLiveUpdate;
		}
		if (onEndRestrictedMediaLiveUpdateDelegate == null)
		{
			onEndRestrictedMediaLiveUpdateDelegate = OnEndRestrictedMediaLiveUpdate;
		}
		if (onRestrictedMediaLiveUpdateCompletedDelegate == null)
		{
			onRestrictedMediaLiveUpdateCompletedDelegate = OnRestrictedMediaLiveUpdateCompleted;
		}
		InvokeAsync(onBeginRestrictedMediaLiveUpdateDelegate, new object[2] { deviceIds, timeStart }, onEndRestrictedMediaLiveUpdateDelegate, onRestrictedMediaLiveUpdateCompletedDelegate, userState);
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveDelete(Guid[] deviceIds)
	{
		return base.Channel.RestrictedMediaLiveDelete(deviceIds);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRestrictedMediaLiveDelete(Guid[] deviceIds, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRestrictedMediaLiveDelete(deviceIds, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public RestrictedMediaLiveResult EndRestrictedMediaLiveDelete(IAsyncResult result)
	{
		return base.Channel.EndRestrictedMediaLiveDelete(result);
	}

	private IAsyncResult OnBeginRestrictedMediaLiveDelete(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid[] deviceIds = (Guid[])inValues[0];
		return BeginRestrictedMediaLiveDelete(deviceIds, callback, asyncState);
	}

	private object[] OnEndRestrictedMediaLiveDelete(IAsyncResult result)
	{
		RestrictedMediaLiveResult restrictedMediaLiveResult = EndRestrictedMediaLiveDelete(result);
		return new object[1] { restrictedMediaLiveResult };
	}

	private void OnRestrictedMediaLiveDeleteCompleted(object state)
	{
		if (this.RestrictedMediaLiveDeleteCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RestrictedMediaLiveDeleteCompleted(this, new RestrictedMediaLiveDeleteCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RestrictedMediaLiveDeleteAsync(Guid[] deviceIds)
	{
		RestrictedMediaLiveDeleteAsync(deviceIds, null);
	}

	public void RestrictedMediaLiveDeleteAsync(Guid[] deviceIds, object userState)
	{
		if (onBeginRestrictedMediaLiveDeleteDelegate == null)
		{
			onBeginRestrictedMediaLiveDeleteDelegate = OnBeginRestrictedMediaLiveDelete;
		}
		if (onEndRestrictedMediaLiveDeleteDelegate == null)
		{
			onEndRestrictedMediaLiveDeleteDelegate = OnEndRestrictedMediaLiveDelete;
		}
		if (onRestrictedMediaLiveDeleteCompletedDelegate == null)
		{
			onRestrictedMediaLiveDeleteCompletedDelegate = OnRestrictedMediaLiveDeleteCompleted;
		}
		InvokeAsync(onBeginRestrictedMediaLiveDeleteDelegate, new object[1] { deviceIds }, onEndRestrictedMediaLiveDeleteDelegate, onRestrictedMediaLiveDeleteCompletedDelegate, userState);
	}

	public RestrictedMediaLive[] RestrictedMediaLiveQueryAll()
	{
		return base.Channel.RestrictedMediaLiveQueryAll();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRestrictedMediaLiveQueryAll(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRestrictedMediaLiveQueryAll(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public RestrictedMediaLive[] EndRestrictedMediaLiveQueryAll(IAsyncResult result)
	{
		return base.Channel.EndRestrictedMediaLiveQueryAll(result);
	}

	private IAsyncResult OnBeginRestrictedMediaLiveQueryAll(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginRestrictedMediaLiveQueryAll(callback, asyncState);
	}

	private object[] OnEndRestrictedMediaLiveQueryAll(IAsyncResult result)
	{
		RestrictedMediaLive[] array = EndRestrictedMediaLiveQueryAll(result);
		return new object[1] { array };
	}

	private void OnRestrictedMediaLiveQueryAllCompleted(object state)
	{
		if (this.RestrictedMediaLiveQueryAllCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RestrictedMediaLiveQueryAllCompleted(this, new RestrictedMediaLiveQueryAllCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RestrictedMediaLiveQueryAllAsync()
	{
		RestrictedMediaLiveQueryAllAsync(null);
	}

	public void RestrictedMediaLiveQueryAllAsync(object userState)
	{
		if (onBeginRestrictedMediaLiveQueryAllDelegate == null)
		{
			onBeginRestrictedMediaLiveQueryAllDelegate = OnBeginRestrictedMediaLiveQueryAll;
		}
		if (onEndRestrictedMediaLiveQueryAllDelegate == null)
		{
			onEndRestrictedMediaLiveQueryAllDelegate = OnEndRestrictedMediaLiveQueryAll;
		}
		if (onRestrictedMediaLiveQueryAllCompletedDelegate == null)
		{
			onRestrictedMediaLiveQueryAllCompletedDelegate = OnRestrictedMediaLiveQueryAllCompleted;
		}
		InvokeAsync(onBeginRestrictedMediaLiveQueryAllDelegate, null, onEndRestrictedMediaLiveQueryAllDelegate, onRestrictedMediaLiveQueryAllCompletedDelegate, userState);
	}

	public ClientProfile GetClientProfile(string token, bool withSetting)
	{
		return base.Channel.GetClientProfile(token, withSetting);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetClientProfile(string token, bool withSetting, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetClientProfile(token, withSetting, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public ClientProfile EndGetClientProfile(IAsyncResult result)
	{
		return base.Channel.EndGetClientProfile(result);
	}

	private IAsyncResult OnBeginGetClientProfile(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		bool withSetting = (bool)inValues[1];
		return BeginGetClientProfile(token, withSetting, callback, asyncState);
	}

	private object[] OnEndGetClientProfile(IAsyncResult result)
	{
		ClientProfile clientProfile = EndGetClientProfile(result);
		return new object[1] { clientProfile };
	}

	private void OnGetClientProfileCompleted(object state)
	{
		if (this.GetClientProfileCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetClientProfileCompleted(this, new GetClientProfileCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetClientProfileAsync(string token, bool withSetting)
	{
		GetClientProfileAsync(token, withSetting, null);
	}

	public void GetClientProfileAsync(string token, bool withSetting, object userState)
	{
		if (onBeginGetClientProfileDelegate == null)
		{
			onBeginGetClientProfileDelegate = OnBeginGetClientProfile;
		}
		if (onEndGetClientProfileDelegate == null)
		{
			onEndGetClientProfileDelegate = OnEndGetClientProfile;
		}
		if (onGetClientProfileCompletedDelegate == null)
		{
			onGetClientProfileCompletedDelegate = OnGetClientProfileCompleted;
		}
		InvokeAsync(onBeginGetClientProfileDelegate, new object[2] { token, withSetting }, onEndGetClientProfileDelegate, onGetClientProfileCompletedDelegate, userState);
	}

	public PresetInfo[] GetPresets(string token, Guid deviceId)
	{
		return base.Channel.GetPresets(token, deviceId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetPresets(string token, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetPresets(token, deviceId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public PresetInfo[] EndGetPresets(IAsyncResult result)
	{
		return base.Channel.EndGetPresets(result);
	}

	private IAsyncResult OnBeginGetPresets(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid deviceId = (Guid)inValues[1];
		return BeginGetPresets(token, deviceId, callback, asyncState);
	}

	private object[] OnEndGetPresets(IAsyncResult result)
	{
		PresetInfo[] array = EndGetPresets(result);
		return new object[1] { array };
	}

	private void OnGetPresetsCompleted(object state)
	{
		if (this.GetPresetsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetPresetsCompleted(this, new GetPresetsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetPresetsAsync(string token, Guid deviceId)
	{
		GetPresetsAsync(token, deviceId, null);
	}

	public void GetPresetsAsync(string token, Guid deviceId, object userState)
	{
		if (onBeginGetPresetsDelegate == null)
		{
			onBeginGetPresetsDelegate = OnBeginGetPresets;
		}
		if (onEndGetPresetsDelegate == null)
		{
			onEndGetPresetsDelegate = OnEndGetPresets;
		}
		if (onGetPresetsCompletedDelegate == null)
		{
			onGetPresetsCompletedDelegate = OnGetPresetsCompleted;
		}
		InvokeAsync(onBeginGetPresetsDelegate, new object[2] { token, deviceId }, onEndGetPresetsDelegate, onGetPresetsCompletedDelegate, userState);
	}

	public void DeletePreset(string token, Guid id, Guid deviceId)
	{
		base.Channel.DeletePreset(token, id, deviceId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDeletePreset(string token, Guid id, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDeletePreset(token, id, deviceId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDeletePreset(IAsyncResult result)
	{
		base.Channel.EndDeletePreset(result);
	}

	private IAsyncResult OnBeginDeletePreset(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid id = (Guid)inValues[1];
		Guid deviceId = (Guid)inValues[2];
		return BeginDeletePreset(token, id, deviceId, callback, asyncState);
	}

	private object[] OnEndDeletePreset(IAsyncResult result)
	{
		EndDeletePreset(result);
		return null;
	}

	private void OnDeletePresetCompleted(object state)
	{
		if (this.DeletePresetCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DeletePresetCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DeletePresetAsync(string token, Guid id, Guid deviceId)
	{
		DeletePresetAsync(token, id, deviceId, null);
	}

	public void DeletePresetAsync(string token, Guid id, Guid deviceId, object userState)
	{
		if (onBeginDeletePresetDelegate == null)
		{
			onBeginDeletePresetDelegate = OnBeginDeletePreset;
		}
		if (onEndDeletePresetDelegate == null)
		{
			onEndDeletePresetDelegate = OnEndDeletePreset;
		}
		if (onDeletePresetCompletedDelegate == null)
		{
			onDeletePresetCompletedDelegate = OnDeletePresetCompleted;
		}
		InvokeAsync(onBeginDeletePresetDelegate, new object[3] { token, id, deviceId }, onEndDeletePresetDelegate, onDeletePresetCompletedDelegate, userState);
	}

	public void UpdatePresetName(string token, Guid id, Guid deviceId, string newName)
	{
		base.Channel.UpdatePresetName(token, id, deviceId, newName);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginUpdatePresetName(string token, Guid id, Guid deviceId, string newName, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginUpdatePresetName(token, id, deviceId, newName, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndUpdatePresetName(IAsyncResult result)
	{
		base.Channel.EndUpdatePresetName(result);
	}

	private IAsyncResult OnBeginUpdatePresetName(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid id = (Guid)inValues[1];
		Guid deviceId = (Guid)inValues[2];
		string newName = (string)inValues[3];
		return BeginUpdatePresetName(token, id, deviceId, newName, callback, asyncState);
	}

	private object[] OnEndUpdatePresetName(IAsyncResult result)
	{
		EndUpdatePresetName(result);
		return null;
	}

	private void OnUpdatePresetNameCompleted(object state)
	{
		if (this.UpdatePresetNameCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.UpdatePresetNameCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void UpdatePresetNameAsync(string token, Guid id, Guid deviceId, string newName)
	{
		UpdatePresetNameAsync(token, id, deviceId, newName, null);
	}

	public void UpdatePresetNameAsync(string token, Guid id, Guid deviceId, string newName, object userState)
	{
		if (onBeginUpdatePresetNameDelegate == null)
		{
			onBeginUpdatePresetNameDelegate = OnBeginUpdatePresetName;
		}
		if (onEndUpdatePresetNameDelegate == null)
		{
			onEndUpdatePresetNameDelegate = OnEndUpdatePresetName;
		}
		if (onUpdatePresetNameCompletedDelegate == null)
		{
			onUpdatePresetNameCompletedDelegate = OnUpdatePresetNameCompleted;
		}
		InvokeAsync(onBeginUpdatePresetNameDelegate, new object[4] { token, id, deviceId, newName }, onEndUpdatePresetNameDelegate, onUpdatePresetNameCompletedDelegate, userState);
	}

	public void UpdatePresetPosition(string token, Guid id, Guid deviceId)
	{
		base.Channel.UpdatePresetPosition(token, id, deviceId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginUpdatePresetPosition(string token, Guid id, Guid deviceId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginUpdatePresetPosition(token, id, deviceId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndUpdatePresetPosition(IAsyncResult result)
	{
		base.Channel.EndUpdatePresetPosition(result);
	}

	private IAsyncResult OnBeginUpdatePresetPosition(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid id = (Guid)inValues[1];
		Guid deviceId = (Guid)inValues[2];
		return BeginUpdatePresetPosition(token, id, deviceId, callback, asyncState);
	}

	private object[] OnEndUpdatePresetPosition(IAsyncResult result)
	{
		EndUpdatePresetPosition(result);
		return null;
	}

	private void OnUpdatePresetPositionCompleted(object state)
	{
		if (this.UpdatePresetPositionCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.UpdatePresetPositionCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void UpdatePresetPositionAsync(string token, Guid id, Guid deviceId)
	{
		UpdatePresetPositionAsync(token, id, deviceId, null);
	}

	public void UpdatePresetPositionAsync(string token, Guid id, Guid deviceId, object userState)
	{
		if (onBeginUpdatePresetPositionDelegate == null)
		{
			onBeginUpdatePresetPositionDelegate = OnBeginUpdatePresetPosition;
		}
		if (onEndUpdatePresetPositionDelegate == null)
		{
			onEndUpdatePresetPositionDelegate = OnEndUpdatePresetPosition;
		}
		if (onUpdatePresetPositionCompletedDelegate == null)
		{
			onUpdatePresetPositionCompletedDelegate = OnUpdatePresetPositionCompleted;
		}
		InvokeAsync(onBeginUpdatePresetPositionDelegate, new object[3] { token, id, deviceId }, onEndUpdatePresetPositionDelegate, onUpdatePresetPositionCompletedDelegate, userState);
	}

	public Guid CreatePreset(string token, Guid deviceId, string name)
	{
		return base.Channel.CreatePreset(token, deviceId, name);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginCreatePreset(string token, Guid deviceId, string name, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginCreatePreset(token, deviceId, name, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Guid EndCreatePreset(IAsyncResult result)
	{
		return base.Channel.EndCreatePreset(result);
	}

	private IAsyncResult OnBeginCreatePreset(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid deviceId = (Guid)inValues[1];
		string name = (string)inValues[2];
		return BeginCreatePreset(token, deviceId, name, callback, asyncState);
	}

	private object[] OnEndCreatePreset(IAsyncResult result)
	{
		Guid guid = EndCreatePreset(result);
		return new object[1] { guid };
	}

	private void OnCreatePresetCompleted(object state)
	{
		if (this.CreatePresetCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.CreatePresetCompleted(this, new CreatePresetCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void CreatePresetAsync(string token, Guid deviceId, string name)
	{
		CreatePresetAsync(token, deviceId, name, null);
	}

	public void CreatePresetAsync(string token, Guid deviceId, string name, object userState)
	{
		if (onBeginCreatePresetDelegate == null)
		{
			onBeginCreatePresetDelegate = OnBeginCreatePreset;
		}
		if (onEndCreatePresetDelegate == null)
		{
			onEndCreatePresetDelegate = OnEndCreatePreset;
		}
		if (onCreatePresetCompletedDelegate == null)
		{
			onCreatePresetCompletedDelegate = OnCreatePresetCompleted;
		}
		InvokeAsync(onBeginCreatePresetDelegate, new object[3] { token, deviceId, name }, onEndCreatePresetDelegate, onCreatePresetCompletedDelegate, userState);
	}

	public void OrderPresets(string token, Guid deviceId, Guid[] ids)
	{
		base.Channel.OrderPresets(token, deviceId, ids);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginOrderPresets(string token, Guid deviceId, Guid[] ids, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginOrderPresets(token, deviceId, ids, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndOrderPresets(IAsyncResult result)
	{
		base.Channel.EndOrderPresets(result);
	}

	private IAsyncResult OnBeginOrderPresets(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string token = (string)inValues[0];
		Guid deviceId = (Guid)inValues[1];
		Guid[] ids = (Guid[])inValues[2];
		return BeginOrderPresets(token, deviceId, ids, callback, asyncState);
	}

	private object[] OnEndOrderPresets(IAsyncResult result)
	{
		EndOrderPresets(result);
		return null;
	}

	private void OnOrderPresetsCompleted(object state)
	{
		if (this.OrderPresetsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.OrderPresetsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void OrderPresetsAsync(string token, Guid deviceId, Guid[] ids)
	{
		OrderPresetsAsync(token, deviceId, ids, null);
	}

	public void OrderPresetsAsync(string token, Guid deviceId, Guid[] ids, object userState)
	{
		if (onBeginOrderPresetsDelegate == null)
		{
			onBeginOrderPresetsDelegate = OnBeginOrderPresets;
		}
		if (onEndOrderPresetsDelegate == null)
		{
			onEndOrderPresetsDelegate = OnEndOrderPresets;
		}
		if (onOrderPresetsCompletedDelegate == null)
		{
			onOrderPresetsCompletedDelegate = OnOrderPresetsCompleted;
		}
		InvokeAsync(onBeginOrderPresetsDelegate, new object[3] { token, deviceId, ids }, onEndOrderPresetsDelegate, onOrderPresetsCompletedDelegate, userState);
	}
}
