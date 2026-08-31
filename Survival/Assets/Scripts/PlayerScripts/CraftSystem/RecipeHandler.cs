using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RecipeHandler : MonoBehaviour
{
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] images = new Image[3];
    [SerializeField] private TMP_Text[] itemsAmountTexts = new TMP_Text[3];

    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;

    private CraftRecipe recipe;
    private void OnEnable()
    {
        craftButton.onClick.AddListener(CraftButtonPressed);
        EventBus<ShowItemCraftEvent>.Subscribe(ShowCraftComponents);
    }
    private void OnDisable()
    {
        craftButton.onClick.RemoveAllListeners();
        EventBus<ShowItemCraftEvent>.Unsubscribe(ShowCraftComponents);
    }
    private void ShowCraftComponents(ShowItemCraftEvent recipeEvent)
    {
        craftButton.onClick.RemoveAllListeners();
        foreach (var image in images)
        {
            image.gameObject.SetActive(false);
        }
        recipe = recipeEvent.Recipe;

        itemNameText.SetText(recipe.ItemToCraft.ItemName);
        itemDescriptionText.SetText(recipe.ItemToCraft.Description);

        for (int i = 0; i < recipeEvent.Recipe.Components.Count; i++)
        {
            images[i].gameObject.SetActive(true);
            images[i].sprite = recipeEvent.Recipe.Components[i]._item.Icon;

            itemsAmountTexts[i].SetText($"x{recipe.Components[i]._amount}");
        }
        craftButton.onClick.AddListener(CraftButtonPressed);
    }
    private void CraftButtonPressed()
    {
        ItemCraftedEvent itemCraftedEvent = new ItemCraftedEvent()
        {
            itemRecipe = recipe
        };
        EventBus<ItemCraftedEvent>.Raise(itemCraftedEvent);
    }
}
