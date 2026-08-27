using VideoOS.IM.Server.Service.Annotations;

namespace VideoOS.IM.Server.Service.DALImplementation.Repository;

public interface IRepositoryService
{
	void Initialize([NotNull] string connectionString);
}
