using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace VideoOS.Common.Drivers;

public interface IPTZArgs
{
	[XmlElement(typeof(PTZPairDouble))]
	ArrayList movement { get; }

	[XmlElement(typeof(PTZPairDouble))]
	ArrayList speed { get; }

	[XmlElement(typeof(PTZPairBoolean))]
	ArrayList automatic { get; }

	bool isPanAvailable { get; }

	bool isTiltAvailable { get; }

	bool isZoomAvailable { get; }

	bool isPanSpeedAvailable { get; }

	bool isTiltSpeedAvailable { get; }

	bool isZoomSpeedAvailable { get; }

	[XmlIgnore]
	double Pan { get; set; }

	[XmlIgnore]
	double Tilt { get; set; }

	[XmlIgnore]
	double Zoom { get; set; }

	[XmlIgnore]
	double PanSpeed { get; set; }

	[XmlIgnore]
	double TiltSpeed { get; set; }

	[XmlIgnore]
	double ZoomSpeed { get; set; }

	new string ToString();

	void LoadXml(string Xml);

	void LoadXmlFragment(XmlTextReader xtr);

	string SaveAsXml();

	void SaveAsXml(XmlTextWriter wr);

	string SaveAsXMLFragment(string primaryTag);

	bool isAutoDefined(string name);

	bool isAutoDefined(Guid guid);

	bool GetAuto(string name);

	void SetAuto(string name, bool value);

	void RemoveAuto(string name);

	void SetAuto(Guid guid, bool value);

	void SetMovement(string name, double value);

	void SetSpeed(string name, double value);

	bool isAvailable(string name);

	bool isSpeedAvailable(string name);

	double GetMovement(string name);

	double GetSpeed(string name);
}
