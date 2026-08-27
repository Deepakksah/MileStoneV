using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.ApplicationDb;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20191017190554_InitialIdentityServerApplicationDbMigration")]
public class InitialIdentityServerApplicationDbMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854").HasAnnotation("Relational:MaxIdentifierLength", 128).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
	}
}
