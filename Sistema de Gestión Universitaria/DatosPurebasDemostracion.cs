using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_de_Gestión_Universitaria
{
   
    /// Clase para generar datos de prueba y demostrar funcionalidades
   
    public class GeneradorDatosPrueba
    {
        private static Random random = new Random();

        //
        /// Genera todos los datos de prueba del sistema
        //
        public static void GenerarDatosPrueba(
            Repositorio<Estudiante> repoEstudiantes,
            Repositorio<Profesor> repoProfesores,
            Repositorio<Curso> repoCursos,
            GestorMatriculas gestorMatriculas)
        {
            Console.WriteLine("\n╔═══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║         GENERANDO DATOS DE PRUEBA DEL SISTEMA            ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════╝\n");

            // 1. Generar Estudiantes
            Console.WriteLine("📋 Generando 15 estudiantes...");
            var estudiantes = GenerarEstudiantes(15);
            foreach (var est in estudiantes)
            {
                repoEstudiantes.Agregar(est);
            }
            Console.WriteLine($"   ✓ {estudiantes.Count} estudiantes agregados\n");

            // 2. Generar Profesores
            Console.WriteLine("👨‍🏫 Generando 5 profesores...");
            var profesores = GenerarProfesores(5);
            foreach (var prof in profesores)
            {
                repoProfesores.Agregar(prof);
            }
            Console.WriteLine($"   ✓ {profesores.Count} profesores agregados\n");

            // 3. Generar Cursos
            Console.WriteLine("📚 Generando 10 cursos...");
            var cursos = GenerarCursos(10, profesores);
            foreach (var curso in cursos)
            {
                repoCursos.Agregar(curso);
            }
            Console.WriteLine($"   ✓ {cursos.Count} cursos agregados\n");

            // 4. Generar Matrículas
            Console.WriteLine("📝 Generando 30 matrículas...");
            int matriculasCreadas = GenerarMatriculas(30, estudiantes, cursos, gestorMatriculas);
            Console.WriteLine($"   ✓ {matriculasCreadas} matrículas creadas\n");

            // 5. Registrar Calificaciones
            Console.WriteLine("📊 Registrando calificaciones (3-4 por matrícula)...");
            int calificacionesRegistradas = GenerarCalificaciones(gestorMatriculas);
            Console.WriteLine($"   ✓ {calificacionesRegistradas} calificaciones registradas\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine("✓ DATOS DE PRUEBA GENERADOS EXITOSAMENTE");
            Console.WriteLine("═══════════════════════════════════════════════════════════\n");
            Console.ResetColor();
        }

        //
        /// Genera una lista de estudiantes con datos aleatorios
        //
        private static List<Estudiante> GenerarEstudiantes(int cantidad)
        {
            var estudiantes = new List<Estudiante>();

            string[] nombres = { "Juan", "María", "Carlos", "Ana", "Luis", "Sofia",
                            "Pedro", "Laura", "Miguel", "Carmen", "Jorge", "Elena",
                            "Roberto", "Patricia", "Fernando", "Isabel", "Diego", "Rosa" };

            string[] apellidos = { "García", "Rodríguez", "Martínez", "López", "González",
                              "Pérez", "Sánchez", "Ramírez", "Torres", "Flores",
                              "Rivera", "Gómez", "Díaz", "Cruz", "Morales" };

            string[] carreras = { "Ingeniería en Software", "Medicina", "Derecho",
                             "Administración de Empresas", "Ingeniería Civil",
                             "Arquitectura", "Contabilidad", "Psicología" };

            for (int i = 0; i < cantidad; i++)
            {
                var estudiante = new Estudiante
                {
                    Identificacion = $"EST-{1000 + i}",
                    Nombre = nombres[random.Next(nombres.Length)],
                    Apellido = apellidos[random.Next(apellidos.Length)],
                    FechaNacimiento = DateTime.Now.AddYears(-random.Next(18, 30))
                                                .AddMonths(-random.Next(0, 12))
                                                .AddDays(-random.Next(0, 30)),
                    Carrera = carreras[random.Next(carreras.Length)],
                    NumeroMatricula = $"{random.Next(100, 999)}-{random.Next(10000, 99999)}"
                };

                estudiantes.Add(estudiante);
            }

            return estudiantes;
        }

        //
        /// Genera una lista de profesores con datos aleatorios
        //
        private static List<Profesor> GenerarProfesores(int cantidad)
        {
            var profesores = new List<Profesor>();

            string[] nombres = { "Alberto", "Beatriz", "César", "Diana", "Eduardo" };
            string[] apellidos = { "Vargas", "Mendoza", "Castro", "Ortiz", "Silva" };
            string[] departamentos = { "Ciencias de la Computación", "Matemáticas",
                                  "Física", "Química", "Ingeniería" };

            var tiposContrato = new[] { TipoContrato.Tiempo_Completo, TipoContrato.Medio_Tiempo,
                                   TipoContrato.Por_Horas, TipoContrato.Temporal };

            for (int i = 0; i < cantidad; i++)
            {
                var profesor = new Profesor
                {
                    Identificacion = $"PROF-{2000 + i}",
                    Nombre = nombres[i % nombres.Length],
                    Apellido = apellidos[i % apellidos.Length],
                    FechaNacimiento = DateTime.Now.AddYears(-random.Next(30, 60))
                                                .AddMonths(-random.Next(0, 12)),
                    Departamento = departamentos[random.Next(departamentos.Length)],
                    TipoContrato = tiposContrato[random.Next(tiposContrato.Length)],
                    SalarioBase = random.Next(1000, 8000)
                };

                profesores.Add(profesor);
            }

            return profesores;
        }

        //
        /// Genera una lista de cursos con profesores asignados
        //
        private static List<Curso> GenerarCursos(int cantidad, List<Profesor> profesores)
        {
            var cursos = new List<Curso>();

            string[] nombresCursos = {
            "Programación I", "Cálculo Diferencial", "Física General",
            "Química Orgánica", "Estructuras de Datos", "Base de Datos",
            "Álgebra Lineal", "Estadística", "Redes de Computadoras",
            "Inteligencia Artificial", "Arquitectura de Software",
            "Sistemas Operativos", "Contabilidad Financiera", "Marketing Digital"
        };

            for (int i = 0; i < cantidad; i++)
            {
                var curso = new Curso
                {
                    Identificacion = $"CUR{100 + i}",
                    Nombre = nombresCursos[i % nombresCursos.Length],
                    Creditos = random.Next(2, 5),
                    ProfesorAsignado = profesores[random.Next(profesores.Count)]
                };

                cursos.Add(curso);
            }

            return cursos;
        }

        //
        /// Genera matrículas aleatorias
        //
        private static int GenerarMatriculas(int cantidad, List<Estudiante> estudiantes,
                                            List<Curso> cursos, GestorMatriculas gestor)
        {
            int creadas = 0;
            int intentos = 0;
            int maxIntentos = cantidad * 3;

            while (creadas < cantidad && intentos < maxIntentos)
            {
                try
                {
                    var estudiante = estudiantes[random.Next(estudiantes.Count)];
                    var curso = cursos[random.Next(cursos.Count)];

                    gestor.MatricularEstudiante(estudiante, curso);
                    creadas++;
                }
                catch
                {
                    // Si el estudiante ya está matriculado en ese curso, intentar otro
                }
                intentos++;
            }

            return creadas;
        }

        //
        /// Genera calificaciones aleatorias para todas las matrículas
        //
        private static int GenerarCalificaciones(GestorMatriculas gestor)
        {
            int calificaciones = 0;
            var matriculas = gestor.ObtenerTodasLasMatriculas();

            foreach (var matricula in matriculas)
            {
                // Generar entre 3 y 4 calificaciones por matrícula
                int cantidadNotas = random.Next(3, 5);

                for (int i = 0; i < cantidadNotas; i++)
                {
                    // Generar calificaciones con distribución realista
                    // 70% entre 7-10, 30% entre 5-7
                    decimal nota = random.NextDouble() < 0.7 ?
                        (decimal)(7.0 + random.NextDouble() * 3.0) :
                        (decimal)(5.0 + random.NextDouble() * 2.0);

                    nota = Math.Round(nota, 2);

                    try
                    {
                        gestor.AgregarCalificacion(
                            matricula.Estudiante.Identificacion,
                            matricula.Curso.Identificacion,
                            nota);
                        calificaciones++;
                    }
                    catch { }
                }
            }

            return calificaciones;
        }


        //
        /// Demuestra todas las funcionalidades implementadas
        //
        public static void DemostrarFuncionalidades(
            Repositorio<Estudiante> repoEstudiantes,
            Repositorio<Profesor> repoProfesores,
            Repositorio<Curso> repoCursos,
            GestorMatriculas gestorMatriculas)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║      DEMOSTRACIÓN DE FUNCIONALIDADES DEL SISTEMA         ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════╝\n");

            // ═══════════════════════════════════════════════════════════
            // 1. CONSULTAS LINQ
            // ═══════════════════════════════════════════════════════════

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("1️⃣  CONSULTAS LINQ AVANZADAS");
            Console.WriteLine(new string('═', 60));
            Console.ResetColor();

            // Top 10 Estudiantes
            Console.WriteLine("\n🏆 TOP 10 ESTUDIANTES CON MEJOR PROMEDIO:");
            var top10 = gestorMatriculas.ObtenerTop10Estudiantes(repoEstudiantes);
            for (int i = 0; i < Math.Min(5, top10.Count); i++)
            {
                var (est, prom) = top10[i];
                Console.WriteLine($"   {i + 1}. {est.Nombre} {est.Apellido} - " +
                                $"Promedio: {prom:F2} - Carrera: {est.Carrera}");
            }

            // Estudiantes en Riesgo
            Console.WriteLine("\n⚠️  ESTUDIANTES EN RIESGO (Promedio < 7.0):");
            var enRiesgo = gestorMatriculas.ObtenerEstudiantesEnRiesgo(repoEstudiantes);
            foreach (var (est, prom, cursosRiesgo) in enRiesgo.Take(5))
            {
                Console.WriteLine($"   • {est.Nombre} {est.Apellido} - " +
                                $"Promedio: {prom:F2} - Cursos en riesgo: {cursosRiesgo}");
            }

            // Cursos Más Populares
            Console.WriteLine("\n📚 CURSOS MÁS POPULARES:");
            var populares = gestorMatriculas.ObtenerCursosMasPopulares(repoCursos);
            foreach (var (curso, cant, prom) in populares.Take(5))
            {
                Console.WriteLine($"   • {curso.Nombre} - Estudiantes: {cant} - " +
                                $"Promedio del curso: {prom:F2}");
            }

            // Promedio General
            Console.WriteLine($"\n📊 PROMEDIO GENERAL DEL SISTEMA: " +
                             $"{gestorMatriculas.ObtenerPromedioGeneral():F2}");

            // Estadísticas por Carrera
            Console.WriteLine("\n🎓 ESTADÍSTICAS POR CARRERA:");
            var estadisticas = gestorMatriculas.ObtenerEstadisticasPorCarrera(repoEstudiantes);
            foreach (var stat in estadisticas.Take(5))
            {
                Console.WriteLine($"\n   {stat.Carrera}");
                Console.WriteLine($"      Estudiantes: {stat.CantidadEstudiantes}");
                Console.WriteLine($"      Promedio: {stat.PromedioGeneral:F2}");
                Console.WriteLine($"      Matrículas: {stat.TotalMatriculas}");
            }

            PausarDemostracion();

            // ═══════════════════════════════════════════════════════════
            // 2. EXPRESIONES LAMBDA
            // ═══════════════════════════════════════════════════════════

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("2️⃣  EXPRESIONES LAMBDA PERSONALIZADAS");
            Console.WriteLine(new string('═', 60));
            Console.ResetColor();

            ExpresionesLambdaAdicionales.DemostrarExpresionesLambda(
                repoEstudiantes, repoProfesores);

            PausarDemostracion();

            // ═══════════════════════════════════════════════════════════
            // 3. REFLECTION
            // ═══════════════════════════════════════════════════════════

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("3️⃣  ANÁLISIS CON REFLECTION");
            Console.WriteLine(new string('═', 60));
            Console.ResetColor();

            // Analizar clase Estudiante
            AnalizadorReflection.AnalizarClaseCompleta(typeof(Estudiante));

            // Analizar clase Profesor
            AnalizadorReflection.AnalizarClaseCompleta(typeof(Profesor));

            // Crear instancia dinámica
            Console.WriteLine("\n🔧 CREACIÓN DINÁMICA DE INSTANCIA:");
            var estudianteDinamico = AnalizadorReflection.CrearInstanciaDinamica(
                typeof(Estudiante));

            // Invocar método dinámicamente
            if (estudianteDinamico != null)
            {
                Console.WriteLine("   Invocando método ObtenerRol()...");
                var rol = AnalizadorReflection.InvocarMetodo(
                    estudianteDinamico, "ObtenerRol");
                Console.WriteLine($"   Resultado: {rol}");
            }

            PausarDemostracion();

            // ═══════════════════════════════════════════════════════════
            // 4. ATRIBUTOS PERSONALIZADOS
            // ═══════════════════════════════════════════════════════════

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("4️⃣  VALIDACIÓN CON ATRIBUTOS PERSONALIZADOS");
            Console.WriteLine(new string('═', 60));
            Console.ResetColor();

            // Mostrar atributos de las clases
            Validador.MostrarAtributosValidacion(typeof(Estudiante));
            Validador.MostrarAtributosValidacion(typeof(Profesor));

            // Ejemplo de validación
            Console.WriteLine("\n✓ VALIDANDO OBJETOS:");

            var profPrueba = new Profesor
            {
                Identificacion = "PROF-TEST",
                Nombre = "Test",
                Apellido = "Profesor",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                Departamento = "Informática",
                SalarioBase = 3000,
                TipoContrato = TipoContrato.Tiempo_Completo
            };

            var errores = Validador.ValidarObjeto(profPrueba);
            if (errores.Count == 0)
            {
                Console.WriteLine("   ✓ Objeto válido: Sin errores de validación");
            }
            else
            {
                Console.WriteLine("   ✗ Errores encontrados:");
                foreach (var error in errores)
                {
                    Console.WriteLine($"      - {error}");
                }
            }

            PausarDemostracion();

            // ═══════════════════════════════════════════════════════════
            // 5. BOXING/UNBOXING Y CONVERSIONES
            // ═══════════════════════════════════════════════════════════

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("5️⃣  BOXING, UNBOXING Y CONVERSIONES DE TIPOS");
            Console.WriteLine(new string('═', 60));
            Console.ResetColor();

            TiposEspeciales.DemostrarBoxingUnboxing();

            // Demostrar Pattern Matching
            Console.WriteLine("\n🔄 PATTERN MATCHING CON DIFERENTES TIPOS:");
            object[] datosDiversos = {
            100,
            9.75m,
            "Universidad 2025",
            DateTime.Now,
            true
        };

            foreach (var dato in datosDiversos)
            {
                Console.WriteLine($"   {TiposEspeciales.ConvertirDatos(dato)}");
            }

            // Demostrar TryParse
            Console.WriteLine("\n🔢 PARSEO SEGURO DE CALIFICACIONES:");
            string[] entradasPrueba = { "8.5", "10.2", "abc", "9.0" };

            foreach (var entrada in entradasPrueba)
            {
                var (exito, valor, mensaje) =
                    TiposEspeciales.ParsearCalificacion(entrada);

                string resultado = exito ?
                    $"✓ '{entrada}' → {valor:F2}" :
                    $"✗ '{entrada}' → {mensaje}";
                Console.WriteLine($"   {resultado}");
            }

            PausarDemostracion();

            // ═══════════════════════════════════════════════════════════
            // RESUMEN FINAL
            // ═══════════════════════════════════════════════════════════

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("✓ DEMOSTRACIÓN COMPLETADA EXITOSAMENTE");
            Console.WriteLine(new string('═', 60));
            Console.WriteLine("\nTodas las funcionalidades del sistema han sido demostradas:");
            Console.WriteLine("  ✓ Consultas LINQ avanzadas");
            Console.WriteLine("  ✓ Expresiones Lambda personalizadas");
            Console.WriteLine("  ✓ Reflection y creación dinámica");
            Console.WriteLine("  ✓ Atributos personalizados y validación");
            Console.WriteLine("  ✓ Boxing, Unboxing y conversiones");
            Console.ResetColor();

            Console.WriteLine("\n\nPresione Enter para volver al menú principal...");
            Console.ReadLine();
        }

        private static void PausarDemostracion()
        {
            Console.WriteLine("\n[Presione Enter para continuar...]");
            Console.ReadLine();
        }
    }
}
