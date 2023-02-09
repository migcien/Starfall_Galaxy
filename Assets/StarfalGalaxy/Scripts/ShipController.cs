using System.Collections;
using UnityEngine;
using Cinemachine;

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

        // Variables for rotation mechanics
        public float rotationSpeed = 50f;
        private Vector3 mousePosition;
        private Vector3 screenPoint;
        private Vector2 mouseOffset;
        private float angle2Follow;
        private Quaternion originalRotation;
        private Quaternion targetRotation;

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

        // Variable for camera
        public CinemachineVirtualCamera vcam;
        private float minOrtho = 5.0f;
        private float maxOrtho = 50.0f;
        private float valueOrtho;

        private void Awake()
        {

        }

            // Start is called before the first frame update
            void Start()
        {
            spaceShip = GetComponent<Rigidbody>();
            currentHealth = startingHealth;
            originalSpeed = speed;
            renderers = GetComponentsInChildren<MeshRenderer>();
            originalMaterial = renderers[0].material;
            originalRotation = transform.rotation;
            valueOrtho = vcam.m_Lens.OrthographicSize;
        }

        // Update is called once per frame
        void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float mouseWheelValue = Input.GetAxis("Mouse ScrollWheel");

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                horizontal -= 1.0f;
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
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                // mouse wheel is scrolled down (back -1)
                mouseWheelValue -= 1;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                // mouse wheel is scrolled up (towards +1)
                mouseWheelValue += 1;
            }

            // adjust the camera zoom
            vcam.m_Lens.OrthographicSize = Mathf.Clamp(vcam.m_Lens.OrthographicSize + mouseWheelValue, minOrtho, maxOrtho);

            // Calculate spaceship displacements

            // Get the forward direction
            Vector3 forwardDirection = transform.right * -vertical; //- sign because of orientation of the ship's CS

            // Get the lateral direction
            //Vector3 forwardDirection = transform.forward * horizontal;

            // I like to move it, move it!
            //Vector3 movement = forwardDirection + rightDirection;
            //transform.position += movement * speed * Time.deltaTime;

            // I like to move it, move it!
            transform.position += forwardDirection * speed * Time.deltaTime;
            transform.position += new Vector3(0.0f, 0.0f, horizontal) * speed * Time.deltaTime;

            // Get the mouse position on the screen
            mousePosition = Input.mousePosition;

            // Get the screen position on the screen
            screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);

            // Convert the mouse position to world coordinates
            //mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.z - Camera.main.transform.position.z));

            // Calculate the offset between the mouse position and the spaceship position
            mouseOffset = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);

            if (mouseOffset.magnitude > 35f)
            {
                // Calculate the angle between the spaceship and mouse position
                angle2Follow = Mathf.Atan2(mouseOffset.y, mouseOffset.x) * Mathf.Rad2Deg;

                // Calculate the target rotation based on the calculated angle
                targetRotation = originalRotation * Quaternion.Euler(0, -angle2Follow + 90, 0);

                // Lerp the rotation of the spaceship towards the target rotation
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

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