using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace winform_app
{
    public partial class frmElementos : Form
    {
         private List<Elemento> listaElementos = new List<Elemento>();
        public frmElementos()
        {
            InitializeComponent();
        }

        private void frmElementos_Load(object sender, EventArgs e)
        {
            ElementoNegocio elemento = new ElementoNegocio();
            listaElementos = elemento.listar();
            dgvElementos.DataSource = listaElementos;
            

        }
    }
}
