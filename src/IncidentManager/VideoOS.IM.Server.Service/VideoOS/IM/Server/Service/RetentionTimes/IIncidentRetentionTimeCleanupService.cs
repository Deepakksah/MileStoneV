using System;
using System.Threading.Tasks;
using VideoOS.IM.Common.Service.Common;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.Service.RetentionTimes.Decorators;
using VideoOS.IM.Server.Service.Time;

namespace VideoOS.IM.Server.Service.RetentionTimes;

public interface IIncidentRetentionTimeCleanupService
{
	void InitApplyIncidentRetentionAfter([NotNull] IRetentionTimeIncidentDecorator retentionTimeIncidentDecorator, [NotNull] ITimeProvider timeProvider, [NotNull] IDtoGlobalSettingsManager dtoGlobalSettingsManager);

	Task StartApplyingRetentionTimeCleanupAsync(TimeSpan? interval = null);

	void StopApplyingRetentionTimeCleanup();
}
