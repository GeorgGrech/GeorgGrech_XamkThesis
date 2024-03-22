using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class DataLogger : MonoBehaviour
{
    public string gameGUID;

    StringBuilder gameSummary;

    DDAManager ddaManager;

    // Start is called before the first frame update
    void Start()
    {
        ddaManager = DDAManager._instance;

        gameGUID = Guid.NewGuid().ToString();
        gameSummary = new StringBuilder("Wave No.,Difficulty Modifier,Enemies Killed");

        if (ddaManager.useRoundTime)
            gameSummary.Append(",Time Taken");

        if(ddaManager.useDamageTaken)
            gameSummary.Append(",Damage Taken");

        if(ddaManager.useShotsFired)
            gameSummary.Append(",Shots Fired");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LogRoundEnd(int roundNum,float difficultyModifier,int enemiesKilled,
        int timeTaken, int dmgTaken, int shotsFired)
    {
        gameSummary.Append("\n")
            .Append(roundNum + ",");


        if (ddaManager.firstRound)
            gameSummary.Append("Default,");
        else
            gameSummary.Append(difficultyModifier + ",");

        gameSummary.Append(enemiesKilled + ",");

        if (ddaManager.useRoundTime)
            gameSummary.Append(timeTaken+",");

        if(ddaManager.useDamageTaken)
            gameSummary.Append(dmgTaken+",");


        if(ddaManager.useShotsFired)
            gameSummary.Append(shotsFired+",");
    }

    public void WriteLog()
    {
        gameSummary.Append("\n\n");
        gameSummary.Append("DDA Mode,");

        if (ddaManager.physicsBasedDDA)
            gameSummary.Append("Physics");
        else
            gameSummary.Append("Traditional");


        Debug.Log("Saving logs with id: " + gameGUID);

        string path;

#if UNITY_EDITOR
        path = Application.streamingAssetsPath + "/" + "Data_" + gameGUID; //Rework for build
#else
        path = GameSettings._instance.resultsPath+"/"+"Data_"+gameGUID;
#endif

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        using (var writer = new StreamWriter(path + "/gameSummary.csv", false)) //Save game summary
        {
            writer.Write(gameSummary);
        }

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}
