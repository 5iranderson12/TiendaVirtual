using System;

namespace TiendaVirtual.Models
{
    // ===== CATEGORÍAS =====
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
    }

    // ===== PROVEEDORES =====
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public bool Estado { get; set; }
    }

    // ===== PRODUCTOS =====
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IdCategoria { get; set; }
        public int IdProveedor { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Estado { get; set; }

        // Propiedades auxiliares (no en BD)
        public string NombreCategoria { get; set; }
        public string NombreProveedor { get; set; }
    }

    // ===== CLIENTES =====
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Estado { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }

    // ===== EMPLEADOS =====
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cargo { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime FechaContratacion { get; set; }
        public decimal Salario { get; set; }
        public bool Estado { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }

    // ===== VENTAS =====
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; }
        public string Estado { get; set; }

        // Propiedades auxiliares
        public string NombreCliente { get; set; }
        public string NombreEmpleado { get; set; }
    }

    // ===== DETALLES DE VENTA =====
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        // Propiedades auxiliares
        public string NombreProducto { get; set; }
    }

    // ===== INVENTARIO =====
    public class Inventario
    {
        public int IdMovimiento { get; set; }
        public int IdProducto { get; set; }
        public string TipoMovimiento { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public int IdEmpleado { get; set; }

        // Propiedades auxiliares
        public string NombreProducto { get; set; }
        public string NombreEmpleado { get; set; }
    }

    // ===== ITEM DEL CARRITO =====
    public class CarritoItem
    {
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => Producto.PrecioVenta * Cantidad;
    }
}
