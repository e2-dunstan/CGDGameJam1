using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterName : MonoBehaviour
{
    private Text[] chars;
    private int index = 0;

    private float flashTimeElapsed = 0;
    private bool axisReleased = true;

    public enum InputType {  KEYBOARD, CONTROLLER }
    public InputType inputType = InputType.KEYBOARD;

    private char[] inputChars = new char[36];
    private int[] currentChar = new int[3]; 


    private void Start()
    {
        chars = GetComponentsInChildren<Text>(true);
        if (chars[0] == this.GetComponent<Text>())
        {
            chars[0] = chars[1];
            chars[1] = chars[2];
            chars[2] = chars[3];
            chars[3] = null;
        }

        currentChar[0] = 0;
        currentChar[1] = 0;
        currentChar[2] = 0;

        #region input chars
        inputChars[0] = 'A';
        inputChars[1] = 'B';
        inputChars[2] = 'C';
        inputChars[3] = 'D';
        inputChars[4] = 'E';
        inputChars[5] = 'F';
        inputChars[6] = 'G';
        inputChars[7] = 'H';
        inputChars[8] = 'I';
        inputChars[9] = 'J';
        inputChars[10] = 'K';
        inputChars[11] = 'L';
        inputChars[12] = 'M';
        inputChars[13] = 'N';
        inputChars[14] = 'O';
        inputChars[15] = 'P';
        inputChars[16] = 'Q';
        inputChars[17] = 'R';
        inputChars[18] = 'S';
        inputChars[19] = 'T';
        inputChars[20] = 'U';
        inputChars[21] = 'V';
        inputChars[22] = 'W';
        inputChars[23] = 'X';
        inputChars[24] = 'Y';
        inputChars[25] = 'Z';

        inputChars[26] = '0';
        inputChars[27] = '1';
        inputChars[28] = '2';
        inputChars[29] = '3';
        inputChars[30] = '4';
        inputChars[31] = '5';
        inputChars[32] = '6';
        inputChars[33] = '7';
        inputChars[34] = '8';
        inputChars[35] = '9';
        #endregion
        
        chars[0].text = inputChars[currentChar[0]].ToString();
        chars[1].text = inputChars[currentChar[1]].ToString();
        chars[2].text = inputChars[currentChar[2]].ToString();
    }

    public bool GetName()
    {
        //flash
        flashTimeElapsed += Time.deltaTime;
        if (flashTimeElapsed > 0.2f)
        {
            chars[index].enabled = false;
            if (flashTimeElapsed > 0.4f) flashTimeElapsed = 0;
        }
        else
        {
            chars[index].enabled = true;
        }
        foreach (Text charElement in chars)
        {
            if (charElement != chars[index] && charElement != null) charElement.enabled = true;
        }
        
        if (inputType == InputType.KEYBOARD) return KeyboardInput();
        else if (inputType == InputType.CONTROLLER) return ControllerInput();
        else return false;
    }


    private bool KeyboardInput()
    {
        string str = Input.inputString;
        if (!string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str))
        {
            chars[index].enabled = true;
            chars[index].text = str[0].ToString().ToUpper();
            index++;
            if (index > 2) return true;
        }
        return false;
    }

    private bool ControllerInput()
    {
        float horizontal = InputManager.Instance().GetHorizontalInput();
        float vertical = InputManager.Instance().GetVerticalInput();

        if (horizontal == 0 && vertical == 0)
        {
            axisReleased = true;
            return false;
        }
        if (!axisReleased) return false;

        if (horizontal > 0)
        {
            index++;
            if (index > 2) return true;
        }
        else if (horizontal < 0)
        {
            index--;
            if (index < 0) index = 0;
        }

        if (vertical > 0)
        {
            currentChar[index]++;
            if (currentChar[index] > 35) currentChar[index] = 0;
        }
        else if (vertical < 0)
        {
            currentChar[index]--;
            if (currentChar[index] < 0) currentChar[index] = 35;
        }
        
        chars[index].text = inputChars[currentChar[index]].ToString();
        axisReleased = false;

        return false;
    }


    public string NameEntered
    {
        get
        {
            return chars[0].text + chars[1].text + chars[2].text;
        }
    }

    public void ForceNameActive()
    {
        chars[0].enabled = true;
        chars[1].enabled = true;
        chars[2].enabled = true;
    }
}
