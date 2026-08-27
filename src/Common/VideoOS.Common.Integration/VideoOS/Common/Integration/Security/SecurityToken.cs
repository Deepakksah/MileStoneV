using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration.Security;

[Serializable]
[DataContract]
public class SecurityToken
{
	[DataMember]
	private string _token;

	[DataMember]
	private Identity _identity;

	[DataMember]
	private TimeSpan _expires;

	[DataMember]
	private DateTime _registrationTime;

	[DataMember]
	private bool _expireTimeLimited;

	[DataMember]
	private bool _managementClientToken;

	private readonly bool _isUnavailable;

	public static readonly TimeSpan ExtraTimeBeforeRemoval = TimeSpan.FromHours(6.0);

	public Identity Identity
	{
		get
		{
			return _identity;
		}
		set
		{
			_identity = value;
		}
	}

	public string Value => _token;

	public string EscapedValue => _token.Replace("#", "%23");

	public bool IsUnavailable => _isUnavailable;

	public bool IsNull => _token == null;

	public TimeSpan Expires
	{
		get
		{
			return _expires;
		}
		set
		{
			_expires = value;
		}
	}

	public DateTime RegistrationTime => _registrationTime;

	public bool ExpireTimeLimited
	{
		get
		{
			return _expireTimeLimited;
		}
		set
		{
			_expireTimeLimited = value;
		}
	}

	public bool IsExpired => DateTime.UtcNow > _registrationTime + _expires;

	public bool IsCloseToExpire => DateTime.UtcNow > _registrationTime + _expires - new TimeSpan(_expires.Ticks / 10);

	public bool IsExpiredAndOkToRemove => DateTime.UtcNow > _registrationTime + _expires + ExtraTimeBeforeRemoval;

	public bool ManagementClientToken => _managementClientToken;

	public static SecurityToken Empty => new SecurityToken(DateTime.UtcNow, default(TimeSpan));

	public SecurityToken(Identity identity, string token, DateTime registrationTime, TimeSpan expires)
		: this(identity, token, registrationTime, expires, managementClientToken: false)
	{
	}

	public SecurityToken(Identity identity, string token, DateTime registrationTime, TimeSpan expires, bool managementClientToken)
	{
		if (identity == null)
		{
			throw new ArgumentNullException("identity");
		}
		if (identity.Type == IdentityType.InvalidIdentity)
		{
			throw new ArgumentException("Identity is of an invalid type", "identity");
		}
		if (string.IsNullOrEmpty(token))
		{
			throw new ArgumentException("Token can not be empty", "token");
		}
		_identity = identity;
		_expires = expires;
		_registrationTime = registrationTime;
		_token = token;
		_managementClientToken = managementClientToken;
	}

	private SecurityToken(DateTime registrationTime, TimeSpan expires)
	{
		_identity = null;
		_expires = expires;
		_registrationTime = registrationTime;
		_token = string.Empty;
		_managementClientToken = false;
	}

	public SecurityToken(string token, bool isUnavailable)
	{
		_token = token;
		_isUnavailable = isUnavailable;
	}

	public SecurityToken(Identity identity, string localAreaNetworkAddress, string wideAreaNetworkAddress, int timeoutMinutes)
		: this(identity, localAreaNetworkAddress, wideAreaNetworkAddress, timeoutMinutes, managementClientToken: false)
	{
	}

	public SecurityToken(Identity identity, string localAreaNetworkAddress, string wideAreaNetworkAddress, int timeoutMinutes, bool managementClientToken)
		: this(identity, NewToken(localAreaNetworkAddress, wideAreaNetworkAddress), DateTime.UtcNow, new TimeSpan(0, timeoutMinutes, 0), managementClientToken)
	{
	}

	private static string NewToken(string localAreaNetworkAddress, string wideAreaNetworkAddress)
	{
		string text = localAreaNetworkAddress;
		if (!string.IsNullOrEmpty(text))
		{
			if (text.StartsWith("http://", StringComparison.Ordinal))
			{
				text = text.Substring("http://".Length);
			}
			text = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", text, "ServerConnector");
		}
		string text2 = wideAreaNetworkAddress;
		if (!string.IsNullOrEmpty(text2))
		{
			if (text2.StartsWith("http://", StringComparison.Ordinal))
			{
				text2 = text2.Substring("http://".Length);
			}
			text2 = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", text2, "ServerConnector");
		}
		return string.Format(CultureInfo.InvariantCulture, "TOKEN#{0}#{1}#{2}", Guid.NewGuid(), text, text2);
	}
}
