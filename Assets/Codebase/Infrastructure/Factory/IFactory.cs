using System.Collections.Generic;
using UnityEngine;

namespace Codebase.Infrastructure.Factory
{
    /// <summary>
    /// Создает сущности(обычно на сервере), хранит в себе все активные экземпляры и уничтожает/перемещает их
    /// </summary>
    public interface IFactory<T>
    {
        List<T> Entities { get; }
        public T CreateProduct(GameObject prefab);
    }
}