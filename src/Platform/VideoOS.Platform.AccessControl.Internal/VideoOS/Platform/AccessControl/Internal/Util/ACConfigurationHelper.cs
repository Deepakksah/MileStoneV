using System.Linq;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Internal.Util;

public class ACConfigurationHelper
{
	public static ACConfiguration CreateACConfiguration(string serializedString)
	{
		ACConfigurationSerializeData aCConfigurationSerializeData = ACConfigurationSerializeData.Deserialize(serializedString);
		if (aCConfigurationSerializeData == null)
		{
			return null;
		}
		return ACConfiguration.CreateACConfiguration(aCConfigurationSerializeData.Timestamp, aCConfigurationSerializeData.ACElements.ToArray());
	}

	public static string Serialize(ACConfiguration acConfiguration)
	{
		return ACConfigurationSerializeData.Serialize(SerializeData(acConfiguration));
	}

	public static ACConfigurationSerializeData SerializeData(ACConfiguration acConfiguration)
	{
		if (acConfiguration == null)
		{
			return null;
		}
		return new ACConfigurationSerializeData
		{
			Timestamp = acConfiguration.Timestamp,
			ACElements = acConfiguration.ACElements.ToList()
		};
	}
}
