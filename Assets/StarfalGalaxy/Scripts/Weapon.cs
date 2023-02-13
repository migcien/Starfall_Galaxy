using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarfallGalaxy.controllers
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firingPoint;
        [SerializeField] private float fireRate = 1.0f;

        public int primaryDamage = 50;
        public int secondaryDamage = 30;
        public bool isPrimary;
        private ShipController shipController;
        private float timer = 3f;
        public bool isDestroyed = false;
        private float timeSinceInstantiated;

        private float fireCooldown = 0.0f;
        //public WeaponModuleInh weaponModule;
        private float damage;

        private void Start()
        {
            //weaponModule = GetComponent<WeaponModuleInh>();
            //shipController = GetComponent<ShipController>();
            //weaponModule = GetComponent<ShipController>().weaponModule;
            timeSinceInstantiated = 0.0f;
            //WeaponModuleInh weapon = GetComponent<ShipController>().GetWeapon();
            //float damage = weapon.projectileDamage;
            //damage = shipController.GetComponent<WeaponModuleInh>().projectileDamage;
            
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
            fireCooldown -= Time.deltaTime;
        }

        public void Fire2()
        {
            if (fireCooldown <= 0.0f)
            {
                Instantiate(projectilePrefab, firingPoint.position, firingPoint.rotation);
                fireCooldown = fireRate;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Enemies")
            {
                Health enemyHealth = collision.gameObject.GetComponent<Health>();
                enemyHealth.TakeDamage(isPrimary ? primaryDamage : secondaryDamage);
                //damage = weaponModule.projectileDamage;
                //enemyHealth.TakeDamage((int)damage);
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