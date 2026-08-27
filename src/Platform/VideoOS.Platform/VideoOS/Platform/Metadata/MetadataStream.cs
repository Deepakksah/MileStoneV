using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class MetadataStream : IXmlSerializable, IEquatable<MetadataStream>
{
	private readonly List<VideoAnalytics> _videoAnalyticsItems = new List<VideoAnalytics>();

	private readonly List<byte> _originalData = new List<byte>();

	public List<VideoAnalytics> VideoAnalyticsItems => _videoAnalyticsItems;

	public List<byte> OriginalData => _originalData;

	public NavigationalData NavigationalData { get; set; }

	public IEnumerable<Frame> GetAllFrames()
	{
		return _videoAnalyticsItems.SelectMany((VideoAnalytics va) => va.Frames);
	}

	public Frame GetFrame()
	{
		return _videoAnalyticsItems.SelectMany((VideoAnalytics va) => va.Frames).FirstOrDefault();
	}

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		reader.MoveToContent();
		int depth = reader.Depth;
		bool isEmptyElement = reader.IsEmptyElement;
		BlankAllFields();
		reader.ReadStartElement();
		if (!isEmptyElement)
		{
			ReadChildren(reader, depth);
			reader.ReadEndElement();
		}
	}

	private void BlankAllFields()
	{
		NavigationalData = null;
		_originalData.Clear();
		_videoAnalyticsItems.Clear();
	}

	private void ReadChildren(XmlReader reader, int rootDepth)
	{
		do
		{
			if ((object)reader.NamespaceURI != MetadataXml.OnvifNamespace || reader.Depth != rootDepth + 1 || reader.NodeType != XmlNodeType.Element)
			{
				continue;
			}
			if ((object)reader.LocalName == MetadataXml.VideoAnalyticsElement)
			{
				using XmlReader reader2 = reader.ReadSubtree();
				VideoAnalytics videoAnalytics = new VideoAnalytics();
				videoAnalytics.ReadXml(reader2);
				_videoAnalyticsItems.Add(videoAnalytics);
			}
			if ((object)reader.LocalName == MetadataXml.ExtensionElement)
			{
				ReadExtensionData(reader.ReadSubtree());
			}
		}
		while (reader.Depth != rootDepth && reader.Read());
	}

	private void ReadExtensionData(XmlReader reader)
	{
		do
		{
			if (reader.NodeType != XmlNodeType.Element)
			{
				continue;
			}
			if ((object)MetadataXml.OriginalDataElement == reader.LocalName)
			{
				byte[] collection = Convert.FromBase64String(reader.ReadElementString());
				_originalData.AddRange(collection);
			}
			if ((object)MetadataXml.NavigationalDataElement == reader.LocalName)
			{
				using XmlReader reader2 = reader.ReadSubtree();
				NavigationalData = new NavigationalData();
				NavigationalData.ReadXml(reader2);
			}
		}
		while (reader.Read());
	}

	public void WriteXml(XmlWriter writer)
	{
		foreach (VideoAnalytics videoAnalyticsItem in VideoAnalyticsItems)
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.VideoAnalyticsElement, MetadataXml.OnvifNamespace);
			videoAnalyticsItem.WriteXml(writer);
			writer.WriteEndElement();
		}
		if (ExtensionDataPresent())
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.ExtensionElement, MetadataXml.OnvifNamespace);
			if (NavigationalData != null)
			{
				writer.WriteStartElement(MetadataXml.NavigationalDataElement);
				NavigationalData.WriteXml(writer);
				writer.WriteEndElement();
			}
			if (_originalData.Count != 0)
			{
				writer.WriteStartElement(MetadataXml.OriginalDataElement);
				writer.WriteBase64(_originalData.ToArray(), 0, _originalData.Count);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}
	}

	private bool ExtensionDataPresent()
	{
		if (NavigationalData == null)
		{
			return _originalData.Count != 0;
		}
		return true;
	}

	public bool Equals(MetadataStream other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (_videoAnalyticsItems.SequenceEqual(other._videoAnalyticsItems))
		{
			return _originalData.SequenceEqual(other._originalData);
		}
		return false;
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
		return Equals((MetadataStream)obj);
	}

	public override int GetHashCode()
	{
		return (((_videoAnalyticsItems != null && _videoAnalyticsItems.Count != 0) ? _videoAnalyticsItems.Select((VideoAnalytics elem) => elem.GetHashCode()).Aggregate((int v1, int v2) => v1.GetHashCode() ^ v2.GetHashCode()) : 0) * 397) ^ ((_originalData != null) ? _originalData.GetHashCode() : 0);
	}
}
