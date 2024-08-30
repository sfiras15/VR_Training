using System.Collections;
using UnityEngine;

public class CompanionMovement : MonoBehaviour
{
    [SerializeField] private Transform firstPositionTransform;
    [SerializeField] private float firstPositionTravelDuration = 5f;
    [SerializeField] private Transform secondPositionTransform;
    [SerializeField] private float secondPositionTravelDuration = 6f;
    [SerializeField] private float rotationSpeed = 10f; // Speed of rotation

    private bool positionReached = false;
    private CompanionAnimation companionAnimation;
    private DialogueHandler dialogueHandler;

    private Transform playerTransform;

    // bools to restrict the quotes reading to only once

    private bool printerHeatingQuoteRead = false;
    private bool materialExtrusionQuoteRead = false;
    private bool homeReachedQuoteRead = false;
    private bool printQuoteRead = false;
    private bool babyStepQuoteRead = false;
    private bool waitPrintQuoteRead = false;
    private bool printCompletedQuoteRead = false;
    private bool printCooledDownQuoteRead = false;
    private void Awake()
    {
        TryGetComponent(out companionAnimation);
        TryGetComponent(out dialogueHandler);
    }

    private void OnEnable()
    {
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.onTabletUsed += MoveToSecondPositionSequence;
            if (dialogueHandler != null)
            {
                GameEventsManager.instance.mainLevelQuests.onMaterialLoaded += MaterialLoad;
                GameEventsManager.instance.mainLevelQuests.onHeatLevelAchieved += PrinterHeat;
                GameEventsManager.instance.mainLevelQuests.onMaterialExtruded += MaterialExtrusion;
                GameEventsManager.instance.mainLevelQuests.onHomePositionAchieved += PrinterHome;
                GameEventsManager.instance.mainLevelQuests.onPrintPreparationStarting += PrintStart;
                GameEventsManager.instance.mainLevelQuests.onBabyStepLevelAchieved += BabyStepCalibrated;
                GameEventsManager.instance.onPrintCompleted += PrintComplete;
                GameEventsManager.instance.mainLevelQuests.onPrinterCooledDown += PrinterCooledDown;


            }
        }
    }


    private void OnDisable()
    {
        if (GameEventsManager.instance != null) 
        {
            GameEventsManager.instance.onTabletUsed -= MoveToSecondPositionSequence;
            if (dialogueHandler != null)
            {
                GameEventsManager.instance.mainLevelQuests.onMaterialLoaded -= MaterialLoad;
                GameEventsManager.instance.mainLevelQuests.onHeatLevelAchieved -= PrinterHeat;
                GameEventsManager.instance.mainLevelQuests.onMaterialExtruded -= MaterialExtrusion;
                GameEventsManager.instance.mainLevelQuests.onHomePositionAchieved -= PrinterHome;
                GameEventsManager.instance.mainLevelQuests.onPrintPreparationStarting -= PrintStart;
                GameEventsManager.instance.mainLevelQuests.onBabyStepLevelAchieved -= BabyStepCalibrated;
                GameEventsManager.instance.onPrintCompleted -= PrintComplete;
                GameEventsManager.instance.mainLevelQuests.onPrinterCooledDown -= PrinterCooledDown;
            }
        }
    }
        
    private void MaterialLoad(bool value)
    {
        if (!printerHeatingQuoteRead)
        {
            printerHeatingQuoteRead = true;
            if (dialogueHandler != null) dialogueHandler.PlaySound(2);

        }
    }
    private void PrinterHeat()
    {
        if (!materialExtrusionQuoteRead)
        {
            materialExtrusionQuoteRead = true;
            if (dialogueHandler != null) dialogueHandler.PlaySound(3);

        }
    }
    private void MaterialExtrusion()
    {
        if (!homeReachedQuoteRead)
        {
            homeReachedQuoteRead = true;
            if (dialogueHandler != null) dialogueHandler.PlaySound(4);

        }
    }
    private void PrinterHome()
    {
        if (!printQuoteRead)
        {
            printQuoteRead = true;
            if (dialogueHandler != null) dialogueHandler.PlaySound(5);

        }
    }
    private void PrintStart()
    {
        if (!babyStepQuoteRead)
        {
            babyStepQuoteRead = true;
            if (dialogueHandler != null) dialogueHandler.PlaySound(6);

        }
    }
    private void BabyStepCalibrated()
    {
        if (!waitPrintQuoteRead)
        {
            waitPrintQuoteRead = true;
            if (dialogueHandler != null) dialogueHandler.PlaySound(7);

        }
    }

    private void PrintComplete()
    {
        if (!printCompletedQuoteRead)
        {
            printCompletedQuoteRead = true;
            if (dialogueHandler != null) dialogueHandler.PlaySound(8);
        }
    }
    private void PrinterCooledDown()
    {
        if (!printCooledDownQuoteRead)
        {
            printCooledDownQuoteRead = true;
            if (dialogueHandler != null) dialogueHandler.PlaySound(9);
        }
    }

    private void Start()
    {
        if (firstPositionTransform != null)
        {
            StartCoroutine(Introduction());
        }
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private IEnumerator Introduction()
    {
        if (companionAnimation != null)
        {
            if (dialogueHandler != null) dialogueHandler.PlaySound(0);
            yield return companionAnimation.ActivateWavingAnimation(true);
            StartCoroutine(MoveToFirstPosition());
        }
    }

    private IEnumerator MoveToFirstPosition()
    {
        if (companionAnimation != null)
        {
            companionAnimation.ActivateMovingAnimation(true);
            yield return StartCoroutine(MoveGameObject(firstPositionTransform.position, firstPositionTravelDuration));
            companionAnimation.ActivateMovingAnimation(false);
        }
    }


    private IEnumerator MoveGameObject(Vector3 destination, float duration)
    {
        Vector3 startPosition = transform.position;
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
    // Smoothly rotate towards the target direction
    private IEnumerator RotateTowardsTarget(Vector3 endPosition)
    {
        Vector3 startPosition = transform.position;
        Quaternion endRotation = Quaternion.LookRotation(endPosition - startPosition);

        while (Quaternion.Angle(transform.rotation, endRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
    public void MoveToSecondPositionSequence()
    {
        if (secondPositionTransform != null)
        {
            if (!positionReached) StartCoroutine(MoveToSecondPosition());
        }
    }

    private IEnumerator MoveToSecondPosition()
    {
        if (companionAnimation != null)
        {
            yield return StartCoroutine(RotateTowardsTarget(secondPositionTransform.position));
            if (dialogueHandler != null) dialogueHandler.PlaySound(1);
            companionAnimation.ActivateMovingAnimation(true);
            yield return StartCoroutine(MoveGameObject(secondPositionTransform.position, secondPositionTravelDuration));
            companionAnimation.ActivateMovingAnimation(false);
            yield return StartCoroutine(RotateTowardsTarget(playerTransform.position));
            
            positionReached = true;
        }
    }
}
