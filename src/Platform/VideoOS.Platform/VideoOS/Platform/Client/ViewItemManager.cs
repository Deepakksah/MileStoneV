using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public abstract class ViewItemManager : ConfigurationItemBase
{
	private readonly Guid _uniqueIdentifier = Guid.NewGuid();

	private string _name;

	private bool _generatePropertiesCalledInternally;

	public virtual FQID FQID { get; set; }

	public virtual string DisplayName => string.Empty;

	public virtual IEnumerable<FQID> RelatedCameraIds => Enumerable.Empty<FQID>();

	public Guid UniqueIdentifier => _uniqueIdentifier;

	public virtual IEnumerable<TimelineSequenceSource> TimelineSequenceSources => null;

	protected ViewItemManager()
	{
		new TrackPageViewData(typeof(ViewItemManager)).Send();
	}

	public ViewItemManager(string name)
	{
		_name = name;
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use GeneratePropertiesWpfUserControl() instead.", false)]
	public virtual PropertiesUserControl GeneratePropertiesUserControl()
	{
		if (_generatePropertiesCalledInternally)
		{
			return null;
		}
		_generatePropertiesCalledInternally = true;
		PropertiesWpfUserControl propertiesWpfUserControl = GeneratePropertiesWpfUserControl();
		_generatePropertiesCalledInternally = false;
		if (propertiesWpfUserControl == null)
		{
			return null;
		}
		ClientControl.Instance.RegisterUIControlForAutoTheming(propertiesWpfUserControl);
		return new PropertiesWpfUserControlWinFormsWrapper
		{
			WpfControl = propertiesWpfUserControl
		};
	}

	public virtual PropertiesWpfUserControl GeneratePropertiesWpfUserControl()
	{
		if (_generatePropertiesCalledInternally)
		{
			return null;
		}
		_generatePropertiesCalledInternally = true;
		PropertiesUserControl propertiesUserControl = GeneratePropertiesUserControl();
		_generatePropertiesCalledInternally = false;
		if (propertiesUserControl == null)
		{
			return null;
		}
		ClientControl.Instance.RegisterUIControlForAutoTheming(propertiesUserControl);
		return new PropertiesUserControlWpfWrapper
		{
			WinFormsControl = propertiesUserControl
		};
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use GenerateViewItemWpfUserControl() instead.", false)]
	public virtual ViewItemUserControl GenerateViewItemUserControl()
	{
		ViewItemWpfUserControl viewItemWpfUserControl = GenerateViewItemWpfUserControl();
		if (viewItemWpfUserControl == null)
		{
			EnvironmentManager.Instance.Log(error: true, "ViewItemManager", "GenerateViewItemUserControl() called on ViewItemManager where both GenerateViewItemUserControl and GenerateViewItemWpfUserControl have not been overridden");
			return null;
		}
		ClientControl.Instance.RegisterUIControlForAutoTheming(viewItemWpfUserControl);
		return new ViewItemWpfUserControlWinFormsWrapper
		{
			WpfControl = viewItemWpfUserControl
		};
	}

	public virtual ViewItemWpfUserControl GenerateViewItemWpfUserControl()
	{
		return null;
	}

	public new void Serialize(XmlTextWriter xmlTextWriter)
	{
		xmlTextWriter.WriteStartElement("viewitem");
		xmlTextWriter.WriteAttributeString("id", _Id.ToString());
		xmlTextWriter.WriteAttributeString("type", SerializeTypeString);
		base.Serialize(xmlTextWriter);
		xmlTextWriter.WriteEndElement();
	}

	public new void Deserialize(XmlNode xmlNode)
	{
		XmlAttribute xmlAttribute = xmlNode.Attributes["id"];
		if (xmlAttribute != null)
		{
			try
			{
				_Id = new Guid(xmlAttribute.InnerText);
			}
			catch
			{
			}
		}
		base.Deserialize(xmlNode);
	}

	public bool UpdateProperties(XmlNode xmlNode, List<Guid> idUpdateForChanges, bool force)
	{
		return Update(xmlNode, idUpdateForChanges, force);
	}

	public virtual void PropertiesLoaded()
	{
	}

	[Obsolete]
	public virtual void Init()
	{
	}

	[Obsolete]
	public virtual void Close()
	{
	}
}
