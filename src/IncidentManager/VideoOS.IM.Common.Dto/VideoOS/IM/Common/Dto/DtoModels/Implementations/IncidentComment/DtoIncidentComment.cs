using VideoOS.IM.Common.Dto.DtoModels.Abstractions.Common;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.Incident;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Common;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.IncidentComment;

namespace VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentComment;

public class DtoIncidentComment : ADtoBaseEntity, IDtoIncidentComment, IDtoBaseEntity
{
	private string _User;

	private string _Comment;

	private long _IncidentId;

	private DtoIncident _Incident;

	public string User
	{
		get
		{
			string text = base.CreatedUserName;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = base.ModifiedUserName;
			}
			return text;
		}
		set
		{
			if (_User != value)
			{
				_User = value;
				NotifyPropertyChanged("User");
			}
		}
	}

	public string Comment
	{
		get
		{
			return _Comment;
		}
		set
		{
			if (_Comment != value)
			{
				_Comment = value;
				NotifyPropertyChanged("Comment");
			}
		}
	}

	public long IncidentId
	{
		get
		{
			return _IncidentId;
		}
		set
		{
			if (_IncidentId != value)
			{
				_IncidentId = value;
				NotifyPropertyChanged("IncidentId");
			}
		}
	}

	public DtoIncident Incident
	{
		get
		{
			return _Incident;
		}
		set
		{
			if (_Incident != value)
			{
				_Incident = value;
				NotifyPropertyChanged("Incident");
			}
		}
	}
}
