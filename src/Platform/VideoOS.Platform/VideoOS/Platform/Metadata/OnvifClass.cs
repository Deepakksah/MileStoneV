using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class OnvifClass : IXmlSerializable, IEquatable<OnvifClass>
{
	private static readonly object Lock = new object();

	private static DateTime _lastInvalidClassCandidate;

	private readonly List<ClassCandidate> _classCandidateItems = new List<ClassCandidate>();

	public IList<ClassCandidate> ClassCandidates => _classCandidateItems;

	public XmlSchema GetSchema()
	{
		throw new NotImplementedException();
	}

	public void ReadXml(XmlReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		reader.MoveToContent();
		ClassCandidates.Clear();
		bool isEmptyElement = reader.IsEmptyElement;
		int depth = reader.Depth;
		reader.ReadStartElement();
		if (!isEmptyElement)
		{
			ReadChildren(reader, depth);
			reader.ReadEndElement();
		}
	}

	private void ReadChildren(XmlReader reader, int rootDepth)
	{
		do
		{
			if ((object)reader.NamespaceURI != MetadataXml.OnvifNamespace || reader.Depth != rootDepth + 1 || reader.NodeType != XmlNodeType.Element || (object)reader.LocalName != MetadataXml.ClassCandidateElement)
			{
				continue;
			}
			using XmlReader reader2 = reader.ReadSubtree();
			ClassCandidate classCandidate = new ClassCandidate();
			classCandidate.ReadXml(reader2);
			if (classCandidate.IsValid)
			{
				ClassCandidates.Add(classCandidate);
				continue;
			}
			lock (Lock)
			{
				if (DateTime.UtcNow - _lastInvalidClassCandidate > MetadataXml.LogIgnoreTimeSpand)
				{
					EnvironmentManager.Instance.Log(GetType().FullName, error: false, "ReadXml", "Element 'ClassCandidate' is invalid and will be discarded. This message is logged at most once per minute", null);
					_lastInvalidClassCandidate = DateTime.UtcNow;
				}
			}
		}
		while (reader.Depth != rootDepth && reader.Read());
	}

	public void WriteXml(XmlWriter writer)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		foreach (ClassCandidate classCandidate in ClassCandidates)
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.ClassCandidateElement, MetadataXml.OnvifNamespace);
			classCandidate.WriteXml(writer);
			writer.WriteEndElement();
		}
	}

	public bool Equals(OnvifClass other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		return _classCandidateItems.SequenceEqual(other._classCandidateItems);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((OnvifClass)obj);
	}

	public override int GetHashCode()
	{
		return _classCandidateItems.GetHashCode();
	}
}
