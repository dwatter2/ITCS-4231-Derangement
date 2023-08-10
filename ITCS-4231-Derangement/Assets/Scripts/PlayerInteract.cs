using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    public float maxDistance = 1.5f;
    public bool isInteractable = false;
    public bool talking = false;
    public string interactObject = "";
    public string npcName = "";
    public string doorName = "";
    public string itemName = "";
    public GameObject player;
    public GameObject userCamera;
    public GameObject EndScreen;

    void FixedUpdate()
    {
        Scan();
        CanInteract();
    }

    void Update()
    {
        Select();
    }

    void Scan()
    {
        RaycastHit hit;
        
        //Draw Ray in Viewport
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * maxDistance, Color.red);

        //Raycast Hits
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistance))
        {
            if(hit.transform.tag == "Door")
            {
                isInteractable = true;
                interactObject = "Door";
                doorName = hit.transform.gameObject.name;
                //Debug.Log("HitDoor");
            }
            else if(hit.transform.tag == "NPC")
            {
                isInteractable = true;
                interactObject = "NPC";
                npcName = hit.transform.gameObject.name;
            }
            else if(hit.transform.tag == "Car")
            {
                isInteractable = true;
                interactObject = "Car";
                //Debug.Log("HitItem");
            }
            else if(hit.transform.tag == "Item")
            {
                isInteractable = true;
                interactObject = "Item";
                itemName = hit.transform.gameObject.name;
                //Debug.Log("HitItem");
            }
        }
        else
        {
            isInteractable = false;
            interactObject = "";
        }
    }

    void CanInteract()
    {
        if(isInteractable)
        {
            UIManager.instance.InteractPopup(true);
        }
        else
        {
            UIManager.instance.InteractPopup(false);
        }
    }

    void Select()
    {
        if(isInteractable)
        {
            if(Input.GetKeyDown("e"))
                {
                    if(interactObject == "Door")
                    {
                        LoadNextScene(doorName);
                    }
                    if(interactObject == "NPC")
                    {
                        StartDialogue(npcName);
                        
                        if(npcName == "CabinOccupant")
                        {
                            player.GetComponent<PlayerManager>().talkedToWitness = true;
                        }
                    }
                    if(interactObject == "Car")
                    {
                        if(player.GetComponent<PlayerManager>().talkedToWitness)
                        {
                            EndScreen.SetActive(true);
                        }
                        else if(player.GetComponent<PlayerManager>().tutorialDone)
                        {
                            LoadNextScene("Town");
                            player.GetComponent<PlayerManager>().inTown = true;
                        }
                    }
                    if(interactObject == "Item")
                    {
                        if(itemName == "Coffee")
                        {
                            GameObject.Find("Coffee").SetActive(false);
                            player.GetComponent<PlayerManager>().tutorialDone = true;
                        }
                    }
                }
        }
    }

    void LoadNextScene(string name)
    {
        //SceneManager.LoadScene("Cabin_Inside", LoadSceneMode.Single);
        //Debug.Log("Moving to Cabin");
        //player.transform.position = new Vector3(3, 0, 2);
        if(doorName != "" && name != "Town")
        {
        GameObject.Find("Player").GetComponent<LevelSelect>().TransitionLevel(doorName);
        }
        else if(name == "Town")
        {
        GameObject.Find("Player").GetComponent<LevelSelect>().TransitionLevel("Town");
        }

        //Bad DO NOT USE (i think)
        //SceneManager.SetActiveScene(insideCabin);
        //SceneManager.MoveGameObjectToScene(player, insideCabin);
        //SceneManager.MoveGameObjectToScene(userCamera, insideCabin);
    }

    void StartDialogue(string name)
    {
        Debug.Log("Start Talking");
        FindObjectOfType<CameraMovement>().enabled = false;
        GameObject.Find("Player").GetComponent<Movement>().enabled = false;
        GameObject.Find("UI").GetComponent<UIManager>().DialogueEnable(npcName);
    }
}
