-- Delete DeleteExpiredMessageInstances procedure if it exists
IF OBJECTPROPERTY(OBJECT_ID('[LogServer].[DeleteExpiredMessageInstances]'), 'IsProcedure') = 1
	DROP PROCEDURE [LogServer].[DeleteExpiredMessageInstances]
GO

-- Recreate DeleteExpiredMessageInstances procedure
CREATE PROCEDURE [LogServer].[DeleteExpiredMessageInstances]
@ThresholdPercentage TINYINT = NULL,
@SystemLogRetention SMALLINT = NULL,
@AuditLogRetention SMALLINT = NULL,
@RuleLogRetention SMALLINT = NULL,
@DeletePerformed BIT OUTPUT
AS
BEGIN
	-- Comment: Checks percentage of alive message instances in inactive message instance structure and triggeres 
	--          swapping workflow if certain threshold is reached. Swapping enlists:
	--          - recreate message instance structure
	--          - transfer alive message instances (those that are not expired) from inactive to new structure
	--          - potentially update expiration dates
	--          - delete inactive structure
	--          - rename new structure to replace deleted one
	--          - acquire lock on active structure and reseed indent of the new one to the indent of the active
	--          - transform all version dependent procedures to work with the new structure (becomes the active) 
	--            thus making currently active to become inactive and making it next candidate for deletion
	--
	-- Param @ThresholdPercentage:   Percentage threshold to trigger expired message instances deletion,
	--                               default is NULL which will unconditionally trigger swapping workflow
	-- Param @SystemLogRetention:	 Retention time for System Logs (in days)
	-- Param @AuditLogRetention:	 Retention time for Audit Logs (in days)
	-- Param @RuleLogRetention:		 Retention time for Rule Logs (in days)
	-- Param @DeletePerformed:       Out parameter telling whether deletion was actually performed

	SET NOCOUNT ON

	DECLARE @alivePercentage DECIMAL(8, 5)

	-- calculates current live percentage in inactive structure
	-- if structure is empty the result will be NULL and considered as 0% alive message instances - will trigger swapping
	IF @ThresholdPercentage IS NOT NULL
	BEGIN
		SELECT	@alivePercentage = 
			ISNULL(
				SUM(CASE WHEN [UtcExpires] <= GETUTCDATE()
					THEN 0 
					ELSE 1 
					END) * 100.0 / COUNT(1),
				0)
		FROM [LogServer].[MessageInstance_Inactive]
	END

	-- If live percentage is above or at given threshold do nothing otherwise trigger swapping
	-- (this will also trigger swapping in the edge case where both are 100)
	
	IF @ThresholdPercentage IS NULL OR @alivePercentage <= @ThresholdPercentage
	BEGIN
		-- recreate message instance structures to get a fresh set of MessageInstance_0 tables
		EXEC [LogServer].[MessageInstance_RecreateStructure]

		SET XACT_ABORT ON
		BEGIN TRANSACTION
			-- Temporarily switch identity insertion to migrate alive message instances to the new primary message instance table
			SET IDENTITY_INSERT [LogServer].[MessageInstance_0] ON

			-- Move all alive message instances to new structure
			IF @SystemLogRetention IS NULL AND
			   @AuditLogRetention IS NULL AND
			   @RuleLogRetention IS NULL
			   -- Expiration did not change
				EXEC [LogServer].[MoveNonExpiredMessageInstances]
			ELSE
				-- We need to both move and update the expiration
				EXEC [LogServer].[MoveMessageInstancesAndUpdateExpiration] @SystemLogRetention, @AuditLogRetention, @RuleLogRetention

			-- switch back off identity insertion for the new primary message instance table
			SET IDENTITY_INSERT [LogServer].[MessageInstance_0] OFF

			-- Move all alive message parameters to new structure
			INSERT INTO [LogServer].[MessageInstance_0_Parameter]
			([MessageInstanceId], [Name], [Value])
			SELECT	[MessageInstanceId], [Name], [Value]
			FROM [LogServer].[MessageInstance_0] MI
			JOIN [LogServer].[MessageInstance_Inactive_Parameter] P ON MI.[Id] = P.[MessageInstanceId]

			-- Drop existing inactive structure
			DROP TABLE [LogServer].[MessageInstance_Inactive_Parameter]
			DROP TABLE [LogServer].[MessageInstance_Inactive]

			DECLARE @Lock bigint
			DECLARE @MaxId_Inactive bigint
			
			ALTER TABLE [LogServer].[MessageInstance_Active] SET (LOCK_ESCALATION = TABLE)
				
			-- Acquire lock (and base for current indent value for reseed of new structure) on active message instance table to lock further increment of primary key
			SELECT TOP 1 @Lock = MAX([Id]) FROM [LogServer].[MessageInstance_Active] WITH (TABLOCK, HOLDLOCK)

			-- Get the max ID of the message instances from the (now dropped) MessageInstance_Inactive table to cover the case where no message instances have been inserted into MessageInstance_Active 
			-- (i.e. the previous reseed of MessageInstance_Active has not been utilized for any message instances, so the max. ID in MessageInstance_Active may be lower than the max. ID in MessageInstance_Inactive)
			SELECT @MaxId_Inactive = MAX([Id]) FROM [LogServer].[MessageInstance_0] 

			-- Offset to avoid duplicate IDs in the two tables
			DECLARE @Seed bigint
			SELECT @Seed = 10000 + MAX([Id]) FROM (VALUES (@Lock),(@MaxId_Inactive)) AS Candidates(Id)

			-- If active message instance table is not empty, reseed new message instance table to current indent value
			IF @Seed IS NOT NULL
				DBCC CHECKIDENT ('[LogServer].[MessageInstance_0]', RESEED, @Seed)
			
			-- Rename the old Active structures to Inactive
			EXEC [LogServer].[MessageInstance_RenameStructure] 'MessageInstance_Active', 'MessageInstance_Inactive'
			
			-- Rename the new tables to the new Active 
			EXEC [LogServer].[MessageInstance_RenameStructure] 'MessageInstance_0', 'MessageInstance_Active'
			
		COMMIT TRANSACTION
		SET @DeletePerformed = 1
	END
	ELSE
	BEGIN
		SET @DeletePerformed = 0
	END
END
GO