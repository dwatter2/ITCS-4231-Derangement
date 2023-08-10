using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public GameObject LoadingScreen;
    public AudioSource audioSource;
    public AudioSource storeMusic;
    public AudioClip jingle;
    private bool hasMusic;
    //public AudioClip idleMusic;
    //public AudioClip outroMusic;

    private void Start()
    {
        hasMusic = GameObject.Find("EventSystem").GetComponent<SaveEventSystem>().musicOn;
    }
    private void LoadNewLevel(string name)
    {
        StartCoroutine(LoadSceneAsync(name));
    }

    IEnumerator LoadSceneAsync(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);

        while(!operation.isDone)
        {
            LoadingScreen.SetActive(true);
            yield return null;
        }

        if(operation.isDone)
        {
            LoadingScreen.SetActive(false);
        }
    }

    public void TransitionLevel(string name)
    {
        switch(name)
        {
            //Inside Cabin
            case "doorToCabin":
            {
                LoadNewLevel("Cabin_Inside");
                this.transform.position = new Vector3(3, 1, 2);
            }
            break;
            //Leaving Cabin
            case "doorFromCabin":
            {
                if(this.GetComponent<PlayerManager>().talkedToWitness)
                {
                    LoadNewLevel("Cabin_Outside");
                }
                else
                {
                    LoadNewLevel("Cabin_Outside_Day");
                }
                this.transform.position = new Vector3(-5f, 1f, -1);
            }
            break;
            //Leaving Cabin Area
            case "doorToField":
            {
                if(this.GetComponent<PlayerManager>().talkedToWitness)
                {
                    LoadNewLevel("Field_Night");
                }
                else
                {
                    LoadNewLevel("Field");
                }
                this.transform.position = new Vector3(90.5f, 1f, 42.5f);
            }
            break;
            //Inside Tutorial Store
            case "doorToStore":
            {
                //SceneManager.LoadScene("Tutorial_Inside", LoadSceneMode.Single);
                LoadNewLevel("Tutorial_Inside");
                this.transform.position = new Vector3(0, 1f, -4);
                if(hasMusic)
                    storeMusic.Play();
            }
            break;
            //Leave Tutorial Store
            case "doorToTutorial":
            {
                //SceneManager.LoadScene("Tutorial_Outside", LoadSceneMode.Single);
                LoadNewLevel("Tutorial_Outside");
                this.transform.position = new Vector3(0, 1, 10);
                storeMusic.Pause();
            }
            break;
            //Head to Town
            case "Town":
            {
                //SceneManager.LoadScene("Tutorial_Outside", LoadSceneMode.Single);
                LoadNewLevel("Town");
                this.transform.position = new Vector3(6, 1, 2.5f);
                if(hasMusic)
                    audioSource.Play();
            }
            break;
            //From field to cabin area
            case "doorToCabinArea":
            {
                if(this.GetComponent<PlayerManager>().talkedToWitness)
                {
                    LoadNewLevel("Cabin_Outside");
                }
                else
                {
                    LoadNewLevel("Cabin_Outside_Day");
                }
                this.transform.position = new Vector3(35, 1, 2);
            }
            break;
            //From cabin area to field
            case "doorFromCabinArea":
            {
                if(this.GetComponent<PlayerManager>().talkedToWitness)
                {
                    LoadNewLevel("Field_Night");
                }
                else
                {
                    LoadNewLevel("Field");
                }
                this.transform.position = new Vector3(8, 1, 13.5f);
            }
            break;
            //From Field to Town
            case "doorToTown":
            {
                if(this.GetComponent<PlayerManager>().talkedToWitness)
                {
                    LoadNewLevel("Town_Night");
                }
                else
                {
                    LoadNewLevel("Town");
                }
                this.transform.position = new Vector3(-27, 1f, 18);
            }
            break;

            default:
            break;
        }
    }
}
