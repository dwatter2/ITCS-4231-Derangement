using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public new GameObject camera;
    public Light flashlight;
    public Vector3 direction;
    public Sprite flashlightOff;
    public Sprite flashlightOn;
    public bool flashlightToggle = false;
    GameObject dialogue;
    public bool isTalking = false;
    public bool hasTalked = false;
    public GameObject buttonBox;
    public GameObject buttonDesign;

    [SerializeField] private Image compassPoint;
    [SerializeField] private Image heldItem;
    [SerializeField] private TextMeshProUGUI popup;
    [SerializeField] private Canvas dialogueBox;
    [SerializeField] private TextMeshProUGUI npcText;
    [SerializeField] private TextMeshProUGUI npcName;

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

        //Set + Disable item on load
        heldItem.sprite = flashlightOff;
        heldItem.enabled = false;

        dialogueBox.enabled = false;
    }

    void Update()
    {
        RotateCompass();
        EquipItem();
        UseItem();
    }

    void RotateCompass()
    {
        direction = camera.transform.eulerAngles;
        compassPoint.transform.eulerAngles = new Vector3(0f, 0f, -direction.y);
    }

    void EquipItem()
    {
        if(Input.GetKeyDown("q") && !isTalking)
        {
            if(!heldItem.enabled)
            {
                heldItem.enabled = true;
            }
            else
            {
                heldItem.enabled = false;
            }
        }
    }

    void UseItem()
    {
        //Left Click with item
        if(Input.GetMouseButtonDown(0) && heldItem.enabled && !isTalking)
        {
            if(!flashlightToggle)
            {
                heldItem.sprite = flashlightOn;
                Debug.Log("Turn Flashlight On");
                flashlightToggle = true;
                flashlight.enabled = true;
            }
            else
            {
                heldItem.sprite = flashlightOff;
                Debug.Log("Turn Flashlight Off");
                flashlightToggle = false;
                flashlight.enabled = false;
            }
        }
        else if(!heldItem.enabled)
        {
            heldItem.sprite = flashlightOff;
            flashlightToggle = false;
            flashlight.enabled = false;
        }
    }

    public void InteractPopup(bool isInteracting)
    {
        if(isInteracting && !isTalking)
        {
            popup.enabled = true;
        }
        else
        {
            popup.enabled = false;
        }
    }

    public void DialogueEnable(string name)
    {
        dialogueBox.enabled = true;
        Cursor.visible = true;
        GameObject talkingTo = GameObject.Find(name);
        npcName.GetComponent<TextMeshProUGUI>().text = talkingTo.GetComponent<NPC>().dialogue.npcName;;

        if(!isTalking) {
            npcText.GetComponent<TextMeshProUGUI>().text = "What can I do for you?";
        }

        //1 Display Text
        //2 On E press move to choices
        //3 Enable ButtonSpace and display choices
        //4 Accept and act on selected choice (Leave if [LEAVE] chosen)
        //5 Disable ButtonSpace and display text response if one exists
        //6 Cycle back to 3 until END

        if(!hasTalked && isTalking && Input.GetKeyDown("e"))
        {
            npcText.enabled = false;
            hasTalked = true;
            buttonBox.SetActive(true);
            DisplayButtons(name);
        }
        else if(isTalking && Input.GetKeyDown("e") && hasTalked)
        {
            npcText.enabled = false;
            buttonBox.SetActive(true);
        }

        isTalking = true;

        //On END
        //isTalking = false
        //FindObjectOfType<CameraMovement>().enabled = true;
        //GameObject.Find("Player").GetComponent<Movement>().enabled = true;
    }

    public void DisplayButtons(string name)
    {
        GameObject talkingTo = GameObject.Find(name);
        string[] v = talkingTo.GetComponent<NPC>().dialogue.playerResponses;
        string[] w = talkingTo.GetComponent<NPC>().dialogue.npcText;

        for(int i = 0; i < v.Length + 1; i++)
        {
            if(i < v.Length )
            {
            GameObject button = Instantiate(buttonDesign, buttonBox.transform);
            int responseIndex = i;
            button.GetComponentInChildren<TextMeshProUGUI>().text = v[i];
            button.GetComponent<Button>().onClick.AddListener(() => {
                buttonBox.SetActive(false);
                npcText.enabled = true;
                npcText.GetComponent<TextMeshProUGUI>().text = w[responseIndex];
                hasTalked = true;
            });
            }
            else
                {
                //END
                GameObject button = Instantiate(buttonDesign, buttonBox.transform);
                button.GetComponentInChildren<TextMeshProUGUI>().text = "[LEAVE]";
                button.GetComponent<Button>().onClick.AddListener(() => {
                    isTalking = false;
                    hasTalked = false;
                    dialogueBox.enabled = false;
                    npcText.enabled = true;
                    FindObjectOfType<CameraMovement>().enabled = true;
                    GameObject.Find("Player").GetComponent<Movement>().enabled = true;
                    DestroyButtons();
                });
            }
        }
    }

    public void DestroyButtons()
    {
        int num = buttonBox.transform.childCount;

        for(int i = 0; i < num; i++)
        {
            Destroy(buttonBox.transform.GetChild(i).gameObject);
        }
    }
}
