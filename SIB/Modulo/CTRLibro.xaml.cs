using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Behaviours;
using SIB.Herramientas;
using SIB.EntityFramework;
using System.Data.Entity;

namespace SIB.Modulo
{
    /// <summary>
    /// Interaction logic for Libro.xaml
    /// </summary>
    public partial class CTRLibro : MetroWindow
    {
        Libros instancia = new Libros();
        private byte[] bytesImagen { get; set; }
        /// <summary>
        /// Constructor sin parametros
        /// </summary>
        public CTRLibro()
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
        /// Constructor que recibe un objeto del tipo Libros
        /// </summary>
        public CTRLibro(Libros instancia)
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
                    imagenLibro.Source = imagen.ImagenABitmap();
                }
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Asigna un contexto de datos a los controles
        /// </summary>
        private void InicializarComponentes()
        {
            try
            {
                using (var context = new BibliotecaContext())
                {
                    cmbEditorial.ItemsSource = context.Editorial.ToList();
                    cmbAutor.ItemsSource = context.Autores.ToList();
                    cmbEstatus.ItemsSource = context.Estatus.ToList();
                }

                if (grdLibros.DataContext != instancia)
                {
                    grdLibros.DataContext = instancia;
                    cargarImagen(instancia.Imagen);
                }
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Guarda o actualiza una entidad del tipo Libro
        /// </summary>
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new BibliotecaContext())
                {

                    if (instancia.idLibro == 0)
                    {
                        instancia.Imagen = bytesImagen;
                        instancia.FechaIngreso = DateTime.Now.Date;
                        context.Libros.Add(instancia);
                    }
                    else
                    {
                        instancia.Imagen = bytesImagen;
                        instancia.FechaModifica = DateTime.Now.Date;
                        context.Libros.Attach(instancia);
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
                    var datos = Herramientas.Herramientas.abrirDialogo("*.jpg|*.jpg|*.png|*.png|*.bmp|*.bmp", false, "Seleccionar imagen del libro");
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
        /// Genera reporte de libros
        /// </summary>
        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new BibliotecaContext())
                {
                    CTRReporte reporte = new CTRReporte();
                    var data = context.GetLibro(instancia.idLibro).ToList();
                    reporte.crearReporte("DataSet1", "SIB.Reportes.ReporteLibro.rdlc", data);
                    reporte.ShowDialog();
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
