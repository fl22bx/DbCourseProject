CREATE TABLE project.dbo.Users (
	Name varchar(20) NOT NULL,
	Password varchar(100) NOT NULL,
	Email varchar(40),
	PRIMARY KEY (Name)
)
GO
