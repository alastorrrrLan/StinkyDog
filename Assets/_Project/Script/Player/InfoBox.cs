using UnityEngine;
using UnityEngine.InputSystem;

public class InfoBox : MonoBehaviour
{
    public bool dogInside, keyPressed;
    public GameObject infoBox, instructionBox;

    public void Update()
    {
        if (Keyboard.current.shiftKey.isPressed)
            keyPressed = true;
        else
            keyPressed = false;

        if (dogInside && keyPressed)
        {
            infoBox.SetActive(true);
        }
        else
        {
            infoBox.SetActive(false);
        }
        if (dogInside && !keyPressed)
        {
            instructionBox.SetActive(true);
        }
        else
        {
            instructionBox.SetActive(false);
        }
    }
}
