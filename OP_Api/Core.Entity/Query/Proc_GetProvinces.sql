SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROCEDURE [dbo].[Proc_GetProvinces]  --ALTER
	@CountryId INT = null,
	@PageNumber INT = null,
	@PageSize INT = null,
	@SearchText NVARCHAR(200) = null,
	@CompanyId INT = null
AS
BEGIN
	IF @PageSize IS NULL BEGIN SET @PageSize = 20 END;
	IF @PageNumber IS NULL BEGIN SET @PageNumber = 1 END;
	WITH _CTE AS(
		SELECT cProvince.Id, cProvince.Name, cProvince.Code, 
			cProvince.Lat, cProvince.Lng,
			cProvince.CreatedBy, cProvince.CreatedWhen, cProvince.ModifiedBy, cProvince.ModifiedWhen, cProvince.IsEnabled,
			cProvince.CountryId, cCountry.Name AS CountryName,
			cProvince.CompanyId, cCompany.Name AS CompanyName,
			(ROW_NUMBER() OVER (ORDER BY cProvince.Id DESC)) AS RowNum
		FROM Core_Province cProvince
		INNER JOIN Core_Country AS cCountry ON cCountry.Id = cProvince.CountryId AND cCountry.IsEnabled = 1
		LEFT JOIN Core_Company AS cCompany ON cCompany.Id = cProvince.CompanyId AND cCompany.IsEnabled = 1
		WHERE cProvince.IsEnabled = 1 AND
		(@CompanyId IS NULL OR cProvince.CompanyId = @CompanyId) AND
		(@SearchText IS NULL OR (
			cProvince.Name LIKE N'%' + @SearchText + '%' OR 
			cProvince.Code LIKE N'%' + @SearchText + '%'
		)) AND
		(@CountryId IS NULL OR cProvince.CountryId = @CountryId)
	)
	SELECT *, (SELECT COUNT(1)  FROM _CTE) AS TotalCount 
	FROM _CTE 
	WHERE RowNum > (@PageNumber * @PageSize - @PageSize) AND RowNum <= @PageNumber * @PageSize
END
GO

