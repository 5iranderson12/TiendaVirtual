using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TiendaVirtual.Data;
using TiendaVirtual.Models;

namespace TiendaVirtual.Forms
{
    public class FormCarrito : Form
    {
        private List<CarritoItem> carrito;
        private Cliente clienteActual;
        private TiendaRepository repository;

        // Controles
        private Panel panelHeader;
        private Panel panelProductos;
        private Panel panelResumen;
        private Label lblSubtotal;
        private Label lblImpuesto;
        private Label lblTotal;
        private ComboBox cboMetodoPago;
        private Button btnFinalizar;
        private Button btnVolver;

        private const decimal PORCENTAJE_IVA = 0.13m; // 13% IVA

        public FormCarrito(List<CarritoItem> items, Cliente cliente)
        {
            carrito = items;
            clienteActual = cliente;
            repository = new TiendaRepository();

            InicializarComponentes();
            MostrarProductos();
            CalcularTotales();
        }

        private void InicializarComponentes()
        {
            // Configuración del formulario
            this.Text = "Carrito de Compras";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#141414");
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(1000, 700);

            // Header
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = ColorTranslator.FromHtml("#1E1E1E")
            };

            Label lblTitulo = new Label
            {
                Text = "🛒 TU CARRITO DE COMPRAS",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            Label lblCliente = new Label
            {
                Text = $"Cliente: {clienteActual.NombreCompleto}",
                Font = new Font("Segoe UI", 14),
                ForeColor = ColorTranslator.FromHtml("#B3B3B3"),
                Location = new Point(30, 60),
                AutoSize = true
            };

            Label lblCantidadItems = new Label
            {
                Text = $"({carrito.Sum(c => c.Cantidad)} productos)",
                Font = new Font("Segoe UI", 14),
                ForeColor = ColorTranslator.FromHtml("#E50914"),
                Location = new Point(300, 60),
                AutoSize = true
            };

            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(lblCliente);
            panelHeader.Controls.Add(lblCantidadItems);

            // Panel de productos con scroll (LADO IZQUIERDO - MÁS GRANDE)
            panelProductos = new Panel
            {
                Location = new Point(20, 120),
                Size = new Size(750, 620),
                BackColor = ColorTranslator.FromHtml("#141414"),
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            // Panel resumen (LADO DERECHO - FIJO)
            panelResumen = new Panel
            {
                Location = new Point(790, 120),
                Size = new Size(380, 620),
                BackColor = ColorTranslator.FromHtml("#1E1E1E"),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right
            };

            // Título resumen
            Label lblTituloResumen = new Label
            {
                Text = "RESUMEN DE COMPRA",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(340, 35)
            };

            // Línea decorativa
            Panel lineaTop = new Panel
            {
                Location = new Point(20, 60),
                Size = new Size(340, 3),
                BackColor = ColorTranslator.FromHtml("#E50914")
            };

            // Cantidad de artículos
            Label lblCantidadArticulos = new Label
            {
                Text = "Artículos diferentes:",
                Font = new Font("Segoe UI", 12),
                ForeColor = ColorTranslator.FromHtml("#B3B3B3"),
                Location = new Point(20, 80),
                AutoSize = true
            };

            Label lblCantidadValor = new Label
            {
                Text = carrito.Count.ToString(),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(280, 80),
                Size = new Size(80, 25),
                TextAlign = ContentAlignment.MiddleRight
            };

            // Unidades totales
            Label lblUnidadesTotales = new Label
            {
                Text = "Unidades totales:",
                Font = new Font("Segoe UI", 12),
                ForeColor = ColorTranslator.FromHtml("#B3B3B3"),
                Location = new Point(20, 110),
                AutoSize = true
            };

            Label lblUnidadesValor = new Label
            {
                Text = carrito.Sum(c => c.Cantidad).ToString(),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(280, 110),
                Size = new Size(80, 25),
                TextAlign = ContentAlignment.MiddleRight
            };

            // Subtotal
            Label lblSubtotalLabel = new Label
            {
                Text = "Subtotal:",
                Font = new Font("Segoe UI", 14),
                ForeColor = ColorTranslator.FromHtml("#B3B3B3"),
                Location = new Point(20, 160),
                AutoSize = true
            };

            lblSubtotal = new Label
            {
                Text = "$0.00",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(200, 160),
                Size = new Size(160, 30),
                TextAlign = ContentAlignment.MiddleRight
            };

            // IVA
            Label lblImpuestoLabel = new Label
            {
                Text = "IVA (13%):",
                Font = new Font("Segoe UI", 14),
                ForeColor = ColorTranslator.FromHtml("#B3B3B3"),
                Location = new Point(20, 200),
                AutoSize = true
            };

            lblImpuesto = new Label
            {
                Text = "$0.00",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(200, 200),
                Size = new Size(160, 30),
                TextAlign = ContentAlignment.MiddleRight
            };

            // Línea separadora
            Panel lineaSeparadora = new Panel
            {
                Location = new Point(20, 250),
                Size = new Size(340, 3),
                BackColor = ColorTranslator.FromHtml("#E50914")
            };

            // Total (MÁS GRANDE Y DESTACADO)
            Label lblTotalLabel = new Label
            {
                Text = "TOTAL A PAGAR:",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 270),
                AutoSize = true
            };

            lblTotal = new Label
            {
                Text = "$0.00",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#E50914"),
                Location = new Point(100, 305),
                Size = new Size(260, 45),
                TextAlign = ContentAlignment.MiddleRight
            };

            // Método de pago
            Label lblMetodoPago = new Label
            {
                Text = "Método de Pago:",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 380),
                AutoSize = true
            };

            cboMetodoPago = new ComboBox
            {
                Location = new Point(20, 410),
                Size = new Size(340, 35),
                Font = new Font("Segoe UI", 12),
                BackColor = ColorTranslator.FromHtml("#333333"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboMetodoPago.Items.AddRange(new string[]
            {
                "💵 Efectivo",
                "💳 Tarjeta de Débito",
                "💳 Tarjeta de Crédito",
                "🏦 Transferencia Bancaria"
            });
            cboMetodoPago.SelectedIndex = 0;

            // Información adicional
            Label lblInfoEnvio = new Label
            {
                Text = "✓ Envío incluido\n✓ Garantía de 30 días\n✓ Devolución gratis",
                Font = new Font("Segoe UI", 10),
                ForeColor = ColorTranslator.FromHtml("#4ADE80"),
                Location = new Point(20, 460),
                Size = new Size(340, 60)
            };

            // Botón finalizar compra (MÁS GRANDE)
            btnFinalizar = new Button
            {
                Text = "🛒 FINALIZAR COMPRA",
                Location = new Point(20, 530),
                Size = new Size(340, 55),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#E50914"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnFinalizar.FlatAppearance.BorderSize = 0;
            btnFinalizar.Click += BtnFinalizar_Click;
            btnFinalizar.MouseEnter += (s, e) => btnFinalizar.BackColor = ColorTranslator.FromHtml("#F40612");
            btnFinalizar.MouseLeave += (s, e) => btnFinalizar.BackColor = ColorTranslator.FromHtml("#E50914");

            // Botón volver
            btnVolver = new Button
            {
                Text = "← SEGUIR COMPRANDO",
                Location = new Point(20, 595),
                Size = new Size(340, 45),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#333333"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            btnVolver.MouseEnter += (s, e) => btnVolver.BackColor = ColorTranslator.FromHtml("#404040");
            btnVolver.MouseLeave += (s, e) => btnVolver.BackColor = ColorTranslator.FromHtml("#333333");

            panelResumen.Controls.Add(lblTituloResumen);
            panelResumen.Controls.Add(lineaTop);
            panelResumen.Controls.Add(lblCantidadArticulos);
            panelResumen.Controls.Add(lblCantidadValor);
            panelResumen.Controls.Add(lblUnidadesTotales);
            panelResumen.Controls.Add(lblUnidadesValor);
            panelResumen.Controls.Add(lblSubtotalLabel);
            panelResumen.Controls.Add(lblSubtotal);
            panelResumen.Controls.Add(lblImpuestoLabel);
            panelResumen.Controls.Add(lblImpuesto);
            panelResumen.Controls.Add(lineaSeparadora);
            panelResumen.Controls.Add(lblTotalLabel);
            panelResumen.Controls.Add(lblTotal);
            panelResumen.Controls.Add(lblMetodoPago);
            panelResumen.Controls.Add(cboMetodoPago);
            panelResumen.Controls.Add(lblInfoEnvio);
            panelResumen.Controls.Add(btnFinalizar);
            panelResumen.Controls.Add(btnVolver);

            this.Controls.Add(panelHeader);
            this.Controls.Add(panelProductos);
            this.Controls.Add(panelResumen);
        }

        private void MostrarProductos()
        {
            panelProductos.Controls.Clear();
            int yOffset = 20;

            // Encabezado de la lista
            Label lblEncabezado = new Label
            {
                Text = $"PRODUCTOS EN TU CARRITO ({carrito.Count} artículos diferentes)",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, yOffset),
                AutoSize = true
            };
            panelProductos.Controls.Add(lblEncabezado);
            yOffset += 50;

            foreach (var item in carrito)
            {
                Panel panelItem = CrearItemCarrito(item);
                panelItem.Location = new Point(10, yOffset);
                panelProductos.Controls.Add(panelItem);
                yOffset += 160;
            }

            // Botón para vaciar carrito completo
            if (carrito.Count > 0)
            {
                Button btnVaciarCarrito = new Button
                {
                    Text = "🗑 VACIAR TODO EL CARRITO",
                    Location = new Point(10, yOffset),
                    Size = new Size(700, 45),
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    BackColor = ColorTranslator.FromHtml("#7C2D12"),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnVaciarCarrito.FlatAppearance.BorderSize = 0;
                btnVaciarCarrito.Click += (s, e) =>
                {
                    var resultado = MessageBox.Show(
                        "¿Estás seguro de que deseas vaciar todo el carrito?",
                        "Confirmar",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (resultado == DialogResult.Yes)
                    {
                        carrito.Clear();
                        MostrarProductos();
                        CalcularTotales();

                        if (carrito.Count == 0)
                        {
                            MessageBox.Show("Carrito vaciado", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.Cancel;
                            this.Close();
                        }
                    }
                };
                panelProductos.Controls.Add(btnVaciarCarrito);
            }
        }

        private Panel CrearItemCarrito(CarritoItem item)
        {
            Panel panel = new Panel
            {
                Size = new Size(700, 140),
                BackColor = ColorTranslator.FromHtml("#1E1E1E"),
                Margin = new Padding(10)
            };

            // Imagen de categoría (MÁS GRANDE)
            Panel panelImagen = new Panel
            {
                Location = new Point(15, 15),
                Size = new Size(110, 110),
                BackColor = ObtenerColorCategoria(item.Producto.IdCategoria)
            };

            Label lblCatImg = new Label
            {
                Text = item.Producto.NombreCategoria,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(5, 40),
                Size = new Size(100, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelImagen.Controls.Add(lblCatImg);

            // Nombre del producto (MÁS GRANDE Y VISIBLE)
            Label lblNombre = new Label
            {
                Text = item.Producto.Nombre,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(140, 15),
                Size = new Size(350, 35)
            };

            // Precio unitario
            Label lblPrecio = new Label
            {
                Text = $"Precio unitario: ${item.Producto.PrecioVenta:N2}",
                Font = new Font("Segoe UI", 11),
                ForeColor = ColorTranslator.FromHtml("#B3B3B3"),
                Location = new Point(140, 50),
                AutoSize = true
            };

            // Stock disponible
            Label lblStockDisp = new Label
            {
                Text = $"Stock disponible: {item.Producto.Stock}",
                Font = new Font("Segoe UI", 10),
                ForeColor = ColorTranslator.FromHtml("#4ADE80"),
                Location = new Point(140, 75),
                AutoSize = true
            };

            // Controles de cantidad (MÁS GRANDES)
            Button btnMenos = new Button
            {
                Text = "−",
                Location = new Point(140, 100),
                Size = new Size(45, 35),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#E50914"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = item
            };
            btnMenos.FlatAppearance.BorderSize = 0;
            btnMenos.Click += BtnMenos_Click;

            Label lblCantidad = new Label
            {
                Text = item.Cantidad.ToString(),
                Location = new Point(190, 100),
                Size = new Size(60, 35),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = ColorTranslator.FromHtml("#333333")
            };

            Button btnMas = new Button
            {
                Text = "+",
                Location = new Point(255, 100),
                Size = new Size(45, 35),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#E50914"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = item
            };
            btnMas.FlatAppearance.BorderSize = 0;
            btnMas.Click += BtnMas_Click;

            // Subtotal del item (MÁS GRANDE Y DESTACADO)
            Label lblSubtotalItem = new Label
            {
                Text = $"${item.Subtotal:N2}",
                Location = new Point(510, 40),
                Size = new Size(140, 40),
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#E50914"),
                TextAlign = ContentAlignment.MiddleRight
            };

            // Botón eliminar (MÁS VISIBLE)
            Button btnEliminar = new Button
            {
                Text = "🗑 Eliminar",
                Location = new Point(510, 90),
                Size = new Size(140, 40),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#DC2626"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = item
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += BtnEliminar_Click;
            btnEliminar.MouseEnter += (s, e) => btnEliminar.BackColor = ColorTranslator.FromHtml("#B91C1C");
            btnEliminar.MouseLeave += (s, e) => btnEliminar.BackColor = ColorTranslator.FromHtml("#DC2626");

            panel.Controls.Add(panelImagen);
            panel.Controls.Add(lblNombre);
            panel.Controls.Add(lblPrecio);
            panel.Controls.Add(lblStockDisp);
            panel.Controls.Add(btnMenos);
            panel.Controls.Add(lblCantidad);
            panel.Controls.Add(btnMas);
            panel.Controls.Add(lblSubtotalItem);
            panel.Controls.Add(btnEliminar);

            return panel;
        }

        private Color ObtenerColorCategoria(int idCategoria)
        {
            switch (idCategoria)
            {
                case 1: return ColorTranslator.FromHtml("#1E3A8A");
                case 2: return ColorTranslator.FromHtml("#7C2D12");
                case 3: return ColorTranslator.FromHtml("#1E293B");
                case 4: return ColorTranslator.FromHtml("#713F12");
                case 5: return ColorTranslator.FromHtml("#164E63");
                case 6: return ColorTranslator.FromHtml("#14532D");
                default: return ColorTranslator.FromHtml("#333333");
            }
        }

        private void BtnMenos_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            CarritoItem item = btn.Tag as CarritoItem;

            if (item.Cantidad > 1)
            {
                item.Cantidad--;
                MostrarProductos();
                CalcularTotales();
                ActualizarHeaderCantidad();
            }
            else
            {
                // Si la cantidad es 1 y presiona menos, preguntar si quiere eliminarlo
                var resultado = MessageBox.Show(
                    $"¿Deseas eliminar '{item.Producto.Nombre}' del carrito?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    carrito.Remove(item);
                    MostrarProductos();
                    CalcularTotales();
                    ActualizarHeaderCantidad();

                    if (carrito.Count == 0)
                    {
                        MessageBox.Show("Tu carrito está vacío", "Carrito Vacío", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                    }
                }
            }
        }

        private void BtnMas_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            CarritoItem item = btn.Tag as CarritoItem;

            if (item.Cantidad < item.Producto.Stock)
            {
                item.Cantidad++;
                MostrarProductos();
                CalcularTotales();
                ActualizarHeaderCantidad();
            }
            else
            {
                MessageBox.Show(
                    $"Stock máximo disponible: {item.Producto.Stock}\n\n" +
                    $"Ya tienes {item.Cantidad} unidades en tu carrito.",
                    "Stock Limitado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            CarritoItem item = btn.Tag as CarritoItem;

            var resultado = MessageBox.Show(
                $"¿Deseas eliminar este producto del carrito?\n\n" +
                $"Producto: {item.Producto.Nombre}\n" +
                $"Cantidad: {item.Cantidad} unidades\n" +
                $"Subtotal: ${item.Subtotal:N2}",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                carrito.Remove(item);
                MostrarProductos();
                CalcularTotales();
                ActualizarHeaderCantidad();

                if (carrito.Count == 0)
                {
                    MessageBox.Show("Tu carrito está vacío", "Carrito Vacío", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
        }

        private void ActualizarHeaderCantidad()
        {
            // Actualizar la etiqueta de cantidad en el header
            foreach (Control ctrl in panelHeader.Controls)
            {
                if (ctrl.Text.Contains("productos)"))
                {
                    ctrl.Text = $"({carrito.Sum(c => c.Cantidad)} productos)";
                    break;
                }
            }
        }

        private void CalcularTotales()
        {
            decimal subtotal = carrito.Sum(c => c.Subtotal);
            decimal impuesto = subtotal * PORCENTAJE_IVA;
            decimal total = subtotal + impuesto;

            lblSubtotal.Text = $"${subtotal:N2}";
            lblImpuesto.Text = $"${impuesto:N2}";
            lblTotal.Text = $"${total:N2}";
        }

        private void BtnFinalizar_Click(object sender, EventArgs e)
        {
            if (carrito.Count == 0)
            {
                MessageBox.Show("El carrito está vacío",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Limpiar el método de pago (quitar emojis)
            string metodoPagoLimpio = cboMetodoPago.Text.Replace("💵 ", "").Replace("💳 ", "").Replace("🏦 ", "");

            var confirmacion = MessageBox.Show(
                $"═══════════════════════════════\n" +
                $"   CONFIRMAR COMPRA\n" +
                $"═══════════════════════════════\n\n" +
                $"Artículos diferentes: {carrito.Count}\n" +
                $"Unidades totales: {carrito.Sum(c => c.Cantidad)}\n\n" +
                $"Subtotal: {lblSubtotal.Text}\n" +
                $"IVA (13%): {lblImpuesto.Text}\n" +
                $"═══════════════════════════════\n" +
                $"TOTAL: {lblTotal.Text}\n" +
                $"═══════════════════════════════\n\n" +
                $"Método de pago: {metodoPagoLimpio}\n\n" +
                $"¿Confirmas esta compra?",
                "Confirmar Compra",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                // Obtener empleado del sistema
                var empleado = repository.ObtenerEmpleadoSistema();

                if (empleado == null)
                {
                    MessageBox.Show("Error: No se encontró empleado del sistema",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                decimal subtotal = carrito.Sum(c => c.Subtotal);
                decimal impuesto = subtotal * PORCENTAJE_IVA;
                decimal total = subtotal + impuesto;

                // Crear venta
                Venta venta = new Venta
                {
                    IdCliente = clienteActual.IdCliente,
                    IdEmpleado = empleado.IdEmpleado,
                    FechaVenta = DateTime.Now,
                    Subtotal = subtotal,
                    Impuesto = impuesto,
                    Total = total,
                    MetodoPago = metodoPagoLimpio,
                    Estado = "COMPLETADA"
                };

                // Crear detalles
                List<DetalleVenta> detalles = new List<DetalleVenta>();

                foreach (var item in carrito)
                {
                    detalles.Add(new DetalleVenta
                    {
                        IdProducto = item.Producto.IdProducto,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.Producto.PrecioVenta,
                        Subtotal = item.Subtotal
                    });
                }

                // Registrar venta en BD
                int idVenta = repository.RegistrarVenta(venta, detalles);

                // Mensaje de éxito personalizado
                MessageBox.Show(
                    $"═══════════════════════════════\n" +
                    $"  ✓ COMPRA EXITOSA\n" +
                    $"═══════════════════════════════\n\n" +
                    $"Número de orden: #{idVenta}\n" +
                    $"Total pagado: {lblTotal.Text}\n" +
                    $"Método: {metodoPagoLimpio}\n\n" +
                    $"¡Gracias por tu compra!\n" +
                    $"Tu pedido será procesado pronto.",
                    "Compra Exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar la compra:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}