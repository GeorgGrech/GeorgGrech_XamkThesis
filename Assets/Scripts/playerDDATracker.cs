using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDDATracker : MonoBehaviour
{
    private DDAManager ddaManager;
    private void Start()
    {
        ddaManager = DDAManager._instance;
    }

    public void TrackDamage(int damage)
    {
        ddaManager.TrackDamage(damage);
    }

    public void TrackShotFired()
    {
        ddaManager.TrackShotFired();
    }
}
