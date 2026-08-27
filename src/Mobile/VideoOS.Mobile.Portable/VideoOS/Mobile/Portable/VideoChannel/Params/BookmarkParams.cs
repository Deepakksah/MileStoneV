using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class BookmarkParams
{
	private uint _paramMask;

	private Guid _bookmarkId;

	private Guid _videoId;

	private Guid _cameraId;

	private string _name;

	private string _description;

	private DateTime _time;

	private DateTime _startTime;

	private DateTime _endTime;

	private int _count;

	private bool _myBookmarks;

	private string _searchCameraIds;

	private string _keyword;

	private string _reference;

	public Guid BookmarkId
	{
		get
		{
			return _bookmarkId;
		}
		set
		{
			_bookmarkId = value;
			_paramMask |= 1u;
		}
	}

	public Guid VideoId
	{
		get
		{
			return _videoId;
		}
		set
		{
			_videoId = value;
			_paramMask |= 2u;
		}
	}

	public Guid CameraId
	{
		get
		{
			return _cameraId;
		}
		set
		{
			_cameraId = value;
			_paramMask |= 4u;
		}
	}

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
			_paramMask |= 8u;
		}
	}

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value;
			_paramMask |= 16u;
		}
	}

	public DateTime Time
	{
		get
		{
			return _time;
		}
		set
		{
			_time = value;
			_paramMask |= 32u;
		}
	}

	public DateTime StartTime
	{
		get
		{
			return _startTime;
		}
		set
		{
			_startTime = value;
			_paramMask |= 64u;
		}
	}

	public DateTime EndTime
	{
		get
		{
			return _endTime;
		}
		set
		{
			_endTime = value;
			_paramMask |= 128u;
		}
	}

	public int Count
	{
		get
		{
			return _count;
		}
		set
		{
			_count = value;
			_paramMask |= 256u;
		}
	}

	public bool MyBookmarks
	{
		get
		{
			return _myBookmarks;
		}
		set
		{
			_myBookmarks = value;
			_paramMask |= 512u;
		}
	}

	public string SearchCameraIds
	{
		get
		{
			return _searchCameraIds;
		}
		set
		{
			_searchCameraIds = value;
			_paramMask |= 1024u;
		}
	}

	public string Keyword
	{
		get
		{
			return _keyword;
		}
		set
		{
			_keyword = value;
			_paramMask |= 2048u;
		}
	}

	public string Reference
	{
		get
		{
			return _reference;
		}
		set
		{
			_reference = value;
			_paramMask |= 4096u;
		}
	}

	public uint ParamMask => _paramMask;

	public BookmarkParams()
	{
		InternalInit();
	}

	public BookmarkParams(Command tCommand, uint unMaskToCheck = 8191u)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_videoId = Guid.Empty;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, BookmarkParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public BookmarkParams InitFromCommand(Command tCommand, uint unMaskToCheck = 8191u)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "BookmarkId", ref _bookmarkId, BookmarkParamsHelper.ParamMask.BookmarkId, ParamsHelper.GuidConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "VideoId", ref _videoId, BookmarkParamsHelper.ParamMask.VideoId, ParamsHelper.GuidConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "CameraId", ref _cameraId, BookmarkParamsHelper.ParamMask.CameraId, ParamsHelper.GuidConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Name", ref _name, BookmarkParamsHelper.ParamMask.Name, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Description", ref _description, BookmarkParamsHelper.ParamMask.Description, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Time", ref _time, BookmarkParamsHelper.ParamMask.Time, TimeConverter.FromStringUtc, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "StartTime", ref _startTime, BookmarkParamsHelper.ParamMask.StartTime, TimeConverter.FromStringUtc, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "EndTime", ref _endTime, BookmarkParamsHelper.ParamMask.EndTime, TimeConverter.FromStringUtc, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Count", ref _count, BookmarkParamsHelper.ParamMask.Count, Convert.ToInt32, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "MyBookmarks", ref _myBookmarks, BookmarkParamsHelper.ParamMask.MyBookmarks, ParamsHelper.BoolConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "SearchCameraIds", ref _searchCameraIds, BookmarkParamsHelper.ParamMask.SearchCameraIds, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Keyword", ref _keyword, BookmarkParamsHelper.ParamMask.Keyword, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Reference", ref _reference, BookmarkParamsHelper.ParamMask.Reference, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "BookmarkParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, BookmarkParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public BookmarkParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "BookmarkId", _bookmarkId, BookmarkParamsHelper.ParamMask.BookmarkId);
			ConvertParameterTo(dictionary, "VideoId", _videoId, BookmarkParamsHelper.ParamMask.VideoId);
			ConvertParameterTo(dictionary, "CameraId", _cameraId, BookmarkParamsHelper.ParamMask.CameraId);
			ConvertParameterTo(dictionary, "Name", _name, BookmarkParamsHelper.ParamMask.Name);
			ConvertParameterTo(dictionary, "Description", _description, BookmarkParamsHelper.ParamMask.Description);
			ConvertParameterTo(dictionary, "Time", _time, BookmarkParamsHelper.ParamMask.Time, TimeConverter.ToMilisecString);
			ConvertParameterTo(dictionary, "StartTime", _startTime, BookmarkParamsHelper.ParamMask.StartTime, TimeConverter.ToMilisecString);
			ConvertParameterTo(dictionary, "EndTime", _endTime, BookmarkParamsHelper.ParamMask.EndTime, TimeConverter.ToMilisecString);
			ConvertParameterTo(dictionary, "Count", _count, BookmarkParamsHelper.ParamMask.Count);
			ConvertParameterTo(dictionary, "MyBookmarks", _myBookmarks, BookmarkParamsHelper.ParamMask.MyBookmarks, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "SearchCameraIds", _searchCameraIds, BookmarkParamsHelper.ParamMask.SearchCameraIds);
			ConvertParameterTo(dictionary, "Keyword", _keyword, BookmarkParamsHelper.ParamMask.Keyword);
			ConvertParameterTo(dictionary, "Reference", _reference, BookmarkParamsHelper.ParamMask.Reference);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "BookmarkParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public bool CheckForMask(BookmarkParamsHelper.ParamMask tMask)
	{
		return (ParamMask & (uint)tMask) == (uint)tMask;
	}
}
