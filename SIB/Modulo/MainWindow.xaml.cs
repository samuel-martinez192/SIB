using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Behaviours;
using SIB.Herramientas;
using SIB.EntityFramework;

namespace SIB.Modulo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
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
        /// Consulta los usuarios activos del sistema
        /// </summary>
        private async void ConsultarUsuario()
        {
            try
            {
                using (var context = new BibliotecaContext())
                {
                    if (txtUsuario.Text == "" || pbContraseña.Password == "")
                    {
                        await this.ShowMessageAsync("Error", "La contraseña y el usuario no pueden estar en blanco");
                    }
                    else
                    {
                        var usuario = context.Usuarios.Where(x => x.NickName == txtUsuario.Text && x.Contraseña == pbContraseña.Password && x.idEstatus == true).FirstOrDefault();
                        if (usuario != null)
                        {
                            await this.ShowMessageAsync("Exito", "Bienvenido " + usuario.NickName);
                            Menu ventanaMenu = new Menu(usuario);
                            ventanaMenu.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            await this.ShowMessageAsync("Error", "Verifica tus datos");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Consulta usuarios activos del sistema
        /// </summary>
        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConsultarUsuario();
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }

        /// <summary>
        /// Dispone del contexto actual y cierra el control
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

        /// <summary>
        /// Consulta usuarios activos del sistema
        /// </summary>
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    ConsultarUsuario();
                }
            }
            catch (Exception ex)
            {
                ex.GuardarError();
            }
        }
    }
}
