using System.Globalization;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Rules;
using VideoOS.Rules.Interfaces;
using VideoOS.Server.Properties;

namespace VideoOS.Server.RuleActions;

public sealed class SendSNMPTrapRuleAction : RuleActionBase
{
	private Server _server;

	public const string IntegerTypeVarBinding = "<variableBinding><name>{0}</name><value type=\"{1}\"><integer>{2}</integer></value></variableBinding>";

	public const string StringTypeVarBinding = "<variableBinding><name>{0}</name><value type=\"{1}\"><string>{2}</string></value></variableBinding>";

	public const string TrapXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><snmpTrap><genericTrapId>SNMP_GENERICTRAP_ENTERSPECIFIC</genericTrapId><specificTrapId>{0}</specificTrapId><variableBindingsList>{1}</variableBindingsList></snmpTrap>";

	public SendSNMPTrapRuleAction(IRuleEngine ruleEngine, Server server)
		: base(ruleEngine)
	{
		_server = server;
	}

	public override void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
		if (triggerEvent == null || triggerEvent.Definition == null || string.IsNullOrEmpty(triggerEvent.Definition.Name))
		{
			return;
		}
		string serverSNMPId = BuiltInEventTypes.GetServerSNMPId(triggerEvent.EventTypeId);
		if (!string.IsNullOrEmpty(serverSNMPId))
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			string empty3 = string.Empty;
			string empty4 = string.Empty;
			SourceType sourceType = _server.ConfigurationLookup.FindEventSourceTypeFromEventTypeID(triggerEvent.EventTypeId);
			empty = _server.ConfigurationLookup.FindSourceDisplayName(triggerEvent.SourceId, sourceType);
			empty2 = string.Format(CultureInfo.InvariantCulture, Resources.SNMPSourceType, sourceType);
			if (!string.IsNullOrEmpty(empty))
			{
				empty2 = empty2 + "; " + string.Format(CultureInfo.InvariantCulture, Resources.SNMPSourceName, empty);
			}
			if (empty2.Length > 1024)
			{
				empty2 = empty2.Remove(1021);
				empty2 += "...";
			}
			empty3 = string.Format(CultureInfo.InvariantCulture, "<variableBinding><name>{0}</name><value type=\"{1}\"><integer>{2}</integer></value></variableBinding>", ServerConfig.Instance.Snmp.OidPrefix + ".1.2.4.1.1", "ASN_INTEGER", 0) + string.Format(CultureInfo.InvariantCulture, "<variableBinding><name>{0}</name><value type=\"{1}\"><string>{2}</string></value></variableBinding>", ServerConfig.Instance.Snmp.OidPrefix + ".1.2.2", "ASN_OCTETSTRING", empty2);
			empty4 = string.Format(CultureInfo.InvariantCulture, "<?xml version=\"1.0\" encoding=\"utf-8\"?><snmpTrap><genericTrapId>SNMP_GENERICTRAP_ENTERSPECIFIC</genericTrapId><specificTrapId>{0}</specificTrapId><variableBindingsList>{1}</variableBindingsList></snmpTrap>", serverSNMPId, empty3);
			if (_server.SNMPManager != null)
			{
				_server.SNMPManager.AddSNMPTrap(empty4);
			}
		}
	}
}
