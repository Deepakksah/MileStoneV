-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- The SUPPORTS_TRANSACTION property specifies if the scripts supports
-- running in multi-statement transaction or if it should be running
-- using implicit transactions
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE [object_id] = OBJECT_ID(N'[VideoWallViewItem]') AND name = N'IX_FK_View_Id')
CREATE NONCLUSTERED INDEX [IX_FK_View_Id] ON [VideoWallViewItem]
(
	[view_id] ASC
)
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE [object_id] = OBJECT_ID(N'[Hardware]') AND name = N'IX_Hardware_IDRecorder')
DROP INDEX [IX_Hardware_IDRecorder] ON [Hardware]
GO

CREATE NONCLUSTERED INDEX [IX_Hardware_IDRecorder] ON [Hardware]
(
	[IDRecorder] ASC,
	[Enabled] ASC
)
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE [object_id] = OBJECT_ID(N'[Devices]') AND name = N'IX_Devices_IDHardware')
DROP INDEX [IX_Devices_IDHardware] ON [Devices]
CREATE NONCLUSTERED INDEX [IX_Devices_IDHardware] ON [Devices]
(
	[IDHardware] ASC,
	[Enabled] ASC
)
GO
