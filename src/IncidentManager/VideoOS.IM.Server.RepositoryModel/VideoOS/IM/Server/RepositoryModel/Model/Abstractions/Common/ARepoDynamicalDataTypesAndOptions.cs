using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.DynamicalDataTypesAndOptions;

namespace VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;

public abstract class ARepoDynamicalDataTypesAndOptions : ARepoBaseEntity, IRepoDynamicalDataTypesAndOptions, IRepoBaseEntity
{
	public int ValueInt { get; set; }

	public string ValueStr { get; set; }

	public decimal ValueDecimal { get; set; }

	public bool ValueBool { get; set; }

	public string Name { get; set; }

	public string Title { get; set; }

	public string Description { get; set; }

	public string ClassOfData { get; set; }

	public int ClassOfDataint { get; set; }

	public string CustomAction { get; set; }
}
