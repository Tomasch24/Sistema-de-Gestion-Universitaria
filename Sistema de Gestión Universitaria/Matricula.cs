using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_de_Gestión_Universitaria
{
    public class Matricula : IEvaluable
    {
        private List<decimal> calificaciones;
        private const decimal NOTA_MINIMA_APROBACION = 7.0m;

        public Estudiante Estudiante { get; set; }
        public Curso Curso { get; set; }
        public DateTime FechaMatricula { get; set; }

        // Lista de calificaciones - Encapsulación
        public List<decimal> Calificaciones => calificaciones;

        public Matricula(Estudiante estudiante, Curso curso)
        {
            Estudiante = estudiante ?? throw new ArgumentNullException(nameof(estudiante));
            Curso = curso ?? throw new ArgumentNullException(nameof(curso));
            FechaMatricula = DateTime.Now;
            calificaciones = new List<decimal>();
        }

        // Implementación de IEvaluable
        public void AgregarCalificacion(decimal calificacion)
        {
            if (calificacion < 0 || calificacion > 10)
                throw new ArgumentException("La calificación debe estar entre 0 y 10");

            calificaciones.Add(calificacion);
        }

        public decimal ObtenerPromedio()
        {
            if (calificaciones.Count == 0)
                return 0;

            // Uso de LINQ para calcular promedio
            return calificaciones.Average();
        }

        public bool HaAprobado()
        {
            if (calificaciones.Count == 0)
                return false;

            return ObtenerPromedio() >= NOTA_MINIMA_APROBACION;
        }

        /// <summary>
        /// Retorna el estado actual de la matrícula
        /// </summary>
        public string ObtenerEstado()
        {
            if (calificaciones.Count == 0)
                return "En Curso";

            return HaAprobado() ? "Aprobado" : "Reprobado";
        }

        public override string ToString()
        {
            return $"Matrícula: {Estudiante.Nombre} {Estudiante.Apellido} en {Curso.Nombre} - " +
                   $"Promedio: {ObtenerPromedio():F2} - Estado: {ObtenerEstado()}";
        }
    }
}
