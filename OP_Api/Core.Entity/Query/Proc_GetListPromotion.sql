SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROC [dbo].[Proc_GetListPromotion]
(
	@PageNumber INT = NULL,
	@PageSize INT = NULL,
	@SearchText NVARCHAR(50) = NULL,
	@FromDate DATETIME = NULL,
	@ToDate DATETIME = NULL,
	@PromotionTypeId INT = NULL,
	@IsPublic BIT = NULL,
	@IsHidden BIT = NULL,
	@CompanyId INT = NULL
)
AS
BEGIN
	IF @PageNumber IS NULL SET @PageNumber = 1;
	IF @PageSize IS NULL SET @PageSize = 20;
	SET NOCOUNT ON;
		WITH _CTE AS (
				 SELECT p.Id, p.Name, p.Code, pt.Id AS PromotionTypeId, pt.Name AS PromotionTypeName, p.PromotionNot, p.TotalPromotion, p.TotalCode,
				 p.FromDate, p.ToDate, p.IsPublic, p.IsHidden, p.ConcurrencyStamp,
				 (ROW_NUMBER() OVER (ORDER BY p.Id DESC)) AS RowNum
				 FROM dbo.Post_Promotion AS p
				 LEFT JOIN dbo.Post_PromotionType AS pt ON pt.id = p.PromotionTypeId AND p.IsEnabled = 1
				 WHERE p.IsEnabled = 1 AND p.CompanyId = @CompanyId
				 AND(@FromDate IS NULL OR (DATEDIFF(DAY,@FromDate,p.FromDate) >= 0) OR p.FromDate IS NULL)
				 AND(@ToDate IS NULL OR (DATEDIFF(DAY,p.ToDate,@ToDate) >=0) OR p.ToDate IS NULL)
				 AND(@PromotionTypeId IS NULL OR p.PromotionTypeId = @PromotionTypeId)
				 AND(@IsPublic IS NULL OR p.IsPublic = @IsPublic)
				 AND(@IsHidden IS NULL OR p.IsHidden = @IsHidden)
				 AND ((@SearchText IS NULL OR p.Code LIKE N'%' + @SearchText + '%')
				 OR (@SearchText IS NULL OR p.Name LIKE N'%' + @SearchText + '%')))
		SELECT *,
		(SELECT COUNT (*) FROM _CTE) AS TotalCount FROM _CTE
		WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
END
GO

