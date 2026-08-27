using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using VideoOS.Platform.SDK.Proxy.Status2.Net;

namespace VideoOS.Platform.SDK.StatusClient.StatusEventArgs;

public class EventFiredEventArgs : StatusApiEventArgs
{
	private readonly Guid _sourceId;

	private readonly Guid _eventId;

	private readonly Collection<Guid> _deviceIds;

	private readonly Collection<Guid> _deviceGroupIds;

	private readonly Dictionary<string, string> _otherMetadata;

	public Guid SourceId => _sourceId;

	public Guid EventId => _eventId;

	public Collection<Guid> DeviceIds => _deviceIds;

	public Collection<Guid> DeviceGroupIds => _deviceGroupIds;

	public Dictionary<string, string> Metadata => _otherMetadata;

	public EventFiredEventArgs(DateTime time, Guid sourceId, Guid eventId, KeyValue[] keyValues)
		: base(time)
	{
		_sourceId = sourceId;
		_eventId = eventId;
		_deviceGroupIds = new Collection<Guid>();
		_deviceIds = new Collection<Guid>();
		_otherMetadata = new Dictionary<string, string>();
		if (keyValues == null)
		{
			return;
		}
		foreach (KeyValue keyValue in keyValues)
		{
			if (keyValue.Key == "TargetDevices")
			{
				try
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(keyValue.Value);
					XmlNodeList xmlNodeList = xmlDocument.SelectNodes("targetDevices/deviceIds/deviceId");
					if (xmlNodeList != null)
					{
						foreach (XmlNode item in xmlNodeList)
						{
							_deviceIds.Add(new Guid(item.InnerText));
						}
					}
					xmlNodeList = xmlDocument.SelectNodes("targetDevices/deviceGroupIds/deviceGroupId");
					if (xmlNodeList == null)
					{
						continue;
					}
					foreach (XmlNode item2 in xmlNodeList)
					{
						_deviceGroupIds.Add(new Guid(item2.InnerText));
					}
				}
				catch (Exception ex)
				{
					EnvironmentManager.Instance.Log(error: true, "EventFiredEventArgs", ex.Message);
				}
			}
			else
			{
				_otherMetadata[keyValue.Key] = keyValue.Value;
			}
		}
	}

	public override string GetUniqueKey()
	{
		return base.GetUniqueKey() + _sourceId.ToString() + _eventId.ToString();
	}
}
