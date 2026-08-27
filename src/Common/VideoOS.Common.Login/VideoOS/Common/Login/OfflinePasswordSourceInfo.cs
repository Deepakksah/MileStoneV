using System;
using System.Security;

namespace VideoOS.Common.Login;

public class OfflinePasswordSourceInfo
{
	private Guid _sourceId;

	private string _sourceName;

	private SecureString _sourcePassword;

	public virtual Guid SourceId
	{
		get
		{
			return _sourceId;
		}
		set
		{
			_sourceId = value;
		}
	}

	public virtual string SourceName
	{
		get
		{
			return _sourceName;
		}
		set
		{
			_sourceName = value;
		}
	}

	public virtual SecureString SourcePassword
	{
		get
		{
			return _sourcePassword;
		}
		set
		{
			_sourcePassword = value;
		}
	}

	public OfflinePasswordSourceInfo(Guid sourceId, string sourceName, string sourcePassword)
	{
		_sourceId = sourceId;
		_sourceName = sourceName;
		_sourcePassword = EncryptString(sourcePassword);
	}

	private static SecureString EncryptString(string value)
	{
		SecureString secureString = new SecureString();
		try
		{
			if (value != null)
			{
				char[] array = value.ToCharArray();
				foreach (char c in array)
				{
					secureString.AppendChar(c);
				}
			}
		}
		catch (Exception)
		{
			secureString.Dispose();
			throw;
		}
		return secureString;
	}
}
