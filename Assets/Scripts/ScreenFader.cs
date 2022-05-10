using System.Collections;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    //If the screen will fade on level start
    public bool fadeOnStart = true;

    //How long it takes to complete a fade
    public float fadeDuration = 2f;

    //The color of the faded screen
    public Color fadeColour;

    //The renderer that is attached to the fader quad
    private Renderer renderer;

    private void Start()
    {
        //Setup references and start fade, if needed
        renderer = GetComponent<Renderer>();

        if (fadeOnStart)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        Fade(1, 0);
    }

    public void FadeOut()
    {
        Fade(0, 1);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        //Start a coroutine to do the fade
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        //Set a timer for the fade duration and lerping
        float timer = 0;
        while (timer <= fadeDuration)
        {
            //Get the colour of the fader then set its value to the Lerp of the function parameters, using timer/fadeDuration as t
            Color newCol = fadeColour;
            newCol.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

            //Set the colour to the parameter of the material
            renderer.material.SetColor("_Col", newCol);

            timer += Time.deltaTime;
            yield return null;
        }

        //Make sure the fader has completed before end of routine
        Color newCol2 = fadeColour;
        newCol2.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

        renderer.material.SetColor("_col", newCol2);
    }
}
