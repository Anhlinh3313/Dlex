SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROC [dbo].[Proc_GetListPromotionCustomer]
(
	@PageNumber INT = NULL,
	@PageSize INT = NULL,
	@SearchText NVARCHAR(50) = NULL,
	@PromotionId INT NULL,
	@CustomerId INT NULL,
	@CompanyId INT = NULL
)
AS
BEGIN
	IF @PageNumber IS NULL SET @PageNumber = 1;
	IF @PageSize IS NULL SET @PageSize = 20;
	SET NOCOUNT ON;
		WITH _CTE AS (
				 SELECT pc.Id, p.Id AS PromotionId, p.Code AS PromotionCode, c.Id AS CustomerId, c.Name AS CustomerName, c.PhoneNumber, pc.ConcurrencyStamp,
				 (ROW_NUMBER() OVER (ORDER BY pc.Id DESC)) AS RowNum
				 FROM dbo.Post_PromotionCustomer AS pc
				 INNER JOIN dbo.Post_Promotion AS p ON p.Id = pc.PromotionId AND p.IsEnabled = 1
				 INNER JOIN dbo.Crm_Customer AS c ON c.Id = pc.CustomerId AND c.IsEnabled = 1
				 WHERE pc.IsEnabled = 1 AND pc.CompanyId = @CompanyId
				 AND(@PromotionId IS NULL OR pc.PromotionId = @PromotionId)
				 AND(@CustomerId IS NULL OR c.Id= @CustomerId)
				 AND ((@SearchText IS NULL OR p.Code LIKE N'%' + @SearchText + '%')
				 OR (@SearchText IS NULL OR c.PhoneNumber LIKE N'%' + @SearchText + '%')
				 OR (@SearchText IS NULL OR c.Name LIKE N'%' + @SearchText + '%')))
		SELECT *,
		(SELECT COUNT (*) FROM _CTE) AS TotalCount FROM _CTE
		WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
END
GO