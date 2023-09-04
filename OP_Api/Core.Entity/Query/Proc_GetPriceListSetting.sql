SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROC [dbo].[Proc_GetListPriceListSetting]
(
	@PageNumber INT = NULL,
	@PageSize INT = NULL,
	@CompanyId INT = NULL,
	@CustomerId INT = NULL,
	@ServiceId INT = NULL,
	@PriceListId INT = NULL
)
AS
BEGIN
	IF @PageNumber IS NULL SET @PageNumber = 1;
	IF @PageSize IS NULL SET @PageSize = 20;
	SET NOCOUNT ON;
		WITH _CTE AS (
				 SELECT ps.Id, ps.Code, ps.Name, p.Code AS PriceListCode, c.Code AS CustomerCode, c.Id AS CustomerId, s.Id AS ServiceId, p.id AS PriceListId,
				 ps.ConcurrencyStamp, ps.VATSurcharge, ps.FuelSurcharge, ps.VSVXSurcharge, ps.DIM,
				 (ROW_NUMBER() OVER (ORDER BY ps.Id DESC)) AS RowNum
				 FROM dbo.Post_PriceListSetting AS ps
				 INNER JOIN dbo.Post_PriceList AS p ON p.id = ps.PriceListId AND p.IsEnabled = 1
				 INNER JOIN dbo.Crm_Customer AS c ON c.Id = ps.CustomerId AND c.IsEnabled = 1
				 INNER JOIN dbo.Post_Service AS s ON s.Id = ps.ServiceId AND c.IsEnabled = 1
				 WHERE ps.IsEnabled = 1 AND ps.CompanyId = @CompanyId
				 AND(@CustomerId IS NULL OR c.Id= @CustomerId)
				 AND(@ServiceId IS NULL OR s.Id= @ServiceId)
				 AND(@PriceListId IS NULL OR p.Id= @PriceListId)
				)
		SELECT *,
		(SELECT COUNT (*) FROM _CTE) AS TotalCount FROM _CTE
		WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
END
GO

