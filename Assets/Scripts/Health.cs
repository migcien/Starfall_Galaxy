using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarfallGalaxy
{
    public class Health : MonoBehaviour
    {
        public int MaximumHealth = 100;
        public int CurrentHealth { get; private set; }

        private void Start()
        {
            CurrentHealth = MaximumHealth;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
        }
    }
}
