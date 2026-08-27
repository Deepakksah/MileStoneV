-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_system_dashboard_config]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_system_dashboard_config]
GO

CREATE PROCEDURE [create_system_dashboard_config]
	@Id INT,
	@Name VARCHAR(100),
	@StateSourceType VARCHAR(50),
	@SingleStateSourceId VARCHAR(50),
	@EventTypes XML,
	@Position INT,
	@session_id [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION
	
		BEGIN TRY
			INSERT INTO [SystemDashboardConfiguration] 
			(
				[id],
				[name],
				[state_source_type],
				[single_state_source_id],
				[event_types],
				[position]
			)
			VALUES 
			(
				@Id,
				@Name,
				@StateSourceType,
				@SingleStateSourceId,
				CAST(@EventTypes as VARCHAR(MAX)),
				@Position
			)
		END TRY
		BEGIN CATCH

			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists on the server
				ROLLBACK TRAN
				;THROW 60461, @procedureName, 1;
			END
			;THROW;
		END CATCH
	
		COMMIT TRANSACTION

		IF (@session_id IS NOT NULL)
		BEGIN
			EXEC create_pending_log @session_id, 23006, @Id, @Name
		END
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
	END CATCH;
END
GO

-- Initialize the ProcedureName for error messages.
DECLARE @procedureName SYSNAME
SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + 'install_systemdashboardconfig'
DECLARE @ErrorArgs NVARCHAR(500)

BEGIN TRY
	BEGIN TRANSACTION

	-- created default System Dashboard Configurations	
	DECLARE @idn INT
	SELECT @idn = 1

	-- All Recording Servers
	IF NOT EXISTS(SELECT * FROM [SystemDashboardConfiguration] WHERE [id] = @idn)
	EXECUTE [create_system_dashboard_config] 
		@Id = @idn,
		@Name = 'All Recording Servers',
		@StateSourceType = '2B2D3601-5857-44A3-880F-BE7957B5B27E', --all recording servers type
		@SingleStateSourceId = NULL,
		@EventTypes = N'<Ids><Id>1</Id><Id>2</Id><Id>6</Id><Id>7</Id><Id>15</Id><Id>16</Id><Id>17</Id></Ids>',
		@Position = 0,
		@session_id = NULL
	
	-- All Servers
	SELECT @idn = 2;
	IF NOT EXISTS(SELECT * FROM [SystemDashboardConfiguration] WHERE [id] = @idn)
	EXECUTE [create_system_dashboard_config] 
		@Id = @idn,
		@Name = 'All Servers',
		@StateSourceType = '4D14ADA2-BCC3-4EBA-B04A-DCA8D2FDDE98', --all servers type
		@SingleStateSourceId = NULL,
		@EventTypes = N'<Ids><Id>1</Id><Id>2</Id></Ids>',
		@Position = 1,
		@session_id = NULL


	-- All cameras
	SELECT @idn = 3;
	IF NOT EXISTS(SELECT * FROM [SystemDashboardConfiguration] WHERE [id] = @idn)
	EXECUTE [create_system_dashboard_config] 
		@Id = @idn,
		@Name = 'All Cameras',
		@StateSourceType = '0E1B0AD3-F67C-4D5F-B792-4BD6C3CF52F8', --all cameras type
		@SingleStateSourceId = NULL,
		@EventTypes = N'<Ids><Id>4</Id><Id>5</Id><Id>3</Id></Ids>',
		@Position = 2,
		@session_id = NULL

	COMMIT TRANSACTION
END TRY
BEGIN CATCH 
	IF @@TRANCOUNT > 0
	BEGIN
		ROLLBACK TRANSACTION
	END;
	DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
END CATCH;
GO