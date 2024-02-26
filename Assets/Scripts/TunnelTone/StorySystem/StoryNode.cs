using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.StorySystem
{
    public class StoryNode : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TMP_Text nodeIDText;
        [SerializeField] private Image icon;

        [Header("Node Information")]
        [SerializeField] private Story content;
        [SerializeField] private string nodeID;

        [SerializeField] private Sprite readIcon;
        private bool isRead;

        private void OnEnable()
        {
            nodeIDText.text = nodeID;
        }

        public void Expand()
        {
            Debug.Log("Expand");
            StoryEntry.OnStoryExpand(content);
        }
    }
}