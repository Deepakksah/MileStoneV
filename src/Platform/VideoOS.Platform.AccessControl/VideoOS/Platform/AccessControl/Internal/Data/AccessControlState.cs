using System;
using System.Runtime.Serialization;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "AccessControlState", Namespace = "urn:milestone-systems")]
public class AccessControlState : IExtensibleDataObject
{
	[DataMember(Order = 1)]
	public Guid AccessControlSystemId { get; set; }

	[DataMember(Order = 2)]
	public Guid AccessControlOperationableInstanceId { get; set; }

	[DataMember(Order = 3)]
	public Guid[] AccessControlStateTypeIds { get; set; }

	[DataMember(Order = 4)]
	public Guid IconKey { get; set; }

	[DataMember(Order = 5)]
	public Property[] Properties { get; set; }

	[DataMember(Order = 6)]
	public DateTime ReceivedTime { get; set; }

	[DataMember(Order = 7)]
	public long SeqNo { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public override bool Equals(object obj)
	{
		if (!(obj is AccessControlState accessControlState))
		{
			return false;
		}
		if (!AccessControlSystemId.Equals(accessControlState.AccessControlSystemId) || !AccessControlOperationableInstanceId.Equals(accessControlState.AccessControlOperationableInstanceId))
		{
			return false;
		}
		if ((AccessControlStateTypeIds == null && accessControlState.AccessControlStateTypeIds != null) || (AccessControlStateTypeIds != null && accessControlState.AccessControlStateTypeIds == null))
		{
			return false;
		}
		if (AccessControlStateTypeIds != null && accessControlState.AccessControlStateTypeIds != null)
		{
			if (AccessControlStateTypeIds.Length != accessControlState.AccessControlStateTypeIds.Length)
			{
				return false;
			}
			Guid[] accessControlStateTypeIds = AccessControlStateTypeIds;
			foreach (Guid g in accessControlStateTypeIds)
			{
				bool flag = false;
				Guid[] accessControlStateTypeIds2 = accessControlState.AccessControlStateTypeIds;
				foreach (Guid guid in accessControlStateTypeIds2)
				{
					if (guid.Equals(g))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
		}
		if (!IconKey.Equals(accessControlState.IconKey))
		{
			return false;
		}
		if ((Properties == null && accessControlState.Properties != null) || (Properties != null && accessControlState.Properties == null))
		{
			return false;
		}
		if (Properties != null && accessControlState.Properties != null)
		{
			if (Properties.Length != accessControlState.Properties.Length)
			{
				return false;
			}
			Property[] properties = Properties;
			foreach (Property obj2 in properties)
			{
				bool flag2 = false;
				Property[] properties2 = accessControlState.Properties;
				for (int j = 0; j < properties2.Length; j++)
				{
					if (properties2[j].Equals(obj2))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					return false;
				}
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		return AccessControlOperationableInstanceId.GetHashCode();
	}
}
