using UnityEngine;

namespace Codebase.Infrastructure.InputService
{
    /// <summary>
    /// Для работы с управлением
    /// </summary>
    public interface IInput
    {
        Vector2 Velocity { get; }

        void Activate();
        void Deactivate();
    }
}
    
