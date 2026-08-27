-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[site_syncronize_slave_site]') AND type = 'P')
DROP PROCEDURE [site_syncronize_slave_site]
GO

CREATE PROCEDURE [site_syncronize_slave_site]
	@SiteXmlDefinition Xml
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		DECLARE @idoc int

		EXEC sp_xml_preparedocument @idoc OUTPUT, @SiteXmlDefinition
	
		DECLARE site_cursor CURSOR
			FOR SELECT [SiteId],
					[MasterSiteId],
					[Name],
					[Description],
					CAST([Addresses] as XML),
					CAST([LastModified] AS DATETIME),
					CAST([LastConfirmedHandshake] AS DATETIME),
					[SiteState],
					[Version],
					[ServiceAccount],
					[SynchronizationStatus]
		FROM OPENXML (@idoc, '/Sites/Site')
			WITH 
			(
				[SiteId]					GuidType 'SiteId',
				[MasterSiteId]				GuidType 'MasterSiteId',
				[Name]						NameType 'Name',
				[Description]				NVARCHAR(MAX) 'Description',
				[Addresses]					NVARCHAR(MAX) 'Addresses',
				[LastModified]				VARCHAR(50) 'LastModified',
				[LastConfirmedHandshake]	VARCHAR(50) 'LastConfirmedHandshake',
				[SiteState]					INT 'SiteState',
				[Version]					NVARCHAR(15) 'Version',
				[ServiceAccount]			NVARCHAR(256) 'ServiceAccount',
				[SynchronizationStatus]		INT 'SynchronizationStatus'
			)

		DECLARE @IDSlaveSite GuidType
		DECLARE @MasterSiteId GuidType
		DECLARE @Name NameType
		DECLARE @Description VARCHAR(MAX)
		DECLARE @Addresses XML
		DECLARE @LastModified DATETIME
		DECLARE @LastConfirmedHandshake DATETIME
		DECLARE @SiteState INT
		DECLARE @Version VARCHAR(15)
		DECLARE @ServiceAccount VARCHAR(256)
		DECLARE @SynchronizationStatus INT
	
		DECLARE @IDSiteToSync [GuidType]
	
		DECLARE @HomeSiteId GuidType
		SET @HomeSiteId = (SELECT [IDSite] 
			FROM Sites 
			WHERE IDSiteType = 1)
	
		DECLARE @SitesInSync TABLE([IDSite] GuidType) 
	
		OPEN site_cursor

		BEGIN TRY
			FETCH NEXT FROM site_cursor 
				INTO @IDSlaveSite, @MasterSiteId, @Name, @Description, @Addresses, @LastModified, @LastConfirmedHandshake, @SiteState, @Version, @ServiceAccount, @SynchronizationStatus
			WHILE @@FETCH_STATUS = 0
			BEGIN
				IF((SELECT COUNT(*) FROM Sites WHERE [IDSite] = @IDSlaveSite) = 0)
				BEGIN
					EXEC [create_slave_site_transaction]
						@IDSite = @IDSlaveSite,
						@Name = @Name,
						@Description = @Description,
						@IDSiteState = @SiteState,
						@Addresses = @Addresses,
						@IDMasterSite = @MasterSiteId,
						@LastModified = @LastModified,
						@LastStatusHandshake = @LastConfirmedHandshake,
						@Version = @Version,
						@ServiceAccount = @ServiceAccount,
						@SynchronizationStatus = @SynchronizationStatus
				END
				ELSE IF(@MasterSiteId = @HomeSiteId)
				BEGIN
					UPDATE [Sites] 
					SET [Name] = @Name, 
						[Description] = @Description,
						[Addresses] = @Addresses,
						[LastModified] = @LastModified,
						[LastStatusHandshake] = @CurrentUtcTime,
						[IDSiteState] = @SiteState,
						[Version] = @Version,
						[ServiceAccount] = @ServiceAccount,
						[SynchronizationStatus] = @SynchronizationStatus
					WHERE IDSite = @IDSlaveSite
		
					SET @IDSiteToSync = @IDSlaveSite
				END
				ELSE IF((SELECT COUNT(*) FROM SitesRelation WHERE IDSiteMaster = @MasterSiteId AND IDSiteSlave = @IDSlaveSite) = 1)
				BEGIN
					UPDATE [Sites] 
					SET [Name] = @Name, 
						[Description] = @Description,
						[Addresses] = @Addresses,
						[LastModified] = @LastModified,
						[LastStatusHandshake] = @LastConfirmedHandshake,
						[IDSiteState] = @SiteState,
						[Version] = @Version,
						[ServiceAccount] = @ServiceAccount,
						[SynchronizationStatus] = @SynchronizationStatus
					WHERE IDSite = @IDSlaveSite
				END
				ELSE
				BEGIN
					EXEC [delete_site_transaction] @IDSite = @IDSlaveSite
			
					EXEC [create_slave_site_transaction]
						@IDSite = @IDSlaveSite,
						@Name = @Name,
						@Description = @Description,
						@IDSiteState = @SiteState,
						@Addresses = @Addresses,
						@IDMasterSite = @MasterSiteId,
						@LastModified = @LastModified,
						@LastStatusHandshake = @LastConfirmedHandshake,
						@Version = @Version,
						@ServiceAccount = @ServiceAccount,
						@SynchronizationStatus = @SynchronizationStatus
				END
	
				INSERT INTO @SitesInSync
				SELECT @IDSlaveSite

				FETCH NEXT FROM site_cursor 
					INTO @IDSlaveSite, @MasterSiteId, @Name, @Description, @Addresses, @LastModified, @LastConfirmedHandshake, @SiteState, @Version, @ServiceAccount, @SynchronizationStatus
			END

			CLOSE site_cursor
			DEALLOCATE site_cursor
		END TRY
		BEGIN CATCH
			CLOSE site_cursor
			DEALLOCATE site_cursor
			;THROW
		END CATCH

		DECLARE @SitesInTree TABLE([IDSite] GuidType)
		;WITH SitesCTE(IDSite) AS
		(
			SELECT @IDSiteToSync

			UNION ALL

			SELECT	sr.[IDSiteSlave]
			FROM SitesCTE AS P
			INNER JOIN SitesRelation sr ON sr.IDSiteMaster = P.IDSite
		)
	
		INSERT INTO @SitesInTree
		SELECT DISTINCT [IDSite] FROM SitesCTE

		IF((SELECT COUNT(*) FROM @SitesInTree WHERE [IDSite] NOT IN (SELECT [IDSite] FROM @SitesInSync)) > 0)
		BEGIN
			DECLARE sites_to_delete_cursor CURSOR
				FOR SELECT [IDSite]
				FROM @SitesInTree 
				WHERE [IDSite] NOT IN (SELECT [IDSite] FROM @SitesInSync)
	
			DECLARE @DeletedSiteId GuidType
	
			OPEN sites_to_delete_cursor
			BEGIN TRY
				FETCH NEXT FROM sites_to_delete_cursor INTO @DeletedSiteId
				WHILE @@FETCH_STATUS = 0
				BEGIN
					IF((SELECT COUNT(IDSite) FROM SITES WHERE [IDSite] = @DeletedSiteId) = 1)
					BEGIN
						EXEC [delete_site_transaction] @IDSite = @DeletedSiteId
					END
					FETCH NEXT FROM sites_to_delete_cursor INTO @DeletedSiteId
				END
			END TRY
			BEGIN CATCH
				CLOSE sites_to_delete_cursor
				DEALLOCATE sites_to_delete_cursor
				;THROW
			END CATCH
		
			CLOSE sites_to_delete_cursor
			DEALLOCATE sites_to_delete_cursor
		END
	
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
	END CATCH;
END
GO