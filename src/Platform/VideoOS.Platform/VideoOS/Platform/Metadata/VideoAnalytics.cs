using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class VideoAnalytics : IXmlSerializable, IEquatable<VideoAnalytics>
{
	private static readonly object Lock = new object();

	private static DateTime _lastFrameDiscarded;

	private readonly List<Frame> _frames = new List<Frame>();

	public List<Frame> Frames => _frames;

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		reader.MoveToContent();
		int depth = reader.Depth;
		bool isEmptyElement = reader.IsEmptyElement;
		_frames.Clear();
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
			if ((object)reader.NamespaceURI != MetadataXml.OnvifNamespace || reader.Depth != rootDepth + 1 || reader.NodeType != XmlNodeType.Element || (object)reader.LocalName != MetadataXml.FrameElement)
			{
				continue;
			}
			using XmlReader reader2 = reader.ReadSubtree();
			Frame frame = new Frame();
			frame.ReadXml(reader2);
			if (frame.UtcTimeAttributeWasPresent)
			{
				Frames.Add(frame);
				continue;
			}
			lock (Lock)
			{
				if (DateTime.UtcNow - _lastFrameDiscarded > MetadataXml.LogIgnoreTimeSpand)
				{
					EnvironmentManager.Instance.Log(GetType().FullName, error: false, "ReadXml", "Element 'Frame' is incomplete and will be discarded", null);
					_lastFrameDiscarded = DateTime.UtcNow;
				}
			}
		}
		while (reader.Depth != rootDepth && reader.Read());
	}

	public void WriteXml(XmlWriter writer)
	{
		foreach (Frame frame in _frames)
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.FrameElement, MetadataXml.OnvifNamespace);
			frame.WriteXml(writer);
			writer.WriteEndElement();
		}
	}

	public bool Equals(VideoAnalytics other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		return _frames.SequenceEqual(other._frames);
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
		return Equals((VideoAnalytics)obj);
	}

	public override int GetHashCode()
	{
		if (_frames == null || _frames.Count == 0)
		{
			return 0;
		}
		return _frames.Select((Frame elem) => elem.GetHashCode()).Aggregate((int v1, int v2) => v1.GetHashCode() ^ v2.GetHashCode());
	}
}
