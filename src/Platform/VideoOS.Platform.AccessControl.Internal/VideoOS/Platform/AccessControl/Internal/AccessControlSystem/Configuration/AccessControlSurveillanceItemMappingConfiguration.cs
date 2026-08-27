using System;
using System.Collections.Generic;
using VideoOS.Platform.AccessControl.Internal.AccessControlElements;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlSystem.Configuration;

public class AccessControlSurveillanceItemMappingConfiguration
{
	private Dictionary<Guid, List<FQID>> _operationableInstanceIdToSurveillanceItemsDictionary = new Dictionary<Guid, List<FQID>>();

	public AccessControlSurveillanceItemMappingConfiguration()
	{
	}

	public AccessControlSurveillanceItemMappingConfiguration(string serializedString)
	{
		_operationableInstanceIdToSurveillanceItemsDictionary.Clear();
		SurveillanceItemMappingSerializeData surveillanceItemMappingSerializeData = SurveillanceItemMappingSerializeData.Deserialize(serializedString);
		if (surveillanceItemMappingSerializeData == null)
		{
			return;
		}
		foreach (SurveillanceItemMappingSerializeData.SurveillanceItemMappingData surveillanceItemMapping in surveillanceItemMappingSerializeData.SurveillanceItemMappings)
		{
			_operationableInstanceIdToSurveillanceItemsDictionary.Add(surveillanceItemMapping.AccessControlOperationableInstanceId, surveillanceItemMapping.RelatedSurveillanceItems);
		}
	}

	public SurveillanceItemMappingSerializeData SerializeData()
	{
		lock (this)
		{
			SurveillanceItemMappingSerializeData surveillanceItemMappingSerializeData = new SurveillanceItemMappingSerializeData();
			foreach (KeyValuePair<Guid, List<FQID>> item in _operationableInstanceIdToSurveillanceItemsDictionary)
			{
				surveillanceItemMappingSerializeData.SurveillanceItemMappings.Add(new SurveillanceItemMappingSerializeData.SurveillanceItemMappingData
				{
					AccessControlOperationableInstanceId = item.Key,
					RelatedSurveillanceItems = item.Value
				});
			}
			return surveillanceItemMappingSerializeData;
		}
	}

	public IEnumerable<FQID> GetMappedSurveillanceItems(AccessControlOperationableInstance accessControlOperationableInstance)
	{
		lock (this)
		{
			if (_operationableInstanceIdToSurveillanceItemsDictionary.ContainsKey(accessControlOperationableInstance.Id))
			{
				return _operationableInstanceIdToSurveillanceItemsDictionary[accessControlOperationableInstance.Id];
			}
			return new FQID[0];
		}
	}

	public void SetMappedSurveillanceItems(AccessControlOperationableInstance accessControlOperationableInstance, IEnumerable<FQID> relatedSurveillanceItems)
	{
		lock (this)
		{
			if (!_operationableInstanceIdToSurveillanceItemsDictionary.ContainsKey(accessControlOperationableInstance.Id))
			{
				_operationableInstanceIdToSurveillanceItemsDictionary.Add(accessControlOperationableInstance.Id, new List<FQID>());
			}
			_operationableInstanceIdToSurveillanceItemsDictionary[accessControlOperationableInstance.Id] = new List<FQID>(relatedSurveillanceItems);
		}
	}
}
