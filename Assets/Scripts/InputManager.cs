using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance = null;

    private void Awake()
    {
        if (_instance == null) _instance = this;
    }

    public static InputManager Instance()
    {
        if (_instance == null)
        {
            _instance = new InputManager();
        }
        return _instance;
    }

    public float GetHorizontalInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public float GetVerticalInput()
    {
        return Input.GetAxisRaw("Vertical");
    }

    public bool GetActionButton0Down()
    {
        return Input.GetButtonDown("ActionButton0");
    }

    public bool GetActionButton0Up()
    {
        return Input.GetButtonUp("ActionButton0");
    }

    public bool GetActionButton1Down()
    {
        return Input.GetButtonDown("ActionButton1");
    }

    public bool GetActionButton1Held()
    {
        return Input.GetButton("ActionButton1");
    }

    public bool GetActionButton1Up()
    {
        return Input.GetButtonUp("ActionButton1");
    }
}
