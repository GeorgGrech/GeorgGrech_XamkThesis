using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDAManager : MonoBehaviour
{
    public static DDAManager _instance;
    public bool physicsBasedDDA; // If true, modify player physics mechanics, else modify enemies

    [Range(0f, 1f)]
    public float difficultyModifier; // 0-1 range. 0 for easiest setting, 1 for hardest.

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
        if (physicsBasedDDA == physicsBasedVar) //If this value actually needs to be modified with current DDA system
        {
            if(invertVal) //Does calculation need to be inverted, i.e lower value actually means higher difficulty
                return ((max - min) * (1-difficultyModifier)) + min;
            else
                return ((max - min) * difficultyModifier) + min;

        }
        else return defaultValue;
    }


}
