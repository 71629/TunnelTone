using System;
using System.Collections.Generic;
using TMPro;
using TunnelTone.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace TunnelTone.UI.Dialog
{
    public class Dialog : MonoBehaviour
    {
        [SerializeField] private Image backdrop;
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI message;
        [SerializeField] private Object dialogOptionPrefab;
        [SerializeField] private Transform foot;

        [SerializeField] private Sprite info;
        [SerializeField] private Sprite warning;
        [SerializeField] private Sprite error;
        
        private List<GameObject> _dialogOptions;
        
        private static readonly int Dismiss = Animator.StringToHash("Dismiss");
        internal static UnityEvent<string, string, DialogOption[]> OnDisplayDialog = new();
        internal static UnityEvent OnAbortDialog = new();

        private void Start()
        {
            SystemEvent.OnDisplayDialog.AddListener(DisplayDialogEvent);
            SystemEvent.OnAbortDialog.AddListener(AbortDialogEvent);
            OnDisplayDialog.AddListener(DisplayDialog);
            OnAbortDialog.AddListener(AbortDialog);
        }
        
        private void DisplayDialog(string title, string message, IEnumerable<DialogOption> options)
        {
            CancelInvoke();
            PurgeOptions();
            foreach (var option in options)
            {
                var gb = (GameObject)Instantiate(dialogOptionPrefab, foot);
                gb.GetComponentInChildren<TextMeshProUGUI>().text = option.text;
                gb.GetComponent<Button>().onClick.AddListener(() => option.action.Invoke());
            }
            
            GetComponent<Canvas>().enabled = true;
            animator.enabled = true;
            animator.Rebind();
        }

        private void DisplayDialogEvent(params object[] param)
        {
            CancelInvoke();
            PurgeOptions();
            title.text = (string)param[0];
            message.text = (string)param[1];
            var optionText = (string[])param[2];
            //Pass the action previewDuration be taken when the option is clicked
            var options = (Action[])param[3];
            backdrop.color = SeverityColor[(Severity)param[4]];

            foreach (var option in options)
            {
                var gb = (GameObject)Instantiate(dialogOptionPrefab, foot);
                gb.GetComponentInChildren<TextMeshProUGUI>().text = optionText[Array.IndexOf(options, option)];
                gb.GetComponent<Button>().onClick.AddListener(() => option.Invoke());
            }
            
            GetComponent<Canvas>().enabled = true;
            animator.enabled = true;
            animator.Rebind();
        }
        
        private void AbortDialogEvent(params object[] param)
        {
            animator.SetTrigger(Dismiss);
            Invoke(nameof(DisableAnimatorAndCanvas), 0.5f);
        }
        
        private void AbortDialog()
        {
            animator.SetTrigger(Dismiss);
            Invoke(nameof(DisableAnimatorAndCanvas), 0.5f);
        }
        
        private void DisableAnimatorAndCanvas()
        {
            animator.enabled = false;
            GetComponent<Canvas>().enabled = false;
            PurgeOptions();
        }

        private void PurgeOptions()
        {
            for (var i = 0; i < foot.childCount; i++)
                Destroy(foot.GetChild(i).gameObject);
        }

        public enum Severity
        {
            Info,
            Warning,
            Error
        }
        
        private static readonly Dictionary<Severity, Color> SeverityColor = new Dictionary<Severity, Color>
        {
            {Severity.Info, Color.black},
            {Severity.Warning, new Color(1f, 1f, 0.17f)},
            {Severity.Error, new Color(1f, 0.26f, 0.26f)}
        };
    }

    internal class DialogOption
    {
        internal string text;
        internal Action action;
    }
}