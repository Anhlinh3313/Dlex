
IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Core_CompanySize'))
BEGIN
   CREATE TABLE Core_CompanySize(
   Id INT PRIMARY KEY IDENTITY,
   IsEnabled BIT DEFAULT 1,
   CreatedBy INT NULL,
   CreatedWhen DATETIME  NULL DEFAULT GETDATE(),
   ModifiedBy INT NULL,
   ModifiedWhen DATETIME NULL DEFAULT GETDATE(),
   ConcurrencyStamp NVARCHAR(200),
   [Name] NVARCHAR(200),
   Code VARCHAR(200)   
   )
END
GO
IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Core_Company'))
BEGIN
   CREATE TABLE Core_Company(
   Id INT PRIMARY KEY IDENTITY,
   IsEnabled BIT DEFAULT 1,
   CreatedBy INT NULL,
   CreatedWhen DATETIME  NULL DEFAULT GETDATE(),
   ModifiedBy INT NULL,
   ModifiedWhen DATETIME NULL DEFAULT GETDATE(),
   ConcurrencyStamp NVARCHAR(200),
   [Name] NVARCHAR(200),
   Code VARCHAR(200),
   PhoneNumber NVARCHAR(20),
   Hotline NVARCHAR(20),
   Email NVARCHAR(100),
   [Address] NVARCHAR(200),
   Website NVARCHAR(500),
   CompanySizeId INT CONSTRAINT fk_CompanySize_Company FOREIGN KEY(CompanySizeId) REFERENCES Core_CompanySize(Id),
   PrefixShipmentNumber VARCHAR(10),
   PrefixRequestNumber VARCHAR(10),
   TopUp FLOAT DEFAULT -500000,
   LogoUrl NVARCHAR(500),
   ContactName NVARCHAR(100),
   ContactPhone NVARCHAR(20)
   )
END
GO
DECLARE @TableName TABLE(IsLoop BIT DEFAULT 0, TableName NVARCHAR(100));
INSERT INTO @TableName (TableName)
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME != 'SAP-INC' AND TABLE_NAME != 'Core_CompanySize' AND TABLE_NAME != 'Core_Company'
SELECT * FROM @TableName
WHILE((SELECT COUNT(1) FROM @TableName WHERE IsLoop=0)>0)
BEGIN
	DECLARE @Name NVARCHAR(100);
	SELECT TOP 1 @Name=TableName FROM @TableName WHERE IsLoop=0;
	IF NOT EXISTS(SELECT 1 FROM sys.columns 
			  WHERE Name = N'CompanyId'
			  AND Object_ID = Object_ID(@Name))
	BEGIN
		DECLARE @query NVARCHAR(500) = 'ALTER TABLE '+@Name+' ADD CompanyId INT CONSTRAINT fk_Company_'+@Name+' FOREIGN KEY(CompanyId) REFERENCES Core_Company(Id)'
		EXEC(@query)
	END
	ELSE
	BEGIN
		DECLARE @query2 NVARCHAR(500) = 'UPDATE '+@Name+' SET CompanyId=1'
		EXEC(@query2)
	END
	UPDATE @TableName SET IsLoop=1 WHERE @Name=TableName;
END
