using System.Collections;
using UnityEngine;

public class PanDetection : MonoBehaviour
{
    private float minimumDistance = 0.2f;
    private float maximumTime = 2f;
    private float directionThreshold = 0.9f;
    [SerializeField]
    private GameObject trailGB;

    private InputManager inputManager;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;

    private Coroutine trailCoroutine;

    private void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<InputManager>();
    }

    private void OnEnable()
    {
        inputManager.OnStartPan += SwipeStart;
        inputManager.OnEndPan += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.OnStartPan -= SwipeStart;
        inputManager.OnEndPan -= SwipeEnd;
    }
    private void SwipeStart(Vector2 position, float time)
    {
        trailGB.SetActive(true);
        trailGB.transform.position = position;
        startPosition = position;
        startTime = time;
        trailCoroutine = StartCoroutine(trail());
    }

    private IEnumerator trail()
    {
        while (true)
        {
            trailGB.transform.position = inputManager.PrimaryPosition();
            yield return null;
        }
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        trailGB.SetActive(false);
        StopCoroutine(trailCoroutine);
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if(Vector3.Distance(startPosition,endPosition) >= minimumDistance &&
            (endTime - startTime <= maximumTime))
        {
            //move camera based on distance
            //Debug.Log("Swiped");

            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            //SwipeDirection(direction2D);
            MoveCamera(direction);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            Debug.Log("Swipe Up");
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Debug.Log("Swipe Down");
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.Log("Swipe Left");
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.Log("Swipe Right");
        }
    }

    private void MoveCamera(Vector3 direction)
    {
        Camera.main.transform.Translate(direction);
    }
}
