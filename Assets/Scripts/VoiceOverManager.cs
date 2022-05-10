using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VoiceOverManager : MonoBehaviour
{
    [Header("Audio stuff")]
    public AudioClip[] audioClipsInOrder;
    public AudioSource audioSource;

    [Header("Video stuff")]
    public VideoClip[] videoClips;
    public VideoPlayer videoPlayer;

    [Header("Text stuff")]
    public string[] audioTextAcompaniment;

    public string stepCounterPreText;

    [Header("UI Stuff")]
    public Text stepCounter;
    public Text textDescription;

    public Button playAudioButton;
    public Button previousStepButton;
    public Button nextStepButton;

    //Private variables for tracking progress
    private int maxStepIndex;
    private int currentStepIndex;

    private void Start()
    {
        //Warning checks
        if (audioClipsInOrder.Length != audioTextAcompaniment.Length)
        {
            //The audio and text acompaniment does not match up this will break the system
            Debug.LogError("The number of audio clips and text acompaniments does not match and will cause an index out of bounds array error");
        }

        //Setup initial values
        currentStepIndex = 0;
        maxStepIndex = audioClipsInOrder.Length - 1;

        //Setup the Canvas and audio starting state
        stepCounter.text = stepCounterPreText + $"{currentStepIndex + 1} / {maxStepIndex + 1}";
        audioSource.clip = audioClipsInOrder[currentStepIndex];
        textDescription.text = audioTextAcompaniment[currentStepIndex];

        previousStepButton.interactable = false;
        PlayAudio();
    }

    public void PlayAudio()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void NextStep()
    {
        audioSource.Stop();
        videoPlayer.Stop();
        //Check if the next step is the last

        if (currentStepIndex < maxStepIndex)
        {
            //Another step to go
            currentStepIndex += 1;
            previousStepButton.interactable = true;
            if (currentStepIndex == maxStepIndex)
            {
                //Now on the last step
                nextStepButton.interactable = false;
            }
            //Change the audio and text prompt
            audioSource.clip = audioClipsInOrder[currentStepIndex];
            textDescription.text = audioTextAcompaniment[currentStepIndex];
            PlayAudio();

            if(videoClips[currentStepIndex] != null)
            {
                videoPlayer.clip = videoClips[currentStepIndex];
                videoPlayer.Play();
            }
            else
            {
                videoPlayer.clip = null;
            }
        }
    }

    public void PreviousStep()
    {
        audioSource.Stop();
        videoPlayer.Stop();

        //Check if we can go back
        if (currentStepIndex > 0)
        {
            currentStepIndex -= 1;
            nextStepButton.interactable = true;
            if (currentStepIndex == 0)
            {
                previousStepButton.interactable = false;
            }
            audioSource.clip = audioClipsInOrder[currentStepIndex];
            textDescription.text = audioTextAcompaniment[currentStepIndex];
            PlayAudio();

            if (videoClips[currentStepIndex] != null)
            {
                videoPlayer.clip = videoClips[currentStepIndex];
                videoPlayer.Play();
            }
            else
            {
                videoPlayer.clip = null;
            }
        }
    }

}
