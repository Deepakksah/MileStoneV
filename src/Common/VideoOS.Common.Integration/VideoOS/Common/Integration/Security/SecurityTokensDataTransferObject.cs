using System;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Security;

[Serializable]
public class SecurityTokensDataTransferObject
{
	private Version _version;

	private SecurityToken[] _securityTokens;

	private long _lastUpdate;

	private long _nextUpdate;

	public SecurityToken[] SecurityTokens => _securityTokens;

	public Version Version => _version;

	public long LastUpdate
	{
		[DebuggerStepThrough]
		get
		{
			return _lastUpdate;
		}
		[DebuggerStepThrough]
		set
		{
			_lastUpdate = value;
		}
	}

	public long NextUpdate
	{
		[DebuggerStepThrough]
		get
		{
			return _nextUpdate;
		}
		[DebuggerStepThrough]
		set
		{
			_nextUpdate = value;
		}
	}

	public SecurityTokensDataTransferObject(SecurityToken securityToken)
		: this(new SecurityToken[1] { securityToken })
	{
	}

	public SecurityTokensDataTransferObject(SecurityToken[] securityTokens)
	{
		_version = new Version(1, 0, 0, 0);
		_securityTokens = securityTokens;
	}
}
