using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Behaviours;
using SIB.EntityFramework;
using SIB.Herramientas;
using System.Data.Entity;

namespace SIB.Modulo
{
    /// <summary>
    /// Interaction logic for Prestamo.xaml
    /// </summary>
    public partial class CTRPrestamo : MetroWindow
    {
        Prestamos instancia = new Prestamos();
        private byte[] bytesImagen { get; set; }

        /// <summary>
        /// Constructor sin parametros
        /// </summary>
        public CTRPrestamo()
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
        /// Constructor que recibe un objeto del tipo Prestamos
        /// </summary>
        public CTRPrestamo(Prestamos instancia)
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
                    imagenPrestamo.Source = imagen.ImagenABitmap();
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
                    cmbLibro.ItemsSource = context.Libros.ToList();
                    cmbUsuario.ItemsSource = context.Usuarios.ToList();
                    cmbEstatus.ItemsSource = context.Estatus.ToList();
                }

                if (grdPrestamo.DataContext != instancia)
                {
                    grdPrestamo.DataContext = instancia;
                    cargarImagen(instancia.Imagen);
                }
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Actualiza la cantidad de libros disponibles cuando se dan altas o bajas de prestamos
        /// </summary>
        private async void actualizarLibro(BibliotecaContext context)
        {
            try
            {
                var libro = context.Libros.Find(instancia.idLibro);

                if (libro != null)
                {
                    if (instancia.idEstatus == true)
                    {
                        libro.Cantidad = libro.Cantidad - 1;
                    }
                    else
                    {
                        libro.Cantidad = libro.Cantidad + 1;
                    }

                    context.SaveChanges();
                }
                else
                {
                    await this.ShowMessageAsync("Error", "No se actualizo la cantidad de libros disponibles");
                }
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Actualiza o guarda a una entidad del tipo prestamo
        /// </summary>
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new BibliotecaContext())
                {

                    if (instancia.idPrestamo == 0)
                    {
                        instancia.Imagen = bytesImagen;
                        instancia.FechaIngreso = DateTime.Now.Date;
                        context.Prestamos.Add(instancia);
                    }
                    else
                    {
                        instancia.Imagen = bytesImagen;
                        instancia.FechaModifica = DateTime.Now.Date;
                        context.Prestamos.Attach(instancia);
                        context.Entry(instancia).State = EntityState.Modified;
                    }
                    context.SaveChanges();
                    actualizarLibro(context);
                }

                await this.ShowMessageAsync("Exito", "Se guardaron los cambios correctamente");
                this.Close();
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
        /// Genera reporte de prestamos
        /// </summary>
        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new BibliotecaContext())
                {
                    CTRReporte reporte = new CTRReporte();
                    var data = context.GetPrestamo(instancia.idUsuario).ToList();
                    reporte.crearReporte("DataSet1", "SIB.Reportes.ReportePrestamos.rdlc", data);
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
