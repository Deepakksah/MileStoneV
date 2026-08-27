-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

BEGIN TRAN

-- Modify existing EventTypes and insert StateGroupId and State

	--                                   IDEventType                            StateGroupId                            State
	EXEC [UpdateEventTypeWithStateGroup] 'F80AF822-6A89-4C2F-AB38-B55D0D2CD13A','F4261175-44D3-4705-B653-C33C8F0CA70E','Critical'
	EXEC [UpdateEventTypeWithStateGroup] 'C9FFDF1E-3766-49C8-BC84-91E95C2E5225','F4261175-44D3-4705-B653-C33C8F0CA70E','Normal'
	EXEC [UpdateEventTypeWithStateGroup] 'B3862572-A265-4A2D-B847-1BE7AFE5610E','F4261175-44D3-4705-B653-C33C8F0CA70E','Warning'

	EXEC [UpdateEventTypeWithStateGroup] 'A4CC82CE-B91D-4CC3-96B0-75B527C5F8F4','8B9B8FA4-3510-4725-8BD0-01AB4E8A71C1','Critical'
	EXEC [UpdateEventTypeWithStateGroup] 'E14E849F-7355-4D03-97FE-77C292FE01C7','8B9B8FA4-3510-4725-8BD0-01AB4E8A71C1','Normal'
	EXEC [UpdateEventTypeWithStateGroup] '54CDA1C8-FB7F-4DAB-B506-B0AD707F2B86','8B9B8FA4-3510-4725-8BD0-01AB4E8A71C1','Warning'

	EXEC [UpdateEventTypeWithStateGroup] 'B4D94AFC-9EFE-4083-89AF-078E6AB871E4','A4F206A2-EAF1-4BC3-9C35-21E9DF0F0FC8','Critical'
	EXEC [UpdateEventTypeWithStateGroup] '94831D23-3267-46E2-811F-D1DA115D63CB','A4F206A2-EAF1-4BC3-9C35-21E9DF0F0FC8','Normal'
	EXEC [UpdateEventTypeWithStateGroup] '3526491F-9CAC-47FB-BEC2-F9C016C26CD5','A4F206A2-EAF1-4BC3-9C35-21E9DF0F0FC8','Warning'
		
	EXEC [UpdateEventTypeWithStateGroup] 'CED4736D-B018-4116-A9F6-1E679741CD77','120B550D-3DBD-46A1-94B9-420CAB8E39E5','Critical'
	EXEC [UpdateEventTypeWithStateGroup] 'BDC4E2BB-C011-4094-994A-AAA9861163D4','120B550D-3DBD-46A1-94B9-420CAB8E39E5','Normal'
	EXEC [UpdateEventTypeWithStateGroup] '2FEBE17F-CB67-4A44-B6D5-D3C5828BF770','120B550D-3DBD-46A1-94B9-420CAB8E39E5','Warning'
		
	EXEC [UpdateEventTypeWithStateGroup] 'D1603FBE-4BB8-49EB-9928-0E8EB3C736CE','9D48B11A-0690-409F-8C7A-8FBC8812F657','Critical'
	EXEC [UpdateEventTypeWithStateGroup] '11C329C1-B36E-4F3F-9AB9-9B3A462DC0DA','9D48B11A-0690-409F-8C7A-8FBC8812F657','Normal'
	EXEC [UpdateEventTypeWithStateGroup] '5851E907-35CD-480D-9198-669E9BD8610A','9D48B11A-0690-409F-8C7A-8FBC8812F657','Warning'
		
	EXEC [UpdateEventTypeWithStateGroup] '766CF688-F3E2-4D2F-8D08-012193783739','72614B0B-62BF-46C2-B74F-9CDE0EEA87EA','Critical'
	EXEC [UpdateEventTypeWithStateGroup] 'C8E0A296-43C7-48CC-825D-74B90B6CBDE6','72614B0B-62BF-46C2-B74F-9CDE0EEA87EA','Normal'
	EXEC [UpdateEventTypeWithStateGroup] 'E68C0857-8D86-4190-B528-96AF21BE0F62','72614B0B-62BF-46C2-B74F-9CDE0EEA87EA','Warning'
		
COMMIT TRAN
GO