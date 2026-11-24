using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Expoumma_TucanTalk.Controls
{
    public partial class controlCuenta : UserControl
    {
        private string rutaFoto = null;

        public controlCuenta()
        {
            InitializeComponent();
        }

        private void controlCuenta_Load(object sender, EventArgs e)
        {
            if (!Sesion.EstaLogueado)
            {
                MessageBox.Show("Debes iniciar sesión para acceder a tu cuenta.");
                return;
            }

            CargarDatosUsuario();
        }

        private void CargarDatosUsuario()
        {
            using (var conn = DB.GetConnection())
            {
                string query = "SELECT * FROM usuarios WHERE usuario = @usuario";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", Sesion.UsuarioActual);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblSaludo.Text = $"¡Hola, {Sesion.UsuarioActual}! ";
                            txtUsuario.Text = reader["usuario"].ToString();
                            txtCorreo.Text = reader["correo"].ToString();
                            txtContraseña.Text = reader["contraseña"].ToString();
                            lblNivel.Text = "Nivel: " + reader["nivel"].ToString();
                            lblXP.Text = "XP: " + reader["xp"].ToString();
                            lblRacha.Text = "Racha: " + reader["racha"].ToString();
                            lblFechaRegistro.Text = "Registro: " + Convert.ToDateTime(reader["fecha_registro"]).ToShortDateString();

                            if (reader["foto_perfil"] != DBNull.Value)
                            {
                                string ruta = reader["foto_perfil"].ToString();
                                if (File.Exists(ruta))
                                    picPerfil.Image = Image.FromFile(ruta);
                            }
                        }
                    }
                }
            }
        }

        private void btnCambiarFoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    rutaFoto = ofd.FileName;
                    picPerfil.Image = Image.FromFile(rutaFoto);
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MessageBox.Show("Usuario y correo no pueden estar vacíos.");
                return;
            }

            using (var conn = DB.GetConnection())
            {
                string query = @"UPDATE usuarios 
                                 SET usuario=@usuario, correo=@correo, contraseña=@contraseña, foto_perfil=@foto 
                                 WHERE usuario=@usuarioActual";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text);
                    cmd.Parameters.AddWithValue("@correo", txtCorreo.Text);
                    cmd.Parameters.AddWithValue("@contraseña", txtContraseña.Text);
                    cmd.Parameters.AddWithValue("@foto", (object)rutaFoto ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@usuarioActual", Sesion.UsuarioActual);

                    cmd.ExecuteNonQuery();
                }
            }

            Sesion.UsuarioActual = txtUsuario.Text; // Actualizamos sesión
            MessageBox.Show("Datos actualizados correctamente.");
        }

       
    }
}
