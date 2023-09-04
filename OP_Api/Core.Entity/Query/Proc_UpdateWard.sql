SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROC Proc_UpdateWard
(
	@WardId INT = NULL
)
AS
BEGIN

	DECLARE @IsSuccess BIT = 1;
	DECLARE @Message NVARCHAR(200);
	DECLARE @CountWardIdt INT = 0;

	SELECT @CountWardIdt = COUNT(1) FROM dbo.Core_Ward WHERE Id=@WardId AND IsEnabled=1 AND @WardId Is NOT NULL;
	IF(@CountWardIdt=0)
	BEGIN
		SET @IsSuccess = 0;
		SET @Message = N'Thông tin phường xã không tồn tại';
	END
	--update quận huyện
	UPDATE dbo.Core_Ward
	SET IsEnabled = 0 
	WHERE id = @WardId
	--
	SELECT @IsSuccess as IsSuccess, @Message AS [Message]

END
GO
