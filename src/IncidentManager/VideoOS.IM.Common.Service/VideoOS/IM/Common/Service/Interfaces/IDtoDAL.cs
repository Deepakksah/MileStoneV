using VideoOS.IM.Common.Service.Interfaces.DraftSequence;
using VideoOS.IM.Common.Service.Interfaces.DynamicalDataTypesAndOptions;
using VideoOS.IM.Common.Service.Interfaces.GlobalSettings;
using VideoOS.IM.Common.Service.Interfaces.Incident;
using VideoOS.IM.Common.Service.Interfaces.IncidentActivityLog;
using VideoOS.IM.Common.Service.Interfaces.IncidentCall;
using VideoOS.IM.Common.Service.Interfaces.IncidentComment;
using VideoOS.IM.Common.Service.Interfaces.IncidentSequence;

namespace VideoOS.IM.Common.Service.Interfaces;

public interface IDtoDAL
{
	IDtoIncidentDAL GetIncidentDAL();

	IDtoDraftSequenceDAL GetDraftSequenceDAL();

	IDtoDynamicalDataTypesAndOptionsDAL GetDynamicalDataTypesAndOptionsDAL();

	IDtoIncidentCallDAL GetIncidentCallDAL();

	IDtoIncidentSequenceDAL GetIncidentSequenceDAL();

	IDtoIncidentCommentDAL GetIncidentCommentDAL();

	IDtoIncidentActivityLogDAL GetIncidentActivityLogDAL();

	IDtoGlobalSettingsDAL GetGlobalSettingsDAL();
}
