-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
DECLARE @WebhooksPluginId UNIQUEIDENTIFIER = '1A726DB5-29C5-461E-8FBE-75B11CA3F87D';

-- Delete existing references to WebhooksPluginPluginId (only present in 25.1)
IF EXISTS (SELECT 1 FROM [dbo].[SecurityAccessEntry] WHERE security_object_id IN (SELECT security_object_id FROM [dbo].[SecurityObject] WHERE namespace_id = @WebhooksPluginId))
BEGIN
    DELETE FROM [dbo].[SecurityAccessEntry] WHERE security_object_id IN (SELECT security_object_id FROM [dbo].[SecurityObject] WHERE namespace_id = @WebhooksPluginId);
END

IF EXISTS (SELECT 1 FROM [dbo].[SecurityObject] WHERE namespace_id = @WebhooksPluginId)
BEGIN
    DELETE FROM [dbo].[SecurityObject] WHERE namespace_id = @WebhooksPluginId;
END

IF EXISTS (SELECT 1 FROM [dbo].[SecurityAction] WHERE namespace_id = @WebhooksPluginId)
BEGIN
    DELETE FROM [dbo].[SecurityAction] WHERE namespace_id = @WebhooksPluginId;
END

IF EXISTS (SELECT 1 FROM [dbo].[SecurityNamespace] WHERE namespace_id = @WebhooksPluginId)
BEGIN
    DELETE FROM [dbo].[SecurityNamespace] WHERE namespace_id = @WebhooksPluginId;
END

IF EXISTS (SELECT 1 FROM [Mip].[Kinds] WHERE PluginId = @WebhooksPluginId)
BEGIN
    DELETE FROM [Mip].[Kinds] WHERE PluginId = @WebhooksPluginId;
END

IF EXISTS (SELECT 1 FROM [Mip].[IntegrationRegistrations] WHERE IntegrationId = @WebhooksPluginId)
BEGIN
    DELETE FROM [Mip].[IntegrationRegistrations] WHERE IntegrationId = @WebhooksPluginId;
END

GO