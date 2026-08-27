-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


-- Variables 

DECLARE @object_type INT = 2
DECLARE @object_name NVARCHAR = ''
DECLARE @TransactionSourceId [GuidType] = '9A1492D1-FCA0-44D7-BA9C-3699C99063A7'
DECLARE @TransactionSourceObjectId [GuidType] = (SELECT [security_object_id] FROM SecurityObject WHERE object_id = @TransactionSourceId);
DECLARE @TransactionDefinitionId [GuidType] = 'B4EE79CC-7D0B-449F-A53A-EBF0A3D1F14B'
DECLARE @TransactionDefinitionObjectId [GuidType] = (SELECT [security_object_id] FROM SecurityObject WHERE object_id = @TransactionDefinitionId);

-- Create new SecurityObjects of Transaction Sources
IF NOT EXISTS (SELECT * FROM [dbo].[SecurityObject] WHERE parent_path = '/' + @TransactionSourceId + '/')
BEGIN
	INSERT INTO [dbo].[SecurityObject](namespace_id, parent_path, object_id, object_type, object_name, security_object_id, inherits_from_id)
		SELECT 
			@TransactionSourceId,
			'/' + @TransactionSourceId + '/',
			Mip.Items.ItemId,
			@object_type,
			@object_name,
			NEWID(),
			@TransactionSourceObjectId
		FROM [Mip].[Items] 
		WHERE Mip.Items.kind = @TransactionSourceId
END 


-- Create new SecurityObjects of Transaction Definitions
IF NOT EXISTS (SELECT * FROM [dbo].[SecurityObject] WHERE parent_path = '/' + @TransactionDefinitionId + '/')
BEGIN
	INSERT INTO [dbo].[SecurityObject](namespace_id, parent_path, object_id, object_type, object_name, security_object_id, inherits_from_id)
		SELECT 
			@TransactionDefinitionId,
			'/' + @TransactionDefinitionId + '/',
			Mip.Items.ItemId,
			@object_type,
			@object_name,
			NEWID(),
			@TransactionDefinitionObjectId
		FROM [Mip].[Items] 
		WHERE Mip.Items.kind = @TransactionDefinitionId
END 


GO