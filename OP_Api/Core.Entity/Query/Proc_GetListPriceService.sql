SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[Proc_GetListPriceService]
(
	@PageNumber INT = NULL,
	@PageSize INT = NULL,
	@SearchText NVARCHAR(50) = NULL,
	@FromDate DATETIME = NULL,
	@ToDate DATETIME = NULL,
	@ServicceId INT = NULL,
	@ProvinceFromId INT = NULL,
	@ProvinceToId INT = NULL,
	@CompanyId INT = NULL
)
AS
BEGIN
	IF @PageNumber IS NULL SET @PageNumber = 1;
	IF @PageSize IS NULL SET @PageSize = 20;
	SET NOCOUNT ON;
		WITH _CTE AS (
				 SELECT ps.id, ps.Code, ps.Name, ps.ConcurrencyStamp, s.Name AS ServiceName, s.Code AS ServiceCode, s.id AS ServiceId, ps.NumOrder, ps.PublicDateFrom, ps.PublicDateTo,
				 ps.DIM, ps.VATPercent, ps.FuelPercent, ps.RemoteAreasPricePercent, ps.IsTwoWay, ps.IsPublic,
				 (ROW_NUMBER() OVER (ORDER BY ps.Id DESC)) AS RowNum
				 FROM dbo.Post_PriceService AS ps
				 INNER JOIN dbo.Post_Service AS s ON s.Id = ps.ServiceId AND s.IsEnabled =1
				 WHERE ps.IsEnabled = 1 AND ps.CompanyId = @CompanyId
				 AND(@FromDate IS NULL OR (DATEDIFF(DAY,@FromDate,ps.PublicDateFrom) >= 0) OR ps.PublicDateFrom IS NULL)
				 AND(@ToDate IS NULL OR (DATEDIFF(DAY,ps.PublicDateTo,@ToDate) >=0) OR ps.PublicDateTo IS NULL)
				 AND(@ServicceId IS NULL OR ps.ServiceId = @ServicceId)
				 AND(@ProvinceFromId IS NULL OR ps.ProvinceFromId = @ProvinceFromId)
				 AND(@ProvinceToId IS NULL OR ps.ProvinceToId = @ProvinceToId)
				 AND ((@SearchText IS NULL OR ps.Code LIKE N'%' + @SearchText + '%')
				 OR (@SearchText IS NULL OR s.Name LIKE N'%' + @SearchText + '%')
				 OR (@SearchText IS NULL OR ps.Name LIKE N'%' + @SearchText + '%')))
		SELECT *,
		(SELECT COUNT (*) FROM _CTE) AS TotalCount FROM _CTE
		WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
END
GO

