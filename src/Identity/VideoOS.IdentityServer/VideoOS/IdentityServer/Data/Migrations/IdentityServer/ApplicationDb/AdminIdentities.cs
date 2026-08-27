using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.ApplicationDb;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20201215184343_AdminIdentities")]
public class AdminIdentities : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable("AdminIdentities", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:Identity", "1, 1");
			int? maxLength = 450;
			return new
			{
				Id = id,
				Identity = table.Column<string>(null, null, maxLength)
			};
		}, "dbo", table =>
		{
			table.PrimaryKey("PK_AdminIdentities", x => x.Id);
		});
		migrationBuilder.CreateIndex("IX_AdminIdentities_Identity", "AdminIdentities", "Identity", "dbo", unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("AdminIdentities", "dbo");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("dbo").HasAnnotation("ProductVersion", "3.1.5").HasAnnotation("Relational:MaxIdentifierLength", 128)
			.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.AdminIdentity", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<string>("Identity").IsRequired().HasColumnType("nvarchar(450)")
				.HasMaxLength(450);
			b.HasKey("Id");
			b.HasIndex("Identity").IsUnique();
			b.ToTable("AdminIdentities");
		});
	}
}
