using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACPropertyDefinition
{
	public string DisplayName { get; private set; }

	public string Key { get; private set; }

	public string DefaultValue { get; private set; }

	public int Importance { get; private set; }

	public bool RequireReconnect { get; private set; }

	public string ToolTip { get; private set; }

	public string ValueType { get; private set; }

	public IEnumerable<ACProperty> ValueTypeProperties { get; private set; }

	public ACPropertyDefinition(string key, string displayName, string defaultValue, int importance, bool requireReconnect, string toolTip, string valueType, IEnumerable<ACProperty> valueTypeProperties)
	{
		DisplayName = displayName;
		Key = key;
		DefaultValue = defaultValue;
		Importance = importance;
		RequireReconnect = requireReconnect;
		ToolTip = toolTip;
		ValueType = valueType;
		ValueTypeProperties = new ReadOnlyCollection<ACProperty>((valueTypeProperties != null) ? valueTypeProperties.ToList() : new List<ACProperty>());
	}
}
