using UnityEngine;

namespace StarfallGalaxy.controllers
{
    [CreateAssetMenu(menuName = "Weapon")]

    public class Weapon2 : ScriptableObject
    {
        [SerializeField] private GameObject projectilePrefab;
        public string weaponName;
        public float fireRate;
        public float projectileSpeed;
        public float projectileDamage;
        public float reloadTime;
        public float range;
        public float accuracy;

        public Weapon2(string weaponName, float fireRate, float projectileSpeed, float projectileDamage, float reloadTime, float range, float accuracy)
        {
            this.weaponName = weaponName;
            this.fireRate = fireRate;
            this.projectileSpeed = projectileSpeed;
            this.projectileDamage = projectileDamage;
            this.reloadTime = reloadTime;
            this.range = range;
            this.accuracy = accuracy;
        }

        public void Fire(Vector3 position, Quaternion rotation)
        {
            GameObject primaryProjectile = Instantiate(projectilePrefab, position, rotation);
            primaryProjectile.transform.rotation = rotation;
            primaryProjectile.GetComponent<Rigidbody>().AddForce(-projectilePrefab.transform.right * 100f, ForceMode.Impulse);
        }
    }
}