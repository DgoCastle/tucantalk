using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using NAudio.Wave;

namespace Expoumma_TucanTalk.Controls
{
    public partial class formVocabulary : UserControl
    {
        // 🔹 Evento para avisar a formJugar que se ganó XP
        public event EventHandler<int> XPCompletado;

        private WaveOutEvent outputAudio;
        private AudioFileReader audioReader;

        private void ReproducirAudio(string ruta)
        {
            try
            {
                outputAudio?.Stop();
                outputAudio?.Dispose();
                audioReader?.Dispose();

                audioReader = new AudioFileReader(ruta);
                outputAudio = new WaveOutEvent();
                outputAudio.Init(audioReader);
                outputAudio.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de audio: " + ex.Message);
            }
        }

        private Dictionary<string, string> palabras = new Dictionary<string, string>()
        {
            {"alex", "Teacher→tiːʧə"},
            {"cuantante", "Singer→sɪŋə"},
            {"dedpul", "Actor→æktə"},
            {"doctor", "Doctor→dɒktə"},
            {"gastro", "Chef→ʃɛf"},
            {"inge", "Engineer→ɛnʤɪˈnɪə"},
            {"juez", "Judge→ʤʌʤ"},
            {"mentirososno1", "Politician→pɒlɪˈtɪʃᵊn"},
            {"mesero", "Waiter→weɪtə"},
        };

        private int cartasVolteadas = 0;
        private bool terminado = false;

        private class FlashCard
        {
            public Panel Panel;
            public PictureBox Picture;
            public Label Label;
            public bool Flipped = false;
        }

        private List<FlashCard> cards = new List<FlashCard>();
        private TableLayoutPanel table;

        public formVocabulary()
        {
            InitializeComponent();
            CrearTablero();
        }

        private void CrearTablero()
        {
            table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 3,
                BackColor = Color.White,
                Padding = new Padding(20),
            };

            for (int i = 0; i < 3; i++)
            {
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            }

            Controls.Add(table);

            foreach (var kv in palabras)
            {
                var flash = CrearFlashCard(kv.Key, kv.Value);
                cards.Add(flash);
                table.Controls.Add(flash.Panel);
            }
        }

        private FlashCard CrearFlashCard(string nombreArchivo, string palabra)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
                BackColor = Color.FromArgb(245, 247, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            var pic = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = CargarImagenDesdeRecursos(nombreArchivo)
            };

            var lbl = new Label
            {
                Text = palabra,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.DodgerBlue,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };

            panel.Controls.Add(pic);
            panel.Controls.Add(lbl);

            var flash = new FlashCard
            {
                Panel = panel,
                Picture = pic,
                Label = lbl
            };

            panel.Click += (s, e) => FlipCard(flash);
            pic.Click += (s, e) => FlipCard(flash);
            lbl.Click += (s, e) => FlipCard(flash);

            return flash;
        }

        private Image CargarImagenDesdeRecursos(string nombreArchivo)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = $"Expoumma_TucanTalk.resources.{nombreArchivo}.jpeg";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                    return Image.FromStream(stream);
                else
                {
                    MessageBox.Show($"No se encontró la imagen {nombreArchivo}.jpeg", "Error");
                    return null;
                }
            }
        }

        private void FlipCard(FlashCard card)
        {
            if (terminado) return;
            if (card.Flipped) return;

            card.Flipped = true;
            card.Picture.Visible = false;
            card.Label.Visible = true;
            cartasVolteadas++;

            // Color al voltearse
            card.Panel.BackColor = Color.FromArgb(230, 247, 255);

            // ---------- AUDIO ----------
            string palabraIngles = card.Label.Text.Split('→')[0]; // "Teacher"
            string nombreAudio = palabraIngles.ToLower();         // "teacher"
            string rutaAudio = Path.Combine("resources", "Words", nombreAudio + ".mp3");

            if (File.Exists(rutaAudio))
                ReproducirAudio(rutaAudio);
            else
                MessageBox.Show("No existe el audio: " + rutaAudio);
            // ----------------------------

            if (cartasVolteadas == palabras.Count)
                FinalizarJuego();
        }

        private void FinalizarJuego()
        {
            terminado = true;

            int xpGanada = 25;
            UserProgress.TemaCompletado("Vocabulario");
            XPCompletado?.Invoke(this, xpGanada);

            MessageBox.Show($"🎉 ¡Completaste todas las fichas!\nHas ganado {xpGanada} XP.",
                            "¡Buen trabajo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

