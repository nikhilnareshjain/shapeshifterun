using UnityEngine;
public class SwipeInput : MonoBehaviour
{
    // Minimum swipe distance required to be considered a swipe
    public float minSwipeDistance = 50f;

    // Stores the start position of the touch
    private Vector2 swipeStartPos;

    // Enum to represent different swipe directions
    public enum SwipeDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    // Function to detect the swipe direction
    public SwipeDirection GetSwipeDirection()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check for the touch phase
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    swipeStartPos = touch.position;

                    Debug.Log("Touch Began");
                    break;

                case TouchPhase.Ended:
                    Debug.Log("Touch Ended");
                    Vector2 swipeEndPos = touch.position;
                    float swipeDistance = Vector2.Distance(swipeStartPos, swipeEndPos);

                    // Check if it meets the minimum swipe distance
                    if (swipeDistance >= minSwipeDistance)
                    {
                        // Calculate the swipe direction
                        Vector2 swipeDirection = swipeEndPos - swipeStartPos;

                        if (swipeDirection.x > 0)
                        {
                            return SwipeDirection.Right;
                        }
                        else if (swipeDirection.x < 0)
                        {
                            return SwipeDirection.Left;
                        }

                        // Normalize the direction to get a consistent magnitude
                        swipeDirection.Normalize();

                        // Determine the swipe direction based on the angle
                        float swipeAngle = Mathf.Atan2(swipeDirection.y, swipeDirection.x) * Mathf.Rad2Deg;

                        // Make sure the angle is positive
                        if (swipeAngle < 0)
                        {
                            swipeAngle += 360;
                        }

                        // Define the angle ranges for different swipe directions
                        const float angleThreshold = 45f;
                        const float rightAngleMin = 360 - angleThreshold;
                        const float rightAngleMax = angleThreshold;
                        const float upAngleMin = 90 - angleThreshold;
                        const float upAngleMax = 90 + angleThreshold;
                        const float leftAngleMin = 180 - angleThreshold;
                        const float leftAngleMax = 180 + angleThreshold;
                        const float downAngleMin = 270 - angleThreshold;
                        const float downAngleMax = 270 + angleThreshold;

                        // Determine the swipe direction based on the angle
                        if (swipeAngle >= rightAngleMin && swipeAngle <= rightAngleMax)
                        {
                            return SwipeDirection.Right;
                        }
                        else if (swipeAngle >= upAngleMin && swipeAngle <= upAngleMax)
                        {
                            return SwipeDirection.Up;
                        }
                        else if (swipeAngle >= leftAngleMin && swipeAngle <= leftAngleMax)
                        {
                            return SwipeDirection.Left;
                        }
                        else if (swipeAngle >= downAngleMin && swipeAngle <= downAngleMax)
                        {
                            return SwipeDirection.Down;
                        }
                    }
                    break;
            }
        }

        return SwipeDirection.None;
    }
}
