using System;
using UnityEngine;

namespace Codebase.Mechanics.Feed_system
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Feadable : MonoBehaviour
    {
        private void Eat(float calory)
        {
        }
        [ContextMenu("Raise mass")]
        private void RaiseMass()
        {
            GetComponent<Rigidbody2D>().mass += 1f;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Food>() != null)
            {
                Eat(other.gameObject.GetComponent<Food>().Calory);
                other.gameObject.GetComponent<Food>().Eat();
            }
        }
    }
}