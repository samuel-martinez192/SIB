using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace SIB.Herramientas
{
    public static class Herramientas
    {
        /// <summary>
        /// Guarda las excepciones en un .log
        /// </summary>
        public static void GuardarError(this Exception error)
        {
            try
            {
                File.WriteAllText(Environment.ExpandEnvironmentVariables("%appData%") + DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + ".log",
                   "Ocurrio el siguiente error: \n" + error.Message + "\nCall Stack: \n" + error.StackTrace + "");
                MessageBox.Show("Ocurrio el siguiente error: \n" + error.Message + "\nCall Stack: \n" + error.StackTrace, "Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Retorna una imagen con nuevas dimensiones
        /// </summary>
        public static Image RedimensionarImagen(this Image imagen, int newWidth, int newHeight)
        {
            try
            {
                var newImage = new Bitmap(newWidth, newHeight);

                using (var graphics = Graphics.FromImage(newImage))
                {
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(imagen, new Rectangle(0, 0, newWidth, newHeight));
                }

                return newImage;
            }
            catch (Exception ex)
            {
                ex.GuardarError();
                return null;
            }
        }

        /// <summary>
        /// Convierte una imagen a un arreglo de bytes
        /// </summary>
        public static byte[] ImagenABytes(this Image imagen)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    imagen.Save(ms, ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                ex.GuardarError();
                return null;
            }
        }

        /// <summary>
        /// Convierte un arreglo de bytes a imagen
        /// </summary>
        public static Image BytesAImagen(this byte[] bytes)
        {
            try
            {
                MemoryStream ms = new MemoryStream(bytes);
                Image imagen = Image.FromStream(ms);
                return imagen;
            }
            catch (Exception ex)
            {
                ex.GuardarError();
                return null;
            }
        }

        public static BitmapImage ImagenABitmap(this Image imagen)
        {
            using (var ms = new MemoryStream())
            {
                imagen.Save(ms, ImageFormat.Jpeg);
                ms.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public static List<string> obtenerRutaDialogo(string strFiltro = "Text Files (.txt)|*.txt|All Files (*.*)|*.*", bool multiSeleccion = false, string titulo = "Seleccionar archivo/s")
        {
            List<string> ret = new List<string>();

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = strFiltro;
                dialog.Multiselect = multiSeleccion;
                dialog.Title = titulo;


                if (!multiSeleccion)
                {
                    if (dialog.ShowDialog() == DialogResult.OK && dialog.FileName != null && dialog.FileName.Length > 0)
                    {
                        string ruta = dialog.FileName;
                        ret.Add(ruta);
                    }
                }
                else
                {
                    if (dialog.ShowDialog() == DialogResult.OK && dialog.FileNames != null && dialog.FileNames.Length > 0)
                    {
                        string[] rutas = dialog.FileNames;
                        foreach (var item in rutas)
                        {
                            ret.Add(item);
                        }
                    }
                }
            }

            return ret;
        }

        public static List<byte[]> obtenerDatosDialogo(string strFiltro = "Text Files (.txt)|*.txt|All Files (*.*)|*.*", bool multiSeleccion = false, string titulo = "Seleccionar archivo/s")
        {
            List<byte[]> ret = new List<byte[]>();

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = strFiltro;
                dialog.Multiselect = multiSeleccion;
                dialog.Title = titulo;


                if (!multiSeleccion)
                {
                    if (dialog.ShowDialog() == DialogResult.OK && dialog.FileName != null && dialog.FileName.Length > 0)
                    {
                        var data = dialog.FileName;
                        var bytes = File.ReadAllBytes(data);
                        ret.Add(bytes);
                    }
                }
                else
                {
                    if (dialog.ShowDialog() == DialogResult.OK && dialog.FileNames != null && dialog.FileNames.Length > 0)
                    {
                        string[] data = dialog.FileNames;
                        foreach (var item in data)
                        {
                            var bytes = File.ReadAllBytes(item);
                            ret.Add(bytes);
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Metodo que recibe el texto plano y nos regresa un texto cifrado usando SHA 256
        /// </summary>
        public static string obtenerSHA256(this string texto)
        {
            string resultado = null;

            // Creamos una instancia de SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computamos el hash y regresa un arreglo de bytes
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                //Verificamos que nuestro arreglo de bytes tenga datos
                if (bytes != null && bytes.Length > 0)
                {
                    //Vamos a convertirlo a una cadena de texto
                    StringBuilder builder = new StringBuilder();

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }

                    resultado = builder.ToString();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Encripta el texto usando el algoritmo AES y lo retona en Base 64
        /// </summary>
        public static string EncriptarTexto(this string textoPlano, string llave)
        {
            // Revisando parametros.
            if (textoPlano == null || textoPlano.Length <= 0)
                throw new ArgumentNullException("textoPlano");
            if (llave == null || llave.Length <= 0)
                throw new ArgumentNullException("llave");

            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(llave);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(textoPlano);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }

        /// <summary>
        /// Desencripta el texto cifrado usando AES en formato Base 64
        /// </summary>
        public static string DesencriptarTexto(this string textoCifrado, string llave)
        {
            // Revisando parametros.
            if (textoCifrado == null || textoCifrado.Length <= 0)
                throw new ArgumentNullException("textoCifrado");
            if (llave == null || llave.Length <= 0)
                throw new ArgumentNullException("llave");

            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(textoCifrado);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(llave);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
