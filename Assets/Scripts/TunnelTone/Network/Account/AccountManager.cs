using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using TunnelTone.Events;
using TunnelTone.Singleton;
using TunnelTone.UI.Dialog;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace TunnelTone.Network.Account
{
    public class AccountManager : Singleton<AccountManager>
    {
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
        
        private const string RegisterUserURL = "https://hashtag071629.com/register";
        private const string LoginUserURL = "https://hashtag071629.com/login";

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
        
        private IEnumerator RegisterUser(string username, string password, string email)
        {
            WWWForm form = new();
            form.AddField("usernamePost", username);
            form.AddField("passwordPost", password);
            form.AddField("eMailPost", email);

            using var req = UnityWebRequest.Post(RegisterUserURL, form);
            
            SystemEventReference.Instance.OnDisplayDialog.Trigger("Account creation", "Connecting to server...", Array.Empty<string>(), Array.Empty<Action>(), Dialog.Severity.Info);

            req.timeout = 12;
            yield return req.SendWebRequest();
            SystemEventReference.Instance.OnAbortDialog.Trigger();

            Debug.Log(req.result != UnityWebRequest.Result.Success ? req.error : "Form post complete!");
        }
        
        private IEnumerator LoginUser(string username, string password)
        {
            WWWForm form = new();
            form.AddField("usernamePost", username);
            form.AddField("passwordPost", password);

            using var req = UnityWebRequest.Post(LoginUserURL, form);
            
            yield return req.SendWebRequest();

            Debug.Log(req.result != UnityWebRequest.Result.Success ? req.error : "Form post complete!");
        }
    }
}