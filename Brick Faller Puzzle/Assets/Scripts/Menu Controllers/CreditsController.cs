using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    public void DismissCredits()
    {
        FindObjectOfType<SoundsEffectsController>().PlaybuttonCliked();

        SceneManager.LoadScene("MainMenu");
    }
}
