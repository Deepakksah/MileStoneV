namespace VideoOS.Installer.Core.Package;

public abstract class Parameter
{
	private string _displayName;

	private string _description;

	private string _name;

	private bool _visible;

	private bool _readOnly;

	private bool _emptyAllowed;

	private bool _omitIfEmpty;

	protected string _value = "";

	public virtual string DisplayName => _displayName;

	public virtual string Description => _description;

	public virtual string Name => _name;

	public virtual bool Visible => _visible;

	public virtual bool ReadOnly => _readOnly;

	public virtual bool EmptyAllowed => _emptyAllowed;

	public virtual bool OmitIfEmpty => _omitIfEmpty;

	public virtual string Value
	{
		get
		{
			return _value;
		}
		set
		{
			if (!_readOnly)
			{
				_value = value;
			}
		}
	}

	public virtual string DisplayValue
	{
		get
		{
			return _value;
		}
		set
		{
			if (!_readOnly)
			{
				_value = value;
			}
		}
	}

	public Parameter(string displayName, string description, string name, bool visible, bool readOnly, bool emptyAllowed, bool omitIfEmpty)
	{
		_displayName = displayName;
		_description = description;
		_name = name;
		_visible = visible;
		_readOnly = readOnly;
		_emptyAllowed = emptyAllowed;
		_omitIfEmpty = omitIfEmpty;
	}
}
