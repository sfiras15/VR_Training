using System;


//This was for testing the quest system in the tutorial level ,
// also for setting up some interactions with the ui in the tutorial level
public class TutorialEvents
{
    public event Action onTeleportLocation;

    public event Action onItemGrabbed;

    public event Action onItemActivated;

    // this events gets triggered when the Audio attached to the welcome Canvas finish playing which will 
    // allow the player to toggle the ray for interacting with the canvas
    public event Action onWelcomeCanvasFinished;


    public void WelcomeCanvasFinished()
    {
        if (onWelcomeCanvasFinished != null) onWelcomeCanvasFinished();
    }
    public void LocationChanged()
    {
        if (onTeleportLocation != null) onTeleportLocation();
    }
    public void ItemGrabbed()
    {
        if (onItemGrabbed != null) onItemGrabbed();
    }
    public void ItemActivated()
    {
        if (onItemActivated != null) onItemActivated();
    }
}
