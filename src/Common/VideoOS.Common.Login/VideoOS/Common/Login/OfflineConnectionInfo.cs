using System;
using System.Collections.Generic;

namespace VideoOS.Common.Login;

public class OfflineConnectionInfo : IOfflineConnectionInfo, IConnectionInfo
{
	private readonly List<OfflinePasswordSourceInfo> _passwordSourceInfos;

	private string _projectPassword;

	public AsyncLoginCallback<IOfflineConnectionInfo> AsyncLoginCallback { get; }

	public string ProjectName { get; set; }

	public string ProjectPassword
	{
		get
		{
			if (string.IsNullOrEmpty(_projectPassword))
			{
				return null;
			}
			try
			{
				return DataProtection.Unprotect(_projectPassword);
			}
			catch
			{
				return string.Empty;
			}
		}
		set
		{
			try
			{
				if (!string.IsNullOrEmpty(value))
				{
					_projectPassword = DataProtection.Protect(value);
				}
				else
				{
					_projectPassword = null;
				}
			}
			catch
			{
			}
		}
	}

	public IList<OfflinePasswordSourceInfo> PasswordSourceInfos => _passwordSourceInfos;

	public OfflineConnectionInfo(AsyncLoginCallback<IOfflineConnectionInfo> asyncLoginCallback)
	{
		AsyncLoginCallback = asyncLoginCallback ?? throw new ArgumentNullException("asyncLoginCallback");
		_passwordSourceInfos = new List<OfflinePasswordSourceInfo>();
	}
}
