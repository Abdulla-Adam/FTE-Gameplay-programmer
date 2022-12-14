using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;
    
    public GameObject qualitySettingsPanel;
    public GameObject audioSettingsPanel;
    public GameObject keySettingsPanel;

    public GameObject PushButtonPanel;

    public GameObject ObjectivePopup;

    public Text ObjectivePopupText;

    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void UpdateObjectivePopup()
    {
        int CurrentLevel = LevelManager.Instance.CurrentLevel;
        int CurrentObjective = LevelManager.Instance.CurrentObjective;
        ObjectivePopupText.text = LevelManager.Instance.Levels[CurrentLevel].Objectives[CurrentObjective].ObjectiveCompletionText;

    }

    public void ShowObjectivePopup()
    {
        ObjectivePopup.SetActive(true);
    }

    public void CloseObjectivePopup()
    {
        ObjectivePopup.SetActive(false);
    }

    public void PauseButton()
    {
        ThirdPersonMovement.IsInputEnabled = false;
        pauseMenuPanel.SetActive(true);
    }

    public void ResumeButton()
    {
        ThirdPersonMovement.IsInputEnabled = true;
        pauseMenuPanel.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SettingsButton()
    {
        settingsPanel.SetActive(true);

    }

   public void QualitySettingsToggle()
   {
        qualitySettingsPanel.SetActive(true);
        audioSettingsPanel.SetActive(false);
        keySettingsPanel.SetActive(false);
   } 
   public void AudioSettingsToggle()
   {
        audioSettingsPanel.SetActive(true);
        qualitySettingsPanel.SetActive(false);        
        keySettingsPanel.SetActive(false);
    } 
   public void KeySettingsToggle()
   {
        keySettingsPanel.SetActive(true);
        qualitySettingsPanel.SetActive(false);
        audioSettingsPanel.SetActive(false);        
    }

    public void BackButton()
    {
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }
}
