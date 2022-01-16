using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //public Button startButton;
    // Start is called before the first frame update
    public GameObject imageToFade;
    void Start()
    {
        //startButton = this.GetComponent<Button>();

        //startButton.onClick.AddListener(StartGameButton);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGameButton()
    {
        Debug.Log("Start game");
        StartCoroutine(Fade(imageToFade));

        
    }


    IEnumerator Fade(GameObject objToFade)
    {

        var img = objToFade.GetComponent<Image>();

        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }

        SceneManager.LoadScene("SampleScene");

    }
}
