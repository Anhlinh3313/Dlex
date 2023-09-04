SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[Proc_GetListCountrys]
	@PageNumber INT = null,
	@PageSize INT = null,
	@SearchText NVARCHAR(200) = null,
	@CompanyId INT = null
AS
BEGIN
	IF @PageSize IS NULL BEGIN SET @PageSize = 20 END;
	IF @PageNumber IS NULL BEGIN SET @PageNumber = 1 END;
	WITH _CTE AS(
		SELECT Country.Id, Country.CreatedWhen, Country.CreatedBy, Country.ModifiedWhen, Country.ModifiedBy,
		Country.IsEnabled, Country.	Code, Country.Name, Country.CompanyId, Company.Name AS CompanyName,
		(ROW_NUMBER() OVER (ORDER BY Country.Id DESC)) AS RowNum
		FROM dbo.Core_Country AS Country
		LEFT JOIN Core_Company AS Company ON Company.Id = Country.CompanyId AND Company.IsEnabled = 1
		WHERE Country.IsEnabled = 1 AND
		(@CompanyId IS NULL OR Country.CompanyId = @CompanyId) AND
		(@SearchText IS NULL OR (
			Country.Name LIKE N'%' + @SearchText + '%' OR 
			Country.Code LIKE N'%' + @SearchText + '%'
		))
	)
	SELECT *, (SELECT COUNT(1)  FROM _CTE) AS TotalCount 
	FROM _CTE 
	WHERE RowNum > (@PageNumber * @PageSize - @PageSize) AND RowNum <= @PageNumber * @PageSize
END
GO

