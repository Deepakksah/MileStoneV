-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityClaim]') AND type in (N'U'))
    CREATE TABLE [SecurityClaim] (
        [IDClaim]           BIGINT IDENTITY(1,1) NOT NULL,
        [LoginProviderId]   UNIQUEIDENTIFIER NOT NULL,
        [ClaimName]         NVARCHAR(128) COLLATE Latin1_General_100_BIN NOT NULL,
        [CaseSensitive]     BIT NOT NULL,
        [DisplayName]       NVARCHAR(128) NOT NULL,
        
        CONSTRAINT [PK_SecurityClaim] PRIMARY KEY CLUSTERED
        (
            [IDClaim] ASC
        )
    ) ON [PRIMARY]
GO


IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityRoleClaim]') AND type in (N'U'))
    CREATE TABLE [SecurityRoleClaim] (
        [IDRoleClaim]   BIGINT IDENTITY(1,1) NOT NULL,
        [IDRole]        [GuidType] NOT NULL,
        [IDClaim]       BIGINT NOT NULL,
        [ClaimValue]    NVARCHAR(512) NOT NULL,
    
        CONSTRAINT [PK_SecurityRoleClaim] PRIMARY KEY CLUSTERED
        (
            [IDRoleClaim] ASC
        ),

        CONSTRAINT [FK_SecurityRoleClaim_SecurityRoles] FOREIGN KEY
        (
            [IDRole]
        )
        REFERENCES [SecurityRoles] ([IDRole]),

        CONSTRAINT [FK_SecurityRoleClaim_SecurityClaim] FOREIGN KEY
        (
            [IDClaim]
        )
        REFERENCES [SecurityClaim] ([IDClaim])
    ) ON [PRIMARY]
GO
