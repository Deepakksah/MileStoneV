using VideoOS.IM.Common.Dto.DtoModels.Implementations.Incident;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.Incident;
using VideoOS.IM.Common.Service.Interfaces.Common;

namespace VideoOS.IM.Common.Service.Interfaces.Incident;

public interface IDtoIncidentDAL : IDtoObjectBaseDAL<long, DtoIncident, DtoIncidentResponse, DtoIncidentListResponse>
{
}
