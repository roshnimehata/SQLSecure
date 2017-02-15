/* ---------------------------------------------------------------------- */
/* Schema changes for SQLsecure 3.1 schema version 3100                   */
/* ---------------------------------------------------------------------- */

/* START SQL Secure 3.1 (Barkha Khatri) Register azure servers */ 
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID('registeredserver', 'U') IS NOT NULL 
BEGIN
IF COL_LENGTH('registeredserver','servertype') IS NULL
 BEGIN
	ALTER TABLE registeredserver
	ADD servertype NVARCHAR(3) NOT NULL DEFAULT('OP')
 END
END

GO


/* END SQL Secure 3.1 (Barkha Khatri) Register azure servers */ 
/* START SQL Secure 3.1 (Barkha Khatri) Risk assessments */ 
IF OBJECT_ID('metric', 'U') IS NOT NULL 
BEGIN
IF COL_LENGTH('metric','applicableonazuredb') IS NULL
 BEGIN
	ALTER TABLE metric
	ADD applicableonazuredb int  
	CONSTRAINT col_applicableonazuredb_def  
	DEFAULT 0  NOT NULL; 
 END
 IF COL_LENGTH('metric','applicableonazurevm') IS NULL
 BEGIN
	ALTER TABLE metric
	ADD applicableonazurevm int  
	CONSTRAINT col_applicableonazurevm_def  
	DEFAULT 0  NOT NULL; 
 END
 IF COL_LENGTH('metric','applicableonpremise') IS NULL
 BEGIN
	ALTER TABLE metric
	ADD applicableonpremise int  
	CONSTRAINT col_applicableonpremise_def  
	DEFAULT 1  NOT NULL; 

 END
END
/* END SQL Secure 3.1 (Barkha Khatri) Risk assessments */
GO
