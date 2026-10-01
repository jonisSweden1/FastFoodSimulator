using System;
using UnityEngine;
using UnityEngine.Events;

// This script is in charge of handling the timer and invoking an event when the timer is complete.
// This is to test scripts in the game, and will be removed later on. This script is not to be used in the final game.
public class Timer : MonoBehaviour
{
    bool hasInvoked = false;

    float timer = 0.0f;

    [SerializeField]
    private float _timeLimit = 10.0f;

    [SerializeField]
    private UnityEvent onTimerComplete;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= _timeLimit)
        {
            if (!hasInvoked)
            {
                onTimerComplete?.Invoke();
                hasInvoked = true;
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
