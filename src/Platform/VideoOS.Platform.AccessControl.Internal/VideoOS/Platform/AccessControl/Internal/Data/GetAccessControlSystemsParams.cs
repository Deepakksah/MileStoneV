using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetAccessControlSystemsParams", Namespace = "urn:milestone-systems")]
public class GetAccessControlSystemsParams : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlPluginId { get; set; }

	[DataMember]
	public bool IncludeDisabledSystems { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	[OnDeserializing]
	private void SetIncludeDisabledSystemsDefaultValue(StreamingContext c)
	{
		IncludeDisabledSystems = true;
	}
}
