SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROC [dbo].[Proc_GetCenterHubs]
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
				SELECT hub.Id, hub.Name, hub.Code,
				(CASE
				WHEN d.IsEnabled = 1 THEN hub.DistrictId
				WHEN d.IsEnabled = 0 THEN NULL
				END) AS DistrictId,
				(CASE
				WHEN d.IsEnabled = 1 THEN d.Name
				WHEN d.IsEnabled = 0 THEN NULL
				END) AS DistrictName,
				(CASE
				WHEN w.IsEnabled = 1 THEN hub.WardId
				WHEN w.IsEnabled = 0 THEN NULL
				END) AS WardId,
				(CASE
				WHEN w.IsEnabled = 1 THEN w.Name
				WHEN w.IsEnabled = 0 THEN NULL
				END) AS WardName, hub.POHubId, hub.PhoneNumber,
				hub.Address, hub.AddressDisplay, hub.Fax, hub.Email, hub.CompanyId,
				(CASE
				WHEN p.IsEnabled = 1 THEN hub.ProvinceId
				WHEN p.IsEnabled = 0 THEN NULL
				END) AS ProvinceId,
				(CASE
				WHEN p.IsEnabled = 1 THEN p.Name
				WHEN p.IsEnabled = 0 THEN NULL
				END) AS ProvinceName, hub.HasAirPort, hub.ConcurrencyStamp,
				(ROW_NUMBER() OVER (ORDER BY hub.Id)) AS RowNum
				FROM dbo.Core_Hub AS hub
				INNER JOIN dbo.Core_Ward AS w ON w.Id = hub.WardId 
				INNER JOIN dbo.Core_District AS d ON d.id = hub.DistrictId 
				INNER JOIN dbo.Core_Province AS p ON p.id = hub.ProvinceId 
				WHERE hub.IsEnabled = 1 AND  hub.CenterHubId IS NULL
				AND (@SearchText IS NULL OR hub.Code LIKE N'%'+ @SearchText +'%'  
										 OR hub.Name LIKE N'%'+ @SearchText +'%'  
										 OR hub.PhoneNumber LIKE N'%'+ @SearchText +'%' 
										 OR hub.Email LIKE N'%'+ @SearchText +'%' )
			)
			SELECT *,(SELECT COUNT(1) FROM _CTE) AS TotalCount 
			FROM _CTE WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
END
GO

