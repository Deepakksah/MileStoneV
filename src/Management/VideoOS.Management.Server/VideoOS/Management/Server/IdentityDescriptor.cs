using System;
using System.Globalization;

namespace VideoOS.Management.Server;

public sealed class IdentityDescriptor
{
	private string _identifier;

	private string _identityType;

	private object _data;

	public string IdentityType
	{
		get
		{
			return _identityType;
		}
		set
		{
			_identityType = value;
			_data = null;
		}
	}

	public string Identifier
	{
		get
		{
			return _identifier;
		}
		set
		{
			_identifier = value;
			_data = null;
		}
	}

	public object Data
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
		}
	}

	public IdentityDescriptor()
	{
	}

	public IdentityDescriptor(IdentityDescriptor clone)
	{
		if (clone == null)
		{
			throw new ArgumentNullException("clone");
		}
		_identityType = clone.IdentityType;
		_identifier = clone.Identifier;
	}

	public IdentityDescriptor(string identifier)
	{
		_identityType = "Identity";
		_identifier = identifier;
	}

	public IdentityDescriptor(string identityType, string identifier)
	{
		_identityType = identityType;
		_identifier = identifier;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "IdentityDescriptor (IdentityType: {0}; Identifier: {1}", _identityType, _identifier);
	}
}
