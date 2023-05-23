using Eva_UrrutiaLucas.Comunicacion;
using MedidorModel.DAL;
using MedidorModel.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eva_UrrutiaLucas
{
    public class Program
    {
        private static IMedidorDAL medidorDAL = MedidorDALArchivos.GetInstancia();

        static bool Menu()
        {
            bool continuar = true;
            Console.WriteLine("Seleccione una opción");
            Console.WriteLine("1. Ingresar\n2. Mostrar\n0. Salir");
            switch (Console.ReadLine().Trim())
            {
                case "1":
                    Ingresar();
                    break;
                case "2":
                    Mostrar();
                    break;
                case "0": 
                    Console.Write("Adiós :)");
                    continuar = false;
                    break;
                default:
                    Console.WriteLine("Ingrese una opción válida");
                    break;
            }

            return continuar;
        }
        static void Main(string[] args)
        {
            HebraServidor hebra = new HebraServidor();
            Thread t = new Thread(new ThreadStart(hebra.Ejecutar));
            t.Start();

            while (Menu()) ;
        }

        static void Ingresar()
        {
            //Validar datos
            int numero = ValidarNumero();
            string fecha = ValidarFecha();
            double valorConsumo = ValidarValorConsumo();

            //Crear un nuevo medidor con los datos ingresados.
            Medidor medidor = new Medidor()
            {
                Numero = numero,
                Fecha = fecha,
                ValorConsumo = valorConsumo
            };
            lock (medidorDAL)
            {
                medidorDAL.AgregarMedidor(medidor);
            }

            Console.WriteLine("Datos Validados y Guardados :D\n\n");
        }

        static void Mostrar()
        {
            // Imprimir encabezado tabla.
            Console.WriteLine("Medidores\n----- ----- ----- ----- ----- ----- ----- ----- -----\nNumero\t\tFecha\t\t\tValor Consumo\n----- ----- ----- ----- ----- ----- ----- ----- -----");
            List<Medidor> medidor = null;
            lock (medidorDAL)
            {
                medidor = medidorDAL.ObtenerMedidor();
            }

            //Recorrer lista de medidores.
            foreach (Medidor m in medidor)
            {
                // Imprimir medidor.
                Console.WriteLine($"{m.Numero}\t\t{m.Fecha}\t{m.ValorConsumo}\n----- ----- ----- ----- ----- ----- ----- ----- -----\n\n");
            }

        }// End método Mostrar()

        static int ValidarNumero()
        {
            // Variable que guardará el valor ingresado.
            int numero;

            // Bucle infinito hasta que se ingrese un número válido.
            while (true)
            {
                // Solicita al usuario que ingrese un número para el medidor
                Console.WriteLine("Ingrese número del medidor");

                // Intentar analizar la entrada del usuario como un número entero
                if (int.TryParse(Console.ReadLine(), out numero))
                {
                    // Muestra un mensaje cuando el dato ingresado sea válido
                    Console.WriteLine("Guardando...");
                    //Devolver el número válido
                    return numero;
                } //End if

                // Muestra un mensaje cuando el dato ingresado no es válido
                Console.WriteLine("Debe ser un numero entero. Vuelva a intentar...");
            } //End while
        } //End método ValidarNumero()

        static string ValidarFecha()
        {
            // Variable que guardará el valor ingresado
            string fecha;
            // Formato para la fecha
            string format = "yyyy-MM-dd-HH-mm-ss";
            // Variable que almacena el resultado del análisis de fecha y hora
            DateTime resultado;

            // Bucle infinito hasta que se ingrese una fecha válida
            while (true)
            {
                // Solicita al usuario que ingrese una fecha utilizando el siguiente formato 
                Console.WriteLine("Ingrese una fecha en el formato yyyy-MM-dd-HH-mm-ss:");
                // Obtener los datos ingresados por el usuario
                fecha = Console.ReadLine();

                // Intentar analizar la entrada del usuario con el formato de fecha indicada
                if (DateTime.TryParseExact(fecha, format, null, System.Globalization.DateTimeStyles.None, out resultado))
                {
                    // Mostrar un mensaje cuando el dato ingresado sea válido
                    Console.WriteLine("Guardando...");
                    // Devolver la fecha válida
                    return fecha;
                } // End if

                // Muestra un mensaje cuando el dato ingresado no es válido
                Console.WriteLine("El formato de la fecha no es correcto. Vuelva a intentar...");
            } // End while
        } //End método ValidarFecha() 

        static double ValidarValorConsumo()
        {
            // Seleccionar la cultura "es-ES" para permitir la coma decimal
            CultureInfo cultura = new CultureInfo("es-ES");

            // Variable que guardará el valor ingresado
            double valorConsumo;

            // Bucle infinito hasta que se ingrese un valor válido.
            while (true)
            {
                // Solicita al usuario que ingrese el consumo
                Console.WriteLine("Ingrese el consumo");

                // Intentar analizar la entrada del usuario como un número entero
                if (double.TryParse(Console.ReadLine(), NumberStyles.Float, cultura, out valorConsumo))
                {
                    // Muestra un mensaje cuando el dato ingresado sea válido
                    Console.WriteLine("Guardando...");
                    //Devolver el consumo válido
                    return valorConsumo;
                } //End if

                // Muestra un mensaje cuando el dato ingresado no es válido
                Console.WriteLine("Debe ser un numero decimal (Utilizando coma decimal). Vuelva a intentar...");
            } //End while
        }// End método ValidarValorConsumo()
    }
}
