using System;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Map;

public interface IMapClient : IClientBase, IDisposable
{
	[Obsolete("Method will be removed from the IMapClient in 2021R2.")]
	Task<AddMapResp> AddMapAsync(AddMapParams parameters);

	Task<UpdateMapResp> UpdateMapAsync(UpdateMapParams parameters);

	[Obsolete("Method will be removed from the IMapClient in 2021R2.")]
	Task<BrowseMapsResp> BrowseMapsAsync(BrowseMapsParams parameters);

	Task<GetMapResp> GetMapAsync(GetMapParams parameters);

	[Obsolete("Method will be removed from the IMapClient in 2021R2.")]
	Task<DeleteMapResp> DeleteMapAsync(DeleteMapParams parameters);

	[Obsolete("Method will be removed from the IMapClient in 2021R2.")]
	AddMapResp AddMap(AddMapParams parameters);

	UpdateMapResp UpdateMap(UpdateMapParams parameters);

	[Obsolete("Method will be removed from the IMapClient in 2021R2.")]
	BrowseMapsResp BrowseMaps(BrowseMapsParams parameters);

	GetMapResp GetMap(GetMapParams parameters);

	[Obsolete("Method will be removed from the IMapClient in 2021R2.")]
	DeleteMapResp DeleteMap(DeleteMapParams parameters);
}
