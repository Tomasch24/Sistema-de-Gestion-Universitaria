using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_de_Gestión_Universitaria
{
    public class AnalizadorReflection
    {
        /// <summary>
        /// Muestra todas las propiedades de un tipo
        /// </summary>
        public static void MostrarPropiedades(Type tipo)
        {
            Console.WriteLine($"\n╔═══ PROPIEDADES DE {tipo.Name.ToUpper()} ═══╗");

            // Obtener todas las propiedades públicas
            PropertyInfo[] propiedades = tipo.GetProperties(
                BindingFlags.Public | BindingFlags.Instance);

            if (propiedades.Length == 0)
            {
                Console.WriteLine("  No hay propiedades públicas");
                return;
            }

            foreach (var prop in propiedades)
            {
                string acceso = "";
                if (prop.CanRead && prop.CanWrite)
                    acceso = "get/set";
                else if (prop.CanRead)
                    acceso = "get";
                else if (prop.CanWrite)
                    acceso = "set";

                Console.WriteLine($"  📌 {prop.Name}");
                Console.WriteLine($"     Tipo: {prop.PropertyType.Name}");
                Console.WriteLine($"     Acceso: {acceso}");
            }

            Console.WriteLine("╚" + new string('═', 50) + "╝");
        }

        /// <summary>
        /// Muestra todos los métodos públicos de un tipo
        /// </summary>
        public static void MostrarMetodos(Type tipo)
        {
            Console.WriteLine($"\n╔═══ MÉTODOS DE {tipo.Name.ToUpper()} ═══╗");

            // Obtener métodos públicos declarados en la clase (no heredados de Object)
            MethodInfo[] metodos = tipo.GetMethods(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            if (metodos.Length == 0)
            {
                Console.WriteLine("  No hay métodos públicos declarados");
                return;
            }

            foreach (var metodo in metodos)
            {
                // Ignorar métodos especiales como get_/set_ de propiedades
                if (metodo.IsSpecialName)
                    continue;

                var parametros = metodo.GetParameters();
                string listaParametros = string.Join(", ",
                    parametros.Select(p => $"{p.ParameterType.Name} {p.Name}"));

                Console.WriteLine($"  🔧 {metodo.Name}({listaParametros})");
                Console.WriteLine($"     Retorna: {metodo.ReturnType.Name}");
            }

            Console.WriteLine("╚" + new string('═', 50) + "╝");
        }

        /// <summary>
        /// Crea una instancia dinámicamente usando Reflection
        /// </summary>
        public static object CrearInstanciaDinamica(Type tipo, params object[] parametros)
        {
            try
            {
                // Usar Activator para crear instancia
                object instancia = Activator.CreateInstance(tipo, parametros);
                Console.WriteLine($"✓ Instancia de {tipo.Name} creada dinámicamente");
                return instancia;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error al crear instancia: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Invoca un método dinámicamente usando Reflection
        /// </summary>
        public static object InvocarMetodo(object instancia, string nombreMetodo,
            params object[] parametros)
        {
            if (instancia == null)
                throw new ArgumentNullException(nameof(instancia));

            Type tipo = instancia.GetType();

            try
            {
                // Buscar el método
                MethodInfo metodo = tipo.GetMethod(nombreMetodo,
                    BindingFlags.Public | BindingFlags.Instance);

                if (metodo == null)
                {
                    Console.WriteLine($"✗ Método '{nombreMetodo}' no encontrado en {tipo.Name}");
                    return null;
                }

                // Invocar el método
                object resultado = metodo.Invoke(instancia, parametros);
                Console.WriteLine($"✓ Método '{nombreMetodo}' invocado exitosamente");
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error al invocar método: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Analiza completamente una clase
        /// </summary>
        public static void AnalizarClaseCompleta(Type tipo)
        {
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine($"ANÁLISIS COMPLETO DE: {tipo.FullName}");
            Console.WriteLine(new string('═', 60));

            // Información básica
            Console.WriteLine($"\nTipo: {tipo.Name}");
            Console.WriteLine($"Namespace: {tipo.Namespace}");
            Console.WriteLine($"Es clase: {tipo.IsClass}");
            Console.WriteLine($"Es abstracta: {tipo.IsAbstract}");
            Console.WriteLine($"Clase base: {tipo.BaseType?.Name ?? "Ninguna"}");

            // Interfaces implementadas
            var interfaces = tipo.GetInterfaces();
            if (interfaces.Length > 0)
            {
                Console.WriteLine($"Interfaces: {string.Join(", ", interfaces.Select(i => i.Name))}");
            }

            // Mostrar propiedades y métodos
            MostrarPropiedades(tipo);
            MostrarMetodos(tipo);
        }
    }
}

