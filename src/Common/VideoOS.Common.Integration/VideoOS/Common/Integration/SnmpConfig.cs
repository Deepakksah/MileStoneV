using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Common.Integration;

public class SnmpConfig
{
	private string _oidPrefix = ".1.3.6.1.4.1.24149";

	private string _trapGroupOid = ".1.3.6.1.4.1.24149.1.2.1";

	private int _remotingPort = 6475;

	private string _snmpTrapNamedEvent = "VideoOSSNMPTrapEventSrv";

	public string OidPrefix
	{
		[DebuggerStepThrough]
		get
		{
			return _oidPrefix;
		}
	}

	public string TrapGroupOid
	{
		[DebuggerStepThrough]
		get
		{
			return _trapGroupOid;
		}
	}

	public int RemotingPort
	{
		[DebuggerStepThrough]
		get
		{
			return _remotingPort;
		}
		[DebuggerStepThrough]
		set
		{
			_remotingPort = value;
		}
	}

	public string SnmpTrapNamedEvent
	{
		[DebuggerStepThrough]
		get
		{
			return _snmpTrapNamedEvent;
		}
	}

	private SnmpConfig(string configName)
	{
		LoadXML(configName);
	}

	public static SnmpConfig ServerSnmpConfig()
	{
		return new SnmpConfig("SNMPAGNTSRV.dll.config");
	}

	public static SnmpConfig RecorderSnmpConfig()
	{
		return new SnmpConfig("SNMPAGNT.dll.config");
	}

	private void LoadXML(string configName)
	{
		XmlDocument xmlDocument = null;
		try
		{
			xmlDocument = new XmlDocument();
			XmlTextReader xmlTextReader = null;
			try
			{
				xmlTextReader = new XmlTextReader(Path.Combine(OemResources.Instance.ResourcePackageDirectory, configName));
				xmlDocument.Load(xmlTextReader);
			}
			finally
			{
				xmlTextReader?.Close();
			}
			XmlNode xmlNode = xmlDocument["configuration"];
			if (xmlNode == null)
			{
				throw new InvalidConfigurationException("'configuration' node missing in snmp config");
			}
			ParseOidPrefix(xmlNode["oidPrefix"]);
			ParseTrapGroupOid(xmlNode["trapGroupOid"]);
			ParseRremotingPort(xmlNode["remotingPort"]);
			ParseSnmpTrapNamedEvent(xmlNode["snmpTrapNamedEvent"]);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void ParseOidPrefix(XmlElement oidPrefix)
	{
		if (oidPrefix != null)
		{
			_oidPrefix = oidPrefix.InnerText;
		}
	}

	private void ParseTrapGroupOid(XmlElement trapOidPrefix)
	{
		if (trapOidPrefix != null)
		{
			_trapGroupOid = trapOidPrefix.InnerText;
		}
	}

	private void ParseRremotingPort(XmlElement remotingPort)
	{
		if (remotingPort != null)
		{
			_remotingPort = Convert.ToInt32(remotingPort.InnerText, CultureInfo.InvariantCulture);
		}
	}

	private void ParseSnmpTrapNamedEvent(XmlElement snmpTrapNamedEvent)
	{
		if (snmpTrapNamedEvent != null)
		{
			_snmpTrapNamedEvent = snmpTrapNamedEvent.InnerText;
		}
	}
}
