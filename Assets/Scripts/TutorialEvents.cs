using System;

public class TutorialEvents
{
    public event Action onTeleportLocation;

    public event Action onItemGrabbed;

    public event Action onItemActivated;

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
