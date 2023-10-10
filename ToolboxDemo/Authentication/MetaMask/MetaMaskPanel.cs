#if NEFTA_INTEGRATION_METAMASK
using System;
using System.Text;
using MetaMask.Models;
using MetaMask.Transports.Unity.UI;
using Nefta.Core;
using Nefta.ToolboxSdk;
using Nefta.ToolboxSdk.Authorization.AuthProviders;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Nefta.ToolboxDemo.Authentication.MetaMask
{
    public class MetaMaskPanel : DemoPanel
    {
        [SerializeField] private GameObject _welcomePanel;
        [SerializeField] private GameObject _connectedPanel;
        [SerializeField] private GameObject _signUpPanel;
        [SerializeField] private Text _walletAddress;
        [SerializeField] private InputField _nameInput;
        [SerializeField] private InputField _emailInput;
        [SerializeField] private Button _singUpButton;

#if NEFTA_INTEGRATION_METAMASK
        private enum Screens
        {
            Welcome,
            Connected,
            SignUp
        }
        
        private MetaMaskWrapper _metaMaskUnity;
        private string _email;
        private string _name;

        protected override void OnInit()
        {
            _metaMaskUnity = gameObject.AddComponent<MetaMaskWrapper>();
            _metaMaskUnity.Init();

            var qrImagePrefab = Resources.Load<MetaMaskUnityUIQRImage>("MetaMask/Prefabs/MetaMask QR RawImage");
            var qrImage = Instantiate(qrImagePrefab, _welcomePanel.transform);
            var qrImageTransform = (RectTransform) qrImage.transform;
            qrImageTransform.anchoredPosition = new Vector2(0, 50);
            qrImageTransform.localScale = new Vector3(2, 2, 2);

            var transportBroadcaster = gameObject.AddComponent<MetaMaskTransportWrapper>();
            transportBroadcaster.AssignListener(qrImage.gameObject);
            
            _metaMaskUnity.Initialize();
            _metaMaskUnity.Wallet.WalletAuthorized += OnWalletAuthorized;
            _metaMaskUnity.Wallet.WalletDisconnected += OnWalletDisconnected;
            _metaMaskUnity.Wallet.WalletReady += OnWalletReady;
            _metaMaskUnity.Wallet.AccountChanged += OnAccountChanged;
            _metaMaskUnity.Wallet.WalletPaused += OnWalletPaused;
            _metaMaskUnity.Wallet.EthereumRequestResultReceived += OnEthereumRequestResultReceived;

            _singUpButton.onClick.AddListener(SignUp);
            
            SetScreen(Screens.Welcome);
        }

        public override void Open()
        {
            base.Open();

            _name = null;
            _email = null;
            _nameInput.text = _name;
            _emailInput.text = _email;
            
            _metaMaskUnity.Wallet.Connect();
        }
        
        private void SetScreen(Screens screen)
        {
            _welcomePanel.SetActive(screen == Screens.Welcome);
            _connectedPanel.SetActive(screen == Screens.Connected);
            _signUpPanel.SetActive(screen == Screens.SignUp);
        }

        private void SignUp()
        {
            ConfirmWalletWithSign(true);
        }
        
        private void OnWalletAuthorized(object sender, EventArgs e)
        {
            NeftaCore.Log("OnWalletAuthorized");
        }

        private void OnWalletDisconnected(object sender, EventArgs e)
        {
            NeftaCore.Log("OnWalletDisconnected");
            SetScreen(Screens.Welcome);
        }

        private void OnWalletReady(object sender, EventArgs e)
        {
            NeftaCore.Log("OnWalletReady");
            
            SetScreen(Screens.Connected);

            ConfirmWalletWithSign(false);
        }

        private void OnAccountChanged(object sender, EventArgs e)
        {
            _walletAddress.text = _metaMaskUnity.Wallet.SelectedAddress;
        }

        private void OnWalletPaused(object sender, EventArgs e)
        {
            NeftaCore.Log("OnWalletPaused");
        }

        private void OnEthereumRequestResultReceived(object sender, EventArgs e)
        {
            NeftaCore.Log("OnEthereumRequestResultReceived");
        }
        
        public void OpenDeepLink() {
            NeftaCore.Log("OpenDeepLink");
        }
        
        private void ConfirmWalletWithSign(bool isSignUp)
        {
            _name = _nameInput.text;
            _email = _emailInput.text;
            
            var timeStamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            var message = "neftaSign_" + timeStamp;
            var request = new MetaMaskEthereumRequest()
            {
                Method = "personal_sign",
                Parameters = new string[] { _metaMaskUnity.Wallet.SelectedAddress, message }
            };
            NeftaCore.Log($"Sign isSignUp:{isSignUp} message:{message}");
            if (isSignUp)
            {
                _metaMaskUnity.Wallet.Socket.On($"message-{_metaMaskUnity.Wallet.Session.Data.ChannelId}", OnSignUpMessageReceived);
            }
            else
            {
                _metaMaskUnity.Wallet.Socket.On($"message-{_metaMaskUnity.Wallet.Session.Data.ChannelId}", OnLogInMessageReceived);
            }
            _metaMaskUnity.Wallet.Request(request);
        }

        private void OnLogInMessageReceived(string response)
        {
            OnMessageReceived(response, false);
        }
        
        private void OnSignUpMessageReceived(string response)
        {
            OnMessageReceived(response, true);
        }
        
        private void OnMessageReceived(string response, bool isSignUp)
        {
            try
            {
                var byteResponse = Encoding.UTF8.GetBytes(response);
                var responseArray = Toolbox.Instance.RestHelper.Deserialize<MetaMaskResponseMessage[]>(byteResponse);
                if (responseArray.Length != 1)
                {
                    return;
                }
                string decrypted = _metaMaskUnity.Wallet.Session.DecryptMessage(responseArray[0]._message);
                var byteDecrypted = Encoding.UTF8.GetBytes(decrypted);
                var decryptedMessage = Toolbox.Instance.RestHelper.Deserialize<MetaMaskDecryptedMessage>(byteDecrypted);
                NeftaCore.Log($"Sign successful: {decryptedMessage._data._result}");

                if (isSignUp)
                {
                    Toolbox.Instance.Authorization.SignUpGamer(_email, _name, _metaMaskUnity.Wallet.SelectedAddress, OnSignUpComplete);     
                }
                else
                {
                    Toolbox.Instance.Authorization.SignUpGamerWithWallet(_metaMaskUnity.Wallet.SelectedAddress, OnLoginComplete);
                }
            }
            catch (Exception e)
            {
                NeftaCore.Warn($"OnMessageReceived error: {e.Message}");
            }
        }

        private void OnSignUpComplete(bool isSuccess)
        {
            if (isSuccess)
            {
                _masterPanel.OpenConfirmCode(_email, false);
            }
            else
            {
                _statusPanel.Show("Error signing up full user", StatusPanel.Types.Error);
            }
        }
        
        private void OnLoginComplete(NeftaUser user)
        {
            if (user != null)
            {
                _masterPanel.OpenUser();
            }
            else
            {
                SetScreen(Screens.SignUp);
            }
        }
#endif
    }
}