-- This header is used by the dbupdate tool.
-- The TARGET poperty is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_basic_user]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_basic_user]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_basic_user_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_basic_user_by_id]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_basic_user_by_name]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_basic_user_by_name]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_basic_user_by_sid]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_basic_user_by_sid]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_basic_user_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_basic_user_ids]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_basic_users]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_basic_users]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_basic_users_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_basic_users_by_ids]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_basic_user]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_basic_user]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_basic_user_change_password]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_basic_user_change_password]
GO