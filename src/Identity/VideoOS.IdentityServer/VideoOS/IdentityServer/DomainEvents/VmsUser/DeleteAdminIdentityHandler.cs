using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Repositories.Interfaces;

namespace VideoOS.IdentityServer.DomainEvents.VmsUser;

public class DeleteAdminIdentityHandler(IAdminIdentityRepository repository, ILogger<DeleteAdminIdentityHandler> logger) : INotificationHandler<UserDeletedEvent>
{
	public async Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
	{
		AdminIdentity adminIdentity = await repository.GetAdminIdentityAsync(notification.Id);
		if (adminIdentity != null)
		{
			logger.LogInformation("Deleting admin identity for UserId: {UserId} on Event: {eventName}", notification.Id, "UserDeletedEvent");
			repository.DeleteAdminIdentity(adminIdentity);
			await repository.SaveChangesAsync();
		}
	}
}
