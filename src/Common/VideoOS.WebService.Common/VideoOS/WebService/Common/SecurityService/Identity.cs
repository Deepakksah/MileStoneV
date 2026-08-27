using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace VideoOS.WebService.Common.SecurityService;

[Serializable]
[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSAuthorization")]
public class Identity
{
	private string _sid;

	private IdentityType _identityType;

	private string _accountName;

	private string _displayName;

	private string _domain;

	private string _distinguishedName;

	private bool _deleted;

	private string _description;

	private string[] _memberOf;

	private string[] _members;

	[DataMember]
	public string Sid
	{
		get
		{
			return _sid;
		}
		set
		{
			_sid = value;
		}
	}

	[DataMember]
	public IdentityType Type
	{
		get
		{
			return _identityType;
		}
		set
		{
			_identityType = value;
		}
	}

	[DataMember]
	public string AccountName
	{
		get
		{
			return _accountName;
		}
		set
		{
			_accountName = value;
		}
	}

	[DataMember]
	public string DisplayName
	{
		get
		{
			return _displayName;
		}
		set
		{
			_displayName = value;
		}
	}

	[DataMember]
	public string Domain
	{
		get
		{
			return _domain;
		}
		set
		{
			_domain = value;
		}
	}

	public string DomainAccountName => string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", _domain, _accountName);

	[DataMember]
	public string DistinguishedName
	{
		get
		{
			return _distinguishedName;
		}
		set
		{
			_distinguishedName = value;
		}
	}

	[DataMember]
	public bool Deleted
	{
		get
		{
			return _deleted;
		}
		set
		{
			_deleted = value;
		}
	}

	[DataMember]
	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value;
		}
	}

	[DataMember]
	public string[] MemberOf
	{
		get
		{
			return _memberOf;
		}
		set
		{
			_memberOf = value;
		}
	}

	public Guid[] MemberOfRoles
	{
		get
		{
			if (_memberOf == null)
			{
				_memberOf = new string[0];
				return new Guid[0];
			}
			return (from i in _memberOf
				where i.IsGuid()
				select Guid.Parse(i)).ToArray();
		}
	}

	[DataMember]
	public string[] Members
	{
		get
		{
			return _members;
		}
		set
		{
			_members = value;
		}
	}

	public Identity()
	{
		_identityType = IdentityType.InvalidIdentity;
		_sid = string.Empty;
		_accountName = string.Empty;
		_displayName = string.Empty;
		_domain = string.Empty;
		_distinguishedName = string.Empty;
		_deleted = false;
		_description = string.Empty;
		_memberOf = new string[0];
		_members = new string[0];
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (_sid != null)
		{
			stringBuilder.Append("[");
			stringBuilder.Append(_sid);
			stringBuilder.Append("] ");
		}
		switch (_identityType)
		{
		case IdentityType.InvalidIdentity:
			return "(invalid)";
		case IdentityType.UnknownIdentityType:
			stringBuilder.Append("Unknown: ");
			stringBuilder.Append(_domain);
			stringBuilder.Append("\\");
			stringBuilder.Append(_accountName);
			stringBuilder.Append(" (");
			stringBuilder.Append(_displayName);
			stringBuilder.Append(")");
			break;
		case IdentityType.WindowsUser:
			stringBuilder.Append("User: ");
			stringBuilder.Append(_domain);
			stringBuilder.Append("\\");
			stringBuilder.Append(_accountName);
			stringBuilder.Append(" (");
			stringBuilder.Append(_displayName);
			stringBuilder.Append(")");
			if (_memberOf != null)
			{
				stringBuilder.Append(" MemberOf: ");
				stringBuilder.Append(_memberOf.Length.ToString(CultureInfo.InvariantCulture));
			}
			break;
		case IdentityType.WindowsGroup:
			stringBuilder.Append("Group: ");
			stringBuilder.Append(_domain);
			stringBuilder.Append("\\");
			stringBuilder.Append(_accountName);
			stringBuilder.Append(" (");
			stringBuilder.Append(_displayName);
			stringBuilder.Append(")");
			if (_members != null)
			{
				stringBuilder.Append(" Members: ");
				stringBuilder.Append(_members.Length.ToString(CultureInfo.InvariantCulture));
			}
			if (_memberOf != null)
			{
				stringBuilder.Append(" MemberOf: ");
				stringBuilder.Append(_memberOf.Length.ToString(CultureInfo.InvariantCulture));
			}
			break;
		case IdentityType.Role:
			stringBuilder.Append("Role: ");
			stringBuilder.Append(_displayName);
			if (_members != null)
			{
				stringBuilder.Append(" Members: ");
				stringBuilder.Append(_members.Length.ToString(CultureInfo.InvariantCulture));
			}
			break;
		}
		return stringBuilder.ToString();
	}
}
