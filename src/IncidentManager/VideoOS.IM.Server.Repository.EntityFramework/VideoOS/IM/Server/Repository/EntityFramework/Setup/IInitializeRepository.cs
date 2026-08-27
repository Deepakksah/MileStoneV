using VideoOS.IM.Server.Repository.EntityFramework.Properties;

namespace VideoOS.IM.Server.Repository.EntityFramework.Setup;

public interface IInitializeRepository
{
	void Initialize([NotNull] string connectionString);
}
