using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    // Start is called before the first frame update
    public string resultsPath;
    public bool enableDDA;
    public bool physicsBasedDDA;

    public static GameSettings _instance;

    public MainMenuSettingsUI mmsui;

    private void Awake() //implement singleton pattern to not permit multiple instances
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        //On Level Load, reset DDA
        //ResetDDA();
    }
}
