CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255)
);


CREATE TABLE Expenses (
    ExpenseId INT PRIMARY KEY IDENTITY(1,1),
    CategoryId INT NOT NULL,
    Amount DECIMAL(10, 2) NOT NULL,
    ExpenseDate DATETIME NOT NULL,
    Description NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId) ON DELETE CASCADE
);