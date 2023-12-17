using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Threading;
using UnityEngine.Events;
using System.Linq;
using UnityEditor.PackageManager;

public class dialogue : MonoBehaviour
{
    public TextMeshProUGUI TextObject;
    public TextMeshProUGUI TextNameObject;
    public Image FaceObject;

    public GameObject Canvas;

    public GameObject SquarePopup;

    public Color32 specialCharacterColor;

    [System.Serializable]
    public class DialogueOption
    {
        public bool isActive = true;

        public dialogueObject dialogueObject;

        public UnityEvent OnDialogueEnd;
    }

    bool specialColor = false;
    bool bold = false;
    bool italic = false;

    bool canSkip;

    public List<DialogueOption> dialogueOptions;

    private List<DialogueOption> ActiveDialogueOptions;

    private int index;

    public int dialogueScreenIndex = 0;

    private bool didRevealText;

    private bool isDoingDialogue;


    private fade canvasFade;


    void Start()
    {
        Canvas.GetComponent<CanvasGroup>().alpha = 0f;

        canvasFade = Canvas.GetComponent<fade>();

    }

    void Update()
    {
        
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }




    public void StartDialogue()
    {
        ActiveDialogueOptions = dialogueOptions.Where(c => c.isActive).ToList();

        if(ActiveDialogueOptions.Count == 0)
        {
            return;
        }

        if (isDoingDialogue)
        {
            if (canSkip)
            {
                NextLine();
                return;
            }
            else
            {
                didRevealText = true;
                return;
            }
        }

        TextNameObject.text = string.Empty;
        TextObject.text = string.Empty;
        index = 0;

        StartCoroutine(EnableCanvas());




        if (dialogueScreenIndex == ActiveDialogueOptions.ToArray().Length)
        {
            dialogueScreenIndex = 0;
        }



        StartCoroutine(TypeLine());

    }

    IEnumerator TypeLine()
    {

        yield return null;

        canSkip = false;

        if (FaceObject != null)
        {
            FaceObject.sprite = ActiveDialogueOptions[dialogueScreenIndex].dialogueObject.dialogue[index].Face;
        }

        TextNameObject.text = ActiveDialogueOptions[dialogueScreenIndex].dialogueObject.dialogue[index].Name;

        specialColor = false;

        foreach (char c in ActiveDialogueOptions[dialogueScreenIndex].dialogueObject.dialogue[index].GetText().ToCharArray())
        {

            if(c.ToString() == "#")
            {
                specialColor = !specialColor;

                if (specialColor)
                {
                    TextObject.text += $"<color=#{ColorUtility.ToHtmlStringRGB(specialCharacterColor)}>";
                }
                else
                {
                    TextObject.text += $"<color=#{ColorUtility.ToHtmlStringRGB(Color.white)}>";
                }
            }
            else if (c.ToString() == "$")
            {
                bold = !bold;

                if (bold)
                {
                    TextObject.text += "<b>";
                }
                else
                {
                    TextObject.text += "</b>";
                }
            }
            else if (c.ToString() == "_")
            {
                italic = !italic;

                if (italic)
                {
                    TextObject.text += "<i>";
                }
                else
                {
                    TextObject.text += "</i>";
                }
            }
            else
            {
                TextObject.text += c;
            }





            if (!didRevealText)
            {
                yield return new WaitForSeconds(ActiveDialogueOptions[dialogueScreenIndex].dialogueObject.dialogue[index].Delay);
            }


        }

        canSkip = true;
        didRevealText = false;
    }

    public void NextLine()
    {

        if (index < ActiveDialogueOptions[dialogueScreenIndex].dialogueObject.dialogue.ToArray().Length - 1)
        {
            index++;

            TextObject.text = string.Empty;



            StartCoroutine(TypeLine());


        }
        else
        {
            dialogueEnd();
        }
    }




    void dialogueEnd()
    {
        canvasFade.FadeOut();
        isDoingDialogue = false;

        if (ActiveDialogueOptions[dialogueScreenIndex].OnDialogueEnd != null)
        {
            ActiveDialogueOptions[dialogueScreenIndex].OnDialogueEnd.Invoke();
        }

        dialogueScreenIndex += 1;
    }


    IEnumerator EnableCanvas() 
    {     
        yield return new WaitForSeconds(0.1f);

        canvasFade.FadeIn();
        isDoingDialogue = true;
    }



    public void DisableDialogueObject(dialogueObject DialogueObject)
    {
        DialogueOption[] SearchOptions = dialogueOptions.Where(c => c.dialogueObject == DialogueObject).ToArray();

        foreach (var dialogue in dialogueOptions)
        {
            dialogue.isActive = false;
        }
    }

    public void EnableDialogueObject(dialogueObject DialogueObject)
    {
        DialogueOption[] SearchOptions = dialogueOptions.Where(c => c.dialogueObject == DialogueObject).ToArray();

        foreach (var dialogue in dialogueOptions)
        {
            dialogue.isActive = true;
        }
    }




}