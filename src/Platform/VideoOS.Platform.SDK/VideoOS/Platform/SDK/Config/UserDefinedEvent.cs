#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class UserDefinedEvent : IDevice
{
	private ISystemInfo _systemInfo;

	private IEventTypeInfo _eventTypeInfo;

	private string _eventType;

	private Guid _cameraGuid;

	private bool _isCameraEvent;

	public string Name { get; set; }

	public IRecorder Recorder { get; set; }

	public Guid Guid { get; set; }

	public bool Disabled { get; set; }

	public string DBExportName => null;

	public PositioningInformation PositioningInformation => null;

	public string FullName
	{
		get
		{
			if (_isCameraEvent && _systemInfo.Cameras.ContainsKey(_cameraGuid))
			{
				ICamera camera = _systemInfo.Cameras[_cameraGuid];
				if (camera != null)
				{
					return camera.Name + "." + Name;
				}
			}
			return Name;
		}
	}

	public string EventType => _eventType;

	public UserDefinedEvent(ISystemInfo systemInfo, IRecorder recorder, IEventTypeInfo eventTypeInfo)
	{
		_systemInfo = systemInfo;
		_eventTypeInfo = eventTypeInfo;
		Guid = _eventTypeInfo.EventTypeId;
		Name = _eventTypeInfo.Name;
		_eventType = "Manual";
		_cameraGuid = Guid.Empty;
		_isCameraEvent = false;
		Recorder = recorder;
	}

	public void Trigger()
	{
		try
		{
			_systemInfo.LoginServer.EventTrigger(Guid);
		}
		catch (Exception ex)
		{
			Trace.WriteLine(ex.GetType()?.ToString() + ": " + ex.Message + "\n" + ex.StackTrace);
		}
	}

	public void Trigger(ICamera camera)
	{
		Trigger(new string[1] { camera.Id }, null);
	}

	public void Trigger(Guid cameraId)
	{
		Trigger(new string[1] { cameraId.ToString() }, null);
	}

	public void Trigger(List<Guid> cameraIdList)
	{
		string[] array = new string[cameraIdList.Count];
		for (int i = 0; i < cameraIdList.Count; i++)
		{
			array[i] = cameraIdList[i].ToString();
		}
		Trigger(array, null);
	}

	public void Trigger(GroupInfo deviceGroupInfo)
	{
		Trigger(null, new string[1] { deviceGroupInfo.GroupId.ToString() });
	}

	public void Trigger(string device, string deviceGroup)
	{
		Trigger(new string[1] { device }, new string[1] { deviceGroup });
	}

	public void Trigger(string[] devices, string[] deviceGroups)
	{
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
			xmlTextWriter.WriteStartDocument(standalone: true);
			xmlTextWriter.WriteStartElement("rootUnused");
			xmlTextWriter.WriteStartElement("targetDevices");
			if (_systemInfo.ServerType == ServerTypes.CORPORATE)
			{
				xmlTextWriter.WriteStartElement("deviceIds");
			}
			else
			{
				xmlTextWriter.WriteStartElement("devicesIds");
			}
			if (devices != null)
			{
				string[] array = devices;
				foreach (string text in array)
				{
					xmlTextWriter.WriteElementString("deviceId", text.ToUpper());
				}
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("deviceGroupIds");
			if (deviceGroups != null)
			{
				string[] array = deviceGroups;
				foreach (string value in array)
				{
					xmlTextWriter.WriteElementString("deviceGroupId", value);
				}
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndDocument();
			xmlTextWriter.Flush();
			memoryStream.Position = 0L;
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(memoryStream);
			KeyValue[] array2 = new KeyValue[1]
			{
				new KeyValue()
			};
			array2[0].Key = "TargetDevices";
			array2[0].Value = xmlDocument.DocumentElement.InnerXml;
			ILoginServer loginServer = _systemInfo.LoginServer;
			Guid guid = Guid;
			IKeyValue[] metadata = array2;
			loginServer.EventTriggerWithMetadata(guid, metadata);
		}
		catch (Exception ex)
		{
			Trace.WriteLine(ex.GetType()?.ToString() + ": " + ex.Message + "\n" + ex.StackTrace);
		}
	}
}
