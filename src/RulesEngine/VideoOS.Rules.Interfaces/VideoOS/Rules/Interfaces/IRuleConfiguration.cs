using System;
using System.Collections.Generic;

namespace VideoOS.Rules.Interfaces;

public interface IRuleConfiguration
{
	bool InitialConfiguration { get; }

	IEnumerable<(RuleModification modification, Guid eventTypeDeleteId, string xmlDefinition)> EventTypesChanges { get; }

	IEnumerable<(RuleModification modification, Guid deviceGroupId, Guid parentId)> DeviceGroupChanges { get; }

	IEnumerable<(RuleModification modification, Guid deviceGroupId, Guid deviceId)> DeviceGroupMemberChanges { get; }

	IEnumerable<(RuleModification modification, Guid serverGroupId, Guid parentId)> ServerGroupChanges { get; }

	IEnumerable<(RuleModification modification, Guid serverGroupId, Guid serverId)> ServerGroupMembershipChanges { get; }

	IEnumerable<(RuleModification modification, Guid recorderId)> RecorderChanges { get; }

	IEnumerable<(RuleModification modification, Guid VariableId, string name, string value, bool reset)> RuleVariablesChanges { get; }

	IEnumerable<(RuleModification modification, Guid ruleId, string name, string xmlDefinition, bool enabled, bool doClearEvent)> RuleChanges { get; }

	IEnumerable<(RuleModification modification, Guid timeProfileId, string timeProfileType, string xmlDefinition)> TimeProfileChanges { get; }

	bool HasAnyChanges { get; }
}
