using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarfallGalaxy.controllers
{
    public class ProjectileCollisionDetector : MonoBehaviour
    {
        public int primaryDamage = 50;
        public int secondaryDamage = 30;
        public bool isPrimary;
        private ShipController shipController;
        private float timer = 3f;
        public bool isDestroyed = false;
        private float timeSinceInstantiated;

        private void Start()
        {
            shipController = GetComponentInParent<ShipController>();
            timeSinceInstantiated = 0.0f;
        }

        private void Update()
        {
            if (gameObject != null)
            {
                timeSinceInstantiated += Time.deltaTime;
                if (timeSinceInstantiated >= timer)
                {
                    Destroy(gameObject);
                    isDestroyed = true;
                }
            }
        }

            private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Enemies")
        {
            Health enemyHealth = collision.gameObject.GetComponent<Health>();
            enemyHealth.TakeDamage(isPrimary ? primaryDamage : secondaryDamage);
            if (enemyHealth.CurrentHealth <= 0)
            {
                    Destroy(collision.gameObject);
                        isDestroyed = true;
                        Debug.Log("Enemy dead");
            }
                Debug.Log("I was destroyed");
            Destroy(gameObject);
        }
            else if (collision.gameObject.tag == "Edge")
            {
                Destroy(gameObject);
                    isDestroyed = true;
                }
        }
    }
}