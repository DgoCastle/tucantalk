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
    public partial class controlCreditos : UserControl
    {
        public controlCreditos()
        {
            InitializeComponent();
            Label label = new Label();
            label.Text =
     "Aplicación: Speakup Visual - Aprende Inglés\n\n" +
     "Descripción:\n" +
     "Speakup Visual es una aplicación educativa creada en C# con Windows Forms, diseñada para ayudar " +
     "a los alumnos de preparatoria a mejorar su vocabulario y comprensión del idioma inglés mediante ejercicios " +
     "interactivos y prácticas dinámicas.\n\n" +
     "Equipo de desarrollo:\n" +
     " - Diego Fernando Castillo Hernández\n" +
     " - Héctor Gabriel Egremy Ovando\n" +
     " - Sarah Cristina Pérez Hidalgo\n" +
     " - Julián Alberto Suárez Urrea\n" +
     " - Paulina Martinez León\n\n" +
     "Versión: 0.5.0\n" +
     "Fecha de desarrollo: Octubre 2025\n\n" +
     "Agradecimientos:\n" +
     "A nuestro profesor por su apoyo y orientación durante el desarrollo del proyecto.";
            label.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            label.Dock = DockStyle.Fill;
            label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Controls.Add(label);
        }
    }
}
