using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VideoOS.IdentityServer.Data.Migrations.Identity.IdentityDb;

[DbContext(typeof(IdentityDbContextWrapper))]
public class IdentityDbContextWrapperModelSnapshot : ModelSnapshot
{
	protected override void BuildModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("ProductVersion", "8.0.4").HasAnnotation("Relational:MaxIdentifierLength", 128);
		modelBuilder.UseIdentityColumns(1L);
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<string>", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("Id").HasColumnType("nvarchar(450)");
			b.Property<string>("ConcurrencyStamp").IsConcurrencyToken().HasColumnType("nvarchar(max)");
			b.Property<string>("Name").HasMaxLength(256).HasColumnType("nvarchar(256)");
			b.Property<string>("NormalizedName").HasMaxLength(256).HasColumnType("nvarchar(256)");
			b.HasKey("Id");
			b.HasIndex("NormalizedName").IsUnique().HasDatabaseName("RoleNameIndex")
				.HasFilter("[NormalizedName] IS NOT NULL");
			b.ToTable("AspNetRoles", (string?)null);
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<string>("ClaimType").HasColumnType("nvarchar(max)");
			b.Property<string>("ClaimValue").HasColumnType("nvarchar(max)");
			b.Property<string>("RoleId").IsRequired().HasColumnType("nvarchar(450)");
			b.HasKey("Id");
			b.HasIndex("RoleId");
			b.ToTable("AspNetRoleClaims", (string?)null);
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("LoginProvider").HasColumnType("nvarchar(450)");
			b.Property<string>("ProviderKey").HasColumnType("nvarchar(450)");
			b.Property<string>("ProviderDisplayName").HasColumnType("nvarchar(max)");
			b.Property<string>("UserId").IsRequired().HasColumnType("nvarchar(450)");
			b.HasKey("LoginProvider", "ProviderKey");
			b.HasIndex("UserId");
			b.ToTable("AspNetUserLogins", (string?)null);
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("UserId").HasColumnType("nvarchar(450)");
			b.Property<string>("RoleId").HasColumnType("nvarchar(450)");
			b.HasKey("UserId", "RoleId");
			b.HasIndex("RoleId");
			b.ToTable("AspNetUserRoles", (string?)null);
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("UserId").HasColumnType("nvarchar(450)");
			b.Property<string>("LoginProvider").HasColumnType("nvarchar(450)");
			b.Property<string>("Name").HasColumnType("nvarchar(450)");
			b.Property<string>("Value").HasColumnType("nvarchar(max)");
			b.HasKey("UserId", "LoginProvider", "Name");
			b.ToTable("AspNetUserTokens", (string?)null);
		});
		modelBuilder.Entity("VideoOS.IdentityServer.DomainEventInfrastructure.OutboxMessage", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier")
				.HasDefaultValueSql("NEWSEQUENTIALID()");
			b.Property<int>("CompatibilityLevel").HasColumnType("int");
			b.Property<DateTime>("CreatedAtUtc").ValueGeneratedOnAdd().HasColumnType("datetime2")
				.HasDefaultValueSql("SYSUTCDATETIME()");
			b.Property<string>("Error").HasColumnType("nvarchar(max)");
			b.Property<string>("EventType").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<DateTime?>("LastAttemptUtc").HasColumnType("datetime2");
			b.Property<DateTime?>("LockExpiresAtUtc").HasColumnType("datetime2");
			b.Property<string>("Payload").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<DateTime?>("ProcessedAtUtc").HasColumnType("datetime2");
			b.Property<string>("ProcessingInstanceId").HasMaxLength(36).HasColumnType("nvarchar(36)");
			b.Property<int>("RetryCount").ValueGeneratedOnAdd().HasColumnType("int")
				.HasDefaultValue(0);
			b.Property<int>("Status").HasColumnType("int");
			b.HasKey("Id");
			b.HasIndex("Status", "CompatibilityLevel", "Id").HasDatabaseName("IX_OutboxMessages_New");
			b.HasIndex("Status", "CompatibilityLevel", "Id").IncludeProperties("ProcessingInstanceId", "LockExpiresAtUtc", "CreatedAtUtc");
			b.HasIndex("Status", "CompatibilityLevel", "LastAttemptUtc", "Id").HasDatabaseName("IX_OutboxMessages_Retry");
			b.HasIndex("Status", "CompatibilityLevel", "LastAttemptUtc", "Id").IncludeProperties("ProcessingInstanceId", "LockExpiresAtUtc", "RetryCount", "CreatedAtUtc");
			b.ToTable("OutboxMessages", null, delegate(TableBuilder t)
			{
				t.ExcludeFromMigrations();
			});
		});
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.EventLog", delegate(EntityTypeBuilder b)
		{
			b.Property<long>("Id").ValueGeneratedOnAdd().HasColumnType("bigint");
			b.Property<long>("Id").UseIdentityColumn(1L);
			b.Property<string>("Event").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("ObjectId").HasColumnType("nvarchar(max)");
			b.Property<string>("ObjectName").HasColumnType("nvarchar(max)");
			b.Property<string>("RemoteIpAddress").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset>("TimeStamp").HasColumnType("datetimeoffset");
			b.Property<string>("UserName").IsRequired().HasColumnType("nvarchar(max)");
			b.HasKey("Id");
			b.ToTable("EventLogs", null, delegate(TableBuilder t)
			{
				t.ExcludeFromMigrations();
			});
		});
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.VmsUser", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("Id").HasColumnType("nvarchar(450)");
			b.Property<int>("AccessFailedCount").HasColumnType("int");
			b.Property<bool>("Active").HasColumnType("bit");
			b.Property<bool>("CanChangePassword").HasColumnType("bit");
			b.Property<string>("ConcurrencyStamp").IsConcurrencyToken().HasColumnType("nvarchar(max)");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<DateTime?>("DeactivatedAtUtc").HasColumnType("datetime2");
			b.Property<string>("Description").HasColumnType("nvarchar(max)");
			b.Property<string>("Email").HasMaxLength(256).HasColumnType("nvarchar(256)");
			b.Property<bool>("EmailConfirmed").HasColumnType("bit");
			b.Property<bool>("ForcePasswordChange").HasColumnType("bit");
			b.Property<bool>("LockoutEnabled").HasColumnType("bit");
			b.Property<DateTimeOffset?>("LockoutEnd").HasColumnType("datetimeoffset");
			b.Property<DateTime>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("NormalizedEmail").HasMaxLength(256).HasColumnType("nvarchar(256)");
			b.Property<string>("NormalizedUserName").HasMaxLength(256).HasColumnType("nvarchar(256)");
			b.Property<string>("PasswordHash").HasColumnType("nvarchar(max)");
			b.Property<string>("PhoneNumber").HasColumnType("nvarchar(max)");
			b.Property<bool>("PhoneNumberConfirmed").HasColumnType("bit");
			b.Property<string>("SecurityStamp").HasColumnType("nvarchar(max)");
			b.Property<bool>("TwoFactorEnabled").HasColumnType("bit");
			b.Property<string>("UserName").HasMaxLength(256).HasColumnType("nvarchar(256)");
			b.HasKey("Id");
			b.HasIndex("NormalizedEmail").HasDatabaseName("EmailIndex");
			b.HasIndex("NormalizedUserName").IsUnique().HasDatabaseName("UserNameIndex")
				.HasFilter("[NormalizedUserName] IS NOT NULL");
			b.ToTable("AspNetUsers", (string?)null);
		});
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.VmsUserClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<string>("ClaimType").HasColumnType("nvarchar(max)");
			b.Property<string>("ClaimValue").HasColumnType("nvarchar(max)");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<string>("LoginProvider").HasColumnType("nvarchar(max)");
			b.Property<DateTime>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("UserId").IsRequired().HasColumnType("nvarchar(450)");
			b.HasKey("Id");
			b.HasIndex("UserId");
			b.ToTable("AspNetUserClaims", (string?)null);
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<string>", null).WithMany().HasForeignKey("RoleId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", delegate(EntityTypeBuilder b)
		{
			b.HasOne("VideoOS.IdentityServer.Entities.VmsUser", null).WithMany("Logins").HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<string>", null).WithMany().HasForeignKey("RoleId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.HasOne("VideoOS.IdentityServer.Entities.VmsUser", null).WithMany().HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", delegate(EntityTypeBuilder b)
		{
			b.HasOne("VideoOS.IdentityServer.Entities.VmsUser", null).WithMany().HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.VmsUserClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("VideoOS.IdentityServer.Entities.VmsUser", null).WithMany("Claims").HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.VmsUser", delegate(EntityTypeBuilder b)
		{
			b.Navigation("Claims");
			b.Navigation("Logins");
		});
	}
}
