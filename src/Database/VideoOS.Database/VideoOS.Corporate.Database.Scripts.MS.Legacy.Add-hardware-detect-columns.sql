-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DELETE [HardwareDriverRecorder]

IF EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[HardwareDriverRecorder]') AND name = N'ClassName')
BEGIN
	ALTER TABLE [HardwareDriverRecorder] DROP COLUMN [ClassName]
END

IF EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[HardwareDriverRecorder]') AND name = N'AssemblyFile')
BEGIN
	ALTER TABLE [HardwareDriverRecorder] DROP COLUMN [AssemblyFile]
END

IF EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[HardwareDriverRecorder]') AND name = N'Definition')
BEGIN
	ALTER TABLE [HardwareDriverRecorder] DROP COLUMN [Definition]
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'HardwareDriverRecorder') AND name = N'DriverNumber')
BEGIN
	
	ALTER TABLE [HardwareDriverRecorder] ADD 
		[DriverName] NVARCHAR(256) NOT NULL,
		[DriverNumber] INT NOT NULL,
		[DriverVersion] NVARCHAR(128) NOT NULL,
		[XmlVersion] NVARCHAR(128) NOT NULL,
		[DllName] NVARCHAR(128) NOT NULL,
		[GroupName] NVARCHAR(128) NOT NULL
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_HardwareDriverRecorder_Recorders]') AND parent_object_id = OBJECT_ID(N'[HardwareDriverRecorder]'))
ALTER TABLE [HardwareDriverRecorder] 
	ADD CONSTRAINT [FK_HardwareDriverRecorder_Recorders] FOREIGN KEY([IDRecorder]) REFERENCES [Recorders] ([IDRecorder]) ON DELETE CASCADE
GO



