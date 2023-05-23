using MedidorModel.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedidorModel.DAL
{
    public class MedidorDALArchivos : IMedidorDAL
    {
        private MedidorDALArchivos() { }

        private static MedidorDALArchivos instancia;

        public static IMedidorDAL GetInstancia()
        {
            if (instancia == null)
            {
                instancia = new MedidorDALArchivos();
            }
            return instancia;
        }

        private static string url = Directory.GetCurrentDirectory();
        private static string archivo = url + "/lecturas.txt";

        public void AgregarMedidor(Medidor medidor)
        {
            try
            {
                using (StreamWriter write = new StreamWriter(archivo, true))
                {
                    write.WriteLine(medidor.ToString());
                    write.Flush();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<Medidor> ObtenerMedidor()
        {
            List<Medidor> lista = new List<Medidor>();
            try
            {
                using (StreamReader read = new StreamReader(archivo))
                {
                    string texto = "";
                    do
                    {
                        texto = read.ReadLine();
                        if (texto != null)
                        {
                            string[] arr = texto.Trim().Split('|');
                            Medidor medidor = new Medidor()
                            {
                                Numero = int.Parse(arr[0]),
                                Fecha = arr[1],
                                ValorConsumo = double.Parse(arr[2])
                            };
                            lista.Add(medidor);
                        }
                    } while (texto != null);
                }
            }
            catch (Exception ex)
            {
                lista = null;
            }
            return lista;
        }
    }
}
