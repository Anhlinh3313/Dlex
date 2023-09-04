SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC Proc_GetUsers
(
	@CompanyId INT = NULL,
	@PageNumber INT = null,
	@PageSize INT = NULL,
	@SearchText NVARCHAR(50) = NULL

)AS
BEGIN

	IF @PageNumber IS NULL SET @PageNumber = 1;
	IF @PageSize IS NULL SET @PageSize = 20;
	IF @SearchText = '' SELECT @SearchText = NULL;

WITH _CTE AS(
				SELECT u.Id, u.TypeUserId, u.CompanyId, u.RoleId, u.HubId, u.UserName, u.Code, u.Email, u.PhoneNumber, u.Address, u.FullName,
				r.Name AS RoleName, dep.Name AS DepartmentName, h.Name AS ManageHubName, hub.Name AS HubName, u.DepartmentId, u.ManageHubId, u.IsBlocked, u.IdentityCard,
				(ROW_NUMBER() OVER (ORDER BY u.Id)) AS RowNum
				FROM dbo.Core_User AS u
				INNER JOIN dbo.Core_Company AS com ON com.Id = u.CompanyId AND com.IsEnabled = 1
				LEFT JOIN dbo.Core_Role AS r ON r.id = u.RoleId AND r.IsEnabled = 1
				LEFT JOIN dbo.Core_Department AS dep ON dep.Id = u.DepartmentId AND dep.IsEnabled = 1
				LEFT JOIN dbo.Core_Hub AS h ON h.id = u.ManageHubId AND h.IsEnabled = 1
				LEFT JOIN dbo.Core_Hub AS hub ON hub.id = u.HubId AND hub.IsEnabled = 1
				WHERE u.IsEnabled = 1
				AND u.CompanyId = @CompanyId
				AND (@SearchText IS NULL OR u.Code LIKE @SearchText 
										 OR u.UserName LIKE @SearchText 
										 OR u.FullName LIKE @SearchText 
										 OR u.PhoneNumber LIKE @SearchText 
										 OR u.Email LIKE @SearchText)
			)
			SELECT *,(SELECT COUNT(1) FROM _CTE) AS TotalCount 
			FROM _CTE WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
END
GO