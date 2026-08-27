IF OBJECT_ID ( '[LogServer].[WriteRetentionConfiguration]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[WriteRetentionConfiguration];
GO

CREATE PROCEDURE [LogServer].[WriteRetentionConfiguration]
@SystemLogRetentionDays int = NULL,
@AuditLogRetentionDays int = NULL,
@RuleLogRetentionDays int = NULL
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			IF @SystemLogRetentionDays IS NOT NULL
			BEGIN
				UPDATE [LogServer].[RetentionConfiguration]
				SET [RetentionDays] = @SystemLogRetentionDays
				WHERE [LogTypeId] = 1
			END

			IF @AuditLogRetentionDays IS NOT NULL
			BEGIN
				UPDATE [LogServer].[RetentionConfiguration]
				SET [RetentionDays] = @AuditLogRetentionDays
				WHERE [LogTypeId] = 2
			END

			IF @RuleLogRetentionDays IS NOT NULL
			BEGIN
				UPDATE [LogServer].[RetentionConfiguration]
				SET [RetentionDays] = @RuleLogRetentionDays
				WHERE [LogTypeId] = 3
			END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK;
		THROW
	END CATCH
END
GO