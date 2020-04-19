using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    public GameObject swipeImage;

    [SerializeField]
    public GameObject TapImage;

    [SerializeField]
    public GameObject swipeDownImage;

    [SerializeField]
    public Text TutorialText;

    int iNbSwipe = 0;
    int iNbRotate = 0;
    int iNbDown = 0;
    float elapseTimeAtCurrentStep = 0;

    TutorialStep currentStep;
    enum TutorialStep { Swipe, Rotate, Bottom, Ended }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("HasDoneTuto", 0) == 1)
        {
            DismissTuto();
            TutorialText.gameObject.SetActive(false);
            enabled = false;
        }
        else
            InitTuto();
    }

    // Update is called once per frame
    void Update()
    {
        //Il faut swip 5 fois ou au moins 1 fois et attendre 4 secondes pour passer à l'étape suivante
        if (currentStep == TutorialStep.Swipe && (iNbSwipe >= 5 || (iNbSwipe >= 1 && Time.time - elapseTimeAtCurrentStep >= 6)))
        {
            elapseTimeAtCurrentStep = Time.time; //reset
            currentStep = TutorialStep.Rotate;
            DisplayRotateTuto();
        }
        //Il faut rotate 4 fois ou au moins 1 fois et attendre 4 secondes pour passer à l'étape suivante
        else if (currentStep == TutorialStep.Rotate && (iNbRotate >= 4 || (iNbRotate >= 1 && Time.time - elapseTimeAtCurrentStep >= 6) ))
        {
            currentStep = TutorialStep.Bottom;
            DisplaySwipeDownTuto();
        }
        //Il faut swipe vers le bas 1 fois pour passer à l'étape suivante
        else if (currentStep == TutorialStep.Bottom && iNbDown >= 1)
        {
            //Tutorial ended !
            PlayerPrefs.SetInt("HasDoneTuto", 1);

            //Dismiss images            
            DismissTuto();

            //Display congratulation text
            TutorialText.text = Translation.Get("tutoEnded");
            currentStep = TutorialStep.Ended;
            elapseTimeAtCurrentStep = Time.time;
        }
        else if (currentStep == TutorialStep.Ended && Time.time - elapseTimeAtCurrentStep >= 4)
        {
            //Dissmis congratulation text and stop tutorial script
            TutorialText.gameObject.SetActive(false);
            enabled = false;
        }
    }

    public void InitTuto()
    {
        TutorialText.text = Translation.Get("tutoTitle");

        FindObjectOfType<GestureManager>().HasBeenToLeft += (sender, evtargs) =>
        {
            iNbSwipe++;
        };

        FindObjectOfType<GestureManager>().HasBeenToRight += (sender, evtargs) =>
        {
            iNbSwipe++;
        };

        FindObjectOfType<GestureManager>().HasRotated += (sender, evtargs) =>
        {
            iNbRotate++;
        };

        FindObjectOfType<GestureManager>().HasBeenToBottom += (sender, evtargs) =>
        {
            iNbDown++;
        };

        elapseTimeAtCurrentStep = Time.time;
        currentStep = TutorialStep.Swipe;
        DisplaySwipeTuto();
    }

    void DisplaySwipeTuto()
    {
        swipeImage.SetActive(true);
        TapImage.SetActive(false);
        swipeDownImage.SetActive(false);
    }

    void DisplayRotateTuto()
    {
        swipeImage.SetActive(false);
        TapImage.SetActive(true);
        swipeDownImage.SetActive(false);
    }

    void DisplaySwipeDownTuto()
    {
        swipeImage.SetActive(false);
        TapImage.SetActive(false);
        swipeDownImage.SetActive(true);
    }

    void DismissTuto()
    {
        swipeImage.SetActive(false);
        TapImage.SetActive(false);
        swipeDownImage.SetActive(false);
    }
}
