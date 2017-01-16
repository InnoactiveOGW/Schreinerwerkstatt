using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;

    class HapticFeedbackController : MonoBehaviour
    {
        ////////////////////////////////////////
        //
        // Haptic Functions

        protected Dictionary<SteamVR_Controller.Device, Coroutine> activeHapticCoroutines = new Dictionary<SteamVR_Controller.Device, Coroutine>();

        public void StartHapticVibration(SteamVR_Controller.Device device, float length, float strength)
        {

            if (activeHapticCoroutines.ContainsKey(device))
            {
                Debug.Log("This device is already vibrating");
                return;
            }

            Coroutine coroutine = StartCoroutine(StartHapticVibrationCoroutine(device, length, strength));
            activeHapticCoroutines.Add(device, coroutine);

        }

        public void StopHapticVibration(SteamVR_Controller.Device device)
        {

            if (!activeHapticCoroutines.ContainsKey(device))
            {
                Debug.Log("Could not find this device");
                return;
            }
            StopCoroutine(activeHapticCoroutines[device]);
            activeHapticCoroutines.Remove(device);
        }

        protected IEnumerator StartHapticVibrationCoroutine(SteamVR_Controller.Device device, float length, float strength)
        {

            for (float i = 0; i < length; i += Time.deltaTime)
            {
                device.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
                yield return null;
            }

            activeHapticCoroutines.Remove(device);
        }

    public void StartHapticPattern(SteamVR_Controller.Device device, float length, float strength) {
        
        // TODO: implement enums for different haptic feedback patterns

    }

    // needs work, not functional atm
    // Mathod should be used to configurate own haptic patterns
    // length: how long the pattern is displayed
    //vibrationCount: how many vibrations
    //vibrationLength: how long each vibration should go for
    //gapLength: how long to wait between vibrations
    //strength: vibration strength from 0-1
    public void StartComplexHapticPattern(SteamVR_Controller.Device device, float strength, int vibrationCount, float vibrationLength, float gapLength, float length)
    {

        // TODO
        strength = Mathf.Clamp01(strength);
        for (int i = 0; i < vibrationCount; i++)
        {
            // if (i != 0) yield return new WaitForSeconds(gapLength);
            StartHapticVibration(device, vibrationLength, strength);
        }
    }
}