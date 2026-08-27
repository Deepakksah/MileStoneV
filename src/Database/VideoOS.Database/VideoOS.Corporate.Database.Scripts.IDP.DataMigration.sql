IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ApiResources]') AND type in (N'U'))
BEGIN
	DELETE FROM [{source}].[IDP].[ApiResources] WHERE [Name] = N'registration' OR [Name] = N'management' OR [Name] = N'recorder' OR [Name] = N'datacollector' OR [Name] = N'managementserver' OR [Name] = N'failoverresponseserver' 
	SET IDENTITY_INSERT [{target}].dbo.[ApiResources] ON  
	INSERT INTO [{target}].dbo.[ApiResources]([Id],[Description],[DisplayName],[Enabled],[Name]) 
	SELECT [Id],[Description],[DisplayName],[Enabled],[Name] 
	FROM [{source}].[IDP].[ApiResources]
	SET IDENTITY_INSERT [{target}].dbo.[ApiResources] OFF
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[Clients]') AND type in (N'U'))
BEGIN
	DELETE FROM [{source}].[IDP].[Clients] WHERE [ClientId] = N'winauthclient'
	SET IDENTITY_INSERT [{target}].dbo.[Clients] ON  
	INSERT INTO [{target}].dbo.[Clients]([Id],[AbsoluteRefreshTokenLifetime],[AccessTokenLifetime],[AccessTokenType],[AllowAccessTokensViaBrowser],[AllowOfflineAccess],[AllowPlainTextPkce],[AllowRememberConsent],[AlwaysIncludeUserClaimsInIdToken],[AlwaysSendClientClaims],[AuthorizationCodeLifetime],[BackChannelLogoutSessionRequired],[BackChannelLogoutUri],[ClientClaimsPrefix],[ClientId],[ClientName],[ClientUri],[ConsentLifetime],[Description],[EnableLocalLogin],[Enabled],[FrontChannelLogoutSessionRequired],[FrontChannelLogoutUri],[IdentityTokenLifetime],[IncludeJwtId],[LogoUri],[PairWiseSubjectSalt],[ProtocolType],[RefreshTokenExpiration],[RefreshTokenUsage],[RequireClientSecret],[RequireConsent],[RequirePkce],[SlidingRefreshTokenLifetime],[UpdateAccessTokenClaimsOnRefresh])
	SELECT [Id],[AbsoluteRefreshTokenLifetime],[AccessTokenLifetime],[AccessTokenType],[AllowAccessTokensViaBrowser],[AllowOfflineAccess],[AllowPlainTextPkce],[AllowRememberConsent],[AlwaysIncludeUserClaimsInIdToken],[AlwaysSendClientClaims],[AuthorizationCodeLifetime],[BackChannelLogoutSessionRequired],[BackChannelLogoutUri],[ClientClaimsPrefix],[ClientId],[ClientName],[ClientUri],[ConsentLifetime],[Description],[EnableLocalLogin],[Enabled],[FrontChannelLogoutSessionRequired],[FrontChannelLogoutUri],[IdentityTokenLifetime],[IncludeJwtId],[LogoUri],[PairWiseSubjectSalt],[ProtocolType],[RefreshTokenExpiration],[RefreshTokenUsage],[RequireClientSecret],[RequireConsent],[RequirePkce],[SlidingRefreshTokenLifetime],[UpdateAccessTokenClaimsOnRefresh] 
	FROM [{source}].[IDP].[Clients]
	SET IDENTITY_INSERT [{target}].dbo.[Clients] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[IdentityResources]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[IdentityResources] ON  
	INSERT INTO [{target}].dbo.[IdentityResources]([Id],[Description],[DisplayName],[Emphasize],[Enabled],[Name],[Required],[ShowInDiscoveryDocument])
	SELECT [Id],[Description],[DisplayName],[Emphasize],[Enabled],[Name],[Required],[ShowInDiscoveryDocument] 
	FROM [{source}].[IDP].[IdentityResources]
	SET IDENTITY_INSERT [{target}].dbo.[IdentityResources] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ApiClaims]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[ApiClaims] ON  
	INSERT INTO [{target}].dbo.[ApiClaims]([Id],[ApiResourceId],[Type]) 
	SELECT [Id],[ApiResourceId],[Type] 
	FROM [{source}].[IDP].[ApiClaims] 
	SET IDENTITY_INSERT [{target}].dbo.[ApiClaims] OFF
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ApiScopes]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[ApiScopes] ON  
	INSERT INTO [{target}].dbo.[ApiScopes]([Id],[ApiResourceId],[Description],[DisplayName],[Emphasize],[Name],[Required],[ShowInDiscoveryDocument])
	SELECT [Id],[ApiResourceId],[Description],[DisplayName],[Emphasize],[Name],[Required],[ShowInDiscoveryDocument] 
	FROM [{source}].[IDP].[ApiScopes]
	SET IDENTITY_INSERT [{target}].dbo.[ApiScopes] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ApiSecrets]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[ApiSecrets] ON  
	INSERT INTO [{target}].dbo.[ApiSecrets]([Id],[ApiResourceId],[Description],[Expiration],[Type],[Value]) 
	SELECT [Id],[ApiResourceId],[Description],[Expiration],[Type],[Value] 
	FROM [{source}].[IDP].[ApiSecrets]
	SET IDENTITY_INSERT [{target}].dbo.[ApiSecrets] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientClaims]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[ClientClaims] ON  
	INSERT INTO [{target}].dbo.[ClientClaims]([Id],[ClientId],[Type],[Value]) 
	SELECT [Id],[ClientId],[Type],[Value] 
	FROM [{source}].[IDP].[ClientClaims]
	SET IDENTITY_INSERT [{target}].dbo.[ClientClaims] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientCorsOrigins]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[ClientCorsOrigins] ON  
	INSERT INTO [{target}].dbo.[ClientCorsOrigins]([Id],[ClientId],[Origin]) 
	SELECT [Id],[ClientId],[Origin] 
	FROM [{source}].[IDP].[ClientCorsOrigins]
	SET IDENTITY_INSERT [{target}].dbo.[ClientCorsOrigins] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientGrantTypes]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[ClientGrantTypes] ON  
	INSERT INTO [{target}].dbo.[ClientGrantTypes]([Id],[ClientId],[GrantType]) 
	SELECT [Id],[ClientId],[GrantType] 
	FROM [{source}].[IDP].[ClientGrantTypes]
	SET IDENTITY_INSERT [{target}].dbo.[ClientGrantTypes] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientIdPRestrictions]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[ClientIdPRestrictions] ON  
	INSERT INTO [{target}].dbo.[ClientIdPRestrictions]([Id],[ClientId],[Provider]) 
	SELECT [Id],[ClientId],[Provider] 
	FROM [{source}].[IDP].[ClientIdPRestrictions]
	SET IDENTITY_INSERT [{target}].dbo.[ClientIdPRestrictions] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientPostLogoutRedirectUris]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[ClientPostLogoutRedirectUris] ON  
	INSERT INTO [{target}].dbo.[ClientPostLogoutRedirectUris]([Id],[ClientId],[PostLogoutRedirectUri]) 
	SELECT [Id],[ClientId],[PostLogoutRedirectUri] 
	FROM [{source}].[IDP].[ClientPostLogoutRedirectUris]
	SET IDENTITY_INSERT [{target}].dbo.[ClientPostLogoutRedirectUris] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientProperties]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[ClientProperties] ON  
	INSERT INTO [{target}].dbo.[ClientProperties]([Id],[ClientId],[Key],[Value]) 
	SELECT [Id],[ClientId],[Key],[Value] 
	FROM [{source}].[IDP].[ClientProperties]
	SET IDENTITY_INSERT [{target}].dbo.[ClientProperties] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientRedirectUris]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[ClientRedirectUris] ON  
	INSERT INTO [{target}].dbo.[ClientRedirectUris]([Id],[ClientId],[RedirectUri]) 
	SELECT [Id],[ClientId],[RedirectUri] 
	FROM [{source}].[IDP].[ClientRedirectUris]
	SET IDENTITY_INSERT [{target}].dbo.[ClientRedirectUris] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientScopes]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[ClientScopes] ON  
	INSERT INTO [{target}].dbo.[ClientScopes]([Id],[ClientId],[Scope]) 
	SELECT [Id],[ClientId],[Scope] 
	FROM [{source}].[IDP].[ClientScopes]
	SET IDENTITY_INSERT [{target}].dbo.[ClientScopes] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientSecrets]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[ClientSecrets] ON  
	INSERT INTO [{target}].dbo.[ClientSecrets]([Id],[ClientId],[Description],[Expiration],[Type],[Value]) 
	SELECT [Id],[ClientId],[Description],[Expiration],[Type],[Value] 
	FROM [{source}].[IDP].[ClientSecrets]
	SET IDENTITY_INSERT [{target}].dbo.[ClientSecrets] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[IdentityClaims]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[IdentityClaims] ON  
	INSERT INTO [{target}].dbo.[IdentityClaims]([Id],[IdentityResourceId],[Type]) 
	SELECT [Id],[IdentityResourceId],[Type] 
	FROM [{source}].[IDP].[IdentityClaims]
	SET IDENTITY_INSERT [{target}].dbo.[IdentityClaims] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[PersistedGrants]') AND type in (N'U'))
BEGIN
	INSERT INTO [{target}].dbo.[PersistedGrants]([Key],[Type],[SubjectId],[ClientId],[CreationTime],[Expiration],[Data]) 
	SELECT [Key],[Type],[SubjectId],[ClientId],[CreationTime],[Expiration],[Data] 
	FROM [{source}].[IDP].[PersistedGrants]
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[SecurityKey]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[SecurityKey] ON  
	INSERT INTO [{target}].dbo.[SecurityKey]([Id],[KeyId],[Expiration],[D],[DP],[DQ],[Exponent],[InverseQ],[Modulus],[P],[Q],[Created]) 
	SELECT [Id],[KeyId],[Expiration],[D],[DP],[DQ],[Exponent],[InverseQ],[Modulus],[P],[Q],[Created] 
	FROM [{source}].[IDP].[SecurityKey]
	SET IDENTITY_INSERT [{target}].dbo.[SecurityKey] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ApiScopeClaims]') AND type in (N'U'))
BEGIN
	SET IDENTITY_INSERT [{target}].dbo.[ApiScopeClaims] ON  
	INSERT INTO [{target}].dbo.[ApiScopeClaims]([Id],[ApiScopeId],[Type]) 
	SELECT [Id],[ApiScopeId],[Type] 
	FROM [{source}].[IDP].[ApiScopeClaims]
	SET IDENTITY_INSERT [{target}].dbo.[ApiScopeClaims] OFF  
END 
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ApiScopeClaims]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[ApiScopeClaims]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[SecurityKey]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[SecurityKey]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[PersistedGrants]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[PersistedGrants]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[IdentityClaims]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[IdentityClaims]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientSecrets]') AND type in (N'U'))
BEGIN	
	DROP TABLE [{source}].[IDP].[ClientSecrets]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientScopes]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[ClientScopes]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientRedirectUris]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[ClientRedirectUris]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientProperties]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[ClientProperties]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientPostLogoutRedirectUris]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[ClientPostLogoutRedirectUris]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientIdPRestrictions]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[ClientIdPRestrictions]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientGrantTypes]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[ClientGrantTypes]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientCorsOrigins]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[ClientCorsOrigins]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ClientClaims]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[ClientClaims]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ApiSecrets]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[ApiSecrets]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ApiScopes]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[ApiScopes]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ApiClaims]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[ApiClaims] 
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[IdentityResources]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[IdentityResources]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[Clients]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[Clients]
END

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].[IDP].[ApiResources]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].[IDP].[ApiResources]
END
GO
