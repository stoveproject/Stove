IF (EXISTS ( SELECT *
             FROM   INFORMATION_SCHEMA.TABLES
             WHERE  TABLE_SCHEMA = 'dbo'
                    AND TABLE_NAME = 'Animals' ))
   BEGIN
         DROP TABLE dbo.Animals
   END
 
IF (EXISTS ( SELECT *
             FROM   INFORMATION_SCHEMA.TABLES
             WHERE  TABLE_SCHEMA = 'dbo'
                    AND TABLE_NAME = 'Persons' ))
   BEGIN
         DROP TABLE dbo.Persons
 
   END
    
IF (EXISTS ( SELECT *
             FROM   INFORMATION_SCHEMA.TABLES
             WHERE  TABLE_SCHEMA = 'dbo'
                    AND TABLE_NAME = 'Mails' ))
   BEGIN
	
         DROP TABLE dbo.Mails
   END


IF (EXISTS ( SELECT *
             FROM   INFORMATION_SCHEMA.TABLES
             WHERE  TABLE_SCHEMA = 'dbo'
                    AND TABLE_NAME = 'Products' ))
   BEGIN
	
         DROP TABLE dbo.Products
 
   END
    


