SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROC [dbo].[Proc_CheckInfoLogin]
(
	@UserName NVARCHAR(100) = NULL,
	@CompanyCode NVARCHAR(10) = NULL,
	@TypeUserId INT = NULL
)

AS
BEGIN
	SELECT 
	_user.Id, _user.PasswordHash, _user.SecurityStamp,_user.UserName,_user.FullName, _user.IsBlocked, _user.IsPassWordBasic, _user.CompanyId, 
	com.Code AS CompanyCode, _user.TypeUserId
	FROM Core_User as _user 
	INNER JOIN Core_Company as com ON com.Id = _user.CompanyId AND com.IsEnabled=1
	WHERE _user.IsEnabled=1 
		 AND com.Code = @CompanyCode AND @CompanyCode IS NOT NULL
		 AND (_user.UserName = @UserName OR _user.Email = @UserName OR _user.PhoneNumber = @UserName)
		 AND _user.TypeUserId = @TypeUserId
END
GO

