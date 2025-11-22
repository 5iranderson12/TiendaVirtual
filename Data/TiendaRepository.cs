using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TiendaVirtual.Models;

namespace TiendaVirtual.Data
{
    /// <summary>
    /// Repositorio con todas las operaciones de base de datos usando ADO.NET puro
    /// </summary>
    public class TiendaRepository
    {
        // ==================== CLIENTES ====================

        /// <summary>
        /// Busca un cliente por su documento
        /// </summary>
        public Cliente ObtenerClientePorDocumento(string documento)
        {
            Cliente cliente = null;
            string query = "SELECT * FROM Clientes WHERE documento = @documento AND estado = 1";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@documento", documento);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cliente = new Cliente
                                {
                                    IdCliente = Convert.ToInt32(reader["idCliente"]),
                                    Documento = reader["documento"].ToString(),
                                    Nombre = reader["nombre"].ToString(),
                                    Apellido = reader["apellido"].ToString(),
                                    Telefono = reader["telefono"].ToString(),
                                    Email = reader["email"].ToString(),
                                    Direccion = reader["direccion"].ToString(),
                                    Ciudad = reader["ciudad"].ToString(),
                                    FechaRegistro = Convert.ToDateTime(reader["fechaRegistro"]),
                                    Estado = Convert.ToBoolean(reader["estado"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener cliente: {ex.Message}", ex);
            }

            return cliente;
        }

        /// <summary>
        /// Registra un nuevo cliente en la base de datos
        /// </summary>
        public int RegistrarCliente(Cliente cliente)
        {
            string query = @"INSERT INTO Clientes 
                (documento, nombre, apellido, telefono, email, direccion, ciudad, fechaRegistro, estado)
                VALUES 
                (@documento, @nombre, @apellido, @telefono, @email, @direccion, @ciudad, @fechaRegistro, @estado);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@documento", cliente.Documento);
                        cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
                        cmd.Parameters.AddWithValue("@apellido", cliente.Apellido);
                        cmd.Parameters.AddWithValue("@telefono", cliente.Telefono ?? "");
                        cmd.Parameters.AddWithValue("@email", cliente.Email ?? "");
                        cmd.Parameters.AddWithValue("@direccion", cliente.Direccion ?? "");
                        cmd.Parameters.AddWithValue("@ciudad", cliente.Ciudad ?? "");
                        cmd.Parameters.AddWithValue("@fechaRegistro", cliente.FechaRegistro);
                        cmd.Parameters.AddWithValue("@estado", cliente.Estado);

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al registrar cliente: {ex.Message}", ex);
            }
        }

        // ==================== CATEGORÍAS ====================

        /// <summary>
        /// Obtiene todas las categorías activas
        /// </summary>
        public List<Categoria> ObtenerCategorias()
        {
            List<Categoria> categorias = new List<Categoria>();
            string query = "SELECT * FROM Categorias WHERE estado = 1 ORDER BY nombre";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categorias.Add(new Categoria
                                {
                                    IdCategoria = Convert.ToInt32(reader["idCategoria"]),
                                    Nombre = reader["nombre"].ToString(),
                                    Descripcion = reader["descripcion"].ToString(),
                                    Estado = Convert.ToBoolean(reader["estado"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener categorías: {ex.Message}", ex);
            }

            return categorias;
        }

        // ==================== PRODUCTOS ====================

        /// <summary>
        /// Obtiene todos los productos activos con información de categoría
        /// </summary>
        public List<Producto> ObtenerProductos()
        {
            List<Producto> productos = new List<Producto>();
            string query = @"SELECT p.*, c.nombre AS nombreCategoria 
                FROM Productos p 
                INNER JOIN Categorias c ON p.idCategoria = c.idCategoria
                WHERE p.estado = 1
                ORDER BY c.nombre, p.nombre";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add(MapearProducto(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener productos: {ex.Message}", ex);
            }

            return productos;
        }

        /// <summary>
        /// Obtiene productos por categoría
        /// </summary>
        public List<Producto> ObtenerProductosPorCategoria(int idCategoria)
        {
            List<Producto> productos = new List<Producto>();
            string query = @"SELECT p.*, c.nombre AS nombreCategoria 
                FROM Productos p 
                INNER JOIN Categorias c ON p.idCategoria = c.idCategoria
                WHERE p.idCategoria = @idCategoria AND p.estado = 1
                ORDER BY p.nombre";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idCategoria", idCategoria);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add(MapearProducto(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener productos por categoría: {ex.Message}", ex);
            }

            return productos;
        }

        /// <summary>
        /// Busca productos por nombre o descripción
        /// </summary>
        public List<Producto> BuscarProductos(string termino)
        {
            List<Producto> productos = new List<Producto>();
            string query = @"SELECT p.*, c.nombre AS nombreCategoria 
                FROM Productos p 
                INNER JOIN Categorias c ON p.idCategoria = c.idCategoria
                WHERE p.estado = 1 AND (p.nombre LIKE @termino OR p.descripcion LIKE @termino)
                ORDER BY p.nombre";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@termino", "%" + termino + "%");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add(MapearProducto(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar productos: {ex.Message}", ex);
            }

            return productos;
        }

        /// <summary>
        /// Mapea un SqlDataReader a un objeto Producto
        /// </summary>
        private Producto MapearProducto(SqlDataReader reader)
        {
            return new Producto
            {
                IdProducto = Convert.ToInt32(reader["idProducto"]),
                Codigo = reader["codigo"].ToString(),
                Nombre = reader["nombre"].ToString(),
                Descripcion = reader["descripcion"].ToString(),
                IdCategoria = Convert.ToInt32(reader["idCategoria"]),
                IdProveedor = Convert.ToInt32(reader["idProveedor"]),
                PrecioCompra = Convert.ToDecimal(reader["precioCompra"]),
                PrecioVenta = Convert.ToDecimal(reader["precioVenta"]),
                Stock = Convert.ToInt32(reader["stock"]),
                StockMinimo = Convert.ToInt32(reader["stockMinimo"]),
                FechaRegistro = Convert.ToDateTime(reader["fechaRegistro"]),
                Estado = Convert.ToBoolean(reader["estado"]),
                NombreCategoria = reader["nombreCategoria"].ToString()
            };
        }

        // ==================== EMPLEADOS ====================

        /// <summary>
        /// Obtiene el empleado por defecto del sistema
        /// </summary>
        public Empleado ObtenerEmpleadoSistema()
        {
            Empleado empleado = null;
            string query = "SELECT TOP 1 * FROM Empleados WHERE estado = 1 ORDER BY idEmpleado";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                empleado = new Empleado
                                {
                                    IdEmpleado = Convert.ToInt32(reader["idEmpleado"]),
                                    Documento = reader["documento"].ToString(),
                                    Nombre = reader["nombre"].ToString(),
                                    Apellido = reader["apellido"].ToString(),
                                    Cargo = reader["cargo"].ToString(),
                                    Telefono = reader["telefono"].ToString(),
                                    Email = reader["email"].ToString(),
                                    FechaContratacion = Convert.ToDateTime(reader["fechaContratacion"]),
                                    Salario = Convert.ToDecimal(reader["salario"]),
                                    Estado = Convert.ToBoolean(reader["estado"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener empleado: {ex.Message}", ex);
            }

            return empleado;
        }

        // ==================== VENTAS ====================

        /// <summary>
        /// Registra una venta completa con sus detalles y actualiza el inventario
        /// Usa transacciones para garantizar consistencia
        /// </summary>
        public int RegistrarVenta(Venta venta, List<DetalleVenta> detalles)
        {
            SqlConnection conn = null;
            SqlTransaction transaction = null;

            try
            {
                conn = DatabaseConnection.GetConnection();
                conn.Open();
                transaction = conn.BeginTransaction();

                // 1. Insertar la venta
                string queryVenta = @"INSERT INTO Ventas 
                    (idCliente, idEmpleado, fechaVenta, subtotal, impuesto, total, metodoPago, estado)
                    VALUES 
                    (@idCliente, @idEmpleado, @fechaVenta, @subtotal, @impuesto, @total, @metodoPago, @estado);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                int idVenta;
                using (SqlCommand cmd = new SqlCommand(queryVenta, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@idCliente", venta.IdCliente);
                    cmd.Parameters.AddWithValue("@idEmpleado", venta.IdEmpleado);
                    cmd.Parameters.AddWithValue("@fechaVenta", venta.FechaVenta);
                    cmd.Parameters.AddWithValue("@subtotal", venta.Subtotal);
                    cmd.Parameters.AddWithValue("@impuesto", venta.Impuesto);
                    cmd.Parameters.AddWithValue("@total", venta.Total);
                    cmd.Parameters.AddWithValue("@metodoPago", venta.MetodoPago);
                    cmd.Parameters.AddWithValue("@estado", venta.Estado);

                    idVenta = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // 2. Insertar detalles de venta y actualizar stock
                string queryDetalle = @"INSERT INTO DetallesVenta 
                    (idVenta, idProducto, cantidad, precioUnitario, subtotal)
                    VALUES 
                    (@idVenta, @idProducto, @cantidad, @precioUnitario, @subtotal)";

                string queryActualizarStock = "UPDATE Productos SET stock = stock - @cantidad WHERE idProducto = @idProducto";

                string queryInventario = @"INSERT INTO Inventario 
                    (idProducto, tipoMovimiento, cantidad, motivo, fechaMovimiento, idEmpleado)
                    VALUES 
                    (@idProducto, 'SALIDA', @cantidad, @motivo, @fechaMovimiento, @idEmpleado)";

                foreach (DetalleVenta detalle in detalles)
                {
                    // Insertar detalle
                    using (SqlCommand cmd = new SqlCommand(queryDetalle, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@idVenta", idVenta);
                        cmd.Parameters.AddWithValue("@idProducto", detalle.IdProducto);
                        cmd.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                        cmd.Parameters.AddWithValue("@precioUnitario", detalle.PrecioUnitario);
                        cmd.Parameters.AddWithValue("@subtotal", detalle.Subtotal);
                        cmd.ExecuteNonQuery();
                    }

                    // Actualizar stock
                    using (SqlCommand cmd = new SqlCommand(queryActualizarStock, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                        cmd.Parameters.AddWithValue("@idProducto", detalle.IdProducto);
                        cmd.ExecuteNonQuery();
                    }

                    // Registrar movimiento de inventario
                    using (SqlCommand cmd = new SqlCommand(queryInventario, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@idProducto", detalle.IdProducto);
                        cmd.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                        cmd.Parameters.AddWithValue("@motivo", $"Venta #{idVenta}");
                        cmd.Parameters.AddWithValue("@fechaMovimiento", DateTime.Now);
                        cmd.Parameters.AddWithValue("@idEmpleado", venta.IdEmpleado);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Confirmar transacción
                transaction.Commit();
                return idVenta;
            }
            catch (Exception ex)
            {
                // Revertir transacción en caso de error
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch { }
                }
                throw new Exception($"Error al registrar venta: {ex.Message}", ex);
            }
            finally
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}