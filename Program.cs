using System;
using System.Windows.Forms;
using TiendaVirtual.Data;
using TiendaVirtual.Forms;

namespace TiendaVirtual
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Probar conexión a la base de datos
            string errorConexion;
            if (!DatabaseConnection.TestConnection(out errorConexion))
            {
                MessageBox.Show(
                    $"No se pudo conectar a la base de datos.\n\nError: {errorConexion}\n\n" +
                    "Verifica que:\n" +
                    "1. SQL Server LocalDB esté instalado\n" +
                    "2. La base de datos 'Tienda' exista\n" +
                    "3. Hayas ejecutado los scripts de creación de tablas y datos",
                    "Error de Conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Mostrar formulario de login
            FormLogin formLogin = new FormLogin();

            if (formLogin.ShowDialog() == DialogResult.OK)
            {
                // Si el login fue exitoso, mostrar el catálogo principal
                FormMain formMain = new FormMain(formLogin.ClienteAutenticado);
                Application.Run(formMain);
            }
        }
    }
}
