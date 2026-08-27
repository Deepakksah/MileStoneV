namespace VideoOS.SqlUpdate.Core.Services;

public interface IDbService
{
	bool Execute();

	bool ShouldExecute();
}
