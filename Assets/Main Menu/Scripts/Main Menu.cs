using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
//using Unity.Rendering.Universal;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    public GameObject Main;
    public GameObject Settings;
    public TMP_Dropdown qualityDropdown;

    private void Awake()
    {
        Settings.SetActive(false);
        Main.SetActive(true);
        Time.timeScale = 0;
        QualitySettings.SetQualityLevel(2, true);
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        Main.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
        print("Exiting game...");
    }

    public void SwitchQualityLevel()
    {
        print(QualitySettings.GetQualityLevel());
        int index = qualityDropdown.value;

        if (index == 0)
        {
            QualitySettings.SetQualityLevel(0, true);
            print("Quality set to low");
        }
        else if (index == 1)
        {
            QualitySettings.SetQualityLevel(1, true);
            print("Quality set to medium");
        }
        else if (index == 2)
        {
            QualitySettings.SetQualityLevel(2, true);
            print("Quality set to high");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
