using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class UserIdentityParams
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		UserName = 1u,
		Email = 2u,
		Authority = 4u,
		All = 7u
	}

	private string _userName;

	private string _email;

	private string _authority;

	private uint _unMaskToCheck;

	public string UserName => _userName;

	public string Email => _email;

	public string Authority => _authority;

	public UserIdentityParams()
	{
		InitDefaultValues();
	}

	private void InitDefaultValues()
	{
		_userName = null;
		_email = null;
		_authority = null;
	}

	public void InitFromCommand(Dictionary<string, string> serverParams)
	{
		ParamsHelper.ConvertParameter(serverParams, "Username", ref _userName, 1u, ParamsHelper.StringConvertion, 7u, ref _unMaskToCheck, bLog: false);
		ParamsHelper.ConvertParameter(serverParams, "Email", ref _email, 2u, ParamsHelper.StringConvertion, 7u, ref _unMaskToCheck, bLog: false);
		ParamsHelper.ConvertParameter(serverParams, "Authority", ref _authority, 4u, ParamsHelper.StringConvertion, 7u, ref _unMaskToCheck, bLog: false);
	}

	public ErrorCodes CheckUserIdentityParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, _unMaskToCheck);
	}
}
