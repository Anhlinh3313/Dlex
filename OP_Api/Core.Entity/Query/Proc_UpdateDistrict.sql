SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROC Proc_UpdateDistrict
(
	@DistrictId INT = NULL
)
AS
BEGIN

	DECLARE @IsSuccess BIT = 1;
	DECLARE @Message NVARCHAR(200);
	DECLARE @CountDistrict INT = 0;

	SELECT @CountDistrict = COUNT(1) FROM dbo.Core_District WHERE Id=@DistrictId AND IsEnabled=1 AND @DistrictId Is NOT NULL;
	IF(@CountDistrict=0)
	BEGIN
		SET @IsSuccess = 0;
		SET @Message = N'Thông tin quận huyện không tồn tại';
	END
	--update quận huyện
	UPDATE dbo.Core_District
	SET IsEnabled = 0 
	WHERE id = @DistrictId
	--update phường xã theo quận huyện
	UPDATE dbo.Core_Ward
	SET IsEnabled = 0
	WHERE DistrictId = @DistrictId
	--
	SELECT @IsSuccess as IsSuccess, @Message AS [Message]

END
GO

