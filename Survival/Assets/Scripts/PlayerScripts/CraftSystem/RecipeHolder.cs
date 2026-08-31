using UnityEngine;
using UnityEngine.UI;

public class RecipeHolder : MonoBehaviour
{
    [SerializeField] private CraftRecipe recipe;

    [SerializeField] private Button showCraftButton;
    [SerializeField] private Image craftingItemImage;
    private void OnEnable()
    {
        showCraftButton.onClick.AddListener(ShowCraftRecipe);
    }
    private void OnDisable()
    {
        showCraftButton.onClick.RemoveAllListeners();
    }
    private void Start()
    {
        if (craftingItemImage == null) return;
        craftingItemImage.sprite = recipe.ItemToCraft.Icon;
    }

    private void ShowCraftRecipe()
    {
        EventBus<ShowItemCraftEvent>.Raise(new ShowItemCraftEvent()
        {
            Recipe = recipe,
        }
        );
    }
}
