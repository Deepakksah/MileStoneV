using VideoOS.IM.Server.Repository.EntityFramework.Setup;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Service.DALImplementation.Repository;

public sealed class RepositoryEntityFrameworkService : IRepositoryService
{
	[NotNull]
	private IInitializeRepository InitializeRepository { get; }

	public RepositoryEntityFrameworkService()
	{
		InitializeRepository = new InitializeRepository();
	}

	public void Initialize([NotNull] string connectionString)
	{
		connectionString.NullCheck("", "Initialize", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\DALImplementation\\Repository\\RepositoryEntityFrameworkService.cs", 20);
		InitializeRepository.Initialize(connectionString);
	}
}
