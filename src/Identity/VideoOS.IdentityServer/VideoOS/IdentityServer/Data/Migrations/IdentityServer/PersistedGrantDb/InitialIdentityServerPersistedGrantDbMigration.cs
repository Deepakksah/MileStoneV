using System;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.PersistedGrantDb;

[DbContext(typeof(PersistedGrantDbContext))]
[Migration("20191017180132_InitialIdentityServerPersistedGrantDbMigration")]
public class InitialIdentityServerPersistedGrantDbMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable("PersistedGrants", delegate(ColumnsBuilder table)
		{
			int? maxLength = 200;
			OperationBuilder<AddColumnOperation> key = table.Column<string>(null, null, maxLength);
			maxLength = 50;
			OperationBuilder<AddColumnOperation> type = table.Column<string>(null, null, maxLength);
			maxLength = 200;
			OperationBuilder<AddColumnOperation> subjectId = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			maxLength = 200;
			OperationBuilder<AddColumnOperation> clientId = table.Column<string>(null, null, maxLength);
			OperationBuilder<AddColumnOperation> creationTime = table.Column<DateTime>();
			OperationBuilder<AddColumnOperation> expiration = table.Column<DateTime>(null, null, null, rowVersion: false, null, nullable: true);
			maxLength = 50000;
			return new
			{
				Key = key,
				Type = type,
				SubjectId = subjectId,
				ClientId = clientId,
				CreationTime = creationTime,
				Expiration = expiration,
				Data = table.Column<string>(null, null, maxLength)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_PersistedGrants", x => x.Key);
		});
		migrationBuilder.CreateIndex("IX_PersistedGrants_SubjectId_ClientId_Type", "PersistedGrants", new string[3] { "SubjectId", "ClientId", "Type" });
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("PersistedGrants");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854").HasAnnotation("Relational:MaxIdentifierLength", 128).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.PersistedGrant", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("Key").HasMaxLength(200);
			b.Property<string>("ClientId").IsRequired().HasMaxLength(200);
			b.Property<DateTime>("CreationTime");
			b.Property<string>("Data").IsRequired().HasMaxLength(50000);
			b.Property<DateTime?>("Expiration");
			b.Property<string>("SubjectId").HasMaxLength(200);
			b.Property<string>("Type").IsRequired().HasMaxLength(50);
			b.HasKey("Key");
			b.HasIndex("SubjectId", "ClientId", "Type");
			b.ToTable("PersistedGrants");
		});
	}
}
