using Expoumma_TucanTalk.Controls;
using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Expoumma_TucanTalk
{
    public partial class formJugar : UserControl
    {
        private PictureBox opcionSeleccionada;
        private string opcionSeleccionadaClave;
        public formJugar()
        {
            InitializeComponent();

            pbGrammar.Click += Opcion_Click;
            pbListening.Click += Opcion_Click;
            pbReading.Click += Opcion_Click;
            pbVocabulary.Click += Opcion_Click;
        }

        private void Opcion_Click(object sender, EventArgs e)
        {
            var pb = (PictureBox)sender;

            // Deseleccionar la anterior
            if (opcionSeleccionada != null)
            {
                opcionSeleccionada.Padding = new Padding(0);
                opcionSeleccionada.BackColor = Color.Transparent;
            }

            // Seleccionar la nueva
            opcionSeleccionada = pb;
            opcionSeleccionada.Padding = new Padding(3);
            opcionSeleccionada.BackColor = Color.DodgerBlue;

            // Guardar la clave de la opción seleccionada
            opcionSeleccionadaClave = pb.Tag.ToString();
        }

        private void AbrirEnPanel(UserControl control)
        {
            panelmid.Controls.Clear();
            control.Dock = DockStyle.Fill;
            panelmid.Controls.Add(control);
            control.BringToFront();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(opcionSeleccionadaClave))
            {
                GunaMessageForm.Show("  Selecciona primero una opción.", "Atención", Expoumma_TucanTalk.Controls.MessageDialogIcon.None, this.FindForm());
                return;
            }

            // Abrir el UserControl según la opción seleccionada
            switch (opcionSeleccionadaClave)
            {
                case "Grammar":
                    var grammarControl = new formGrammar();
                    grammarControl.XPCompletado += GrammarControl_XPCompletado;
                    AbrirEnPanel(grammarControl);
                    break;

                case "Listening":
                    // Abrir el control de Listening
                    var listeningControl = new formListening();
                    AbrirEnPanel(listeningControl);
                    break;

                case "Reading":
                    // Abrir el control de Reading
                    var readingControl = new formReading();
                    AbrirEnPanel(readingControl);
                    break;

                case "Vocabulary":
                    // Abrir el control de Vocabulary
                    var vocabControl = new formVocabulary();
                    AbrirEnPanel(vocabControl);
                    break;
            }
        }

        private void GrammarControl_XPCompletado(object sender, int xp)
        {
            SumarXP(xp);
        }

        private void formJugar_Load(object sender, EventArgs e)
        {
            ActualizarDatosUsuario();
            UserProgress.OnXPChanged += ActualizarDatosUsuario;
        }

        public void ActualizarDatosUsuario()
        {
            // Labels
            lbXP.Text = $"XP: {UserProgress.XP}";
            lbNivel.Text = $"Nivel: {UserProgress.Nivel}";
            lbRacha.Text = $"Racha: {UserProgress.Racha}";

            // Configurar Guna2ProgressBar
            int xpParaNivel = 100; // XP necesario para subir de nivel
            int xpActualNivel = UserProgress.XP % xpParaNivel; // XP relativo al nivel actual

            guna2ProgressBarXP.Maximum = xpParaNivel;
            AnimarProgressBar(guna2ProgressBarXP, xpActualNivel);
        }
        private void AnimarProgressBar(Guna2ProgressBar progressBar, int valorFinal)
        {
            Timer t = new Timer();
            t.Interval = 15; // velocidad de animación
            t.Tick += (s, e) =>
            {
                if (progressBar.Value < valorFinal)
                    progressBar.Value++;
                else
                    t.Stop();
            };
            t.Start();
        }

        public void SumarXP(int cantidad)
        {
            int nivelAnterior = UserProgress.Nivel;

            UserProgress.AgregarXP(cantidad);
            ActualizarDatosUsuario();

            if (UserProgress.Nivel > nivelAnterior)
            {
                lbLevelUp.Visible = true;
                Timer t = new Timer();
                t.Interval = 1500;
                t.Tick += (s, e) =>
                {
                    lbLevelUp.Visible = false;
                    t.Stop();
                    t.Dispose();
                };
                t.Start();
            }
        }
    }
}
