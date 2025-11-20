-- =============================================
-- User Management Stored Procedures
-- =============================================

USE [MarketStaffIndustrial]
GO

-- =============================================
-- User Operations
-- =============================================

-- Get User By Email
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[msinet_GetUserByEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[msinet_GetUserByEmail]
GO

CREATE PROCEDURE [dbo].[msinet_GetUserByEmail]
    @email nvarchar(256)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        u.UserId, 
        u.UserName, 
        m.Email, 
        m.IsApproved, 
        m.IsLockedOut, 
        m.CreateDate, 
        m.LastLoginDate, 
        u.LastActivityDate
    FROM 
        dbo.aspnet_Users u
        INNER JOIN dbo.aspnet_Membership m ON u.UserId = m.UserId
    WHERE 
        m.Email = @email
END
GO

-- Get User By ID
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[msinet_GetUserById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[msinet_GetUserById]
GO

CREATE PROCEDURE [dbo].[msinet_GetUserById]
    @userId uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        u.UserId, 
        u.UserName, 
        m.Email, 
        m.IsApproved, 
        m.IsLockedOut, 
        m.CreateDate, 
        m.LastLoginDate, 
        u.LastActivityDate
    FROM 
        dbo.aspnet_Users u
        INNER JOIN dbo.aspnet_Membership m ON u.UserId = m.UserId
    WHERE 
        u.UserId = @userId
END
GO

-- Update User
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[msinet_UpdateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[msinet_UpdateUser]
GO

CREATE PROCEDURE [dbo].[msinet_UpdateUser]
    @username nvarchar(256),
    @email nvarchar(256),
    @isApproved bit
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @userId uniqueidentifier
    SELECT @userId = UserId FROM dbo.aspnet_Users WHERE UserName = @username
    
    IF @userId IS NULL
        RETURN 1 -- User not found
    
    BEGIN TRY
        BEGIN TRANSACTION
        
        UPDATE dbo.aspnet_Membership
        SET 
            Email = @email,
            LoweredEmail = LOWER(@email),
            IsApproved = @isApproved
        WHERE 
            UserId = @userId
        
        COMMIT TRANSACTION
        RETURN 0 -- Success
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION
        
        RETURN -1 -- Error
    END CATCH
END
GO

-- =============================================
-- Client Membership Operations
-- =============================================

-- Add User to Client
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[msinet_AddUserToClient]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[msinet_AddUserToClient]
GO

CREATE PROCEDURE [dbo].[msinet_AddUserToClient]
    @username nvarchar(256),
    @clientId int,
    @preferredClient bit,
    @canViewVoidedClients bit
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @userId uniqueidentifier
    SELECT @userId = UserId FROM dbo.aspnet_Users WHERE UserName = @username
    
    IF @userId IS NULL
        RETURN 1 -- User not found
    
    -- Check if client exists
    IF NOT EXISTS (SELECT 1 FROM dbo.client WHERE client_id = @clientId)
        RETURN 2 -- Client not found
    
    -- Check if user already has access to this client
    IF EXISTS (SELECT 1 FROM dbo.client_membership WHERE UserId = @userId AND client_id = @clientId)
        RETURN 3 -- User already has access to this client
    
    BEGIN TRY
        BEGIN TRANSACTION
        
        -- If this is set as preferred client, reset all other preferred flags for this user
        IF @preferredClient = 1
        BEGIN
            UPDATE dbo.client_membership
            SET preferred_client = 0
            WHERE UserId = @userId
        END
        
        -- Add user to client
        INSERT INTO dbo.client_membership (UserId, client_id, preferred_client, CanViewVoidedClients)
        VALUES (@userId, @clientId, @preferredClient, @canViewVoidedClients)
        
        COMMIT TRANSACTION
        RETURN 0 -- Success
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION
        
        RETURN -1 -- Error
    END CATCH
END
GO

-- Remove User from Client
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[msinet_RemoveUserFromClient]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[msinet_RemoveUserFromClient]
GO

CREATE PROCEDURE [dbo].[msinet_RemoveUserFromClient]
    @username nvarchar(256),
    @clientId int
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @userId uniqueidentifier
    SELECT @userId = UserId FROM dbo.aspnet_Users WHERE UserName = @username
    
    IF @userId IS NULL
        RETURN 1 -- User not found
    
    -- Check if client exists
    IF NOT EXISTS (SELECT 1 FROM dbo.client WHERE client_id = @clientId)
        RETURN 2 -- Client not found
    
    -- Check if user has access to this client
    IF NOT EXISTS (SELECT 1 FROM dbo.client_membership WHERE UserId = @userId AND client_id = @clientId)
        RETURN 3 -- User does not have access to this client
    
    BEGIN TRY
        BEGIN TRANSACTION
        
        -- First, remove any department access for this client
        DELETE FROM dbo.user_department
        WHERE client_membership_id IN (
            SELECT client_membership_id 
            FROM dbo.client_membership 
            WHERE UserId = @userId AND client_id = @clientId
        )
        
        -- Then remove client membership
        DELETE FROM dbo.client_membership
        WHERE UserId = @userId AND client_id = @clientId
        
        COMMIT TRANSACTION
        RETURN 0 -- Success
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION
        
        RETURN -1 -- Error
    END CATCH
END
GO

-- Set Preferred Client
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[msinet_SetPreferredClient]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[msinet_SetPreferredClient]
GO

CREATE PROCEDURE [dbo].[msinet_SetPreferredClient]
    @username nvarchar(256),
    @clientId int
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @userId uniqueidentifier
    SELECT @userId = UserId FROM dbo.aspnet_Users WHERE UserName = @username
    
    IF @userId IS NULL
        RETURN 1 -- User not found
    
    -- Check if client exists
    IF NOT EXISTS (SELECT 1 FROM dbo.client WHERE client_id = @clientId)
        RETURN 2 -- Client not found
    
    -- Check if user has access to this client
    IF NOT EXISTS (SELECT 1 FROM dbo.client_membership WHERE UserId = @userId AND client_id = @clientId)
        RETURN 3 -- User does not have access to this client
    
    BEGIN TRY
        BEGIN TRANSACTION
        
        -- Reset all preferred flags for this user
        UPDATE dbo.client_membership
        SET preferred_client = 0
        WHERE UserId = @userId
        
        -- Set the specified client as preferred
        UPDATE dbo.client_membership
        SET preferred_client = 1
        WHERE UserId = @userId AND client_id = @clientId
        
        COMMIT TRANSACTION
        RETURN 0 -- Success
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION
        
        RETURN -1 -- Error
    END CATCH
END
GO

-- =============================================
-- Department Operations
-- =============================================

-- Add User to Department
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[msinet_AddUserToDepartment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[msinet_AddUserToDepartment]
GO

CREATE PROCEDURE [dbo].[msinet_AddUserToDepartment]
    @username nvarchar(256),
    @clientId int,
    @departmentId int,
    @overrideRoleId uniqueidentifier = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @userId uniqueidentifier
    SELECT @userId = UserId FROM dbo.aspnet_Users WHERE UserName = @username
    
    IF @userId IS NULL
        RETURN 1 -- User not found
    
    -- Check if client exists
    IF NOT EXISTS (SELECT 1 FROM dbo.client WHERE client_id = @clientId)
        RETURN 2 -- Client not found
    
    -- Check if department exists
    IF NOT EXISTS (SELECT 1 FROM dbo.department WHERE department_id = @departmentId)
        RETURN 3 -- Department not found
    
    -- Check if department belongs to client
    IF NOT EXISTS (
        SELECT 1 
        FROM dbo.client_shift_location 
        WHERE client_id = @clientId AND department_id = @departmentId
    )
        RETURN 4 -- Department does not belong to client
    
    -- Get client membership ID
    DECLARE @clientMembershipId int
    SELECT @clientMembershipId = client_membership_id 
    FROM dbo.client_membership 
    WHERE UserId = @userId AND client_id = @clientId
    
    IF @clientMembershipId IS NULL
    BEGIN
        -- User does not have access to this client, add them first
        INSERT INTO dbo.client_membership (UserId, client_id, preferred_client, CanViewVoidedClients)
        VALUES (@userId, @clientId, 0, 0)
        
        SET @clientMembershipId = SCOPE_IDENTITY()
    END
    
    -- Check if user already has access to this department
    IF EXISTS (
        SELECT 1 
        FROM dbo.user_department 
        WHERE client_membership_id = @clientMembershipId AND department_id = @departmentId
    )
        RETURN 5 -- User already has access to this department
    
    BEGIN TRY
        BEGIN TRANSACTION
        
        -- Add user to department
        INSERT INTO dbo.user_department (client_membership_id, department_id, override_role_id)
        VALUES (@clientMembershipId, @departmentId, @overrideRoleId)
        
        COMMIT TRANSACTION
        RETURN 0 -- Success
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION
        
        RETURN -1 -- Error
    END CATCH
END
GO

-- Remove User from Department
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[msinet_RemoveUserFromDepartment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[msinet_RemoveUserFromDepartment]
GO

CREATE PROCEDURE [dbo].[msinet_RemoveUserFromDepartment]
    @username nvarchar(256),
    @clientId int,
    @departmentId int
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @userId uniqueidentifier
    SELECT @userId = UserId FROM dbo.aspnet_Users WHERE UserName = @username
    
    IF @userId IS NULL
        RETURN 1 -- User not found
    
    -- Get client membership ID
    DECLARE @clientMembershipId int
    SELECT @clientMembershipId = client_membership_id 
    FROM dbo.client_membership 
    WHERE UserId = @userId AND client_id = @clientId
    
    IF @clientMembershipId IS NULL
        RETURN 2 -- User does not have access to this client
    
    -- Check if user has access to this department
    IF NOT EXISTS (
        SELECT 1 
        FROM dbo.user_department 
        WHERE client_membership_id = @clientMembershipId AND department_id = @departmentId
    )
        RETURN 3 -- User does not have access to this department
    
    BEGIN TRY
        BEGIN TRANSACTION
        
        -- Remove user from department
        DELETE FROM dbo.user_department
        WHERE client_membership_id = @clientMembershipId AND department_id = @departmentId
        
        COMMIT TRANSACTION
        RETURN 0 -- Success
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION
        
        RETURN -1 -- Error
    END CATCH
END
GO

-- Get Departments by User
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[msinet_GetDepartmentsByUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[msinet_GetDepartmentsByUser]
GO

CREATE PROCEDURE [dbo].[msinet_GetDepartmentsByUser]
    @username nvarchar(256),
    @clientId int
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @userId uniqueidentifier
    SELECT @userId = UserId FROM dbo.aspnet_Users WHERE UserName = @username
    
    IF @userId IS NULL
        RETURN -- User not found
    
    -- Get client membership ID
    DECLARE @clientMembershipId int
    SELECT @clientMembershipId = client_membership_id 
    FROM dbo.client_membership 
    WHERE UserId = @userId AND client_id = @clientId
    
    IF @clientMembershipId IS NULL
        RETURN -- User does not have access to this client
    
    -- Get departments for this user and client
    SELECT 
        d.department_id,
        d.department_name,
        @clientId AS client_id,
        ud.override_role_id
    FROM 
        dbo.department d
        INNER JOIN dbo.user_department ud ON d.department_id = ud.department_id
    WHERE 
        ud.client_membership_id = @clientMembershipId
    ORDER BY 
        d.department_name
END
GO