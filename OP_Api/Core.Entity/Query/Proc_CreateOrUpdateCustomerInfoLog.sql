CREATE PROC [dbo].[Proc_CreateOrUpdateCustomerInfoLog]
(
@Code VARCHAR(50) = NULL,
@Name NVARCHAR(200) = NULL,
@PhoneNumber NVARCHAR(50) NULL,
@CompanyName NVARCHAR(200) = NULL,
@Address NVARCHAR(200) = NULL,
@AddressNote NVARCHAR(200) = NULL,
@ProvinceId INT = NULL,
@DistrictId INT = NULL,
@WardId INT  = NULL,
@Lat FLOAT = NULL,
@Lng FLOAT = NULL,
@SenderId INT = NULL,
@CompanyId INT = NULL
)as
BEGIN
	DECLARE @IsSuccess BIT = 0;
	DECLARE @Message NVARCHAR(500);
	IF @SenderId IS NULL SET @Message = N'#PROC: Vui lòng chọn khách hàng gửi.';
	ELSE IF @Code IS NULL SET @Message = N'#PROC: Vui lòng nhập mã khách nhận.';
	ELSE IF (SELECT COUNT(1) FROM Crm_Customer WHERE Id=@SenderId)=0 SET @Message = N'#PROC: Không tìm thấy không tin khách hàng gửi.';
	ELSE IF (SELECT COUNT(1) FROM Crm_Customer WHERE CompanyId=@CompanyId)=0 SET @Message = N'#PROC: Không tìm thấy không tin công ty.';
	ELSE IF (SELECT COUNT(1) FROM Crm_CustomerInfoLog WHERE SenderId=@SenderId AND Code=@Code AND CompanyId=@CompanyId AND IsEnabled=1) = 0 
		BEGIN
			INSERT INTO Crm_CustomerInfoLog (IsEnabled,Code,Name,PhoneNumber,CompanyName,Address,AddressNote,ProvinceId,DistrictId,WardId,Lat,Lng,SenderId,CompanyId)
			VALUES(1,@Code,@Name,@PhoneNumber,@CompanyName,@Address,@AddressNote,@ProvinceId,@DistrictId,@WardId,@Lat,@Lng,@SenderId,@CompanyId)
			SET @IsSuccess = 1;
			SET @Message = N'#PROC: OK.';
		END
	ELSE
		BEGIN
			UPDATE Crm_CustomerInfoLog SET Name=@Name,PhoneNumber= @PhoneNumber,CompanyName=@CompanyName,Address=@Address,
			AddressNote=@AddressNote,ProvinceId=@ProvinceId,DistrictId=@DistrictId,WardId=@WardId,Lat=@Lat,Lng=@Lng,CompanyId=@CompanyId
			WHERE SenderId=@SenderId AND Code=@Code
			SET @IsSuccess = 1;
			SET @Message = N'#PROC: OK.';
		END
	SELECT CONVERT(BIT,@IsSuccess) AS IsSuccess, @Message as [Message]
END
