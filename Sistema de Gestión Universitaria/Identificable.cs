using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_de_Gestión_Universitaria
{

    public abstract class PersonaIdentificable : Persona, IIdentificable
    {
        // Ya tiene la propiedad Identificacion heredada de Persona
    }

    // Hacer que Curso implemente IIdentificable
    public class CursoIdentificable : Curso, IIdentificable
    {
        // Para Curso, usaremos Codigo como Identificacion
        string IIdentificable.Identificacion => Identificacion;
    }
}
