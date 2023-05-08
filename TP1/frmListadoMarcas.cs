﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Modelo;
using Controlador;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;

namespace TP1
{
    public partial class frmListadoMarcas : Form
    {
        private List<Marca> marcas;
        private List<Marca> lista;

        private List<Articulo> listaArticulos;


        private void cargarLista()
        {
            MarcaNegocio negocio = new MarcaNegocio();
            marcas = negocio.listarMarcas();
        }

        private void cargarListaArticulos()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulos = negocio.listar(getMarcaSeleccionada());
                dataGridArticulosPorCategoria.DataSource = listaArticulos;
                dataGridArticulosPorCategoria.Columns["Id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            labelListadoArt.Text = "Listado de Artículos con Marca " + getMarcaSeleccionada().Nombre;
        }

        private Marca getMarcaSeleccionada()
        {
            return (Marca)listaMarcas.CurrentRow.DataBoundItem;
        }
        public frmListadoMarcas()
        {
            InitializeComponent();
            this.Controls.Add(listaMarcas);
            this.Load += Form4_Load;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            this.AutoSize = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            cargarLista();
            lista = marcas;
            listaMarcas.DataSource = lista;
            
        }

        
        private void onTextChange(object sender, EventArgs e)
        {
            lista = marcas.FindAll(x => x.Nombre.ToUpper().Contains(textBoxFiltro.Text.ToUpper()));
            listaMarcas.DataSource = lista;

        }

        private void btnAgregarMarca_Click(object sender, EventArgs e)
        {
            frmDialogAgregarMarca form = new frmDialogAgregarMarca();
            form.Owner = this;
            form.ShowDialog();
        }

        private void btnModificarMarca_Click(object sender, EventArgs e)
        {
            frmDialogEditarMarca form = new frmDialogEditarMarca(getMarcaSeleccionada());
            form.Owner= this;
            form.ShowDialog();
        }

        public void reload()
        {
            cargarLista();
            if(textBoxFiltro.Text != "")
            {
                lista = marcas.FindAll(x => x.Nombre.ToUpper().Contains(textBoxFiltro.Text.ToUpper()));
                listaMarcas.DataSource = lista;
            }
            else
            {
                listaMarcas.DataSource = marcas;
            }

        }

        private void btnEliminarMarca_Click(object sender, EventArgs e)
        {
            frmDialogEliminarMarca form = new frmDialogEliminarMarca(getMarcaSeleccionada());
            form.Owner= this;
            form.ShowDialog();
        }

        private void listaMarcas_SelectionChanged(object sender, EventArgs e)
        {
            cargarListaArticulos(); 
        }

        private void btnRestablecer_Click(object sender, EventArgs e)
        {
            textBoxFiltro.Text = "";
            textBoxFiltro.Focus();
        }
    }
}
