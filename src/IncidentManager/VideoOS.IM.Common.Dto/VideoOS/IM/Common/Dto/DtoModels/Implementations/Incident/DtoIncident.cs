using System;
using System.Collections.Generic;
using VideoOS.IM.Common.Dto.DtoModels.Abstractions.Common;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.DynamicalDataTypesAndOptions;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentActivityLog;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentCall;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentComment;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentSequence;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Common;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Incident;

namespace VideoOS.IM.Common.Dto.DtoModels.Implementations.Incident;

public class DtoIncident : ADtoBaseEntity, IDtoIncident, IDtoBaseEntity
{
	private long _Reference;

	private string _ReferenceStr;

	private string _TypeStr;

	private long? _TypeId;

	private DtoDynamicalDataTypesAndOptions _Type;

	private string _StatusStr;

	private long? _StatusId;

	private DtoDynamicalDataTypesAndOptions _Status;

	private int? _SequencesCount;

	private string _User;

	private DateTime? _Start;

	private string _StartStr;

	private DateTime? _End;

	private TimeSpan? _Time;

	private DateTime? _Date;

	private string _DynamicLockerStatusStrDb1;

	private string _DynamicLockerStatusStrDb2;

	private string _Description;

	private long? _Category1Id;

	private DtoDynamicalDataTypesAndOptions _Category1;

	private long? _Category2Id;

	private DtoDynamicalDataTypesAndOptions _Category2;

	private long? _Category3Id;

	private DtoDynamicalDataTypesAndOptions _Category3;

	private long? _Category4Id;

	private DtoDynamicalDataTypesAndOptions _Category4;

	private long? _Category5Id;

	private DtoDynamicalDataTypesAndOptions _Category5;

	private List<DtoIncidentComment> _IncidentCommentsList;

	private List<DtoIncidentCall> _IncidentCallsList;

	private List<DtoIncidentSequence> _SequencesList;

	private List<DtoIncidentActivityLog> _ActivityLogsList;

	public long Reference
	{
		get
		{
			return _Reference;
		}
		set
		{
			if (_Reference != value)
			{
				_Reference = value;
				NotifyPropertyChanged("Reference");
			}
		}
	}

	public string ReferenceStr
	{
		get
		{
			return _ReferenceStr;
		}
		set
		{
			if (_ReferenceStr != value)
			{
				_ReferenceStr = value;
				NotifyPropertyChanged("ReferenceStr");
			}
		}
	}

	public string TypeStr
	{
		get
		{
			return _TypeStr;
		}
		set
		{
			if (_TypeStr != value)
			{
				_TypeStr = value;
				NotifyPropertyChanged("TypeStr");
			}
		}
	}

	public long? TypeId
	{
		get
		{
			return _TypeId;
		}
		set
		{
			if (_TypeId != value)
			{
				_TypeId = value;
				NotifyPropertyChanged("TypeId");
			}
		}
	}

	public DtoDynamicalDataTypesAndOptions Type
	{
		get
		{
			return _Type;
		}
		set
		{
			if (_Type != value)
			{
				_Type = value;
				NotifyPropertyChanged("Type");
			}
		}
	}

	public string StatusStr
	{
		get
		{
			return _StatusStr;
		}
		set
		{
			if (_StatusStr != value)
			{
				_StatusStr = value;
				NotifyPropertyChanged("StatusStr");
			}
		}
	}

	public long? StatusId
	{
		get
		{
			return _StatusId;
		}
		set
		{
			if (_StatusId != value)
			{
				_StatusId = value;
				NotifyPropertyChanged("StatusId");
			}
		}
	}

	public DtoDynamicalDataTypesAndOptions Status
	{
		get
		{
			return _Status;
		}
		set
		{
			if (_Status != value)
			{
				_Status = value;
				NotifyPropertyChanged("Status");
			}
		}
	}

	public int? SequencesCount
	{
		get
		{
			return _SequencesCount;
		}
		set
		{
			if (_SequencesCount != value)
			{
				_SequencesCount = value;
				NotifyPropertyChanged("SequencesCount");
			}
		}
	}

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

	public DateTime? Start
	{
		get
		{
			return _Start;
		}
		set
		{
			if (_Start != value)
			{
				_Start = value;
				NotifyPropertyChanged("Start");
			}
		}
	}

	public string StartStr
	{
		get
		{
			return _StartStr;
		}
		set
		{
			if (_StartStr != value)
			{
				_StartStr = value;
				NotifyPropertyChanged("StartStr");
			}
		}
	}

	public DateTime? End
	{
		get
		{
			return _End;
		}
		set
		{
			if (_End != value)
			{
				_End = value;
				NotifyPropertyChanged("End");
			}
		}
	}

	public TimeSpan? Time
	{
		get
		{
			return _Time;
		}
		set
		{
			if (_Time != value)
			{
				_Time = value;
				NotifyPropertyChanged("Time");
			}
		}
	}

	public DateTime? Date
	{
		get
		{
			return _Date;
		}
		set
		{
			if (_Date != value)
			{
				_Date = value;
				NotifyPropertyChanged("Date");
			}
		}
	}

	public string DynamicLockerStatusStrDb1
	{
		get
		{
			return _DynamicLockerStatusStrDb1;
		}
		set
		{
			if (_DynamicLockerStatusStrDb1 != value)
			{
				_DynamicLockerStatusStrDb1 = value;
				NotifyPropertyChanged("DynamicLockerStatusStrDb1");
			}
		}
	}

	public string DynamicLockerStatusStrDb2
	{
		get
		{
			return _DynamicLockerStatusStrDb2;
		}
		set
		{
			if (_DynamicLockerStatusStrDb2 != value)
			{
				_DynamicLockerStatusStrDb2 = value;
				NotifyPropertyChanged("DynamicLockerStatusStrDb2");
			}
		}
	}

	public string DynamicLockerStatusStr
	{
		get
		{
			string text = "";
			int? num = SequencesList?.Count;
			if (num.HasValue && num.Value > 0)
			{
				return $"{num} Videos in Incident";
			}
			return "No Videos";
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

	public long ExtPropLong1 { get; set; }

	public string ExtPropLong1_Title { get; set; }

	public string ExtPropLong1_Desc { get; set; }

	public long ExtPropLong2 { get; set; }

	public string ExtPropLong2_Title { get; set; }

	public string ExtPropLong2_Desc { get; set; }

	public long ExtPropLong3 { get; set; }

	public string ExtPropLong3_Title { get; set; }

	public string ExtPropLong3_Desc { get; set; }

	public string ExtPropString1 { get; set; }

	public string ExtPropString1_Title { get; set; }

	public string ExtPropString1_Desc { get; set; }

	public string ExtPropString2 { get; set; }

	public string ExtPropString2_Title { get; set; }

	public string ExtPropString2_Desc { get; set; }

	public string ExtPropString3 { get; set; }

	public string ExtPropString3_Title { get; set; }

	public string ExtPropString3_Desc { get; set; }

	public bool ExtPropBool1 { get; set; }

	public string ExtPropBool1_Title { get; set; }

	public string ExtPropBool1_Desc { get; set; }

	public bool ExtPropBool2 { get; set; }

	public string ExtPropBool2_Title { get; set; }

	public string ExtPropBool2_Desc { get; set; }

	public bool ExtPropBool3 { get; set; }

	public string ExtPropBool3_Title { get; set; }

	public string ExtPropBool3_Desc { get; set; }

	public decimal ExtPropDecimal1 { get; set; }

	public string ExtPropDecimal1_Title { get; set; }

	public string ExtPropDecimal1_Desc { get; set; }

	public decimal ExtPropDecimal2 { get; set; }

	public string ExtPropDecimal2_Title { get; set; }

	public string ExtPropDecimal2_Desc { get; set; }

	public decimal ExtPropDecimal3 { get; set; }

	public string ExtPropDecimal3_Title { get; set; }

	public string ExtPropDecimal3_Desc { get; set; }

	public DateTime? ExtPropDateTime1 { get; set; }

	public string ExtPropDateTime1_Title { get; set; }

	public string ExtPropDateTime1_Desc { get; set; }

	public string ExtPropDateTime1_Configuration { get; set; }

	public DateTime? ExtPropDateTime2 { get; set; }

	public string ExtPropDateTime2_Title { get; set; }

	public string ExtPropDateTime2_Desc { get; set; }

	public string ExtPropDateTime2_Configuration { get; set; }

	public DateTime? ExtPropDateTime3 { get; set; }

	public string ExtPropDateTime3_Title { get; set; }

	public string ExtPropDateTime3_Desc { get; set; }

	public string ExtPropDateTime3_Configuration { get; set; }

	public string ExtPropJSon1 { get; set; }

	public string ExtPropJSon1_Title { get; set; }

	public string ExtPropJSon1_Desc { get; set; }

	public string ExtPropJSon1_Configuration { get; set; }

	public string ExtPropJSon2 { get; set; }

	public string ExtPropJSon2_Title { get; set; }

	public string ExtPropJSon2_Desc { get; set; }

	public string ExtPropJSon2_Configuration { get; set; }

	public string ExtPropJSon3 { get; set; }

	public string ExtPropJSon3_Title { get; set; }

	public string ExtPropJSon3_Desc { get; set; }

	public string ExtPropJSon3_Configuration { get; set; }

	public string ExtendedPropsGroup1_JSonConfiguration { get; set; }

	public string ExtendedPropsGroup2_JSonConfiguration { get; set; }

	public long? Category1Id
	{
		get
		{
			return _Category1Id;
		}
		set
		{
			if (_Category1Id != value)
			{
				_Category1Id = value;
				NotifyPropertyChanged("Category1Id");
			}
		}
	}

	public DtoDynamicalDataTypesAndOptions Category1
	{
		get
		{
			return _Category1;
		}
		set
		{
			if (_Category1 != value)
			{
				_Category1 = value;
				NotifyPropertyChanged("Category1");
			}
		}
	}

	public long? Category2Id
	{
		get
		{
			return _Category2Id;
		}
		set
		{
			if (_Category2Id != value)
			{
				_Category2Id = value;
				NotifyPropertyChanged("Category2Id");
			}
		}
	}

	public DtoDynamicalDataTypesAndOptions Category2
	{
		get
		{
			return _Category2;
		}
		set
		{
			if (_Category2 != value)
			{
				_Category2 = value;
				NotifyPropertyChanged("Category2");
			}
		}
	}

	public long? Category3Id
	{
		get
		{
			return _Category3Id;
		}
		set
		{
			if (_Category3Id != value)
			{
				_Category3Id = value;
				NotifyPropertyChanged("Category3Id");
			}
		}
	}

	public DtoDynamicalDataTypesAndOptions Category3
	{
		get
		{
			return _Category3;
		}
		set
		{
			if (_Category3 != value)
			{
				_Category3 = value;
				NotifyPropertyChanged("Category3");
			}
		}
	}

	public long? Category4Id
	{
		get
		{
			return _Category4Id;
		}
		set
		{
			if (_Category4Id != value)
			{
				_Category4Id = value;
				NotifyPropertyChanged("Category4Id");
			}
		}
	}

	public DtoDynamicalDataTypesAndOptions Category4
	{
		get
		{
			return _Category4;
		}
		set
		{
			if (_Category4 != value)
			{
				_Category4 = value;
				NotifyPropertyChanged("Category4");
			}
		}
	}

	public long? Category5Id
	{
		get
		{
			return _Category5Id;
		}
		set
		{
			if (_Category5Id != value)
			{
				_Category5Id = value;
				NotifyPropertyChanged("Category5Id");
			}
		}
	}

	public DtoDynamicalDataTypesAndOptions Category5
	{
		get
		{
			return _Category5;
		}
		set
		{
			if (_Category5 != value)
			{
				_Category5 = value;
				NotifyPropertyChanged("Category5");
			}
		}
	}

	public List<DtoIncidentComment> IncidentCommentsList
	{
		get
		{
			return _IncidentCommentsList;
		}
		set
		{
			if (_IncidentCommentsList != value)
			{
				_IncidentCommentsList = value;
				NotifyPropertyChanged("IncidentCommentsList");
			}
		}
	}

	public List<DtoIncidentCall> IncidentCallsList
	{
		get
		{
			return _IncidentCallsList;
		}
		set
		{
			if (_IncidentCallsList != value)
			{
				_IncidentCallsList = value;
				NotifyPropertyChanged("IncidentCallsList");
			}
		}
	}

	public List<DtoIncidentSequence> SequencesList
	{
		get
		{
			return _SequencesList;
		}
		set
		{
			if (_SequencesList != value)
			{
				_SequencesList = value;
				NotifyPropertyChanged("SequencesList");
			}
		}
	}

	public List<DtoIncidentActivityLog> ActivityLogsList
	{
		get
		{
			return _ActivityLogsList;
		}
		set
		{
			if (_ActivityLogsList != value)
			{
				_ActivityLogsList = value;
				NotifyPropertyChanged("ActivityLogsList");
			}
		}
	}

	public void CopyMyBasicPropsToObject(DtoIncident currentObject_InitialValues)
	{
		if (currentObject_InitialValues != null)
		{
			currentObject_InitialValues.Category2 = Category2;
			currentObject_InitialValues.Category2Id = Category2Id;
			currentObject_InitialValues.Category4 = Category4;
			currentObject_InitialValues.Category4Id = Category4Id;
			currentObject_InitialValues.CreatedUserId = base.CreatedUserId;
			currentObject_InitialValues.CreatedUserName = base.CreatedUserName;
			currentObject_InitialValues.Date = Date;
			currentObject_InitialValues.Description = Description;
			currentObject_InitialValues.End = End;
			currentObject_InitialValues.ExtendedPropsGroup1_JSonConfiguration = ExtendedPropsGroup1_JSonConfiguration;
			currentObject_InitialValues.ExtendedPropsGroup2_JSonConfiguration = ExtendedPropsGroup2_JSonConfiguration;
			currentObject_InitialValues.ExtPropBool1 = ExtPropBool1;
			currentObject_InitialValues.ExtPropBool1_Desc = ExtPropBool1_Desc;
			currentObject_InitialValues.ExtPropBool1_Title = ExtPropBool1_Title;
			currentObject_InitialValues.ExtPropBool2 = ExtPropBool2;
			currentObject_InitialValues.ExtPropBool2_Desc = ExtPropBool2_Desc;
			currentObject_InitialValues.ExtPropBool2_Title = ExtPropBool2_Title;
			currentObject_InitialValues.ExtPropBool3 = ExtPropBool3;
			currentObject_InitialValues.ExtPropBool3_Desc = ExtPropBool3_Desc;
			currentObject_InitialValues.ExtPropBool3_Title = ExtPropBool3_Title;
			currentObject_InitialValues.ExtPropDateTime1 = ExtPropDateTime1;
			currentObject_InitialValues.ExtPropDateTime1_Configuration = ExtPropDateTime1_Configuration;
			currentObject_InitialValues.ExtPropDateTime1_Desc = ExtPropDateTime1_Desc;
			currentObject_InitialValues.ExtPropDateTime1_Title = ExtPropDateTime1_Title;
			currentObject_InitialValues.ExtPropDateTime2 = ExtPropDateTime2;
			currentObject_InitialValues.ExtPropDateTime2_Configuration = ExtPropDateTime2_Configuration;
			currentObject_InitialValues.ExtPropDateTime2_Desc = ExtPropDateTime2_Desc;
			currentObject_InitialValues.ExtPropDateTime2_Title = ExtPropDateTime2_Title;
			currentObject_InitialValues.ExtPropDateTime3 = ExtPropDateTime3;
			currentObject_InitialValues.ExtPropDateTime3_Configuration = ExtPropDateTime3_Configuration;
			currentObject_InitialValues.ExtPropDateTime3_Desc = ExtPropDateTime3_Desc;
			currentObject_InitialValues.ExtPropDateTime3_Title = ExtPropDateTime3_Title;
			currentObject_InitialValues.ExtPropDecimal1 = ExtPropDecimal1;
			currentObject_InitialValues.ExtPropDecimal1_Desc = ExtPropDecimal1_Desc;
			currentObject_InitialValues.ExtPropDecimal1_Title = ExtPropDecimal1_Title;
			currentObject_InitialValues.ExtPropDecimal2 = ExtPropDecimal2;
			currentObject_InitialValues.ExtPropDecimal2_Desc = ExtPropDecimal2_Desc;
			currentObject_InitialValues.ExtPropDecimal2_Title = ExtPropDecimal2_Title;
			currentObject_InitialValues.ExtPropDecimal3 = ExtPropDecimal3;
			currentObject_InitialValues.ExtPropDecimal3_Desc = ExtPropDecimal3_Desc;
			currentObject_InitialValues.ExtPropDecimal3_Title = ExtPropDecimal3_Title;
			currentObject_InitialValues.ExtPropJSon1 = ExtPropJSon1;
			currentObject_InitialValues.ExtPropJSon1_Configuration = ExtPropJSon1_Configuration;
			currentObject_InitialValues.ExtPropJSon1_Desc = ExtPropJSon1_Desc;
			currentObject_InitialValues.ExtPropJSon1_Title = ExtPropJSon1_Title;
			currentObject_InitialValues.ExtPropJSon2 = ExtPropJSon2;
			currentObject_InitialValues.ExtPropJSon2_Configuration = ExtPropJSon2_Configuration;
			currentObject_InitialValues.ExtPropJSon2_Desc = ExtPropJSon2_Desc;
			currentObject_InitialValues.ExtPropJSon2_Title = ExtPropJSon2_Title;
			currentObject_InitialValues.ExtPropJSon3 = ExtPropJSon3;
			currentObject_InitialValues.ExtPropJSon3_Configuration = ExtPropJSon3_Configuration;
			currentObject_InitialValues.ExtPropJSon3_Desc = ExtPropJSon3_Desc;
			currentObject_InitialValues.ExtPropJSon3_Title = ExtPropJSon3_Title;
			currentObject_InitialValues.ExtPropLong1 = ExtPropLong1;
			currentObject_InitialValues.ExtPropLong1_Desc = ExtPropLong1_Desc;
			currentObject_InitialValues.ExtPropLong1_Title = ExtPropLong1_Title;
			currentObject_InitialValues.ExtPropLong2 = ExtPropLong2;
			currentObject_InitialValues.ExtPropLong2_Desc = ExtPropLong2_Desc;
			currentObject_InitialValues.ExtPropLong2_Title = ExtPropLong2_Title;
			currentObject_InitialValues.ExtPropLong3 = ExtPropLong3;
			currentObject_InitialValues.ExtPropLong3_Desc = ExtPropLong3_Desc;
			currentObject_InitialValues.ExtPropLong3_Title = ExtPropLong3_Title;
			currentObject_InitialValues.ExtPropString1 = ExtPropString1;
			currentObject_InitialValues.ExtPropString1_Desc = ExtPropString1_Desc;
			currentObject_InitialValues.ExtPropString1_Title = ExtPropString1_Title;
			currentObject_InitialValues.ExtPropString2 = ExtPropString2;
			currentObject_InitialValues.ExtPropString2_Desc = ExtPropString2_Desc;
			currentObject_InitialValues.ExtPropString2_Title = ExtPropString2_Title;
			currentObject_InitialValues.ExtPropString3 = ExtPropString3;
			currentObject_InitialValues.ExtPropString3_Desc = ExtPropString3_Desc;
			currentObject_InitialValues.ExtPropString3_Title = ExtPropString3_Title;
			currentObject_InitialValues.FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog = base.FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog;
			currentObject_InitialValues.FlagNoDb_UserOpenedIncident = base.FlagNoDb_UserOpenedIncident;
			currentObject_InitialValues.FlagNoDb_UserOpenedIncidentStrInfo = base.FlagNoDb_UserOpenedIncidentStrInfo;
			currentObject_InitialValues.Category3 = Category3;
			currentObject_InitialValues.Category3Id = Category3Id;
			currentObject_InitialValues.Category5 = Category5;
			currentObject_InitialValues.Category5Id = Category5Id;
			currentObject_InitialValues.ModifiedUserId = base.ModifiedUserId;
			currentObject_InitialValues.ModifiedUserName = base.ModifiedUserName;
			currentObject_InitialValues.Reference = Reference;
			currentObject_InitialValues.ReferenceStr = ReferenceStr;
			currentObject_InitialValues.Category1 = Category1;
			currentObject_InitialValues.Category1Id = Category1Id;
			currentObject_InitialValues.Start = Start;
			currentObject_InitialValues.StartStr = StartStr;
			currentObject_InitialValues.Status = Status;
			currentObject_InitialValues.StatusId = StatusId;
			currentObject_InitialValues.StatusStr = StatusStr;
			currentObject_InitialValues.SequencesCount = SequencesCount;
			currentObject_InitialValues.SystemString = base.SystemString;
			currentObject_InitialValues.Time = Time;
			currentObject_InitialValues.TimeCreated = base.TimeCreated;
			currentObject_InitialValues.TimeModified = base.TimeModified;
			currentObject_InitialValues.Type = Type;
			currentObject_InitialValues.TypeId = TypeId;
			currentObject_InitialValues.TypeStr = TypeStr;
			currentObject_InitialValues.UIIsSelected = base.UIIsSelected;
			currentObject_InitialValues.User = User;
		}
	}

	public bool IsChangesExistOnBasicProps(DtoIncident currentObject_InitialValues)
	{
		bool flag = false;
		int num;
		if (currentObject_InitialValues != null)
		{
			if (!flag && currentObject_InitialValues.Category2 == Category2 && currentObject_InitialValues.Category2Id == Category2Id && currentObject_InitialValues.Category4 == Category4 && currentObject_InitialValues.Category4Id == Category4Id && !(currentObject_InitialValues.CreatedUserId != base.CreatedUserId) && !(currentObject_InitialValues.CreatedUserName != base.CreatedUserName))
			{
				DateTime? date = currentObject_InitialValues.Date;
				DateTime? date2 = Date;
				if (date.HasValue == date2.HasValue && (!date.HasValue || !(date.GetValueOrDefault() != date2.GetValueOrDefault())) && !(currentObject_InitialValues.Description != Description))
				{
					date2 = currentObject_InitialValues.End;
					date = End;
					if (date2.HasValue == date.HasValue && (!date2.HasValue || !(date2.GetValueOrDefault() != date.GetValueOrDefault())) && !(currentObject_InitialValues.ExtendedPropsGroup1_JSonConfiguration != ExtendedPropsGroup1_JSonConfiguration) && !(currentObject_InitialValues.ExtendedPropsGroup2_JSonConfiguration != ExtendedPropsGroup2_JSonConfiguration) && currentObject_InitialValues.ExtPropBool1 == ExtPropBool1 && !(currentObject_InitialValues.ExtPropBool1_Desc != ExtPropBool1_Desc) && !(currentObject_InitialValues.ExtPropBool1_Title != ExtPropBool1_Title) && currentObject_InitialValues.ExtPropBool2 == ExtPropBool2 && !(currentObject_InitialValues.ExtPropBool2_Desc != ExtPropBool2_Desc) && !(currentObject_InitialValues.ExtPropBool2_Title != ExtPropBool2_Title) && currentObject_InitialValues.ExtPropBool3 == ExtPropBool3 && !(currentObject_InitialValues.ExtPropBool3_Desc != ExtPropBool3_Desc) && !(currentObject_InitialValues.ExtPropBool3_Title != ExtPropBool3_Title))
					{
						date = currentObject_InitialValues.ExtPropDateTime1;
						date2 = ExtPropDateTime1;
						if (date.HasValue == date2.HasValue && (!date.HasValue || !(date.GetValueOrDefault() != date2.GetValueOrDefault())) && !(currentObject_InitialValues.ExtPropDateTime1_Configuration != ExtPropDateTime1_Configuration) && !(currentObject_InitialValues.ExtPropDateTime1_Desc != ExtPropDateTime1_Desc) && !(currentObject_InitialValues.ExtPropDateTime1_Title != ExtPropDateTime1_Title))
						{
							date2 = currentObject_InitialValues.ExtPropDateTime2;
							date = ExtPropDateTime2;
							if (date2.HasValue == date.HasValue && (!date2.HasValue || !(date2.GetValueOrDefault() != date.GetValueOrDefault())) && !(currentObject_InitialValues.ExtPropDateTime2_Configuration != ExtPropDateTime2_Configuration) && !(currentObject_InitialValues.ExtPropDateTime2_Desc != ExtPropDateTime2_Desc) && !(currentObject_InitialValues.ExtPropDateTime2_Title != ExtPropDateTime2_Title))
							{
								date = currentObject_InitialValues.ExtPropDateTime3;
								date2 = ExtPropDateTime3;
								if (date.HasValue == date2.HasValue && (!date.HasValue || !(date.GetValueOrDefault() != date2.GetValueOrDefault())) && !(currentObject_InitialValues.ExtPropDateTime3_Configuration != ExtPropDateTime3_Configuration) && !(currentObject_InitialValues.ExtPropDateTime3_Desc != ExtPropDateTime3_Desc) && !(currentObject_InitialValues.ExtPropDateTime3_Title != ExtPropDateTime3_Title) && !(currentObject_InitialValues.ExtPropDecimal1 != ExtPropDecimal1) && !(currentObject_InitialValues.ExtPropDecimal1_Desc != ExtPropDecimal1_Desc) && !(currentObject_InitialValues.ExtPropDecimal1_Title != ExtPropDecimal1_Title) && !(currentObject_InitialValues.ExtPropDecimal2 != ExtPropDecimal2) && !(currentObject_InitialValues.ExtPropDecimal2_Desc != ExtPropDecimal2_Desc) && !(currentObject_InitialValues.ExtPropDecimal2_Title != ExtPropDecimal2_Title) && !(currentObject_InitialValues.ExtPropDecimal3 != ExtPropDecimal3) && !(currentObject_InitialValues.ExtPropDecimal3_Desc != ExtPropDecimal3_Desc) && !(currentObject_InitialValues.ExtPropDecimal3_Title != ExtPropDecimal3_Title) && !(currentObject_InitialValues.ExtPropJSon1 != ExtPropJSon1) && !(currentObject_InitialValues.ExtPropJSon1_Configuration != ExtPropJSon1_Configuration) && !(currentObject_InitialValues.ExtPropJSon1_Desc != ExtPropJSon1_Desc) && !(currentObject_InitialValues.ExtPropJSon1_Title != ExtPropJSon1_Title) && !(currentObject_InitialValues.ExtPropJSon2 != ExtPropJSon2) && !(currentObject_InitialValues.ExtPropJSon2_Configuration != ExtPropJSon2_Configuration) && !(currentObject_InitialValues.ExtPropJSon2_Desc != ExtPropJSon2_Desc) && !(currentObject_InitialValues.ExtPropJSon2_Title != ExtPropJSon2_Title) && !(currentObject_InitialValues.ExtPropJSon3 != ExtPropJSon3) && !(currentObject_InitialValues.ExtPropJSon3_Configuration != ExtPropJSon3_Configuration) && !(currentObject_InitialValues.ExtPropJSon3_Desc != ExtPropJSon3_Desc) && !(currentObject_InitialValues.ExtPropJSon3_Title != ExtPropJSon3_Title) && currentObject_InitialValues.ExtPropLong1 == ExtPropLong1 && !(currentObject_InitialValues.ExtPropLong1_Desc != ExtPropLong1_Desc) && !(currentObject_InitialValues.ExtPropLong1_Title != ExtPropLong1_Title) && currentObject_InitialValues.ExtPropLong2 == ExtPropLong2 && !(currentObject_InitialValues.ExtPropLong2_Desc != ExtPropLong2_Desc) && !(currentObject_InitialValues.ExtPropLong2_Title != ExtPropLong2_Title) && currentObject_InitialValues.ExtPropLong3 == ExtPropLong3 && !(currentObject_InitialValues.ExtPropLong3_Desc != ExtPropLong3_Desc) && !(currentObject_InitialValues.ExtPropLong3_Title != ExtPropLong3_Title) && !(currentObject_InitialValues.ExtPropString1 != ExtPropString1) && !(currentObject_InitialValues.ExtPropString1_Desc != ExtPropString1_Desc) && !(currentObject_InitialValues.ExtPropString1_Title != ExtPropString1_Title) && !(currentObject_InitialValues.ExtPropString2 != ExtPropString2) && !(currentObject_InitialValues.ExtPropString2_Desc != ExtPropString2_Desc) && !(currentObject_InitialValues.ExtPropString2_Title != ExtPropString2_Title) && !(currentObject_InitialValues.ExtPropString3 != ExtPropString3) && !(currentObject_InitialValues.ExtPropString3_Desc != ExtPropString3_Desc) && !(currentObject_InitialValues.ExtPropString3_Title != ExtPropString3_Title) && currentObject_InitialValues.FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog == base.FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog && currentObject_InitialValues.FlagNoDb_UserOpenedIncident == base.FlagNoDb_UserOpenedIncident && !(currentObject_InitialValues.FlagNoDb_UserOpenedIncidentStrInfo != base.FlagNoDb_UserOpenedIncidentStrInfo) && currentObject_InitialValues.Category3 == Category3 && currentObject_InitialValues.Category3Id == Category3Id && currentObject_InitialValues.Category5 == Category5 && currentObject_InitialValues.Category5Id == Category5Id && !(currentObject_InitialValues.ModifiedUserId != base.ModifiedUserId) && !(currentObject_InitialValues.ModifiedUserName != base.ModifiedUserName) && currentObject_InitialValues.Reference == Reference && !(currentObject_InitialValues.ReferenceStr != ReferenceStr) && currentObject_InitialValues.Category1 == Category1 && currentObject_InitialValues.Category1Id == Category1Id)
								{
									date2 = currentObject_InitialValues.Start;
									date = Start;
									if (date2.HasValue == date.HasValue && (!date2.HasValue || !(date2.GetValueOrDefault() != date.GetValueOrDefault())) && !(currentObject_InitialValues.StartStr != StartStr) && currentObject_InitialValues.Status == Status && currentObject_InitialValues.StatusId == StatusId && !(currentObject_InitialValues.StatusStr != StatusStr) && currentObject_InitialValues._SequencesCount == _SequencesCount && !(currentObject_InitialValues.SystemString != base.SystemString))
									{
										TimeSpan? time = currentObject_InitialValues.Time;
										TimeSpan? time2 = Time;
										if (time.HasValue == time2.HasValue && (!time.HasValue || !(time.GetValueOrDefault() != time2.GetValueOrDefault())) && currentObject_InitialValues.Type == Type && currentObject_InitialValues.TypeId == TypeId && !(currentObject_InitialValues.TypeStr != TypeStr) && currentObject_InitialValues.UIIsSelected == base.UIIsSelected)
										{
											num = ((currentObject_InitialValues.User != User) ? 1 : 0);
											goto IL_0b7a;
										}
									}
								}
							}
						}
					}
				}
			}
			num = 1;
			goto IL_0b7a;
		}
		goto IL_0b7b;
		IL_0b7b:
		return flag;
		IL_0b7a:
		flag = (byte)num != 0;
		goto IL_0b7b;
	}
}
