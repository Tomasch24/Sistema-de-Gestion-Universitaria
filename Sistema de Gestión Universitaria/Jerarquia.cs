using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_de_Gestión_Universitaria
{
    public abstract class Persona : IIdentificable
    {
        private string identificacion;
        public string Identificacion 
        { get => identificacion; 
            
          set 
          { 
                if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("La identificacion no puede estar vacia"); 
                identificacion = value;
          } 
        }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public int Edad => DateTime.Now.Year - FechaNacimiento.Year -
            (DateTime.Now.DayOfYear < FechaNacimiento.DayOfYear ? 1 : 0);

        public abstract string ObtenerRol();

        // Sobrescritura de ToString() - Polimorfismo
        public override string ToString()
        {
            return $"[{ObtenerRol()}] {Nombre} {Apellido} - ID: {Identificacion} - Edad: {Edad} años";
        }


    }
    public class Estudiante : Persona
    {
        public string Carrera { get; set; }

        [Requerido("El número de matrícula es obligatorio")]
        [Formato("XXX-XXXXX", Descripcion = "Formato: 3 dígitos, guión, 5 dígitos")]
        public string numeroMatricula { get; set; }

        //Constructor para generar el id y el numeroMatricula automaticamente
        public Estudiante()
        {
            Identificacion = GeneradorIdentificaciones.GenerarIdEstudiante();
            NumeroMatricula = GeneradorIdentificaciones.GenerarNumeroMatricula();
        }

        public string NumeroMatricula
        {
            get => numeroMatricula;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El número de matrícula no puede estar vacío");
                numeroMatricula = value;
            }
        }

        public void ValidarEdad()
        {
            if (Edad < 15)
                throw new InvalidOperationException("El estudiante debe tener 15 años como minimo");
        }

        public override string ObtenerRol()
        {
            return "ESTUDIANTE";
        }

        public override string ToString()
        {
            return base.ToString() + $" - Carrera: {Carrera} - Matrícula: {NumeroMatricula}";
        }

    }
    public enum TipoContrato
    {
        Tiempo_Completo,
        Medio_Tiempo,
        Temporal,
        Por_Horas
    }
    public class Profesor : Persona
    {
        public string Departamento { get; set; }

        public TipoContrato TipoContrato { get; set; }

        [ValidacionRango(500, 10000)]
        private decimal salarioBase { get; set; }

        //Constructor para generar el id automaticamente
        public Profesor()
        {
           
            Identificacion = GeneradorIdentificaciones.GenerarIdProfesor();
        }



        public decimal SalarioBase
        {
            get => salarioBase;
            set
            {
                if (value < 0)
                    throw new ArgumentException("El salario no puede ser negativo");
                salarioBase = value;
            }
        }

        public void ValidarEdad()
        {
            if (Edad < 25)
                throw new InvalidOperationException("El Profesor debe tener 25 años como minimo");
        }
        public override string ObtenerRol()
        {
            return "PROFESOR";
        }

        public override string ToString()
        {
            return base.ToString() +
                   $"Departamento: {Departamento} Contrato: {TipoContrato} Salario: ${SalarioBase:N2}";
        }
    }
}
