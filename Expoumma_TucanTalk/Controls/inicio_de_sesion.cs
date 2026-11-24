using Expoumma_TucanTalk.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Expoumma_TucanTalk
{
    public partial class inicio_de_sesion : UserControl
    {
        public inicio_de_sesion()
        {
            InitializeComponent();
        }

        // 🔹 Evento que se dispara al iniciar sesión correctamente
        public event Action<string> OnLoginSuccess;

        private void AbrirEnPanel(UserControl control)
        {
            panelDockFill.Controls.Clear();
            control.Dock = DockStyle.Fill;
            panelDockFill.Controls.Add(control);
            control.BringToFront();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AbrirEnPanel(new sign_in());
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contraseña = txtContraseña.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contraseña))
            {
                GunaMessageForm.Show("  Ingresa usuario y contraseña.", "Atención",
                    MessageDialogIcon.None, this.FindForm());
                return;
            }

            // 🔹 Generar hash de la contraseña ingresada
            string hashedPassword = HashPassword(contraseña);

            using (var conn = DB.GetConnection())
            {
                string query = "SELECT * FROM usuarios WHERE usuario = @usuario AND contraseña = @contraseña";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@contraseña", hashedPassword);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string usuarioLogueado = reader["usuario"].ToString();

                            // 🔹 Guardar usuario en sesión global
                            Sesion.UsuarioActual = usuarioLogueado;

                            // 🔹 Cargar progreso del usuario desde la BD
                            UserProgress.Cargar();

                            GunaMessageForm.Show(
                                $"  ¡Bienvenido {usuarioLogueado}!",
                                "Inicio de sesión",
                                MessageDialogIcon.None,
                                this.FindForm()
                            );

                            // 🔹 Avisar al formulario principal
                            OnLoginSuccess?.Invoke(usuarioLogueado);
                        }

                        else
                        {
                            GunaMessageForm.Show(
                                "  Usuario o contraseña incorrectos.",
                                "Atención",
                                MessageDialogIcon.None,
                                this.FindForm()
                            );
                        }
                    }
                }
            }
        }

        // 🔹 Método para generar hash SHA256 de la contraseña
        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
