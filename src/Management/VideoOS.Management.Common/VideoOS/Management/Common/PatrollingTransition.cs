using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Management.Common;

public class PatrollingTransition
{
	private TimeSpan _expectedTransitionTime;

	private bool _isPanSpeedAvailable;

	private double _panSpeed;

	private bool _isTiltSpeedAvailable;

	private double _tiltSpeed;

	private bool _isZoomSpeedAvailable;

	private double _zoomSpeed;

	public TimeSpan ExpectedTransitionTime
	{
		get
		{
			return _expectedTransitionTime;
		}
		set
		{
			_expectedTransitionTime = value;
		}
	}

	public bool IsPanSpeedAvailable
	{
		get
		{
			return _isPanSpeedAvailable;
		}
		set
		{
			_isPanSpeedAvailable = value;
		}
	}

	public bool IsTiltSpeedAvailable
	{
		get
		{
			return _isTiltSpeedAvailable;
		}
		set
		{
			_isTiltSpeedAvailable = value;
		}
	}

	public bool IsZoomSpeedAvailable
	{
		get
		{
			return _isZoomSpeedAvailable;
		}
		set
		{
			_isZoomSpeedAvailable = value;
		}
	}

	public double PanSpeed
	{
		get
		{
			if (!_isPanSpeedAvailable)
			{
				return double.NaN;
			}
			return _panSpeed;
		}
		set
		{
			_panSpeed = value;
			_isPanSpeedAvailable = true;
		}
	}

	public double TiltSpeed
	{
		get
		{
			if (!_isTiltSpeedAvailable)
			{
				return double.NaN;
			}
			return _tiltSpeed;
		}
		set
		{
			_tiltSpeed = value;
			_isTiltSpeedAvailable = true;
		}
	}

	public double ZoomSpeed
	{
		get
		{
			if (!_isZoomSpeedAvailable)
			{
				return double.NaN;
			}
			return _zoomSpeed;
		}
		set
		{
			_zoomSpeed = value;
			_isZoomSpeedAvailable = true;
		}
	}

	public void LoadXml(XmlNode transitionNode)
	{
		_expectedTransitionTime = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(transitionNode.SelectSingleNode("ExpectedTransitionTime").InnerText, CultureInfo.InvariantCulture));
		XmlNode xmlNode = transitionNode.SelectSingleNode("PanSpeed");
		if (xmlNode == null)
		{
			_isPanSpeedAvailable = false;
		}
		else
		{
			_isPanSpeedAvailable = true;
			_panSpeed = Convert.ToDouble(xmlNode.InnerText, CultureInfo.InvariantCulture);
		}
		XmlNode xmlNode2 = transitionNode.SelectSingleNode("TiltSpeed");
		if (xmlNode2 == null)
		{
			_isTiltSpeedAvailable = false;
		}
		else
		{
			_isTiltSpeedAvailable = true;
			_tiltSpeed = Convert.ToDouble(xmlNode2.InnerText, CultureInfo.InvariantCulture);
		}
		XmlNode xmlNode3 = transitionNode.SelectSingleNode("ZoomSpeed");
		if (xmlNode3 == null)
		{
			_isZoomSpeedAvailable = false;
			return;
		}
		_isZoomSpeedAvailable = true;
		_zoomSpeed = Convert.ToDouble(xmlNode3.InnerText, CultureInfo.InvariantCulture);
	}

	public string ToXml(bool initialTransition)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
		if (initialTransition)
		{
			xmlTextWriter.WriteStartElement("InitialTransition");
		}
		else
		{
			xmlTextWriter.WriteStartElement("Transition");
		}
		xmlTextWriter.WriteElementString("ExpectedTransitionTime", _expectedTransitionTime.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
		if (_isPanSpeedAvailable)
		{
			xmlTextWriter.WriteElementString("PanSpeed", _panSpeed.ToString(CultureInfo.InvariantCulture));
		}
		if (_isTiltSpeedAvailable)
		{
			xmlTextWriter.WriteElementString("TiltSpeed", _tiltSpeed.ToString(CultureInfo.InvariantCulture));
		}
		if (_isZoomSpeedAvailable)
		{
			xmlTextWriter.WriteElementString("ZoomSpeed", _zoomSpeed.ToString(CultureInfo.InvariantCulture));
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.Close();
		return stringBuilder.ToString();
	}

	public void Validate()
	{
		if (_panSpeed < 0.0 || _panSpeed > 1.0)
		{
			throw new ArgumentOutOfRangeException("PanSpeed", "Pan speed can't be less than zero or greater than 1");
		}
		if (_zoomSpeed < 0.0 || _zoomSpeed > 1.0)
		{
			throw new ArgumentOutOfRangeException("ZoomSpeed", "Zoom speed can't be less than zero or greater than 1");
		}
		if (_tiltSpeed < 0.0 || _tiltSpeed > 1.0)
		{
			throw new ArgumentOutOfRangeException("TiltSpeed", "Tilt speed can't be less than zero or greater than 1");
		}
		if (_expectedTransitionTime < TimeSpan.Zero)
		{
			throw new FormatException("Expected transition time can't be less than zero");
		}
	}
}
