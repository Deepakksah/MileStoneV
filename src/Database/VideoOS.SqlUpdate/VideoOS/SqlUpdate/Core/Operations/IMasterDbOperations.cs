namespace VideoOS.SqlUpdate.Core.Operations;

public interface IMasterDbOperations
{
	void CreateDb(string connectionString);

	bool DropDb(string connectionString);

	int GetCompatibilityLevel(string connectionString);
}
