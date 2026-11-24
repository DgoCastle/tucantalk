using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Expoumma_TucanTalk
{
    public partial class formProgreso : UserControl
    {
        public formProgreso()
        {
            InitializeComponent();
        }

        private void formProgreso_Load(object sender, EventArgs e)
        {
            // Carga los datos al abrir el form
            CargarDatos();
        }

        private void CargarDatos()
        {
            // Muestra los datos reales del progreso guardado
            lblXP.Text = $"XP total: {UserProgress.XP}";
            lblNivel.Text = $"Nivel: {UserProgress.Nivel}";
            lblRacha.Text = $"Racha: {UserProgress.Racha} días";

            // Configurar barra de progreso XP
            int xpPorNivel = 100;
            int xpActualNivel = UserProgress.XP % xpPorNivel;

            guna2ProgressBarXP.Maximum = xpPorNivel;
            guna2ProgressBarXP.Value = Math.Min(xpActualNivel, xpPorNivel);

            // Actualizar el gráfico
            ConfigurarGraficoRacha();
        }

        private void ConfigurarGraficoRacha()
        {
            chartRacha.Series.Clear();
            chartRacha.ChartAreas.Clear();

            ChartArea area = new ChartArea();
            area.AxisX.Title = "Últimos días";
            area.AxisY.Title = "Actividad (%)";
            area.AxisX.Interval = 1;
            area.AxisY.Maximum = 100;
            chartRacha.ChartAreas.Add(area);

            Series serie = new Series("Racha")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = System.Drawing.Color.RoyalBlue,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8
            };

            chartRacha.Series.Add(serie);

            // Simulación de los últimos 7 días (puedes hacerlo dinámico más adelante)
            Random rnd = new Random();
            for (int i = 6; i >= 0; i--)
            {
                string dia = DateTime.Today.AddDays(-i).ToString("dd/MM");
                int actividad = (i == 0) ? 100 : rnd.Next(20, 100); // hoy 100%, otros aleatorios
                serie.Points.AddXY(dia, actividad);
            }
        }
    }
}
