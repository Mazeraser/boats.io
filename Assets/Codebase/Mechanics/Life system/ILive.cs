namespace Codebase.Mechanics.Life_system
{
    /// <summary>
    /// Класс, реализующий механику жизни
    /// </summary>
    public interface ILive
    {
        public int HP { get; }
        public void GetDamage(int damage);
        public void Death();
    }
}