using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSettings : MonoBehaviour
{
    public static MultiplayerSettings mpSettings;

    public bool delayStart;
    public int maxPlayers;
    public int menuScene;
    public int mpScene;

    private void Awake()
    {
        if (MultiplayerSettings.mpSettings == null)
        {
            MultiplayerSettings.mpSettings = this;
        }
        else
        {
            if(MultiplayerSettings.mpSettings != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
