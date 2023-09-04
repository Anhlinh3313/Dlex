
DROP PROC Proc_CheckInfoLogin -- Proc_CheckInfoLogin 'administrator','DSC'
GO
CREATE PROC Proc_CheckInfoLogin
(
@UserName NVARCHAR(100) NULL,
@CompanyCode NVARCHAR(10) NULL
)AS
BEGIN
	SELECT 
	_user.Id, _user.PasswordHash, _user.SecurityStamp,_user.UserName,_user.FullName, _user.IsBlocked, _user.IsPassWordBasic, _user.CompanyId, com.Code AS CompanyCode
	FROM Core_User as _user 
	INNER JOIN Core_Company as com ON com.Id = _user.CompanyId AND com.IsEnabled=1
	WHERE _user.IsEnabled=1 AND com.Code = @CompanyCode AND @CompanyCode IS NOT NULL
	 AND (_user.UserName = @UserName OR _user.Email = @UserName OR _user.PhoneNumber = @UserName)
END
GO
IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Core_TypeUser'))
BEGIN
   CREATE TABLE Core_TypeUser(
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
IF(SELECT COUNT(1) FROM Core_TypeUser WHERE Id=1)=0
BEGIN
	INSERT INTO Core_TypeUser(Code,Name) VALUES('TU001',N'Admin')
	INSERT INTO Core_TypeUser(Code,Name) VALUES('TU002',N'Company')
	INSERT INTO Core_TypeUser(Code,Name) VALUES('TU003',N'Customer')
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'TypeUserId'
          AND Object_ID = Object_ID(N'dbo.Core_User'))
BEGIN
	ALTER TABLE Core_User ADD TypeUserId INT not null CONSTRAINT fk_user_typeUser FOREIGN KEY(TypeUserId) REFERENCES Core_TypeUser(Id)
    --Column Exists
	--ALTER TABLE Core_User alter column TypeUserId int not null
END
