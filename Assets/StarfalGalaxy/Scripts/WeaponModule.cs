using UnityEngine;

public abstract class WeaponModule : ScriptableObject
{
    public string weaponName;
    public float fireRate;
    public float reloadTime;
    public float range;
    public float accuracy;

    public WeaponModule(string weaponName, float fireRate, float reloadTime, float range, float accuracy)
    {
        this.weaponName = weaponName;
        this.fireRate = fireRate;
        this.reloadTime = reloadTime;
        this.range = range;
        this.accuracy = accuracy;
    }

    public abstract void Fire(UnityEngine.Vector3 position, Quaternion rotation);
}