using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIB.Herramientas
{
    public static class LlaveJson
    {
        private static bool crearDirectorio(string directorio)
        {
            bool resultado = (Directory.Exists(directorio)) ? true : Directory.CreateDirectory(directorio).Exists;

            return resultado;
        }

        public static Llave ObtenerLlave(string directorio, string nombreArchivo)
        {
            Llave llave = null;

            if (crearDirectorio(directorio))
            {
                string archivo = directorio + nombreArchivo;

                if (!File.Exists(archivo))
                {
                    Llave temp = new Llave();
                    temp.Autor = "Samuel Martinez";
                    temp.Compañia = "UACJ";
                    temp.Valor = "b14ca5898a4e4133bbce2ea2315a1916";
                    temp.FechaCreacion = DateTime.Now;

                    string contenidoJson = JsonConvert.SerializeObject(temp);

                    File.WriteAllText(archivo, contenidoJson);

                    if (File.Exists(archivo))
                    {
                        var datos = File.ReadAllText(archivo);
                        llave = JsonConvert.DeserializeObject<Llave>(datos);
                    }
                    else
                    {
                        throw new IOException("Error al crear el archivo");
                    }
                }
                else
                {
                    var datos = File.ReadAllText(archivo);
                    llave = JsonConvert.DeserializeObject<Llave>(datos);
                }
            }

            return llave;
        }
    }
}
