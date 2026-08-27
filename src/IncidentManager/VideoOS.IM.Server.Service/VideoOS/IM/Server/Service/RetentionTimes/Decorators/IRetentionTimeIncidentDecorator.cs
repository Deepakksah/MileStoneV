using System.Collections.Generic;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.Incident;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Common;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Incident;
using VideoOS.IM.Common.Service.Properties;

namespace VideoOS.IM.Server.Service.RetentionTimes.Decorators;

public interface IRetentionTimeIncidentDecorator : IDtoIncident, IDtoBaseEntity
{
	void Decorate([NotNull] IEnumerable<DtoIncident> dtoIncidents, int daysToKeepIncidents);
}
