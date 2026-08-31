using UnityEngine;

public class TheHand : MonoBehaviour
{
    public Transform handPalm;
    private void OnEnable()
    {
        EventBus<ItemInHands>.Subscribe(PutItemInHands);
    }
    private void OnDisable()
    {
        EventBus<ItemInHands>.Unsubscribe(PutItemInHands);
    }

    private void PutItemInHands(ItemInHands item)
    {
        if (item._Item == null)
        {
            RemoveItemInHand();
            return;
        }

        if (item._Item.InHandPrefab != null)
        {
            RemoveItemInHand();

            GameObject objectInHands = Instantiate(item._Item.InHandPrefab);

            objectInHands.transform.position = handPalm.transform.position;
            objectInHands.transform.rotation = handPalm.transform.rotation;
            objectInHands.transform.SetParent(handPalm.transform);
        }

    }
    private void RemoveItemInHand()
    {
        if(handPalm.transform.childCount > 0)
            Destroy(handPalm.transform.GetChild(0).gameObject);
    }
}
