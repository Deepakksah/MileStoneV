using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Management.Common;

public class PatrollingSchema
{
	private class EdgeOrderComparer : IComparer
	{
		int IComparer.Compare(object x, object y)
		{
			return new CaseInsensitiveComparer(CultureInfo.InvariantCulture).Compare(((PatrollingEdge)x).Order, ((PatrollingEdge)y).Order);
		}
	}

	private Guid _id;

	private Guid _deviceId;

	private bool _hasEndPosition;

	private bool _showCustomizeTransitions;

	private PatrollingTransition _initialTransition = new PatrollingTransition();

	private PatrollingEdge _endPosition = new PatrollingEdge();

	private ArrayList _patrollingEdges = new ArrayList();

	public PatrollingTransition InitialTransition
	{
		get
		{
			return _initialTransition;
		}
		set
		{
			_initialTransition = value;
		}
	}

	public PatrollingEdge EndPosition
	{
		get
		{
			return _endPosition;
		}
		set
		{
			if (value == null)
			{
				_endPosition = new PatrollingEdge();
				_hasEndPosition = false;
			}
			else
			{
				_endPosition = value;
				_hasEndPosition = true;
			}
		}
	}

	public ArrayList Edges
	{
		get
		{
			return _patrollingEdges;
		}
		set
		{
			_patrollingEdges = value;
		}
	}

	public Guid Id => _id;

	public Guid DeviceId
	{
		get
		{
			return _deviceId;
		}
		set
		{
			_deviceId = value;
		}
	}

	public bool HasEndPosition => _hasEndPosition;

	public bool ShowCustomizeTransitions
	{
		get
		{
			return _showCustomizeTransitions;
		}
		set
		{
			_showCustomizeTransitions = value;
		}
	}

	private void RecalculateOrder()
	{
		_patrollingEdges.Sort(new EdgeOrderComparer());
		for (int i = 0; i < _patrollingEdges.Count; i++)
		{
			((PatrollingEdge)_patrollingEdges[i]).Order = i;
		}
	}

	public PatrollingSchema(Guid id)
	{
		_id = id;
	}

	public void LoadXml(string profileXml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(profileXml);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("/PatrollingSchema/ShowCustomizeTransitions");
		if (xmlNode != null)
		{
			_showCustomizeTransitions = Convert.ToBoolean(xmlNode.InnerText, CultureInfo.InvariantCulture);
		}
		_initialTransition.LoadXml(xmlDocument.SelectSingleNode("/PatrollingSchema/InitialTransition"));
		_hasEndPosition = _endPosition.LoadXml(xmlDocument.SelectSingleNode("/PatrollingSchema/EndPosition"));
		foreach (XmlNode item in xmlDocument.SelectNodes("/PatrollingSchema/Path/Edge"))
		{
			PatrollingEdge patrollingEdge = new PatrollingEdge();
			patrollingEdge.LoadXml(item);
			_patrollingEdges.Add(patrollingEdge);
		}
		_patrollingEdges.Sort(new EdgeOrderComparer());
	}

	public string ToXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
		xmlTextWriter.WriteStartElement("PatrollingSchema");
		xmlTextWriter.WriteElementString("ID", _id.ToString().ToUpperInvariant());
		xmlTextWriter.WriteElementString("ShowCustomizeTransitions", _showCustomizeTransitions.ToString());
		xmlTextWriter.WriteRaw(_initialTransition.ToXml(initialTransition: true));
		xmlTextWriter.WriteRaw(_endPosition.ToXml(endPosition: true, _hasEndPosition));
		_patrollingEdges.Sort(new EdgeOrderComparer());
		xmlTextWriter.WriteStartElement("Path");
		foreach (PatrollingEdge patrollingEdge in _patrollingEdges)
		{
			xmlTextWriter.WriteRaw(patrollingEdge.ToXml());
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.Close();
		return stringBuilder.ToString();
	}

	public void RemovePreset(string presetKey)
	{
		if (HasEndPosition && EndPosition.StartPosition.Preset.Key == presetKey)
		{
			EndPosition = null;
		}
		for (int num = Edges.Count - 1; num >= 0; num--)
		{
			if (((PatrollingEdge)Edges[num]).StartPosition.Preset.Key == presetKey)
			{
				Edges.RemoveAt(num);
			}
		}
		RecalculateOrder();
	}

	public bool ContainsPreset(string presetKey)
	{
		if (HasEndPosition && EndPosition.StartPosition.Preset.Key == presetKey)
		{
			return true;
		}
		foreach (PatrollingEdge edge in Edges)
		{
			if (edge.StartPosition.Preset.Key == presetKey)
			{
				return true;
			}
		}
		return false;
	}

	public void ChangePresetKey(string oldPresetKey, string newPresetKey)
	{
		if (HasEndPosition && EndPosition.StartPosition.Preset.Key == oldPresetKey)
		{
			EndPosition.StartPosition.Preset.Key = newPresetKey;
		}
		foreach (PatrollingEdge edge in Edges)
		{
			if (edge.StartPosition.Preset.Key == oldPresetKey)
			{
				edge.StartPosition.Preset.Key = newPresetKey;
			}
		}
	}

	public void Validate()
	{
		if (_initialTransition != null)
		{
			_initialTransition.Validate();
		}
		if (_endPosition != null)
		{
			_endPosition.Validate();
		}
		if (_patrollingEdges != null)
		{
			for (int i = 0; i < _patrollingEdges.Count; i++)
			{
				((PatrollingEdge)_patrollingEdges[i]).Validate();
			}
		}
	}
}
