using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject settingPanel;
    private bool isSettingPanelActive = false;
    public Button[] mainMenuButton;

    public void PlayButtonClicked()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void SettingButtonClicked()
    {
       isSettingPanelActive = !isSettingPanelActive;
        if (isSettingPanelActive)
        {
            settingPanel.SetActive(true);
            DisableMainMenuButton();
        }
        else
        {
            settingPanel.SetActive(false);
            EnableMainMenuButton();
        }      
    }
    private void DisableMainMenuButton()
    {
        foreach ( Button button in mainMenuButton ) 
        { 
            button.gameObject.SetActive(false);
        }
    }
    private void EnableMainMenuButton()
    {
        foreach( Button button in mainMenuButton)
        {
            button.gameObject.SetActive(true);
        }
    }
    public void ExitButtonClicked()
    {   
        {
            Application.Quit();
        }
    }
}
   
