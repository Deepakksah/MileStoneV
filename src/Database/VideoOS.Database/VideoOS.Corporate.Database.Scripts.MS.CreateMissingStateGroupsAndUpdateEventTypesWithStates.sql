-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

BEGIN TRAN

-- Create State groups for existing events

    --								 GroupId	GroupName	List of States (Comma seprated)
	EXEC [stategroup_create_initial] '5E0C77F3-AC22-47F0-B67C-351173FCBA27','DatabaseDisk','Available,Unavailable'
	EXEC [stategroup_create_initial] '46F1D0BE-9DFF-42EE-AB75-05D52BACE11C','ArchiveDisk','Available,Unavailable'

COMMIT TRAN
GO

BEGIN TRAN

-- Modify existing EventTypes and insert StateGroupId and State

	--                                   IDEventType                            StateGroupId                            State
	EXEC [UpdateEventTypeWithStateGroup] '29986AA1-924B-4c8d-AAC7-A876883DC82C','5E0C77F3-AC22-47F0-B67C-351173FCBA27','Unavailable'
	EXEC [UpdateEventTypeWithStateGroup] 'FAB03C4D-0589-48c8-9328-E28B76C95AF0','5E0C77F3-AC22-47F0-B67C-351173FCBA27','Available'

	EXEC [UpdateEventTypeWithStateGroup] '72C29AAE-DDB2-46df-B6FD-7F728A85F292','46F1D0BE-9DFF-42EE-AB75-05D52BACE11C','Unavailable'
	EXEC [UpdateEventTypeWithStateGroup] 'B7F0003B-1392-450e-B6CC-9BBE6E7DF065','46F1D0BE-9DFF-42EE-AB75-05D52BACE11C','Available'

COMMIT TRAN
GO