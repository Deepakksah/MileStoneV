using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Common;

namespace VideoOS.IM.Common.Dto.DtoModels.Interfaces.User;

public interface IDtoMilestoneUser : IDtoBaseEntity
{
	string MilestoneUserId { get; set; }

	string UserName { get; set; }
}
