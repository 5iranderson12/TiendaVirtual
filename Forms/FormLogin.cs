using System;
using System.Drawing;
using System.Windows.Forms;
using TiendaVirtual.Data;
using TiendaVirtual.Models;

namespace TiendaVirtual.Forms
{
    public class FormLogin : Form
    {
        private TiendaRepository repository;
        private Cliente clienteAutenticado;

        // Controles
        private Panel panelPrincipal;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private TextBox txtNombre;
        private TextBox txtEdad;
        private TextBox txtTarjeta;
        private Button btnIngresar;
        private Label lblError;

        public Cliente ClienteAutenticado => clienteAutenticado;

        public FormLogin()
        {
            repository = new TiendaRepository();
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            // Configuración del formulario
            this.Text = "Tienda Virtual - Bienvenida";
            this.Size = new Size(500, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ColorTranslator.FromHtml("#141414");

            // Panel principal
            panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40)
            };

            // Título
            lblTitulo = new Label
            {
                Text = "BIENVENIDO",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#E50914"),
                AutoSize = true,
                Location = new Point(40, 40)
            };

            // Subtítulo
            lblSubtitulo = new Label
            {
                Text = "Por favor ingresa tus datos para continuar",
                Font = new Font("Segoe UI", 11),
                ForeColor = ColorTranslator.FromHtml("#B3B3B3"),
                AutoSize = true,
                Location = new Point(40, 95)
            };

            // Campo Nombre Completo
            Label lblNombre = new Label
            {
                Text = "Nombre Completo:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(40, 150),
                AutoSize = true
            };

            txtNombre = new TextBox
            {
                Location = new Point(40, 175),
                Size = new Size(380, 35),
                Font = new Font("Segoe UI", 12),
                BackColor = ColorTranslator.FromHtml("#333333"),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Campo Edad
            Label lblEdad = new Label
            {
                Text = "Edad:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(40, 230),
                AutoSize = true
            };

            txtEdad = new TextBox
            {
                Location = new Point(40, 255),
                Size = new Size(380, 35),
                Font = new Font("Segoe UI", 12),
                BackColor = ColorTranslator.FromHtml("#333333"),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                MaxLength = 3
            };

            // Campo Tarjeta de Débito
            Label lblTarjeta = new Label
            {
                Text = "Número de Tarjeta de Débito (16 dígitos):",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(40, 310),
                AutoSize = true
            };

            txtTarjeta = new TextBox
            {
                Location = new Point(40, 335),
                Size = new Size(380, 35),
                Font = new Font("Segoe UI", 12),
                BackColor = ColorTranslator.FromHtml("#333333"),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                MaxLength = 16
            };

            // Etiqueta de error
            lblError = new Label
            {
                Location = new Point(40, 390),
                Size = new Size(380, 50),
                Font = new Font("Segoe UI", 9),
                ForeColor = ColorTranslator.FromHtml("#E50914"),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };

            // Botón Ingresar
            btnIngresar = new Button
            {
                Text = "INGRESAR",
                Location = new Point(40, 460),
                Size = new Size(380, 50),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#E50914"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.Click += BtnIngresar_Click;
            btnIngresar.MouseEnter += (s, e) => btnIngresar.BackColor = ColorTranslator.FromHtml("#F40612");
            btnIngresar.MouseLeave += (s, e) => btnIngresar.BackColor = ColorTranslator.FromHtml("#E50914");

            // Solo permitir números en edad
            txtEdad.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            // Solo permitir números en tarjeta
            txtTarjeta.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            // Formatear tarjeta mientras se escribe
            txtTarjeta.TextChanged += (s, e) =>
            {
                if (txtTarjeta.Text.Length == 16)
                {
                    string formateado = FormatearTarjeta(txtTarjeta.Text);
                    lblError.Text = $"Tarjeta: {formateado}";
                    lblError.ForeColor = ColorTranslator.FromHtml("#B3B3B3");
                    lblError.Visible = true;
                }
                else
                {
                    lblError.Visible = false;
                }
            };

            // Agregar controles al panel
            panelPrincipal.Controls.Add(lblTitulo);
            panelPrincipal.Controls.Add(lblSubtitulo);
            panelPrincipal.Controls.Add(lblNombre);
            panelPrincipal.Controls.Add(txtNombre);
            panelPrincipal.Controls.Add(lblEdad);
            panelPrincipal.Controls.Add(txtEdad);
            panelPrincipal.Controls.Add(lblTarjeta);
            panelPrincipal.Controls.Add(txtTarjeta);
            panelPrincipal.Controls.Add(lblError);
            panelPrincipal.Controls.Add(btnIngresar);

            this.Controls.Add(panelPrincipal);
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
                return;

            try
            {
                // Separar nombre y apellido
                string nombreCompleto = txtNombre.Text.Trim();
                string[] partes = nombreCompleto.Split(new[] { ' ' }, 2);
                string nombre = partes[0];
                string apellido = partes.Length > 1 ? partes[1] : "";

                // Buscar si el cliente ya existe
                clienteAutenticado = repository.ObtenerClientePorDocumento(txtTarjeta.Text.Trim());

                if (clienteAutenticado == null)
                {
                    // Registrar nuevo cliente
                    clienteAutenticado = new Cliente
                    {
                        Documento = txtTarjeta.Text.Trim(),
                        Nombre = nombre,
                        Apellido = apellido,
                        Telefono = "",
                        Email = "",
                        Direccion = "",
                        Ciudad = "",
                        FechaRegistro = DateTime.Now,
                        Estado = true
                    };

                    int idCliente = repository.RegistrarCliente(clienteAutenticado);
                    clienteAutenticado.IdCliente = idCliente;

                    MessageBox.Show($"¡Bienvenido {nombreCompleto}!\n\nTu cuenta ha sido creada exitosamente.",
                        "Registro Exitoso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"¡Hola de nuevo {clienteAutenticado.NombreCompleto}!",
                        "Bienvenido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MostrarError($"Error al procesar el ingreso: {ex.Message}");
            }
        }

        private bool ValidarCampos()
        {
            // Validar nombre
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MostrarError("Por favor ingresa tu nombre completo");
                txtNombre.Focus();
                return false;
            }

            if (txtNombre.Text.Trim().Length < 3)
            {
                MostrarError("El nombre debe tener al menos 3 caracteres");
                txtNombre.Focus();
                return false;
            }

            // Validar edad
            if (string.IsNullOrWhiteSpace(txtEdad.Text))
            {
                MostrarError("Por favor ingresa tu edad");
                txtEdad.Focus();
                return false;
            }

            if (!int.TryParse(txtEdad.Text, out int edad))
            {
                MostrarError("La edad debe ser un número válido");
                txtEdad.Focus();
                return false;
            }

            if (edad < 18)
            {
                MostrarError("Debes ser mayor de 18 años para continuar");
                txtEdad.Focus();
                return false;
            }

            if (edad > 120)
            {
                MostrarError("Por favor ingresa una edad válida");
                txtEdad.Focus();
                return false;
            }

            // Validar tarjeta
            if (string.IsNullOrWhiteSpace(txtTarjeta.Text))
            {
                MostrarError("Por favor ingresa tu número de tarjeta");
                txtTarjeta.Focus();
                return false;
            }

            if (txtTarjeta.Text.Length != 16)
            {
                MostrarError("El número de tarjeta debe tener exactamente 16 dígitos");
                txtTarjeta.Focus();
                return false;
            }

            return true;
        }

        private void MostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            lblError.ForeColor = ColorTranslator.FromHtml("#E50914");
            lblError.Visible = true;
        }

        private string FormatearTarjeta(string tarjeta)
        {
            return $"{tarjeta.Substring(0, 4)} {tarjeta.Substring(4, 4)} {tarjeta.Substring(8, 4)} {tarjeta.Substring(12, 4)}";
        }
    }
}
