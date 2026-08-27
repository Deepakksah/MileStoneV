using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace VideoOS.IdentityServer.Data.Migrations.DataProtection.KeysDb;

[DbContext(typeof(DataProtectionKeysDbContext))]
[Migration("20220111134501_AddDataProtectionKeys")]
public class AddDataProtectionKeys : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable("DataProtectionKeys", (ColumnsBuilder table) => new
		{
			Id = table.Column<int>().Annotation("SqlServer:Identity", "1, 1"),
			FriendlyName = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			Xml = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true)
		}, null, table =>
		{
			table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
		});
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("DataProtectionKeys");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("ProductVersion", "3.1.5").HasAnnotation("Relational:MaxIdentifierLength", 128).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
		modelBuilder.Entity("Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.DataProtectionKey", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<string>("FriendlyName").HasColumnType("nvarchar(max)");
			b.Property<string>("Xml").HasColumnType("nvarchar(max)");
			b.HasKey("Id");
			b.ToTable("DataProtectionKeys");
		});
	}
}
