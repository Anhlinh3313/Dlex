SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROC Proc_GetCenterHubs
(
	@PageNumber INT = null,
	@PageSize INT = NULL,
	@SearchText NVARCHAR(50) = NULL

)AS
BEGIN

	IF @PageNumber IS NULL SET @PageNumber = 1;
	IF @PageSize IS NULL SET @PageSize = 20;
	IF @SearchText = '' SELECT @SearchText = NULL;

WITH _CTE AS(
				SELECT hub.Id, hub.Name, hub.Code, hub.DistrictId, d.Name AS DistrictName, hub.WardId, w.Name AS WardName, hub.POHubId, hub.PhoneNumber,
				hub.Address, hub.AddressDisplay, hub.Fax, hub.Email, hub.CompanyId, hub.ProvinceId, p.Name AS ProvinceName, hub.HasAirPort,
				(ROW_NUMBER() OVER (ORDER BY hub.Id)) AS RowNum
				FROM dbo.Core_Hub AS hub
				LEFT JOIN dbo.Core_Ward AS w ON w.Id = hub.WardId AND w.IsEnabled = 1
				LEFT JOIN dbo.Core_District AS d ON d.id = hub.DistrictId AND d.IsEnabled = 1
				LEFT JOIN dbo.Core_Province AS p ON p.id = hub.ProvinceId AND p.IsEnabled = 1
				WHERE hub.IsEnabled = 1 AND  hub.CenterHubId IS NULL
				AND (@SearchText IS NULL OR hub.Code LIKE @SearchText 
										 OR hub.Name LIKE @SearchText 
										 OR hub.PhoneNumber LIKE @SearchText 
										 OR hub.Email LIKE @SearchText)
			)
			SELECT *,(SELECT COUNT(1) FROM _CTE) AS TotalCount 
			FROM _CTE WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
END
GO


