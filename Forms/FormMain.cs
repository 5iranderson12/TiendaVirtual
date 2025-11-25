using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TiendaVirtual.Data;
using TiendaVirtual.Models;

namespace TiendaVirtual.Forms
{
    public class FormMain : Form
    {
        private TiendaRepository repository;
        private Cliente clienteActual;
        private List<Producto> todosProductos;
        private List<Categoria> categorias;
        private List<CarritoItem> carrito;

        // Controles principales
        private Panel panelHeader;
        private Panel panelBusqueda;
        private TextBox txtBusqueda;
        private Button btnCarrito;
        private Label lblContadorCarrito;
        private Panel panelBanner;
        private Panel panelCatalogo;

        public FormMain(Cliente cliente)
        {
            clienteActual = cliente;
            repository = new TiendaRepository();
            carrito = new List<CarritoItem>();

            InicializarComponentes();
            CargarDatos();
        }

        private void InicializarComponentes()
        {
            // Configuración del formulario
            this.Text = "Tienda Virtual - Catálogo";
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#141414");
            this.WindowState = FormWindowState.Maximized;

            // Header
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = ColorTranslator.FromHtml("#141414")
            };

            // Logo/Título
            Label lblTitulo = new Label
            {
                Text = "TIENDA VIRTUAL",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#E50914"),
                Location = new Point(30, 20),
                AutoSize = true
            };

            // Panel de búsqueda
            panelBusqueda = new Panel
            {
                Location = new Point(400, 20),
                Size = new Size(500, 40),
                BackColor = ColorTranslator.FromHtml("#333333")
            };

            txtBusqueda = new TextBox
            {
                Location = new Point(10, 8),
                Size = new Size(480, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = ColorTranslator.FromHtml("#333333"),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                Text = "Buscar productos..."
            };

            txtBusqueda.GotFocus += (s, e) =>
            {
                if (txtBusqueda.Text == "Buscar productos...")
                {
                    txtBusqueda.Text = "";
                    txtBusqueda.ForeColor = Color.White;
                }
            };

            txtBusqueda.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtBusqueda.Text))
                {
                    txtBusqueda.Text = "Buscar productos...";
                    txtBusqueda.ForeColor = ColorTranslator.FromHtml("#B3B3B3");
                    // Cuando pierde el foco y está vacío, mostrar todos los productos
                    CrearCatalogoPorCategorias();
                }
            };

            txtBusqueda.TextChanged += TxtBusqueda_TextChanged;

            panelBusqueda.Controls.Add(txtBusqueda);

            // Botón carrito 
            btnCarrito = new Button
            {
                Text = "🛒 CARRITO",
                Location = new Point(1050, 20),
                Size = new Size(230, 45),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#E50914"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCarrito.FlatAppearance.BorderSize = 0;
            btnCarrito.Click += BtnCarrito_Click;
            btnCarrito.MouseEnter += (s, e) => btnCarrito.BackColor = ColorTranslator.FromHtml("#F40612");
            btnCarrito.MouseLeave += (s, e) => btnCarrito.BackColor = ColorTranslator.FromHtml("#E50914");

            // Contador de productos en carrito (badge) - VISIBLE
            lblContadorCarrito = new Label
            {
                Text = "0",
                Size = new Size(35, 35),
                Location = new Point(1260, 8),
                BackColor = ColorTranslator.FromHtml("#FFD700"),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };

            // Hacer el badge circular
            lblContadorCarrito.Paint += (s, e) =>
            {
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(0, 0, lblContadorCarrito.Width - 1, lblContadorCarrito.Height - 1);
                lblContadorCarrito.Region = new Region(path);
            };

            // Panel banner principal (MÁS PEQUEÑO)
            panelBanner = new Panel
            {
                Location = new Point(0, 80),
                Size = new Size(this.Width, 280),
                BackColor = ColorTranslator.FromHtml("#1E1E1E"),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // Panel catálogo con scroll (EMPIEZA MÁS ARRIBA)
            panelCatalogo = new Panel
            {
                Location = new Point(0, 360),
                Size = new Size(this.Width, this.Height - 360),
                BackColor = ColorTranslator.FromHtml("#141414"),
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(panelBusqueda);
            panelHeader.Controls.Add(btnCarrito);
            panelHeader.Controls.Add(lblContadorCarrito);

            this.Controls.Add(panelHeader);
            this.Controls.Add(panelBanner);
            this.Controls.Add(panelCatalogo);
        }

        private void CargarDatos()
        {
            try
            {
                categorias = repository.ObtenerCategorias();
                todosProductos = repository.ObtenerProductos();

                CrearBanner();
                CrearCatalogoPorCategorias();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CrearBanner()
        {
            if (todosProductos.Count == 0) return;

            // Producto destacado (el más caro)
            var productoDestacado = todosProductos.OrderByDescending(p => p.PrecioVenta).FirstOrDefault();

            if (productoDestacado == null) return;

            // Gradiente de fondo
            Panel panelGradiente = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#1E1E1E")
            };

            // Contenido del banner (MÁS COMPACTO)
            Label lblCategoria = new Label
            {
                Text = productoDestacado.NombreCategoria.ToUpper(),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#E50914"),
                Location = new Point(50, 20),
                AutoSize = true
            };

            Label lblNombre = new Label
            {
                Text = productoDestacado.Nombre,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 50),
                Size = new Size(700, 50)
            };

            Label lblDescripcion = new Label
            {
                Text = productoDestacado.Descripcion,
                Font = new Font("Segoe UI", 11),
                ForeColor = ColorTranslator.FromHtml("#B3B3B3"),
                Location = new Point(50, 110),
                Size = new Size(700, 50)
            };

            Label lblPrecio = new Label
            {
                Text = $"${productoDestacado.PrecioVenta:N2}",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#E50914"),
                Location = new Point(50, 170),
                AutoSize = true
            };

            Button btnAgregarBanner = new Button
            {
                Text = "🛒 AGREGAR AL CARRITO",
                Location = new Point(50, 215),
                Size = new Size(250, 45),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#E50914"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = productoDestacado
            };
            btnAgregarBanner.FlatAppearance.BorderSize = 0;
            btnAgregarBanner.Click += BtnAgregarProducto_Click;
            btnAgregarBanner.MouseEnter += (s, e) => btnAgregarBanner.BackColor = ColorTranslator.FromHtml("#F40612");
            btnAgregarBanner.MouseLeave += (s, e) => btnAgregarBanner.BackColor = ColorTranslator.FromHtml("#E50914");

            panelGradiente.Controls.Add(lblCategoria);
            panelGradiente.Controls.Add(lblNombre);
            panelGradiente.Controls.Add(lblDescripcion);
            panelGradiente.Controls.Add(lblPrecio);
            panelGradiente.Controls.Add(btnAgregarBanner);

            panelBanner.Controls.Add(panelGradiente);
        }

        private void CrearCatalogoPorCategorias()
        {
            panelCatalogo.Controls.Clear();
            int yOffset = 20;

            foreach (var categoria in categorias)
            {
                var productosCategoria = todosProductos
                    .Where(p => p.IdCategoria == categoria.IdCategoria)
                    .ToList();

                if (productosCategoria.Count == 0) continue;

                // Título de categoría
                Label lblCategoria = new Label
                {
                    Text = categoria.Nombre.ToUpper(),
                    Font = new Font("Segoe UI", 18, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(30, yOffset),
                    AutoSize = true
                };

                panelCatalogo.Controls.Add(lblCategoria);
                yOffset += 40;

                // Panel horizontal para productos
                FlowLayoutPanel panelProductos = new FlowLayoutPanel
                {
                    Location = new Point(30, yOffset),
                    Size = new Size(this.Width - 60, 360),
                    AutoScroll = false,
                    WrapContents = false,
                    FlowDirection = FlowDirection.LeftToRight,
                    BackColor = Color.Transparent
                };

                // Crear cards de productos
                foreach (var producto in productosCategoria)
                {
                    Panel cardProducto = CrearCardProducto(producto);
                    panelProductos.Controls.Add(cardProducto);
                }

                panelCatalogo.Controls.Add(panelProductos);
                yOffset += 380;
            }
        }

        // Continuación de FormMain.cs

        private Panel CrearCardProducto(Producto producto)
        {
            Panel card = new Panel
            {
                Size = new Size(280, 330),
                BackColor = ColorTranslator.FromHtml("#1E1E1E"),
                Margin = new Padding(10),
                Cursor = Cursors.Hand,
                Tag = producto
            };

            // Efecto hover
            card.MouseEnter += (s, e) =>
            {
                card.BackColor = ColorTranslator.FromHtml("#2A2A2A");
                card.Size = new Size(285, 335);
            };

            card.MouseLeave += (s, e) =>
            {
                card.BackColor = ColorTranslator.FromHtml("#1E1E1E");
                card.Size = new Size(280, 330);
            };

            // Panel de "imagen" (color representativo)
            Panel panelImagen = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(280, 180),
                BackColor = ObtenerColorCategoria(producto.IdCategoria)
            };

            Label lblCategoriaCard = new Label
            {
                Text = producto.NombreCategoria.ToUpper(),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 70),
                Size = new Size(260, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };

            panelImagen.Controls.Add(lblCategoriaCard);

            // Nombre del producto
            Label lblNombre = new Label
            {
                Text = producto.Nombre,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 190),
                Size = new Size(260, 50),
                TextAlign = ContentAlignment.TopLeft
            };

            // Precio
            Label lblPrecio = new Label
            {
                Text = $"${producto.PrecioVenta:N2}",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#E50914"),
                Location = new Point(10, 245),
                AutoSize = true
            };

            // Stock
            Label lblStock = new Label
            {
                Text = $"Stock: {producto.Stock} unidades",
                Font = new Font("Segoe UI", 9),
                ForeColor = ColorTranslator.FromHtml("#B3B3B3"),
                Location = new Point(10, 270),
                AutoSize = true
            };

            // Botón agregar
            Button btnAgregar = new Button
            {
                Text = "➕",
                Location = new Point(230, 280),
                Size = new Size(40, 40),
                Font = new Font("Segoe UI", 14),
                BackColor = ColorTranslator.FromHtml("#E50914"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = producto
            };
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.Click += BtnAgregarProducto_Click;

            card.Controls.Add(panelImagen);
            card.Controls.Add(lblNombre);
            card.Controls.Add(lblPrecio);
            card.Controls.Add(lblStock);
            card.Controls.Add(btnAgregar);

            return card;
        }

        private Color ObtenerColorCategoria(int idCategoria)
        {
            switch (idCategoria)
            {
                case 1: return ColorTranslator.FromHtml("#1E3A8A"); // Tecnología - Azul oscuro
                case 2: return ColorTranslator.FromHtml("#7C2D12"); // Muebles - Marrón
                case 3: return ColorTranslator.FromHtml("#1E293B"); // Automóviles - Gris oscuro
                case 4: return ColorTranslator.FromHtml("#713F12"); // Utilerías - Naranja oscuro
                case 5: return ColorTranslator.FromHtml("#164E63"); // Electrodomésticos - Cyan oscuro
                case 6: return ColorTranslator.FromHtml("#14532D"); // Deportes - Verde oscuro
                default: return ColorTranslator.FromHtml("#333333");
            }
        }

        private void BtnAgregarProducto_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Producto producto = btn.Tag as Producto;
            if (producto == null) return;

            try
            {
                // Verificar stock antes de cualquier operación
                if (producto.Stock <= 0)
                {
                    MessageBox.Show(
                        $"❌ SIN STOCK DISPONIBLE\n\n" +
                        $"Producto: {producto.Nombre}\n" +
                        $"Stock actual: 0 unidades\n\n" +
                        $"Este producto no está disponible en este momento.",
                        "Sin Stock",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Verificar si el producto ya está en el carrito
                var itemExistente = carrito.FirstOrDefault(c => c.Producto.IdProducto == producto.IdProducto);

                if (itemExistente != null)
                {
                    // Verificar stock disponible contra lo que ya tiene en el carrito
                    if (itemExistente.Cantidad >= producto.Stock)
                    {
                        MessageBox.Show(
                            $"⚠️ STOCK INSUFICIENTE\n\n" +
                            $"Producto: {producto.Nombre}\n" +
                            $"Stock disponible: {producto.Stock} unidades\n" +
                            $"Ya tienes en el carrito: {itemExistente.Cantidad} unidades\n\n" +
                            $"Has alcanzado el máximo disponible de este producto.",
                            "Stock Insuficiente",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    itemExistente.Cantidad++;
                }
                else
                {
                    // Es un producto nuevo, agregarlo al carrito
                    carrito.Add(new CarritoItem
                    {
                        Producto = producto,
                        Cantidad = 1
                    });
                }

                // Actualizar contador visual
                ActualizarContadorCarrito();

                // Feedback visual mejorado
                string textoOriginal = btn.Text;
                Color colorOriginal = btn.BackColor;

                btn.Text = "✓ AGREGADO";
                btn.BackColor = Color.Green;
                btn.Enabled = false;

                // Animar el contador
                AnimarContador();

                Timer timer = new Timer { Interval = 1200 };
                timer.Tick += (s, ev) =>
                {
                    btn.Text = textoOriginal;
                    btn.BackColor = colorOriginal;
                    btn.Enabled = true;
                    timer.Stop();
                };
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar producto: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ActualizarContadorCarrito()
        {
            int totalProductos = carrito.Sum(c => c.Cantidad);

            if (totalProductos > 0)
            {
                lblContadorCarrito.Text = totalProductos.ToString();
                lblContadorCarrito.Visible = true;

                // Actualizar texto del botón carrito
                btnCarrito.Text = $"🛒 CARRITO ({totalProductos})";
            }
            else
            {
                lblContadorCarrito.Visible = false;
                btnCarrito.Text = "🛒 CARRITO";
            }
        }

        private void AnimarContador()
        {
            // Efecto de "pop" en el contador
            if (lblContadorCarrito.Visible)
            {
                int sizeTemporal = 35;
                lblContadorCarrito.Size = new Size(sizeTemporal, sizeTemporal);
                lblContadorCarrito.Font = new Font("Segoe UI", 13, FontStyle.Bold);

                Timer timer = new Timer { Interval = 200 };
                timer.Tick += (s, e) =>
                {
                    lblContadorCarrito.Size = new Size(30, 30);
                    lblContadorCarrito.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    timer.Stop();
                };
                timer.Start();
            }
        }

        private void BtnCarrito_Click(object sender, EventArgs e)
        {
            if (carrito.Count == 0)
            {
                MessageBox.Show(
                    "Tu carrito está vacío.\n\n" +
                    "Agrega productos usando el botón '🛒 AGREGAR' en cualquier producto.",
                    "Carrito Vacío",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            FormCarrito formCarrito = new FormCarrito(carrito, clienteActual);
            DialogResult resultado = formCarrito.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                // La compra fue exitosa, limpiar carrito y recargar datos
                carrito.Clear();
                ActualizarContadorCarrito();
                CargarDatos();

                MessageBox.Show(
                    "═══════════════════════════════\n" +
                    "  ✓ COMPRA FINALIZADA\n" +
                    "═══════════════════════════════\n\n" +
                    "¡Gracias por tu compra!\n\n" +
                    "Tu pedido ha sido registrado exitosamente\n" +
                    "y será procesado pronto.\n\n" +
                    "Puedes seguir navegando y\n" +
                    "agregar más productos.",
                    "Compra Exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else if (resultado == DialogResult.Cancel)
            {
                // El usuario canceló o siguió comprando
                // Actualizar el contador por si eliminó productos
                ActualizarContadorCarrito();
            }
        }

        private void TxtBusqueda_TextChanged(object sender, EventArgs e)
        {
            string termino = txtBusqueda.Text.Trim();

            // Si está vacío o es el placeholder, mostrar todos los productos
            if (termino == "Buscar productos..." || string.IsNullOrWhiteSpace(termino))
            {
                CrearCatalogoPorCategorias();
                return;
            }

            try
            {
                var resultados = repository.BuscarProductos(termino);

                if (resultados == null || resultados.Count == 0)
                {
                    MostrarResultadosBusqueda(new List<Producto>());
                }
                else
                {
                    MostrarResultadosBusqueda(resultados);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la búsqueda: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // En caso de error, mostrar todos los productos
                CrearCatalogoPorCategorias();
            }
        }

        private void MostrarResultadosBusqueda(List<Producto> resultados)
        {
            panelCatalogo.Controls.Clear();

            if (resultados.Count == 0)
            {
                Label lblSinResultados = new Label
                {
                    Text = "No se encontraron productos que coincidan con tu búsqueda",
                    Font = new Font("Segoe UI", 16),
                    ForeColor = ColorTranslator.FromHtml("#B3B3B3"),
                    Location = new Point(30, 50),
                    AutoSize = true
                };

                panelCatalogo.Controls.Add(lblSinResultados);
                return;
            }

            // Título de resultados
            Label lblResultados = new Label
            {
                Text = $"RESULTADOS DE BÚSQUEDA ({resultados.Count} productos encontrados)",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            panelCatalogo.Controls.Add(lblResultados);

            // Panel con resultados
            FlowLayoutPanel panelResultados = new FlowLayoutPanel
            {
                Location = new Point(30, 70),
                Size = new Size(this.Width - 60, this.Height - 200),
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.Transparent
            };

            foreach (var producto in resultados)
            {
                Panel cardProducto = CrearCardProducto(producto);
                panelResultados.Controls.Add(cardProducto);
            }

            panelCatalogo.Controls.Add(panelResultados);
        }
    }
}