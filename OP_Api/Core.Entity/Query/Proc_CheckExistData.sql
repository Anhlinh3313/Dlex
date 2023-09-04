
DROP PROC Proc_CheckExistData
GO -- Proc_CheckExistData 1,'Core_User',NULL,NULL,'UserName=''nvdschcm1'''
CREATE PROC Proc_CheckExistData -- Proc_CheckExistData 1,'Core_User','PhoneNumber','administrator'
(
@CompanyId INT = NULL,
@TableName NVARCHAR(200),
@ColumnName NVARCHAR(200) = NULL,
@Value NVARCHAR(500) = NULL,
@Where NVARCHAR(500) = NULL
)AS
BEGIN
	DECLARE @Query NVARCHAR(1000) = (N'SELECT NEWID() AS FakeId, COUNT(1) AS DataCount FROM '+@TableName+' WHERE IsEnabled=1 AND CompanyId='+ CONVERT(VARCHAR(20),@CompanyId))
	IF(@ColumnName IS NOT NULL AND @Value IS NOT NULL)
	BEGIN
		SET @Query = @Query + ' AND '+ @ColumnName +'=N'''+@Value+''''
	END
	IF(@Where IS NOT NULL)
	BEGIN
		SET  @Query = @Query + ' AND '+ @Where
	END
	EXEC(@Query)
	--SELECT @Query
END