namespace Expoumma_TucanTalk
{
    partial class formProgreso
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbTitulo = new System.Windows.Forms.Label();
            this.guna2ProgressBarXP = new Guna.UI2.WinForms.Guna2CircleProgressBar();
            this.chartRacha = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblXP = new System.Windows.Forms.Label();
            this.lblRacha = new System.Windows.Forms.Label();
            this.lblNivel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRacha)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel1.Controls.Add(this.lbTitulo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(835, 109);
            this.panel1.TabIndex = 0;
            // 
            // lbTitulo
            // 
            this.lbTitulo.AutoSize = true;
            this.lbTitulo.Font = new System.Drawing.Font("Segoe UI Variable Display", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitulo.ForeColor = System.Drawing.Color.White;
            this.lbTitulo.Location = new System.Drawing.Point(35, 31);
            this.lbTitulo.Name = "lbTitulo";
            this.lbTitulo.Size = new System.Drawing.Size(456, 43);
            this.lbTitulo.TabIndex = 3;
            this.lbTitulo.Text = "Tu progreso en Speakup Visual";
            // 
            // guna2ProgressBarXP
            // 
            this.guna2ProgressBarXP.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(213)))), ((int)(((byte)(218)))), ((int)(((byte)(223)))));
            this.guna2ProgressBarXP.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.guna2ProgressBarXP.ForeColor = System.Drawing.Color.White;
            this.guna2ProgressBarXP.Location = new System.Drawing.Point(345, 138);
            this.guna2ProgressBarXP.Minimum = 0;
            this.guna2ProgressBarXP.Name = "guna2ProgressBarXP";
            this.guna2ProgressBarXP.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2ProgressBarXP.Size = new System.Drawing.Size(146, 146);
            this.guna2ProgressBarXP.TabIndex = 1;
            this.guna2ProgressBarXP.Text = "guna2ProgressBar1";
            // 
            // chartRacha
            // 
            chartArea2.Name = "ChartArea1";
            this.chartRacha.ChartAreas.Add(chartArea2);
            this.chartRacha.Dock = System.Windows.Forms.DockStyle.Bottom;
            legend2.Name = "Legend1";
            this.chartRacha.Legends.Add(legend2);
            this.chartRacha.Location = new System.Drawing.Point(0, 320);
            this.chartRacha.Name = "chartRacha";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartRacha.Series.Add(series2);
            this.chartRacha.Size = new System.Drawing.Size(835, 300);
            this.chartRacha.TabIndex = 2;
            this.chartRacha.Text = "chart1";
            // 
            // lblXP
            // 
            this.lblXP.AutoSize = true;
            this.lblXP.Font = new System.Drawing.Font("Segoe UI Variable Display", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXP.ForeColor = System.Drawing.Color.DimGray;
            this.lblXP.Location = new System.Drawing.Point(497, 138);
            this.lblXP.Name = "lblXP";
            this.lblXP.Size = new System.Drawing.Size(36, 30);
            this.lblXP.TabIndex = 4;
            this.lblXP.Text = "xp";
            // 
            // lblRacha
            // 
            this.lblRacha.AutoSize = true;
            this.lblRacha.Font = new System.Drawing.Font("Segoe UI Variable Display", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRacha.ForeColor = System.Drawing.Color.DimGray;
            this.lblRacha.Location = new System.Drawing.Point(497, 184);
            this.lblRacha.Name = "lblRacha";
            this.lblRacha.Size = new System.Drawing.Size(36, 30);
            this.lblRacha.TabIndex = 5;
            this.lblRacha.Text = "xp";
            // 
            // lblNivel
            // 
            this.lblNivel.AutoSize = true;
            this.lblNivel.Font = new System.Drawing.Font("Segoe UI Variable Display", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNivel.ForeColor = System.Drawing.Color.DimGray;
            this.lblNivel.Location = new System.Drawing.Point(497, 232);
            this.lblNivel.Name = "lblNivel";
            this.lblNivel.Size = new System.Drawing.Size(36, 30);
            this.lblNivel.TabIndex = 6;
            this.lblNivel.Text = "xp";
            // 
            // formProgreso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblNivel);
            this.Controls.Add(this.lblRacha);
            this.Controls.Add(this.lblXP);
            this.Controls.Add(this.chartRacha);
            this.Controls.Add(this.guna2ProgressBarXP);
            this.Controls.Add(this.panel1);
            this.Name = "formProgreso";
            this.Size = new System.Drawing.Size(835, 620);
            this.Load += new System.EventHandler(this.formProgreso_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRacha)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbTitulo;
        private Guna.UI2.WinForms.Guna2CircleProgressBar guna2ProgressBarXP;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRacha;
        private System.Windows.Forms.Label lblXP;
        private System.Windows.Forms.Label lblRacha;
        private System.Windows.Forms.Label lblNivel;
    }
}
