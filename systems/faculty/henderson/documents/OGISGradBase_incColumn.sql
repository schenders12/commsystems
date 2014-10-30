/*
   Thursday, May 01, 20143:40:22 PM
   User: 
   Server: rockyridge
   Database: People
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_OGISGradBase
	(
	RecID int NOT NULL,
	Semester varchar(12) NOT NULL,
	UserId varchar(20) NOT NULL,
	LastNm varchar(25) NOT NULL,
	FirstNm varchar(15) NOT NULL,
	MiddleInt varchar(1) NOT NULL,
	Suffix varchar(4) NOT NULL,
	EmailId varchar(50) NOT NULL,
	DegProg varchar(7) NOT NULL,
	ProgDept varchar(6) NOT NULL,
	ProgStudy varchar(75) NOT NULL,
	AreaOfStudy varchar(75) NOT NULL,
	MajorProf varchar(12) NOT NULL,
	CoMajorProf varchar(12) NOT NULL,
	BaccDegCollNm varchar(30) NOT NULL,
	BaccDegCurr varchar(50) NOT NULL,
	BaccDegRec varchar(3) NOT NULL,
	GradDegCollNm varchar(30) NOT NULL,
	GradDegCurr varchar(20) NOT NULL,
	GradDegRec varchar(3) NOT NULL,
	ProcessDt datetime NOT NULL
	)  ON [PRIMARY]
GO
IF EXISTS(SELECT * FROM dbo.OGISGradBase)
	 EXEC('INSERT INTO dbo.Tmp_OGISGradBase (RecID, Semester, UserId, LastNm, FirstNm, MiddleInt, Suffix, EmailId, DegProg, ProgDept, ProgStudy, AreaOfStudy, MajorProf, CoMajorProf, BaccDegCollNm, BaccDegCurr, BaccDegRec, GradDegCollNm, GradDegCurr, GradDegRec, ProcessDt)
		SELECT RecID, Semester, UserId, LastNm, FirstNm, MiddleInt, Suffix, EmailId, DegProg, ProgDept, ProgStudy, AreaOfStudy, MajorProf, CoMajorProf, BaccDegCollNm, BaccDegCurr, BaccDegRec, GradDegCollNm, GradDegCurr, GradDegRec, ProcessDt FROM dbo.OGISGradBase WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.OGISGradBase
GO
EXECUTE sp_rename N'dbo.Tmp_OGISGradBase', N'OGISGradBase', 'OBJECT' 
GO
ALTER TABLE dbo.OGISGradBase ADD CONSTRAINT
	pk_OGISGrads PRIMARY KEY CLUSTERED 
	(
	RecID,
	Semester
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
