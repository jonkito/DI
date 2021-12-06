using Calculadora_BLL;
using System;

namespace Calculadora_Consola
{
    class Calculadora
    {
        private enum SalidaPrograma
        {
            CONTINUAR,
            SALIDA,
            BORRADOBBDD
        }

        static void Main()
        {
            do
            {
                Intro();

                int operador = ElegirOperador();
                if (operador < 5 && operador > 0) { Console.WriteLine("  Introduce el primer operando: "); }
                else if (operador == -1) { break; }
                else if (operador == 0) { continue; }
                else { Console.WriteLine("  Introduce el operando: "); }
                string operando1 = Console.ReadLine();

                string operando2;
                //si el operador no es raiz cuadrada o potencia cuadrado solicitamos el segundo operando
                if (operador < 5)
                {
                    Console.WriteLine("  Introduce el segundo operando: ");
                    operando2 = Console.ReadLine();
                }
                else { operando2 = "0"; }

                // Comprobamos que los operandos introducidos son correctos y realizamos la operacion
                ComprobarOperandos(operando1, operando2, operador);

                Console.WriteLine("  \n\nPulsa INTRO para continuar...  ");
                Console.ReadLine();
            } while (true);

            Console.WriteLine("PULSA UNA TECLA PARA FINALIZAR");
            Console.ReadKey();
        }

        /// <summary>
        /// Muestra la intoducción a la aplicación
        /// </summary>
        static void Intro()
        {
            Console.Clear();
            Console.WriteLine("Bienvenidos a la C A L C U L A D O R A", Console.ForegroundColor = ConsoleColor.Red, Console.BackgroundColor = ConsoleColor.White);
            Console.ResetColor();

            MostrarUltimas10Operaciones();
            Console.WriteLine("\n  Introduce 'q' para salir!  \n  Iantoduce 'c' para borrar la BBDD", Console.ForegroundColor = ConsoleColor.Black, Console.BackgroundColor = ConsoleColor.White);
            Console.ResetColor();
        }

        /// <summary>
        /// se comprueban que los operandos son correctos en formato para realizar
        /// operaciones matemáticas.
        /// En caso afirmativo se realizar la operación y se salva en la BBDD.
        /// En caso negativo se informa que no son correctos.
        /// </summary>
        /// <param name="operando1">primer operando para la operacion</param>
        /// <param name="operando2">segundo operando para la operacion</param>
        /// <param name="operador">operador para realizar la operacion</param>
        static void ComprobarOperandos(string operando1, string operando2, int operador)
        {
            if (double.TryParse(SustituirPunto(operando1), out double dOperando1) && double.TryParse(SustituirPunto(operando2), out double dOperando2))
            {
                DTO dto = BLL.RealizarOperacion(new DTO(dOperando1, dOperando2, operador));

                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write("  " + dto.ToString() + "  ");
                Console.ResetColor();

                BLL.GuardarOperacion(dto.ToString()); //guardamos en BBDD la operacion
            }
            else
            {
                Console.WriteLine("  ALGUNO DE LOS OPERADORES NO ES CORRECTO...  ", Console.ForegroundColor = ConsoleColor.Black, Console.BackgroundColor = ConsoleColor.Red);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Muestra las ultimas 10 operaciones alcacenadas en BBDD
        /// </summary>
        private static void MostrarUltimas10Operaciones()
        {
            Console.WriteLine("\n  Ultimas 10 operaciones  ", Console.ForegroundColor = ConsoleColor.Black, Console.BackgroundColor = ConsoleColor.White);
            Console.ResetColor();
            Console.WriteLine("\n--------------------------\n{0}" +
                               "\n--------------------------", BLL.ObtenerOperaciones());
        }

        /// <summary>
        /// Evalúa el input del usuari@ y indica si es la opción de salir
        /// </summary>
        /// <param name="cadena">input del usuari@</param>
        /// <returns>true si es salir, false si no lo es</returns>
        static SalidaPrograma ComprobarInput(string cadena)
        {
            if (cadena.ToLower() == "q")
            {
                Console.WriteLine("  CERRANDO CALCULADORA...  ");
                return SalidaPrograma.SALIDA;
            }
            else if (cadena.ToLower() == "c")
            {
                Console.WriteLine("  BORRANDO BBDD...  ");
                BLL.BorrarOperaciones();
                return SalidaPrograma.BORRADOBBDD;
            }
            return SalidaPrograma.CONTINUAR;
        }

        /// <summary>
        /// Muestra las operaciones posibles y retorna la elección 
        /// </summary>
        /// <returns>elección del usuari@</returns>
        static int ElegirOperador()
        {
            do
            {
                Console.WriteLine("\n  Elige una operacion..." +
                                    "\n   1 - SUMA" +
                                    "\n   2 - RESTA" +
                                    "\n   3 - MULTIPLICACION" +
                                    "\n   4 - DIVISION" +
                                    "\n   5 - RAIZ CUADRADA" +
                                    "\n   6 - POTENCIA CUADRADO");
                string sEleccion = Console.ReadLine();

                if (int.TryParse(sEleccion, out int eleccion) && eleccion > 0 && eleccion < 7)
                {
                    return eleccion;
                }
                else
                {
                    // Comprobamos la acción solicitada por el usuari@
                    SalidaPrograma salida = ComprobarInput(sEleccion);
                    if (salida == SalidaPrograma.SALIDA) { return -1; }
                    else if (salida == SalidaPrograma.BORRADOBBDD) { return 0; }

                    Console.WriteLine("  OPERADOR INCORRECTO...", Console.ForegroundColor = ConsoleColor.Black, Console.BackgroundColor = ConsoleColor.Red);
                    Console.ResetColor();
                }
            } while (true);
        }

        /// <summary>
        /// Sustituye punto por coma para las operacione aritméticas
        /// </summary>
        /// <param name="operador">cadenas que contiene puntos</param>
        /// <returns>string con los puntos sustituidas por comas</returns>
        static string SustituirPunto(string operador) => operador.Replace('.', ',');
    }
}
