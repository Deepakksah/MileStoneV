using System;
using System.Collections.Generic;
using System.Threading;
using VideoOS.Mobile.Management;
using VideoOS.Mobile.Oem;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.TrayIconComm;
using VideoOS.Platform;
using VideoOS.Platform.SDK;

namespace VideoOS.Mobile.LoginThread;

public class LoginThread
{
	private Thread _thread;

	private bool _stop;

	private ILoginData _loginData;

	private ILoginConfiguration _loginConfiguration;

	private ILoginConfigSpecific _loginConfigSpecific;

	private IMipConfig _mipConfigSpecific;

	private MipSdkWrapper _mipSdkWrapper;

	private MipSdkRegister _mipSdkRegister;

	private DateTime _lastLoginUtc;

	public readonly StateChanger<TrayComm.MobileServerState> StateChanger = new StateChanger<TrayComm.MobileServerState>(TrayComm.MobileServerState.Initializing);

	public static StateChanger<TrayComm.MobileServerState> StateChangerInstance;

	public bool ServiceInitializedCorrectly { get; private set; }

	public LoginThread()
	{
		StateChangerInstance = StateChanger;
	}

	public virtual void Init(ILoginData loginData, ILoginConfiguration loginConfiguration, ILoginConfigSpecific loginConfigSpecific, string name = "LoginThread.Run", IMipConfig mipSdkSpecificConfig = null)
	{
		_loginData = loginData;
		_loginConfiguration = loginConfiguration;
		_loginConfigSpecific = loginConfigSpecific;
		_mipConfigSpecific = mipSdkSpecificConfig;
		_thread = new Thread(Run);
		_thread.Name = name;
		_thread.Start();
	}

	public virtual ItemStatus UpdateConfiguraton(Item configuration)
	{
		return ItemStatus.Unchanged;
	}

	public virtual void Close()
	{
		_stop = true;
		_thread.Join();
	}

	private void Run()
	{
		Log.TraceBegin("Run", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\LoginThread.cs", 69);
		try
		{
			Log.Trace("LoginThread.Run", "Entered Run method");
			RunInitialize();
			Log.Trace("LoginThread.Run", "Initialized");
			bool flag = false;
			_lastLoginUtc = DateTime.MinValue;
			StateChanger.ChangeState(TrayComm.MobileServerState.Initializing);
			_mipSdkWrapper = new MipSdkWrapper(_loginData, _loginConfiguration, _loginConfigSpecific, _mipConfigSpecific);
			_mipSdkRegister = new MipSdkRegister(_loginData, _loginConfiguration, _loginConfigSpecific);
			_mipSdkWrapper.InitializeMipSdk();
			while (!_stop && !_mipSdkWrapper.InitializeMipMultiUser())
			{
				_mipSdkWrapper.UnInitializeMipMultiUser();
				StateChanger.ChangeState(TrayComm.MobileServerState.FailedToConnect);
				Thread.Sleep(1000);
			}
			RunOnMipSdkInitialized(_mipSdkWrapper);
			Log.Trace("LoginThread.Run", "MIP SDK Initialized");
			VmsConfigChangeChecker.Instance.VmsLicenseChanged.Add(this, OnVmsLicenseChanged);
			VmsConfigChangeChecker.Instance.MsConfigurationChanged.Add(this, OnMsConfigurationChanged);
			VmsConfigChangeChecker.Instance.VmsConfigurationChanged.Add(this, OnVmsConfigurationChanged);
			while (!_stop)
			{
				TimeSpan timeSpan = DateTime.UtcNow - _lastLoginUtc;
				if ((!flag && timeSpan >= _loginConfiguration.ManagementServerFailLoginTimeout) || timeSpan >= _loginConfiguration.ManagementServerSuccessLoginTimeout)
				{
					if (StateChanger.State != TrayComm.MobileServerState.Connected)
					{
						StateChanger.ChangeState(TrayComm.MobileServerState.Initializing);
					}
					flag = MipSdkWrapperServer.Instance.Login(_loginData, out var _);
					if (flag)
					{
						SurveillanceServer.Initialize(_mipSdkWrapper.GetSiteItem());
						SurveillanceServer.Initialize(_loginData.ServerFqid);
						VmsConfigChangeChecker.Instance.Initialize();
						InitializeUsers();
						flag = IsServerAllowed(SurveillanceServer.Instance, LicenseWrapper.Instance);
						if (!flag)
						{
							StateChanger.ChangeState(TrayComm.MobileServerState.FailedNoGoForOem);
						}
					}
					else
					{
						StateChanger.ChangeState(TrayComm.MobileServerState.FailedToConnect);
					}
					if (flag)
					{
						TrayComm.MobileServerState mobileServerState = TrayComm.MobileServerState.Initializing;
						if (!_mipSdkRegister.RegisterMobileService())
						{
							mobileServerState = TrayComm.MobileServerState.InitializedWithWarningsRegister;
						}
						Item currentServerConfiguration = _mipSdkWrapper.GetCurrentServerConfiguration();
						if (currentServerConfiguration != null && UpdateConfiguraton(currentServerConfiguration) == ItemStatus.Changed)
						{
							_mipSdkWrapper.SaveUpdatedItemConfiguration(currentServerConfiguration, updateConfigurationId: false);
						}
						CoBrandingManager.Instance.InitFromEnvironmentSdk();
						if (!RunOnSuccessfullLogin(currentServerConfiguration?.Properties))
						{
							mobileServerState = TrayComm.MobileServerState.InitializedWithWarningsCommunication;
						}
						StateChanger.ChangeState((mobileServerState == TrayComm.MobileServerState.Initializing) ? TrayComm.MobileServerState.Connected : mobileServerState);
					}
					ConfigurationHasBeenRefreshed();
				}
				RunOnCycle();
				Thread.Sleep(1000);
			}
			Log.Trace("LoginThread.Run", "Stop flag raised");
			VmsConfigChangeChecker.Instance.VmsLicenseChanged.RemoveItem(this);
			VmsConfigChangeChecker.Instance.Denitialize();
			RunDenitialize();
			Log.Trace("LoginThread.Run", "ConfigurationChangeChecker deintialized");
			MipSdkWrapperServer.Instance.Logout();
			_mipSdkWrapper.UnInitializeMipMultiUser();
			Log.Trace("LoginThread.Run", "Login thread run finished.");
		}
		catch (Exception ex)
		{
			StateChanger.ChangeState(TrayComm.MobileServerState.FailedToConnect);
			Log.Error("LoginThread.Run()", ex.Message);
			Log.Exception(ex);
		}
		Log.TraceEnd("Run", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\LoginThread.cs", 203);
	}

	private void OnMsConfigurationChanged(object sender, EventArgs e)
	{
		Log.TraceBegin("OnMsConfigurationChanged", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\LoginThread.cs", 208);
		ConfigurationNeedsToBeRefreshed();
		Log.TraceEnd("OnMsConfigurationChanged", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\LoginThread.cs", 212);
	}

	private void OnVmsConfigurationChanged(object sender, EventArgs e)
	{
		Log.TraceBegin("OnVmsConfigurationChanged", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\LoginThread.cs", 216);
		MultiUserEnvironment.ReloadConfiguration(EnvironmentManager.Instance.CurrentSite.ServerId.UserContext, EnvironmentManager.Instance.CurrentSite);
		Log.TraceEnd("OnVmsConfigurationChanged", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\LoginThread.cs", 220);
	}

	private void OnVmsLicenseChanged(object sender, EventArgs eventArgs)
	{
		ConfigurationNeedsToBeRefreshed();
	}

	protected void ConfigurationHasBeenRefreshed()
	{
		Log.TraceBegin("ConfigurationHasBeenRefreshed", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\LoginThread.cs", 230);
		_lastLoginUtc = DateTime.UtcNow;
		Log.TraceEnd("ConfigurationHasBeenRefreshed", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\LoginThread.cs", 234);
	}

	protected void ConfigurationNeedsToBeRefreshed()
	{
		_lastLoginUtc = DateTime.MinValue;
	}

	protected virtual void InitializeUsers()
	{
	}

	protected virtual void RunInitialize()
	{
	}

	protected virtual void RunDenitialize()
	{
	}

	protected virtual void RunOnMipSdkInitialized(MipSdkWrapper mipSdkWrapper = null)
	{
	}

	protected virtual bool IsServerAllowed(SurveillanceServer server, LicenseWrapper licenseWrapper)
	{
		return true;
	}

	protected virtual bool RunOnSuccessfullLogin(Dictionary<string, string> configuration)
	{
		return true;
	}

	protected virtual void RunOnCycle()
	{
		VmsConfigChangeChecker.Instance.Process();
	}
}
