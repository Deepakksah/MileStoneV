using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Principal;

namespace VideoOS.Platform.Util.ImageServer;

public class UserIdentification
{
	private string _name;

	private string _sid;

	private string[] _memberOf;

	private string[] _members;

	private string _domainNameExtra;

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			if (value != null)
			{
				_name = value.ToLower(CultureInfo.InvariantCulture);
			}
			else
			{
				_name = "";
			}
		}
	}

	public string Sid
	{
		get
		{
			return _sid;
		}
		set
		{
			_sid = value?.ToUpper(CultureInfo.InvariantCulture);
		}
	}

	public string[] AllSids
	{
		get
		{
			List<string> list = new List<string>();
			if (!string.IsNullOrEmpty(Sid))
			{
				list.Add(Sid);
			}
			if (MemberOf != null)
			{
				string[] memberOf = MemberOf;
				foreach (string text in memberOf)
				{
					try
					{
						new Guid(text);
						list.Add(text.ToUpper(CultureInfo.InvariantCulture));
					}
					catch (Exception)
					{
					}
				}
			}
			return list.ToArray();
		}
	}

	public string Location { get; set; }

	public bool Deleted { get; set; }

	public string Description { get; set; }

	public string DisplayName { get; set; }

	public string DistinguishedName { get; set; }

	public string Email { get; set; }

	public string[] MemberOf
	{
		get
		{
			return _memberOf;
		}
		set
		{
			if (value != null)
			{
				for (int i = 0; i < value.Length; i++)
				{
					value[i] = value[i].ToUpper(CultureInfo.InvariantCulture);
				}
				_memberOf = value;
			}
			else
			{
				_memberOf = Array.Empty<string>();
			}
		}
	}

	public string[] Members
	{
		get
		{
			return _members;
		}
		set
		{
			if (value != null)
			{
				for (int i = 0; i < value.Length; i++)
				{
					value[i] = value[i].ToUpper(CultureInfo.InvariantCulture);
				}
			}
			_members = value;
		}
	}

	public UserIdType.UserIDType Type { get; set; }

	public string AccountName
	{
		get
		{
			int num = Name.IndexOfAny(new char[2] { '\\', '/' });
			if (num < 0)
			{
				return Name;
			}
			return Name.Substring(num + 1);
		}
	}

	public string Domain
	{
		get
		{
			if (_domainNameExtra != null)
			{
				return _domainNameExtra;
			}
			int num = Name.IndexOfAny(new char[2] { '\\', '/' });
			if (num < 0)
			{
				return "";
			}
			return Name.Substring(0, num).Trim();
		}
		set
		{
			_domainNameExtra = value;
		}
	}

	public UserIdentification()
	{
		_name = "";
		_sid = "";
		Location = "";
		Deleted = false;
		Description = "";
		DisplayName = "";
		DistinguishedName = "";
		MemberOf = null;
		Members = null;
		Type = UserIdType.UserIDType.UnknownIdentityType;
		Email = "";
	}

	public UserIdentification(string name, string sid, string location, bool deleted, string description, string displayName, string distinguishedName, string[] memberOf, string[] members, UserIdType.UserIDType userIdType)
	{
		Name = name;
		Sid = sid;
		Location = location;
		Deleted = deleted;
		Description = description;
		DisplayName = displayName;
		DistinguishedName = distinguishedName;
		MemberOf = memberOf;
		Members = members;
		Type = userIdType;
	}

	public static UserIdentification GetCurrentWindowsIdentityUserID()
	{
		WindowsIdentity current = WindowsIdentity.GetCurrent();
		UserIdentification userIdentification = new UserIdentification();
		userIdentification.Name = current.Name;
		if (current.User != null)
		{
			userIdentification.Sid = current.User.ToString();
		}
		return userIdentification;
	}
}
