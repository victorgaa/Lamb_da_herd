using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private CollectableType value;

    void Update()
    {
        transform.Rotate(0f, 180f * Time.deltaTime, 0f, Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerIdentity player = other.GetComponent<PlayerIdentity>();

            if (player != null)
            {
                Messenger<(string, int)>.Broadcast(
                    GameEvent.PICKUP_ITEM,
                    (value.ToString(), player.playerId)
                );
            }

            Destroy(gameObject);
        }
    }

    public enum CollectableType
    {
        Paint,
        Grenade
    }
}
