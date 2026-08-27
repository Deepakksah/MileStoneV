using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
public class AppointmentDefinitions
{
	[DataMember]
	public Collection<AppointmentDefinition> Appointments { get; private set; }

	public AppointmentDefinitions()
	{
		Appointments = new Collection<AppointmentDefinition>();
	}
}
