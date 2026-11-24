using Guna.UI2.WinForms;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Expoumma_TucanTalk.Controls
{
    public partial class formListening : UserControl
    {
        private Dictionary<string, TemaListening> temas;
        private TemaListening temaActual;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        public event EventHandler<int> XPCompletado;

        public formListening()
        {
            InitializeComponent();
            InicializarTemas();
        }

        // ====== INICIALIZACIÓN ======
        private void InicializarTemas()
        {
            string audioPath = Path.Combine(Application.StartupPath, "resources", "Listening", "speech.mp3");

            temas = new Dictionary<string, TemaListening>
            {
                ["SarahTrip"] = new TemaListening(
                    "B2 Listening Practice: Sarah’s Trip",
                    "Listen carefully to the story and answer the following questions.",
                    new[]
                    {
                        new Pregunta(
                            "1. Why did Sarah cancel her trip to Paris?",
                            new[] {
                                "Her flight was too expensive.",
                                "Her boss sent her to a conference in New York.",
                                "She got sick before traveling."
                            },
                            1
                        ),
                        new Pregunta(
                            "2. What happened when she arrived in New York?",
                            new[] {
                                "Her luggage was lost and her file didn’t open.",
                                "Everything went perfectly.",
                                "She decided to return home immediately."
                            },
                            0
                        ),
                        new Pregunta(
                            "3. What did Sarah realize by the end of her trip?",
                            new[] {
                                "That the experience helped her career and she made new friends.",
                                "That she never wanted to travel again.",
                                "That she should have gone to Paris anyway."
                            },
                            0
                        )
                    },
                    audioPath
                )
            };

            temaActual = temas["SarahTrip"];
            CargarTema();
        }

        private void CargarTema()
        {
            if (temaActual == null) return;

            etTema.Text = temaActual.Nombre;
            etInstruccion.Text = temaActual.Instruccion;

            for (int i = 0; i < temaActual.Preguntas.Count; i++)
            {
                var p = temaActual.Preguntas[i];
                var lbl = Controls.Find($"etEjercicio{i + 1}", true).FirstOrDefault() as Label;
                if (lbl != null) lbl.Text = p.Texto;

                for (int j = 0; j < 3; j++)
                {
                    var rb = Controls.Find($"rb{i + 1}{(char)('a' + j)}", true).FirstOrDefault() as RadioButton;
                    if (rb != null) rb.Text = p.Opciones[j];
                }

                OcultarIconos(i + 1);
            }
        }
        private bool isPaused = false;

        private void btnPlay_Click(object sender, EventArgs e)
        {
            try
            {
                string audioPath = temaActual.AudioPath;

                if (!File.Exists(audioPath))
                {
                    MessageBox.Show("Audio file not found.\nExpected path:\n" + audioPath,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Si está reproduciendo, pausamos
                if (outputDevice != null && outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    outputDevice.Pause();
                    isPaused = true;
                    btnPlay.Text = "▶ Resume";
                    return;
                }

                // Si está pausado, reanudamos
                if (isPaused && outputDevice != null)
                {
                    outputDevice.Play();
                    isPaused = false;
                    btnPlay.Text = "⏸ Pause";
                    return;
                }

                // Si llega aquí, es que no hay audio reproduciéndose actualmente
                outputDevice?.Stop();
                outputDevice?.Dispose();
                audioFile?.Dispose();

                audioFile = new AudioFileReader(audioPath);
                outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
                outputDevice.Play();
                btnPlay.Text = "⏸ Pause";

                outputDevice.PlaybackStopped += (s, a) =>
                {
                    audioFile.Dispose();
                    outputDevice.Dispose();
                    outputDevice = null;
                    audioFile = null;
                    btnPlay.Text = "▶ Play";
                    isPaused = false;
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing audio: " + ex.Message,
                                "Audio Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====== VERIFICAR RESPUESTAS ======
        private void btnVerificar_Click(object sender, EventArgs e)
        {
            int correctas = 0;

            for (int i = 0; i < temaActual.Preguntas.Count; i++)
            {
                int seleccion = ObtenerSeleccion(i + 1);
                bool esCorrecta = seleccion == temaActual.Preguntas[i].IndiceCorrecto;

                MostrarResultado(i + 1, esCorrecta);
                if (esCorrecta) correctas++;
            }

            if (correctas == temaActual.Preguntas.Count)
            {
                MessageBox.Show("Excellent! All answers are correct 🎉");
                XPCompletado?.Invoke(this, 10);
                UserProgress.TemaCompletado("Listening");
            }
            else
            {
                MessageBox.Show($"You got {correctas}/{temaActual.Preguntas.Count} correct. Keep practicing!");
            }
        }

        // ====== OBTENER RESPUESTA SELECCIONADA ======
        private int ObtenerSeleccion(int numero)
        {
            for (int i = 0; i < 3; i++)
            {
                var rb = Controls.Find($"rb{numero}{(char)('a' + i)}", true).FirstOrDefault() as RadioButton;
                if (rb != null && rb.Checked) return i;
            }
            return -1;
        }

        // ====== ÍCONOS DE RESULTADO ======
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

        // ====== CLASES AUXILIARES ======
        public class TemaListening
        {
            public string Nombre { get; }
            public string Instruccion { get; }
            public List<Pregunta> Preguntas { get; }
            public string AudioPath { get; }

            public TemaListening(string nombre, string instruccion, Pregunta[] preguntas, string audioPath)
            {
                Nombre = nombre;
                Instruccion = instruccion;
                Preguntas = preguntas.ToList();
                AudioPath = audioPath;
            }
        }

        public class Pregunta
        {
            public string Texto { get; }
            public string[] Opciones { get; }
            public int IndiceCorrecto { get; }

            public Pregunta(string texto, string[] opciones, int indiceCorrecto)
            {
                Texto = texto;
                Opciones = opciones;
                IndiceCorrecto = indiceCorrecto;
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            outputDevice?.Dispose();
            audioFile?.Dispose();
        }
    }
}
