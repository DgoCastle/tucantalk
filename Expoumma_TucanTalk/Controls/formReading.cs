using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Expoumma_TucanTalk.Controls
{
    public partial class formReading : UserControl
    {
        private Dictionary<string, TemaReading> temas;
        private TemaReading temaActual;

        public event EventHandler<int> XPCompletado;

        public formReading()
        {
            InitializeComponent();
            InicializarTemas();
        }

        private void InicializarTemas()
        {
            temas = new Dictionary<string, TemaReading>
            {
                ["EmmaTokyo"] = new TemaReading(
                    "SITUATION 1 (Emma in Tokyo)",
                    "Emma was sent to Tokyo by her company. She had never been to Asia before, and at first she experienced culture shock — everything was new and different. However, she soon learned to appreciate how organized and friendly people were. By the time she returned home, she had learned Japanese and improved her time management skills. It was one of the most positive experiences of her life.",
                    new[]
                    {
                        "Emma decided to go to Tokyo because...",
                        "At first, Emma found it difficult because of...",
                        "What surprised her the most?",
                        "What did she learn by the end of her stay?",
                        "Overall, Emma’s experience was..."
                    },
                    new[]
                    {
                        "she was sent there by her company",
                        "the culture shock",
                        "how friendly and organized people were",
                        "japanese and time management",
                        "positive"
                    }
                ),

                ["DanielChange"] = new TemaReading(
                    "SITUATION 2 (Daniel’s healthy change)",
                    "Daniel used to eat junk food every day until one morning he felt extremely tired and realized he was unhealthy. He decided to start exercising and eating better. At first, avoiding fast food was very hard for him, but after three months he felt full of energy and happier. The change in his habits completely transformed his life.",
                    new[]
                    {
                        "Daniel changed his habits because...",
                        "What was difficult for Daniel at first?",
                        "How did he feel after three months?",
                        "What does the text suggest?",
                        "The main idea of the text is about..."
                    },
                    new[]
                    {
                        "he felt unhealthy",
                        "avoiding fast food",
                        "more energetic",
                        "healthy food made him happier",
                        "changing bad habits"
                    }
                ),

                ["MayaSpeaking"] = new TemaReading(
                    "SITUATION 3 (Maya and public speaking)",
                    "Maya used to be terrified of speaking in public. When her professor asked her to present her project, she almost refused. Her professor encouraged her to practice a little every day and to believe in herself. After several days of preparation, she finally stood in front of the class and spoke confidently. From that day on, she knew she could overcome her fear.",
                    new[]
                    {
                        "Maya almost refused to present because...",
                        "Who helped her?",
                        "How did she prepare?",
                        "What happened during her presentation?",
                        "What did Maya learn?"
                    },
                    new[]
                    {
                        "she was afraid",
                        "her professor",
                        "by practicing every day",
                        "she spoke with confidence",
                        "she could overcome fear"
                    }
                )
            };
        }

        private void CambiarTema(string clave)
        {
            if (!temas.ContainsKey(clave)) return;

            temaActual = temas[clave];

            etTema.Text = temaActual.Nombre;
            guna2TextBox1.Text = temaActual.Texto;

            for (int i = 0; i < 5; i++)
            {
                var lbl = Controls.Find($"etPregunta{i + 1}", true).FirstOrDefault() as Label;
                if (lbl != null)
                    lbl.Text = temaActual.Preguntas[i];

                var txt = Controls.Find($"txtEjercicio{i + 1}", true).FirstOrDefault() as Guna2TextBox;
                if (txt != null)
                    txt.Text = "";

                OcultarIconos(i + 1);
            }
        }

        private void btnVerificar_Click(object sender, EventArgs e)
        {
            if (temaActual == null)
            {
                MessageBox.Show("Please select a situation first.");
                return;
            }

            int correctas = 0;

            for (int i = 0; i < temaActual.Preguntas.Length; i++)
            {
                var txt = Controls.Find($"txtEjercicio{i + 1}", true).FirstOrDefault() as Guna2TextBox;
                if (txt == null) continue;

                string respuestaUsuario = Normalizar(txt.Text);
                string respuestaCorrecta = Normalizar(temaActual.Respuestas[i]);

                bool esCorrecta = respuestaUsuario == respuestaCorrecta;
                MostrarResultado(i + 1, esCorrecta);

                if (esCorrecta) correctas++;
            }

            if (correctas == 5)
            {
                MessageBox.Show("Excellent! All answers are correct 🎉");
                int xpGanada = 10;
                UserProgress.TemaCompletado("Reading");
                XPCompletado?.Invoke(this, xpGanada);
            }
            else
            {
                MessageBox.Show($"You got {correctas}/5 correct. Keep practicing!");
            }
        }

        private void MostrarResultado(int numero, bool correcto)
        {
            var picC = Controls.Find($"picCorrecto{numero}", true).FirstOrDefault() as PictureBox;
            var picI = Controls.Find($"picIncorrecto{numero}", true).FirstOrDefault() as PictureBox;

            if (picC != null && picI != null)
            {
                picC.Visible = correcto;
                picI.Visible = !correcto;
            }
        }

        private void OcultarIconos(int numero)
        {
            var picC = Controls.Find($"picCorrecto{numero}", true).FirstOrDefault() as PictureBox;
            var picI = Controls.Find($"picIncorrecto{numero}", true).FirstOrDefault() as PictureBox;

            if (picC != null) picC.Visible = false;
            if (picI != null) picI.Visible = false;
        }

        private string Normalizar(string texto)
        {
            return texto.Trim().ToLower()
                .Replace(".", "")
                .Replace(",", "")
                .Replace("’", "'")
                .Replace("‘", "'")
                .Replace("´", "'")
                .Replace("“", "\"")
                .Replace("”", "\"");
        }

        private void rbEmma_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEmma.Checked) CambiarTema("EmmaTokyo");
        }

        private void rbDaniel_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDaniel.Checked) CambiarTema("DanielChange");
        }

        private void rbMaya_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMaya.Checked) CambiarTema("MayaSpeaking");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            rbEmma.Checked = true;
        }

        // ======== CLASES INTERNAS ========
        public class TemaReading
        {
            public string Nombre { get; }
            public string Texto { get; }
            public string[] Preguntas { get; }
            public string[] Respuestas { get; }

            public TemaReading(string nombre, string texto, string[] preguntas, string[] respuestas)
            {
                Nombre = nombre;
                Texto = texto;
                Preguntas = preguntas;
                Respuestas = respuestas;
            }
        }
    }
}
