SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROC [dbo].[Proc_GetUsersBySearch]
(
	@CompanyId INT = NULL,
	@PageNumber INT = null,
	@PageSize INT = NULL,
	@SearchText NVARCHAR(50) = NULL
)
AS
BEGIN

	IF @PageNumber IS NULL SET @PageNumber = 1;
	IF @PageSize IS NULL SET @PageSize = 20;
	IF @SearchText = '' SELECT @SearchText = NULL;

WITH _CTE AS(
				SELECT u.Id, u.TypeUserId, u.CompanyId, u.HubId, u.UserName, u.Code, u.Email, u.PhoneNumber, u.Address, u.FullName,
				u.DepartmentId, u.ManageHubId, u.IsBlocked, u.IdentityCard,
				(ROW_NUMBER() OVER (ORDER BY u.Id DESC)) AS RowNum
				FROM dbo.Core_User AS u
				WHERE u.IsEnabled = 1 AND u.FullName IS NOT NULL AND u.CompanyId = @CompanyId
				AND ((@SearchText IS NULL OR u.Code   LIKE N'%' + @SearchText + '%')
				OR (@SearchText IS NULL OR u.UserName LIKE N'%' + @SearchText + '%')
				OR (@SearchText IS NULL OR u.FullName LIKE N'%' + @SearchText + '%')
				OR (@SearchText IS NULL OR u.PhoneNumber LIKE N'%' + @SearchText + '%'))
			)
			SELECT *,(SELECT COUNT(1) FROM _CTE) AS TotalCount 
			FROM _CTE WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
END
GO

