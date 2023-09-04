SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROC Proc_UpdateCountry
(
	@CountryId INT = NULL
)
as
BEGIN

	DECLARE @IsSuccess BIT = 1;
	DECLARE @Message NVARCHAR(200);
	DECLARE @tableProvince TABLE (Id INT);
	DECLARE @tableDistrict TABLE (Id INT);
	DECLARE @CountCountry INT = 0;

	SELECT @CountCountry = COUNT(1) FROM dbo.Core_Country WHERE Id=@CountryId AND IsEnabled=1 AND @CountryId Is NOT NULL;
	IF(@CountCountry=0)
	BEGIN
		SET @IsSuccess = 0;
		SET @Message = N'Thông tin quốc gia không tồn tại';
	END

	--delete quốc gia
	UPDATE dbo.Core_Country
	SET IsEnabled = 0 
	WHERE id = @CountryId
	--lấy danh tỉnh thành theo quốc gia rồi xoá
	UPDATE dbo.Core_Province
	SET IsEnabled = 0 
	WHERE CountryId IN (SELECT Id FROM dbo.Core_Country WHERE Id = @CountryId)
	INSERT INTO @tableProvince (Id) (SELECT Id FROM dbo.Core_Province AS Province WHERE Province.CountryId = @CountryId)
	--update quận huyện theo tỉnh thành
	UPDATE dbo.Core_District
	SET IsEnabled = 0
	WHERE ProvinceId IN (SELECT Id FROM @tableProvince)
	INSERT INTO @tableDistrict (Id) 
	(SELECT Id FROM dbo.Core_District AS District WHERE District.ProvinceId IN (SELECT Id FROM @tableProvince))
	--update phường xã theo tỉnh thành
	UPDATE dbo.Core_Ward
	SET IsEnabled = 0
	WHERE DistrictId IN (SELECT id FROM @tableDistrict)
	--update hubCenter 
	UPDATE dbo.Core_Hub
	SET IsEnabled = 0
	WHERE CenterHubId IS NULL AND POHubId IS NULL AND ProvinceId IN (SELECT Id FROM @tableProvince)
	-- update poHub
	UPDATE dbo.Core_Hub
	SET IsEnabled = 0
	WHERE CenterHubId IS NOT NULL AND POHubId IS NULL AND ProvinceId IN (SELECT Id FROM @tableProvince)
	--update stationHub
	UPDATE dbo.Core_Hub
	SET IsEnabled = 0
	WHERE CenterHubId IS NOT NULL AND POHubId IS NOT NULL AND ProvinceId IN (SELECT Id FROM @tableProvince)
	--
	SELECT @IsSuccess as IsSuccess, @Message AS [Message]

END
GO

