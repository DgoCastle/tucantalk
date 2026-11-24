using Expoumma_TucanTalk.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Expoumma_TucanTalk
{
    public partial class InfoControl : UserControl // el user control es importante para identificar 
    {
        public InfoControl()
        {
            InitializeComponent();
        }
        private void AbrirEnPanel(UserControl control)
        {
            InfoPanel.Controls.Clear(); // Limpia los controles actuales dentro del panel, para que no se acumulen
            control.Dock = DockStyle.Fill; // Hace que el control ocupe todo el espacio del panel
            InfoPanel.Controls.Add(control);// Agrega el nuevo control al panel
            control.BringToFront(); // Trae el control al frente para asegurarse de que sea visible
        }
        private void btnCreditos_Click(object sender, EventArgs e)
        {
            AbrirEnPanel(new controlCreditos());
        }
    }
}
