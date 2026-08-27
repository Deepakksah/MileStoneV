using System;
using System.Collections.Generic;
using System.Xml;
using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK;

public sealed class Ptz
{
	private IPtzInfo _ptzInfo = new PtzInfo();

	private List<Capability> _capabilityList;

	private bool _browsable;

	private object _lockObject = new object();

	public bool Browsable => _browsable;

	public List<Capability> CapabilityList
	{
		get
		{
			if (_capabilityList == null)
			{
				lock (_lockObject)
				{
					if (_capabilityList == null)
					{
						_capabilityList = new List<Capability>();
						if (_ptzInfo != null && _ptzInfo.Capabilities != null)
						{
							ICapabilityInfo[] capabilities = _ptzInfo.Capabilities;
							foreach (ICapabilityInfo capabilityInfo in capabilities)
							{
								_capabilityList.Add(new Capability(capabilityInfo));
							}
						}
					}
				}
			}
			return _capabilityList;
		}
	}

	public bool PtzEnabled => _ptzInfo.PtzEnabled;

	public bool IsCenterOnPositionInViewSupported => _ptzInfo.IsCenterOnPositionInViewSupported;

	public bool IsPtzCenterAndZoomToRectangleSupported => _ptzInfo.IsPtzCenterAndZoomToRectangleSupported;

	public bool IsPtzHomeSupported => _ptzInfo.IsPtzHomeSupported;

	public bool IsPtzDiagonalSupported => _ptzInfo.IsPtzDiagonalSupported;

	public Capability GetCapability(string capabilityName)
	{
		foreach (Capability capability in CapabilityList)
		{
			if (capability.Name.Equals(capabilityName, StringComparison.InvariantCultureIgnoreCase))
			{
				return capability;
			}
		}
		return null;
	}

	public Ptz(IPtzInfo ptzInfo)
	{
		_ptzInfo = ptzInfo;
	}

	public Ptz(XmlElement cameraNode)
	{
		bool speedSupport = false;
		_ptzInfo = new PtzInfo();
		List<CapabilityInfo> list = new List<CapabilityInfo>();
		if (cameraNode["ptz"] != null)
		{
			_ptzInfo.IsPtzHomeSupported = true;
			XmlElement xmlElement = cameraNode["ptz"]["ptzjoystick"];
			if (xmlElement != null)
			{
				speedSupport = xmlElement.InnerText.Equals("yes", StringComparison.InvariantCultureIgnoreCase);
				_ptzInfo.IsPtzDiagonalSupported = true;
			}
			XmlElement xmlElement2 = cameraNode["ptz"]["ptzrectangle"];
			if (xmlElement2 != null)
			{
				_ptzInfo.IsPtzCenterAndZoomToRectangleSupported = xmlElement2.InnerText.Equals("yes", StringComparison.InvariantCultureIgnoreCase);
			}
			XmlElement xmlElement3 = cameraNode["ptz"]["ptzcenter"];
			if (xmlElement3 != null)
			{
				_ptzInfo.IsCenterOnPositionInViewSupported = xmlElement3.InnerText.Equals("yes", StringComparison.InvariantCultureIgnoreCase);
			}
			_ = cameraNode["ptz"]["editpreset"];
			XmlElement xmlElement4 = cameraNode["ptz"]["pan"];
			if (xmlElement4 != null && !xmlElement4.InnerText.Equals("none", StringComparison.InvariantCultureIgnoreCase) && !xmlElement4.InnerText.Equals("ipix", StringComparison.InvariantCultureIgnoreCase))
			{
				CapabilityInfo capabilityInfo = new CapabilityInfo
				{
					Name = "pan"
				};
				ReadSupportTypes(capabilityInfo, xmlElement4.InnerText, speedSupport);
				list.Add(capabilityInfo);
			}
			XmlElement xmlElement5 = cameraNode["ptz"]["tilt"];
			if (xmlElement5 != null && !xmlElement5.InnerText.Equals("none", StringComparison.InvariantCultureIgnoreCase) && !xmlElement5.InnerText.Equals("ipix", StringComparison.InvariantCultureIgnoreCase))
			{
				CapabilityInfo capabilityInfo2 = new CapabilityInfo
				{
					Name = "tilt"
				};
				ReadSupportTypes(capabilityInfo2, xmlElement5.InnerText, speedSupport);
				list.Add(capabilityInfo2);
			}
			XmlElement xmlElement6 = cameraNode["ptz"]["zoom"];
			if (xmlElement6 != null && !xmlElement6.InnerText.Equals("none", StringComparison.InvariantCultureIgnoreCase) && !xmlElement6.InnerText.Equals("ipix", StringComparison.InvariantCultureIgnoreCase))
			{
				CapabilityInfo capabilityInfo3 = new CapabilityInfo
				{
					Name = "zoom"
				};
				ReadSupportTypes(capabilityInfo3, xmlElement6.InnerText, speedSupport);
				list.Add(capabilityInfo3);
			}
		}
		if (cameraNode["browsable"] != null)
		{
			XmlElement xmlElement7 = cameraNode["browsable"]["streamable"];
			if (xmlElement7 != null)
			{
				_browsable = xmlElement7.InnerText.Equals("yes", StringComparison.InvariantCultureIgnoreCase);
			}
		}
		if (list.Count > 0 || _ptzInfo.IsPtzCenterAndZoomToRectangleSupported || _ptzInfo.IsCenterOnPositionInViewSupported)
		{
			_ptzInfo.PtzEnabled = true;
		}
		IPtzInfo ptzInfo = _ptzInfo;
		ICapabilityInfo[] capabilities = list.ToArray();
		ptzInfo.Capabilities = capabilities;
	}

	private static void ReadSupportTypes(CapabilityInfo capabilityInfo, string type, bool speedSupport)
	{
		capabilityInfo.Absolute = false;
		capabilityInfo.Relative = false;
		capabilityInfo.Start = false;
		capabilityInfo.Stop = false;
		capabilityInfo.Speed = false;
		capabilityInfo.Automatic = false;
		if (!(type == "absolute"))
		{
			if (type == "relative")
			{
				capabilityInfo.Relative = true;
			}
		}
		else
		{
			capabilityInfo.Absolute = true;
		}
	}
}
