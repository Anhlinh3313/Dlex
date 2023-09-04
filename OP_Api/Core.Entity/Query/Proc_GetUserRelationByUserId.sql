SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROC Proc_GetUserRelationByUserId
(
	@UserId INT,	@PageNumber INT = null,
	@PageSize INT = NULL

)AS
	BEGIN

		IF @PageNumber IS NULL SET @PageNumber = 1;
		IF @PageSize IS NULL SET @PageSize = 20;

	WITH _CTE AS(
				SELECT ur.Id, ur.Code, ur.Name, ur.UserId, ur.UserRelationId,				(ROW_NUMBER() OVER (ORDER BY ur.Id)) AS RowNum
				FROM dbo.Core_UserRelation AS ur				INNER JOIN dbo.Core_User AS u ON u.id = ur.UserRelationId AND u.IsEnabled = 1				WHERE ur.IsEnabled = 1 				AND ur.UserId = @UserId
				)
	SELECT *,(SELECT COUNT(1) FROM _CTE) AS TotalCount 
	FROM _CTE WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
END
GO

