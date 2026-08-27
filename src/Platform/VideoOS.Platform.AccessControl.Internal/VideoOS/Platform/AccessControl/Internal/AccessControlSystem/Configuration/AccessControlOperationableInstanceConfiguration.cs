using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.AccessControl.Internal.AccessControlElements;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;
using VideoOS.Platform.AccessControl.Internal.Util;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlSystem.Configuration;

public class AccessControlOperationableInstanceConfiguration
{
	private object _lockObject = new object();

	private Dictionary<Guid, bool> _operationableInstanceEnabled = new Dictionary<Guid, bool>();

	public AccessControlOperationableInstanceConfiguration()
	{
	}

	public Dictionary<Guid, bool> GetOperationableInstancesEnabled()
	{
		return _operationableInstanceEnabled;
	}

	public AccessControlOperationableInstanceConfiguration(string serializedString)
	{
		_operationableInstanceEnabled.Clear();
		OperationableInstanceConfigurationSerializeData operationableInstanceConfigurationSerializeData = OperationableInstanceConfigurationSerializeData.Deserialize(serializedString);
		if (operationableInstanceConfigurationSerializeData == null)
		{
			return;
		}
		foreach (OperationableInstanceConfigurationSerializeData.OperationableInstanceConfigurationData item in operationableInstanceConfigurationSerializeData.OperationableInstanceConfiguration)
		{
			_operationableInstanceEnabled.Add(item.AccessControlOperationableInstanceId, item.Enabled);
		}
	}

	public OperationableInstanceConfigurationSerializeData SerializeData()
	{
		lock (_lockObject)
		{
			OperationableInstanceConfigurationSerializeData operationableInstanceConfigurationSerializeData = new OperationableInstanceConfigurationSerializeData();
			foreach (KeyValuePair<Guid, bool> item in _operationableInstanceEnabled)
			{
				operationableInstanceConfigurationSerializeData.OperationableInstanceConfiguration.Add(new OperationableInstanceConfigurationSerializeData.OperationableInstanceConfigurationData
				{
					AccessControlOperationableInstanceId = item.Key,
					Enabled = item.Value
				});
			}
			return operationableInstanceConfigurationSerializeData;
		}
	}

	public bool GetOperationableInstanceEnabled(AccessControlOperationableInstance operationableInstance)
	{
		lock (_lockObject)
		{
			if (_operationableInstanceEnabled.TryGetValue(operationableInstance.Id, out var enabled))
			{
				return enabled;
			}
			if (!(operationableInstance is AccessControlUnit accessControlUnit))
			{
				return true;
			}
			AccessControlUnit doorAncestorUnit = DoorHelper.GetDoorAncestorUnit(accessControlUnit);
			if (doorAncestorUnit != null)
			{
				if (_operationableInstanceEnabled.TryGetValue(doorAncestorUnit.Id, out enabled))
				{
					return enabled;
				}
				SetOperationableInstanceEnabled(doorAncestorUnit, enabled: true);
				return true;
			}
			IEnumerable<AccessControlUnit> descendantDoorUnits = DoorHelper.GetDescendantDoorUnits(accessControlUnit);
			if (descendantDoorUnits.Any())
			{
				return descendantDoorUnits.Any((AccessControlUnit d) => !_operationableInstanceEnabled.TryGetValue(d.Id, out enabled) || enabled);
			}
			return true;
		}
	}

	public void SetOperationableInstanceEnabled(AccessControlOperationableInstance operationableInstance, bool enabled)
	{
		lock (_lockObject)
		{
			_operationableInstanceEnabled[operationableInstance.Id] = enabled;
		}
	}
}
