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
    LastUpdateDate DATETIME NULL,
    PasswordHash NVARCHAR(64) NULL,
    Salt NVARCHAR(24) NULL
);
GO

INSERT INTO Customers (FirstName, LastName, Age, Location, LastPurchaseDate, LastUpdateDate, PasswordHash, Salt)
VALUES 
('Glaiza Loren', 'Malit', 40, 'Canada', GETDATE(), NULL, '', ''),
('Michael', 'Dune', 30, 'USA', GETDATE(), NULL, '', ''),
('Anna Marie', 'Santos', 28, 'Philippines', GETDATE(), NULL, '', ''),
('Carlos Alberto', 'Garcia', 50, 'Mexico', GETDATE(), NULL, '', ''),
('Sophia Louise', 'Miller', 35, 'Australia', GETDATE(), NULL, '', ''),
('Ethan James', 'Clark', 25, 'UK', GETDATE(), NULL, '', ''),
('Gladiz Loren', 'Sembrano', 38, 'Philippines', GETDATE(), NULL, '', ''),
('Oliver William', 'Davis', 38, 'Canada', GETDATE(), NULL, '', ''),
('Emily Rose', 'Martinez', 32, 'Spain', GETDATE(), NULL, '', ''),
('Jacob Daniel', 'Brown', 22, 'USA', GETDATE(), NULL, '', '');