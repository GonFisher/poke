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
    public partial class frmPokemons : Form
    {
        private List<Pokemon> listaPokemon;//Atributo Privado

        public frmPokemons()
        {
            InitializeComponent();
        }

        private void frmPokemons_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Numero");//Campos se refiere a los campos numericos o campos de letras
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripcion");


            
        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvPokemons.CurrentRow != null)
            {
                Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem; //del data greed view de la lista, dame los dators
                cargarImagen(seleccionado.UrlImagen);

            }
        }

        private void cargar()
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                listaPokemon = negocio.listar();
                dgvPokemons.DataSource = listaPokemon;
                ocultarColumnas();
               

                cargarImagen(listaPokemon[0].UrlImagen);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvPokemons.Columns["Id"].Visible = false;
            dgvPokemons.Columns["UrlImagen"].Visible = false;
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                ptbPokemons.Load(imagen);

            }
            catch (Exception ex)
            {
                
                ptbPokemons.Load("https://media.istockphoto.com/id/1147544807/vector/thumbnail-image-vector-graphic.jpg?s=612x612&w=0&k=20&c=rnCKVbdxqkjlcs3xH87-9gocETqpspHFXu5dIGB4wuM=");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaPokemons alta = new frmAltaPokemons();
            alta.ShowDialog();

            cargar();
        }

        private void btnElementos_Click(object sender, EventArgs e)
        {
            frmElementos verElementos = new frmElementos();
            verElementos.ShowDialog();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Pokemon seleccionado;
            seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;

            frmAltaPokemons modificar = new frmAltaPokemons(seleccionado);
            modificar.ShowDialog();
            cargar();
        }
        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            eliminar();
        }


        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }

        private void eliminar(bool logico = false)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            Pokemon seleccionado;
            try
            {

                DialogResult respuesta = MessageBox.Show("¿De Verdad Lo Queres Eliminar?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); //tenemos un texto, un titulo
                //Con el dialogREsult tomamos la respuesta del yes o no de la botonera del show.....

                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;

                    if (logico)
                    {
                        negocio.eliminarLogico(seleccionado.Id);

                    }
                    else
                    {
                        negocio.eliminar(seleccionado.Id);
                    }

                    MessageBox.Show("Eliminado");

                    cargar();
                }
                else
                {
                    return;
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool validarFiltro()
        {
            if(cboCampo.SelectedIndex < 0) //Establece un indice por eso es que lo puedo comparar con un 0
            {
                MessageBox.Show("Llene el Campo Para Filtar");
                return true;
            }
            if (cboCriterio.SelectedIndex <0)
            {
                MessageBox.Show("Llene el Criterio Para Filtar");
                return true;
            }
            if(cboCampo.SelectedItem.ToString() == "Numero")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Debes Completar el Filtro DB con Numeros");
                    return true;
                }
                if(!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Si Seleccionaste Numeros en Campo, Ingresa Solo Numeros en el Filtro DB");
                    return true;
                }

            }

            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter))) 
                {
                    return false;
                }

                return true;
            }
            return true;
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
             PokemonNegocio negocio = new PokemonNegocio();

            try
            {
                if (validarFiltro())
                    return;

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;

                dgvPokemons.DataSource = negocio.filtrar(campo,criterio,filtro);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }

        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
      
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> listafiltrada; //No creo una nueva lista porque ya le asigno un liosta en el proximo paso
            string filtro = txtFiltro.Text;

            if (filtro.Length>=3)
            {
                listafiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Debilidad.Descripcion.ToUpper().Contains(filtro.ToUpper())); //LAMDA //Como es una condicion logica podemos agregar 

            }
            else
            {
                listafiltrada = listaPokemon;
            }


            dgvPokemons.DataSource = null; //limpio el data grid view
            dgvPokemons.DataSource = listafiltrada;
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
             if(opcion == "Numero")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("igual a");


            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene ");
            }
        }
    }
}
