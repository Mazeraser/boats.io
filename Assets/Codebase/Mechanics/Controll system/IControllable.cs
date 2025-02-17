using UnityEngine;

namespace Codebase.Mechanics.ControllSystem
{
    /// <summary>
    /// Интерфейс для реализации алгоритмов управления
    /// </summary>
    public interface IControllable
    {
        void ControllMove(Vector2 direction);
    }
}