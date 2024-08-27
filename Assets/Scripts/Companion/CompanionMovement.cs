using System.Collections;
using UnityEngine;

public class CompanionMovement : MonoBehaviour
{
    // The position the companion will move to in order to greet the player
    [SerializeField] private Transform firstPositionTransform;

    // Movement duration
    [SerializeField] private float duration = 6f; // Duration for movement

    private CompanionAnimation companionAnimation;
    private DialogueHandler dialogueHandler;
    private void Awake()
    {
        TryGetComponent(out companionAnimation);
        TryGetComponent(out dialogueHandler);
    }
    private void Start()
    {
        if (firstPositionTransform != null)
        {
            StartCoroutine(Introduction());
        }
    }

    // This method will handle the waving of the companion,once it's done it will move to the first transform Position
    private IEnumerator Introduction()
    {
        if (companionAnimation != null)
        {
            //add sound 
            if (dialogueHandler != null) dialogueHandler.PlaySound();

            yield return companionAnimation.ActivateWavingAnimation(true);
            
            StartCoroutine(MoveToFirstPosition());
        }

    }
    // Add a function that makes him move to the 2nd position
    private IEnumerator MoveToFirstPosition()
    {
        if(companionAnimation != null)
        {
            companionAnimation.ActivateMovingAnimation(true);
            yield return StartCoroutine(MoveGameObject(firstPositionTransform.position));
            companionAnimation.ActivateMovingAnimation(false);
        }
        
    }

    private IEnumerator MoveGameObject(Vector3 destination)
    {
        Vector3 startPosition = transform.position;
        // Interpolate only the x and z positions, keeping y constant
        Vector3 endPosition = new Vector3(destination.x, transform.position.y, destination.z);
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {

            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            transform.position = newPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPosition;
    }
}
