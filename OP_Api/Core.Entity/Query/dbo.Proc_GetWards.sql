SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROCEDURE [dbo].[Proc_GetWards]  --ALTER
	@ProvinceId INT = null,
	@District INT = null,
	@PageNumber INT = null,
	@PageSize INT = null,
	@SearchText NVARCHAR(200) = null,
	@IsRemote BIT = null,
	@CompanyId INT = null
AS
BEGIN
	IF @PageSize IS NULL BEGIN SET @PageSize = 20 END;
	IF @PageNumber IS NULL BEGIN SET @PageNumber = 1 END;
	WITH _CTE AS(
		SELECT cWard.Id, cWard.Name, cWard.Code, 
			cWard.Lat, cWard.Lng, cWard.IsRemote, cWard.KmNumber,
			cWard.CreatedBy, cWard.CreatedWhen, cWard.ModifiedBy, cWard.ModifiedWhen, cWard.IsEnabled,
			cDistrict.ProvinceId, cProvince.Name AS ProvinceName,
			cWard.DistrictId, cDistrict.Name AS DistrictName,
			cWard.CompanyId, cCompany.Name AS CompanyName,
			(ROW_NUMBER() OVER (ORDER BY cDistrict.Id)) AS RowNum
		FROM Core_Ward cWard
		INNER JOIN Core_District AS cDistrict ON cDistrict.Id = cWard.DistrictId AND cDistrict.IsEnabled = 1
		INNER JOIN Core_Province AS cProvince ON cProvince.Id = cDistrict.ProvinceId AND cProvince.IsEnabled = 1
		LEFT JOIN Core_Company AS cCompany ON cCompany.Id = cWard.CompanyId
		WHERE cWard.IsEnabled = 1 AND
		(@CompanyId IS NULL OR cWard.CompanyId = @CompanyId) AND
		(@SearchText IS NULL OR (
			cWard.Name LIKE N'%' + @SearchText + '%' OR 
			cWard.Code LIKE N'%' + @SearchText + '%'
		)) AND
		(@District IS NULL OR cWard.DistrictId = @District) AND 
		(@ProvinceId IS NULL OR cDistrict.ProvinceId = @ProvinceId) AND 
		(@IsRemote IS NULL OR
			(@IsRemote = 0 AND cWard.IsRemote = 0) OR
			(@IsRemote = 1 AND cWard.IsRemote = 1)
		)
	)
	SELECT *, (SELECT COUNT(1)  FROM _CTE) AS TotalCount 
	FROM _CTE 
	WHERE RowNum > (@PageNumber * @PageSize - @PageSize) AND RowNum <= @PageNumber * @PageSize
END
GO

