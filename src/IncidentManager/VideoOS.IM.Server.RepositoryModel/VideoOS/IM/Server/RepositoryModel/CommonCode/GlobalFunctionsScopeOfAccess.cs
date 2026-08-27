using VideoOS.IM.Common.Dto.DtoModels.Implementations.Incident;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentActivityLog;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentCall;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentComment;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentSequence;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentActivityLog;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentCall;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentComment;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentSequence;

namespace VideoOS.IM.Server.RepositoryModel.CommonCode;

public class GlobalFunctionsScopeOfAccess
{
	public static string GetClassTitleFromClassName(string inClassName)
	{
		string result = "Unknown type";
		if (!string.IsNullOrWhiteSpace(inClassName))
		{
			string text = typeof(RepoIncident).ToString();
			string text2 = typeof(RepoIncidentCall).ToString();
			string text3 = typeof(RepoIncidentSequence).ToString();
			string text4 = typeof(RepoIncidentComment).ToString();
			string text5 = typeof(RepoIncidentActivityLog).ToString();
			if (inClassName == text)
			{
				return "Incident";
			}
			if (inClassName == text2)
			{
				return "Incident Call";
			}
			if (inClassName == text3)
			{
				return "Incident Video Clip";
			}
			if (inClassName == text4)
			{
				return "Incident Comment";
			}
			if (inClassName == text5)
			{
				return "Incident ActivityLog";
			}
			string text6 = typeof(DtoIncident).ToString();
			string text7 = typeof(DtoIncidentCall).ToString();
			string text8 = typeof(DtoIncidentSequence).ToString();
			string text9 = typeof(DtoIncidentComment).ToString();
			string text10 = typeof(DtoIncidentActivityLog).ToString();
			if (inClassName == text6)
			{
				return "Incident";
			}
			if (inClassName == text7)
			{
				return "Incident Call";
			}
			if (inClassName == text8)
			{
				return "Incident Video Clip";
			}
			if (inClassName == text9)
			{
				return "Incident Comment";
			}
			if (inClassName == text10)
			{
				return "Incident ActivityLog";
			}
		}
		return result;
	}
}
