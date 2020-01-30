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
using System.IO;

namespace SIB.Modulo
{
    /// <summary>
    /// Interaction logic for Libro.xaml
    /// </summary>
    public partial class CTRLibro : MetroWindow
    {
        Libros instancia = new Libros();
        Llave llave = null;
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

                    if (instancia.Cifrado == null || instancia.Cifrado == false)
                    {
                        btnCifrado.Content = "Cifrar";
                    }
                    else
                    {
                        btnCifrado.Content = "Descifrar";
                    }
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
                    var datos = Herramientas.Herramientas.obtenerDatosDialogo("*.jpg|*.jpg|*.png|*.png|*.bmp|*.bmp", false, "Seleccione una imagen para el libro");
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
                Console.WriteLine("Saliendo del modulo");
                this.Close();
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        private void BtnLllave_Click(object sender, RoutedEventArgs e)
        {
            var ruta = Herramientas.Herramientas.obtenerRutaDialogo("*.json|", false, "Seleccione un archivo JSON");
            if (ruta.Count > 0)
            {
                string directorio = Path.GetDirectoryName(ruta[0]);
                string archivo = Path.GetFileName(ruta[0]);
                llave = LlaveJson.ObtenerLlave(directorio, archivo);
            }
        }

        private async void BtnCifrado_Click(object sender, RoutedEventArgs e)
        {
            
            if (llave != null)
            {
                if (instancia.Cifrado == null || instancia.Cifrado == false)
                {
                    string textoOriginal = instancia.Ubicacion;
                    instancia.Ubicacion = textoOriginal.EncriptarTexto(llave.Valor);
                    txtUbicacion.Text = instancia.Ubicacion;
                    instancia.Cifrado = true;
                    btnCifrado.Content = "Descifrar";
                }
                else
                {
                    string textoCifrado = instancia.Ubicacion;
                    instancia.Ubicacion = textoCifrado.DesencriptarTexto(llave.Valor);
                    txtUbicacion.Text = instancia.Ubicacion;
                    instancia.Cifrado = false;
                    btnCifrado.Content = "Cifrar";
                }
            }
            else
            {
                await this.ShowMessageAsync("Notificacion", "Se requiere buscar o generar una llave de cifrado");
            }
        }
    }
}
