SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
ALTER PROC [dbo].[Proc_GetHolidayByYear]
(
	@Year INT,
	@PageNumber INT = NULL,
	@PageSize INT = NULL,
	@CompanyId INT = NULL
)AS
BEGIN
	IF @PageNumber IS NULL SET @PageNumber = 1;
	IF @PageSize IS NULL SET @PageSize = 20;

	IF (@Year != 0)
	BEGIN
		SET NOCOUNT ON;
		WITH _CTE AS (
				 SELECT  Id, Code, Name, Date, CreatedWhen, NotHoliday,
				 (ROW_NUMBER() OVER (ORDER BY Id DESC)) AS RowNum
				 FROM dbo.Post_Holiday 
				 WHERE IsEnabled = 1 AND YEAR(Date) = @Year AND CompanyId = @CompanyId
				 )
		SELECT *,
		(SELECT COUNT (*) FROM _CTE) AS TotalCount FROM _CTE
		WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
	END

	ELSE 
	BEGIN
		SET NOCOUNT ON;
		WITH _CTE AS (
				 SELECT  Id, Code, Name, Date, CreatedWhen, NotHoliday,
				 (ROW_NUMBER() OVER (ORDER BY Id DESC)) AS RowNum
				 FROM dbo.Post_Holiday 
				 WHERE IsEnabled = 1AND CompanyId = @CompanyId
				 )
		SELECT *,
		(SELECT COUNT (*) FROM _CTE) AS TotalCount FROM _CTE
		WHERE RowNum > (@PageNumber*@PageSize-@PageSize) AND RowNum <= @PageNumber*@PageSize
	END
END
GO

