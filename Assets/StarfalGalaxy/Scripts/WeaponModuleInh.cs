using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarfallGalaxy.controllers
{
    [CreateAssetMenu(menuName = "WeaponModule")]
    public class WeaponModuleInh : WeaponModule
    {
        [SerializeField] private GameObject projectilePrefab;
        public float projectileSpeed;
        public float projectileDamage;

        public WeaponModuleInh(string weaponName, float fireRate, float projectileSpeed, float projectileDamage, float reloadTime, float range, float accuracy) : base(weaponName, fireRate, reloadTime, range, accuracy)
        {
            this.projectileSpeed = projectileSpeed;
            this.projectileDamage = projectileDamage;
        }

        public override void Fire(Vector3 position, Quaternion rotation)
        {
            GameObject primaryProjectile = Instantiate(projectilePrefab, position, rotation);
            primaryProjectile.transform.rotation = rotation;
            primaryProjectile.GetComponent<Rigidbody>().AddForce(-primaryProjectile.transform.right * projectileSpeed, ForceMode.Impulse);



        }
    }
}