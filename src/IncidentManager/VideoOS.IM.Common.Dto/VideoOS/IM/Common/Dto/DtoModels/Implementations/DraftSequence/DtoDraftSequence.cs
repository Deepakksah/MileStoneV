using System;
using System.Collections.Generic;
using VideoOS.IM.Common.Dto.DtoModels.Abstractions.Common;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentSequence;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Common;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.DraftSequence;

namespace VideoOS.IM.Common.Dto.DtoModels.Implementations.DraftSequence;

public class DtoDraftSequence : ADtoBaseEntity, IDtoDraftSequence, IDtoBaseEntity
{
	private string _Title;

	private DateTime? _TimeStamp;

	private string _Description;

	private string _DraftSequenceIdentificationMsUSerId;

	private List<DtoIncidentSequence> _SequenceList;

	public string Title
	{
		get
		{
			return _Title;
		}
		set
		{
			if (_Title != value)
			{
				_Title = value;
				NotifyPropertyChanged("Title");
			}
		}
	}

	public DateTime? TimeStamp
	{
		get
		{
			return _TimeStamp;
		}
		set
		{
			if (_TimeStamp != value)
			{
				_TimeStamp = value;
				NotifyPropertyChanged("TimeStamp");
			}
		}
	}

	public string Description
	{
		get
		{
			return _Description;
		}
		set
		{
			if (_Description != value)
			{
				_Description = value;
				NotifyPropertyChanged("Description");
			}
		}
	}

	public string DraftSequenceIdentificationMsUSerId
	{
		get
		{
			return _DraftSequenceIdentificationMsUSerId;
		}
		set
		{
			if (_DraftSequenceIdentificationMsUSerId != value)
			{
				_DraftSequenceIdentificationMsUSerId = value;
				NotifyPropertyChanged("DraftSequenceIdentificationMsUSerId");
			}
		}
	}

	public List<DtoIncidentSequence> SequenceList
	{
		get
		{
			return _SequenceList;
		}
		set
		{
			if (_SequenceList != value)
			{
				_SequenceList = value;
				NotifyPropertyChanged("SequenceList");
			}
		}
	}
}
