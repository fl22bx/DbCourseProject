CREATE TABLE project.dbo.Sections (
	ID int IDENTITY(1,1) NOT NULL,
	Destination1 varchar(30) NOT NULL,
	Destination2 varchar(30),
	Part decimal(4,1) NOT NULL,
	Length int NOT NULL,
	LevelOfDifficulty int,
	GPXLink varchar(200),
	partOf int,
	PRIMARY KEY (ID)
)
GO
ALTER TABLE project.dbo.Sections
	ADD FOREIGN KEY (partOf) 
	REFERENCES dbo.PartTrail (ID)
GO


