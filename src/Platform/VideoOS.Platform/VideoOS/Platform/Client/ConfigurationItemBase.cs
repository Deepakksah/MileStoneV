using System;
using System.Collections.Generic;
using System.Xml;

namespace VideoOS.Platform.Client;

public abstract class ConfigurationItemBase : PropertyClass
{
	protected Guid _Id;

	public Guid Id
	{
		get
		{
			return _Id;
		}
		public set
		{
			_Id = value;
		}
	}

	public virtual string SerializeTypeString => GetType().FullName + ", " + GetType().Assembly.GetName().Name;

	public event EventHandler PropertyChangedEvent;

	public override bool Update(XmlNode xmlNode, List<Guid> idUpdateForChanges, bool force)
	{
		return base.Update(xmlNode, idUpdateForChanges, force);
	}

	protected virtual void SaveProperties()
	{
		if (this.PropertyChangedEvent != null)
		{
			this.PropertyChangedEvent(this, new EventArgs());
		}
	}
}
