SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[Proc_CheckInfoRole]
(
	@UserId INT = NULL
)

AS
BEGIN
	SELECT _user.Id
	FROM Core_User as _user 
	INNER JOIN dbo.Core_UserRole AS ur ON ur.UserId = _user.Id AND ur.IsEnabled = 1
	WHERE _user.IsEnabled = 1 
	AND ur.RoleId = 1 AND _user.id = @UserId
END
GO

