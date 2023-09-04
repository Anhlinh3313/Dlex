SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC Proc_CreatePriceListSetting
(
	@CustomerId INT = NULL,
	@ServiceId INT = NULL,	
	@UserId INT = NULL,
	@CompanyId INT = NULL
)
as
BEGIN

	DECLARE @IsSuccess BIT = 1;
	DECLARE @Message NVARCHAR(200);
	DECLARE @tablePromotionCustomer TABLE (Id INT);
	DECLARE @CountPromotionCustomer INT = 0;

	SELECT @CountPromotionCustomer = COUNT(1) FROM dbo.Post_PromotionCustomer WHERE PromotionId=@PromotionId AND CustomerId = @CustomerId AND IsEnabled=1 AND @PromotionId Is NOT NULL;
	IF(@CountPromotionCustomer > 0)
	BEGIN
		SET @IsSuccess = 0;
		SET @Message = N'Áp dụng giảm giá khách hàng đã tồn tại';
	END
	ELSE
	BEGIN
		--INSERT
		INSERT dbo.Post_PromotionCustomer
		(
			CreatedWhen,
			CreatedBy,
			ModifiedWhen,
			ModifiedBy,
			IsEnabled,
			CustomerId,
			PromotionId,
			ConcurrencyStamp,
			CompanyId
		)
		VALUES
		(   GETDATE(), -- CreatedWhen - datetime
			@UserId,         -- CreatedBy - int
			GETDATE(), -- ModifiedWhen - datetime
			@UserId,         -- ModifiedBy - int
			1,      -- IsEnabled - bit
			@CustomerId,         -- CustomerId - int
			@PromotionId,         -- PromotionId - int
			N'',       -- ConcurrencyStamp - nvarchar(max)
			@CompanyId          -- CompanyId - int
			)
		--
	END
	--
	SELECT @IsSuccess as IsSuccess, @Message AS [Message]

END
GO