using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_de_Gestión_Universitaria
{
    public class TiposEspeciales
    {
        //
        //TODO Convierte y formatea datos de diferentes tipos usando Pattern Matching
        // Aplica: Boxing/Unboxing, Pattern Matching, Conversiones
        //
        public static string ConvertirDatos(object dato)
        {
            // Pattern Matching con switch expression
            return dato switch
            {
                null => "Valor nulo",
                int entero => $"Entero: {entero:N0}",
                double doble => $"Decimal: {doble:F2}",
                decimal dec => $"Decimal preciso: {dec:F2}",
                string texto => $"Texto: '{texto}'",
                DateTime fecha => $"Fecha: {fecha:dd/MM/yyyy HH:mm:ss}",
                bool booleano => $"Booleano: {(booleano ? "Verdadero" : "Falso")}",
                char caracter => $"Carácter: '{caracter}'",
                _ => $"Tipo no reconocido: {dato.GetType().Name}"
            };
        }
    
    
        //
        //TODO Parsea una calificación de forma segura usando TryParse
        //Aplica: TryParse, Validaciones
        //
        public static (bool exito, decimal valor, string mensaje) ParsearCalificacion(string entrada)
        {
            if (string.IsNullOrWhiteSpace(entrada))
                return (false, 0, "La entrada no puede estar vacía");

            // TryParse para conversión segura
            if (!decimal.TryParse(entrada, out decimal calificacion))
                return (false, 0, "La entrada no es un número válido");

            // Validar rango
            if (calificacion < 0 || calificacion > 10)
                return (false, calificacion, "La calificación debe estar entre 0 y 10");

            return (true, calificacion, "Calificación válida");
        }

        //
        //TODO Demuestra Boxing y Unboxing con calificaciones
        //
        public static void DemostrarBoxingUnboxing()
        {
            Console.WriteLine("\n═══ DEMOSTRACIÓN DE BOXING Y UNBOXING ═══");

            // Boxing: valor decimal a object
            decimal calificacion = 8.5m;
            object calificacionBoxed = calificacion; // BOXING
            Console.WriteLine($"Boxing: decimal {calificacion} → object");
            Console.WriteLine($"Tipo boxed: {calificacionBoxed.GetType().Name}");

            // Unboxing: object a decimal
            decimal calificacionUnboxed = (decimal)calificacionBoxed; // UNBOXING
            Console.WriteLine($"Unboxing: object → decimal {calificacionUnboxed}");

            // Array de objetos (boxing implícito)
            object[] datos = { 10, 9.5m, "Excelente", true, DateTime.Now };
            Console.WriteLine("\nArray de objetos con tipos mixtos:");
            foreach (var dato in datos)
            {
                Console.WriteLine($"  {ConvertirDatos(dato)}");
            }
        }
    }
}
