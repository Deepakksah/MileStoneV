using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class ClassCandidate : IXmlSerializable, IEquatable<ClassCandidate>
{
	private const float DefaultLikelihood = 0f;

	private const string DefaultType = null;

	private static readonly object Lock = new object();

	private static DateTime _lastTypeErrorLogMessage;

	private static DateTime _lastLikelihoodErrorLogMessage;

	public string Type { get; set; }

	public float Likelihood { get; set; }

	public bool IsValid
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(Type) && Likelihood > 0f)
			{
				return Likelihood <= 1f;
			}
			return false;
		}
	}

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

	public void WriteXml(XmlWriter writer)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		writer.WriteElementString(MetadataXml.OnvifPrefix, MetadataXml.TypeElement, MetadataXml.OnvifNamespace, Type);
		writer.WriteElementString(MetadataXml.OnvifPrefix, MetadataXml.LikelihoodElement, MetadataXml.OnvifNamespace, Likelihood.ToString(MetadataXml.Culture));
	}

	private void BlankAllFields()
	{
		Type = null;
		Likelihood = 0f;
	}

	private void ReadChildren(XmlReader reader, int rootDepth)
	{
		bool flag = false;
		bool flag2 = false;
		do
		{
			if ((object)reader.NamespaceURI == MetadataXml.OnvifNamespace && reader.Depth == rootDepth + 1 && reader.NodeType == XmlNodeType.Element)
			{
				if ((object)reader.LocalName == MetadataXml.TypeElement)
				{
					flag = true;
					ConsumeChildTreeNodes(reader, rootDepth);
					Type = ParseRequiredStringValue(reader.ReadContentAsString());
				}
				else if ((object)reader.LocalName == MetadataXml.LikelihoodElement)
				{
					flag2 = true;
					ConsumeChildTreeNodes(reader, rootDepth);
					Likelihood = ParseRequiredLikelihoodValue(reader.ReadContentAsString());
				}
			}
		}
		while (reader.Depth != rootDepth && reader.Read());
		if (!flag)
		{
			LogTypeElementError("Required element 'Type' is missing.");
		}
		if (!flag2)
		{
			LogLikelihoodElementError("Required element 'Likelihood' is missing.");
		}
	}

	private static void ConsumeChildTreeNodes(XmlReader reader, int rootDepth)
	{
		while (reader.NodeType != XmlNodeType.Text && reader.Depth >= rootDepth + 1 && reader.Read())
		{
		}
	}

	private string ParseRequiredStringValue(string typeValue)
	{
		if (string.IsNullOrWhiteSpace(typeValue))
		{
			LogTypeElementError("Element 'Type' is empty.");
			return null;
		}
		return typeValue;
	}

	private float ParseRequiredLikelihoodValue(string likelihoodValue)
	{
		if (!float.TryParse(likelihoodValue, MetadataXml.FloatStyle, MetadataXml.Culture, out var result))
		{
			string message = string.Format(CultureInfo.InvariantCulture, "Required element 'Likelihood' is not a number. Value read: {0}", likelihoodValue);
			LogLikelihoodElementError(message);
			return 0f;
		}
		return result;
	}

	private void LogLikelihoodElementError(string message)
	{
		lock (Lock)
		{
			if (DateTime.UtcNow - _lastLikelihoodErrorLogMessage > MetadataXml.LogIgnoreTimeSpand)
			{
				EnvironmentManager.Instance.Log(GetType().FullName, error: false, "ReadXml", message, null);
				_lastLikelihoodErrorLogMessage = DateTime.UtcNow;
			}
		}
	}

	private void LogTypeElementError(string message)
	{
		lock (Lock)
		{
			if (DateTime.UtcNow - _lastTypeErrorLogMessage > MetadataXml.LogIgnoreTimeSpand)
			{
				EnvironmentManager.Instance.Log(GetType().FullName, error: false, "ReadXml", message, null);
				_lastTypeErrorLogMessage = DateTime.UtcNow;
			}
		}
	}

	public bool Equals(ClassCandidate other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (string.Equals(Type, other.Type))
		{
			return Likelihood.Equals(other.Likelihood);
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
		return Equals((ClassCandidate)obj);
	}

	public override int GetHashCode()
	{
		return (((Type != null) ? Type.GetHashCode() : 0) * 397) ^ Likelihood.GetHashCode();
	}
}
