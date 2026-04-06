using UnityEngine;
public class BasePopup : MonoBehaviour
{
    public virtual void Open()
    {
        if (!IsActive())
        {
            gameObject.SetActive(true);
            Messenger.Broadcast(GameEvent.POPUP_OPENED);
        }
        else
        {
            Debug.LogError(this + ".Open() - already active!");
        }
    }

    public virtual void Close()
    {
        if (IsActive())
        {
            gameObject.SetActive(false);
            Messenger.Broadcast(GameEvent.POPUP_CLOSED);
        }
        else
        {
            Debug.LogError(this + ".Close() - already closed!");
        }
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
