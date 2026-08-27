using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Management.Common;

public class PatrollingEdge
{
	private int _order;

	private PatrollingPosition _startPosition = new PatrollingPosition();

	private PatrollingTransition _transition = new PatrollingTransition();

	public int Order
	{
		get
		{
			return _order;
		}
		set
		{
			_order = value;
		}
	}

	public PatrollingPosition StartPosition => _startPosition;

	public PatrollingTransition Transition => _transition;

	public bool LoadXml(XmlNode edgeNode)
	{
		bool result = false;
		if (edgeNode.Attributes.Count > 0)
		{
			if (edgeNode.Attributes[0].Name == "Order")
			{
				_order = Convert.ToInt32(edgeNode.Attributes["Order"].InnerText, CultureInfo.InvariantCulture);
			}
			else if (edgeNode.Attributes[0].Name == "Enabled")
			{
				result = Convert.ToBoolean(edgeNode.Attributes["Enabled"].InnerText, CultureInfo.InvariantCulture);
			}
		}
		_startPosition.LoadXml(edgeNode.SelectSingleNode("StartPosition"));
		_transition.LoadXml(edgeNode.SelectSingleNode("Transition"));
		return result;
	}

	public string ToXml()
	{
		return ToXml(endPosition: false, enabled: false);
	}

	public string ToXml(bool endPosition, bool enabled)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
		if (endPosition)
		{
			xmlTextWriter.WriteStartElement("EndPosition");
			xmlTextWriter.WriteAttributeString("Enabled", enabled.ToString(CultureInfo.InvariantCulture));
		}
		else
		{
			xmlTextWriter.WriteStartElement("Edge");
			xmlTextWriter.WriteAttributeString("Order", _order.ToString(CultureInfo.InvariantCulture));
		}
		xmlTextWriter.WriteRaw(_startPosition.ToXml());
		xmlTextWriter.WriteRaw(_transition.ToXml(initialTransition: false));
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.Close();
		return stringBuilder.ToString();
	}

	public void Validate()
	{
		if (_startPosition != null)
		{
			_startPosition.Validate();
		}
		if (_transition != null)
		{
			_transition.Validate();
		}
	}
}
