using UnityEngine;

namespace Codebase.Mechanics.MoveSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class MovableParent : MonoBehaviour
    {
        private Rigidbody2D _rb;

        public virtual void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        protected void Move(Vector2 direction, float speed)
        {

            Vector2 movement = new Vector2(direction.x, direction.y);

            _rb.AddForce(movement * speed, ForceMode2D.Impulse);
        }
    }
}