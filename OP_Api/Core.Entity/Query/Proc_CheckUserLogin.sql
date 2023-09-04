SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[Proc_CheckUserLogin]
(
	@Email NVARCHAR(250) = NULL
)

AS
BEGIN

	SELECT 
		_user.Id, _user.Code, _user.FullName AS Name, _user.Email, _user.TypeUserId, _user.CompanyId, _user.SecurityStamp, _user.PasswordHash,
		_user.CodeResetPassWord, _user.ResetPassWordSentat, _user.IsPassWordBasic
	FROM dbo.Core_User as _user 
	WHERE _user.IsEnabled=1 AND _user.Email = @Email
	OR _user.PhoneNumber = @Email

END
GO