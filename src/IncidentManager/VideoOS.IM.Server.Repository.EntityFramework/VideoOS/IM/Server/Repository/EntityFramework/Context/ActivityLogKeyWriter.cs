using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Newtonsoft.Json;
using VideoOS.IM.Common.GlobalDefinitions.Configuration;
using VideoOS.IM.Common.GlobalDefinitions.Localization;
using VideoOS.IM.Common.GlobalDefinitions.Utilities;
using VideoOS.IM.Server.RepositoryModel.Annotations;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentActivityLog;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentCall;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentComment;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentSequence;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Repository.EntityFramework.Context;

public sealed class ActivityLogKeyWriter
{
	public enum EntityLogTypeEnum
	{
		Unknown,
		Call,
		Comment,
		Incident,
		Sequence
	}

	public enum EntityLogActionEnum
	{
		Unknown,
		Created,
		Edited,
		Deleted,
		NewLine,
		Opened,
		Printed,
		SentToExport,
		Added,
		Removed
	}

	private static readonly string LogEntryToLocalizePrefix = Constants.Translations.TranslationPrefix + "ActivityLog";

	private readonly RepoIncidentActivityLog _activityLog;

	private readonly IEnumerable<DbEntityEntry> _entities;

	private readonly EntityLogTypeEnum _entityLogType;

	private readonly EntityLogActionEnum _entityLogAction;

	private readonly IncidentProperties _incidentProperties;

	public ActivityLogKeyWriter([NotNull] RepoIncidentActivityLog repoIncidentActivityLog, [NotNull] List<DbEntityEntry> entities, [NotNull] IncidentProperties incidentProperties)
	{
		repoIncidentActivityLog.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Repository.EntityFramework\\Context\\ActivityLogKeyWriter.cs", 36);
		entities.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Repository.EntityFramework\\Context\\ActivityLogKeyWriter.cs", 37);
		incidentProperties.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Repository.EntityFramework\\Context\\ActivityLogKeyWriter.cs", 38);
		_activityLog = repoIncidentActivityLog;
		_entities = entities;
		_incidentProperties = incidentProperties;
		_entityLogType = GetEntityLogTypeEnum();
		_entityLogAction = GetEntityActionEnum();
		if (_entityLogAction == EntityLogActionEnum.Unknown)
		{
			throw new ArgumentException($"Entity action cannot be logged: {_entityLogType}: {_entityLogAction}", _activityLog.tmpNoDbTracker_entityState.ToString());
		}
	}

	public string GetDescription()
	{
		return JsonConvert.SerializeObject(GetDescriptionWrapper());
	}

	private LocalizableDescriptionWrapper GetDescriptionWrapper()
	{
		return new LocalizableDescriptionWrapper(GetKey(), GetParameters(), GetMultilineWrapperCollection());
	}

	private string GetKey()
	{
		return $"{LogEntryToLocalizePrefix}_{_entityLogType}_{_entityLogAction}";
	}

	private EntityLogTypeEnum GetEntityLogTypeEnum()
	{
		string text = ObjectContext.GetObjectType(typeof(RepoIncident)).ToString();
		string text2 = ObjectContext.GetObjectType(typeof(RepoIncidentCall)).ToString();
		string text3 = ObjectContext.GetObjectType(typeof(RepoIncidentSequence)).ToString();
		string text4 = ObjectContext.GetObjectType(typeof(RepoIncidentComment)).ToString();
		if (_activityLog.EntityType == text)
		{
			return EntityLogTypeEnum.Incident;
		}
		if (_activityLog.EntityType == text2)
		{
			return EntityLogTypeEnum.Call;
		}
		if (_activityLog.EntityType == text3)
		{
			return EntityLogTypeEnum.Sequence;
		}
		if (_activityLog.EntityType == text4)
		{
			return EntityLogTypeEnum.Comment;
		}
		return EntityLogTypeEnum.Unknown;
	}

	private EntityLogActionEnum GetEntityActionEnum()
	{
		string text = _activityLog.tmpNoDbTracker_entityState.ToString();
		if (text == EntityState.Added.ToString())
		{
			if (_entityLogType == EntityLogTypeEnum.Sequence)
			{
				return EntityLogActionEnum.Added;
			}
			return EntityLogActionEnum.Created;
		}
		if (text == EntityState.Modified.ToString())
		{
			if (_entityLogType == EntityLogTypeEnum.Incident && _activityLog.FlagNoDb_UserOpenedIncident)
			{
				return EntityLogActionEnum.Opened;
			}
			if (_entityLogType == EntityLogTypeEnum.Sequence)
			{
				DbEntityEntry dbEntryEntityByBaseType = GetDbEntryEntityByBaseType<RepoIncidentSequence>();
				if (!object.Equals(dbEntryEntityByBaseType.CurrentValues["IncidentId"], dbEntryEntityByBaseType.OriginalValues["IncidentId"]))
				{
					return EntityLogActionEnum.Added;
				}
			}
			return EntityLogActionEnum.Edited;
		}
		if (text == EntityState.Deleted.ToString())
		{
			if (_entityLogType == EntityLogTypeEnum.Sequence)
			{
				return EntityLogActionEnum.Removed;
			}
			return EntityLogActionEnum.Deleted;
		}
		if (text == EntityState.Unchanged.ToString() && _entityLogType == EntityLogTypeEnum.Incident && _activityLog.FlagNoDb_UserOpenedIncident)
		{
			return EntityLogActionEnum.Opened;
		}
		return EntityLogActionEnum.Unknown;
	}

	private IEnumerable<LocalizableDescriptionParameter> GetParameters()
	{
		return _entityLogType switch
		{
			EntityLogTypeEnum.Call => GetCallParameters(), 
			EntityLogTypeEnum.Comment => GetCommentParameters(), 
			EntityLogTypeEnum.Sequence => GetSequenceParameters(), 
			_ => new List<LocalizableDescriptionParameter>(), 
		};
	}

	private IEnumerable<LocalizableDescriptionParameter> GetCallParameters()
	{
		RepoIncidentCall entity = GetEntity<RepoIncidentCall>();
		if (entity != null)
		{
			return entity.GetLoggingParameters();
		}
		return new List<LocalizableDescriptionParameter>();
	}

	private IEnumerable<LocalizableDescriptionParameter> GetCommentParameters()
	{
		RepoIncidentComment entity = GetEntity<RepoIncidentComment>();
		if (entity != null)
		{
			return entity.GetLoggingParameters();
		}
		return new List<LocalizableDescriptionParameter>();
	}

	private IEnumerable<LocalizableDescriptionParameter> GetSequenceParameters()
	{
		RepoIncidentSequence entity = GetEntity<RepoIncidentSequence>();
		if (entity != null)
		{
			return entity.GetLoggingParameters();
		}
		return new List<LocalizableDescriptionParameter>();
	}

	private DbEntityEntry GetDbEntryEntityByBaseType<T>()
	{
		return _entities.Where((DbEntityEntry e) => e.Entity.GetType().BaseType == typeof(T) || e.Entity.GetType().FullName.Contains(typeof(T).FullName)).FirstOrDefault();
	}

	private T GetEntity<T>()
	{
		return (T)(GetDbEntryEntityByBaseType<T>()?.Entity);
	}

	private IEnumerable<LocalizableDescriptionWrapper> GetMultilineWrapperCollection()
	{
		if (_entityLogType == EntityLogTypeEnum.Incident && _entityLogAction == EntityLogActionEnum.Edited)
		{
			RepoIncident entity = GetEntity<RepoIncident>();
			RepoIncident oldIncident = GetOldIncident(GetDbEntryEntityByBaseType<RepoIncident>().OriginalValues);
			IncidentProperties incidentProperties = (oldIncident.IncidentProperties = _incidentProperties);
			entity.IncidentProperties = incidentProperties;
			return entity.GetChildEntries(oldIncident);
		}
		return Enumerable.Empty<LocalizableDescriptionWrapper>();
	}

	private RepoIncident GetOldIncident(DbPropertyValues originalValues)
	{
		return new RepoIncident
		{
			Description = (string)originalValues["Description"],
			TypeId = (long?)originalValues["TypeId"],
			StatusId = (long?)originalValues["StatusId"],
			ExtPropLong1 = (long)originalValues["ExtPropLong1"],
			ExtPropLong2 = (long)originalValues["ExtPropLong2"],
			ExtPropLong3 = (long)originalValues["ExtPropLong3"],
			ExtPropString1 = (string)originalValues["ExtPropString1"],
			ExtPropString2 = (string)originalValues["ExtPropString2"],
			ExtPropString3 = (string)originalValues["ExtPropString3"],
			ExtPropBool1 = (bool)originalValues["ExtPropBool1"],
			ExtPropBool2 = (bool)originalValues["ExtPropBool2"],
			ExtPropBool3 = (bool)originalValues["ExtPropBool3"],
			ExtPropDecimal1 = (decimal)originalValues["ExtPropDecimal1"],
			ExtPropDecimal2 = (decimal)originalValues["ExtPropDecimal2"],
			ExtPropDecimal3 = (decimal)originalValues["ExtPropDecimal3"],
			ExtPropDateTime1 = (DateTime?)originalValues["ExtPropDateTime1"],
			ExtPropDateTime1_Configuration = (string)originalValues["ExtPropDateTime1_Configuration"],
			ExtPropDateTime2 = (DateTime?)originalValues["ExtPropDateTime2"],
			ExtPropDateTime2_Configuration = (string)originalValues["ExtPropDateTime2_Configuration"],
			ExtPropDateTime3 = (DateTime?)originalValues["ExtPropDateTime3"],
			ExtPropDateTime3_Configuration = (string)originalValues["ExtPropDateTime3_Configuration"],
			ExtPropJSon1 = (string)originalValues["ExtPropJSon1"],
			ExtPropJSon1_Configuration = (string)originalValues["ExtPropJSon1_Configuration"],
			ExtPropJSon2 = (string)originalValues["ExtPropJSon2"],
			ExtPropJSon2_Configuration = (string)originalValues["ExtPropJSon2_Configuration"],
			ExtPropJSon3 = (string)originalValues["ExtPropJSon3"],
			ExtPropJSon3_Configuration = (string)originalValues["ExtPropJSon3_Configuration"],
			ExtendedPropsGroup1_JSonConfiguration = (string)originalValues["ExtendedPropsGroup1_JSonConfiguration"],
			ExtendedPropsGroup2_JSonConfiguration = (string)originalValues["ExtendedPropsGroup2_JSonConfiguration"],
			Category1Id = (long?)originalValues["Category1Id"],
			Category2Id = (long?)originalValues["Category2Id"],
			Category3Id = (long?)originalValues["Category3Id"],
			Category4Id = (long?)originalValues["Category4Id"],
			Category5Id = (long?)originalValues["Category5Id"]
		};
	}
}
