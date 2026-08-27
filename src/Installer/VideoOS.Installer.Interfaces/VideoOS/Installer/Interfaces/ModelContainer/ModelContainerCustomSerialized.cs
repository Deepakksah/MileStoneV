using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Installer.Interfaces.ModelContainer;

public abstract class ModelContainerCustomSerialized<TModel> : IXmlSerializable, IModelContainer
{
	[XmlIgnore]
	protected TModel Content { get; set; }

	protected abstract IEnumerable<Type> Interfaces { get; }

	object IModelContainer.Content => Content;

	IEnumerable<Type> IModelContainer.Interfaces => Interfaces;

	protected abstract TModel CreateModel();

	protected abstract void Persist();

	protected abstract void SerializeModel(XmlWriter xmlWriter);

	protected abstract void DeserializeModel(XmlReader xmlReader);

	void IModelContainer.Persist()
	{
		Persist();
	}

	public XmlSchema GetSchema()
	{
		return null;
	}

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
		int num = 0;
		while (reader.NodeType != XmlNodeType.EndElement)
		{
			reader.Read();
			Content = CreateModel();
			DeserializeModel(reader);
			reader.MoveToContent();
			num++;
		}
		if (reader.NodeType == XmlNodeType.EndElement)
		{
			reader.ReadEndElement();
		}
	}

	public void WriteXml(XmlWriter writer)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		SerializeModel(writer);
	}
}
