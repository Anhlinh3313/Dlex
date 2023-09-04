SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[Proc_GetListReason]
(
	@PageNumber INT = NULL,
	@PageSize INT = NULL,
	@SearchText NVARCHAR(50) = NULL,
	@CompanyId INT = NULL
)
AS
BEGIN
	IF @PageNumber IS NULL SET @PageNumber = 1;
	IF @PageSize IS NULL SET @PageSize = 20;
	SET NOCOUNT ON;
		WITH _CTE AS (
				 SELECT r.Id, r.Code, r.Name, r.PickFail, r.PickCancel, r.PickReject, r.DeliverFail, r.DeliverCancel,
				 r.ReturnFail, r.ReturnCancel, r.IsDelay, r.IsIncidents, r.IsPublic, r.IsAcceptReturn,
				 r.IsMustInput, r.ItemOrder, r.RoleId, r.IsUnlockListGood, r.ConcurrencyStamp,
				 (ROW_NUMBER() OVER (ORDER BY Id DESC)) AS RowNum
				 FROM dbo.Post_Reason AS r
				 WHERE r.IsEnabled = 1 AND r.CompanyId = @CompanyId
				 AND ((@SearchText IS NULL OR r.Code LIKE N'%' + @SearchText + '%')
				 OR (@SearchText IS NULL OR r.Name LIKE N'%' + @SearchText + '%')))
		SELECT *,
		(SELECT COUNT (*) FROM _CTE) AS TotalCount FROM _CTE
		WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
END
GO