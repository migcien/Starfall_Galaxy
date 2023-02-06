using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarfallGalaxy
{
    public class Shield : MonoBehaviour
    {
        public float shieldHealth = 100f;

        public void DecreaseShieldHealth(float damage)
        {
            shieldHealth -= damage;
            if (shieldHealth <= 0f)
            {
                DeactivateShield();
            }
        }

        private void DeactivateShield()
        {
            gameObject.SetActive(false);
        }
    }
}

