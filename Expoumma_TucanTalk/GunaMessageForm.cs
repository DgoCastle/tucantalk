using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Expoumma_TucanTalk.Controls
{
    public partial class GunaMessageForm : Form
    {
        private Guna2BorderlessForm borderless;
        private Guna2HtmlLabel lblMensaje;
        private Guna2Button btnAceptar;

        public GunaMessageForm(string mensaje, string titulo = "Aviso", MessageDialogIcon icon = MessageDialogIcon.Information)
        {
            

            // Configuración del Form
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Size = new Size(350, 180);

            // Borderless y arrastrable
            borderless = new Guna2BorderlessForm();
            borderless.ContainerControl = this;
            borderless.DockIndicatorTransparencyValue = 0.6;
            borderless.TransparentWhileDrag = true;
            borderless.BorderRadius = 12;

            // Label
            lblMensaje = new Guna2HtmlLabel();
            lblMensaje.Text = mensaje;
            lblMensaje.ForeColor = Color.Black;
            lblMensaje.Font = new Font("Segoe UI", 16, FontStyle.Regular);
            lblMensaje.Location = new Point(20, 40);
            lblMensaje.Size = new Size(this.Width - 40, 60);
            lblMensaje.TextAlignment = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblMensaje);

            // Botón aceptar
            btnAceptar = new Guna2Button();
            btnAceptar.Text = "Aceptar";
            btnAceptar.Size = new Size(100, 35);
            btnAceptar.Location = new Point((this.Width - btnAceptar.Width) / 2, this.Height - 60);
            btnAceptar.Click += (s, e) => this.Close();
            btnAceptar.FillColor = Color.FromArgb(98, 102, 244);
            btnAceptar.ForeColor = Color.White;
            btnAceptar.BorderRadius = 8;
            this.Controls.Add(btnAceptar);

            // Icono opcional
            if (icon != MessageDialogIcon.None)
            {
                var picIcon = new PictureBox();
                picIcon.Size = new Size(32, 32);
                picIcon.Location = new Point((this.Width - 32) / 2, 10);
                picIcon.SizeMode = PictureBoxSizeMode.StretchImage;

                switch (icon)
                {
                    case MessageDialogIcon.Information:
                        picIcon.Image = SystemIcons.Information.ToBitmap();
                        break;
                    case MessageDialogIcon.Warning:
                        picIcon.Image = SystemIcons.Warning.ToBitmap();
                        break;
                    case MessageDialogIcon.Error:
                        picIcon.Image = SystemIcons.Error.ToBitmap();
                        break;
                    case MessageDialogIcon.Question:
                        picIcon.Image = SystemIcons.Question.ToBitmap();
                        break;
                }

                this.Controls.Add(picIcon);
            }
        }

        // Método estático para mostrar de forma rápida
        public static void Show(string mensaje, string titulo = "Aviso", MessageDialogIcon icon = MessageDialogIcon.Information, Form owner = null)
        {
            using (var form = new GunaMessageForm(mensaje, titulo, icon))
            {
                if (owner != null)
                    form.ShowDialog(owner);
                else
                    form.ShowDialog();
            }
        }
    }

    public enum MessageDialogIcon
    {
        None,
        Information,
        Warning,
        Error,
        Question
    }
}
