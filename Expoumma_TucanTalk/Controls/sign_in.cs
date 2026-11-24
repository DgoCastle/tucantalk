using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Expoumma_TucanTalk.Controls
{
    public partial class sign_in : UserControl
    {
        public sign_in()
        {
            InitializeComponent();
        }
        private void AbrirEnPanel(UserControl control)
        {
            panelDesktop.Controls.Clear(); // Limpia los controles actuales dentro del panel, para que no se acumulen
            control.Dock = DockStyle.Fill; // Hace que el control ocupe todo el espacio del panel
            panelDesktop.Controls.Add(control);// Agrega el nuevo control al panel
            control.BringToFront(); // Trae el control al frente para asegurarse de que sea visible
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AbrirEnPanel(new inicio_de_sesion());
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string password = txtPassword.Text;
            string confirmar = txtConfirmPassword.Text;

            // Validaciones básicas
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(password))
            {
                GunaMessageForm.Show("  Todos los campos son obligatorios", "Atención", Expoumma_TucanTalk.Controls.MessageDialogIcon.None, this.FindForm());
                return;
            }

            if (password != confirmar)
            {
                GunaMessageForm.Show("  las contraseñas no coinciden", "Atención", Expoumma_TucanTalk.Controls.MessageDialogIcon.None, this.FindForm());
                return;
            }

            try
            {
                using (var conn = DB.GetConnection())
                {
                    string query = "INSERT INTO usuarios (usuario, correo, contraseña) VALUES (@usuario, @correo, @password)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@correo", correo);
                        cmd.Parameters.AddWithValue("@password", HashPassword(password));


                        int resultado = cmd.ExecuteNonQuery();
                        if (resultado > 0)
                        {
                            GunaMessageForm.Show("  Registro exitoso!", "Atención", Expoumma_TucanTalk.Controls.MessageDialogIcon.None, this.FindForm());
                            LimpiarCampos();
                        }
                        else
                        {
                            GunaMessageForm.Show("  No se pudo registrar el usuario :/ intente denuevo", "Atención", Expoumma_TucanTalk.Controls.MessageDialogIcon.None, this.FindForm());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message);
            }
        }

        private void LimpiarCampos()
        {
            txtUsuario.Clear();
            txtCorreo.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

    }
}
