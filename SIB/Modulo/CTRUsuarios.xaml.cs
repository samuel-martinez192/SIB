using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Behaviours;
using SIB.Herramientas;
using SIB.EntityFramework;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace SIB.Modulo
{
    /// <summary>
    /// Interaction logic for Usuarios.xaml
    /// </summary>
    public partial class CTRUsuarios : MetroWindow
    {
        Usuarios instancia = new Usuarios();
        private byte[] bytesImagen { get; set; }
        /// <summary>
        /// Constructor sin parametros
        /// </summary>
        public CTRUsuarios()
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
        /// Constructor que recibe un objeto del tipo Usuarios
        /// </summary>
        public CTRUsuarios(Usuarios instancia)
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
                    imagenUsuario.Source = imagen.ImagenABitmap();
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
                    cmbAlumno.ItemsSource = context.Alumnos.ToList();
                    cmbMaestro.ItemsSource = context.Maestros.ToList();
                    cmbPermisos.ItemsSource = context.Permisos.ToList();
                    cmbEstatus.ItemsSource = context.Estatus.ToList();
                }

                if (grdUsuarios.DataContext != instancia)
                {
                    grdUsuarios.DataContext = instancia;
                    pbContraseña.Password = instancia.Contraseña;
                    cargarImagen(instancia.Imagen);
                }
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Actualiza o guarda una entidad del tipo usuario
        /// </summary>
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new BibliotecaContext())
                {

                    if (instancia.idUsuario == 0)
                    {
                        instancia.Imagen = bytesImagen;
                        instancia.Contraseña = pbContraseña.Password;
                        instancia.FechaIngreso = DateTime.Now.Date;
                        context.Usuarios.Add(instancia);
                    }
                    else
                    {
                        instancia.Imagen = bytesImagen;
                        instancia.Contraseña = pbContraseña.Password;
                        instancia.FechaModifica = DateTime.Now.Date;
                        context.Usuarios.Attach(instancia);
                        context.Entry(instancia).State = EntityState.Modified;
                    }
                    context.SaveChanges();
                    await this.ShowMessageAsync("Exito", "Se guardaron los cambios correctamente");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().IsAssignableFrom(typeof(DbEntityValidationException)))
                {
                    var validacionesEF = (DbEntityValidationException)ex;
                    var errores = validacionesEF.EntityValidationErrors;
                    foreach (var error in errores)
                    {
                        foreach (var item in error.ValidationErrors)
                        {
                            await this.ShowMessageAsync("Validacion", item.ErrorMessage);
                        }
                    }
                }
                else
                {
                    ex.GuardarError();
                }
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
                    var datos = Herramientas.Herramientas.abrirDialogo("*.jpg|*.jpg|*.png|*.png|*.bmp|*.bmp", false, "Seleccione una imagen para el usuario");
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
