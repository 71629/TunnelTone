using UnityEngine;
using UnityEngine.InputSystem;

namespace TunnelTone.PlayAreaManager
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayAreaManager : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            
            var inGameActionMap = _playerInput.actions.FindActionMap("InGameMap");
            inGameActionMap.FindAction("Tap").started += ctx => OnTap();
            inGameActionMap.FindAction("Tap").performed += ctx => OnHold();
            inGameActionMap.FindAction("Flick").performed += ctx => OnFlick();
        }

        private void OnHold()
        {
            throw new System.NotImplementedException();
        }

        private void OnFlick()
        {
            throw new System.NotImplementedException();
        }

        private void OnTap()
        {
            throw new System.NotImplementedException();
        }
    }
}