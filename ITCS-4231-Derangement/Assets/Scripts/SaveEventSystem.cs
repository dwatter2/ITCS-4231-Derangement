using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveEventSystem : MonoBehaviour
{
    public static SaveEventSystem instance;
    public bool musicOn = true;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMusic()
    {
        musicOn = !musicOn;
    }
}
