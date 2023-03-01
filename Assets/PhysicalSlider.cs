using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalSlider : MonoBehaviour {
    public Transform LeftPosition, RightPosition;
    public Transform Follower;

    public KMSelectable LeftButton, RightButton;

    [Range(0.01f, 1f)]
    public float Step;

    public float Value;

    public delegate void UpdateDelegate();
    public event UpdateDelegate OnUpdate = () => { };

    private Coroutine currentMovement;

	void Start () {
        LeftButton.OnInteract += Left;
        LeftButton.OnInteractEnded += Off;
        RightButton.OnInteract += Right;
        RightButton.OnInteractEnded += Off;
	}

    private bool Left()
    {
        currentMovement = StartCoroutine(MoveLeft());
        return false;
    }

    private bool Right()
    {
        currentMovement = StartCoroutine(MoveRight());
        return false;
    }

    private void Off()
    {
        StopCoroutine(currentMovement);
    }

    private IEnumerator MoveLeft()
    {
        while (true)
        {
            Value -= Step;
            Value = Mathf.Max(Value, 0f);
            UpdateFollower();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator MoveRight()
    { 
        while (true)
        {
            Value += Step;
            Value = Mathf.Min(Value, 1f);
            UpdateFollower();
            yield return new WaitForSeconds(0.1f);
        }
    }

    internal void UpdateFollower()
    {
        Follower.localPosition = Vector3.Lerp(LeftPosition.localPosition, RightPosition.localPosition, Value);
        OnUpdate();
    }
}
