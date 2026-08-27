using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class QueryDeviceEvidenceLockSizesResponseModel : ResponseModel
{
	[Required]
	public DeviceEvidenceLockSizeModel[] Sizes { get; set; }

	public static QueryDeviceEvidenceLockSizesResponseModel FromResponse(QueryDeviceEvidenceLockSizesResponse response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		return new QueryDeviceEvidenceLockSizesResponseModel
		{
			Sizes = response.Sizes.Select((DeviceEvidenceLockSize s) => new DeviceEvidenceLockSizeModel
			{
				DeviceId = s.DeviceId,
				Size = s.Size
			}).ToArray()
		};
	}

	public QueryDeviceEvidenceLockSizesResponse ToResponse()
	{
		QueryDeviceEvidenceLockSizesResponse queryDeviceEvidenceLockSizesResponse = new QueryDeviceEvidenceLockSizesResponse();
		DeviceEvidenceLockSizeModel[] sizes = Sizes;
		foreach (DeviceEvidenceLockSizeModel deviceEvidenceLockSizeModel in sizes)
		{
			queryDeviceEvidenceLockSizesResponse.Sizes.Add(new DeviceEvidenceLockSize(deviceEvidenceLockSizeModel.DeviceId, deviceEvidenceLockSizeModel.Size));
		}
		return queryDeviceEvidenceLockSizesResponse;
	}
}
