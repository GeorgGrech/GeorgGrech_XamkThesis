using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettingsUI : MonoBehaviour
{

    public Toggle enableDDAToggle;
    public Toggle physicsDDAToggle;
    public TMP_InputField resultsPathField;
    
    private GameSettings gameSettings;

    // Start is called before the first frame update
    void Start()
    {
        gameSettings = FindObjectOfType<GameSettings>();
        gameSettings.mmsui = this;

        if(gameSettings.resultsPath.Length > 0)
        {
            enableDDAToggle.isOn = gameSettings.enableDDA;
            physicsDDAToggle.isOn = gameSettings.physicsBasedDDA;
            resultsPathField.text = gameSettings.resultsPath;
        }
    }

    public void EnableDDA()
    {
        gameSettings.enableDDA = enableDDAToggle.isOn;
    }

    public void PhysicsDDA()
    {
        gameSettings.physicsBasedDDA = physicsDDAToggle.isOn;
    }

    public void SetResultsPath()
    {
        gameSettings.resultsPath = resultsPathField.text;
    }
}
