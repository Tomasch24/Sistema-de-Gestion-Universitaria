using System;

namespace Sistema_de_Gestión_Universitaria
{
   
    //
    //TODO Generador de identificaciones únicas para el sistema
    //
    public static class GeneradorIdentificaciones
    {
        private static int contadorEstudiantes = 1000;
        private static int contadorProfesores = 2000;
        private static int contadorCursos = 100;
        private static Random random = new Random();

        //
        //TODO Genera una identificación única para estudiante (formato: EST-XXXX)
        //
        public static string GenerarIdEstudiante()
        {
            return $"EST-{contadorEstudiantes++:D4}";
        }

        //
        //TODO Genera una identificación única para profesor (formato: PROF-XXXX)
        //
        public static string GenerarIdProfesor()
        {
            return $"PROF-{contadorProfesores++:D4}";
        }

        //
        //TODO Genera un código único para curso (formato: CURXXX)
        //
        public static string GenerarCodigoCurso()
        {
            return $"CUR{contadorCursos++:D3}";
        }

        //
        //TODO Genera un número de matrícula con formato XXX-XXXXX
        // Basado en año actual y contador secuencial
        //
        public static string GenerarNumeroMatricula()
        {
            int año = DateTime.Now.Year % 100; // Últimos 2 dígitos del año
            int secuencial = contadorEstudiantes - 1000; // Número secuencial del estudiante

            // Formato: AÑO-SECUENCIAL (ejemplo: 25-00001 para año 2025)
            return $"{año:D2}{random.Next(1, 10)}-{secuencial:D5}";
        }

        //
        //TODO Reinicia los contadores (útil para testing)
        //
        public static void ReiniciarContadores()
        {
            contadorEstudiantes = 1000;
            contadorProfesores = 2000;
            contadorCursos = 100;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var sistema = new SistemaUniversitarioCompleto();
            sistema.Iniciar();
        }
    }

    //
    //TODO Sistema completo con todas las funcionalidades integradas
    //
    public class SistemaUniversitarioCompleto
    {
        private Repositorio<Estudiante> repoEstudiantes;
        private Repositorio<Profesor> repoProfesores;
        private Repositorio<Curso> repoCursos;
        private GestorMatriculas gestorMatriculas;

        public SistemaUniversitarioCompleto()
        {
            repoEstudiantes = new Repositorio<Estudiante>();
            repoProfesores = new Repositorio<Profesor>();
            repoCursos = new Repositorio<Curso>();
            gestorMatriculas = new GestorMatriculas(repoEstudiantes, repoCursos);
        }

        public void Iniciar()
        {
            MostrarBienvenida();

            bool continuar = true;
            while (continuar)
            {
                try
                {
                    MostrarMenuPrincipal();
                    string opcion = Console.ReadLine();
                    continuar = ProcesarOpcionPrincipal(opcion);
                }
                catch (Exception ex)
                {
                    MostrarError($"Error: {ex.Message}");
                    Pausar();
                }
            }

            MostrarDespedida();
        }

        private void MostrarBienvenida()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║              SISTEMA DE GESTIÓN UNIVERSITARIA              ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║                Universidad Technologia 2025                ║");
            Console.WriteLine("║                     Tarea Practica C#                      ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine("\n   Implementa: Herencia, Polimorfismo, Interfaces,");
            Console.WriteLine("   Genéricos, LINQ, Reflection y Atributos Personalizados");
            Console.WriteLine("\nPresione Enter para continuar...");
            Console.ReadLine();
        }

        private void MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n═══════════════ MENÚ PRINCIPAL ═══════════════");
            Console.ResetColor();

            Console.WriteLine("\n1.  🧑‍🎓 Gestionar Estudiantes");
            Console.WriteLine("2.  👨‍🏫 Gestionar Profesores");
            Console.WriteLine("3.  📚 Gestionar Cursos");
            Console.WriteLine("4.  📝 Matricular Estudiante en Curso");
            Console.WriteLine("5.  📊 Registrar Calificaciones");
            Console.WriteLine("6.  📈 Ver Reportes");
            Console.WriteLine("7.  🔍 Análisis con Reflection");
            Console.WriteLine("8.  🧪 Demostrar Funcionalidades Avanzadas");
            Console.WriteLine("9.  🚀 Generar Datos de Prueba");
            Console.WriteLine("0.  ❌ Salir");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n Seleccione una opción: ");
            Console.ResetColor();
        }

        private bool ProcesarOpcionPrincipal(string opcion)
        {
            switch (opcion)
            {
                case "1": return MenuEstudiantes();
                case "2": return MenuProfesores();
                case "3": return MenuCursos();
                case "4": return MatricularEstudiante();
                case "5": return RegistrarCalificaciones();
                case "6": return MenuReportes();
                case "7": return MenuReflection();
                case "8":
                    GeneradorDatosPrueba.DemostrarFuncionalidades(
                        repoEstudiantes, repoProfesores, repoCursos, gestorMatriculas);
                    return true;
                case "9":
                    GeneradorDatosPrueba.GenerarDatosPrueba(
                        repoEstudiantes, repoProfesores, repoCursos, gestorMatriculas);
                    Pausar();
                    return true;
                case "0": return false;
                default:
                    MostrarError("Opción no válida");
                    Pausar();
                    return true;
            }
        }

    
        //TODO GESTIÓN DE ESTUDIANTES
       

        private bool MenuEstudiantes()
        {
            Console.Clear();
            Console.WriteLine("\n═══ GESTIÓN DE ESTUDIANTES ═══\n");
            Console.WriteLine("1. Agregar Estudiante");
            Console.WriteLine("2. Listar Estudiantes");
            Console.WriteLine("3. Buscar Estudiante");
            Console.WriteLine("4. Modificar Estudiante");
            Console.WriteLine("5. Eliminar Estudiante");
            Console.WriteLine("0. Volver");

            Console.Write("\n▶ Opción: ");
            string opcion = Console.ReadLine();

            try
            {
                switch (opcion)
                {
                    case "1": AgregarEstudiante(); break;
                    case "2": ListarEstudiantes(); break;
                    case "3": BuscarEstudiante(); break;
                    case "4": ModificarEstudiante(); break;
                    case "5": EliminarEstudiante(); break;
                    case "0": return true;
                    default:
                        MostrarError("Opción no válida");
                        Pausar();
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error: {ex.Message}");
                Pausar();
            }

            return true;
        }

        private void AgregarEstudiante()
        {
            Console.Clear();
            Console.WriteLine("\n═══ AGREGAR ESTUDIANTE ═══\n");
            
            try
            {
                var estudiante = new Estudiante();
               
                estudiante.Nombre = LeerTextoRequerido("Nombre: ");
                estudiante.Apellido = LeerTextoRequerido("Apellido: ");
                estudiante.FechaNacimiento = LeerFecha("Fecha de Nacimiento (mm/dd/yyyy): ");
                estudiante.Carrera = LeerTextoRequerido("Carrera: ");
                

                estudiante.ValidarEdad();
                repoEstudiantes.Agregar(estudiante);

                MostrarExito($"✓ Estudiante {estudiante.Nombre} {estudiante.Apellido} agregado exitosamente");
            }
            catch (Exception ex)
            {
                MostrarError($"Error al agregar estudiante: {ex.Message}");
            }

            Pausar();
        }

        private void ListarEstudiantes()
        {
            Console.Clear();
            Console.WriteLine("\n═══ LISTA DE ESTUDIANTES ═══\n");

            var estudiantes = repoEstudiantes.ObtenerTodos();

            if (estudiantes.Count == 0)
            {
                Console.WriteLine("No hay estudiantes registrados.");
            }
            else
            {
                Console.WriteLine($"Total: {estudiantes.Count} estudiante(s)\n");
                var ordenados = estudiantes.OrderBy(e => e.Carrera)
                                          .ThenBy(e => e.Apellido);

                string carreraActual = "";
                foreach (var est in ordenados)
                {
                    if (est.Carrera != carreraActual)
                    {
                        carreraActual = est.Carrera;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\n📚 {carreraActual}:");
                        Console.ResetColor();
                    }
                    Console.WriteLine($"  • {est.Nombre} {est.Apellido} - ID: {est.Identificacion} - Edad: {est.Edad}");
                }
            }

            Pausar();
        }

        private void BuscarEstudiante()
        {
            Console.Clear();
            Console.WriteLine("\n═══ BUSCAR ESTUDIANTE ═══\n");
            Console.WriteLine("1. Por Identificación");
            Console.WriteLine("2. Por Nombre/Apellido");
            Console.WriteLine("3. Por Carrera");

            Console.Write("\n▶ Opción: ");
            string opcion = Console.ReadLine();

            List<Estudiante> resultados = new List<Estudiante>();

            try
            {
                switch (opcion)
                {
                    case "1":
                        string id = LeerTextoRequerido("Identificación: ");
                        var est = repoEstudiantes.BuscarPorId(id);
                        if (est != null) resultados.Add(est);
                        break;

                    case "2":
                        string texto = LeerTextoRequerido("Nombre o Apellido: ").ToLower();
                        resultados = repoEstudiantes.Buscar(e =>
                            e.Nombre.ToLower().Contains(texto) ||
                            e.Apellido.ToLower().Contains(texto));
                        break;

                    case "3":
                        string carrera = LeerTextoRequerido("Carrera: ").ToLower();
                        resultados = repoEstudiantes.Buscar(e =>
                            e.Carrera.ToLower().Contains(carrera));
                        break;
                }

                Console.WriteLine($"\n✓ Resultados encontrados: {resultados.Count}");
                foreach (var e in resultados)
                {
                    Console.WriteLine($"  • {e}");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error en búsqueda: {ex.Message}");
            }

            Pausar();
        }

        private void ModificarEstudiante()
        {
            Console.Clear();
            Console.WriteLine("\n═══ MODIFICAR ESTUDIANTE ═══\n");

            try
            {
                string id = LeerTextoRequerido("Identificación del estudiante: ");
                var estudiante = repoEstudiantes.BuscarPorId(id);

                if (estudiante == null)
                {
                    MostrarError("Estudiante no encontrado");
                    Pausar();
                    return;
                }

                Console.WriteLine($"\n✓ Estudiante encontrado: {estudiante}");
                Console.WriteLine("\nIngrese los nuevos datos (Enter para mantener):");

                string nuevoNombre = LeerTextoOpcional("Nombre: ");
                if (!string.IsNullOrWhiteSpace(nuevoNombre))
                    estudiante.Nombre = nuevoNombre;

                string nuevoApellido = LeerTextoOpcional("Apellido: ");
                if (!string.IsNullOrWhiteSpace(nuevoApellido))
                    estudiante.Apellido = nuevoApellido;

                string nuevaCarrera = LeerTextoOpcional("Carrera: ");
                if (!string.IsNullOrWhiteSpace(nuevaCarrera))
                    estudiante.Carrera = nuevaCarrera;

                repoEstudiantes.Actualizar(estudiante);
                MostrarExito("✓ Estudiante modificado exitosamente");
            }
            catch (Exception ex)
            {
                MostrarError($"Error al modificar: {ex.Message}");
            }

            Pausar();
        }

        private void EliminarEstudiante()
        {
            Console.Clear();
            Console.WriteLine("\n═══ ELIMINAR ESTUDIANTE ═══\n");

            try
            {
                string id = LeerTextoRequerido("Identificación del estudiante: ");
                var estudiante = repoEstudiantes.BuscarPorId(id);

                if (estudiante == null)
                {
                    MostrarError("Estudiante no encontrado");
                    Pausar();
                    return;
                }

                Console.WriteLine($"\n✓ Estudiante: {estudiante}");
                Console.Write("\n¿Está seguro de eliminar? (S/N): ");

                if (Console.ReadLine().ToUpper() == "S")
                {
                    repoEstudiantes.Eliminar(id);
                    MostrarExito("✓ Estudiante eliminado exitosamente");
                }
                else
                {
                    Console.WriteLine("Operación cancelada");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al eliminar: {ex.Message}");
            }

            Pausar();
        }

       
        //TODO GESTIÓN DE PROFESORES
      

        private bool MenuProfesores()
        {
            Console.Clear();
            Console.WriteLine("\n═══ GESTIÓN DE PROFESORES ═══\n");
            Console.WriteLine("1. Agregar Profesor");
            Console.WriteLine("2. Listar Profesores");
            Console.WriteLine("3. Buscar Profesor");
            Console.WriteLine("4. Eliminar Profesor");
            Console.WriteLine("0. Volver");

            Console.Write("\n▶ Opción: ");
            string opcion = Console.ReadLine();

            try
            {
                switch (opcion)
                {
                    case "1": AgregarProfesor(); break;
                    case "2": ListarProfesores(); break;
                    case "3": BuscarProfesor(); break;
                    case "4": EliminarProfesor(); break;
                    case "0": return true;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error: {ex.Message}");
                Pausar();
            }

            return true;
        }

        private void AgregarProfesor()
        {
            Console.Clear();
            Console.WriteLine("\n═══ AGREGAR PROFESOR ═══\n");

            try
            {
                var profesor = new Profesor();
                profesor.Nombre = LeerTextoRequerido("Nombre: ");
                profesor.Apellido = LeerTextoRequerido("Apellido: ");
                profesor.FechaNacimiento = LeerFecha("Fecha de Nacimiento: ");
                profesor.Departamento = LeerTextoRequerido("Departamento: ");
                profesor.SalarioBase = LeerDecimal("Salario Base: ");

                Console.WriteLine("\nTipo de Contrato:");
                Console.WriteLine("1. Tiempo Completo");
                Console.WriteLine("2. Medio Tiempo");
                Console.WriteLine("3. Por Horas");
                Console.WriteLine("4. Temporal");
                Console.Write("▶ Opción: ");

                profesor.TipoContrato = Console.ReadLine() switch
                {
                    "1" => TipoContrato.Tiempo_Completo,
                    "2" => TipoContrato.Medio_Tiempo,
                    "3" => TipoContrato.Por_Horas,
                    _ => TipoContrato.Temporal
                };

                profesor.ValidarEdad();
                repoProfesores.Agregar(profesor);

                MostrarExito($"✓ Profesor {profesor.Nombre} {profesor.Apellido} agregado exitosamente");
            }
            catch (Exception ex)
            {
                MostrarError($"Error al agregar profesor: {ex.Message}");
            }

            Pausar();
        }

        private void ListarProfesores()
        {
            Console.Clear();
            Console.WriteLine("\n═══ LISTA DE PROFESORES ═══\n");

            var profesores = repoProfesores.ObtenerTodos();

            if (profesores.Count == 0)
            {
                Console.WriteLine("No hay profesores registrados.");
            }
            else
            {
                Console.WriteLine($"Total: {profesores.Count} profesor(es)\n");
                foreach (var prof in profesores.OrderBy(p => p.Departamento).ThenBy(p => p.Apellido))
                {
                    Console.WriteLine($"• {prof}");
                }
            }

            Pausar();
        }

        private void BuscarProfesor()
        {
            Console.Clear();
            Console.WriteLine("\n═══ BUSCAR PROFESOR ═══\n");

            string id = LeerTextoRequerido("Identificación: ");
            var profesor = repoProfesores.BuscarPorId(id);

            if (profesor == null)
            {
                MostrarError("Profesor no encontrado");
            }
            else
            {
                Console.WriteLine($"\n✓ {profesor}");
            }

            Pausar();
        }
        private void EliminarProfesor()
        {
            Console.Clear();
            Console.WriteLine("\n═══ ELIMINAR PROFESOR ═══\n");

            try
            {
                string id = LeerTextoRequerido("Identificación del Profesor: ");
                var profesor = repoProfesores.BuscarPorId(id);

                if (profesor == null)
                {
                    MostrarError("Profesor no encontrado");
                    Pausar();
                    return;
                }

                Console.WriteLine($"\n✓ Profesor: {profesor}");
                Console.Write("\n¿Está seguro de eliminar? (S/N): ");

                if (Console.ReadLine().ToUpper() == "S")
                {
                    repoProfesores.Eliminar(id);
                    MostrarExito("✓ Profesor eliminado exitosamente");
                }
                else
                {
                    Console.WriteLine("Operación cancelada");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al eliminar: {ex.Message}");
            }

            Pausar();
        }


        //TODO GESTIÓN DE CURSOS
        private bool MenuCursos()
        {
            Console.Clear();
            Console.WriteLine("\n═══ GESTIÓN DE CURSOS ═══\n");
            Console.WriteLine("1. Agregar Curso");
            Console.WriteLine("2. Listar Cursos");
            Console.WriteLine("3. Buscar Curso");
            Console.WriteLine("4. Modificar Curso");
            Console.WriteLine("5. Eliminar Curso");
            Console.WriteLine("6. Asignar Profesor a Curso");
            Console.WriteLine("0. Volver");

            Console.Write("\n▶ Opción: ");
            string opcion = Console.ReadLine();

            try
            {
                switch (opcion)
                {
                    case "1": AgregarCurso(); break;
                    case "2": ListarCursos(); break;
                    case "3": BuscarCurso(); break;
                    case "4": ModificarCurso(); break;
                    case "5": EliminarCurso(); break;
                    case "6": AsignarProfesor(); break;
                    case "0": return true;
                    default:
                        MostrarError("Opción no válida");
                        Pausar();
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error: {ex.Message}");
                Pausar();
            }

            return true;
        }

        private void AgregarCurso()
        {
            Console.Clear();
            Console.WriteLine("\n═══ AGREGAR CURSO ═══\n");

            try
            {
                var curso = new Curso();
              
                curso.Nombre = LeerTextoRequerido("Nombre del Curso: ");
                curso.Creditos = (int)LeerDecimal("Créditos (1-5): ");

                // Validar rango de créditos
                if (curso.Creditos < 1 || curso.Creditos > 5)
                {
                    MostrarError("Los créditos deben estar entre 1 y 5");
                    Pausar();
                    return;
                }

                // Opcionalmente asignar profesor
                Console.Write("\n¿Desea asignar un profesor ahora? (S/N): ");
                if (Console.ReadLine()?.ToUpper() == "S")
                {
                    var profesores = repoProfesores.ObtenerTodos();

                    if (profesores.Count == 0)
                    {
                        Console.WriteLine("\n⚠ No hay profesores registrados en el sistema.");
                    }
                    else
                    {
                        Console.WriteLine("\n--- Profesores Disponibles ---");
                        foreach (var prof in profesores.OrderBy(p => p.Apellido))
                        {
                            Console.WriteLine($"  • {prof.Identificacion} - {prof.Nombre} {prof.Apellido} ({prof.Departamento})");
                        }

                        Console.WriteLine();
                        string idProf = LeerTextoRequerido("ID del Profesor: ");
                        var profesor = repoProfesores.BuscarPorId(idProf);

                        if (profesor != null)
                        {
                            curso.ProfesorAsignado = profesor;
                            Console.WriteLine($"✓ Profesor {profesor.Nombre} {profesor.Apellido} asignado");
                        }
                        else
                        {
                            Console.WriteLine("⚠ Profesor no encontrado. Curso creado sin profesor.");
                        }
                    }
                }

                repoCursos.Agregar(curso);

                MostrarExito($"✓ Curso {curso.Nombre} agregado exitosamente");
                Console.WriteLine($"   Código: {curso.Identificacion}");
                Console.WriteLine($"   Créditos: {curso.Creditos}");
                if (curso.ProfesorAsignado != null)
                {
                    Console.WriteLine($"   Profesor: {curso.ProfesorAsignado.Nombre} {curso.ProfesorAsignado.Apellido}");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al agregar curso: {ex.Message}");
            }

            Pausar();
        }

        private void ListarCursos()
        {
            Console.Clear();
            Console.WriteLine("\n═══ LISTA DE CURSOS ═══\n");

            var cursos = repoCursos.ObtenerTodos();

            if (cursos.Count == 0)
            {
                Console.WriteLine("No hay cursos registrados.");
            }
            else
            {
                Console.WriteLine($"Total: {cursos.Count} curso(s)\n");

                // Agrupar por tener profesor o no
                var cursosConProfesor = cursos.Where(c => c.ProfesorAsignado != null).OrderBy(c => c.Nombre);
                var cursosSinProfesor = cursos.Where(c => c.ProfesorAsignado == null).OrderBy(c => c.Nombre);

                if (cursosConProfesor.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("✓ Cursos con Profesor Asignado:");
                    Console.ResetColor();
                    foreach (var curso in cursosConProfesor)
                    {
                        Console.WriteLine($"  • {curso}");
                    }
                    Console.WriteLine();
                }

                if (cursosSinProfesor.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("⚠ Cursos sin Profesor Asignado:");
                    Console.ResetColor();
                    foreach (var curso in cursosSinProfesor)
                    {
                        Console.WriteLine($"  • [{curso.Identificacion}] {curso.Nombre} - {curso.Creditos} créditos");
                    }
                }
            }

            Pausar();
        }

        private void BuscarCurso()
        {
            Console.Clear();
            Console.WriteLine("\n═══ BUSCAR CURSO ═══\n");
            Console.WriteLine("1. Por Código");
            Console.WriteLine("2. Por Nombre");
            Console.WriteLine("3. Por Créditos");

            Console.Write("\n▶ Opción: ");
            string opcion = Console.ReadLine();

            List<Curso> resultados = new List<Curso>();

            try
            {
                switch (opcion)
                {
                    case "1":
                        string codigo = LeerTextoRequerido("Código del Curso: ");
                        var curso = repoCursos.BuscarPorId(codigo);
                        if (curso != null) resultados.Add(curso);
                        break;

                    case "2":
                        string nombre = LeerTextoRequerido("Nombre (o parte del nombre): ").ToLower();
                        resultados = repoCursos.Buscar(c =>
                            c.Nombre.ToLower().Contains(nombre));
                        break;

                    case "3":
                        int creditos = (int)LeerDecimal("Número de Créditos: ");
                        resultados = repoCursos.Buscar(c => c.Creditos == creditos);
                        break;
                }

                Console.WriteLine($"\n✓ Resultados encontrados: {resultados.Count}");
                foreach (var c in resultados)
                {
                    Console.WriteLine($"  • {c}");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error en búsqueda: {ex.Message}");
            }

            Pausar();
        }

        private void ModificarCurso()
        {
            Console.Clear();
            Console.WriteLine("\n═══ MODIFICAR CURSO ═══\n");

            try
            {
                string codigo = LeerTextoRequerido("Código del Curso: ");
                var curso = repoCursos.BuscarPorId(codigo);

                if (curso == null)
                {
                    MostrarError("Curso no encontrado");
                    Pausar();
                    return;
                }

                Console.WriteLine($"\n✓ Curso encontrado: {curso}");
                Console.WriteLine("\nIngrese los nuevos datos (Enter para mantener):");

                string nuevoNombre = LeerTextoOpcional("Nombre: ");
                if (!string.IsNullOrWhiteSpace(nuevoNombre))
                    curso.Nombre = nuevoNombre;

                Console.Write("Créditos (1-5) [Enter para mantener]: ");
                string creditosStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(creditosStr) && int.TryParse(creditosStr, out int creditos))
                {
                    if (creditos >= 1 && creditos <= 5)
                        curso.Creditos = creditos;
                    else
                        Console.WriteLine("⚠ Créditos inválidos, se mantiene el valor actual");
                }

                repoCursos.Actualizar(curso);
                MostrarExito("✓ Curso modificado exitosamente");
            }
            catch (Exception ex)
            {
                MostrarError($"Error al modificar: {ex.Message}");
            }

            Pausar();
        }

        private void EliminarCurso()
        {
            Console.Clear();
            Console.WriteLine("\n═══ ELIMINAR CURSO ═══\n");

            try
            {
                string codigo = LeerTextoRequerido("Código del Curso: ");
                var curso = repoCursos.BuscarPorId(codigo);

                if (curso == null)
                {
                    MostrarError("Curso no encontrado");
                    Pausar();
                    return;
                }

                Console.WriteLine($"\n✓ Curso: {curso}");

                // Verificar si hay estudiantes matriculados
                var estudiantes = gestorMatriculas.ObtenerEstudiantesPorCurso(codigo);
                if (estudiantes.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n⚠ ADVERTENCIA: Hay {estudiantes.Count} estudiante(s) matriculado(s) en este curso.");
                    Console.WriteLine("Si elimina el curso, también se eliminarán las matrículas asociadas.");
                    Console.ResetColor();
                }

                Console.Write("\n¿Está seguro de eliminar? (S/N): ");

                if (Console.ReadLine()?.ToUpper() == "S")
                {
                    repoCursos.Eliminar(codigo);
                    MostrarExito("✓ Curso eliminado exitosamente");
                }
                else
                {
                    Console.WriteLine("Operación cancelada");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al eliminar: {ex.Message}");
            }

            Pausar();
        }

        private void AsignarProfesor()
        {
            Console.Clear();
            Console.WriteLine("\n═══ ASIGNAR PROFESOR A CURSO ═══\n");

            try
            {
                string codCurso = LeerTextoRequerido("Código del Curso: ");
                var curso = repoCursos.BuscarPorId(codCurso);

                if (curso == null)
                {
                    MostrarError("Curso no encontrado");
                    Pausar();
                    return;
                }

                Console.WriteLine($"\n✓ Curso: {curso.Nombre} ({curso.Identificacion})");
                if (curso.ProfesorAsignado != null)
                {
                    Console.WriteLine($"Profesor actual: {curso.ProfesorAsignado.Nombre} {curso.ProfesorAsignado.Apellido}");
                    Console.Write("\n¿Desea cambiar el profesor? (S/N): ");
                    if (Console.ReadLine()?.ToUpper() != "S")
                    {
                        Console.WriteLine("Operación cancelada");
                        Pausar();
                        return;
                    }
                }

                var profesores = repoProfesores.ObtenerTodos();

                if (profesores.Count == 0)
                {
                    MostrarError("No hay profesores registrados en el sistema");
                    Pausar();
                    return;
                }

                Console.WriteLine("\n--- Profesores Disponibles ---");
                foreach (var prof in profesores.OrderBy(p => p.Apellido))
                {
                    Console.WriteLine($"  • {prof.Identificacion} - {prof.Nombre} {prof.Apellido} ({prof.Departamento})");
                }

                Console.WriteLine();
                string idProf = LeerTextoRequerido("ID del Profesor: ");
                var profesor = repoProfesores.BuscarPorId(idProf);

                if (profesor == null)
                {
                    MostrarError("Profesor no encontrado");
                }
                else
                {
                    curso.ProfesorAsignado = profesor;
                    repoCursos.Actualizar(curso);
                    MostrarExito($"✓ Profesor {profesor.Nombre} {profesor.Apellido} asignado a {curso.Nombre}");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error: {ex.Message}");
            }

            Pausar();
        }

        private bool MatricularEstudiante()
        {
            Console.Clear();
            Console.WriteLine("\n═══ MATRICULAR ESTUDIANTE EN CURSO ═══\n");

            try
            {
                string idEst = LeerTextoRequerido("ID del Estudiante: ");
                var estudiante = repoEstudiantes.BuscarPorId(idEst);

                if (estudiante == null)
                {
                    MostrarError("Estudiante no encontrado");
                    Pausar();
                    return true;
                }

                string codCurso = LeerTextoRequerido("Código del Curso: ");
                var curso = repoCursos.BuscarPorId(codCurso);

                if (curso == null)
                {
                    MostrarError("Curso no encontrado");
                    Pausar();
                    return true;
                }

                gestorMatriculas.MatricularEstudiante(estudiante, curso);
                MostrarExito($"✓ {estudiante.Nombre} matriculado en {curso.Nombre}");
            }
            catch (Exception ex)
            {
                MostrarError($"Error: {ex.Message}");
            }

            Pausar();
            return true;
        }

        private bool RegistrarCalificaciones()
        {
            Console.Clear();
            Console.WriteLine("\n═══ REGISTRAR CALIFICACIONES ═══\n");

            try
            {
                string idEst = LeerTextoRequerido("ID del Estudiante: ");
                string codCurso = LeerTextoRequerido("Código del Curso: ");
                decimal calificacion = LeerDecimal("Calificación (0-10): ");

                gestorMatriculas.AgregarCalificacion(idEst, codCurso, calificacion);
                MostrarExito("✓ Calificación registrada exitosamente");
            }
            catch (Exception ex)
            {
                MostrarError($"Error: {ex.Message}");
            }

            Pausar();
            return true;
        }

        private bool MenuReportes()
        {
            Console.Clear();
            Console.WriteLine("\n═══ REPORTES ═══\n");
            Console.WriteLine("1. Reporte de Estudiante");
            Console.WriteLine("2. Top 10 Mejores Estudiantes");
            Console.WriteLine("3. Estudiantes en Riesgo");
            Console.WriteLine("4. Estadísticas por Carrera");
            Console.WriteLine("0. Volver");

            Console.Write("\n▶ Opción: ");
            string opcion = Console.ReadLine();

            try
            {
                switch (opcion)
                {
                    case "1":
                        string id = LeerTextoRequerido("ID del Estudiante: ");
                        string reporte = gestorMatriculas.GenerarReporteEstudiante(id);
                        Console.WriteLine(reporte);
                        break;

                    case "2":
                        var top10 = gestorMatriculas.ObtenerTop10Estudiantes(repoEstudiantes);
                        Console.WriteLine("\n🏆 TOP 10 MEJORES ESTUDIANTES:\n");
                        int pos = 1;
                        foreach (var (est, prom) in top10)
                        {
                            Console.WriteLine($"{pos}. {est.Nombre} {est.Apellido} - " +
                                            $"Promedio: {prom:F2}");
                            pos++;
                        }
                        break;

                    case "3":
                        var enRiesgo = gestorMatriculas.ObtenerEstudiantesEnRiesgo(repoEstudiantes);
                        Console.WriteLine("\n⚠️  ESTUDIANTES EN RIESGO:\n");
                        foreach (var (est, prom, cursos) in enRiesgo)
                        {
                            Console.WriteLine($"• {est.Nombre} {est.Apellido} - " +
                                            $"Promedio: {prom:F2}");
                        }
                        break;

                    case "4":
                        var stats = gestorMatriculas.ObtenerEstadisticasPorCarrera(repoEstudiantes);
                        Console.WriteLine("\n📊 ESTADÍSTICAS POR CARRERA:\n");
                        foreach (var stat in stats)
                        {
                            Console.WriteLine(stat);
                            Console.WriteLine();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error: {ex.Message}");
            }

            Pausar();
            return true;
        }

        private bool MenuReflection()
        {
            Console.Clear();
            Console.WriteLine("\n═══ ANÁLISIS CON REFLECTION ═══\n");
            Console.WriteLine("1. Analizar clase Estudiante");
            Console.WriteLine("2. Analizar clase Profesor");
            Console.WriteLine("3. Analizar clase Curso");
            Console.WriteLine("0. Volver");

            Console.Write("\n▶ Opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    AnalizadorReflection.AnalizarClaseCompleta(typeof(Estudiante));
                    break;
                case "2":
                    AnalizadorReflection.AnalizarClaseCompleta(typeof(Profesor));
                    break;
                case "3":
                    AnalizadorReflection.AnalizarClaseCompleta(typeof(Curso));
                    break;
            }

            Pausar();
            return true;
        }

        //TODO MÉTODOS AUXILIARES
    

        private void MostrarError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n✗ {mensaje}");
            Console.ResetColor();
        }

        private void MostrarExito(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{mensaje}");
            Console.ResetColor();
        }

        private void Pausar()
        {
            Console.WriteLine("\nPresione Enter para continuar...");
            Console.ReadLine();
        }

        private string LeerTextoRequerido(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string texto = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(texto))
                    return texto;
                MostrarError("Este campo es requerido");
            }
        }

        private string LeerTextoOpcional(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine();
        }

        private DateTime LeerFecha(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                if (DateTime.TryParse(Console.ReadLine(), out DateTime fecha))
                    return fecha;
                MostrarError("Fecha inválida. Use formato mm/dd/yyyy");
            }
        }

        private decimal LeerDecimal(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor >= 0)
                    return valor;
                MostrarError("Valor inválido");
            }
        }

        private void MostrarDespedida()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║         ¡Gracias por usar el sistema!                      ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║         Sistema de Gestión Universitaria                   ║");
            Console.WriteLine("║                   2025                                     ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
            Console.ResetColor();
        }
    }
}