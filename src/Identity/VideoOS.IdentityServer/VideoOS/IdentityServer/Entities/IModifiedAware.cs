using System;

namespace VideoOS.IdentityServer.Entities;

public interface IModifiedAware
{
	DateTime CreatedDate { get; set; }

	DateTime ModifiedDate { get; set; }
}
