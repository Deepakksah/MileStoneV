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

/*DTS-43001 - http://dts/show_bug.cgi?id=43001 */

/*UPDATING POSSITION UNDER SMARTWALL*/
UPDATE [dbo].[SecurityAction] SET [order_no] = 5 WHERE [name] = 'ADMIN_SECURITY' AND [namespace_id] = '8732FCAC-A63C-46EA-9683-532B4C6644E7'
UPDATE [dbo].[SecurityAction] SET [order_no] = 4 WHERE [name] = 'CREATE_SMARTWALL' AND [namespace_id] = '8732FCAC-A63C-46EA-9683-532B4C6644E7'

/*UPDATING POSSITION UNDER VIEW GROUPS*/
UPDATE [dbo].[SecurityAction] SET [order_no] = 5 WHERE [name] = 'ADMIN_SECURITY' AND [namespace_id] = 'EC543C56-ADF7-4600-B9AD-97E166552BE9'
UPDATE [dbo].[SecurityAction] SET [order_no] = 4 WHERE [name] = 'CREATE_VIEWGROUP' AND [namespace_id] = 'EC543C56-ADF7-4600-B9AD-97E166552BE9'

/*UPDATING POSSITION UNDER USER DEFINED EVENTS*/
UPDATE [dbo].[SecurityAction] SET [order_no] = 5 WHERE [name] = 'ADMIN_SECURITY' AND [namespace_id] = 'D4FF31CD-D9AC-46DF-8E92-5B545D191BAF'
UPDATE [dbo].[SecurityAction] SET [order_no] = 4 WHERE [name] = 'CREATE_USER_DEFINED_EVENT' AND [namespace_id] = 'D4FF31CD-D9AC-46DF-8E92-5B545D191BAF'

/*UPDATING POSSITION UNDER MATRIX*/
UPDATE [dbo].[SecurityAction] SET [order_no] = 4 WHERE [name] = 'ADMIN_SECURITY' AND [namespace_id] = '988D493D-1597-4BE6-AFF0-396BFD5D047A'
UPDATE [dbo].[SecurityAction] SET [order_no] = 3 WHERE [name] = 'CREATE_MATRIX' AND [namespace_id] = '988D493D-1597-4BE6-AFF0-396BFD5D047A'

/*UPDATING POSSITION UNDER RULES*/
UPDATE [dbo].[SecurityAction] SET [order_no] = 4 WHERE [name] = 'ADMIN_SECURITY' AND [namespace_id] = 'C24C91A7-7D89-459B-B78E-C4548C74640D'
UPDATE [dbo].[SecurityAction] SET [order_no] = 3 WHERE [name] = 'CREATE_RULE' AND [namespace_id] = 'C24C91A7-7D89-459B-B78E-C4548C74640D'

GO