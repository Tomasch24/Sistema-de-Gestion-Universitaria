using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_de_Gestión_Universitaria
{
    /// <summary>
    /// Atributo para validar rangos numéricos
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidacionRangoAttribute : Attribute
    {
        public decimal Minimo { get; set; }
        public decimal Maximo { get; set; }

        public ValidacionRangoAttribute(double min, double max)
        {
            Minimo = (decimal)min;
            Maximo = (decimal)max;
        }

        public bool EsValido(decimal valor)
        {
            return valor >= Minimo && valor <= Maximo;
        }

        public override string ToString()
        {
            return $"Rango válido: {Minimo} - {Maximo}";
        }
    }

    /// <summary>
    /// Atributo para campos requeridos
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RequeridoAttribute : Attribute
    {
        public string MensajeError { get; set; }

        public RequeridoAttribute()
        {
            MensajeError = "Este campo es requerido";
        }

        public RequeridoAttribute(string mensaje)
        {
            MensajeError = mensaje;
        }

        public bool EsValido(object valor)
        {
            if (valor == null)
                return false;

            if (valor is string texto)
                return !string.IsNullOrWhiteSpace(texto);

            return true;
        }
    }

    /// <summary>
    /// Atributo para especificar formato de strings
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FormatoAttribute : Attribute
    {
        public string Patron { get; set; }
        public string Descripcion { get; set; }

        public FormatoAttribute(string patron)
        {
            Patron = patron;
        }

        public bool EsValido(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return false;

            // Verificar longitud basada en el patrón
            int longitudEsperada = Patron.Replace("-", "").Length;
            string valorLimpio = valor.Replace("-", "");

            return valorLimpio.Length == longitudEsperada;
        }
    }

    /// <summary>
    /// Clase para validar objetos usando atributos personalizados y Reflection
    /// </summary>
    public class Validador
    {
        /// <summary>
        /// Valida un objeto y retorna lista de errores
        /// </summary>
        public static List<string> ValidarObjeto(object obj)
        {
            var errores = new List<string>();

            if (obj == null)
            {
                errores.Add("El objeto no puede ser nulo");
                return errores;
            }

            Type tipo = obj.GetType();
            PropertyInfo[] propiedades = tipo.GetProperties();

            foreach (var propiedad in propiedades)
            {
                object valor = propiedad.GetValue(obj);

                // Validar atributo Requerido
                var atributoRequerido = propiedad.GetCustomAttribute<RequeridoAttribute>();
                if (atributoRequerido != null)
                {
                    if (!atributoRequerido.EsValido(valor))
                    {
                        errores.Add($"{propiedad.Name}: {atributoRequerido.MensajeError}");
                    }
                }

                // Validar atributo ValidacionRango
                var atributoRango = propiedad.GetCustomAttribute<ValidacionRangoAttribute>();
                if (atributoRango != null && valor != null)
                {
                    if (valor is decimal decimalValor)
                    {
                        if (!atributoRango.EsValido(decimalValor))
                        {
                            errores.Add($"{propiedad.Name}: Debe estar entre " +
                                      $"{atributoRango.Minimo} y {atributoRango.Maximo}. " +
                                      $"Valor actual: {decimalValor}");
                        }
                    }
                }

                // Validar atributo Formato
                var atributoFormato = propiedad.GetCustomAttribute<FormatoAttribute>();
                if (atributoFormato != null && valor is string textoValor)
                {
                    if (!atributoFormato.EsValido(textoValor))
                    {
                        errores.Add($"{propiedad.Name}: Formato inválido. " +
                                  $"Formato esperado: {atributoFormato.Patron}");
                    }
                }
            }

            return errores;
        }

        /// <summary>
        /// Muestra los atributos de validación de una clase
        /// </summary>
        public static void MostrarAtributosValidacion(Type tipo)
        {
            Console.WriteLine($"\n╔═══ ATRIBUTOS DE VALIDACIÓN: {tipo.Name} ═══╗");

            PropertyInfo[] propiedades = tipo.GetProperties();

            foreach (var propiedad in propiedades)
            {
                var atributos = propiedad.GetCustomAttributes();

                if (!atributos.Any())
                    continue;

                Console.WriteLine($"\n  📋 {propiedad.Name} ({propiedad.PropertyType.Name}):");

                foreach (var atributo in atributos)
                {
                    if (atributo is RequeridoAttribute req)
                    {
                        Console.WriteLine($"     ✓ Requerido: {req.MensajeError}");
                    }
                    else if (atributo is ValidacionRangoAttribute rango)
                    {
                        Console.WriteLine($"     ✓ {rango}");
                    }
                    else if (atributo is FormatoAttribute formato)
                    {
                        Console.WriteLine($"     ✓ Formato: {formato.Patron}");
                        if (!string.IsNullOrEmpty(formato.Descripcion))
                            Console.WriteLine($"        {formato.Descripcion}");
                    }
                }
            }

            Console.WriteLine("\n╚" + new string('═', 50) + "╝");
        }
    }
}
