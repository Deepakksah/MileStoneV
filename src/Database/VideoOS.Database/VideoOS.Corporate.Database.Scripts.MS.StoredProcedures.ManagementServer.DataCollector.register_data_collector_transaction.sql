-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[register_data_collector_transaction]') AND type = 'P')
DROP PROCEDURE [register_data_collector_transaction]
GO

CREATE PROCEDURE [register_data_collector_transaction]
	@application_id UNIQUEIDENTIFIER,
	@host_name NVARCHAR(100), 
	@version INT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DECLARE @data_collector_id BIGINT
	DECLARE @existing_host_name NVARCHAR(200)

	SELECT @data_collector_id = [pkid]
	FROM [DataCollector]
	WHERE [application_id] = @application_id
		
	IF(@data_collector_id IS NULL)
	BEGIN
		SELECT @existing_host_name = [host_name]
		FROM [DataCollector]
		WHERE [host_name] = @host_name
		
		-- Either the data collector does not exists or it has a new application id
		IF(@existing_host_name IS NULL)
		BEGIN
			-- The data collector has not been registered before
			INSERT [DataCollector] ([application_id], [host_name], [version])
			VALUES (@application_id, @host_name, @version)

			SELECT @data_collector_id = @@IDENTITY

			INSERT [DataCollectorConfiguration]([data_collector_id], [category_name], [counter_name], [instance_name])
				
			SELECT @data_collector_id, 'LogicalDisk', 'Disk Read Bytes/sec', '' UNION ALL
			SELECT @data_collector_id, 'LogicalDisk', 'Disk Write Bytes/sec', '' UNION ALL

			SELECT @data_collector_id, 'Memory', 'Available MBytes', '' UNION ALL

			SELECT @data_collector_id, 'Network Interface', 'Bytes Received/sec', '' UNION ALL
			SELECT @data_collector_id, 'Network Interface', 'Bytes Sent/sec', '' UNION ALL
			SELECT @data_collector_id, 'Network Interface', 'Bytes Total/sec', '' UNION ALL

			SELECT @data_collector_id, 'Processor', '% Processor Time', '_Total' UNION ALL
				
			SELECT @data_collector_id, 'VideoOS Recording Server Storage', 'Used Bytes', '' UNION ALL
			SELECT @data_collector_id, 'VideoOS Recording Server Device Storage', 'Used Bytes', '' UNION ALL
			SELECT @data_collector_id, 'VideoOS Recording Server Database Disk', 'Free Bytes', '' UNION ALL
			SELECT @data_collector_id, 'VideoOS Recording Server Database Disk', 'Media Data Bytes', '' UNION ALL
			SELECT @data_collector_id, 'VideoOS Recording Server Database Disk', 'Other Data Bytes', '' UNION ALL
			SELECT @data_collector_id, 'VideoOS Recording Server Drivers', 'Bytes/sec', '' UNION ALL
				
			SELECT @data_collector_id, 'VideoOS Recording Server Device', 'Media/sec perc', '' UNION ALL
			SELECT @data_collector_id, 'VideoOS Recording Server Device', 'State', '' UNION ALL
			SELECT @data_collector_id, 'VideoOS Recording Server Storage Archive', 'ArchiveAvailable', '' UNION ALL
			SELECT @data_collector_id, 'VideoOS Recording Server Storage Database', 'DatabaseAvailable', '' UNION ALL
			SELECT @data_collector_id, 'VideoOS Failover State', 'Started', '' UNION ALL
			SELECT @data_collector_id, 'VideoOS Failover State', 'Error State', '' UNION ALL
			SELECT @data_collector_id, 'VideoOS Recording Server Drivers', 'Media/sec perc', ''
		END
		ELSE
		BEGIN
			-- An existing data collector (host_name) has a new application id
			UPDATE [DataCollector]
			SET [application_id] = @application_id,
				[version] = @version
			WHERE [host_name] = @existing_host_name

			SELECT @data_collector_id = [pkid]
			FROM [DataCollector]
			WHERE [application_id] = @application_id
		END
	END
	ELSE
	BEGIN
		SELECT @existing_host_name = [host_name]
		FROM [DataCollector]
		WHERE [host_name] = @host_name AND [application_id] <> @application_id
		
		-- An existing data collector has a new host name
		IF(@existing_host_name IS NOT NULL)
		BEGIN
			-- Another data collector exists with this name.
			-- We assume that this happened because the already existing 
			-- data collector was replaced by this data collector.
			--
			-- Fix this by renaming the existing data collector to something unique.

			DECLARE @i INT
			DECLARE @unique_host_name NVARCHAR(100)

			SET @i = 1
			SET @unique_host_name = @host_name + '$$' + CAST(@i AS NVARCHAR(5))
			WHILE (EXISTS(SELECT 1 FROM [DataCollector] WHERE [host_name] = @unique_host_name))
			BEGIN
				SET @i = @i + 1
				SET @unique_host_name = @host_name + '$$' + CAST(@i AS NVARCHAR(5))
			END

			UPDATE [DataCollector]
			SET [host_name] = @unique_host_name
			WHERE [host_name] = @host_name
		END

		-- Update host name for the current data collector
		UPDATE [DataCollector]
		SET [host_name] = @host_name,
			[version] = @version
		WHERE [application_id] = @application_id
	END
		
	RETURN @data_collector_id
END
GO