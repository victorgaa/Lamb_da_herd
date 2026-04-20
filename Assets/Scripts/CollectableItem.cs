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
            Messenger<string>.Broadcast(GameEvent.PICKUP_ITEM, value.ToString());
            Destroy(this.gameObject);
        }
    }

    public enum CollectableType
    {
        Paint,
        Grenade
    }
}
