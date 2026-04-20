using UnityEngine;
public class BasePopup : MonoBehaviour
{
    public virtual void Open(BasePopup caller = null)
    {
        if (caller != null)
            Debug.Log($"{name} opened from {caller.name}");

        gameObject.SetActive(true);
        Messenger.Broadcast(GameEvent.POPUP_OPENED);
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
