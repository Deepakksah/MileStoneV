using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration.Security;

public abstract class TokenHandlerBase : IDisposable
{
	private ILogger logger;

	private const int MilliSecBetweenChecks = 300000;

	private Timer tokenTimer;

	public Dictionary<string, SecurityToken> Tokens { get; private set; }

	public bool RelaxedTokenCheck { get; set; }

	protected TokenHandlerBase(ILogger logger)
	{
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		this.logger = logger;
		Tokens = new Dictionary<string, SecurityToken>();
		tokenTimer = new Timer(CheckTokenExpirationTimeCallback, null, 300000, 300000);
	}

	protected abstract void AddToken(SecurityToken securityToken);

	protected abstract SecurityToken FindToken(string token);

	public TokenStatus CheckToken(string token)
	{
		SecurityToken securityToken = FindToken(token);
		return VerifyToken(securityToken, allowRelaxedTokenCheck: true);
	}

	public SecurityToken GetToken(string token, out TokenStatus tokenStatus)
	{
		SecurityToken securityToken = FindToken(token);
		tokenStatus = VerifyToken(securityToken, allowRelaxedTokenCheck: false);
		if (tokenStatus != TokenStatus.Ok)
		{
			securityToken = null;
		}
		return securityToken;
	}

	public SecurityToken GetToken(string token)
	{
		TokenStatus tokenStatus;
		return GetToken(token, out tokenStatus);
	}

	public TokenStatus CheckToken(SecurityToken securityToken)
	{
		return VerifyToken(securityToken, allowRelaxedTokenCheck: true);
	}

	public void UpdateTokenIdentity(string token, Identity identity)
	{
		lock (Tokens)
		{
			SecurityToken value = null;
			if (Tokens.TryGetValue(token, out value))
			{
				value.Identity = identity;
			}
		}
	}

	protected TokenStatus VerifyToken(SecurityToken securityToken, bool allowRelaxedTokenCheck)
	{
		TokenStatus tokenStatus = TokenStatus.Invalid;
		if (securityToken != null)
		{
			tokenStatus = (securityToken.IsUnavailable ? TokenStatus.Unavailable : (securityToken.IsExpired ? ((!allowRelaxedTokenCheck || !RelaxedTokenCheck) ? TokenStatus.Expired : TokenStatus.Ok) : TokenStatus.Ok));
		}
		if (tokenStatus != TokenStatus.Ok)
		{
			LoggerExtensions.LogWarning(logger, "Token verify failed: {TokenStatus}", new object[1] { tokenStatus.ToString() });
		}
		return tokenStatus;
	}

	private void CheckTokenExpirationTimeCallback(object state)
	{
		List<SecurityToken> list = new List<SecurityToken>();
		lock (Tokens)
		{
			list.AddRange(Tokens.Values.Where((SecurityToken token) => token.IsExpiredAndOkToRemove));
			list.ForEach(delegate(SecurityToken token)
			{
				Tokens.Remove(token.Value);
				LoggerExtensions.LogInformation(logger, "Token deleted ({TokensCount})", new object[1] { Tokens.Count });
			});
		}
	}

	public void Close()
	{
		lock (Tokens)
		{
			tokenTimer.Dispose();
			Tokens.Clear();
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && tokenTimer != null)
		{
			tokenTimer.Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
