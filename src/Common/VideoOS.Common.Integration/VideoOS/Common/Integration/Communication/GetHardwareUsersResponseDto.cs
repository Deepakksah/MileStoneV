using System;
using System.Collections.ObjectModel;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public class GetHardwareUsersResponseDto
{
	public Guid HardwareId { get; private set; }

	public HardwareUsersDtoCollection HardwareUsers { get; private set; }

	public static GetHardwareUsersResponseDto CreateDto(GetHardwareUsersResponse response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		GetHardwareUsersResponseDto getHardwareUsersResponseDto = new GetHardwareUsersResponseDto();
		getHardwareUsersResponseDto.HardwareId = response.HardwareId;
		getHardwareUsersResponseDto.HardwareUsers = new HardwareUsersDtoCollection();
		foreach (HardwareUser hardwareUser in response.HardwareUsers)
		{
			HardwareUserDto item = HardwareUserDto.CreateDto(hardwareUser);
			getHardwareUsersResponseDto.HardwareUsers.Add(item);
		}
		return getHardwareUsersResponseDto;
	}

	public static GetHardwareUsersResponse CreateResponse(GetHardwareUsersResponseDto dto)
	{
		if (dto == null)
		{
			throw new ArgumentNullException("dto");
		}
		GetHardwareUsersResponse getHardwareUsersResponse = new GetHardwareUsersResponse(dto.HardwareId, new Collection<HardwareUser>());
		foreach (HardwareUserDto hardwareUser in dto.HardwareUsers)
		{
			HardwareUser item = HardwareUserDto.CreateHardwareUser(hardwareUser);
			getHardwareUsersResponse.HardwareUsers.Add(item);
		}
		return getHardwareUsersResponse;
	}
}
