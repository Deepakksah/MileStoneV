-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- The SUPPORTS_TRANSACTION property specifies if the scripts supports
-- running in multi-statement transaction or if it should be running
-- using implicit transactions
-- =========================================================================
-- TARGET: Surveillance
-- SUPPORTS_TRANSACTION: FALSE
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EncryptionKeys]') AND type = 'U')
CREATE TABLE [dbo].[EncryptionKeys](
	[Id] [GuidType] NOT NULL,	
	[Key] [nvarchar](512) NOT NULL
	CONSTRAINT [PK_EncryptionKeys] PRIMARY KEY ( [Id] ));
GO

IF NOT EXISTS (SELECT * FROM [dbo].[EncryptionKeys] WHERE [Id] = '49c1d447-0c9f-4814-8c34-73937fe87073')
BEGIN
	INSERT INTO [dbo].[EncryptionKeys] ([Id], [Key])
	VALUES ('49c1d447-0c9f-4814-8c34-73937fe87073', '{EncryptedMasterEncryptionKey}')
END
GO
