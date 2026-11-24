using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Expoumma_TucanTalk.Controls
{
    public partial class formGrammar : UserControl
    {
        private Dictionary<string, TemaGrammar> temas;
        private TemaGrammar temaActual;

        // Evento para notificar XP al form principal
        public event EventHandler<int> XPCompletado;

        public formGrammar()
        {
            InitializeComponent();
            InicializarTemas();
        }

        private void InicializarTemas()
        {
            temas = new Dictionary<string, TemaGrammar>
            {
                ["UsedTo"] = new TemaGrammar(
                    "Used To",
                    "Make an affirmative, negative and interrogative sentence using 'Used To'.",
                    new[]
                    {
                        "I live in a flat when I was a child.",
                        "We go to the beach every summer?",
                        "She love eating chocolate, but now she hates it.",
                        "He not smoke.",
                        "She be able to speak French, but she has forgotten it all."
                    },
                    new[]
                    {
                        "i used to live in a flat when i was a child",
                        "did we use to go to the beach every summer?",
                        "she used to love eating chocolate, but now she hates it",
                        "he didn't use to smoke",
                        "she used to be able to speak french, but she has forgotten it all"
                    }
                ),
                ["PresentContinuous"] = new TemaGrammar(
                    "Present Continuous",
                    "Complete the statements in present continuous. Use the verbs in the box.",
                    new[]
                    {
                        "Gabriela ______ the house. (clean)",
                        "I ______ breakfast now. (make)",
                        "Sara and Luis _______ the house now. (not/clean)",
                        "My sister ______ the laundry right now. (do)",
                        "Paolo ______ soccer in the stadium. (play)"
                    },
                    new[] { "is cleaning", "am making", "are not cleaning", "is doing", "is playing" }
                ),
                ["PresentSimple"] = new TemaGrammar(
                    "Present Simple",
                    "Complete the following sentences using present simple.",
                    new[]
                    {
                        "You _____ (have) a big house.",
                        "He _____ (catch) the train on Wednesdays.",
                        "The teacher _____ (not/study) Chinese at university.",
                        "Mike and Sara _____ (take) the dog for a walk everyday.",
                        "___ he ____ (watch) TV on Mondays?"
                    },
                    new[] { "have", "catches", "doesn't study", "take", "does watch" }
                ),
                ["AtOnIn"] = new TemaGrammar(
                    "At, On, In",
                    "Fill in the blanks with the correct preposition.",
                    new[]
                    {
                        "I am going to Italy ___ holidays.",
                        "He practices ___ the park.",
                        "The keys are ____ my purse.",
                        "The restaurant is ___ the 47th floor.",
                        "My cousins are _____ Mexico."
                    },
                    new[] { "on", "in", "in", "on", "in" }
                ),
                ["ModalVerbs"] = new TemaGrammar(
                    "Modal Verbs",
                    "Complete with the appropriate modal (May, Can, Should, Must, Could).",
                    new[]
                    {
                        "____ I use your phone, please? (permission)",
                        "She ___ play the guitar very well. (ability)",
                        "It’s cold today. You ____ take a jacket. (advice)",
                        "We ____ visit Paris next summer. (possibility)",
                        "They ____ be at home, the lights are on. (obligation)"
                    },
                    new[] { "may", "can", "should", "could", "must" }
                )
            };
        }

        private void CambiarTema(string claveTema)
        {
            if (!temas.ContainsKey(claveTema)) return;

            temaActual = temas[claveTema];
            etTema.Text = temaActual.Nombre;
            etInstruccion.Text = temaActual.Instruccion;

            etEjercicio1.Text = temaActual.Ejercicios[0];
            etEjercicio2.Text = temaActual.Ejercicios[1];
            etEjercicio3.Text = temaActual.Ejercicios[2];
            etEjercicio4.Text = temaActual.Ejercicios[3];
            etEjercicio5.Text = temaActual.Ejercicios[4];

            LimpiarCampos();
        }

        private void btnVerificar_Click(object sender, EventArgs e)
        {
            if (temaActual == null)
            {
                MessageBox.Show("Please select a topic first.");
                return;
            }

            string[] respuestasUsuario =
            {
                Normalizar(txtEjercicio1.Text),
                Normalizar(txtEjercicio2.Text),
                Normalizar(txtEjercicio3.Text),
                Normalizar(txtEjercicio4.Text),
                Normalizar(txtEjercicio5.Text)
            };

            int correctas = 0;

            for (int i = 0; i < temaActual.Respuestas.Length; i++)
            {
                bool esCorrecta = respuestasUsuario[i] == Normalizar(temaActual.Respuestas[i]);
                MostrarResultado(i + 1, esCorrecta);
                if (esCorrecta) correctas++;
            }

            if (correctas == temaActual.Respuestas.Length)
            {
                MessageBox.Show("Excelente, todas las respuestas son correctas 🎉");

                // 🔹 Sumar XP usando el sistema de UserProgress
                int xpGanada = 10; // XP_GRAMMAR
                UserProgress.TemaCompletado("Grammar");

                // 🔹 Notificar al formJugar para actualizar etiquetas y LevelUp
                XPCompletado?.Invoke(this, xpGanada);
            }
            else
            {
                MessageBox.Show($"Tienes {correctas}/{temaActual.Respuestas.Length} correctas. ¡Sigue practicando!");
            }
        }

        private string Normalizar(string texto)
        {
            return texto.Trim().ToLower().Replace(".", "").Replace(",", "").Replace("'", "");
        }

        private void MostrarResultado(int numero, bool correcto)
        {
            PictureBox picC = Controls.Find($"picCorrecto{numero}", true).FirstOrDefault() as PictureBox;
            PictureBox picI = Controls.Find($"picIncorrecto{numero}", true).FirstOrDefault() as PictureBox;

            if (picC != null && picI != null)
            {
                picC.Visible = correcto;
                picI.Visible = !correcto;
            }
        }

        private void LimpiarCampos()
        {
            foreach (var tb in Controls.OfType<Guna2TextBox>())
                tb.Clear();

            for (int i = 1; i <= 5; i++)
            {
                var c = Controls.Find($"picCorrecto{i}", true).FirstOrDefault() as PictureBox;
                var iC = Controls.Find($"picIncorrecto{i}", true).FirstOrDefault() as PictureBox;

                if (c != null) c.Visible = false;
                if (iC != null) iC.Visible = false;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e) => LimpiarCampos();

        private void rbUsedTo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUsedTo.Checked) CambiarTema("UsedTo");
        }

        private void rbPresentContinuous_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPresentContinuous.Checked) CambiarTema("PresentContinuous");
        }

        private void rbPresentSimple_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPresentSimple.Checked) CambiarTema("PresentSimple");
        }

        private void rbAtonin_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAtonin.Checked) CambiarTema("AtOnIn");
        }

        private void rbModalVerbs_CheckedChanged(object sender, EventArgs e)
        {
            if (rbModalVerbs.Checked) CambiarTema("ModalVerbs");
        }

        public class TemaGrammar
        {
            public string Nombre { get; }
            public string Instruccion { get; }
            public string[] Ejercicios { get; }
            public string[] Respuestas { get; }

            public TemaGrammar(string nombre, string instruccion, string[] ejercicios, string[] respuestas)
            {
                Nombre = nombre;
                Instruccion = instruccion;
                Ejercicios = ejercicios;
                Respuestas = respuestas;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            rbUsedTo.Checked = true; // Selecciona el radio button por defecto
        }
    }
}
