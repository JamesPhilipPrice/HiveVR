using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ToolManager : MonoBehaviour
{
    [Header("Inputs")]
    public InputActionProperty leftHandToolEnable;
    public InputActionProperty rightHandToolEnable;
    public InputActionProperty leftPointerEnable;

    [Header("Objects")]
    public GameObject leftHandTool;
    public GameObject rightHandTool;
    public GameObject leftPointer;

    [Header("Hands")]
    public GameObject leftHand;

    public void Start()
    {
        leftHandTool.SetActive(false);
        rightHandTool.SetActive(false);
        leftPointer.SetActive(false);

        leftHandToolEnable.action.performed += _ctx => EnableLeftTool();
        rightHandToolEnable.action.performed += _ctx => EnableRightTool();
        leftPointerEnable.action.performed += _ctx => EnableLeftPointer();
    }

    public void EnableLeftTool()
    {
        if (leftPointer.activeSelf)
        {
            leftPointer.SetActive(false);
            leftHand.SetActive(true);
            leftHandTool.SetActive(true);
        }
        else
        {
            leftHandTool.SetActive(!leftHandTool.activeSelf);
        }
    }

    public void EnableRightTool()
    {
        rightHandTool.SetActive(!rightHandTool.activeSelf);
    }

    public void EnableLeftPointer()
    {
        //Check if the left pointer is enabled
        if (!leftPointer.activeSelf)
        {
            leftHand.SetActive(false);
            leftHandTool.SetActive(false);
            leftPointer.SetActive(true);
        }
        else
        {
            leftPointer.SetActive(false);
            leftHand.SetActive(true);
        }
        
    }
}
