using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_de_Gestión_Universitaria
{
    public abstract class Persona
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
        public DateTime Fechanacimiento { get; set; }

        public int Edad => DateTime.Now.Year - Fechanacimiento.Year -
            (DateTime.Now.DayOfYear < Fechanacimiento.DayOfYear ? 1 : 0);

        public abstract string ObtenerRol();

    }
    public class Estudiante : Persona
    {
        public string Carrera { get; set; }

        public string NumeroMatricula { get; set; }

       public void ValidarEdad()
       {
            if (Edad < 15)
                throw new InvalidOperationException("El estudiante debe tener 15 años como minimo");
       }

    }
    public class Profesor : Persona
    {
        public string Departamento { get; set; }

        public enum TipoContrato 
        { 
            Indefinido,
            Temporal        
        };

        public decimal SalarioBase { get; set; }

        public void ValidarEdad()
        {
            if (Edad < 25)
                throw new InvalidOperationException("El Profesor debe tener 25 años como minimo");
        }
    }
}
