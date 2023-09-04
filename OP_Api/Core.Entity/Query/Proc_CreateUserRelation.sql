SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC Proc_CreateUserRelation
(
	@Code NVARCHAR(MAX) = NULL,
	@Name NVARCHAR(MAX) = NULL,
	@UserId INT,
	@UserRelationId INT,
	@CompanyId INT
) AS
BEGIN
	INSERT dbo.Core_UserRelation
	(
	    CreatedWhen,
	    CreatedBy,
	    ModifiedWhen,
	    ModifiedBy,
	    IsEnabled,
	    ConcurrencyStamp,
	    Code,
	    Name,
	    UserId,
	    UserRelationId,
	    CompanyId
	)
	VALUES
	(   GETDATE(), -- CreatedWhen - datetime
	    @UserId,         -- CreatedBy - int
	    GETDATE(), -- ModifiedWhen - datetime
	    0,         -- ModifiedBy - int
	    1,      -- IsEnabled - bit
	    N'',       -- ConcurrencyStamp - nvarchar(max)
	    @Code,       -- Code - nvarchar(100)
	    @Name,       -- Name - nvarchar(300)
	    @UserId,         -- UserId - int
	    @UserRelationId,         -- UserRelationId - int
	    @CompanyId          -- CompanyId - int
	    )
	DECLARE @Result BIT
	SET @Result = 1
	SELECT @Result AS Result
END
GO

