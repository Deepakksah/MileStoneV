using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VideoOS.IM.Common.GlobalDefinitions.Configuration;
using VideoOS.IM.Server.Logger;
using VideoOS.IM.Server.Repository.EntityFramework.Migrations;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.DraftSequence;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.DynamicalDataTypesAndOptions;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.GlobalSettings;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentActivityLog;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentCall;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentComment;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentSequence;

namespace VideoOS.IM.Server.Repository.EntityFramework.Context;

[DbConfigurationType(typeof(MicrosoftSqlDbConfiguration))]
public class IncidentManagerContext : DbContext
{
	private static Func<DbEntityEntry, bool> IsAddedOrUpdated = (DbEntityEntry d) => d.State == EntityState.Added || d.State == EntityState.Modified;

	private static Func<DbEntityEntry, bool> IsDeleted = (DbEntityEntry d) => d.State == EntityState.Deleted;

	private static Func<DbEntityEntry, bool> IsRepoIncident = (DbEntityEntry d) => d.Entity.GetType().IsAssignableFrom(typeof(RepoIncident)) || d.Entity.GetType().BaseType.IsAssignableFrom(typeof(RepoIncident));

	private static Func<DbEntityEntry, bool> IsRepoIncidentDeleted = (DbEntityEntry d) => IsDeleted(d) && IsRepoIncident(d);

	public static bool EnableGlobalIncidentDbTracker { get; set; } = true;

	public DbSet<RepoDraftSequence> DraftSequence { get; set; }

	public DbSet<RepoDynamicalDataTypesAndOptions> DynamicalDataTypesAndOptions { get; set; }

	public DbSet<RepoIncident> Incidents { get; set; }

	public DbSet<RepoIncidentCall> IncidentCall { get; set; }

	public DbSet<RepoIncidentSequence> IncidentSequence { get; set; }

	public DbSet<RepoIncidentComment> IncidentComment { get; set; }

	public DbSet<RepoIncidentActivityLog> IncidentActivityLog { get; set; }

	public DbSet<RepoGlobalSettings> GlobalSettings { get; set; }

	public void InitializeDatabase()
	{
		Database.SetInitializer(new MigrateDatabaseToLatestVersion<IncidentManagerContext, Configuration>(useSuppliedContext: true));
		base.Database.Initialize(force: true);
	}

	public IncidentManagerContext()
	{
	}

	public IncidentManagerContext(string inConnectionString)
		: base(inConnectionString)
	{
	}

	protected override void OnModelCreating(DbModelBuilder modelBuilder)
	{
		modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		modelBuilder.Entity<RepoDraftSequence>().HasMany((RepoDraftSequence o) => o.SequenceList).WithOptional((RepoIncidentSequence o) => o.DraftSequence)
			.HasForeignKey((RepoIncidentSequence o) => o.DraftSequenceId)
			.WillCascadeOnDelete(value: false);
		modelBuilder.Entity<RepoDynamicalDataTypesAndOptions>().HasMany((RepoDynamicalDataTypesAndOptions o) => o.UsedInIncident_AsType).WithOptional((RepoIncident o) => o.Type)
			.HasForeignKey((RepoIncident o) => o.TypeId)
			.WillCascadeOnDelete(value: false);
		modelBuilder.Entity<RepoDynamicalDataTypesAndOptions>().HasMany((RepoDynamicalDataTypesAndOptions o) => o.UsedInIncident_AsStatus).WithOptional((RepoIncident o) => o.Status)
			.HasForeignKey((RepoIncident o) => o.StatusId)
			.WillCascadeOnDelete(value: false);
		modelBuilder.Entity<RepoDynamicalDataTypesAndOptions>().HasMany((RepoDynamicalDataTypesAndOptions o) => o.UsedInIncident_AsCategory1).WithOptional((RepoIncident o) => o.Category1)
			.HasForeignKey((RepoIncident o) => o.Category1Id)
			.WillCascadeOnDelete(value: false);
		modelBuilder.Entity<RepoDynamicalDataTypesAndOptions>().HasMany((RepoDynamicalDataTypesAndOptions o) => o.UsedInIncident_AsCategory2).WithOptional((RepoIncident o) => o.Category2)
			.HasForeignKey((RepoIncident o) => o.Category2Id)
			.WillCascadeOnDelete(value: false);
		modelBuilder.Entity<RepoDynamicalDataTypesAndOptions>().HasMany((RepoDynamicalDataTypesAndOptions o) => o.UsedInIncident_AsCategory3).WithOptional((RepoIncident o) => o.Category3)
			.HasForeignKey((RepoIncident o) => o.Category3Id)
			.WillCascadeOnDelete(value: false);
		modelBuilder.Entity<RepoDynamicalDataTypesAndOptions>().HasMany((RepoDynamicalDataTypesAndOptions o) => o.UsedInIncident_AsCategory4).WithOptional((RepoIncident o) => o.Category4)
			.HasForeignKey((RepoIncident o) => o.Category4Id)
			.WillCascadeOnDelete(value: false);
		modelBuilder.Entity<RepoDynamicalDataTypesAndOptions>().HasMany((RepoDynamicalDataTypesAndOptions o) => o.UsedInIncident_AsCategory5).WithOptional((RepoIncident o) => o.Category5)
			.HasForeignKey((RepoIncident o) => o.Category5Id)
			.WillCascadeOnDelete(value: false);
		modelBuilder.Entity<RepoIncident>().HasMany((RepoIncident o) => o.IncidentCommentsList).WithRequired((RepoIncidentComment o) => o.Incident)
			.HasForeignKey((RepoIncidentComment o) => o.IncidentId)
			.WillCascadeOnDelete(value: true);
		modelBuilder.Entity<RepoIncident>().HasMany((RepoIncident o) => o.IncidentCallsList).WithRequired((RepoIncidentCall o) => o.Incident)
			.HasForeignKey((RepoIncidentCall o) => o.IncidentId)
			.WillCascadeOnDelete(value: true);
		modelBuilder.Entity<RepoIncident>().HasMany((RepoIncident o) => o.SequencesList).WithOptional((RepoIncidentSequence o) => o.Incident)
			.HasForeignKey((RepoIncidentSequence o) => o.IncidentId)
			.WillCascadeOnDelete(value: true);
		modelBuilder.Entity<RepoIncident>().HasMany((RepoIncident o) => o.ActivityLogsList).WithRequired((RepoIncidentActivityLog o) => o.Incident)
			.HasForeignKey((RepoIncidentActivityLog o) => o.IncidentId)
			.WillCascadeOnDelete(value: true);
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
	{
		AddCreatedModifiedInfo();
		List<RepoIncidentActivityLog> addedActivityLogs = AddIncidentActivityLogInfo();
		int toRet = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (addedActivityLogs != null && addedActivityLogs.Any() && UpdateIncidentActivityLogInfoForInsertedActivityLogs(addedActivityLogs))
		{
			await base.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		return toRet;
	}

	private void AddCreatedModifiedInfo()
	{
		foreach (DbEntityEntry item in from x in base.ChangeTracker.Entries()
			where x.Entity is ARepoBaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified)
			select x)
		{
			if (item.State == EntityState.Added)
			{
				((ARepoBaseEntity)item.Entity).TimeCreated = DateTime.UtcNow;
				if (string.IsNullOrWhiteSpace(((ARepoBaseEntity)item.Entity).CreatedUserId))
				{
					((ARepoBaseEntity)item.Entity).CreatedUserId = ((ARepoBaseEntity)item.Entity).ModifiedUserId;
				}
				if (string.IsNullOrWhiteSpace(((ARepoBaseEntity)item.Entity).CreatedUserName))
				{
					((ARepoBaseEntity)item.Entity).CreatedUserName = ((ARepoBaseEntity)item.Entity).ModifiedUserName;
				}
			}
			((ARepoBaseEntity)item.Entity).TimeModified = DateTime.UtcNow;
		}
	}

	private List<RepoIncidentActivityLog> AddIncidentActivityLogInfo()
	{
		List<RepoIncidentActivityLog> list = new List<RepoIncidentActivityLog>();
		try
		{
			if (EnableGlobalIncidentDbTracker)
			{
				List<DbEntityEntry> list2 = base.ChangeTracker.Entries().ToList();
				if (list2.Any(IsRepoIncidentDeleted))
				{
					return list;
				}
				List<DbEntityEntry> list3 = list2.Where((DbEntityEntry o) => IsAddedOrUpdated(o) || !IsRepoIncidentDeleted(o)).ToList();
				if (list3.Any())
				{
					foreach (DbEntityEntry item in list3)
					{
						if (item == null)
						{
							continue;
						}
						long? num = null;
						RepoIncident repoIncident = null;
						RepoIncidentCall repoIncidentCall = null;
						RepoIncidentSequence repoIncidentSequence = null;
						RepoIncidentComment repoIncidentComment = null;
						bool flag = false;
						if (item != null && item.Entity != null && (typeof(RepoIncident) == item.Entity.GetType() || typeof(RepoIncident) == item.Entity.GetType()?.BaseType))
						{
							repoIncident = item.Entity as RepoIncident;
							if (repoIncident != null)
							{
								num = repoIncident.Id;
								flag = true;
							}
						}
						if (item != null && item.Entity != null && (typeof(RepoIncidentCall) == item.Entity.GetType() || typeof(RepoIncidentCall) == item.Entity.GetType()?.BaseType) && item.Entity is RepoIncidentCall repoIncidentCall2)
						{
							num = repoIncidentCall2.IncidentId;
							flag = true;
						}
						if (item != null && item.Entity != null && (typeof(RepoIncidentSequence) == item.Entity.GetType() || typeof(RepoIncidentSequence) == item.Entity.GetType()?.BaseType) && item.Entity is RepoIncidentSequence { IncidentId: var incidentId })
						{
							num = incidentId ?? GetRelatedIncident()?.Id;
							flag = true;
						}
						if (item != null && item.Entity != null && (typeof(RepoIncidentComment) == item.Entity.GetType() || typeof(RepoIncidentComment) == item.Entity.GetType()?.BaseType) && item.Entity is RepoIncidentComment repoIncidentComment2)
						{
							num = repoIncidentComment2.IncidentId;
							flag = true;
						}
						if (!flag || !num.HasValue)
						{
							continue;
						}
						ObjectStateEntry objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(item.Entity);
						RepoIncidentActivityLog repoIncidentActivityLog = new RepoIncidentActivityLog
						{
							Description = "",
							EntityId = ConvertEntityKeyToString(objectStateEntry.EntityKey),
							EntityType = ObjectContext.GetObjectType(item.Entity.GetType()).ToString(),
							TimeCreated = DateTime.UtcNow,
							TimeModified = DateTime.UtcNow,
							TimeOfActivityLog = DateTime.UtcNow,
							IncidentId = num.Value,
							SystemString = "{regular-action}, {for-operators}"
						};
						if (item.Entity is ARepoBaseEntity aRepoBaseEntity)
						{
							repoIncidentActivityLog.tmpNoDbTracker_entityState = item.State;
							repoIncidentActivityLog.tmpNoDbTracker_originalBaseObjectStateEntry = objectStateEntry;
							repoIncidentActivityLog.tmpNoDbTracker_originalBaseEntity = aRepoBaseEntity;
							repoIncidentActivityLog.CreatedUserId = aRepoBaseEntity.ModifiedUserId;
							repoIncidentActivityLog.ModifiedUserId = aRepoBaseEntity.ModifiedUserId;
							repoIncidentActivityLog.CreatedUserName = aRepoBaseEntity.ModifiedUserName;
							repoIncidentActivityLog.ModifiedUserName = aRepoBaseEntity.ModifiedUserName;
							repoIncidentActivityLog.FlagNoDb_UserOpenedIncident = aRepoBaseEntity.FlagNoDb_UserOpenedIncident;
							repoIncidentActivityLog.FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog = aRepoBaseEntity.FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog;
							if (repoIncidentActivityLog.FlagNoDb_UserOpenedIncident && repoIncidentActivityLog.FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog)
							{
								repoIncidentActivityLog.SystemString = "{open-view-action}, {for-system-super-admin}";
							}
							else if (!repoIncidentActivityLog.FlagNoDb_UserOpenedIncident && repoIncidentActivityLog.FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog)
							{
								repoIncidentActivityLog.SystemString = "{re-save-action}, {for-system-super-admin}";
							}
							else if (repoIncidentActivityLog.FlagNoDb_UserOpenedIncident && !repoIncidentActivityLog.FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog)
							{
								repoIncidentActivityLog.SystemString = "{open-view-action}, {for-operators}";
							}
						}
						Dictionary<string, object> dictionary = new Dictionary<string, object>();
						Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
						if (item.State == EntityState.Modified)
						{
							object entity = item.Entity;
							DbPropertyValues originalValues = Entry(entity).OriginalValues;
							DbPropertyValues currentValues = Entry(entity).CurrentValues;
							foreach (string propertyName in originalValues.PropertyNames)
							{
								object obj = originalValues[propertyName];
								object obj2 = currentValues[propertyName];
								if (!object.Equals(obj, obj2))
								{
									dictionary.Add(propertyName, obj);
									dictionary2.Add(propertyName, obj2);
								}
							}
						}
						else if (item.State == EntityState.Added)
						{
							object entity2 = item.Entity;
							DbPropertyValues currentValues2 = Entry(entity2).CurrentValues;
							foreach (string propertyName2 in currentValues2.PropertyNames)
							{
								object value = currentValues2[propertyName2];
								dictionary2.Add(propertyName2, value);
							}
						}
						else if (item.State == EntityState.Deleted)
						{
							object entity3 = item.Entity;
							DbPropertyValues originalValues2 = Entry(entity3).OriginalValues;
							foreach (string propertyName3 in originalValues2.PropertyNames)
							{
								object value2 = originalValues2[propertyName3];
								dictionary.Add(propertyName3, value2);
							}
						}
						string jsonOldValues = JsonConvert.SerializeObject(dictionary);
						string jsonNewValues = JsonConvert.SerializeObject(dictionary2);
						string text = "";
						if (repoIncident != null && dictionary != null && dictionary.Count > 0 && dictionary2 != null && dictionary2.Count > 0)
						{
							List<string> mainPropsList = new List<string> { "Id", "User", "Type", "TypeId", "Status", "StatusId", "Start", "End" };
							List<string> extendedPropsList = new List<string>
							{
								"Category1", "Action", "Category3", "Category4", "Category5", "Category1Id", "ActionId", "Category3Id", "Category4Id", "Category5Id",
								"ExtPropLong1", "ExtPropLong2", "ExtPropLong3", "ExtPropString1", "ExtPropString2", "ExtPropString3", "ExtPropBool1", "ExtPropBool2", "ExtPropBool3", "ExtPropDecimal1",
								"ExtPropDecimal2", "ExtPropDecimal3", "ExtPropDateTime1", "ExtPropDateTime2", "ExtPropDateTime3"
							};
							if (dictionary.Keys.Any((string o) => mainPropsList.Any((string value3) => o.Equals(value3, StringComparison.InvariantCultureIgnoreCase))))
							{
								text += " main details";
							}
							if (dictionary.Keys.Any((string o) => extendedPropsList.Any((string value3) => o.Equals(value3, StringComparison.InvariantCultureIgnoreCase))))
							{
								if (string.IsNullOrWhiteSpace(text))
								{
									text += " extended information";
								}
								else
								{
									text += " and extended information";
								}
							}
						}
						repoIncidentActivityLog.JsonOldValues = jsonOldValues;
						repoIncidentActivityLog.JsonNewValues = jsonNewValues;
						try
						{
							IncidentProperties incidentProperties = GetIncidentProperties();
							string description = new ActivityLogKeyWriter(repoIncidentActivityLog, list2, incidentProperties).GetDescription();
							if (!string.IsNullOrEmpty(description))
							{
								repoIncidentActivityLog.Description = description;
							}
							list.Add(repoIncidentActivityLog);
						}
						catch (ArgumentException arg)
						{
							LoggerProvider.Instance.Logger.LogError($"Error while logging activity: {arg}", repoIncidentActivityLog);
						}
					}
				}
			}
		}
		catch (Exception arg2)
		{
			LoggerProvider.Instance.Logger.LogError($"Error while logging activity: {arg2}");
		}
		try
		{
			if (list.Any())
			{
				foreach (RepoIncidentActivityLog item2 in list)
				{
					if (item2 != null)
					{
						IncidentActivityLog.Add(item2);
					}
				}
			}
		}
		catch (Exception arg3)
		{
			LoggerProvider.Instance.Logger.LogError($"Error while logging activity: {arg3}");
		}
		return list;
	}

	private bool UpdateIncidentActivityLogInfoForInsertedActivityLogs(List<RepoIncidentActivityLog> inActivityLogs)
	{
		try
		{
			bool result = false;
			if (inActivityLogs != null && inActivityLogs.Any())
			{
				foreach (RepoIncidentActivityLog inActivityLog in inActivityLogs)
				{
					if (inActivityLog == null)
					{
						continue;
					}
					try
					{
						if (inActivityLog.tmpNoDbTracker_entityState != EntityState.Added || inActivityLog.tmpNoDbTracker_originalBaseObjectStateEntry == null)
						{
							continue;
						}
						result = true;
						ObjectStateEntry tmpNoDbTracker_originalBaseObjectStateEntry = inActivityLog.tmpNoDbTracker_originalBaseObjectStateEntry;
						inActivityLog.EntityId = ConvertEntityKeyToString(tmpNoDbTracker_originalBaseObjectStateEntry.EntityKey);
						RepoIncidentActivityLog repoIncidentActivityLog = inActivityLog;
						try
						{
							IncidentProperties incidentProperties = GetIncidentProperties();
							string description = new ActivityLogKeyWriter(repoIncidentActivityLog, base.ChangeTracker.Entries().ToList(), incidentProperties).GetDescription();
							if (!string.IsNullOrEmpty(description))
							{
								repoIncidentActivityLog.Description = description;
							}
						}
						catch (ArgumentException arg)
						{
							LoggerProvider.Instance.Logger.LogError($"Error while logging activity: {arg}", repoIncidentActivityLog);
						}
					}
					catch (Exception arg2)
					{
						LoggerProvider.Instance.Logger.LogError($"Error while updating log activity: {arg2}");
					}
				}
			}
			return result;
		}
		catch (Exception arg3)
		{
			LoggerProvider.Instance.Logger.LogError($"Error while updating log activity: {arg3}");
		}
		return false;
	}

	private string ConvertEntityKeyToString(EntityKey inEk)
	{
		if (inEk != null && inEk.EntityKeyValues != null && inEk.EntityKeyValues.Length != 0)
		{
			return string.Join(", ", from x in inEk.EntityKeyValues
				where x != null
				select $"{x.Key}: {x.Value}");
		}
		return string.Empty;
	}

	private IncidentProperties GetIncidentProperties()
	{
		IncidentProperties incidentProperties = new IncidentProperties();
		RepoGlobalSettings repoGlobalSettings = GlobalSettings.FirstOrDefault((RepoGlobalSettings o) => o.GlobalSettingsIndentificator == "{incidentManager-incident-extendedInformationConfiguration}");
		if (repoGlobalSettings != null)
		{
			incidentProperties = JsonConvert.DeserializeObject<IncidentProperties>(repoGlobalSettings.ValueJSon);
			incidentProperties?.ReplacePropsWithObjectsFromList();
		}
		return incidentProperties;
	}

	private RepoIncident GetRelatedIncident()
	{
		return (from x in base.ChangeTracker.Entries()
			select x.Entity).OfType<RepoIncident>().FirstOrDefault();
	}
}
