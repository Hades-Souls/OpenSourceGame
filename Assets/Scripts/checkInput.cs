using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class checkInput : MonoBehaviour
{
    public static PlayerInput playerInput;
    public static Vector2 MoveInput;
    public static bool wasinteractPress;
    private InputAction _interactactions;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        _interactactions = playerInput.actions["Interact"];
    }
    private void Update()
    {
        wasinteractPress = _interactactions.WasPerformedThisFrame();

 
    }
}
