using System;
using System.Collections;

namespace VideoOS.Common.Integration.Security;

public class SecurityAttributes : DictionaryBase
{
	protected SecurityManager _manager;

	private SecurityDataSet.SecurityAttributesDataTable _table;

	public SecurityAttribute this[string name] => (SecurityAttribute)base.Dictionary[name];

	public ICollection Keys => base.Dictionary.Keys;

	public ICollection Values => base.Dictionary.Values;

	public SecurityAttributes(SecurityManager manager)
	{
		if (manager == null)
		{
			throw new ArgumentNullException("manager");
		}
		_manager = manager;
		_table = _manager.DataSet.SecurityAttributes;
		foreach (SecurityDataSet.SecurityAttributesRow row in _table.Rows)
		{
			Add(row);
		}
	}

	public SecurityAttribute Add(string name, string displayName, string description, string parentAttribute, string type, bool hasObject, int displayOrder, string definition, bool isTimeProfileBased, bool isTimeBackwardBased)
	{
		SecurityAttribute securityAttribute = new SecurityAttribute(_manager.DataSet.SecurityAttributes.AddSecurityAttributesRow(DateTime.UtcNow, name, displayName, description, parentAttribute, type, hasObject, displayOrder, definition, isTimeProfileBased, isTimeBackwardBased));
		base.Dictionary.Add(securityAttribute.ID.Trim(), securityAttribute);
		return securityAttribute;
	}

	public SecurityAttribute Add(SecurityDataSet.SecurityAttributesRow row)
	{
		SecurityAttribute securityAttribute = new SecurityAttribute(row);
		base.Dictionary.Add(securityAttribute.ID.Trim(), securityAttribute);
		return securityAttribute;
	}

	public bool Contains(string name)
	{
		return base.Dictionary.Contains(name);
	}

	public void Remove(string Name)
	{
		SecurityAttribute securityAttribute = this[Name];
		if (securityAttribute != null)
		{
			SecurityDataSet.SecurityAttributesRow dataRow = securityAttribute.DataRow;
			base.Dictionary.Remove(Name);
			_manager.DataSet.SecurityAttributes.RemoveSecurityAttributesRow(dataRow);
		}
	}
}
