using Core.Player;
using UnityEngine;

namespace UI
{
    public class ChangeInput : MonoBehaviour
    {
        [SerializeField] private PlayerInputSettings playerInputSettings;
        public static readonly string InputSettingsKey = "InputSettings";

        // public void ChangeInputType(InputType inputType)
        // {
        //     if (playerInputSettings == null)
        //     {
        //         PlayerPrefs.SetInt(InputSettingsKey, (int)inputType);
        //         PlayerPrefs.Save();
        //     }
        //     else
        //     {
        //         if(inputType == InputType.Arrows)
        //             UseArrows();
        //         else
        //             UseWasd();
        //     }
        // }

        public void UseWasd()
        {
            if (playerInputSettings == null)
            {
                PlayerPrefs.SetInt(InputSettingsKey, (int)InputType.Wasd);
                PlayerPrefs.Save();
            }
            else
            {
                playerInputSettings.ActivateWasd();
            }
        }

    
        public void UseArrows()
        {
            if (playerInputSettings == null)
            {
                PlayerPrefs.SetInt(InputSettingsKey, (int)InputType.Arrows);
                PlayerPrefs.Save();
            }
            else
            {
                playerInputSettings.ActivateArrows();
            }
        }
    
    }
}
