using UnityEngine;
using StarfallGalaxy;
public enum GameState
{
    Play,
    Won,
    Lost
}

namespace StarfallGalaxy.controllers
{
    public class GameController : MonoBehaviour
    {
        public GameState gameState { get; private set; }

        public ShipController playerSpaceship;
        public bool autoFindShips = true;

        ShipController[] spaceships;

        public GameObject spaceship1;
        public GameObject spaceship2;
        public GameObject spaceship3;
        public GameObject spaceship4;

        private GameObject spaceshipSelected;

        // Whenever this scene becomes active, the Start() method is called.
        // This is the first method that is called when the scene is loaded.
        void OnEnable()
        {
            switch (Intro_Scene.selectedSpaceship)
            {
                case 6:
                    spaceshipSelected = spaceship1;
                    break;
                case 7:
                    spaceshipSelected = spaceship2;
                    break;
                case 8:
                    spaceshipSelected = spaceship3;
                    break;
                case 9:
                    spaceshipSelected = spaceship4;
                    break;
                default:
                    Debug.Log("Invalid value for Intro_Scene.selectedSpaceship");
                    break;
            }

            // Instantiate your corresponding spaceship, according to your chosen NFT
            //Instantiate(spaceshipSelected, transform.position, transform.rotation);

            if (autoFindShips)
            {
                spaceships = FindObjectsOfType<ShipController>();
                if (spaceships.Length > 0)
                {
                    if (!playerSpaceship) playerSpaceship = spaceships[0];
                }
            }
        }
            // Start is called before the first frame update
            void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public async void Claim(int collected)
        {
            // Get the ERC20 Fluxx Token contract
            Thirdweb.Contract contractFluxxTokens = ThirdwebManager.Instance.SDK.GetContract("0xF8692e6bf092E1De97346767372D4ED28839d4F9");
            // Update claim button text
            //claimButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Claiming...";

            var result = await contractFluxxTokens.ERC20.Claim(collected.ToString());

            // hide claim button
            //claimButton.SetActive(false);

            // Update balance
            //UpdateBalance();
        }

        private async void UpdateBalance()
        {
            // Get the ERC20 Fluxx Token contract
            Thirdweb.Contract contractFluxxTokens = ThirdwebManager.Instance.SDK.GetContract("0xF8692e6bf092E1De97346767372D4ED28839d4F9");

            // Set text to user's balance
            var bal = await contractFluxxTokens.ERC20.Balance();

            // Update Token balance
            //balanceText.GetComponent<TMPro.TextMeshProUGUI>().text = bal.displayValue + " " + bal.symbol;
        }

    }
}