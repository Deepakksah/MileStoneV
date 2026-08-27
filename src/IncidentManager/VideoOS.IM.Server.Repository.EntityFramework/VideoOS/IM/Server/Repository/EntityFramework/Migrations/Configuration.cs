using System.Data.Entity.Migrations;
using VideoOS.IM.Server.Repository.EntityFramework.Context;

namespace VideoOS.IM.Server.Repository.EntityFramework.Migrations;

public sealed class Configuration : DbMigrationsConfiguration<IncidentManagerContext>
{
	public Configuration()
	{
		base.AutomaticMigrationsEnabled = false;
	}

	protected override void Seed(IncidentManagerContext context)
	{
	}
}
