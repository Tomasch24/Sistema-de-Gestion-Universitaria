using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_de_Gestión_Universitaria
{
    public class Repositorio<T> where T : IIdentificable
    {
        // Dictionary para almacenamiento eficiente por clave
        private Dictionary<string, T> items = new Dictionary<string, T>();

        /// <summary>
        /// Agrega un elemento al repositorio
        /// </summary>
        public void Agregar(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (items.ContainsKey(item.Identificacion))
                throw new InvalidOperationException(
                    $"Ya existe un elemento con la identificación: {item.Identificacion}");

            items.Add(item.Identificacion, item);
        }

        /// <summary>
        /// Elimina un elemento por su identificación
        /// </summary>
        public bool Eliminar(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID no puede estar vacío");

            return items.Remove(id);
        }

        /// <summary>
        /// Busca un elemento por su identificación
        /// </summary>
        public T BuscarPorId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID no puede estar vacío");

            items.TryGetValue(id, out T item);
            return item;
        }
        /// <summary>
        /// Obtiene todos los elementos del repositorio
        /// </summary>
        public List<T> ObtenerTodos()
        {
            return items.Values.ToList();
        }

        /// <summary>
        /// Busca elementos usando un predicado (delegate)
        /// Aplica: Delegates, Lambda Expressions
        /// </summary>
        public List<T> Buscar(Func<T, bool> predicado)
        {
            if (predicado == null)
                throw new ArgumentNullException(nameof(predicado));

            // Uso de LINQ con lambda expression
            return items.Values.Where(predicado).ToList();
        }

        /// <summary>
        /// Cuenta elementos que cumplen un criterio
        /// </summary>
        public int Contar(Func<T, bool> predicado = null)
        {
            if (predicado == null)
                return items.Count;

            return items.Values.Count(predicado);
        }

        /// <summary>
        /// Verifica si existe un elemento con el ID dado
        /// </summary>
        public bool Existe(string id)
        {
            return items.ContainsKey(id);
        }

        /// <summary>
        /// Actualiza un elemento existente
        /// </summary>
        public void Actualizar(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (!items.ContainsKey(item.Identificacion))
                throw new InvalidOperationException(
                    $"No existe un elemento con la identificación: {item.Identificacion}");

            items[item.Identificacion] = item;
        }

        /// <summary>
        /// Limpia todos los elementos del repositorio
        /// </summary>
        public void LimpiarTodo()
        {
            items.Clear();
        }
    }
}
