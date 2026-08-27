using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class Export
{
	public const string UserDomainNameXmlNodeName = "UserSid";

	public const string FullUserXmlNodeName = "FullUserName";

	public const string UserXmlNodeName = "User";

	public const string CameraNameXmlNode = "CameraName";

	public const string TypeName = "InitType";

	public static readonly DateTime InvalidDateTime = TimeConverter.Dt1970;

	private DateTime _startTime;

	private DateTime _endTime;

	private ExportType _exportType;

	private ExportInitType _exportInitType;

	private bool _adUser;

	private DateTime _queuedOnTime;

	private DateTime _completedTime;

	[JsonProperty("StartTime")]
	[XmlAttribute(AttributeName = "StartTime")]
	public string StartTime
	{
		get
		{
			if (_startTime == InvalidDateTime)
			{
				return null;
			}
			return _startTime.ToMilisecString();
		}
		set
		{
			_startTime = TimeConverter.FromString(value);
		}
	}

	[JsonProperty("EndTime")]
	[XmlAttribute(AttributeName = "EndTime")]
	public string EndTime
	{
		get
		{
			if (_endTime == InvalidDateTime)
			{
				return null;
			}
			return _endTime.ToMilisecString();
		}
		set
		{
			_endTime = TimeConverter.FromString(value);
		}
	}

	[JsonProperty("ExportId")]
	[XmlAttribute(AttributeName = "ExportId")]
	public Guid ExportId { get; set; }

	[JsonProperty("Name")]
	[XmlAttribute(AttributeName = "Name")]
	public string ExportName { get; set; }

	[JsonProperty("FileName")]
	[XmlAttribute(AttributeName = "FileName")]
	public string FileName { get; set; }

	[JsonProperty("CameraId")]
	[XmlAttribute(AttributeName = "CameraId")]
	public Guid CameraId
	{
		get
		{
			if (Cameras.Count > 0)
			{
				return Cameras[0].CameraId;
			}
			return Guid.Empty;
		}
		set
		{
			if (!Cameras.Any((CameraRelatedIds c) => c.CameraId == value))
			{
				Cameras.Insert(0, new CameraRelatedIds
				{
					CameraId = value
				});
			}
		}
	}

	[JsonProperty("CameraName")]
	[XmlAttribute(AttributeName = "CameraName")]
	public string CameraName { get; set; }

	[JsonProperty("Type")]
	[XmlAttribute(AttributeName = "Type")]
	public string ExportType
	{
		get
		{
			return EnumsHelper.GetDescription(_exportType);
		}
		set
		{
			_exportType = EnumsHelper.EnumConverter<ExportType>(value);
		}
	}

	[JsonProperty("CodecName")]
	[XmlAttribute(AttributeName = "CodecName")]
	public string CodecName { get; set; }

	[JsonProperty("State")]
	[XmlAttribute(AttributeName = "State")]
	public int State { get; set; }

	[JsonProperty("User")]
	[XmlAttribute(AttributeName = "User")]
	public string User { get; set; }

	[JsonProperty("FullUserName")]
	[XmlAttribute(AttributeName = "FullUserName")]
	public string FullUserName { get; set; }

	[JsonProperty("UserSid")]
	[XmlAttribute(AttributeName = "UserSid")]
	public string UserSid { get; set; }

	[JsonProperty("Hash")]
	[XmlAttribute(AttributeName = "Hash")]
	public string Hash { get; set; }

	[JsonProperty("HashF")]
	[XmlAttribute(AttributeName = "HashF")]
	public string FullHash { get; set; }

	[JsonProperty("HashR")]
	[XmlAttribute(AttributeName = "HashR")]
	public string HashR { get; set; }

	[JsonProperty("HashU")]
	[XmlAttribute(AttributeName = "HashU")]
	public string HashU { get; set; }

	[JsonProperty("InitType")]
	[XmlAttribute(AttributeName = "InitType")]
	public string ExportInitType
	{
		get
		{
			return EnumsHelper.GetDescription(_exportInitType);
		}
		set
		{
			_exportInitType = EnumsHelper.EnumConverter<ExportInitType>(value);
		}
	}

	[JsonProperty("UserAd")]
	[XmlAttribute(AttributeName = "UserAd")]
	public string UserAd
	{
		get
		{
			if (_adUser)
			{
				return "Yes";
			}
			return null;
		}
		set
		{
			if (value == "Yes")
			{
				_adUser = true;
			}
		}
	}

	[JsonProperty("QueuedTime")]
	[XmlAttribute(AttributeName = "QueuedTime")]
	public string QueuedTime
	{
		get
		{
			if (_queuedOnTime == InvalidDateTime)
			{
				return null;
			}
			return _queuedOnTime.ToMilisecString();
		}
		set
		{
			_queuedOnTime = TimeConverter.FromString(value);
		}
	}

	[JsonProperty("CompletedTime")]
	[XmlAttribute(AttributeName = "CompletedTime")]
	public string CompletedTime
	{
		get
		{
			if (_completedTime == InvalidDateTime)
			{
				return null;
			}
			return _completedTime.ToMilisecString();
		}
		set
		{
			_completedTime = TimeConverter.FromString(value);
		}
	}

	[JsonProperty("Size")]
	[XmlAttribute(AttributeName = "Size")]
	public long SizeBytes { get; set; }

	[JsonProperty("Items")]
	[XmlArray("Items")]
	[XmlArrayItem("Camera")]
	public List<CameraRelatedIds> Cameras { get; set; }

	[JsonProperty("KeepOriginals")]
	[XmlAttribute(AttributeName = "KeepOriginals")]
	public bool KeepOriginals { get; set; }

	[JsonProperty("IncludeAudio")]
	[XmlAttribute(AttributeName = "IncludeAudio")]
	public bool IncludeAudio { get; set; }

	[JsonProperty("InvestigationId")]
	[XmlAttribute(AttributeName = "InvestigationId")]
	public Guid InvestigationId { get; set; }

	[JsonProperty("ReExportFromId")]
	[XmlAttribute(AttributeName = "ReExportFromId")]
	public Guid ReExportFromId { get; set; }

	[JsonProperty("ServerId")]
	[XmlAttribute(AttributeName = "ServerId")]
	public Guid ProcessingServerId { get; set; }

	[JsonProperty("SubExportIds")]
	[XmlArray("SubExportIds")]
	[XmlArrayItem("SubExportId")]
	public List<Guid> SubExportIds { get; set; }

	public DateTime GetStartTime()
	{
		return _startTime;
	}

	public void SetStartTime(DateTime startTime)
	{
		_startTime = startTime;
	}

	public DateTime GetEndTime()
	{
		return _endTime;
	}

	public void SetEndTime(DateTime endTime)
	{
		_endTime = endTime;
	}

	public ExportType GetExportType()
	{
		return _exportType;
	}

	public void SetExportType(ExportType type)
	{
		_exportType = type;
	}

	public ExportInitType GetExportInitType()
	{
		return _exportInitType;
	}

	public void SetExportType(ExportInitType type)
	{
		_exportInitType = type;
	}

	public bool GetUserAd()
	{
		return _adUser;
	}

	public void SetUserAd(bool value)
	{
		_adUser = value;
	}

	public DateTime GetQueuedTime()
	{
		return _queuedOnTime;
	}

	public void SetQueuedTime(DateTime startTime)
	{
		_queuedOnTime = startTime;
	}

	public DateTime GetCompletedTime()
	{
		return _completedTime;
	}

	public void SetCompletedTime(DateTime startTime)
	{
		_completedTime = startTime;
	}

	public Export()
	{
		InternalInit();
	}

	public Export(IExportEntry entry)
	{
		InternalInit();
		InitFrom(entry);
	}

	private void InternalInit()
	{
		_startTime = (_endTime = (_queuedOnTime = (_completedTime = InvalidDateTime)));
		Cameras = new List<CameraRelatedIds>();
	}

	public void InitFrom(IExportEntry entry)
	{
		ExportId = entry.Params.ExportId;
		ExportName = entry.Params.ExportName;
		FileName = entry.FileName;
		if (entry.Params.Cameras != null)
		{
			Cameras.AddRange(entry.Params.Cameras);
		}
		_exportType = entry.Params.Type;
		_startTime = entry.Params.StartTime;
		if (entry.Params.CheckForMask(ExportParamsHelper.ParamMask.EndTime))
		{
			_endTime = entry.Params.EndTime;
		}
		if (entry.Params.CheckForMask(ExportParamsHelper.ParamMask.CodecName))
		{
			CodecName = entry.Params.CodecName;
		}
		if (entry.Params.CheckForMask(ExportParamsHelper.ParamMask.KeepOriginals))
		{
			KeepOriginals = entry.Params.KeepOriginals;
		}
		if (entry.Params.CheckForMask(ExportParamsHelper.ParamMask.InvestigationId))
		{
			InvestigationId = entry.Params.InvestigationId;
		}
		if (entry.Params.CheckForMask(ExportParamsHelper.ParamMask.ReExportFromId))
		{
			ReExportFromId = entry.Params.ReExportFromId;
		}
		if (entry.Params.CheckForMask(ExportParamsHelper.ParamMask.SubExportIds))
		{
			SubExportIds = new List<Guid>(entry.Params.SubExportIds);
		}
		if (entry.Params.CheckForMask(ExportParamsHelper.ParamMask.IncludeAudio))
		{
			IncludeAudio = entry.Params.IncludeAudio;
		}
		CameraName = entry.CameraName;
		State = entry.State;
		User = entry.Username;
		FullUserName = entry.FullUserName;
		UserSid = entry.UserSid;
		_queuedOnTime = entry.QueuedOnUTC;
		_completedTime = entry.CompletedOnUTC;
		SizeBytes = entry.SizeBytes;
		_exportInitType = entry.ExportInitType;
		ProcessingServerId = entry.ProcessingServerId;
	}

	public void CorrectCameras()
	{
		Cameras = Cameras.Distinct(new CameraRelatedIdsEqualityComparer()).ToList();
		List<CameraRelatedIds> second = Cameras.Where((CameraRelatedIds cameraItem) => cameraItem.RelatedMicrohponeId == Guid.Empty && cameraItem.RelatedSpeakerId == Guid.Empty && Cameras.Count((CameraRelatedIds cam) => cam.CameraId == cameraItem.CameraId) > 1).ToList();
		Cameras = Cameras.Except(second).ToList();
	}
}
