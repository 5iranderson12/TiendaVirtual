USE Tienda;
GO


DELETE FROM Inventario;
DELETE FROM DetallesVenta;
DELETE FROM Ventas;
DELETE FROM Productos;
DELETE FROM Clientes;
DELETE FROM Empleados;
DELETE FROM Proveedores;
DELETE FROM Categorias;

DBCC CHECKIDENT ('Categorias', RESEED, 0);
DBCC CHECKIDENT ('Proveedores', RESEED, 0);
DBCC CHECKIDENT ('Productos', RESEED, 0);
DBCC CHECKIDENT ('Clientes', RESEED, 0);
DBCC CHECKIDENT ('Empleados', RESEED, 0);
DBCC CHECKIDENT ('Ventas', RESEED, 0);
DBCC CHECKIDENT ('DetallesVenta', RESEED, 0);
DBCC CHECKIDENT ('Inventario', RESEED, 0);

PRINT 'Datos anteriores eliminados';

INSERT INTO Categorias (nombre, descripcion, estado) VALUES
('Tecnología', 'Productos electrónicos y gadgets', 1),
('Muebles', 'Muebles para hogar y oficina', 1),
('Automóviles', 'Vehículos y accesorios automotrices', 1),
('Utilerías', 'Herramientas y equipos de trabajo', 1),
('Electrodomésticos', 'Aparatos eléctricos para el hogar', 1),
('Deportes', 'Equipos y artículos deportivos', 1);

INSERT INTO Proveedores (nombre, telefono, email, direccion, ciudad, estado) VALUES
('TechWorld Inc.', '2222-5555', 'ventas@techworld.com', 'Av. Tecnológica 123', 'San Salvador', 1),
('Muebles El Confort', '2233-4444', 'info@elconfort.com', 'Calle Principal 45', 'Santa Ana', 1),
('AutoMotriz S.A.', '2244-6666', 'contacto@automotriz.com', 'Blvd. Los Héroes 789', 'San Salvador', 1),
('Herramientas Pro', '2255-7777', 'ventas@herramientaspro.com', 'Zona Industrial 12', 'Sonsonate', 1),
('ElectroHogar', '2266-8888', 'info@electrohogar.com', 'Centro Comercial 34', 'San Miguel', 1),
('Deportes Máximos', '2277-9999', 'contacto@deportesmaximos.com', 'Plaza Deportiva 56', 'San Salvador', 1);

INSERT INTO Productos (codigo, nombre, descripcion, idCategoria, idProveedor, precioCompra, precioVenta, stock, stockMinimo, fechaRegistro, estado) VALUES
('TECH001', 'Laptop Dell Inspiron 15', 'Laptop Intel Core i7, 16GB RAM, 512GB SSD, Pantalla 15.6"', 1, 1, 650.00, 899.00, 15, 5, GETDATE(), 1),
('TECH002', 'iPhone 15 Pro Max', 'Smartphone Apple 256GB, Cámara 48MP, Pantalla 6.7"', 1, 1, 950.00, 1299.00, 20, 5, GETDATE(), 1),
('TECH003', 'Samsung Galaxy Tab S9', 'Tablet Android 128GB, Pantalla 11" AMOLED', 1, 1, 420.00, 599.00, 25, 8, GETDATE(), 1),
('TECH004', 'MacBook Air M2', 'Laptop Apple Silicon M2, 8GB RAM, 256GB SSD', 1, 1, 850.00, 1149.00, 12, 4, GETDATE(), 1),
('TECH005', 'AirPods Pro 2da Gen', 'Audífonos inalámbricos con cancelación de ruido', 1, 1, 180.00, 249.00, 30, 10, GETDATE(), 1),
('TECH006', 'Smartwatch Apple Watch Series 9', 'Reloj inteligente GPS + Cellular, Pantalla Retina', 1, 1, 320.00, 429.00, 18, 6, GETDATE(), 1);

INSERT INTO Productos (codigo, nombre, descripcion, idCategoria, idProveedor, precioCompra, precioVenta, stock, stockMinimo, fechaRegistro, estado) VALUES
('MUEB001', 'Sofá Seccional Moderno', 'Sofá en forma de L, tapizado en tela gris, capacidad 5 personas', 2, 2, 580.00, 849.00, 8, 2, GETDATE(), 1),
('MUEB002', 'Mesa de Comedor Extensible', 'Mesa de madera de roble para 6-8 personas, diseño contemporáneo', 2, 2, 320.00, 499.00, 10, 3, GETDATE(), 1),
('MUEB003', 'Cama King Size con Cabecera', 'Cama tapizada en cuero sintético color café', 2, 2, 450.00, 699.00, 12, 4, GETDATE(), 1),
('MUEB004', 'Escritorio Ejecutivo L-Shape', 'Escritorio esquinero con cajones, acabado cerezo', 2, 2, 280.00, 429.00, 15, 5, GETDATE(), 1),
('MUEB005', 'Silla Ergonómica Oficina', 'Silla giratoria con soporte lumbar, reposabrazos ajustables', 2, 2, 120.00, 189.00, 25, 8, GETDATE(), 1),
('MUEB006', 'Librero Modular 5 Niveles', 'Estantería de madera color nogal, 180cm altura', 2, 2, 95.00, 149.00, 20, 6, GETDATE(), 1);

INSERT INTO Productos (codigo, nombre, descripcion, idCategoria, idProveedor, precioCompra, precioVenta, stock, stockMinimo, fechaRegistro, estado) VALUES
('AUTO001', 'Toyota Corolla 2024', 'Sedán 4 puertas, motor 1.8L, transmisión automática', 3, 3, 18500.00, 24999.00, 5, 1, GETDATE(), 1),
('AUTO002', 'Honda CR-V 2024', 'SUV compacta, motor turbo 1.5L, tracción AWD', 3, 3, 26500.00, 34999.00, 4, 1, GETDATE(), 1),
('AUTO003', 'Mazda CX-5 Grand Touring', 'SUV mediana, motor 2.5L Turbo, interiores premium', 3, 3, 28000.00, 36999.00, 3, 1, GETDATE(), 1),
('AUTO004', 'Ford F-150 XLT', 'Pickup doble cabina, motor V6 3.5L, caja 6.5 pies', 3, 3, 32000.00, 42999.00, 6, 2, GETDATE(), 1),
('AUTO005', 'Chevrolet Silverado 1500', 'Pickup crew cab, motor V8 5.3L, 4x4', 3, 3, 34500.00, 45999.00, 4, 1, GETDATE(), 1),
('AUTO006', 'Hyundai Tucson Hybrid', 'SUV híbrida, motor 1.6L turbo + eléctrico, económica', 3, 3, 24000.00, 31999.00, 7, 2, GETDATE(), 1);

INSERT INTO Productos (codigo, nombre, descripcion, idCategoria, idProveedor, precioCompra, precioVenta, stock, stockMinimo, fechaRegistro, estado) VALUES
('UTIL001', 'Taladro Inalámbrico DeWalt', 'Taladro/atornillador 20V MAX, 2 baterías, estuche', 4, 4, 110.00, 169.00, 20, 6, GETDATE(), 1),
('UTIL002', 'Set Herramientas 150 Piezas', 'Caja de herramientas completa con llaves, dados, destornilladores', 4, 4, 75.00, 119.00, 30, 10, GETDATE(), 1),
('UTIL003', 'Sierra Circular Makita', 'Sierra de mano 7.25", 1800W, con guía láser', 4, 4, 95.00, 149.00, 15, 5, GETDATE(), 1),
('UTIL004', 'Compresor de Aire Portátil', 'Compresor 6 galones, 150 PSI, libre de aceite', 4, 4, 180.00, 279.00, 12, 4, GETDATE(), 1),
('UTIL005', 'Escalera Telescópica Aluminio', 'Escalera extensible 16 pies, capacidad 300 lbs', 4, 4, 85.00, 139.00, 18, 6, GETDATE(), 1),
('UTIL006', 'Generador Eléctrico 3500W', 'Generador a gasolina, arranque eléctrico, 4 tomas', 4, 4, 420.00, 649.00, 8, 2, GETDATE(), 1);

INSERT INTO Productos (codigo, nombre, descripcion, idCategoria, idProveedor, precioCompra, precioVenta, stock, stockMinimo, fechaRegistro, estado) VALUES
('ELEC001', 'Refrigeradora Samsung 28 pies', 'Refrigerador French Door, dispensador agua/hielo, inox', 5, 5, 980.00, 1499.00, 10, 3, GETDATE(), 1),
('ELEC002', 'Lavadora LG Carga Frontal 18kg', 'Lavadora inteligente con vapor, 14 programas', 5, 5, 520.00, 799.00, 12, 4, GETDATE(), 1),
('ELEC003', 'Microondas Panasonic Inverter', 'Horno microondas 1.2 pies³, 1200W, sensor automático', 5, 5, 110.00, 179.00, 25, 8, GETDATE(), 1),
('ELEC004', 'Smart TV Samsung 65" QLED', 'Televisor 4K, HDR10+, Tizen OS, 120Hz', 5, 5, 720.00, 1099.00, 15, 5, GETDATE(), 1),
('ELEC005', 'Aire Acondicionado Split 18000BTU', 'A/C inverter, frío/calor, bajo consumo energético', 5, 5, 480.00, 749.00, 14, 4, GETDATE(), 1),
('ELEC006', 'Aspiradora Robot iRobot Roomba', 'Aspiradora inteligente, WiFi, mapeo láser, autocarga', 5, 5, 280.00, 429.00, 18, 6, GETDATE(), 1);

INSERT INTO Productos (codigo, nombre, descripcion, idCategoria, idProveedor, precioCompra, precioVenta, stock, stockMinimo, fechaRegistro, estado) VALUES
('DEPO001', 'Bicicleta Montaña Trek 27.5"', 'MTB suspensión delantera, 21 velocidades Shimano', 6, 6, 320.00, 499.00, 12, 4, GETDATE(), 1),
('DEPO002', 'Caminadora Eléctrica ProForm', 'Trotadora plegable, velocidad max 12 km/h, inclinación ajustable', 6, 6, 420.00, 649.00, 8, 3, GETDATE(), 1),
('DEPO003', 'Set Pesas Ajustables 24kg', 'Mancuernas con discos intercambiables, incluye barra', 6, 6, 65.00, 99.00, 20, 6, GETDATE(), 1),
('DEPO004', 'Balón Basketball Spalding NBA', 'Balón oficial tamaño 7, cuero sintético', 6, 6, 28.00, 45.00, 35, 10, GETDATE(), 1),
('DEPO005', 'Bicicleta Estática Spinning', 'Bici indoor con monitor digital, resistencia magnética', 6, 6, 280.00, 429.00, 10, 3, GETDATE(), 1),
('DEPO006', 'Raqueta Tenis Wilson Pro Staff', 'Raqueta profesional, fibra de carbono, grip 4 3/8"', 6, 6, 95.00, 149.00, 15, 5, GETDATE(), 1);

INSERT INTO Empleados (documento, nombre, apellido, cargo, telefono, email, fechaContratacion, salario, estado) VALUES
('00000000-0', 'Sistema', 'Automatizado', 'Administrador', '0000-0000', 'sistema@tienda.com', GETDATE(), 0.00, 1);

PRINT '========================================';
PRINT 'RESUMEN DE DATOS INSERTADOS:';
PRINT '========================================';
PRINT 'Categorías: ' + CAST((SELECT COUNT(*) FROM Categorias) AS VARCHAR);
PRINT 'Proveedores: ' + CAST((SELECT COUNT(*) FROM Proveedores) AS VARCHAR);
PRINT 'Productos: ' + CAST((SELECT COUNT(*) FROM Productos) AS VARCHAR);
PRINT 'Empleados: ' + CAST((SELECT COUNT(*) FROM Empleados) AS VARCHAR);
PRINT '========================================';
PRINT '';
PRINT 'PRODUCTOS POR CATEGORÍA:';
SELECT c.nombre AS Categoria, COUNT(p.idProducto) AS CantidadProductos
FROM Categorias c
LEFT JOIN Productos p ON c.idCategoria = p.idCategoria
GROUP BY c.nombre
ORDER BY c.idCategoria;
