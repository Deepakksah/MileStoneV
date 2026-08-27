IF OBJECT_ID ( '[LogServer].[WriteConfiguration]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[WriteConfiguration];
GO

CREATE PROCEDURE [LogServer].[WriteConfiguration]
@Configurations ConfigurationTable READONLY
AS
BEGIN
	BEGIN TRANSACTION

	DECLARE @Name nvarchar(256)
	DECLARE @Value nvarchar(max)
	DECLARE ConfigurationsCursor CURSOR FOR
	SELECT [Name], [Value]
	FROM @Configurations
	OPEN ConfigurationsCursor
	FETCH NEXT FROM ConfigurationsCursor INTO @Name, @Value
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		UPDATE [LogServer].[Configuration]
		SET [Value] = @Value
		WHERE [Name] = @Name

		IF @@ROWCOUNT = 0
		BEGIN

			BEGIN TRY
				INSERT INTO [LogServer].[Configuration]([Name], [Value])
				VALUES (@Name, @Value)
			END TRY
			BEGIN CATCH
				IF ERROR_NUMBER() IN (2601, 2627)	-- unique constraint violation
				BEGIN
					UPDATE [LogServer].[Configuration]
					SET [Value] = @Value
					WHERE [Name] = @Name
				END
				ELSE
				BEGIN
					ROLLBACK;
					THROW
				END

			END CATCH

		END
		FETCH NEXT FROM ConfigurationsCursor INTO @Name, @Value
	END
	CLOSE ConfigurationsCursor
	DEALLOCATE ConfigurationsCursor

	COMMIT TRANSACTION
END
GO