using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDAManager : MonoBehaviour
{
    public static DDAManager _instance;
    public bool enableDDA; // If false, always use default values
    public bool physicsBasedDDA; // If true, modify player physics mechanics, else modify enemies

    [Range(0f, 1f)]
    public float difficultyModifier; // 0-1 range. 0 for easiest setting, 1 for hardest.

    //Round variables
    private int roundTime;
    private int damageTaken;
    private int shotsFired;
    private Coroutine timeTracker;

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

    public void GenerateDifficultyModifier() //Generate difficulty modifier
    {

    }

    public float GetDynamicValue(bool physicsBasedVar, bool invertVal, float min, float max, float defaultValue)
    {
        if (!enableDDA) return defaultValue;

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
