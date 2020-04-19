using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureManager : MonoBehaviour
{
    private static GestureManager instance;
    private SwipeDirection currentSwipe;

    public event EventHandler HasBeenToLeft;
    public event EventHandler HasBeenToRight;
    public event EventHandler HasBeenToBottom;
    public event EventHandler HasRotated;

    public GestureManager()
    {
        instance = this;
        sqrDeadzone = deadzone * deadzone;
    }

    public static GestureManager GetInstance()
    {
        return GestureManager.instance;
    }

    public static bool ShouldGoLeft()
    {
        bool bRes = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            bRes = true;

        if (GetInstance().currentSwipe == SwipeDirection.Left)
        {
            GetInstance().currentSwipe = SwipeDirection.None;
            bRes = true;
        }

        //For tutorial controller
        if (bRes && GetInstance().HasBeenToLeft != null)
            GetInstance().HasBeenToLeft(GetInstance(), EventArgs.Empty);

        return bRes;
    }

    public static bool ShouldGoRight()
    {
        bool bRes = false;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            bRes = true;

        if (GetInstance().currentSwipe == SwipeDirection.Right)
        {
            GetInstance().currentSwipe = SwipeDirection.None;
            bRes = true;
        }

        //For tutorial controller
        if (bRes && GetInstance().HasBeenToRight != null)
            GetInstance().HasBeenToRight(GetInstance(), EventArgs.Empty);

        return bRes;
    }

    public static bool ShouldRotate()
    {
        bool bRes = false;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            bRes = true;

        /*if (GetInstance().currentSwipe == SwipeDirection.Up)
        {
            GetInstance().currentSwipe = SwipeDirection.None;
            return true;
        }*/

        if (GetInstance().taping)
        {
            GetInstance().taping = false;
            bRes = true;
        }

        //For tutorial controller
        if (bRes && GetInstance().HasRotated != null)
            GetInstance().HasRotated(GetInstance(), EventArgs.Empty);

        return bRes;
    }

    public static bool ShouldGoDown()
    {
        bool bRes = false;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            bRes = true;

        if (GetInstance().currentSwipe == SwipeDirection.Down)
        {
            GetInstance().currentSwipe = SwipeDirection.None;
            bRes = true;
        }

        //For tutorial controller
        if (bRes && GetInstance().HasBeenToBottom != null)
            GetInstance().HasBeenToBottom(GetInstance(), EventArgs.Empty);

        return bRes;
    }

    public enum SwipeDirection
    {
        None,
        Up,
        Down,
        Right,
        Left
    }

    //public static event Action<SwipeDirection> Swipe;
   // private bool swiping = false;
    private bool taping = false;
    //private bool eventSent = false;
    private Vector2 lastPosition;
    private Vector2 beganPosition;
    private Vector2 swipeDelta;
    private float deadzone = 80;
    private float sqrDeadzone;
    bool ignoreSwipe = false;

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch currentTouch = Input.GetTouch(0);

/*#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            beganPosition = lastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (beganPosition == (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition))
                taping = true;
            currentSwipe = SwipeDirection.None;
            beganPosition = lastPosition = Vector2.zero;
        }

        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#else*/
        if (currentTouch.phase == TouchPhase.Began)
        {
            beganPosition = lastPosition = currentTouch.position;
        }
        else if (currentTouch.phase == TouchPhase.Ended || currentTouch.phase == TouchPhase.Canceled)
        {
            if (beganPosition == lastPosition)
                taping = true;
            currentSwipe = SwipeDirection.None;
            beganPosition = lastPosition = Vector2.zero;
            ignoreSwipe = false;
            return;
        }

        if (ignoreSwipe)
            return;

        Vector2 touchPos = currentTouch.position;
//#endif        

        /*float deltaX = Mathf.Abs(touchPos.x - lastPosition.x);
        float deltaY = Mathf.Abs(touchPos.y - lastPosition.y);

        if (deltaX < 8 && deltaY < 8)
            return;*/
        swipeDelta = touchPos - lastPosition;

        if (swipeDelta.sqrMagnitude > sqrDeadzone)
        {
            Vector2 direction = currentTouch.position - lastPosition;


/*#if UNITY_EDITOR
            lastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#else*/
            lastPosition = currentTouch.position;
//#endif
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                    currentSwipe = SwipeDirection.Right;
                else
                    currentSwipe = SwipeDirection.Left;
            }
            else
            {
                if (direction.y > 0)
                    currentSwipe = SwipeDirection.Up;
                else
                {
                    currentSwipe = SwipeDirection.Down;
                    //Should force end swipe
                    ignoreSwipe = true;
                }
            }
        }
/*
        if (currentTouch.deltaPosition.sqrMagnitude != 0)
        {
            if (swiping == false)
            {
                swiping = true;
                lastPosition = currentTouch.position;
                return;
            }
            else
            {
                if (!eventSent)
                {
                    Vector2 direction = currentTouch.position - lastPosition;

                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                    {
                        if (direction.x > 0)
                            currentSwipe = SwipeDirection.Right;
                        else
                            currentSwipe = SwipeDirection.Left;
                    }
                    else
                    {
                        if (direction.y > 0)
                            currentSwipe = SwipeDirection.Up;
                        else
                            currentSwipe = SwipeDirection.Down;
                    }

                    eventSent = true;
                }
            }
        }
        else
        {
            swiping = false;
            eventSent = false;
        }*/
    }
}
