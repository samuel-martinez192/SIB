using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using SIB.EntityFramework;
using SIB.Herramientas;
using System.Data.Entity;

namespace SIB.Modulo
{
    /// <summary>
    /// Interaction logic for Maestros.xaml
    /// </summary>
    public partial class CTRMaestros : MetroWindow
    {
        Maestros instancia = new Maestros();
        private byte[] bytesImagen { get; set; }

        /// <summary>
        /// Constructor sin parametros
        /// </summary>
        public CTRMaestros()
        {
            try
            {
                InitializeComponent();
                InicializarComponentes();
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Constructor que recibe un objeto del tipo Maestros
        /// </summary>
        public CTRMaestros(Maestros instancia)
        {
            try
            {
                InitializeComponent();
                this.instancia = instancia;
                InicializarComponentes();
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Transforma de bytes a una imagen redimensionada y asigna los nuevos bytes a una propiedad
        /// </summary>
        private void cargarImagen(byte[] datos)
        {
            try
            {
                if (datos != null)
                {
                    var temp = datos.BytesAImagen();
                    var imagen = temp.RedimensionarImagen(200, 200);
                    bytesImagen = imagen.ImagenABytes();
                    imagenMaestro.Source = imagen.ImagenABitmap();
                }
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        ///  Asigna un contexto de datos a los controles
        /// </summary>
        private void InicializarComponentes()
        {
            try
            {
                using (var context = new BibliotecaContext())
                {
                    cmbEstatus.ItemsSource = context.Estatus.ToList();
                }

                if (grdMaestros.DataContext != instancia)
                {
                    grdMaestros.DataContext = instancia;
                    cargarImagen(instancia.Imagen);
                }
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Actualiza o guarda una entidad del tipo maestro
        /// </summary>
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new BibliotecaContext())
                {

                    if (instancia.idMaestro == 0)
                    {
                        instancia.Imagen = bytesImagen;
                        instancia.FechaIngreso = DateTime.Now.Date;
                        context.Maestros.Add(instancia);
                    }
                    else
                    {
                        instancia.Imagen = bytesImagen;
                        instancia.FechaModifica = DateTime.Now.Date;
                        context.Maestros.Attach(instancia);
                        context.Entry(instancia).State = EntityState.Modified;
                    }
                    context.SaveChanges();
                    await this.ShowMessageAsync("Exito", "Se guardaron los cambios correctamente");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Abre un dialogo y asigna la imagen al control
        /// </summary>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount == 2)
                {
                    var datos = Herramientas.Herramientas.abrirDialogo("*.jpg|*.jpg|*.png|*.png|*.bmp|*.bmp", false, "Seleccione una imagen para el maestro");
                    if (datos.Count > 0)
                    {
                        cargarImagen(datos[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Cierra la ventana actual
        /// </summary>
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }
    }
}
