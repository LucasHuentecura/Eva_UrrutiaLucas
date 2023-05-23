using MedidorModel.DAL;
using MedidorModel.DTO;
using ServidorSocketUtils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eva_UrrutiaLucas.Comunicacion
{
    public class HebraCliente
    {
        private ClienteCom clienteCom;
        private IMedidorDAL medidorDAL = MedidorDALArchivos.GetInstancia();

        public HebraCliente(ClienteCom clienteCom)
        {
            this.clienteCom = clienteCom;
        }

        public void Ejecutar()
        {
            int numero = ValidarNumero();
            string fecha = ValidarFecha();
            double valorConsumo = ValidarValorConsumo();
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

            clienteCom.Escribir("Datos Validados y Guardados :D");
            clienteCom.Desconectar();
        }

        private int ValidarNumero()
        {
            // Variable que guardará el valor ingresado.
            int numero;

            // Bucle infinito hasta que se ingrese un número válido.
            while (true)
            {
                // Solicita al usuario que ingrese un número para el medidor
                clienteCom.Escribir("\nIngrese numero del medidor\n");
                // Intentar analizar la entrada del usuario como un número entero
                if (int.TryParse(clienteCom.Leer(), out numero))
                {
                    // Muestra un mensaje cuando el dato ingresado sea válido
                    clienteCom.Escribir("\nGuardando...\n");
                    //Devolver el número válido
                    return numero;
                } //End if

                // Muestra un mensaje cuando el dato ingresado no es válido
                clienteCom.Escribir("\nDebe ingresar un numero entero\n");
            } //End while
        } //End método ValidarNumero()

        private string ValidarFecha()
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
                clienteCom.Escribir("\nIngrese una fecha en el formato yyyy-MM-dd-HH-mm-ss:\n");
                // Obtener los datos ingresados por el usuario
                fecha = clienteCom.Leer();

                // Intentar analizar la entrada del usuario con el formato de fecha indicada
                if (DateTime.TryParseExact(fecha, format, null, System.Globalization.DateTimeStyles.None, out resultado))
                {
                    // Mostrar un mensaje cuando el dato ingresado sea válido
                    clienteCom.Escribir("\nGuardando...\n");
                    // Devolver la fecha válida
                    return fecha;
                } // End if

                // Muestra un mensaje cuando el dato ingresado no es válido
                clienteCom.Escribir("\nEl formato de la fecha no es correcto. Vuelva a intentar...\n");
            } // End while
        } //End método ValidarFecha() 

        private double ValidarValorConsumo()
        {
            // Seleccionar la cultura "es-ES" para permitir la coma decimal
            CultureInfo cultura = new CultureInfo("es-ES");

            // Variable que guardará el valor ingresado
            double valorConsumo;

            // Bucle infinito hasta que se ingrese un valor válido.
            while (true)
            {
                // Solicita al usuario que ingrese el consumo
                clienteCom.Escribir("\nIngrese el consumo\n");

                // Intentar analizar la entrada del usuario como un número entero
                if (double.TryParse(clienteCom.Leer(), NumberStyles.Float, cultura, out valorConsumo))
                {
                    // Muestra un mensaje cuando el dato ingresado sea válido
                    clienteCom.Escribir("\nGuardando...\n");
                    //Devolver el consumo válido
                    return valorConsumo;
                } //End if

                // Muestra un mensaje cuando el dato ingresado no es válido
                clienteCom.Escribir("\nDebe ser un numero decimal (Utilizando coma decimal). Vuelva a intentar...\n");
            } //End while
        }// End método ValidarValorConsumo()
    }
}
