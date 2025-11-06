using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_de_Gestión_Universitaria
{
    public class Curso : IIdentificable
    {
        private string identificacion;
        public string Nombre {  get; set; }
        public int Creditos {  get; set; }

        public string Identificacion
        {
            get => identificacion;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El código no puede estar vacío");
                identificacion = value;
            }
        }

        public Profesor ProfesorAsignado { get; set; }

        public override string ToString()
        {
            string profesor = ProfesorAsignado != null ?
                $"{ProfesorAsignado.Nombre} {ProfesorAsignado.Apellido}" :
                "Sin asignar";
            return $"[{Identificacion}] {Nombre} - {Creditos} créditos - Profesor: {profesor}";
        }
    }

}

