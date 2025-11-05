using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_de_Gestión_Universitaria
{
    public  interface IEvaluable
    {
        public void AgregarCalificacion(decimal Calificacion);

        public void ObtenerPromedio();

        public void HaAprovado();
    }
}
