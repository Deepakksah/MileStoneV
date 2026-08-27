using System;

namespace VideoOS.Management.Server;

public interface IRuleComponent : IDisposable
{
	void CreateRule(string id, string name, string description, string definition, bool enabled);

	string[] QueryRuleIds();

	Rule[] QueryRulesByIds(string[] ids);

	Rule QueryRuleById(string id);

	void UpdateRule(string id, string name, string description, string definition, bool enabled, Guid sessionId);

	void DeleteRule(string id, Guid sessionId);
}
