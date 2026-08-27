-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[process_performance_counter_data]') AND type = 'P')
DROP PROCEDURE [process_performance_counter_data]
GO

CREATE PROCEDURE [process_performance_counter_data]
	@data_collector_id INT,
	@counter_values XML
AS
BEGIN
	SET NOCOUNT ON;

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	DECLARE @instance_id INT
	
	BEGIN TRY
		BEGIN TRANSACTION

		-- This holds the transformed xml including a formattet counter id
		CREATE TABLE #TempPerformanceCounterValue
		(
			[category_name] NVARCHAR(120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
			[counter_name] NVARCHAR(120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
			[instance_name] NVARCHAR(120) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
			[collection_time] DATETIME NOT NULL,
			[raw_value] BIGINT NOT NULL,
			[calculated_value] FLOAT NOT NULL,
			[counter_id] AS (CASE WHEN SUBSTRING(REVERSE([instance_name]), 1, 1) = ']' -- Ensure we have a ']' as the last character
				AND SUBSTRING(REVERSE([instance_name]), 38, 1) = '[' -- Ensure we have a '[' 37 characters before enclosing ']'
				AND SUBSTRING([instance_name], LEN([instance_name]) - 36, 36) LIKE '[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]'
				THEN SUBSTRING([instance_name], LEN([instance_name]) - 36, 36)
				ELSE NULL
				END)
		)

		-- using a calculated column instead of parse it during load appears to save about 50 ms execution on average

		INSERT #TempPerformanceCounterValue
		SELECT
			[category_name] = v.value('@c1', 'NVARCHAR(120)'),
			[counter_name] = v.value('@c2', 'NVARCHAR(120)'),
			[instance_name] = v.value('@in', 'NVARCHAR(120)'),
			[collection_time] = v.value('@ct', 'DATETIME'),
			[raw_value] = v.value('@rw', 'BIGINT'),
			[calculated_value] = v.value('@cv', 'FLOAT')
		FROM @counter_values.nodes('cvs/cv') AS T(v)

		-- This will ensure that the input data are cleaned up before starting to process data. Cleanup means that all counter values for a specific
		-- counter has the same name. The name all values must have is the instance name of latest reported value.
		;WITH cleanup AS 
		(	
			SELECT 
				[category_name], 
				[counter_name], 
				[instance_name], 
				[collection_time],
				[counter_id],
				ROW_NUMBER() OVER ( PARTITION BY [category_name], [counter_name], [counter_id] ORDER BY [collection_time] DESC ) AS newest_name
			FROM [#TempPerformanceCounterValue]
		)
		UPDATE tpcv
		SET tpcv.[instance_name] = c.[instance_name]		
		FROM #TempPerformanceCounterValue tpcv
		INNER JOIN cleanup c ON 
					c.[category_name] = tpcv.[category_name]
			AND c.[counter_name] = tpcv.[counter_name]
			AND c.[counter_id] = tpcv.[counter_id]
		WHERE c.newest_name = 1

		DECLARE @instancesToUpdate INT

		SELECT @instancesToUpdate = COUNT(pci.pkid)
		FROM [PerformanceCounterInstance] pci WITH(NOLOCK)
		INNER JOIN #TempPerformanceCounterValue tempVal ON tempVal.counter_id = pci.counter_id
		WHERE 
					pci.[data_collector_id] = @data_collector_id
			AND (pci.[instance_name] <> tempVal.[instance_name] OR pci.[is_obsolete] = 1 )-- Ensure that the name has changed or has been marked as obsolete
			AND pci.[category_name] = tempVal.[category_name] -- Ensure the category match
			AND pci.[counter_name] = tempVal.[counter_name] -- Ensure the counter name match

		IF(@instancesToUpdate > 0)
		BEGIN
			-- If we have any counters based on guids - update the names of these if the name has changed
			UPDATE pci
			SET pci.[instance_name] = tempVal.[instance_name],
				pci.[is_obsolete] = 0
			FROM [PerformanceCounterInstance] pci
			INNER JOIN #TempPerformanceCounterValue tempVal ON tempVal.counter_id = pci.counter_id
			WHERE 
						pci.[data_collector_id] = @data_collector_id
				AND (pci.[instance_name] <> tempVal.[instance_name] OR pci.is_obsolete = 1) -- Ensure that the name has changed
				AND pci.[category_name] = tempVal.[category_name] -- Ensure the category match
				AND pci.[counter_name] = tempVal.[counter_name] -- Ensure the counter name match
		END

		-- Make sure all new instances are created
		INSERT [PerformanceCounterInstance] ([data_collector_id], [category_name], [counter_name], [instance_name], [counter_type])
		SELECT 
			@data_collector_id,
			tempVal.[category_name],
			tempVal.[counter_name],
			tempVal.[instance_name],
			0
		FROM [#TempPerformanceCounterValue] tempVal
		LEFT JOIN [PerformanceCounterInstance] counterInstance ON 
			@data_collector_id = counterInstance.[data_collector_id] AND
			tempVal.[category_name] = counterInstance.[category_name] AND
			tempVal.[counter_name] = counterInstance.[counter_name] AND
			tempVal.[instance_name] = counterInstance.[instance_name]
		WHERE counterInstance.[pkid] IS NULL
		GROUP BY counterInstance.[pkid], tempVal.[category_name], tempVal.[counter_name], tempVal.[instance_name]

		BEGIN TRY
			-- Insert performance counter values from temp table
			INSERT [PerformanceCounterValue] ([instance_id], [collection_time], [raw_value], [calculated_value])
			SELECT counterInstance.[pkid], tempVal.[collection_time], tempVal.[raw_value], tempVal.[calculated_value]
			FROM [#TempPerformanceCounterValue] tempVal
			INNER JOIN [PerformanceCounterInstance] counterInstance ON
						@data_collector_id = counterInstance.[data_collector_id] 
				AND tempVal.[category_name] = counterInstance.[category_name]
				AND tempVal.[counter_name] = counterInstance.[counter_name] 
				AND tempVal.[instance_name] = counterInstance.[instance_name]
		END TRY
		BEGIN CATCH
			-- This might happen in the unlikely event that two values for the same counter-instance has the same collection time. There is
			-- a primary key constraint on PerformanceCounterValue base on collection_time and instance_id.
		END CATCH

		---- Delete old performance counter values
		DELETE [PerformanceCounterValue]
		WHERE [collection_time] < DATEADD(dd, -32, GETUTCDATE())

		---- Delete obsolete performance counter instances if there is no [PerformanceCounterValue] anymore
		DELETE FROM [PerformanceCounterInstance] 
		WHERE [PerformanceCounterInstance].is_obsolete = 1 AND NOT EXISTS(SELECT TOP 1 value.instance_id
		FROM [PerformanceCounterValue] as value 
		WHERE value.instance_id = [PerformanceCounterInstance].pkid)

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
END
GO