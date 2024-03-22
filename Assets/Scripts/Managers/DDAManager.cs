using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DDAManager : MonoBehaviour
{
    public static DDAManager _instance;
    public bool enableDDA; // If false, always use default values
    public bool firstRound; // If true, use default values
    public bool physicsBasedDDA; // If true, modify player physics mechanics, else modify enemies

    [Range(0f, 1f)]
    public float difficultyModifier; // 0-1 range. 0 for easiest setting, 1 for hardest.

    //Round variables
    private int roundTime;
    private int damageTaken;
    private int shotsFired;
    private Coroutine timeTracker;
    [Space(10)]

    [Header("Variables to use in calculations")]
    public bool useRoundTime;
    public bool useDamageTaken;
    public bool useShotsFired;
    [Space(10)]

    //Min player struggle comparison values - If player variables equals or less than these, player struggle is at min - highest difficulty next round.
    [Header("Min player struggle comparison values")]
    [SerializeField] private int minRoundTime;
    [SerializeField] private int minDamageTaken;
    [SerializeField] private int minShotsFired;

    //Max player struggle comparison values - If player variables equals or greater than these, player struggle is at max - lowest difficulty next round.
    [Header("Max player struggle comparison values")]
    [SerializeField] private int maxRoundTime;
    [SerializeField] private int maxDamageTaken;
    [SerializeField] private int maxShotsFired;

    DataLogger dataLogger;
    GameSettings gameSettings;

    private void Awake()
    {
        _instance = this;
        dataLogger = FindObjectOfType<DataLogger>();

        gameSettings = GameSettings._instance;
        enableDDA = gameSettings.enableDDA;
        physicsBasedDDA = gameSettings.physicsBasedDDA;

        UpdateModeSelectText();
        
    }

    //I used this garbage system for a project back in 2022 knowing damn well it was awful. I can't believe I'm using it again for a thesis project.
    public void GenerateDifficultyModifier() //Generate difficulty modifier
    {
        List<float> ratioVariables = new List<float>(); 

        if(useRoundTime)
        {
            float timeRatio = GetRatioVariable((float)roundTime, (float)minRoundTime, (float)maxRoundTime); //Generate value between 0 and 1
            ratioVariables.Add(timeRatio);
            Debug.Log("Time taken: " + roundTime + ". RatioVariable is: " + timeRatio);
        }

        if(useDamageTaken)
        {
            float damageRatio = GetRatioVariable((float)damageTaken, (float)minDamageTaken, (float)maxDamageTaken);
            ratioVariables.Add(damageRatio);
            Debug.Log("Damage taken: " + damageTaken+ ". RatioVariable is: " + damageRatio);
        }

        if (useShotsFired)
        {
            float shotsRatio = GetRatioVariable((float)shotsFired, (float)minShotsFired, (float)maxShotsFired);
            ratioVariables.Add(shotsRatio);
            Debug.Log("Shots fired: " + shotsFired + ". RatioVariable is: " + shotsRatio);
        }

        float cumulRatios = 0;

        foreach (float ratio in ratioVariables)
        {
            cumulRatios += ratio;
        }

        difficultyModifier = 1 - cumulRatios/ratioVariables.Count; 

        Debug.Log("New difficulty modifier is: " + difficultyModifier);
    }

    private float GetRatioVariable(float value, float min, float max)
    {
        return Mathf.Clamp01((value - min) / (max - min)); //Clamp between 0 and 1
    }

    public float GetDynamicValue(bool physicsBasedVar, bool invertVal, float min, float max, float defaultValue)
    {
        if (!enableDDA || firstRound) return defaultValue;

        if (physicsBasedDDA == physicsBasedVar) //If this value actually needs to be modified with current DDA system
        {
            if(invertVal) //Does calculation need to be inverted, i.e lower value actually means higher difficulty
                return ((max - min) * (1-difficultyModifier)) + min;
            else
                return ((max - min) * difficultyModifier) + min;

        }
        else return defaultValue;
    }

    //Track statistics during round

    public void StartRoundTrack()
    {
        roundTime = 0;
        damageTaken = 0;
        shotsFired = 0;
        timeTracker = StartCoroutine(RoundTimeTracker());
    }

    public void EndRoundTrack(int waveNo, int enemiesKilled)
    {
        dataLogger.LogRoundEnd(waveNo, difficultyModifier, enemiesKilled, roundTime, damageTaken, shotsFired);
        firstRound = false; //No longer first round, stop using default values

        Debug.Log("Time taken: " + roundTime+ " - "+
            "damage taken: " + damageTaken+ " - " +
            "shots fired: " + shotsFired);

        StopCoroutine(timeTracker);

        GenerateDifficultyModifier();
    }

    private IEnumerator RoundTimeTracker()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            roundTime++; //Count seconds
        }
    }

    public void TrackDamage(int damage)
    {
        damageTaken += damage;
        Debug.Log("Damage taken this round: " + damageTaken);
    }

    public void TrackShotFired()
    {
        shotsFired++;
        Debug.Log("Shots fired this round: " + shotsFired);
    }

    void UpdateModeSelectText()
    {
        TextMeshProUGUI mode = GameObject.Find("ModeSelectText").GetComponent<TextMeshProUGUI>();
        if (!gameSettings.enableDDA)
            mode.text = "/";
        else
        {
            if (gameSettings.physicsBasedDDA)
                mode.text = "A";
            else mode.text = "B";
        }
    }
}
