using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_de_Gestión_Universitaria
{
    public static class Consultas
    {
        /// <summary>
        /// Obtiene los 10 estudiantes con mejor promedio general
        /// Aplica: LINQ, OrderByDescending, Take, Select
        /// </summary>
        public static List<(Estudiante estudiante, decimal promedio)>
            ObtenerTop10Estudiantes(this GestorMatriculas gestor, Repositorio<Estudiante> repoEstudiantes)
        {
            var estudiantes = repoEstudiantes.ObtenerTodos();
            var todasMatriculas = gestor.ObtenerTodasLasMatriculas();

            // Calcular promedio general por estudiante
            var estudiantesConPromedio = estudiantes
                .Select(est => new
                {
                    Estudiante = est,
                    MatriculasConNotas = todasMatriculas
                        .Where(m => m.Estudiante.Identificacion == est.Identificacion &&
                                   m.Calificaciones.Count > 0)
                        .ToList()
                })
                .Where(x => x.MatriculasConNotas.Any()) // Solo estudiantes con calificaciones
                .Select(x => new
                {
                    x.Estudiante,
                    PromedioGeneral = x.MatriculasConNotas.Average(m => m.ObtenerPromedio())
                })
                .OrderByDescending(x => x.PromedioGeneral) // Ordenar de mayor a menor
                .Take(10) // Tomar los primeros 10
                .Select(x => (x.Estudiante, x.PromedioGeneral))
                .ToList();

            return estudiantesConPromedio;
        }

        /// <summary>
        /// Obtiene estudiantes con promedio menor a 7.0 (en riesgo)
        /// Aplica: LINQ, Where, Lambda expressions
        /// </summary>
        public static List<(Estudiante estudiante, decimal promedio, int cursosEnRiesgo)>
            ObtenerEstudiantesEnRiesgo(this GestorMatriculas gestor, Repositorio<Estudiante> repoEstudiantes)
        {
            var estudiantes = repoEstudiantes.ObtenerTodos();
            var todasMatriculas = gestor.ObtenerTodasLasMatriculas();

            var estudiantesEnRiesgo = estudiantes
                .Select(est => new
                {
                    Estudiante = est,
                    Matriculas = todasMatriculas
                        .Where(m => m.Estudiante.Identificacion == est.Identificacion &&
                                   m.Calificaciones.Count > 0)
                        .ToList()
                })
                .Where(x => x.Matriculas.Any()) // Solo con calificaciones
                .Select(x => new
                {
                    x.Estudiante,
                    PromedioGeneral = x.Matriculas.Average(m => m.ObtenerPromedio()),
                    CursosEnRiesgo = x.Matriculas.Count(m => m.ObtenerPromedio() < 7.0m)
                })
                .Where(x => x.PromedioGeneral < 7.0m) // Filtrar en riesgo
                .OrderBy(x => x.PromedioGeneral) // Los más bajos primero
                .Select(x => (x.Estudiante, x.PromedioGeneral, x.CursosEnRiesgo))
                .ToList();

            return estudiantesEnRiesgo;
        }

        /// <summary>
        /// Obtiene cursos ordenados por popularidad (cantidad de estudiantes)
        /// Aplica: GroupBy, OrderByDescending, Select
        /// </summary>
        public static List<(Curso curso, int cantidadEstudiantes, decimal promedioDelCurso)>
            ObtenerCursosMasPopulares(this GestorMatriculas gestor, Repositorio<Curso> repoCursos)
        {
            var todasMatriculas = gestor.ObtenerTodasLasMatriculas();

            var cursosPopulares = todasMatriculas
                .GroupBy(m => m.Curso.Identificacion) // Agrupar por curso
                .Select(grupo => new
                {
                    Curso = grupo.First().Curso,
                    CantidadEstudiantes = grupo.Count(),
                    PromedioDelCurso = grupo
                        .Where(m => m.Calificaciones.Count > 0)
                        .Select(m => m.ObtenerPromedio())
                        .DefaultIfEmpty(0)
                        .Average()
                })
                .OrderByDescending(x => x.CantidadEstudiantes) // Más populares primero
                .Select(x => (x.Curso, x.CantidadEstudiantes, x.PromedioDelCurso))
                .ToList();

            return cursosPopulares;
        }

        /// <summary>
        /// Calcula el promedio general de todos los estudiantes
        /// Aplica: SelectMany, Average
        /// </summary>
        public static decimal ObtenerPromedioGeneral(this GestorMatriculas gestor)
        {
            var todasMatriculas = gestor.ObtenerTodasLasMatriculas();

            var matriculasConNotas = todasMatriculas
                .Where(m => m.Calificaciones.Count > 0)
                .ToList();

            if (!matriculasConNotas.Any())
                return 0;

            // Promedio de todos los promedios
            return matriculasConNotas.Average(m => m.ObtenerPromedio());
        }

        /// <summary>
        /// Obtiene estadísticas agrupadas por carrera
        /// Aplica: GroupBy, Select, Aggregate functions
        /// </summary>
        public static List<EstadisticaCarrera> ObtenerEstadisticasPorCarrera(
            this GestorMatriculas gestor, Repositorio<Estudiante> repoEstudiantes)
        {
            var estudiantes = repoEstudiantes.ObtenerTodos();
            var todasMatriculas = gestor.ObtenerTodasLasMatriculas();

            var estadisticas = estudiantes
                .GroupBy(e => e.Carrera) // Agrupar por carrera
                .Select(grupo => new EstadisticaCarrera
                {
                    Carrera = grupo.Key,
                    CantidadEstudiantes = grupo.Count(),
                    PromedioGeneral = grupo
                        .Select(est => new
                        {
                            Estudiante = est,
                            Promedio = todasMatriculas
                                .Where(m => m.Estudiante.Identificacion == est.Identificacion &&
                                           m.Calificaciones.Count > 0)
                                .Select(m => m.ObtenerPromedio())
                                .DefaultIfEmpty(0)
                                .Average()
                        })
                        .Where(x => x.Promedio > 0)
                        .Select(x => x.Promedio)
                        .DefaultIfEmpty(0)
                        .Average(),
                    MejorEstudiante = grupo
                        .Select(est => new
                        {
                            Estudiante = est,
                            Promedio = todasMatriculas
                                .Where(m => m.Estudiante.Identificacion == est.Identificacion &&
                                           m.Calificaciones.Count > 0)
                                .Select(m => m.ObtenerPromedio())
                                .DefaultIfEmpty(0)
                                .Average()
                        })
                        .OrderByDescending(x => x.Promedio)
                        .FirstOrDefault()?.Estudiante,
                    TotalMatriculas = todasMatriculas
                        .Count(m => grupo.Any(e => e.Identificacion == m.Estudiante.Identificacion))
                })
                .OrderByDescending(e => e.CantidadEstudiantes)
                .ToList();

            return estadisticas;
        }

        /// <summary>
        /// Búsqueda flexible de estudiantes usando predicado personalizado
        /// Aplica: Func<T, bool>, Lambda expressions
        /// </summary>
        public static List<Estudiante> BuscarEstudiantes(
            this Repositorio<Estudiante> repositorio, Func<Estudiante, bool> criterio)
        {
            return repositorio.Buscar(criterio);
        }
    }

    /// <summary>
    /// Clase para almacenar estadísticas por carrera
    /// </summary>
    public class EstadisticaCarrera
    {
        public string Carrera { get; set; }
        public int CantidadEstudiantes { get; set; }
        public decimal PromedioGeneral { get; set; }
        public Estudiante MejorEstudiante { get; set; }
        public int TotalMatriculas { get; set; }

        public override string ToString()
        {
            string mejorEst = MejorEstudiante != null ?
                $"{MejorEstudiante.Nombre} {MejorEstudiante.Apellido}" :
                "N/A";

            return $"Carrera: {Carrera}\n" +
                   $"  Estudiantes: {CantidadEstudiantes}\n" +
                   $"  Promedio General: {PromedioGeneral:F2}\n" +
                   $"  Mejor Estudiante: {mejorEst}\n" +
                   $"  Total Matrículas: {TotalMatriculas}";
        }
    }

    /// <summary>
    /// Expresiones Lambda adicionales para filtrado y ordenamiento
    /// </summary>
    public static class ExpresionesLambdaAdicionales
    {
        // Lambda 1: Filtrar estudiantes por edad
        public static Func<Estudiante, bool> FiltrarPorEdad(int edadMinima, int edadMaxima) =>
            est => est.Edad >= edadMinima && est.Edad <= edadMaxima;

        // Lambda 2: Filtrar por carrera (case insensitive)
        public static Func<Estudiante, bool> FiltrarPorCarrera(string carrera) =>
            est => est.Carrera.ToLower().Contains(carrera.ToLower());

        // Lambda 3: Filtrar por letra inicial del apellido
        public static Func<Estudiante, bool> FiltrarPorInicialApellido(char inicial) =>
            est => est.Apellido.ToUpper().StartsWith(inicial.ToString().ToUpper());

        // Lambda 4: Ordenar por nombre completo
        public static Func<Estudiante, string> OrdenarPorNombreCompleto =>
            est => $"{est.Apellido}, {est.Nombre}";

        // Lambda 5: Filtrar profesores por salario
        public static Func<Profesor, bool> FiltrarPorRangoSalarial(decimal min, decimal max) =>
            prof => prof.SalarioBase >= min && prof.SalarioBase <= max;

        // Lambda 6: Filtrar profesores por tipo de contrato
        public static Func<Profesor, bool> FiltrarPorTipoContrato(TipoContrato tipo) =>
            prof => prof.TipoContrato == tipo;

        /// <summary>
        /// Demostración de uso de lambdas personalizadas
        /// </summary>
        public static void DemostrarExpresionesLambda(
            Repositorio<Estudiante> repoEstudiantes,
            Repositorio<Profesor> repoProfesores)
        {
            Console.WriteLine("\n═══ DEMOSTRACIÓN DE EXPRESIONES LAMBDA ═══\n");

            // Lambda 1: Estudiantes entre 18 y 25 años
            Console.WriteLine("🔹 Estudiantes entre 18 y 25 años:");
            var jovenes = repoEstudiantes.Buscar(FiltrarPorEdad(18, 25));
            Console.WriteLine($"   Encontrados: {jovenes.Count}");

            // Lambda 2: Estudiantes de ingeniería
            Console.WriteLine("\n🔹 Estudiantes de carreras de Ingeniería:");
            var ingenieros = repoEstudiantes.Buscar(FiltrarPorCarrera("ingeniería"));
            Console.WriteLine($"   Encontrados: {ingenieros.Count}");

            // Lambda 3: Estudiantes con apellido que inicia con 'G'
            Console.WriteLine("\n🔹 Estudiantes con apellido que inicia con 'G':");
            var conG = repoEstudiantes.Buscar(FiltrarPorInicialApellido('G'));
            Console.WriteLine($"   Encontrados: {conG.Count}");

            // Lambda 4: Ordenar estudiantes por nombre completo
            Console.WriteLine("\n🔹 Primeros 5 estudiantes ordenados alfabéticamente:");
            var ordenados = repoEstudiantes.ObtenerTodos()
                .OrderBy(OrdenarPorNombreCompleto)
                .Take(5);
            foreach (var est in ordenados)
            {
                Console.WriteLine($"   • {est.Apellido}, {est.Nombre}");
            }

            // Lambda 5: Profesores con salario entre 2000 y 5000
            Console.WriteLine("\n🔹 Profesores con salario entre $2000 y $5000:");
            var profRango = repoProfesores.Buscar(FiltrarPorRangoSalarial(2000, 5000));
            Console.WriteLine($"   Encontrados: {profRango.Count}");

            // Lambda 6: Profesores de tiempo completo
            Console.WriteLine("\n🔹 Profesores de tiempo completo:");
            var tiempoCompleto = repoProfesores.Buscar(
                FiltrarPorTipoContrato(TipoContrato.Tiempo_Completo));
            Console.WriteLine($"   Encontrados: {tiempoCompleto.Count}");

            // Lambda personalizada inline
            Console.WriteLine("\n🔹 Estudiantes con matrícula que contiene '001':");
            var conMatriculaEspecial = repoEstudiantes.Buscar(
                est => est.NumeroMatricula.Contains("001"));
            Console.WriteLine($"   Encontrados: {conMatriculaEspecial.Count}");
        }
    }
}
