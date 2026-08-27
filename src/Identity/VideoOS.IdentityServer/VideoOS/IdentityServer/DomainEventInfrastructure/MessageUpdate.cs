using System;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public record MessageUpdate(Guid Id, OutboxMessageStatus Status, string Error = null);
