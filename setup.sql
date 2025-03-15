CREATE DATABASE CustomerDB;
GO
USE CustomerDB;
GO

CREATE TABLE Customers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Age INT NOT NULL,
    Location NVARCHAR(100) NOT NULL,
    LastPurchaseDate DATETIME NOT NULL,
    LastUpdateDate DATETIME NOT NULL,
    PasswordHash NVARCHAR(64) NULL,
    Salt NVARCHAR(24) NULL
);
GO

INSERT INTO Customers (FirstName, LastName, Age, Location, LastPurchaseDate, LastUpdateDate, PasswordHash, Salt)
VALUES ('Glaiza Loren', 'Malit', 40, 'Canada', GETDATE(), GETDATE(), '', '');
