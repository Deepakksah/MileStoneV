using System;

namespace VideoOS.Common.Integration.Security;

public class SecurityAttribute
{
	private SecurityDataSet.SecurityAttributesRow _row;

	private SecurityAttributeDefinition _definition;

	private IComparable _Tag;

	public SecurityDataSet.SecurityAttributesRow DataRow => _row;

	public string Description
	{
		get
		{
			return _row.Description;
		}
		set
		{
			_row.Description = value;
		}
	}

	public string ID
	{
		get
		{
			return _row.SecurityAttributeID.Trim();
		}
		set
		{
			_row.SecurityAttributeID = value;
		}
	}

	public string Name
	{
		get
		{
			return _row.Name.Trim();
		}
		set
		{
			_row.Name = value;
		}
	}

	public string ParentName
	{
		get
		{
			return _row.ParentSecurityAttributeName.Trim();
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			_row.ParentSecurityAttributeName = value.ToString();
		}
	}

	public string Type
	{
		get
		{
			return _row.SecurityType.Trim();
		}
		set
		{
			_row.SecurityType = value;
		}
	}

	public int DisplayOrder
	{
		get
		{
			return _row.DisplayOrder;
		}
		set
		{
			_row.DisplayOrder = value;
		}
	}

	public SecurityAttributeDefinition Definition
	{
		get
		{
			if (_definition == null)
			{
				_definition = new SecurityAttributeDefinition(_row.Definition);
			}
			return _definition;
		}
	}

	public bool HasObject
	{
		get
		{
			return _row.HasObject;
		}
		set
		{
			_row.HasObject = value;
		}
	}

	public bool IsTimeProfileBased
	{
		get
		{
			return _row.TimeProfileBased;
		}
		set
		{
			_row.TimeProfileBased = value;
		}
	}

	public bool IsTimeBackwardBased
	{
		get
		{
			return _row.TimeBackwardBased;
		}
		set
		{
			_row.TimeBackwardBased = value;
		}
	}

	public IComparable Tag
	{
		get
		{
			return _Tag;
		}
		set
		{
			_Tag = value;
		}
	}

	public SecurityAttribute(SecurityDataSet.SecurityAttributesRow dataRow)
	{
		_row = dataRow;
	}
}
