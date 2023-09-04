SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC Proc_UpdateCustomerbyUserFail
(
	@CustomerId INT = NULL
)
AS
BEGIN

	DECLARE @IsSuccess BIT = 1;
	DECLARE @Message NVARCHAR(200);
	DECLARE @CountCustomerId INT = 0;

	SELECT @CountCustomerId = COUNT(1) FROM dbo.Crm_Customer WHERE Id=@CustomerId AND IsEnabled=1 AND @CustomerId Is NOT NULL;
	IF(@CountCustomerId = 0)
	BEGIN
		SET @IsSuccess = 0;
		SET @Message = N'Thông tin khách hàng không tồn tại';
	END
		UPDATE dbo.Crm_Customer
		SET IsEnabled = 0
		WHERE Id = @CustomerId
	--
	SELECT @IsSuccess as IsSuccess, @Message AS [Message]

END
GO
