CREATE TABLE project.dbo.TrekkedSections (
	ID int IDENTITY(1,1) NOT NULL,
	trekker varchar(20),
	path int,
	PRIMARY KEY (ID)
)
GO
ALTER TABLE project.dbo.TrekkedSections
	ADD FOREIGN KEY (trekker) 
	REFERENCES dbo.Users (Name)
GO

ALTER TABLE project.dbo.TrekkedSections
	ADD FOREIGN KEY (path) 
	REFERENCES dbo.Sections (ID)
GO


