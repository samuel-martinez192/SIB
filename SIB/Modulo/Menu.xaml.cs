using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Behaviours;
using SIB.Herramientas;
using SIB.EntityFramework;
using System.Data;

namespace SIB.Modulo
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : MetroWindow
    {
        Usuarios usuario = new Usuarios();

        /// <summary>
        /// Constructor sin parametros
        /// </summary>
        public Menu()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Constructor que recibe entidad usuario
        /// </summary>
        public Menu(Usuarios usuario)
        {
            try
            {
                InitializeComponent();
                this.usuario = usuario;
                Validaciones();
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Muestra los Tab del MetroTab ocultos
        /// </summary>
        private void Validaciones()
        {
            try
            {
                if (usuario.idPermiso == 1)
                {
                    //administrador
                    mtiPrestamo.Visibility = Visibility.Visible;
                    mtiMaestros.Visibility = Visibility.Visible;
                    mtiAlumnos.Visibility = Visibility.Visible;
                    mtiIncidencias.Visibility = Visibility.Visible;
                    mtiUsuarios.Visibility = Visibility.Visible;

                }
                else if (usuario.idPermiso == 2)
                {
                    //administrador biblioteca
                    mtiPrestamo.Visibility = Visibility.Visible;
                    mtiMaestros.Visibility = Visibility.Visible;
                    mtiAlumnos.Visibility = Visibility.Visible;
                    mtiIncidencias.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Se ejecuta un control y valida permisos
        /// </summary>
        private async void tile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (e.Source == tileUsuarios)
                {
                    if (usuario.idPermiso == 1)
                    {
                        CTRUsuarios ctrUsuarios = new CTRUsuarios();
                        ctrUsuarios.ShowDialog();
                    }
                    else
                    {
                        await this.ShowMessageAsync("Informacion", "Solo el administrador puede dar de alta usuarios");
                    }

                }
                else
                {
                    if (usuario.idPermiso == 1 || usuario.idPermiso == 2)
                    {
                        if (e.Source == tileMaestros)
                        {
                            CTRMaestros ctrMaestros = new CTRMaestros();
                            ctrMaestros.ShowDialog();
                        }
                        else if (e.Source == tileAlumnos)
                        {
                            CTRAlumno ctrAlumno = new CTRAlumno();
                            ctrAlumno.ShowDialog();
                        }
                        else if (e.Source == tileAutores)
                        {
                            CTRAutores ctrAutores = new CTRAutores();
                            ctrAutores.ShowDialog();
                        }
                        else if (e.Source == tileEditorial)
                        {
                            CTREditorial ctrEditorial = new CTREditorial();
                            ctrEditorial.ShowDialog();
                        }
                        else if (e.Source == tileLibro)
                        {
                            CTRLibro ctrLibro = new CTRLibro();
                            ctrLibro.ShowDialog();
                        }
                        else if (e.Source == tilePrestamo)
                        {
                            CTRPrestamo ctrPrestamo = new CTRPrestamo();
                            ctrPrestamo.ShowDialog();
                        }
                        else if (e.Source == tileIncidencias)
                        {
                            CTRIncidencias ctrIncidencias = new CTRIncidencias();
                            ctrIncidencias.ShowDialog();
                        }
                    }
                    else
                    {
                        await this.ShowMessageAsync("Informacion", "Solo los administradores pueden dar altas");
                    }
                }
            }

            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Consulta las entidades de los datagrid
        /// </summary>
        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new BibliotecaContext())
                {
                    if (e.Source == btnConsultarLibro)
                    {
                        dgLibros.ItemsSource = context.Libros.Where(x => x.FechaIngreso >= dpFechaInicioLibro.SelectedDate
                        && x.FechaIngreso <= dpFechaFinLibro.SelectedDate && x.idEstatus == true).ToList();
                    }
                    else if (e.Source == btnConsultarAutor)
                    {
                        dgAutores.ItemsSource = context.Autores.Where(x => x.FechaIngreso >= dpFechaInicioAutor.SelectedDate
                        && x.FechaIngreso <= dpFechaFinAutor.SelectedDate && x.idEstatus == true).ToList();
                    }
                    else if (e.Source == btnConsultarEditorial)
                    {
                        dgEditorial.ItemsSource = context.Editorial.Where(x => x.FechaIngreso >= dpFechaInicioEditorial.SelectedDate
                        && x.FechaIngreso <= dpFechaFinEditorial.SelectedDate && x.idEstatus == true).ToList();
                    }
                    else if (e.Source == btnConsultarPrestamo)
                    {
                        dgPrestamos.ItemsSource = context.Prestamos.Where(x => x.FechaIngreso >= dpFechaInicioPrestamo.SelectedDate
                        && x.FechaIngreso <= dpFechaFinPrestamo.SelectedDate).ToList();
                    }
                    else if (e.Source == btnConsultarMaestros)
                    {
                        dgMaestros.ItemsSource = context.Maestros.Where(x => x.FechaIngreso >= dpFechaInicioMaestros.SelectedDate
                        && x.FechaIngreso <= dpFechaFinMaestros.SelectedDate && x.idEstatus == true).ToList();
                    }
                    else if (e.Source == btnConsultarAlumnos)
                    {
                        dgAlumnos.ItemsSource = context.Alumnos.Where(x => x.FechaIngreso >= dpFechaInicioAlumnos.SelectedDate
                        && x.FechaIngreso <= dpFechaFinAlumnos.SelectedDate && x.idEstatus == true).ToList();
                    }
                    else if (e.Source == btnConsultarUsuarios)
                    {

                        dgUsuarios.ItemsSource = context.Usuarios.Where(x => x.FechaIngreso >= dpFechaInicioUsuarios.SelectedDate
                        && x.FechaIngreso <= dpFechaFinUsuarios.SelectedDate && x.idEstatus == true).ToList();
                    }
                    else if (e.Source == btnConsultarIncidencias)
                    {
                        dgIncidencias.ItemsSource = context.Incidencias.Where(x => x.FechaIngreso >= dpFechaInicioIncidencias.SelectedDate
                        && x.FechaIngreso <= dpFechaFinIncidencias.SelectedDate && x.idEstatus == true).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Filtra las consultas
        /// </summary>
        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                using (var context = new BibliotecaContext())
                {
                    if (e.Source == txtFiltroLibro)
                    {
                        dgLibros.ItemsSource = context.Libros.Where(x => x.FechaIngreso >= dpFechaInicioLibro.SelectedDate
                        && x.FechaIngreso <= dpFechaFinLibro.SelectedDate
                        && x.idEstatus == true && x.Titulo.Contains(txtFiltroLibro.Text)).ToList();
                    }
                    else if (e.Source == txtFiltroAutor)
                    {
                        dgAutores.ItemsSource = context.Autores.Where(x => x.FechaIngreso >= dpFechaInicioAutor.SelectedDate
                        && x.FechaIngreso <= dpFechaFinAutor.SelectedDate
                        && x.idEstatus == true && x.Nombre.Contains(txtFiltroAutor.Text)).ToList();
                    }
                    else if (e.Source == txtFiltroEditorial)
                    {
                        dgEditorial.ItemsSource = context.Editorial.Where(x => x.FechaIngreso >= dpFechaInicioEditorial.SelectedDate
                        && x.FechaIngreso <= dpFechaFinEditorial.SelectedDate
                        && x.idEstatus == true && x.Nombre.Contains(txtFiltroEditorial.Text)).ToList();
                    }
                    else if (e.Source == txtFiltroPrestamo)
                    {

                    }
                    else if (e.Source == txtFiltroMaestros)
                    {
                        dgMaestros.ItemsSource = context.Maestros.Where(x => x.FechaIngreso >= dpFechaInicioMaestros.SelectedDate
                        && x.FechaIngreso <= dpFechaFinMaestros.SelectedDate
                        && x.idEstatus == true && x.Nombre.Contains(txtFiltroMaestros.Text)).ToList();
                    }
                    else if (e.Source == txtFiltroAlumnos)
                    {
                        dgAlumnos.ItemsSource = context.Alumnos.Where(x => x.FechaIngreso >= dpFechaInicioAlumnos.SelectedDate
                        && x.FechaIngreso <= dpFechaFinAlumnos.SelectedDate
                        && x.idEstatus == true && x.Nombre.Contains(txtFiltroAlumnos.Text)).ToList();
                    }
                    else if (e.Source == txtFiltroUsuarios)
                    {
                        dgUsuarios.ItemsSource = context.Usuarios.Where(x => x.FechaIngreso >= dpFechaInicioUsuarios.SelectedDate
                        && x.FechaIngreso <= dpFechaFinUsuarios.SelectedDate
                        && x.idEstatus == true && x.NickName.Contains(txtFiltroUsuarios.Text)).ToList();
                    }
                    else if (e.Source == txtFiltroIncidencias)
                    {
                        dgIncidencias.ItemsSource = context.Incidencias.Where(x => x.FechaIngreso >= dpFechaInicioIncidencias.SelectedDate
                        && x.FechaIngreso <= dpFechaFinIncidencias.SelectedDate
                        && x.idEstatus == true && x.Descripcion.Contains(txtFiltroIncidencias.Text)).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Ejecuta la instancia en edicion de los controles
        /// </summary>
        private void dataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.Source == dgLibros)
                {
                    Libros libro = (Libros)dgLibros.SelectedValue;
                    CTRLibro control = new CTRLibro(libro);
                    control.btnImprimir.Visibility = Visibility.Visible;
                    control.ShowDialog();
                }
                else if (e.Source == dgAutores)
                {
                    Autores autor = (Autores)dgAutores.SelectedValue;
                    CTRAutores control = new CTRAutores(autor);
                    control.ShowDialog();
                }
                else if (e.Source == dgEditorial)
                {
                    Editorial editorial = (Editorial)dgEditorial.SelectedValue;
                    CTREditorial control = new CTREditorial(editorial);
                    control.ShowDialog();
                }
                else if (e.Source == dgPrestamos)
                {
                    Prestamos prestamo = (Prestamos)dgPrestamos.SelectedValue;
                    CTRPrestamo control = new CTRPrestamo(prestamo);
                    control.btnImprimir.Visibility = Visibility.Visible;
                    control.ShowDialog();
                }
                else if (e.Source == dgMaestros)
                {
                    Maestros maestro = (Maestros)dgMaestros.SelectedValue;
                    CTRMaestros control = new CTRMaestros(maestro);
                    control.ShowDialog();
                }
                else if (e.Source == dgAlumnos)
                {
                    Alumnos alumno = (Alumnos)dgAlumnos.SelectedValue;
                    CTRAlumno control = new CTRAlumno(alumno);
                    control.ShowDialog();
                }
                else if (e.Source == dgUsuarios)
                {
                    Usuarios usuario = (Usuarios)dgUsuarios.SelectedValue;
                    CTRUsuarios control = new CTRUsuarios(usuario);
                    control.ShowDialog();
                }
                else if (e.Source == dgIncidencias)
                {
                    Incidencias inciencida = (Incidencias)dgIncidencias.SelectedValue;
                    CTRIncidencias control = new CTRIncidencias(inciencida);
                    control.btnImprimir.Visibility = Visibility.Visible;
                    control.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }
    }
}
