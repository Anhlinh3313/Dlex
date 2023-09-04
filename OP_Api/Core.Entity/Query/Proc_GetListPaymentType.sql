SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[Proc_GetListPaymentType] -- EXEC Proc_GetListRole 
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
				 SELECT pt.Id, pt.Code, pt.Name, pt.ConcurrencyStamp, pt.PaymentNow,
				 (ROW_NUMBER() OVER (ORDER BY Id DESC)) AS RowNum
				 FROM dbo.Post_PaymentType AS pt
				 WHERE pt.IsEnabled = 1 AND pt.CompanyId = @CompanyId
				 AND ((@SearchText IS NULL OR pt.Code LIKE N'%' + @SearchText + '%')
				 OR (@SearchText IS NULL OR pt.Name LIKE N'%' + @SearchText + '%')))
		SELECT *,
		(SELECT COUNT (*) FROM _CTE) AS TotalCount FROM _CTE
		WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
END
GO
