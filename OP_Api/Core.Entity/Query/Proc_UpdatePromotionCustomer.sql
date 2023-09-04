SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC Proc_UpdatePromotionCustomer
(
	@PromotionCustomerId INT = NULL
)
as
BEGIN

	DECLARE @IsSuccess BIT = 1;
	DECLARE @Message NVARCHAR(200);
	DECLARE @tablePromotionCustomer TABLE (Id INT);
	DECLARE @CountPromotionCustomer INT = 0;

	SELECT @CountPromotionCustomer = COUNT(1) FROM dbo.Post_PromotionCustomer WHERE Id=@PromotionCustomerId AND IsEnabled=1 AND @PromotionCustomerId Is NOT NULL;
	IF(@CountPromotionCustomer=0)
	BEGIN
		SET @IsSuccess = 0;
		SET @Message = N'Thông tin giảm giá khách hàng không tồn tại';
	END
	--update
	UPDATE dbo.Post_PromotionCustomer
	SET IsEnabled = 0 
	WHERE id = @PromotionCustomerId
	--
	SELECT @IsSuccess as IsSuccess, @Message AS [Message]

END
GO
