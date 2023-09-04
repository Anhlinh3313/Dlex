SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROCEDURE [dbo].[Proc_GetDistricts]  --ALTER
	@ProvinceId INT = null,
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
		SELECT cDistrict.Id, cDistrict.Name, cDistrict.Code, 
			cDistrict.Lat, cDistrict.Lng, cDistrict.IsRemote, cDistrict.KmNumber,
			cDistrict.CreatedBy, cDistrict.CreatedWhen, cDistrict.ModifiedBy, cDistrict.ModifiedWhen, cDistrict.IsEnabled,
			cDistrict.ProvinceId, cProvince.Name AS ProvinceName,
			cDistrict.CompanyId, cCompany.Name AS CompanyName,
			(ROW_NUMBER() OVER (ORDER BY cDistrict.Id DESC)) AS RowNum
		FROM Core_District cDistrict
		INNER JOIN Core_Province AS cProvince ON cProvince.Id = cDistrict.ProvinceId AND cProvince.IsEnabled = 1
		LEFT JOIN Core_Company AS cCompany ON cCompany.Id = cProvince.CompanyId AND cCompany.IsEnabled = 1
		WHERE cDistrict.IsEnabled = 1 AND
		(@CompanyId IS NULL OR cDistrict.CompanyId = @CompanyId) AND
		(@SearchText IS NULL OR (
			cDistrict.Name LIKE N'%' + @SearchText + '%' OR 
			cDistrict.Code LIKE N'%' + @SearchText + '%'
		)) AND
		(@ProvinceId IS NULL OR cDistrict.ProvinceId = @ProvinceId) AND 
		(@IsRemote IS NULL OR
			(@IsRemote = 0 AND cDistrict.IsRemote = 0) OR
			(@IsRemote = 1 AND cDistrict.IsRemote = 1)
		)
	)
	SELECT *, (SELECT COUNT(1)  FROM _CTE) AS TotalCount 
	FROM _CTE 
	WHERE RowNum > (@PageNumber * @PageSize - @PageSize) AND RowNum <= @PageNumber * @PageSize
END
GO

