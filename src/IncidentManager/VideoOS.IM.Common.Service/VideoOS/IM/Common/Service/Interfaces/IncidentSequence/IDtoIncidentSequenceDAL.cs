using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentSequence;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.IncidentSequence;
using VideoOS.IM.Common.Service.Interfaces.Common;

namespace VideoOS.IM.Common.Service.Interfaces.IncidentSequence;

public interface IDtoIncidentSequenceDAL : IDtoObjectBaseDAL<long, DtoIncidentSequence, DtoIncidentSequenceResponse, DtoIncidentSequenceListResponse>
{
}
