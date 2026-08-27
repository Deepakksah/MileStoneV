using VideoOS.IM.Common.Dto.DtoModels.Abstractions.Common;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Common;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.User;

namespace VideoOS.IM.Common.Dto.DtoModels.Implementations.User;

public class DtoMilestoneUser : ADtoBaseEntity, IDtoMilestoneUser, IDtoBaseEntity
{
	public string UserName { get; set; }

	public string MilestoneUserId { get; set; }
}
