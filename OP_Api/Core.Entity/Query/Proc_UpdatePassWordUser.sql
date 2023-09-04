SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC Proc_UpdatePassWordUser
(
	@Id INT,
	@PasswordHash NVARCHAR(MAX) = NULL,
	@CodeResetPassWord NVARCHAR(MAX) = NULL,
	@ResetPassWordSentat DateTime,
	@IsPassWordBasic bit
) AS
BEGIN

	UPDATE dbo.Core_User
	SET 
		PasswordHash = @PasswordHash,
		CodeResetPassWord = @CodeResetPassWord,
		ResetPassWordSentat = @ResetPassWordSentat,
		IsPassWordBasic = @IsPassWordBasic
	WHERE id = @Id
	DECLARE @Result BIT
	SET @Result =1
	SELECT @Result AS Result
END