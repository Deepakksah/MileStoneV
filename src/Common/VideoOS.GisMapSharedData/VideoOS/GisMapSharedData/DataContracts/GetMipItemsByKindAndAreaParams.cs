using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Building;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetMipItemsByKindAndAreaParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetMipItemsByKindAndAreaParams : ParamsRespBase
{
	[DataProperty]
	public Guid Kind { get; set; }

	[DataProperty]
	public Rectangle Area { get; set; }

	[DataProperty]
	public double ClusteringThreshold { get; set; }

	[DataProperty]
	public double MinimumScaleToCluster { get; set; }

	[DataProperty]
	public int ClusterLevelOffset { get; set; }

	[DataProperty]
	public BuildingWithActiveLevelData[] BuildingsInExtent { get; set; }
}
