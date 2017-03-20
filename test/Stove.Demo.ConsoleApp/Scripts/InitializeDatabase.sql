IF (NOT EXISTS ( SELECT *
                 FROM   INFORMATION_SCHEMA.TABLES
                 WHERE  TABLE_SCHEMA = 'dbo'
                        AND TABLE_NAME = 'Animals' )
   )
   BEGIN
         CREATE TABLE dbo.Animals
                (
                 Id INT NOT NULL
                        IDENTITY(1, 1)
               , Name NVARCHAR(50) NOT NULL
                )

   END
 
IF (NOT EXISTS ( SELECT *
                 FROM   INFORMATION_SCHEMA.TABLES
                 WHERE  TABLE_SCHEMA = 'dbo'
                        AND TABLE_NAME = 'Persons' )
   )
   BEGIN
	
         CREATE TABLE dbo.Persons
                (
                 Id INT NOT NULL
                        IDENTITY(1, 1)
               , Name NVARCHAR(50) NOT NULL
               , IsDeleted BIT NOT NULL
                )
   END
    
IF (NOT EXISTS ( SELECT *
                 FROM   INFORMATION_SCHEMA.TABLES
                 WHERE  TABLE_SCHEMA = 'dbo'
                        AND TABLE_NAME = 'Mails' )
   )
   BEGIN
	
         CREATE TABLE dbo.Mails
                (
                 Id UNIQUEIDENTIFIER NOT NULL
                                     DEFAULT NEWSEQUENTIALID()
               , Subject NVARCHAR(50) NOT NULL
               , IsDeleted BIT NOT NULL
               , DeleterUserId BIGINT NULL
               , DeletionTime DATETIME NULL
               , LastModificationTime DATETIME NULL
               , LastModifierUserId BIGINT NULL
               , CreationTime DATETIME NOT NULL
               , CreatorUserId BIGINT NULL
                )
   END


IF (NOT EXISTS ( SELECT *
                 FROM   INFORMATION_SCHEMA.TABLES
                 WHERE  TABLE_SCHEMA = 'dbo'
                        AND TABLE_NAME = 'Products' )
   )
   BEGIN
	

         CREATE TABLE dbo.Products
                (
                 Id INT NOT NULL
                        IDENTITY(1, 1)
               , Name NVARCHAR(50) NOT NULL
               , IsDeleted BIT NOT NULL
               , DeleterUserId BIGINT NULL
               , DeletionTime DATETIME NULL
               , LastModificationTime DATETIME NULL
               , LastModifierUserId BIGINT NULL
               , CreationTime DATETIME NOT NULL
               , CreatorUserId BIGINT NULL
                )
   END
    


