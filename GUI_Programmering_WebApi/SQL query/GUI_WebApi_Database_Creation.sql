CREATE DATABASE GUI_WebApi_Database
GO

USE GUI_WebApi_Database
GO

CREATE TABLE Categories (
    categoryId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    categoryName VARCHAR(100) NOT NULL
);

CREATE TABLE Products (
    productId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    productName VARCHAR(150) NOT NULL,
    productPrice DECIMAL(10, 2) NOT NULL,
    categoryId INT NOT NULL,
    CONSTRAINT FK_Products_Categories FOREIGN KEY (categoryId)
        REFERENCES Categories(categoryId)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);


GO
CREATE TABLE Customers (
    customerId INT IDENTITY(1,1) PRIMARY KEY,
    firstName VARCHAR(100) NOT NULL,
    lastName VARCHAR(100) NOT NULL,
    email VARCHAR(150) UNIQUE NOT NULL,
    phone VARCHAR(20)
);

GO
CREATE TABLE Orders (
    orderId INT IDENTITY(1,1) PRIMARY KEY,
    orderDate DATETIME NOT NULL DEFAULT GETDATE(),
    customerId INT NOT NULL,
    CONSTRAINT FK_Orders_Customers FOREIGN KEY (customerId)
        REFERENCES Customers(customerId)
        ON DELETE CASCADE
);

GO
CREATE TABLE OrderDetails (
    orderDetailId INT IDENTITY(1,1) PRIMARY KEY,
    orderId INT NOT NULL,
    productId INT NOT NULL,
    quantity INT NOT NULL CHECK (quantity > 0),
    unitPrice DECIMAL(10,2) NOT NULL,

    CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (orderId)
        REFERENCES Orders(orderId)
        ON DELETE CASCADE,

    CONSTRAINT FK_OrderDetails_Products FOREIGN KEY (productId)
        REFERENCES Products(productId)
        ON DELETE CASCADE
);

