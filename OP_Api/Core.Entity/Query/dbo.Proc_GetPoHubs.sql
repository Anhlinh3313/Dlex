SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROC [dbo].[Proc_GetPoHubs]
(
	@PageNumber INT = null,
	@PageSize INT = NULL,
	@CenterHubId INT = NULL,
	@SearchText NVARCHAR(50) = NULL

)AS
BEGIN

	IF @PageNumber IS NULL SET @PageNumber = 1;
	IF @PageSize IS NULL SET @PageSize = 20;
	IF @SearchText = '' SELECT @SearchText = NULL;

WITH _CTE AS(
				SELECT h.Id, h.Name, h.Code,
				(CASE
				WHEN d.IsEnabled = 1 THEN h.DistrictId
				WHEN d.IsEnabled = 0 THEN NULL
				END) AS DistrictId,
				(CASE
				WHEN d.IsEnabled = 1 THEN d.Name
				WHEN d.IsEnabled = 0 THEN NULL
				END) AS DistrictName,
				(CASE
				WHEN w.IsEnabled = 1 THEN h.WardId
				WHEN w.IsEnabled = 0 THEN NULL
				END) AS WardId,
				(CASE
				WHEN w.IsEnabled = 1 THEN w.Name
				WHEN w.IsEnabled = 0 THEN NULL
				END) AS WardName,h.POHubId, h.PhoneNumber,
				h.Address, h.AddressDisplay, h.Fax, h.Email, h.CompanyId,
				(CASE
				WHEN p.IsEnabled = 1 THEN h.ProvinceId
				WHEN p.IsEnabled = 0 THEN NULL
				END) AS ProvinceId,
				(CASE
				WHEN p.IsEnabled = 1 THEN p.Name
				WHEN p.IsEnabled = 0 THEN NULL
				END) AS ProvinceName, h.HasAirPort, hub.Name AS PoHubName, h.CenterHubId, h.ConcurrencyStamp,
				(ROW_NUMBER() OVER (ORDER BY hub.Id)) AS RowNum
				FROM dbo.Core_Hub AS h
				INNER JOIN dbo.Core_Ward AS w ON w.Id = h.WardId 
				INNER JOIN dbo.Core_District AS d ON d.id = h.DistrictId 
				INNER JOIN dbo.Core_Province AS p ON p.id = h.ProvinceId
				INNER JOIN dbo.Core_Hub AS hub ON hub.id = h.CenterHubId  AND hub.IsEnabled = 1
				WHERE h.IsEnabled = 1 AND  h.CenterHubId IS NOT NULL AND h.POHubId IS NULL
				AND(@CenterHubId IS NULL OR h.CenterHubId = @CenterHubId) 
				AND (@SearchText IS NULL OR (h.Code LIKE N'%'+ @SearchText +'%'  
										 OR h.PhoneNumber LIKE N'%'+ @SearchText +'%'  
										 OR h.Email LIKE N'%'+ @SearchText +'%' ))
			)
			SELECT *,(SELECT COUNT(1) FROM _CTE) AS TotalCount 
			FROM _CTE WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
END
GO

