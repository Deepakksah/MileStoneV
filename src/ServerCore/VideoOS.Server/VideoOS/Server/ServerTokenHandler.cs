using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Security;
using VideoOS.Management.Common;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server;

public sealed class ServerTokenHandler : TokenHandlerBase
{
	private static readonly ILogger Logger = Telemetry.GetILogger("SecurityTokenHandler");

	private Timer expiredTokenTimer;

	private const int MilliSecBetweenChecks = 3600000;

	private readonly ITokenSqlData tokenSqlData;

	private bool firstGetAllTokensCall = true;

	private static readonly object _getAllTokenLocker = new object();

	private volatile bool _updateSecurityTokenPending;

	private volatile bool _backgroundThreadRunning;

	private static readonly object _threadLocker = new object();

	private Thread _updateSecurityTokenThread;

	public ServerTokenHandler()
		: base(Logger)
	{
		Logger.LogInformation("TokenHandler started");
		tokenSqlData = new TokenSqlData(GetIdentityFromSid, Logger);
		expiredTokenTimer = new Timer(CheckTokenExpirationTimeCallback, null, 3600000, 3600000);
	}

	private void CheckTokenExpirationTimeCallback(object state)
	{
		tokenSqlData.ClearExpiredTokens();
	}

	public Identity GetIdentityFromSid(string sid)
	{
		return IdentityCache.ReadIdentityFromCache(sid, QueryMembership.Expanded);
	}

	public SecurityToken GetNewToken(string sid, TimeSpan timeout)
	{
		SecurityToken securityToken = tokenSqlData.AddToken(sid, timeout);
		Logger.LogInformation("Token created, RegTime = {RegistrationDate} {RegistrationTime}", securityToken.RegistrationTime.ToShortDateString(), securityToken.RegistrationTime.ToShortTimeString());
		AddToken(securityToken);
		return securityToken;
	}

	public SecurityToken[] GetAllTokens()
	{
		SecurityToken[] array = null;
		lock (_getAllTokenLocker)
		{
			if (firstGetAllTokensCall && tokenSqlData.GetAllTokens(out var result))
			{
				SecurityToken[] array2 = result;
				foreach (SecurityToken securityToken in array2)
				{
					lock (base.Tokens)
					{
						if (!base.Tokens.ContainsKey(securityToken.Value))
						{
							base.Tokens.Add(securityToken.Value, securityToken);
						}
					}
				}
				firstGetAllTokensCall = false;
			}
			lock (base.Tokens)
			{
				if (base.Tokens.Count > 0)
				{
					array = base.Tokens.Values.Where((SecurityToken token) => !token.IsExpired && token.Identity != null).ToArray();
				}
				Logger.LogDebug("GetAllTokens, count = {TokenCount}", (array != null) ? array.Length.ToString(CultureInfo.InvariantCulture) : "0");
			}
			return array;
		}
	}

	public void UpdateSecurityToken()
	{
		_updateSecurityTokenPending = true;
		if (_backgroundThreadRunning)
		{
			return;
		}
		lock (_threadLocker)
		{
			if (!_backgroundThreadRunning)
			{
				_backgroundThreadRunning = true;
				_updateSecurityTokenThread = new Thread(UpdateSecurityTokenThreadFunction);
				_updateSecurityTokenThread.Name = "UpdateSecurityTokenThreadFunction";
				_updateSecurityTokenThread.Start();
			}
		}
	}

	private void UpdateSecurityTokenThreadFunction()
	{
		try
		{
			while (_updateSecurityTokenPending)
			{
				_updateSecurityTokenPending = false;
				Dictionary<string, Dictionary<string, Identity>> dictionary = new Dictionary<string, Dictionary<string, Identity>>();
				List<SecurityToken> list = new List<SecurityToken>();
				SecurityToken[] allTokens = GetAllTokens();
				if (allTokens != null)
				{
					SecurityToken[] array = allTokens;
					foreach (SecurityToken securityToken in array)
					{
						string sid = securityToken.Identity.Sid;
						if (!dictionary.TryGetValue(sid, out var value))
						{
							value = new Dictionary<string, Identity>();
							dictionary.Add(sid, value);
						}
						value.Add(securityToken.Value, securityToken.Identity);
					}
					foreach (KeyValuePair<string, Dictionary<string, Identity>> item in dictionary)
					{
						string key = item.Key;
						Dictionary<string, Identity> value2 = item.Value;
						List<string> newIdentityRoles = new List<string>();
						Identity identity = IdentityCache.ReadIdentityFromCache(key, QueryMembership.Expanded);
						if (identity != null)
						{
							newIdentityRoles = new List<string>(identity.MemberOf.Where((string input) => input.IsGuid()));
						}
						foreach (KeyValuePair<string, Identity> item2 in value2)
						{
							string key2 = item2.Key;
							Identity value3 = item2.Value;
							List<string> list2 = new List<string>(value3.MemberOf.Where((string input) => input.IsGuid()));
							if (newIdentityRoles.Count != list2.Count || list2.RemoveAll((string x) => !newIdentityRoles.Contains(x)) > 0)
							{
								if (identity == null)
								{
									identity = new Identity();
									identity.Sid = value3.Sid;
									identity.AccountName = value3.AccountName;
									identity.DisplayName = value3.DisplayName;
									identity.Domain = value3.Domain;
									identity.DistinguishedName = value3.DistinguishedName;
									identity.Deleted = value3.Deleted;
									identity.Description = value3.Description;
									identity.MemberOf = new string[0];
									identity.Members = new string[0];
									identity.Type = value3.Type;
								}
								UpdateTokenIdentity(key2, identity);
								SecurityToken token = GetToken(key2);
								if (token != null)
								{
									list.Add(token);
								}
							}
						}
					}
					if (list.Count > 0)
					{
						ICallContextInfo callContextInfo = Server.Instance.CreateServerServiceCallContextUserInfo();
						Server.Instance.RecorderCommManager.UpdateUserSecurityTokensAsync(list.ToArray(), callContextInfo);
					}
				}
				Thread.Sleep(1000);
			}
		}
		catch (ThreadInterruptedException)
		{
			return;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			Logger.LogError("ServerTokenHandler.UpdateSecurityTokenThreadFunction exception: {Exception}", ex3);
		}
		if (_updateSecurityTokenPending)
		{
			UpdateSecurityTokenThreadFunction();
		}
		else
		{
			_backgroundThreadRunning = false;
		}
	}

	protected override SecurityToken FindToken(string token)
	{
		SecurityToken value = null;
		lock (base.Tokens)
		{
			base.Tokens.TryGetValue(token, out value);
			if (value == null)
			{
				value = tokenSqlData.GetSecurityToken(token);
				if (value != null && !value.IsUnavailable)
				{
					base.Tokens.Add(value.Value, value);
					return value;
				}
			}
			if (value != null && value.IsUnavailable)
			{
				base.Tokens.Remove(token);
				tokenSqlData.DeleteToken(token);
				value = null;
			}
		}
		return value;
	}

	protected override void AddToken(SecurityToken securityToken)
	{
		if (securityToken == null)
		{
			throw new ArgumentNullException("securityToken");
		}
		lock (base.Tokens)
		{
			SecurityToken value = null;
			if (base.Tokens.TryGetValue(securityToken.Value, out value))
			{
				value.Identity = securityToken.Identity;
				Logger.LogInformation("Token updated ({TokenCount})", base.Tokens.Count);
			}
			else
			{
				base.Tokens.Add(securityToken.Value, securityToken);
				Logger.LogInformation("Token added ({TokenCount})", base.Tokens.Count);
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (expiredTokenTimer != null)
			{
				expiredTokenTimer.Dispose();
			}
			if (_updateSecurityTokenThread != null)
			{
				_updateSecurityTokenThread.Interrupt();
				_updateSecurityTokenThread.Join(1000);
				_updateSecurityTokenThread = null;
			}
		}
		base.Dispose(disposing);
	}
}
