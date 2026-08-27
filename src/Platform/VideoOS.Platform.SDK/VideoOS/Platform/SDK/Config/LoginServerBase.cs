#define TRACE
using System;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading;
using System.Xml;
using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.SDK.Util;

namespace VideoOS.Platform.SDK.Config;

public abstract class LoginServerBase
{
	public ISystemInfo _systemInfo;

	private ILoginInfo _loginInfo;

	private readonly Guid _instanceId;

	private Timer _tokenExpireTimer;

	private bool _loggedIn;

	public volatile bool LoginInProgress;

	public virtual ILoginInfo LoginInfo
	{
		get
		{
			return _loginInfo;
		}
		set
		{
			_loginInfo = value;
		}
	}

	public Guid InstanceGuid => _instanceId;

	public virtual ServerTypes ServerType { get; set; }

	public int ServerVersion { get; set; }

	public virtual UserContext UserContext { get; set; }

	public bool UsingLogin { get; set; }

	public bool AutoReconnect { get; set; }

	public virtual Exception LastException => null;

	protected LoginServerBase()
	{
		_loginInfo = new LoginInfo
		{
			Token = ""
		};
		_instanceId = Guid.NewGuid();
	}

	public virtual bool Login(Uri uri)
	{
		if (_loggedIn)
		{
			EnvironmentManager.Instance.Log(error: false, "LoginServerBase", "Login called while already logged-in ?");
		}
		_loggedIn = false;
		try
		{
			CreateTokenTimer();
			LoginInfo = LoginService(_instanceId, "");
			GetGuidAndName();
		}
		catch (WebException ex)
		{
			if (ex.Response is HttpWebResponse { StatusCode: HttpStatusCode.Unauthorized })
			{
				throw new InvalidCredentialsMIPException(uri.DnsSafeHost);
			}
			throw new ServerNotFoundMIPException(uri.DnsSafeHost);
		}
		catch (MessageSecurityException ex2)
		{
			throw new InvalidCredentialsMIPException(GetExceptionMessageWithHostName(ex2, uri), ex2);
		}
		catch (SecurityNegotiationException)
		{
			throw new InvalidCredentialsMIPException(uri.DnsSafeHost);
		}
		catch (FaultException ex4)
		{
			switch (ex4.Code.Name)
			{
			case "50000":
			case "51000":
				throw new InvalidCredentialsMIPException(uri.DnsSafeHost);
			case "51100":
				throw new NotDualAuthorizedMIPException(ex4.Message);
			default:
				throw new MIPException(ex4.Message, ex4);
			}
		}
		catch (CommunicationException ex5)
		{
			if (ex5.InnerException is IdpException idpException)
			{
				throw IdpExceptionsHelper.ConvertToMIPException(GetExceptionMessageWithHostName(ex5, uri), idpException);
			}
			if ((ex5.InnerException as WebException)?.Response is HttpWebResponse { StatusCode: HttpStatusCode.Unauthorized })
			{
				throw new InvalidCredentialsMIPException(GetExceptionMessageWithHostName(ex5, uri), ex5);
			}
			throw new MIPException(GetExceptionMessageWithHostName(ex5, uri), ex5);
		}
		NewTokenAquired();
		UsingLogin = _loggedIn;
		return _loggedIn;
	}

	private static string GetExceptionMessageWithHostName(Exception ex, Uri uri)
	{
		return $"{ex.Message}{System.Environment.NewLine}Host name: {uri.DnsSafeHost}";
	}

	public virtual bool IsLoggedIn()
	{
		return _loggedIn;
	}

	public virtual void Logout()
	{
		if (LoginInfo != null)
		{
			LogoutService(_instanceId, LoginInfo.Token);
		}
		ServerTypeCache.ServerInfo serverInfo = ServerTypeCache.TryGet(_systemInfo.Uri);
		if (serverInfo != null && serverInfo.IsOAuthServer)
		{
			EnvironmentManager.Instance.Log(error: false, "LoginServerBase.Logout", $"Setting IdentityToken = null for instance={_systemInfo.LoginSettings.InstanceGuid}");
			_systemInfo.LoginSettings.IdentityTokenCache?.Dispose();
			_systemInfo.LoginSettings.IdentityTokenCache = null;
		}
		DisposeTokenTimer();
		_loggedIn = false;
	}

	public void ServerCommandServiceLoginCompleted(object sender, ILoginCompletedEventArgs e)
	{
		try
		{
			ValidateAsyncResponse(e);
			LoginInfo = e.Result;
			NewTokenAquired();
			_loggedIn = true;
		}
		catch (Exception ex)
		{
			LoginInfo.Token = "";
			_loggedIn = false;
			Trace.WriteLine(ex.GetType()?.ToString() + ": " + ex.Message + "\n" + ex.StackTrace);
			NewTokenNotAquired();
		}
	}

	private static void ValidateAsyncResponse(IAsyncCompletedEventArgs asyncCompletedEventArgs)
	{
		if (asyncCompletedEventArgs.Error != null)
		{
			throw asyncCompletedEventArgs.Error;
		}
	}

	public virtual ILoginInfo LoginService(Guid instanceGuid, string currentToken)
	{
		return null;
	}

	public virtual void LogoutService(Guid instanceGuid, string currentToken)
	{
	}

	public virtual bool IsServerAvailable(Uri uri)
	{
		return false;
	}

	public virtual void GetGuidAndName()
	{
	}

	public virtual void TokenExpireTimer_Callback(object state)
	{
	}

	public void CreateTokenTimer()
	{
		if (_tokenExpireTimer == null)
		{
			_tokenExpireTimer = new Timer(TokenExpireTimer_Callback);
			_tokenExpireTimer.Change(-1, -1);
		}
	}

	public void DisposeTokenTimer()
	{
		if (_tokenExpireTimer != null)
		{
			_tokenExpireTimer.Change(-1, -1);
			_tokenExpireTimer.Dispose();
			_tokenExpireTimer = null;
		}
	}

	public void NewTokenAquired()
	{
		if (LoginInfo == null)
		{
			return;
		}
		long microSeconds = LoginInfo.TimeToLive.MicroSeconds;
		long val = microSeconds / 1000 - 300000;
		long val2 = (long)((double)(microSeconds / 1000) * 0.6);
		long dueTime = Math.Max(val, val2);
		if (_tokenExpireTimer != null)
		{
			_tokenExpireTimer.Change(dueTime, SystemInfo.TokenRetryInterval);
		}
		LoginInProgress = false;
		_loggedIn = true;
		if (_systemInfo != null)
		{
			if (_loginInfo.RegistrationTime.Kind == DateTimeKind.Local)
			{
				_systemInfo.LoginSettings.TokenTimeToLive = new DateTime(_loginInfo.RegistrationTime.Ticks + _loginInfo.TimeToLive.MicroSeconds * 10, _loginInfo.RegistrationTime.Kind).ToUniversalTime();
			}
			else
			{
				_systemInfo.LoginSettings.TokenTimeToLive = new DateTime(_loginInfo.RegistrationTime.Ticks + _loginInfo.TimeToLive.MicroSeconds * 10, _loginInfo.RegistrationTime.Kind);
			}
			_systemInfo.LoginSettings.Token = LoginInfo.Token;
			if (_systemInfo.LoginSettings.IdentityTokenCache == null)
			{
				EnvironmentManager.Instance.Log(error: false, "LoginServerbase", $"New token posted - Server: {_systemInfo.Name}, IdentityTokenCache is null");
			}
		}
		EnvironmentManager.Instance.Log(error: false, "LoginServerbase", $"New token posted - Server: {_systemInfo.Name}");
		EnvironmentManager.Instance.PostMessage(new Message(MessageId.System.LoginTokenChangedIndication, _systemInfo.LoginSettings));
	}

	public void NewTokenNotAquired()
	{
		LoginInProgress = false;
		_loggedIn = false;
		if (_systemInfo != null)
		{
			if (_tokenExpireTimer != null)
			{
				_tokenExpireTimer.Change(SystemInfo.TokenRetryInterval, SystemInfo.TokenRetryInterval);
			}
			else
			{
				EnvironmentManager.Instance.Log(error: false, "LoginCompleted null Timer", "", null);
			}
			if (DateTime.UtcNow.Ticks > _systemInfo.LoginSettings.TokenTimeToLive.Ticks && _systemInfo.LoginSettings.Token != "")
			{
				_systemInfo.LoginSettings.Token = "";
				EnvironmentManager.Instance.Log(error: false, "LoginServerBase", $"{DateTime.Now.ToShortTimeString()}: New token NOT Received -- Server: {_systemInfo.Name}");
				EnvironmentManager.Instance.PostMessage(new Message(MessageId.System.LoginTokenChangedIndication, _systemInfo.LoginSettings));
			}
		}
	}

	public void RequestNewToken()
	{
		if (_tokenExpireTimer != null)
		{
			_tokenExpireTimer.Change(1000L, SystemInfo.TokenRetryInterval);
		}
		else
		{
			EnvironmentManager.Instance.Log(error: false, "LoginCompleted null Timer", "", null);
		}
	}

	public virtual SequenceCollection GetSequences(Guid deviceId, DateTime time, TimeSpan minutesBefore, int maxCountBefore, TimeSpan minutesAfter, int maxCountAfter, Guid seqType)
	{
		throw new NotImplementedException("Get Sequences via LoginServer...");
	}

	public virtual void PTZMove(Guid deviceGuid, string direction)
	{
	}

	public virtual void SetCustomSettingGlobal(Guid id, XmlNode xml, int version, bool global)
	{
		throw new NotImplementedException();
	}

	public virtual void RegisterIntegration(string instanceId, Guid integrationId, string integrationName, string version, string manufacturerName)
	{
	}
}
