using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_de_Gestión_Universitaria
{
    public class Repositorio<T> where T : IIdentificable
    {
        //TODO Dictionary para almacenamiento eficiente por clave
        private Dictionary<string, T> items = new Dictionary<string, T>();

        //
        //TODO Agrega un elemento al repositorio
        //
        public void Agregar(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (items.ContainsKey(item.Identificacion))
                throw new InvalidOperationException(
                    $"Ya existe un elemento con la identificación: {item.Identificacion}");

            items.Add(item.Identificacion, item);
        }

        //
        //TODO Elimina un elemento por su identificación
        //
        public bool Eliminar(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID no puede estar vacío");

            return items.Remove(id);
        }

        //
        //TODO Busca un elemento por su identificación
        //
        public T BuscarPorId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID no puede estar vacío");

            items.TryGetValue(id, out T item);
            return item;
        }
        //
        //TODO Obtiene todos los elementos del repositorio
        //
        public List<T> ObtenerTodos()
        {
            return items.Values.ToList();
        }

        //
        //TODO Busca elementos usando un predicado (delegate)
        // Aplica: Delegates, Lambda Expressions
        //
        public List<T> Buscar(Func<T, bool> predicado)
        {
            if (predicado == null)
                throw new ArgumentNullException(nameof(predicado));

            // Uso de LINQ con lambda expression
            return items.Values.Where(predicado).ToList();
        }

        //
        //TODO Cuenta elementos que cumplen un criterio
        //
        public int Contar(Func<T, bool> predicado = null)
        {
            if (predicado == null)
                return items.Count;

            return items.Values.Count(predicado);
        }

        //
        //TODO Verifica si existe un elemento con el ID dado
        //
        public bool Existe(string id)
        {
            return items.ContainsKey(id);
        }

        //
        //TODO Actualiza un elemento existente
        //
        public void Actualizar(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (!items.ContainsKey(item.Identificacion))
                throw new InvalidOperationException(
                    $"No existe un elemento con la identificación: {item.Identificacion}");

            items[item.Identificacion] = item;
        }

        //
        //TODO Limpia todos los elementos del repositorio
        //
        public void LimpiarTodo()
        {
            items.Clear();
        }
    }
}
