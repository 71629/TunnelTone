using System;
using System.Collections.Generic;
using TMPro;
using TunnelTone.Events;
using UnityEngine;
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

        private void Start()
        {
            SystemEventReference.Instance.OnDisplayDialog.AddListener(DisplayDialog);
            SystemEventReference.Instance.OnAbortDialog.AddListener(AbortDialog);
        }

        private void DisplayDialog(params object[] param)
        {
            title.text = (string)param[0];
            message.text = (string)param[1];
            var optionText = (string[])param[2];
            //Pass the action to be taken when the option is clicked
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
        
        private void AbortDialog(params object[] param)
        {
            animator.SetTrigger(Dismiss);
            Invoke(nameof(DisableAnimatorAndCanvas), 0.5f);
        }
        
        private void DisableAnimatorAndCanvas()
        {
            animator.enabled = false;
            GetComponent<Canvas>().enabled = false;
            for(var i = 0; i < foot.childCount; i++)
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
}