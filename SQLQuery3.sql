
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Tienda')
BEGIN
    CREATE DATABASE Tienda;
    PRINT 'Base de datos Tienda creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La base de datos Tienda ya existe';
END
GO

USE Tienda;
GO

PRINT 'Eliminando tablas existentes si las hay...';

IF OBJECT_ID('dbo.Inventario', 'U') IS NOT NULL
    DROP TABLE dbo.Inventario;

IF OBJECT_ID('dbo.DetallesVenta', 'U') IS NOT NULL
    DROP TABLE dbo.DetallesVenta;

IF OBJECT_ID('dbo.Ventas', 'U') IS NOT NULL
    DROP TABLE dbo.Ventas;

IF OBJECT_ID('dbo.Empleados', 'U') IS NOT NULL
    DROP TABLE dbo.Empleados;

IF OBJECT_ID('dbo.Clientes', 'U') IS NOT NULL
    DROP TABLE dbo.Clientes;

IF OBJECT_ID('dbo.Productos', 'U') IS NOT NULL
    DROP TABLE dbo.Productos;

IF OBJECT_ID('dbo.Proveedores', 'U') IS NOT NULL
    DROP TABLE dbo.Proveedores;

IF OBJECT_ID('dbo.Categorias', 'U') IS NOT NULL
    DROP TABLE dbo.Categorias;

PRINT 'Tablas eliminadas (si existían)';
GO

CREATE TABLE Categorias (
    idCategoria INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255) NULL,
    estado BIT NOT NULL DEFAULT 1
);
PRINT 'Tabla Categorias creada';
GO

CREATE TABLE Proveedores (
    idProveedor INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    telefono VARCHAR(20) NULL,
    email VARCHAR(100) NULL,
    direccion VARCHAR(200) NULL,
    ciudad VARCHAR(50) NULL,
    estado BIT NOT NULL DEFAULT 1
);
PRINT 'Tabla Proveedores creada';
GO

CREATE TABLE Productos (
    idProducto INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(20) NOT NULL UNIQUE,
    nombre VARCHAR(150) NOT NULL,
    descripcion VARCHAR(255) NULL,
    idCategoria INT NOT NULL,
    idProveedor INT NOT NULL,
    precioCompra DECIMAL(10,2) NOT NULL,
    precioVenta DECIMAL(10,2) NOT NULL,
    stock INT NOT NULL DEFAULT 0,
    stockMinimo INT NOT NULL DEFAULT 0,
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Productos_Categorias FOREIGN KEY (idCategoria) 
        REFERENCES Categorias(idCategoria),
    CONSTRAINT FK_Productos_Proveedores FOREIGN KEY (idProveedor) 
        REFERENCES Proveedores(idProveedor)
);
PRINT 'Tabla Productos creada';
GO

CREATE TABLE Clientes (
    idCliente INT PRIMARY KEY IDENTITY(1,1),
    documento VARCHAR(20) NOT NULL UNIQUE,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NULL,
    telefono VARCHAR(20) NULL,
    email VARCHAR(100) NULL,
    direccion VARCHAR(200) NULL,
    ciudad VARCHAR(50) NULL,
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado BIT NOT NULL DEFAULT 1
);
PRINT 'Tabla Clientes creada';
GO

CREATE TABLE Empleados (
    idEmpleado INT PRIMARY KEY IDENTITY(1,1),
    documento VARCHAR(20) NOT NULL UNIQUE,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NULL,
    cargo VARCHAR(50) NULL,
    telefono VARCHAR(20) NULL,
    email VARCHAR(100) NULL,
    fechaContratacion DATETIME NOT NULL DEFAULT GETDATE(),
    salario DECIMAL(10,2) NULL,
    estado BIT NOT NULL DEFAULT 1
);
PRINT 'Tabla Empleados creada';
GO

CREATE TABLE Ventas (
    idVenta INT PRIMARY KEY IDENTITY(1,1),
    idCliente INT NOT NULL,
    idEmpleado INT NOT NULL,
    fechaVenta DATETIME NOT NULL DEFAULT GETDATE(),
    subtotal DECIMAL(10,2) NOT NULL,
    impuesto DECIMAL(10,2) NOT NULL,
    total DECIMAL(10,2) NOT NULL,
    metodoPago VARCHAR(50) NULL,
    estado VARCHAR(20) NOT NULL DEFAULT 'COMPLETADA',
    CONSTRAINT FK_Ventas_Clientes FOREIGN KEY (idCliente) 
        REFERENCES Clientes(idCliente),
    CONSTRAINT FK_Ventas_Empleados FOREIGN KEY (idEmpleado) 
        REFERENCES Empleados(idEmpleado)
);
PRINT 'Tabla Ventas creada';
GO

CREATE TABLE DetallesVenta (
    idDetalleVenta INT PRIMARY KEY IDENTITY(1,1),
    idVenta INT NOT NULL,
    idProducto INT NOT NULL,
    cantidad INT NOT NULL,
    precioUnitario DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_DetallesVenta_Ventas FOREIGN KEY (idVenta) 
        REFERENCES Ventas(idVenta),
    CONSTRAINT FK_DetallesVenta_Productos FOREIGN KEY (idProducto) 
        REFERENCES Productos(idProducto)
);
PRINT 'Tabla DetallesVenta creada';
GO

CREATE TABLE Inventario (
    idMovimiento INT PRIMARY KEY IDENTITY(1,1),
    idProducto INT NOT NULL,
    tipoMovimiento VARCHAR(20) NOT NULL,
    cantidad INT NOT NULL,
    motivo VARCHAR(255) NULL,
    fechaMovimiento DATETIME NOT NULL DEFAULT GETDATE(),
    idEmpleado INT NOT NULL,
    CONSTRAINT FK_Inventario_Productos FOREIGN KEY (idProducto) 
        REFERENCES Productos(idProducto),
    CONSTRAINT FK_Inventario_Empleados FOREIGN KEY (idEmpleado) 
        REFERENCES Empleados(idEmpleado),
    CONSTRAINT CK_Inventario_TipoMovimiento CHECK (tipoMovimiento IN ('ENTRADA', 'SALIDA'))
);
PRINT 'Tabla Inventario creada';
GO

CREATE INDEX IX_Productos_Categoria ON Productos(idCategoria);
CREATE INDEX IX_Productos_Estado ON Productos(estado);
CREATE INDEX IX_Ventas_Cliente ON Ventas(idCliente);
CREATE INDEX IX_Ventas_Fecha ON Ventas(fechaVenta);
CREATE INDEX IX_DetallesVenta_Venta ON DetallesVenta(idVenta);
PRINT 'Índices creados';
GO


PRINT '';
PRINT '========================================';
PRINT 'ESTRUCTURA DE BASE DE DATOS CREADA';
PRINT '========================================';
PRINT '';

SELECT 
    t.name AS 'Tabla',
    COUNT(c.column_id) AS 'Columnas'
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
WHERE t.name IN (
    'Categorias', 'Proveedores', 'Productos', 'Clientes', 
    'Empleados', 'Ventas', 'DetallesVenta', 'Inventario'
)
GROUP BY t.name
ORDER BY t.name;
