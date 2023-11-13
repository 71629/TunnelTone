using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TMPro;
using TunnelTone.Events;
using TunnelTone.Singleton;
using TunnelTone.UI.Dialog;
using TunnelTone.UI.Reference;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace TunnelTone.Network.Account
{
    public class AccountManager : Singleton<AccountManager>
    {
        [SerializeField] private Animator accountInfo;
        
        [Header("Input fields for account registration")]
        [SerializeField] private TMP_InputField registerUsername;
        [SerializeField] private TMP_InputField registerPassword;
        [SerializeField] private TMP_InputField registerConfirmPassword;
        [SerializeField] private TMP_InputField registerEmail;
        
        [Header("Input fields for account login")]
        [SerializeField] private TMP_InputField loginUsername;
        [SerializeField] private TMP_InputField loginPassword;
        
        [Header("Error messages")]
        [SerializeField] private TextMeshProUGUI registerUsernameError;
        [SerializeField] private TextMeshProUGUI registerPasswordError;
        [SerializeField] private TextMeshProUGUI registerConfirmPasswordError;
        [SerializeField] private TextMeshProUGUI registerEmailError;
        
        [Header("Display elements")] 
        [SerializeField] private TextMeshProUGUI user;
        [SerializeField] private Image statusIndicator;
        private static readonly int Dismiss = Animator.StringToHash("Dismiss");

        [Header("Panels")] 
        [SerializeField] private GameObject accountInfoPanel;
        [SerializeField] private GameObject offlinePanel;
        
        [Header("Text File")]
        [SerializeField] private TextAsset accountInfoText;

        private const string APIURL = "https://hashtag071629.com/";
        
        private Credential _credential;

        private void Start()
        {
            _credential = JsonConvert.DeserializeObject<Credential>(accountInfoText.text);
            StartCoroutine(LoginUser(SystemInfo.deviceUniqueIdentifier));
        }
        
        public void ShowAccountInfo()
        {
            if (_credential.uid == -1)
            {
                accountInfoPanel.SetActive(false);
                offlinePanel.SetActive(true);
                return;
            }
            accountInfoPanel.SetActive(true);
            offlinePanel.SetActive(false);
        }
        
        private void ResetErrors()
        {
            registerUsernameError.enabled = false;
            registerUsername.GetComponent<Image>().color = new Color(.65f, .65f, .65f);
            registerPasswordError.enabled = false;
            registerPassword.GetComponent<Image>().color = new Color(.65f, .65f, .65f);
            registerConfirmPasswordError.enabled = false;
            registerConfirmPassword.GetComponent<Image>().color = new Color(.65f, .65f, .65f);
            registerEmailError.enabled = false;
            registerEmail.GetComponent<Image>().color = new Color(.65f, .65f, .65f);
        }

        private void ResetRegisterText()
        {
            registerUsername.text = "";
            registerPassword.text = "";
            registerConfirmPassword.text = "";
            registerEmail.text = "";
        }
        
        private void ResetLoginText()
        {
            loginUsername.text = "";
            loginPassword.text = "";
        }

        public void Register()
        {
            ResetErrors();
            var isValid = true;
            
            if (!registerUsername.text.All(char.IsLetterOrDigit))
            {
                isValid = false;
                registerUsernameError.enabled = true;
                registerUsername.GetComponent<Image>().color = new Color(1, .6f, .6f);
                registerUsernameError.text = "Only half-width alphanumeric characters are allowed";
            }

            if (string.IsNullOrWhiteSpace(registerUsername.text))
            {
                isValid = false;
                registerUsernameError.enabled = true;
                registerUsername.GetComponent<Image>().color = new Color(1, .6f, .6f);
                registerUsernameError.text = "Username cannot be empty";
            }
            
            if (registerPassword.text.Length < 6)
            {
                isValid = false;
                registerPasswordError.enabled = true;
                registerPassword.GetComponent<Image>().color = new Color(1, .6f, .6f);
                registerPasswordError.text = "Password must be at least 6 characters long";
            }
            
            if(registerPassword.text != registerConfirmPassword.text)
            {
                isValid = false;
                registerConfirmPasswordError.enabled = true;
                registerConfirmPassword.GetComponent<Image>().color = new Color(1, .6f, .6f);
                registerConfirmPasswordError.text = "Password does not match";
            }
            
            if (!Regex.IsMatch(registerEmail.text, @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$"))
            {
                isValid = false;
                registerEmailError.enabled = true;
                registerEmail.GetComponent<Image>().color = new Color(1, 0.6f, 0.6f);
                registerEmailError.text = "Invalid email address";
            }
            
            if (!isValid) return;
            Debug.Log("Sent");
            StartCoroutine(RegisterUser(registerUsername.text, registerPassword.text, registerEmail.text));
        }
        
        public void Login()
        {
            StartCoroutine(LoginUser(loginUsername.text, loginPassword.text));
        }

        public void Logout()
        {
            StartCoroutine(UserLogout());
        }
        
        public IEnumerator UserLogout()
        {
            WWWForm form = new();
            form.AddField("uidPost", _credential.uid);
            form.AddField("deviceIDPost", SystemInfo.deviceUniqueIdentifier);

            using var req = UnityWebRequest.Post($"{APIURL}logout", form);
            
            SystemEventReference.Instance.OnDisplayDialog.Trigger("Logout", "Logging out...", Array.Empty<string>(), Array.Empty<Action>(), Dialog.Severity.Info);
            
            yield return req.SendWebRequest();
            SystemEventReference.Instance.OnAbortDialog.Trigger();
            
            Debug.Log(req.result != UnityWebRequest.Result.Success ? req.error : req.downloadHandler.text);
            
            if (req.downloadHandler.text == "LOGOUT_SUCCESS")
            {
                SystemEventReference.Instance.OnDisplayDialog.Trigger("Success", "Logged out successfully", new[]{"OK"}, new Action[]{() => { SystemEventReference.Instance.OnAbortDialog.Trigger(); UIElementReference.Instance.startSlider.interactable = true; }}, Dialog.Severity.Info);
                user.text = "GUEST";
                statusIndicator.color = new Color(.7f, .7f, .7f);
                _credential = null;
                accountInfoText = new TextAsset(JsonConvert.SerializeObject(_credential));
                accountInfo.SetTrigger(Dismiss);
            }
        }
        
        private IEnumerator RegisterUser(string username, string password, string email)
        {
            WWWForm form = new();
            form.AddField("usernamePost", username);
            form.AddField("passwordPost", password);
            form.AddField("eMailPost", email);
            form.AddField("deviceIDPost", SystemInfo.deviceUniqueIdentifier);

            using var req = UnityWebRequest.Post($"{APIURL}register", form);
            
            SystemEventReference.Instance.OnDisplayDialog.Trigger("Account creation", "Connecting to server...", Array.Empty<string>(), Array.Empty<Action>(), Dialog.Severity.Info);

            req.timeout = 10;
            yield return req.SendWebRequest();
            SystemEventReference.Instance.OnAbortDialog.Trigger();

            Debug.Log(req.result != UnityWebRequest.Result.Success ? req.error : req.downloadHandler.text);

            if (req.downloadHandler.text == "DUPLICATE_EMAIL")
            {
                SystemEventReference.Instance.OnDisplayDialog.Trigger("Account creation", $"{email} is already registered", new[]{"OK"}, new Action[]{() => { SystemEventReference.Instance.OnAbortDialog.Trigger(); UIElementReference.Instance.startSlider.interactable = true; }}, Dialog.Severity.Error);
            }
            else if (req.downloadHandler.text == "DUPLICATE_USERNAME")
            {
                SystemEventReference.Instance.OnDisplayDialog.Trigger("Account creation", $"{username} is already registered", new[]{"OK"}, new Action[]{() => { SystemEventReference.Instance.OnAbortDialog.Trigger(); UIElementReference.Instance.startSlider.interactable = true; }}, Dialog.Severity.Error);
            }
            else if (req.downloadHandler.text == "SUCCESS")
            {
                SystemEventReference.Instance.OnDisplayDialog.Trigger("Account creation", "Account created successfully", new[]{"OK"}, new Action[]{() => { SystemEventReference.Instance.OnAbortDialog.Trigger(); UIElementReference.Instance.startSlider.interactable = true; }}, Dialog.Severity.Info);
                accountInfo.SetTrigger(Dismiss);
            }
        }

        private IEnumerator LoginUser(string deviceID)
        {
            WWWForm form = new();
            form.AddField("deviceIDPost", deviceID);
            
            using var req = UnityWebRequest.Post($"{APIURL}autologin", form);

            yield return req.SendWebRequest();
            
            Debug.Log(req.result != UnityWebRequest.Result.Success ? req.error : req.downloadHandler.text);
            
            if (req.downloadHandler.text.Contains("LOGIN_SUCCESS"))
            {
                // output format LOGIN_SUCCESS_{uid:int}_{username:string}
                var data = req.downloadHandler.text.Remove(0, "LOGIN_SUCCESS_".Length);
                user.text = data.Split('_')[1];
                _credential.uid = int.Parse(data.Split('_')[0]);
                statusIndicator.color = new Color(0.07f, 0.8f, 0.13f);
            }
        }
        
        private IEnumerator LoginUser(string username, string password)
        {
            WWWForm form = new();
            form.AddField("usernamePost", username);
            form.AddField("passwordPost", password);
            form.AddField("deviceIDPost", SystemInfo.deviceUniqueIdentifier);

            using var req = UnityWebRequest.Post($"{APIURL}login", form);
            
            SystemEventReference.Instance.OnDisplayDialog.Trigger("Login", "Logging in...", Array.Empty<string>(), Array.Empty<Action>(), Dialog.Severity.Info);
            yield return req.SendWebRequest();
            SystemEventReference.Instance.OnAbortDialog.Trigger();

            Debug.Log(req.result != UnityWebRequest.Result.Success ? req.error : req.downloadHandler.text);

            if (req.downloadHandler.text.Contains("LOGIN_SUCCESS"))
            {
                _credential.uid = int.Parse(req.downloadHandler.text.Remove(0, "LOGIN_SUCCESS_".Length));
                accountInfoText = new TextAsset(JsonConvert.SerializeObject(_credential));
                SystemEventReference.Instance.OnDisplayDialog.Trigger("Success", $"Logged in as {username}", new[]{"OK"}, new Action[]{() => { SystemEventReference.Instance.OnAbortDialog.Trigger(); UIElementReference.Instance.startSlider.interactable = true; }}, Dialog.Severity.Info);
                user.text = username;
                statusIndicator.color = new Color(0.07f, 0.8f, 0.13f);
                accountInfo.SetTrigger(Dismiss);
            }
        }
        
        public void UploadScore(string song, int score, int difficulty)
        {
            WWWForm form = new();
            form.AddField("uidPost", _credential.uid);
            form.AddField("songPost", song);
            form.AddField("scorePost", score);
            form.AddField("difficultyPost", difficulty);
            form.AddField("deviceIDPost", SystemInfo.deviceUniqueIdentifier);
        }
        
        internal class Credential
        {
            public int uid { get; set; }
        }
    }
}