using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private bool useRoundTime;
    [SerializeField] private bool useDamageTaken;
    [SerializeField] private bool useShotsFired;
    [Space(10)]

    //Max player struggle comparison values - If player variables equals or greater to these, player struggle is at max - lowest difficulty next round.
    [Header("Max player struggle comparison values")]
    [SerializeField] private int maxRoundTime;
    [SerializeField] private int maxDamageTaken;
    [SerializeField] private int maxShotsFired;

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

    //I used this garbage system for a project back in 2022 knowing damn well it was awful. I can't believe I'm using it again for a thesis project.
    public void GenerateDifficultyModifier() //Generate difficulty modifier
    {
        List<float> ratioVariables = new List<float>(); 

        if(useRoundTime)
        {
            float timeRatio = (float) roundTime / maxRoundTime;
            ratioVariables.Add(Mathf.Clamp01(timeRatio)); //clamp value to 1 incase it exceeds max
        }

        if(useDamageTaken)
        {
            float damageRatio = (float)damageTaken/ maxDamageTaken;
            ratioVariables.Add(Mathf.Clamp01(damageRatio)); //clamp value to 1 incase it exceeds max
        }

        if (useShotsFired)
        {
            float shotsRatio = (float)shotsFired/ maxShotsFired;
            ratioVariables.Add(Mathf.Clamp01(shotsRatio)); //clamp value to 1 incase it exceeds max
        }

        float cumulRatios = 0;

        foreach (float ratio in ratioVariables)
        {
            cumulRatios += ratio;
        }

        difficultyModifier = 1 - cumulRatios/ratioVariables.Count; 

        Debug.Log("New difficulty modifier is: " + difficultyModifier);
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

    public void EndRoundTrack()
    {
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
}
