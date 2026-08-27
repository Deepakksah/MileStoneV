using System;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration;

public class SerializableObject : IXmlSerializable
{
	private Type _objectType;

	public string AssemblyQualifiedName
	{
		get
		{
			return _objectType?.AssemblyQualifiedName ?? string.Empty;
		}
		set
		{
			_objectType = (string.IsNullOrEmpty(value) ? null : Type.GetType(value));
		}
	}

	[XmlIgnore]
	public object InnerObject { get; private set; }

	public SerializableObject()
	{
	}

	public SerializableObject(object innerObject)
	{
		InnerObject = innerObject ?? throw new ArgumentNullException("innerObject");
		_objectType = InnerObject.GetType();
		if (_objectType.GetConstructor(Type.EmptyTypes) == null)
		{
			throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("The {0} must have a public paramterless constructor. This is not the caae for the type {1}", "innerObject", _objectType)));
		}
	}

	[Obsolete("This property is for serialization only. NOT to be used by the application")]
	public XmlSchema GetSchema()
	{
		return null;
	}

	[Obsolete("This property is for serialization only. NOT to be used by the application")]
	public void ReadXml(XmlReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		if (reader.IsEmptyElement)
		{
			reader.Read();
			return;
		}
		string attribute = reader.GetAttribute("Type");
		AssemblyQualifiedName = attribute;
		int num = 0;
		while (reader.NodeType != XmlNodeType.EndElement)
		{
			if (_objectType != null)
			{
				reader.Read();
				InnerObject = new XmlSerializer(_objectType).Deserialize(reader);
				reader.MoveToContent();
				num++;
				continue;
			}
			reader.Skip();
			break;
		}
		if (reader.NodeType == XmlNodeType.EndElement)
		{
			reader.ReadEndElement();
		}
	}

	[Obsolete("This property is for serialization only. NOT to be used by the application")]
	public void WriteXml(XmlWriter writer)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		if (!(_objectType == null))
		{
			writer.WriteAttributeString("Type", _objectType.AssemblyQualifiedName);
			new XmlSerializer(_objectType).Serialize(writer, InnerObject);
		}
	}
}
