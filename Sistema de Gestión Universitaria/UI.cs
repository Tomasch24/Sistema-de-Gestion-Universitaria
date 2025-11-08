using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_de_Gestión_Universitaria
{
    public class SistemaUniversitario
    {
        private Repositorio<Estudiante> repoEstudiantes;
        private Repositorio<Profesor> repoProfesores;
        private Repositorio<Curso> repoCursos;
        private GestorMatriculas gestorMatriculas;

        public SistemaUniversitario()
        {
            repoEstudiantes = new Repositorio<Estudiante>();
            repoProfesores = new Repositorio<Profesor>();
            repoCursos = new Repositorio<Curso>();
            gestorMatriculas = new GestorMatriculas(repoEstudiantes, repoCursos);
        }

        //
        //TODO Inicia el sistema con menú principal
        //
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
            Console.WriteLine("║        SISTEMA DE GESTIÓN UNIVERSITARIA                    ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║            Universidad Tecnológica 2025                    ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine("\nPresione Enter para continuar...");
            Console.ReadLine();
        }

        private void MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n═══════════════ MENÚ PRINCIPAL ═══════════════");
            Console.ResetColor();

            Console.WriteLine("\n1.  👨‍🎓 Gestionar Estudiantes");
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
            Console.Write("\n▶ Seleccione una opción: ");
            Console.ResetColor();
        }

        private bool ProcesarOpcionPrincipal(string opcion)
        {
            return opcion switch
            {
                "1" => MenuEstudiantes(),
                "2" => MenuProfesores(),
                "3" => MenuCursos(),
                "4" => MatricularEstudiante(),
                "5" => RegistrarCalificaciones(),
                "6" => MenuReportes(),
                "7" => MenuReflection(),
                "8" => DemostrarFuncionalidades(),
                "9" => GenerarDatosPrueba(),
                "0" => false,
                _ => OpcionInvalida()
            };
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
                    default: OpcionInvalida(); break;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error: {ex.Message}");
            }

            return true;
        }

        private void AgregarEstudiante()
        {
            Console.Clear();
            Console.WriteLine("\n═══ AGREGAR ESTUDIANTE ═══\n");

            var estudiante = new Estudiante();

            estudiante.Identificacion = LeerTextoRequerido("Identificación (cédula/pasaporte): ");
            estudiante.Nombre = LeerTextoRequerido("Nombre: ");
            estudiante.Apellido = LeerTextoRequerido("Apellido: ");
            estudiante.FechaNacimiento = LeerFecha("Fecha de Nacimiento (dd/mm/yyyy): ");
            estudiante.Carrera = LeerTextoRequerido("Carrera: ");
            estudiante.NumeroMatricula = LeerTextoRequerido("Número de Matrícula (XXX-XXXXX): ");

            estudiante.ValidarEdad();
            repoEstudiantes.Agregar(estudiante);

            MostrarExito($"✓ Estudiante {estudiante.Nombre} agregado exitosamente");
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
                foreach (var est in estudiantes.OrderBy(e => e.Apellido))
                {
                    Console.WriteLine($"• {est}");
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

            Console.WriteLine($"\nResultados encontrados: {resultados.Count}");
            foreach (var e in resultados)
            {
                Console.WriteLine($"• {e}");
            }

            Pausar();
        }

        private void ModificarEstudiante()
        {
            Console.Clear();
            Console.WriteLine("\n═══ MODIFICAR ESTUDIANTE ═══\n");

            string id = LeerTextoRequerido("Identificación del estudiante: ");
            var estudiante = repoEstudiantes.BuscarPorId(id);

            if (estudiante == null)
            {
                MostrarError("Estudiante no encontrado");
                Pausar();
                return;
            }

            Console.WriteLine($"\nEstudiante actual: {estudiante}");
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
            Pausar();
        }

        private void EliminarEstudiante()
        {
            Console.Clear();
            Console.WriteLine("\n═══ ELIMINAR ESTUDIANTE ═══\n");

            string id = LeerTextoRequerido("Identificación del estudiante: ");
            var estudiante = repoEstudiantes.BuscarPorId(id);

            if (estudiante == null)
            {
                MostrarError("Estudiante no encontrado");
                Pausar();
                return;
            }

            Console.WriteLine($"\nEstudiante: {estudiante}");
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
            Console.WriteLine("4. Modificar Profesor");
            Console.WriteLine("5. Eliminar Profesor");
            Console.WriteLine("0. Volver");

            Console.Write("\n▶ Opción: ");
            string opcion = Console.ReadLine();

            try
            {
                switch (opcion)
                {
                    case "1": AgregarProfesor(); break;
                    case "2": ListarProfesores(); break;
                    case "0": return true;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error: {ex.Message}");
            }

            return true;
        }

        private void AgregarProfesor()
        {
            Console.Clear();
            Console.WriteLine("\n═══ AGREGAR PROFESOR ═══\n");

            var profesor = new Profesor();

            profesor.Identificacion = LeerTextoRequerido("Identificación: ");
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
                "4" => TipoContrato.Temporal,
                _ => TipoContrato.Temporal
            };

            profesor.ValidarEdad();
            repoProfesores.Agregar(profesor);

            MostrarExito($"✓ Profesor {profesor.Nombre} agregado exitosamente");
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
                foreach (var prof in profesores.OrderBy(p => p.Apellido))
                {
                    Console.WriteLine($"• {prof}");
                }
            }

            Pausar();
        }

       
        private bool OpcionInvalida()
        {
            MostrarError("Opción no válida");
            Pausar();
            return true;
        }

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
                MostrarError("Fecha inválida. Use formato dd/mm/yyyy");
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
            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║                                        ║");
            Console.WriteLine("║   ¡Gracias por usar el sistema!        ║");
            Console.WriteLine("║                                        ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");
            Console.ResetColor();
        }

        //TODO Metods Auxiliares
        private bool MenuCursos() { return true; }
        private bool MatricularEstudiante() { return true; }
        private bool RegistrarCalificaciones() { return true; }
        private bool MenuReportes() { return true; }
        private bool MenuReflection() { return true; }
        private bool DemostrarFuncionalidades() { return true; }
        private bool GenerarDatosPrueba() { return true; }
    }
}
