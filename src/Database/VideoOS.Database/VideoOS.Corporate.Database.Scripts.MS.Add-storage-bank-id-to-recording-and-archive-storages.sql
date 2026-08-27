-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingStorage]') AND name = N'bank_id')
BEGIN
	ALTER TABLE [RecordingStorage]
	ADD [bank_id] UNIQUEIDENTIFIER NULL
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[ArchiveStorage]') AND name = N'bank_id')
BEGIN
	ALTER TABLE [ArchiveStorage]
	ADD [bank_id] UNIQUEIDENTIFIER NULL
END
GO


UPDATE	[RecordingStorage]
SET		[bank_id] = NEWID()
WHERE	[bank_id] IS NULL

UPDATE	[ArchiveStorage]
SET		[bank_id] = NEWID()
WHERE	[bank_id] IS NULL
GO


ALTER TABLE [RecordingStorage]
ALTER COLUMN [bank_id] UNIQUEIDENTIFIER NOT NULL

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'RecordingStorage' AND CONSTRAINT_NAME = 'UQ_RecordingStorage_BankId')
BEGIN
	ALTER TABLE [RecordingStorage]
	ADD CONSTRAINT UQ_RecordingStorage_BankId UNIQUE (bank_id); 
END

ALTER TABLE [ArchiveStorage]
ALTER COLUMN [bank_id] UNIQUEIDENTIFIER NOT NULL

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'ArchiveStorage' AND CONSTRAINT_NAME = 'UQ_ArchiveStorage_BankId')
BEGIN
	ALTER TABLE [ArchiveStorage]
	ADD CONSTRAINT UQ_ArchiveStorage_BankId UNIQUE (bank_id); 
END
GO


UPDATE t
		SET [data_a] = [data_a].query('
									   let $task := task
									   for $e in task
									   return
										 <task id="{ $task/@id }" type="{ $task/@type }">
											 { $e/* }
											 <bankIdSource>{sql:column("D.source_bank_id")}</bankIdSource>
											 <bankIdTarget>{sql:column("r.bank_id")}</bankIdTarget>
										 </task>
									')
  FROM	[Task] t
		INNER JOIN RecordingStorage r ON r.[pkid] = t.[data_a].value('(task/idStorage)[1]', 'char(36)')
		CROSS APPLY ( SELECT NEWID() AS [source_bank_id] ) D
  WHERE t.[data_a].exist('task/bankIdSource') = 0
		AND t.[data_a].value('(task/@type)[1]', 'varchar(100)') = 'Database.MoveStorage'

UPDATE t
		SET [data_a] = [data_a].query('
									   let $task := task
									   for $e in task
									   return
										 <task id="{ $task/@id }" type="{ $task/@type }">
											 { $e/* }
											 <bankIdSource>{sql:column("D.source_bank_id")}</bankIdSource>
											 <bankIdTarget>{sql:column("a.bank_id")}</bankIdTarget>
										 </task>
									')
  FROM	[Task] t
		INNER JOIN ArchiveStorage a ON a.[pkid] = t.[data_a].value('(task/idArchive)[1]', 'char(36)')
		CROSS APPLY ( SELECT NEWID() AS [source_bank_id] ) D
  WHERE t.[data_a].exist('task/bankIdSource') = 0
		AND t.[data_a].value('(task/@type)[1]', 'varchar(100)') = 'Database.MoveArchive'

GO