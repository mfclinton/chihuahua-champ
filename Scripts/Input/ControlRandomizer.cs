using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlRandomizer
{
    // Internal Variables
    private InputAction inputAction;
    private Key[] randomizableKeys;
    public Key RandomKey { get; private set; }
    
    public ControlRandomizer(InputAction inputAction, Key[] randomizableKeys)
    {
        this.inputAction = inputAction;
        this.randomizableKeys = randomizableKeys;
    }
    
    public void BindRandomKey()
    {
        inputAction.RemoveAllBindingOverrides();

        RandomKey = randomizableKeys[Random.Range(0, randomizableKeys.Length)];
        string bindingPath = "<Keyboard>/" + RandomKey.ToString().ToLower();
        inputAction.ApplyBindingOverride(0, bindingPath);
        
        Debug.Log("Randomized Key: " + RandomKey);
    }
}