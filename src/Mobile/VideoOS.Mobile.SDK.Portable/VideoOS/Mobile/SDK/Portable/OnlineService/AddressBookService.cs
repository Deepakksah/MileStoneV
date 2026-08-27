using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.OnlineService;

public class AddressBookService : IControl, IAddressBookService, IAddressBookServiceSync, IAddressBookServiceAsync, IErrorResult
{
	public static TimeSpan AbsDefaultTimeout = TimeSpan.FromMinutes(2.0);

	public static IAddressBookService Instance = new AddressBookService();

	private Connection _connection;

	private bool _enabled = true;

	private string _externalIp = string.Empty;

	private bool _featureAllowed;

	private bool _isInitialized;

	private readonly Dictionary<string, string> _headers = new Dictionary<string, string>
	{
		["MobileServer-Version"] = "25.3.0.0",
		["MobileServer-ProductCode"] = "1"
	};

	public string ExternalIp => _externalIp;

	public bool Enabled
	{
		set
		{
			_enabled = value;
		}
	}

	public Command.CommandResultTypes Result => _connection.Result;

	public string ErrorMessage => _connection.ErrorMessage;

	public ErrorCodes ErrorCode => _connection.ErrorCode;

	private AddressBookService()
	{
		_featureAllowed = false;
	}

	public ServerConfigurationParams UpdateServerCapabilities(ServerConfigurationParams serverConfigurationParams, TimeSpan timeout)
	{
		ServerConfigurationParams result = null;
		if (_featureAllowed)
		{
			AddressBookServiceResponse addressBookServiceResponse = null;
			IAsyncResult asyncResult = null;
			try
			{
				asyncResult = _connection.AddressBookService.SetServerEndpoints(serverConfigurationParams, ProcessResult);
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "AddressBookService.UpdateServerCapabilities", ex.Message);
			}
			if (asyncResult != null)
			{
				_connection.ProcessResult(asyncResult, timeout);
				addressBookServiceResponse = new AddressBookServiceResponse(asyncResult.AsyncState as BaseCommandResponse);
			}
			if (addressBookServiceResponse != null)
			{
				result = addressBookServiceResponse.ServerConfigurationParams;
			}
		}
		return result;
	}

	public void Initialize(UriBuilder uri, bool isAllowed)
	{
		_featureAllowed = isAllowed;
		if (!_isInitialized && isAllowed)
		{
			_isInitialized = true;
			_connection = new Connection((!(uri.Scheme == "http")) ? ChannelTypes.HTTPSecure : ChannelTypes.HTTP, uri.Host, (uint)uri.Port, uri.Path, plainAuthentication: false, requireCommandEncryption: false, null, _headers)
			{
				RunHeartBeat = false
			};
		}
	}

	public IAsyncResult CheckServerAccessibility(ServerConfigurationParams serverConfigurationParams, OnResponse<AddressBookServiceResponse> onSuccess = null, OnResponse<AddressBookServiceResponse> onFail = null)
	{
		IAsyncResult result = null;
		if (IsAllowed())
		{
			try
			{
				result = _connection.AddressBookService.CheckServerAccessibility(serverConfigurationParams, (OnResponse<AddressBookServiceResponse>)Delegate.Combine(new OnResponse<AddressBookServiceResponse>(ProcessResult), onSuccess), onFail);
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "AddressBookService.CheckServerAccessibility", ex.Message);
			}
		}
		return result;
	}

	public IAsyncResult SetServerEndpoints(ServerConfigurationParams serverConfigurationParams, OnResponse<AddressBookServiceResponse> onSuccess = null, OnResponse<AddressBookServiceResponse> onFail = null)
	{
		IAsyncResult result = null;
		if (IsAllowed())
		{
			try
			{
				result = _connection.AddressBookService.SetServerEndpoints(serverConfigurationParams, (OnResponse<AddressBookServiceResponse>)Delegate.Combine(new OnResponse<AddressBookServiceResponse>(ProcessResult), onSuccess), onFail);
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "AddressBookService.SetServerEndpoints", ex.Message);
			}
		}
		return result;
	}

	public IAsyncResult GetServerEndpoints(ServerConfigurationParams serverConfigurationParams, OnResponse<AddressBookServiceResponse> onSuccess = null, OnResponse<AddressBookServiceResponse> onFail = null)
	{
		IAsyncResult result = null;
		if (IsAllowed())
		{
			try
			{
				result = _connection.AddressBookService.GetServerEndpoints(serverConfigurationParams, (OnResponse<AddressBookServiceResponse>)Delegate.Combine(new OnResponse<AddressBookServiceResponse>(ProcessResult), onSuccess), onFail);
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "AddressBookService.GetServerEndpoints", ex.Message);
			}
		}
		return result;
	}

	public IAsyncResult SendInvitation(InvitationParams invitationParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		IAsyncResult result = null;
		if (IsAllowed())
		{
			try
			{
				result = _connection.AddressBookService.SendInvitation(invitationParams, onSuccess, onFail);
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "AddressBookService.SendInvitation", ex.Message);
			}
		}
		return result;
	}

	public IAsyncResult GetInvitationLinks(OnResponse<GetInvitationLinksResponse> onSuccess = null, OnResponse<GetInvitationLinksResponse> onFail = null)
	{
		IAsyncResult result = null;
		if (IsAllowed())
		{
			try
			{
				result = _connection.AddressBookService.GetInvitationLinks(onSuccess, onFail);
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "AddressBookService.GetInvitationLinks", ex.Message);
			}
		}
		return result;
	}

	public void Start()
	{
		_connection?.Start();
	}

	public void Stop()
	{
		if (_connection != null)
		{
			_connection.Stop();
			_connection.DisposeQuiet();
		}
	}

	public AddressBookServiceResponse SetServerEndpoints(ServerConfigurationParams serverConfigurationParams, TimeSpan timeout)
	{
		AddressBookServiceResponse result = null;
		IAsyncResult asyncResult = SetServerEndpoints(serverConfigurationParams);
		if (asyncResult != null)
		{
			result = new AddressBookServiceResponse(_connection.ProcessResult(asyncResult, timeout));
		}
		return result;
	}

	public AddressBookServiceResponse GetServerEndpoints(ServerConfigurationParams serverConfigurationParams, TimeSpan timeout)
	{
		AddressBookServiceResponse result = null;
		IAsyncResult serverEndpoints = GetServerEndpoints(serverConfigurationParams);
		if (serverEndpoints != null)
		{
			result = new AddressBookServiceResponse(_connection.ProcessResult(serverEndpoints, timeout));
		}
		return result;
	}

	public AddressBookServiceResponse CheckServerAccessibility(ServerConfigurationParams serverConfigurationParams, TimeSpan timeout)
	{
		AddressBookServiceResponse result = null;
		IAsyncResult asyncResult = CheckServerAccessibility(serverConfigurationParams);
		if (asyncResult != null)
		{
			result = new AddressBookServiceResponse(_connection.ProcessResult(asyncResult, timeout));
		}
		return result;
	}

	public BaseCommandResponse SendInvitation(InvitationParams invitationParams, TimeSpan timeout)
	{
		BaseCommandResponse result = null;
		IAsyncResult asyncResult = SendInvitation(invitationParams);
		if (asyncResult != null)
		{
			result = _connection.ProcessResult(asyncResult, timeout);
		}
		return result;
	}

	public GetInvitationLinksResponse GetInvitationLinks(TimeSpan timeout)
	{
		GetInvitationLinksResponse result = null;
		IAsyncResult invitationLinks = GetInvitationLinks();
		if (invitationLinks != null)
		{
			result = new GetInvitationLinksResponse(_connection.ProcessResult(invitationLinks, timeout));
		}
		return result;
	}

	private bool IsAllowed()
	{
		if (_enabled)
		{
			return _featureAllowed;
		}
		return false;
	}

	private void ProcessResult(AddressBookServiceResponse result)
	{
		if (result != null && result.ServerConfigurationParams != null && result.ServerConfigurationParams.CheckForMask(ServerConfigurationParamsHelper.ParamMask.LocalIp))
		{
			_externalIp = result.ServerConfigurationParams.LocalIp;
		}
	}
}
