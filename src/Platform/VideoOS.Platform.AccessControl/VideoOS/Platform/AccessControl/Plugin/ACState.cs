using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACState
{
	public string OperationableInstanceId { get; private set; }

	public IEnumerable<string> StateTypeIds { get; private set; }

	public string IconKey { get; private set; }

	public IEnumerable<ACProperty> Properties { get; private set; }

	public ACState(string operationableInstanceId, IEnumerable<string> stateTypeIds, string iconKey, IEnumerable<ACProperty> properties)
	{
		OperationableInstanceId = operationableInstanceId;
		StateTypeIds = new ReadOnlyCollection<string>((stateTypeIds != null) ? stateTypeIds.ToList() : new List<string>());
		IconKey = iconKey ?? string.Empty;
		Properties = new ReadOnlyCollection<ACProperty>((properties != null) ? properties.ToList() : new List<ACProperty>());
	}

	public override string ToString()
	{
		return $"OperationableInstanceId: {OperationableInstanceId}, StateTypeIds: {StateTypeIds.ToArray()}, IconKey: {IconKey}";
	}
}
