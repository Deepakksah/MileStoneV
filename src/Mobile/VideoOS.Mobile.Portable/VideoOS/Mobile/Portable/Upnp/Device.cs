using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace VideoOS.Mobile.Portable.Upnp;

public class Device
{
	[XmlElement("deviceType")]
	public string DeviceType { get; set; }

	[XmlElement("friendlyName")]
	public string FriendlyName { get; set; }

	[XmlElement("manufacturer")]
	public string Manufacturer { get; set; }

	[XmlElement("manufacturerURL")]
	public string ManufacturerUrl { get; set; }

	[XmlElement("modelDescription")]
	public string ModelDescription { get; set; }

	[XmlElement("modelName")]
	public string ModelName { get; set; }

	[XmlElement("modelNumber")]
	public string ModelNumber { get; set; }

	[XmlElement("modelURL")]
	public string ModelUrl { get; set; }

	[XmlElement("presentationURL")]
	public string PresentationUrl
	{
		get
		{
			if (PresentationUrlFormat != null)
			{
				PresentationUrlFormat.Host = Address;
				return PresentationUrlFormat.ToString();
			}
			return string.Empty;
		}
		set
		{
			try
			{
				Uri uri = new Uri(value);
				Address = uri.Host;
				PresentationUrlFormat = new UriBuilder(uri.Scheme, "{0}", uri.Port, uri.LocalPath);
			}
			catch (Exception)
			{
			}
		}
	}

	[XmlElement("UDN")]
	public string Udn { get; set; }

	[XmlArray("serviceList")]
	[XmlArrayItem("service")]
	public List<UpnpService> ServiceList { get; set; }

	[XmlIgnore]
	public string Address { get; set; }

	[XmlIgnore]
	public UriBuilder PresentationUrlFormat { get; set; }
}
