using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.Incident;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Common;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Incident;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Service.RetentionTimes.Decorators;

public sealed class RetentionTimeIncidentDecorator : IRetentionTimeIncidentDecorator, IDtoIncident, IDtoBaseEntity
{
	public void Decorate(IEnumerable<DtoIncident> dtoIncidents, int daysToKeepIncidents)
	{
		dtoIncidents.NullCheck("", "Decorate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\RetentionTimes\\Decorators\\RetentionTimeIncidentDecorator.cs", 16);
		if (daysToKeepIncidents < 0)
		{
			throw new ArgumentException("The provided number should be higher or equal than zero.", "daysToKeepIncidents");
		}
		Parallel.ForEach(dtoIncidents.ToList(), delegate(DtoIncident dtoIncident)
		{
			DateTime? timeCreated = dtoIncident.TimeCreated;
			dtoIncident.TimeCreated = (timeCreated.HasValue ? new DateTime?(timeCreated.GetValueOrDefault().AddDays(daysToKeepIncidents)) : dtoIncident.TimeCreated);
		});
	}
}
