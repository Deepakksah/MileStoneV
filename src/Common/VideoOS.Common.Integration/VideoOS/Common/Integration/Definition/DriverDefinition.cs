using System;
using System.Diagnostics;
using System.Globalization;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class DriverDefinition
{
	private driverType driver;

	public MainDefinition MainDefinition { get; private set; }

	public Guid Id
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(driver.id);
		}
	}

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return driver.name;
		}
	}

	public int Number
	{
		[DebuggerStepThrough]
		get
		{
			return Convert.ToInt32(driver.number, CultureInfo.InvariantCulture);
		}
		[DebuggerStepThrough]
		set
		{
			driver.number = value.ToString(CultureInfo.InvariantCulture);
		}
	}

	public string XmlVersion
	{
		[DebuggerStepThrough]
		get
		{
			return driver.versionid;
		}
	}

	public string Version
	{
		[DebuggerStepThrough]
		get
		{
			return driver.version;
		}
	}

	public bool SupportsProxyDistribution
	{
		[DebuggerStepThrough]
		get
		{
			return driver.proxyprocess?.supportsdistribution ?? false;
		}
	}

	public bool AllowTcpCheck
	{
		[DebuggerStepThrough]
		get
		{
			return driver.deviceconnection?.allowtcpcheck ?? true;
		}
	}

	public DriverDefinition(MainDefinition mainDefinition)
	{
		if (mainDefinition == null)
		{
			throw new ArgumentNullException("mainDefinition");
		}
		MainDefinition = mainDefinition;
		driver = mainDefinition.Base.driver;
	}
}
