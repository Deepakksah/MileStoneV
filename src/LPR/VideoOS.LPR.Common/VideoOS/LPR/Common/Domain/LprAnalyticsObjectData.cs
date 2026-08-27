using System.Runtime.Serialization;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.Domain;

public class LprAnalyticsObjectData
{
	[DataMember]
	public string PlateStyleId { get; set; }

	[DataMember]
	public MatchListItemType MatchListItemType { get; set; }

	[DataMember]
	public VehicleProperty VehicleType { get; set; }

	[DataMember]
	public VehicleProperty VehicleColor { get; set; }

	[DataMember]
	public VehicleProperty VehicleMake { get; set; }

	[DataMember]
	public VehicleProperty VehicleModel { get; set; }

	[DataMember]
	public VehicleProperty VehicleSide { get; set; }
}
