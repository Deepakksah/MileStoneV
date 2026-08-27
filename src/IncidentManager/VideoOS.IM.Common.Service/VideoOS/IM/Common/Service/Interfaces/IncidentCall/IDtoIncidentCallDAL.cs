using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentCall;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.IncidentCall;
using VideoOS.IM.Common.Service.Interfaces.Common;

namespace VideoOS.IM.Common.Service.Interfaces.IncidentCall;

public interface IDtoIncidentCallDAL : IDtoObjectBaseDAL<long, DtoIncidentCall, DtoIncidentCallResponse, DtoIncidentCallListResponse>
{
}
