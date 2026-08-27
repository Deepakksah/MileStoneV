-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE [object_id] = OBJECT_ID(N'HardwareBlock'))
BEGIN
	CREATE TABLE [dbo].[HardwareBlock]
	(
		[pkid] INT IDENTITY(1,1) NOT NULL,
		[hardware_id] [GuidType] NOT NULL,
		[expires] DATETIME NOT NULL,
		[owner] NVARCHAR(256) NOT NULL,
		[data] NVARCHAR(4000) NOT NULL,

		CONSTRAINT [PK_HardwareBlock] PRIMARY KEY CLUSTERED ([pkid] ASC),

		CONSTRAINT [FK_HardwareBlock_Hardware] FOREIGN KEY ([hardware_id]) REFERENCES [Hardware] ([IDHardware]),
	)
END
GO
