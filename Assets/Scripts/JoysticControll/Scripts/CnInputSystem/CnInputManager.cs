using System.Collections.Generic;
using UnityEngine;

namespace JoystickControls
{
    /// <summary>
    /// Common input manager class
    /// Can be used instead of Input logic, as it replicates the standard behaviour but adds additional logic
    /// </summary>
    public class JoystickInputManager
    {
        private static JoystickInputManager _instance;

        private static JoystickInputManager Instance
        {
            get { return _instance ?? (_instance = new JoystickInputManager()); }
        }

        private JoystickInputManager() { }

        /// <summary>
        /// Dictionary of virtual axis
        /// Every axis can be mapped to a number of actual virtual axis, 
        /// as with standard Unity Input system where you can create different buttons for, say, "Horizontal" axis
        /// </summary>
        private Dictionary<string, List<VirtualAxis>> _virtualAxisDictionary =
            new Dictionary<string, List<VirtualAxis>>();
        
        /// <summary>
        /// Additional logic for touch retreival
        /// It's possible to add some reflection-based emulated touches
        /// </summary>
        public static int TouchCount
        {
            get
            {
                return Input.touchCount;
            }
        }

        /// <summary>
        /// Additional logic for touch retreival
        /// It's possible to add some reflection-based emulated touches
        /// </summary>
        public static Touch GetTouch(int touchIndex)
        {
            return Input.GetTouch(touchIndex);
        }

        /// <summary>
        /// GetAxis method for getting current values for any desired axis
        /// </summary>
        /// <param name="axisName">The name of the axis to get value from</param>
        /// <returns>
        /// Current value of FIRST NON ZERO axis that are registered for that name
        /// ZERO if non if the virtual axis are being tweaked
        /// </returns>
        public static float GetAxis(string axisName)
        {
            return GetAxis(axisName, false);
        }

        /// <summary>
        /// "Copy" of the Input.GetAxisRaw method
        /// </summary>
        /// <param name="axisName">The name of the axis to get value from</param>
        /// <returns>
        /// Current value of FIRST NON ZERO axis that are registered for that name
        /// ZERO if non if the virtual axis are being tweaked
        /// </returns>
        public static float GetAxisRaw(string axisName)
        {
            return GetAxis(axisName, true);
        }

        /// <summary>
        /// Common private method for getting the axis values
        /// </summary>
        /// <param name="axisName">The name of the axis to get value from</param>
        /// <param name="isRaw">Whether the method sould return the raw value of the axis</param>
        /// <returns></returns>
        private static float GetAxis(string axisName, bool isRaw)
        {
            // If we have the axis registered as virtual, we call the retreival logic
            if (AxisExists(axisName))
            {
                return GetVirtualAxisValue(Instance._virtualAxisDictionary[axisName], axisName, isRaw);
            }

            // If we don't have the desired virtual axis registered, we just fallback to the default Unity Input behaviour
            return isRaw ? Input.GetAxisRaw(axisName) : Input.GetAxis(axisName);
        }

        /// <summary>
        /// Check whether the specified axis exists
        /// </summary>
        /// <param name="axisName">Name of the axis to check</param>
        /// <returns>Does this axis exist?</returns>
        public static bool AxisExists(string axisName)
        {
            return Instance._virtualAxisDictionary.ContainsKey(axisName);
        }

        /// <summary>
        /// Registers the provided virtual axis
        /// </summary>
        /// <param name="virtualAxis">Virtual axis to register</param>
        public static void RegisterVirtualAxis(VirtualAxis virtualAxis)
        {
            // If it's the first such virtual axis, create a new list for that axis name
            if (!Instance._virtualAxisDictionary.ContainsKey(virtualAxis.Name))
            {
                Instance._virtualAxisDictionary[virtualAxis.Name] = new List<VirtualAxis>();
            }

            Instance._virtualAxisDictionary[virtualAxis.Name].Add(virtualAxis);
        }

        /// <summary>
        /// Unregisters the provided virtual axis
        /// </summary>
        /// <param name="virtualAxis">Virtual axis to unregister</param>
        public static void UnregisterVirtualAxis(VirtualAxis virtualAxis)
        {
            // If it's the first such virtual axis, create a new list for that axis name
            if (Instance._virtualAxisDictionary.ContainsKey(virtualAxis.Name))
            {
                if (!Instance._virtualAxisDictionary[virtualAxis.Name].Remove(virtualAxis))
                {
                    Debug.LogError("Requested axis " + virtualAxis.Name + " exists, but there's no such virtual axis that you're trying to unregister");                    
                }
            }
            else
            {
                Debug.LogError("Trying to unregister an axis " + virtualAxis.Name + " that was never registered");
            }
        }

        /// <summary>
        /// Private method that get's the value of the first non-zero virtual axis, registered with the specified name
        /// </summary>
        /// <param name="virtualAxisList">List of virtual axis to search through</param>
        /// <param name="axisName">Name of the axis (for the standard Input behaviour)</param>
        /// <param name="isRaw">Whether the method should return the Raw value of the axis</param>
        /// <returns></returns>
        private static float GetVirtualAxisValue(List<VirtualAxis> virtualAxisList, string axisName, bool isRaw)
        {
            // The method is really straightforward here
            // First, we check the standard Input.GetAxis method
            // If it's not zero, we return the value
            // If it IS zero, we return first non-zero value of any of the passed virtual axis
            // Or zero if all of them are zero

            float axisValue = isRaw ? Input.GetAxisRaw(axisName) : Input.GetAxis(axisName);
            if (!Mathf.Approximately(axisValue, 0f))
            {
                return axisValue;
            }

            for (int i = 0; i < virtualAxisList.Count; i++)
            {
                var currentAxisValue = virtualAxisList[i].Value;
                if (!Mathf.Approximately(currentAxisValue, 0f))
                {
                    return currentAxisValue;
                }
            }

            return 0f;
        }
    }
}

