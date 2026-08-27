using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.Common;

namespace VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;

public abstract class ARepoBaseEntity : IRepoBaseEntity
{
	public long Id { get; set; }

	public DateTime? TimeCreated { get; set; }

	public string CreatedUserId { get; set; }

	public string CreatedUserName { get; set; }

	public DateTime? TimeModified { get; set; }

	public string ModifiedUserId { get; set; }

	public string ModifiedUserName { get; set; }

	public string SystemString { get; set; }

	[NotMapped]
	public EntityState tmpNoDbTracker_entityState { get; set; }

	[NotMapped]
	public ObjectStateEntry tmpNoDbTracker_originalBaseObjectStateEntry { get; set; }

	[NotMapped]
	public ARepoBaseEntity tmpNoDbTracker_originalBaseEntity { get; set; }

	[NotMapped]
	public bool FlagNoDb_UserOpenedIncident { get; set; }

	[NotMapped]
	public bool FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog { get; set; }

	public override string ToString()
	{
		string text = "";
		string text2 = $"\tId: {Id}\n\tTimeCreated: {TimeCreated}\n\tCreatedUserId: {CreatedUserId}\n\tTimeModified: {TimeModified}\n\tModifiedUserId; {ModifiedUserId}\n";
		return text + text2;
	}
}
