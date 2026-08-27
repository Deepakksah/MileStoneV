using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace VideoOS.IM.Server.Repository.EntityFramework;

public class IncidentManagerDbConfiguration : DbConfiguration
{
	public IncidentManagerDbConfiguration()
	{
		SetProviderServices("Microsoft.Data.SqlClient", SqlProviderServices.Instance);
		SetExecutionStrategy("Microsoft.Data.SqlClient", () => new SqlAzureExecutionStrategy());
	}
}
