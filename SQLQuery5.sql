USE Tienda;
GO

-- Ver productos de Tecnología
SELECT * FROM Productos WHERE idCategoria = 1;

-- Buscar productos con texto 'Tecnologia'
SELECT * FROM Productos 
WHERE nombre LIKE '%Tecnologia%' 
   OR descripcion LIKE '%Tecnologia%';

-- Buscar productos con texto 'Laptop'
SELECT * FROM Productos 
WHERE nombre LIKE '%Laptop%' 
   OR descripcion LIKE '%Laptop%';