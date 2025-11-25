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
GO

SET IDENTITY_INSERT Categorias ON;

INSERT INTO Categorias (idCategoria, nombre, descripcion, estado) VALUES
(1, 'Tecnologia', 'Productos electronicos y gadgets', 1),
(2, 'Muebles', 'Muebles para hogar y oficina', 1),
(3, 'Automoviles', 'Vehiculos y accesorios automotrices', 1),
(4, 'Utilerias', 'Herramientas y equipos de trabajo', 1),
(5, 'Electrodomesticos', 'Aparatos electricos para el hogar', 1),
(6, 'Deportes', 'Equipos y articulos deportivos', 1);

SET IDENTITY_INSERT Categorias OFF;
GO

SET IDENTITY_INSERT Proveedores ON;

INSERT INTO Proveedores (idProveedor, nombre, telefono, email, direccion, ciudad, estado) VALUES
(1, 'TechWorld Inc.', '2222-5555', 'ventas@techworld.com', 'Av. Tecnologica 123', 'San Salvador', 1),
(2, 'Muebles El Confort', '2233-4444', 'info@elconfort.com', 'Calle Principal 45', 'Santa Ana', 1),
(3, 'AutoMotriz S.A.', '2244-6666', 'contacto@automotriz.com', 'Blvd. Los Heroes 789', 'San Salvador', 1),
(4, 'Herramientas Pro', '2255-7777', 'ventas@herramientaspro.com', 'Zona Industrial 12', 'Sonsonate', 1),
(5, 'ElectroHogar', '2266-8888', 'info@electrohogar.com', 'Centro Comercial 34', 'San Miguel', 1),
(6, 'Deportes Maximos', '2277-9999', 'contacto@deportesmaximos.com', 'Plaza Deportiva 56', 'San Salvador', 1);

SET IDENTITY_INSERT Proveedores OFF;
GO

SET IDENTITY_INSERT Productos ON;

INSERT INTO Productos (idProducto, codigo, nombre, descripcion, idCategoria, idProveedor, precioCompra, precioVenta, stock, stockMinimo, fechaRegistro, estado) VALUES
(1, 'TECH001', 'Laptop Dell Inspiron 15', 'Laptop Intel Core i7, 16GB RAM, 512GB SSD, Pantalla 15.6 pulgadas', 1, 1, 650.00, 899.00, 15, 5, GETDATE(), 1),
(2, 'TECH002', 'iPhone 15 Pro Max', 'Smartphone Apple 256GB, Camara 48MP, Pantalla 6.7 pulgadas', 1, 1, 950.00, 1299.00, 20, 5, GETDATE(), 1),
(3, 'TECH003', 'Samsung Galaxy Tab S9', 'Tablet Android 128GB, Pantalla 11 pulgadas AMOLED', 1, 1, 420.00, 599.00, 25, 8, GETDATE(), 1),
(4, 'TECH004', 'MacBook Air M2', 'Laptop Apple Silicon M2, 8GB RAM, 256GB SSD', 1, 1, 850.00, 1149.00, 12, 4, GETDATE(), 1),
(5, 'TECH005', 'AirPods Pro 2da Gen', 'Audifonos inalambricos con cancelacion de ruido', 1, 1, 180.00, 249.00, 30, 10, GETDATE(), 1),
(6, 'TECH006', 'Smartwatch Apple Watch Series 9', 'Reloj inteligente GPS + Cellular, Pantalla Retina', 1, 1, 320.00, 429.00, 18, 6, GETDATE(), 1),
(7, 'MUEB001', 'Sofa Seccional Moderno', 'Sofa en forma de L, tapizado en tela gris, capacidad 5 personas', 2, 2, 580.00, 849.00, 8, 2, GETDATE(), 1),
(8, 'MUEB002', 'Mesa de Comedor Extensible', 'Mesa de madera de roble para 6-8 personas, diseno contemporaneo', 2, 2, 320.00, 499.00, 10, 3, GETDATE(), 1),
(9, 'MUEB003', 'Cama King Size con Cabecera', 'Cama tapizada en cuero sintetico color cafe', 2, 2, 450.00, 699.00, 12, 4, GETDATE(), 1),
(10, 'MUEB004', 'Escritorio Ejecutivo L-Shape', 'Escritorio esquinero con cajones, acabado cerezo', 2, 2, 280.00, 429.00, 15, 5, GETDATE(), 1),
(11, 'MUEB005', 'Silla Ergonomica Oficina', 'Silla giratoria con soporte lumbar, reposabrazos ajustables', 2, 2, 120.00, 189.00, 25, 8, GETDATE(), 1),
(12, 'MUEB006', 'Librero Modular 5 Niveles', 'Estanteria de madera color nogal, 180cm altura', 2, 2, 95.00, 149.00, 20, 6, GETDATE(), 1),
(13, 'AUTO001', 'Toyota Corolla 2024', 'Sedan 4 puertas, motor 1.8L, transmision automatica', 3, 3, 18500.00, 24999.00, 5, 1, GETDATE(), 1),
(14, 'AUTO002', 'Honda CR-V 2024', 'SUV compacta, motor turbo 1.5L, traccion AWD', 3, 3, 26500.00, 34999.00, 4, 1, GETDATE(), 1),
(15, 'AUTO003', 'Mazda CX-5 Grand Touring', 'SUV mediana, motor 2.5L Turbo, interiores premium', 3, 3, 28000.00, 36999.00, 3, 1, GETDATE(), 1),
(16, 'AUTO004', 'Ford F-150 XLT', 'Pickup doble cabina, motor V6 3.5L, caja 6.5 pies', 3, 3, 32000.00, 42999.00, 6, 2, GETDATE(), 1),
(17, 'AUTO005', 'Chevrolet Silverado 1500', 'Pickup crew cab, motor V8 5.3L, 4x4', 3, 3, 34500.00, 45999.00, 4, 1, GETDATE(), 1),
(18, 'AUTO006', 'Hyundai Tucson Hybrid', 'SUV hibrida, motor 1.6L turbo + electrico, economica', 3, 3, 24000.00, 31999.00, 7, 2, GETDATE(), 1),
(19, 'UTIL001', 'Taladro Inalambrico DeWalt', 'Taladro atornillador 20V MAX, 2 baterias, estuche', 4, 4, 110.00, 169.00, 20, 6, GETDATE(), 1),
(20, 'UTIL002', 'Set Herramientas 150 Piezas', 'Caja de herramientas completa con llaves, dados, destornilladores', 4, 4, 75.00, 119.00, 30, 10, GETDATE(), 1),
(21, 'UTIL003', 'Sierra Circular Makita', 'Sierra de mano 7.25 pulgadas, 1800W, con guia laser', 4, 4, 95.00, 149.00, 15, 5, GETDATE(), 1),
(22, 'UTIL004', 'Compresor de Aire Portatil', 'Compresor 6 galones, 150 PSI, libre de aceite', 4, 4, 180.00, 279.00, 12, 4, GETDATE(), 1),
(23, 'UTIL005', 'Escalera Telescopica Aluminio', 'Escalera extensible 16 pies, capacidad 300 lbs', 4, 4, 85.00, 139.00, 18, 6, GETDATE(), 1),
(24, 'UTIL006', 'Generador Electrico 3500W', 'Generador a gasolina, arranque electrico, 4 tomas', 4, 4, 420.00, 649.00, 8, 2, GETDATE(), 1),
(25, 'ELEC001', 'Refrigeradora Samsung 28 pies', 'Refrigerador French Door, dispensador agua hielo, inox', 5, 5, 980.00, 1499.00, 10, 3, GETDATE(), 1),
(26, 'ELEC002', 'Lavadora LG Carga Frontal 18kg', 'Lavadora inteligente con vapor, 14 programas', 5, 5, 520.00, 799.00, 12, 4, GETDATE(), 1),
(27, 'ELEC003', 'Microondas Panasonic Inverter', 'Horno microondas 1.2 pies cubicos, 1200W, sensor automatico', 5, 5, 110.00, 179.00, 25, 8, GETDATE(), 1),
(28, 'ELEC004', 'Smart TV Samsung 65 QLED', 'Televisor 4K, HDR10+, Tizen OS, 120Hz', 5, 5, 720.00, 1099.00, 15, 5, GETDATE(), 1),
(29, 'ELEC005', 'Aire Acondicionado Split 18000BTU', 'AC inverter, frio calor, bajo consumo energetico', 5, 5, 480.00, 749.00, 14, 4, GETDATE(), 1),
(30, 'ELEC006', 'Aspiradora Robot iRobot Roomba', 'Aspiradora inteligente, WiFi, mapeo laser, autocarga', 5, 5, 280.00, 429.00, 18, 6, GETDATE(), 1),
(31, 'DEPO001', 'Bicicleta Montana Trek 27.5', 'MTB suspension delantera, 21 velocidades Shimano', 6, 6, 320.00, 499.00, 12, 4, GETDATE(), 1),
(32, 'DEPO002', 'Caminadora Electrica ProForm', 'Trotadora plegable, velocidad max 12 km/h, inclinacion ajustable', 6, 6, 420.00, 649.00, 8, 3, GETDATE(), 1),
(33, 'DEPO003', 'Set Pesas Ajustables 24kg', 'Mancuernas con discos intercambiables, incluye barra', 6, 6, 65.00, 99.00, 20, 6, GETDATE(), 1),
(34, 'DEPO004', 'Balon Basketball Spalding NBA', 'Balon oficial tamano 7, cuero sintetico', 6, 6, 28.00, 45.00, 35, 10, GETDATE(), 1),
(35, 'DEPO005', 'Bicicleta Estatica Spinning', 'Bici indoor con monitor digital, resistencia magnetica', 6, 6, 280.00, 429.00, 10, 3, GETDATE(), 1),
(36, 'DEPO006', 'Raqueta Tenis Wilson Pro Staff', 'Raqueta profesional, fibra de carbono, grip 4 3/8', 6, 6, 95.00, 149.00, 15, 5, GETDATE(), 1);

SET IDENTITY_INSERT Productos OFF;
GO

SET IDENTITY_INSERT Empleados ON;

INSERT INTO Empleados (idEmpleado, documento, nombre, apellido, cargo, telefono, email, fechaContratacion, salario, estado) VALUES
(1, '00000000-0', 'Sistema', 'Automatizado', 'Administrador', '0000-0000', 'sistema@tienda.com', GETDATE(), 0.00, 1);

SET IDENTITY_INSERT Empleados OFF;
GO

SELECT 'Categorias' AS Tabla, COUNT(*) AS Total FROM Categorias
UNION ALL
SELECT 'Proveedores', COUNT(*) FROM Proveedores
UNION ALL
SELECT 'Productos', COUNT(*) FROM Productos
UNION ALL
SELECT 'Empleados', COUNT(*) FROM Empleados;