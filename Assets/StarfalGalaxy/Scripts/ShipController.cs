using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography;
using UnityEngine;

namespace StarfallGalaxy.controllers
{
    public class ShipController : MonoBehaviour
    {
        // The ship itself
        private Rigidbody spaceShip;

        // Variables for weapons
        public GameObject primaryWeaponPrefab;
        public GameObject secondaryWeaponPrefab;
        //public single primaryWeaponVelocity = 50.0f;
        //public float secondaryWeaponVelocity = 70.0f;
        public System.Single varSpeed = 200f;
        public float primaryWeaponCooldown = 0.1f;
        private float primaryLastShoot = 0.0f;
        public float secondaryWeaponCooldown = 0.0f;
        private float secondaryLastShoot = 0.0f;

        // Variables for shield mechanics
        public GameObject shieldPrefab;
        private GameObject activeShield;
        private float shieldHealth = 100f;
        private float shieldActivationCooldown = 10f;
        private float shieldActiveTime = 6f;
        private float lastShieldActivationTime = 0f;
        private bool shieldActive = false;

        // Variables for navigation mechanics
        public float speed = 10.0f;
        private float originalSpeed;
        public float rotationSpeed = 100f;
        private Vector2 mousePosition;
        private Vector2 mouseInput;
        public float yawPower = 5;
        private float yaw;

        // Health parameters of the player
        public float startingHealth = 100f;
        private float maxHealth = 100f;
        private float currentHealth;

        // Variables for burst warp mechanics
        public float burstWarpVelocity = 20.0f;
        public float burstWarpDuration = 3.0f;
        public float burstWarpCooldown = 10.0f;
        public Material burstWarpMaterial;
        private Material originalMaterial;
        private MeshRenderer[] renderers;
        private bool canUseBurstWarp = true;

        // Start is called before the first frame update
        void Start()
        {
            spaceShip = GetComponent<Rigidbody>();
            currentHealth = startingHealth;
            originalSpeed = speed;
            renderers = GetComponentsInChildren<MeshRenderer>();
            originalMaterial = renderers[0].material;
        }

        // Update is called once per frame
        void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                horizontal -= 1.0f;
                transform.Rotate(Vector3.up, -25 * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                horizontal += 1.0f;
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                vertical += 1.0f;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                vertical -= 1.0f;
            }

            //yaw = Input.GetAxis("Yaw") * yawPower * Time.deltaTime;
            //transform.Rotate(0, yaw, 0);

            transform.position += new Vector3(horizontal, vertical, 0.0f) * speed * Time.deltaTime;
            //spaceShip.AddForce(new Vector3(horizontal, vertical, 0.0f) * 0.1f);

            if (Input.GetMouseButtonDown(0))
            {
                FirePrimaryWeapon();
                if (Time.time > primaryLastShoot + primaryWeaponCooldown)
                {
                    FirePrimaryWeapon();
                    primaryLastShoot = Time.time;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                    FireSecondaryWeapon();
            }
            // Check if the middle mouse button is pressed
            if (Input.GetMouseButtonDown(2) && Time.time - lastShieldActivationTime >= shieldActivationCooldown && !shieldActive)
            {
                ActivateShield();
            }

            // Check if the shift key is pressed
            if (Input.GetKeyDown(KeyCode.LeftShift) && canUseBurstWarp)
            {
                StartCoroutine(BurstWarp());
            }
        }

        void FirePrimaryWeapon()
        {
            GameObject primaryProjectile = Instantiate(primaryWeaponPrefab, transform.position, transform.rotation);
            primaryProjectile.transform.rotation = transform.rotation;
            primaryProjectile.GetComponent<Rigidbody>().AddForce(-transform.right * 100f, ForceMode.Impulse);
        }

        void FireSecondaryWeapon()
        {
            GameObject secondaryProjectile = Instantiate(secondaryWeaponPrefab, transform.position, transform.rotation);
            secondaryProjectile.transform.rotation = transform.rotation;
            secondaryProjectile.GetComponent<Rigidbody>().AddForce(-transform.right * 100f, ForceMode.Impulse);
            secondaryLastShoot = Time.time;
        }

        // Function for activating the shield
        private void ActivateShield()
        {
            activeShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
            activeShield.transform.SetParent(transform);
            shieldActive = true;
            lastShieldActivationTime = Time.time;
            StartCoroutine(UpdateShield());
        }

        private IEnumerator UpdateShield()
        {
            float startTime = Time.time;
            while (Time.time - startTime <= shieldActiveTime)
            {
                if (shieldHealth <= 0)
                {
                    DestroyShield();
                    yield break;
                }
                yield return null;
            }
            DestroyShield();
        }

        IEnumerator BurstWarp()
        {
            canUseBurstWarp = false;
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.material = burstWarpMaterial;
            }
            speed *= 2.5f;
            yield return new WaitForSeconds(burstWarpDuration);
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.material = originalMaterial;
            }
            speed = originalSpeed;
            yield return new WaitForSeconds(burstWarpCooldown);
            canUseBurstWarp = true;
        }

        // Function for disappearing the shield after n seconds
        private void DestroyShield()
        {
            Destroy(activeShield);
            shieldActive = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Enemy Projectile")
            {
                TakeDamage(10);
                if (currentHealth <= 0)
                {
                    //Destroy the ship, but we don't want that otherwise we'll be sad :(
                    //You just died
                }
            }
        }
        
        public void TakeDamage(float damage)
        {
            if (!shieldActive)
            {
                currentHealth -= damage;
            }
            else
            {
                ShieldTakeDamage(damage);
            }
        }

        public void ShieldTakeDamage(float damage)
        {
            if (shieldActive)
            {
                shieldHealth -= damage;
            }
        }

        public void RecoverHealth(float recoverAmount)
        {
            currentHealth = Mathf.Clamp(currentHealth + recoverAmount, 0, maxHealth);
        }
    }
}