using UnityEngine;

namespace Codebase.Mechanics.MoveSystem
{
    /// <summary>
    /// Интерфейс для перемещения объекта
    /// </summary>
    public interface IMovable
    {
        float Speed { get; }
        void Turn(Vector2 direction);
    }
}