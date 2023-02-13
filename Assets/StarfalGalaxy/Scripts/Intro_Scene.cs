using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace StarfallGalaxy
{
    public class Intro_Scene : MonoBehaviour
    {
        [Header("SETTINGS")]
        public List<Wallet> supportedWallets = new List<Wallet> { Wallet.MetaMask, Wallet.CoinbaseWallet, Wallet.WalletConnect };
        public bool supportSwitchingNetwork = false;

        [Header("UI ELEMENTS (DO NOT EDIT)")]
        // Connecting
        public GameObject connectButton;
        public GameObject connectDropdown;
        public List<WalletButton> walletButtons;
        // Connected
        public GameObject connectedButton;
        public GameObject connectedDropdown;
        public TMP_Text balanceText;
        public TMP_Text walletAddressText;
        public Image walletImage;
        public TMP_Text currentNetworkText;
        public Image currentNetworkImage;
        public Image chainImage;
        // Network Switching
        public GameObject networkSwitchButton;
        public GameObject networkDropdown;
        public GameObject networkButtonPrefab;
        public List<NetworkSprite> networkSprites;

        string address;
        Wallet wallet;

        // UI Initialization
        public GameObject fluxxText;
        public GameObject connectedState;
        public GameObject disconnectedState;
        private Contract contractFluxxTokens;
        private Contract contractMarketplace;
        private Contract contractCollection;
        private Contract contractEditionDrop;

        private List<NFT> owned;
        public GameObject spaceship1;
        public GameObject spaceship2;
        public GameObject spaceship3;
        public GameObject spaceship4;
        public static int selectedSpaceship;

        private void Start()
        {
            address = null;

            if (supportedWallets.Count == 1)
                connectButton.GetComponent<Button>().onClick.AddListener(() => OnConnect(supportedWallets[0]));
            else
                connectButton.GetComponent<Button>().onClick.AddListener(() => OnClickDropdown());


            foreach (WalletButton wb in walletButtons)
            {
                if (supportedWallets.Contains(wb.wallet))
                {
                    wb.walletButton.SetActive(true);
                    wb.walletButton.GetComponent<Button>().onClick.AddListener(() => OnConnect(wb.wallet));
                }
                else
                {
                    wb.walletButton.SetActive(false);
                }
            }

            connectButton.SetActive(true);
            connectedButton.SetActive(false);

            connectDropdown.SetActive(false);
            connectedDropdown.SetActive(false);

            networkSwitchButton.SetActive(supportSwitchingNetwork);
            networkDropdown.SetActive(false);
        }

        // Connecting

        public async void OnConnect(Wallet _wallet)
        {
            try
            {
                address = await ThirdwebManager.Instance.SDK.wallet.Connect(
                   new WalletConnection()
                   {
                       provider = GetWalletProvider(_wallet),
                       chainId = (int)ThirdwebManager.Instance.chain,
                   });
                wallet = _wallet;
                OnConnected();
                print($"Connected successfully to: {address}");
            }
            catch (Exception e)
            {
                print($"Error Connecting Wallet: {e.Message}");
            }
        }

        async void OnConnected()
        {
            try
            {
                Chain _chain = ThirdwebManager.Instance.chain;
                CurrencyValue nativeBalance = await ThirdwebManager.Instance.SDK.wallet.GetBalance();
                balanceText.text = $"{nativeBalance.value.ToEth()} {nativeBalance.symbol}";
                walletAddressText.text = address.ShortenAddress();
                currentNetworkText.text = ThirdwebManager.Instance.chainIdentifiers[_chain];
                currentNetworkImage.sprite = networkSprites.Find(x => x.chain == _chain).sprite;
                connectButton.SetActive(false);
                connectedButton.SetActive(true);
                connectDropdown.SetActive(false);
                connectedDropdown.SetActive(false);
                networkDropdown.SetActive(false);
                walletImage.sprite = walletButtons.Find(x => x.wallet == wallet).icon;
                chainImage.sprite = networkSprites.Find(x => x.chain == _chain).sprite;

                ShowConnectedState();
                GetAllContracts();
                LoadBalance();
                CheckFluxxPrices();
                ShowMarketPlace();
            }
            catch (Exception e)
            {
                print($"Error Fetching Native Balance: {e.Message}");
            }
        }

        private void ShowConnectedState()
        {
            connectedState.SetActive(true);
            disconnectedState.SetActive(false);
        }

        private void ShowDisConnectedState()
        {
            connectedState.SetActive(false);
            disconnectedState.SetActive(true);
        }

        private void GetAllContracts()
        {
            // Get the ERC20 Fluxx Token contract
            contractFluxxTokens = ThirdwebManager.Instance.SDK.GetContract("0xF8692e6bf092E1De97346767372D4ED28839d4F9");

            // Get the NFT collection contract
            contractCollection = ThirdwebManager.Instance.SDK.GetContract("0xF3e5782e277545b9389B860018F71B8736Cd7C39");

            // Get the Marketplace contract
            contractMarketplace = ThirdwebManager.Instance.SDK.GetContract("0x9977f1AAaaFCEade945a92EDd2f45097A3108Dcf");

            // Get the Edition Drop
            contractEditionDrop = ThirdwebManager.Instance.SDK.GetContract("0x2cD6d09a9c8f09821BB7188bf008DF529afB2D7E");
        }

        public async void LoadBalance()
        {
            // We are about ot check your balance
            fluxxText.GetComponent<TMPro.TextMeshProUGUI>().text = "Loading...";

            // Get your supply
            CurrencyValue fluxxBalance = await contractFluxxTokens.ERC20.BalanceOf(address);

            // Set balance text
            fluxxText.GetComponent<TMPro.TextMeshProUGUI>().text = "Your balance: " + fluxxBalance.displayValue + " " + fluxxBalance.symbol;
        }

        public async void ClaimNFT(string tokenId)
        {
            Debug.Log("Claim button clicked");
            fluxxText.GetComponent<TMPro.TextMeshProUGUI>().text = "Claiming...";

            // claim
            var canClaim = await contractEditionDrop.ERC1155.claimConditions.CanClaim(tokenId, 1);
            if (canClaim)
            {
                try
                {
                    var result = await contractEditionDrop.ERC1155.Claim(tokenId, 1);
                    var newSupply = await contractEditionDrop.ERC1155.TotalSupply(tokenId);
                    fluxxText.GetComponent<TMPro.TextMeshProUGUI>().text = "Claim successful! New supply: " + newSupply;
                }
                catch (System.Exception e)
                {
                    fluxxText.GetComponent<TMPro.TextMeshProUGUI>().text = "Claim Failed: " + e.Message;
                }
            }
            else
            {
                fluxxText.GetComponent<TMPro.TextMeshProUGUI>().text = "Can't claim";
            }
        }



        public async void MintSpaceship(string tokenId)
        {
            // buy listing
            try
            {
                var result = await contractMarketplace.marketplace.BuyListing(tokenId, 1);
                // We are about ot check your balance
                fluxxText.GetComponent<TMPro.TextMeshProUGUI>().text = "Processing...";
                if (result.isSuccessful())
                {
                    if (tokenId == "6")
                    {
                        CheckIfOwned(spaceship1, owned, 6, "3");
                    }
                    else if (tokenId == "7")
                    {
                        CheckIfOwned(spaceship2, owned, 7, "4");
                    }
                    else if (tokenId == "8")
                    {
                        CheckIfOwned(spaceship3, owned, 8, "5");
                    }
                    else if (tokenId == "9")
                    {
                        CheckIfOwned(spaceship4, owned, 9, "6");
                    }
                    // Update the balance
                    LoadBalance();
                }
                else
                {
                    fluxxText.GetComponent<TMPro.TextMeshProUGUI>().text = "Error. Try again.";
                }
            }
            catch (System.Exception e)
            {
                //fluxxText.GetComponent<TMPro.TextMeshProUGUI>().text = "Error Buying listing: " + e.Message;
                fluxxText.GetComponent<TMPro.TextMeshProUGUI>().text = "Error Buying listing: Make sure you have enough Fluxx and try again";
            }
        }

        public async void ShowMarketPlace()
        {
            // First, check to see if the you own the NFT
            owned = await contractCollection.ERC1155.GetOwned();

            //Check to see if you own an NFT from the collection
            CheckIfOwned(spaceship1, owned, 6, "3");
            CheckIfOwned(spaceship2, owned, 7, "4");
            CheckIfOwned(spaceship3, owned, 8, "5");
            CheckIfOwned(spaceship4, owned, 9, "6");
        }

        // Check if player already owns any of the NFTs
        public async void CheckIfOwned(GameObject obj, List<NFT> NftOwned, int st, string token)
        {
            // if ownedNFTs contains a token with the same ID as the listing, then you own it
            //bool ownNFT = NftOwned.Exists(nft => nft.metadata.id == st.ToString());
            bool ownNFT = NftOwned.Exists(nft => nft.metadata.id == token);

            if (ownNFT)
            {
                // Apply the condition for owning the NFT
                var text1 = obj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                text1.text = "Play the game";

                //Set the on click to start the game by loading mane scene
                obj.GetComponent<UnityEngine.UI.Button>().onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
                obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    selectedSpaceship = st;
                    SceneManager.LoadSceneAsync("Controller1");
                });
            }
            else
            {
                // Once we have the price, we update the text to the price
                var price = await contractMarketplace.marketplace.GetListing(st.ToString());

                // Set the price in the button to buy
                var text1 = obj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                text1.text = "Buy:" + " " + price.buyoutCurrencyValuePerToken.displayValue +
                    " " + price.buyoutCurrencyValuePerToken.symbol;
            }
        }

        // Check and display Fluxx prices
        public async void CheckFluxxPrices()
        {
            // Get the ERC20 Fluxx Token contract
            contractFluxxTokens = ThirdwebManager.Instance.SDK.GetContract("0xF8692e6bf092E1De97346767372D4ED28839d4F9");

            // Load the claim condition information from the contract
            var activeClaimCondition = await contractFluxxTokens.ERC20.claimConditions.GetActive();

            // Get currency price
            var price = float.Parse(activeClaimCondition.currencyMetadata.displayValue);

            // Get game objects "Price 1", "Price 2", and "Price 3"
            var price1 = GameObject.Find("Price100Fxx");

            // Set the text of each price to the price of the token (which is 0.001 MATIC)
            price1.GetComponent<TMPro.TextMeshProUGUI>().text = (100 * price).ToString().Substring(0, 5) + " MATIC";
        }

        // Method to buy Fluxx tokens
        public async void BuyTokens(string amount)
        {
            // Buying process, sign to be happy
            fluxxText.GetComponent<TMPro.TextMeshProUGUI>().text = "Aproving transaction...";
            try
            {
                // Buy and mint Fluxx tokens!
                var result = await contractFluxxTokens.ERC20.Claim(amount);
                if (result.isSuccessful())
                {
                    fluxxText.GetComponent<TMPro.TextMeshProUGUI>().text = "Purchase successful!";
                }
            }
            catch (Exception e)
            {
                fluxxText.GetComponent<TMPro.TextMeshProUGUI>().text = "Buying Error: " + e.Message;
            }
            
            // Update the balance
            LoadBalance();
        }

        // Disconnecting
        public async void OnDisconnect()
        {
            try
            {
                await ThirdwebManager.Instance.SDK.wallet.Disconnect();
                OnDisconnected();
                print($"Disconnected successfully.");
                ShowDisConnectedState();

            }
            catch (Exception e)
            {
                print($"Error Disconnecting Wallet: {e.Message}");
            }
        }

        void OnDisconnected()
        {
            address = null;
            connectButton.SetActive(true);
            connectedButton.SetActive(false);
            connectDropdown.SetActive(false);
            connectedDropdown.SetActive(false);
        }

        // Switching Network
        public async void OnSwitchNetwork(Chain _chain)
        {
            try
            {
                ThirdwebManager.Instance.chain = _chain;
                await ThirdwebManager.Instance.SDK.wallet.SwitchNetwork((int)_chain);
                OnConnected();
                print($"Switched Network Successfully: {_chain}");

            }
            catch (Exception e)
            {
                print($"Error Switching Network: {e.Message}");
            }
        }

        // UI
        public void OnClickDropdown()
        {
            if (String.IsNullOrEmpty(address))
                connectDropdown.SetActive(!connectDropdown.activeInHierarchy);
            else
                connectedDropdown.SetActive(!connectedDropdown.activeInHierarchy);
        }

        public void OnClickNetworkSwitch()
        {
            if (networkDropdown.activeInHierarchy)
            {
                networkDropdown.SetActive(false);
                return;
            }

            networkDropdown.SetActive(true);

            foreach (Transform child in networkDropdown.transform)
                Destroy(child.gameObject);

            foreach (Chain chain in Enum.GetValues(typeof(Chain)))
            {
                if (chain == ThirdwebManager.Instance.chain || !ThirdwebManager.Instance.supportedNetworks.Contains(chain))
                    continue;

                GameObject networkButton = Instantiate(networkButtonPrefab, networkDropdown.transform);
                networkButton.GetComponent<Button>().onClick.RemoveAllListeners();
                networkButton.GetComponent<Button>().onClick.AddListener(() => OnSwitchNetwork(chain));
                networkButton.transform.Find("Text_Network").GetComponent<TMP_Text>().text = ThirdwebManager.Instance.chainIdentifiers[chain];
                networkButton.transform.Find("Icon_Network").GetComponent<Image>().sprite = networkSprites.Find(x => x.chain == chain).sprite;
            }
        }

        // Utility
        WalletProvider GetWalletProvider(Wallet _wallet)
        {
            switch (_wallet)
            {
                case Wallet.MetaMask:
                    return WalletProvider.MetaMask;
                case Wallet.CoinbaseWallet:
                    return WalletProvider.CoinbaseWallet;
                case Wallet.WalletConnect:
                    return WalletProvider.WalletConnect;
                default:
                    throw new UnityException($"Wallet Provider for wallet {_wallet} unimplemented!");
            }
        }
    }

}