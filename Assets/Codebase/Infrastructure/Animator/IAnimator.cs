using DG.Tweening;

namespace Codebase.Infrastructure.Animator
{
    /// <summary>
    /// Интерфейс для анимаций
    /// </summary>
    public interface IAnimator
    {
        public void Death(TweenCallback callback = null);
        public void Born();
    }
}