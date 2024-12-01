using System.Collections.Generic;
using System.Linq;
using Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Controller
{
    public class EasterEggController : MonoBehaviour
    {
        [SerializeField] private string lastInputString;
        private Keyboard _keyboard;

        [SerializeField] private bool isListenining;

        [SerializeField] private List<EasterEgg> eggs;
        [SerializeField] private GameObject eggAlert;
        [SerializeField] private GameObject eggAlertAnchor;

        private void Start()
        {
            isListenining = false;
            _keyboard = Keyboard.current;

            foreach (EasterEgg egg in eggs)
            {
                egg.Initialize();
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            if (_keyboard.enterKey.wasPressedThisFrame)
            {
                isListenining = !isListenining;
                if (isListenining) return;
                if (!isListenining) ProcessEasterEgg();
            }

            if (isListenining)
            {
                foreach (var key in _keyboard.allKeys.Where(key => key.wasPressedThisFrame))
                {
                    lastInputString += key.displayName;
                }
            }
        }

        void ProcessEasterEgg()
        {
            foreach (var egg in eggs)
            {
                if (lastInputString.ToLower() == egg.triggerCode)
                {
                    egg.Trigger();

                    //Spawn Alert
                    var newAlert = Instantiate(eggAlert, eggAlertAnchor.transform).GetComponent<Alert>();
                    var status = egg.isActive ? "Activated" : "Deactivated";
                    newAlert.SetText($"{egg.displayName} {status}");
                    newAlert.SetColour(egg.isActive ? Color.cyan : Color.red);
                }
            }
            lastInputString = string.Empty;
        }
    }
}