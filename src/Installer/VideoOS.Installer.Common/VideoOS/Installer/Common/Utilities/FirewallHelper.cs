using System;
using NetFwTypeLib;

namespace VideoOS.Installer.Common.Utilities;

public static class FirewallHelper
{
	public static void CreateRule(string name, int[] ports)
	{
		try
		{
			INetFwRule netFwRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
			netFwRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
			netFwRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
			netFwRule.Enabled = true;
			netFwRule.Name = name;
			netFwRule.Protocol = 6;
			netFwRule.LocalPorts = string.Join(",", ports);
			((INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"))).Rules.Add(netFwRule);
		}
		catch (Exception innerException)
		{
			throw new FirewallException("Error occurs during creating firewall rule.", innerException);
		}
	}

	public static void CreateInboundRule(string path, string name, int[] ports, bool allowAll = false, Protocol protocol = Protocol.Any, bool enabled = true)
	{
		try
		{
			if (ports.Length == 0 && !allowAll)
			{
				throw new FirewallException("No ports specified.");
			}
			if (ports.Length != 0 && protocol == Protocol.Any)
			{
				throw new FirewallException("Ports specified with Protocol.Any. This is not a valid configuration.");
			}
			INetFwRule netFwRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
			netFwRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
			netFwRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
			netFwRule.Name = name;
			netFwRule.ApplicationName = path;
			netFwRule.Protocol = (int)protocol;
			if (protocol != Protocol.Any)
			{
				netFwRule.LocalPorts = ((ports.Length != 0) ? string.Join(",", ports) : "*");
			}
			netFwRule.Enabled = enabled;
			((INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"))).Rules.Add(netFwRule);
		}
		catch (Exception innerException)
		{
			throw new FirewallException("Error occurred during creating inbound firewall rule.", innerException);
		}
	}

	public static void CreateOutboundRule(string path, string name, int[] ports, bool allowAll = false, Protocol protocol = Protocol.Any, bool enabled = true)
	{
		try
		{
			if (ports.Length == 0 && !allowAll)
			{
				throw new FirewallException("No ports specified.");
			}
			if (ports.Length != 0 && protocol == Protocol.Any)
			{
				throw new FirewallException("Ports specified with Protocol.Any. This is not a valid configuration.");
			}
			INetFwRule netFwRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
			netFwRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
			netFwRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
			netFwRule.Name = name;
			netFwRule.ApplicationName = path;
			netFwRule.Protocol = (int)protocol;
			if (protocol != Protocol.Any)
			{
				netFwRule.LocalPorts = ((ports.Length != 0) ? string.Join(",", ports) : "*");
			}
			netFwRule.Enabled = enabled;
			((INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"))).Rules.Add(netFwRule);
		}
		catch (Exception innerException)
		{
			throw new FirewallException("Error occurred during creating outbound firewall rule.", innerException);
		}
	}

	public static void RemoveRule(string name)
	{
		try
		{
			((INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"))).Rules.Remove(name);
		}
		catch (Exception innerException)
		{
			throw new FirewallException("Error occurs during removing firewall rule.", innerException);
		}
	}
}
