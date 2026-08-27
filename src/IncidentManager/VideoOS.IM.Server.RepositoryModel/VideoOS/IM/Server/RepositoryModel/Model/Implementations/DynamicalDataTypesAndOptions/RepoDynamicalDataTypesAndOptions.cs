using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;

namespace VideoOS.IM.Server.RepositoryModel.Model.Implementations.DynamicalDataTypesAndOptions;

public class RepoDynamicalDataTypesAndOptions : ARepoDynamicalDataTypesAndOptions
{
	public string TypeOfItemStr { get; set; }

	[NotMapped]
	public Type TypeOfItem { get; set; }

	[NotMapped]
	public int? NumberOfForeignReferences { get; set; }

	public string EntityScopeStr { get; set; }

	public string PropertyScopeStr { get; set; }

	public virtual List<RepoIncident> UsedInIncident_AsType { get; set; }

	public virtual List<RepoIncident> UsedInIncident_AsStatus { get; set; }

	public virtual List<RepoIncident> UsedInIncident_AsCategory1 { get; set; }

	public virtual List<RepoIncident> UsedInIncident_AsCategory2 { get; set; }

	public virtual List<RepoIncident> UsedInIncident_AsCategory3 { get; set; }

	public virtual List<RepoIncident> UsedInIncident_AsCategory4 { get; set; }

	public virtual List<RepoIncident> UsedInIncident_AsCategory5 { get; set; }
}
