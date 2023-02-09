using UnityEngine;

[CreateAssetMenu(menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField] private GameObject projectilePrefab;
    public string weaponName;
    public float fireRate;
    public float projectileSpeed;
    public float projectileDamage;
    public float reloadTime;
    public float range;
    public float accuracy;

    public Weapon(string weaponName, float fireRate, float projectileSpeed, float projectileDamage, float reloadTime, float range, float accuracy)
    {
        this.weaponName = weaponName;
        this.fireRate = fireRate;
        this.projectileSpeed = projectileSpeed;
        this.projectileDamage = projectileDamage;
        this.reloadTime = reloadTime;
        this.range = range;
        this.accuracy = accuracy;
    }

}