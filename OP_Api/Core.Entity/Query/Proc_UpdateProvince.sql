SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROC Proc_UpdateProvince
(
	@ProvinceId INT = NULL
)
AS
BEGIN

	DECLARE @IsSuccess BIT = 1;
	DECLARE @Message NVARCHAR(200);
	DECLARE @tableDistrict TABLE (Id INT);
	DECLARE @CountProvince INT = 0;

	SELECT @CountProvince = COUNT(1) FROM dbo.Core_Province WHERE Id=@ProvinceId AND IsEnabled=1 AND @ProvinceId Is NOT NULL;
	IF(@CountProvince=0)
	BEGIN
		SET @IsSuccess = 0;
		SET @Message = N'Thông tin tỉnh thành không tồn tại';
	END
	--delete quốc gia
	UPDATE dbo.Core_Province
	SET IsEnabled = 0 
	WHERE id = @ProvinceId
	--update quận huyện theo tỉnh thành
	UPDATE dbo.Core_District
	SET IsEnabled = 0
	WHERE ProvinceId  = @ProvinceId
	INSERT INTO @tableDistrict (Id) (SELECT Id FROM dbo.Core_District AS District WHERE District.ProvinceId  = @ProvinceId)
	--update phường xã theo tỉnh thành
	UPDATE dbo.Core_Ward
	SET IsEnabled = 0
	WHERE DistrictId IN (SELECT id FROM @tableDistrict)
	--
	SELECT @IsSuccess as IsSuccess, @Message AS [Message]

END
GO
