using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_de_Gestión_Universitaria
{
    public class GestorMatriculas
    {
        private List<Matricula> matriculas;
        private Repositorio<Estudiante> repositorioEstudiantes;
        private Repositorio<Curso> repositorioCursos;

        public GestorMatriculas(Repositorio<Estudiante> repoEstudiantes, Repositorio<Curso> repoCursos)
        {
            matriculas = new List<Matricula>();
            repositorioEstudiantes = repoEstudiantes ??
                throw new ArgumentNullException(nameof(repoEstudiantes));
            repositorioCursos = repoCursos ??
                throw new ArgumentNullException(nameof(repoCursos));
        }

    
    
        //
        //TODO Matricula un estudiante en un curso
        //
        public Matricula MatricularEstudiante(Estudiante estudiante, Curso curso)
        {
            if (estudiante == null)
                throw new ArgumentNullException(nameof(estudiante));
            if (curso == null)
                throw new ArgumentNullException(nameof(curso));

            // Validar que el estudiante no esté ya matriculado en el curso
            bool yaMatriculado = matriculas.Any(m =>
                m.Estudiante.Identificacion == estudiante.Identificacion &&
                m.Curso.Identificacion == curso.Identificacion);

            if (yaMatriculado)
                throw new InvalidOperationException(
                    $"El estudiante {estudiante.Nombre} {estudiante.Apellido} " +
                    $"ya está matriculado en el curso {curso.Nombre}");

            var matricula = new Matricula(estudiante, curso);
            matriculas.Add(matricula);

            return matricula;
        }

        //
        //TODO Agrega una calificación a una matrícula específica
        //
        public void AgregarCalificacion(string idEstudiante, string IDCurso, decimal calificacion)
        {
            if (string.IsNullOrWhiteSpace(idEstudiante))
                throw new ArgumentException("El ID del estudiante no puede estar vacío");
            if (string.IsNullOrWhiteSpace(IDCurso))
                throw new ArgumentException("El código del curso no puede estar vacío");

            // Validar rango de calificación
            if (calificacion < 0 || calificacion > 10)
                throw new ArgumentException("La calificación debe estar entre 0 y 10");

            // Buscar la matrícula usando LINQ
            var matricula = matriculas.FirstOrDefault(m =>
                m.Estudiante.Identificacion == idEstudiante &&
                m.Curso.Identificacion == IDCurso);

            if (matricula == null)
                throw new InvalidOperationException(
                    "No se encontró una matrícula con los datos proporcionados");

            matricula.AgregarCalificacion(calificacion);
        }

        //
        //TODO Obtiene todas las matrículas de un estudiante
        //
        public List<Matricula> ObtenerMatriculasPorEstudiante(string idEstudiante)
        {
            if (string.IsNullOrWhiteSpace(idEstudiante))
                throw new ArgumentException("El ID del estudiante no puede estar vacío");

            // Uso de LINQ para filtrar
            return matriculas.Where(m => m.Estudiante.Identificacion == idEstudiante).ToList();
        }

        //
        //TODO Obtiene todos los estudiantes matriculados en un curso
        //
        public List<Estudiante> ObtenerEstudiantesPorCurso(string codigoCurso)
        {
            if (string.IsNullOrWhiteSpace(codigoCurso))
                throw new ArgumentException("El código del curso no puede estar vacío");

            // Uso de LINQ para filtrar y proyectar
            return matriculas
                .Where(m => m.Curso.Identificacion == codigoCurso)
                .Select(m => m.Estudiante)
                .Distinct()
                .ToList();
        }

        //
        //TODO Genera un reporte detallado de un estudiante
        //
        public string GenerarReporteEstudiante(string idEstudiante)
        {
            if (string.IsNullOrWhiteSpace(idEstudiante))
                throw new ArgumentException("El ID del estudiante no puede estar vacío");

            var estudiante = repositorioEstudiantes.BuscarPorId(idEstudiante);
            if (estudiante == null)
                throw new InvalidOperationException($"No se encontró estudiante con ID: {idEstudiante}");

            var matriculasEstudiante = ObtenerMatriculasPorEstudiante(idEstudiante);

            var sb = new StringBuilder();
            sb.AppendLine("╔════════════════════════════════════════════════════════════╗");
            sb.AppendLine("║           REPORTE ACADÉMICO DEL ESTUDIANTE                 ║");
            sb.AppendLine("╠════════════════════════════════════════════════════════════╣");
            sb.AppendLine($"  Nombre: {estudiante.Nombre} {estudiante.Apellido}");
            sb.AppendLine($"  Identificación: {estudiante.Identificacion}");
            sb.AppendLine($"  Carrera: {estudiante.Carrera}");
            sb.AppendLine($"  Matrícula: {estudiante.NumeroMatricula}");
            sb.AppendLine("╠════════════════════════════════════════════════════════════╣");
            sb.AppendLine("  CURSOS MATRICULADOS:");
            sb.AppendLine("────────────────────────────────────────────────────────────");

            if (matriculasEstudiante.Count == 0)
            {
                sb.AppendLine("  No hay cursos matriculados");
            }
            else
            {
                decimal sumaPromedios = 0;
                int cursosConCalificaciones = 0;

                foreach (var mat in matriculasEstudiante)
                {
                    sb.AppendLine($"\n  📚 {mat.Curso.Nombre} ({mat.Curso.Identificacion})");
                    sb.AppendLine($"     Créditos: {mat.Curso.Creditos}");

                    if (mat.Calificaciones.Count > 0)
                    {
                        sb.AppendLine($"     Calificaciones: {string.Join(", ", mat.Calificaciones.Select(c => c.ToString("F2")))}");
                        sb.AppendLine($"     Promedio: {mat.ObtenerPromedio():F2}");
                        sb.AppendLine($"     Estado: {mat.ObtenerEstado()}");

                        sumaPromedios += mat.ObtenerPromedio();
                        cursosConCalificaciones++;
                    }
                    else
                    {
                        sb.AppendLine("     Sin calificaciones registradas");
                    }
                }

                sb.AppendLine("\n────────────────────────────────────────────────────────────");
                if (cursosConCalificaciones > 0)
                {
                    decimal promedioGeneral = sumaPromedios / cursosConCalificaciones;
                    sb.AppendLine($"  PROMEDIO GENERAL: {promedioGeneral:F2}");

                    int aprobados = matriculasEstudiante.Count(m => m.HaAprobado());
                    int reprobados = matriculasEstudiante.Count(m =>
                        m.Calificaciones.Count > 0 && !m.HaAprobado());
                    int enCurso = matriculasEstudiante.Count(m => m.Calificaciones.Count == 0);

                    sb.AppendLine($"  Cursos Aprobados: {aprobados}");
                    sb.AppendLine($"  Cursos Reprobados: {reprobados}");
                    sb.AppendLine($"  Cursos en Curso: {enCurso}");
                }
            }

            sb.AppendLine("╚════════════════════════════════════════════════════════════╝");
            return sb.ToString();
        }

        //
        //TODO Obtiene todas las matrículas del sistema
        //
        public List<Matricula> ObtenerTodasLasMatriculas()
        {
            return matriculas.ToList();
        }

        //
        //TODO Obtiene el número total de matrículas
        //
        public int ObtenerTotalMatriculas()
        {
            return matriculas.Count;
        }
    }
}
