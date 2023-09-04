SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[Proc_GetListPrice]
(
	@PageNumber INT = NULL,
	@PageSize INT = NULL,
	@SearchText NVARCHAR(50) = NULL,
	@HubId INT = NULL,
	@CompanyId INT = NULL
)
AS
BEGIN
	IF @PageNumber IS NULL SET @PageNumber = 1;
	IF @PageSize IS NULL SET @PageSize = 20;
	SET NOCOUNT ON;
		WITH _CTE AS (
				 SELECT pl.id, pl.Code, pl.Name, PDVGT.Name AS PriceListDVGTName, h.Name AS HubName, pl.FuelSurcharge, pl.RemoteSurcharge, 
				 pl.PublicDateFrom, pl.PublicDateTo, pl.NumOrder, pl.IsPublic,
				 (ROW_NUMBER() OVER (ORDER BY pl.Id DESC)) AS RowNum
				 FROM dbo.Post_PriceList AS pl
				 INNER JOIN dbo.Core_Hub AS h ON h.id = pl.HubId AND h.IsEnabled = 1
				 INNER JOIN dbo.Post_PriceListDVGT AS PDVGT ON PDVGT.Id = pl.PriceListDVGTId AND PDVGT.IsEnabled = 1 
				 WHERE pl.IsEnabled = 1 AND pl.CompanyId = @CompanyId
				 AND(@HubId IS NULL OR pl.HubId = @HubId)
				 AND ((@SearchText IS NULL OR pl.Code LIKE N'%' + @SearchText + '%')
				 OR (@SearchText IS NULL OR h.Name LIKE N'%' + @SearchText + '%')
				 OR (@SearchText IS NULL OR PDVGT.Name LIKE N'%' + @SearchText + '%')
				 OR (@SearchText IS NULL OR pl.Name LIKE N'%' + @SearchText + '%')))
		SELECT *,
		(SELECT COUNT (*) FROM _CTE) AS TotalCount FROM _CTE
		WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
END
GO
