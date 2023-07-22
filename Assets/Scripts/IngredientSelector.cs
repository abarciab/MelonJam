using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class IngredientSelector : MonoBehaviour
{
    [System.Serializable]
    public class ButtonWrapper
    {
        public bool selected;
        public Image img;
        public Ingredient ingrd;
        public TextMeshProUGUI amount;

        public void ToggleButton() {
            selected = !selected;   
        }
    }

    [SerializeField] Transform buttonParent;
    [SerializeField] GameObject buttonPrefab;

    [SerializeField] Color selectedColor, unSelectedColor;
    [SerializeField] Button combineButton;

    List<ButtonWrapper> buttons = new List<ButtonWrapper>();

    List<Ingredient> currentlySelected = new List<Ingredient>();

    private void Start() {
        GameManager.i.OnDayEnd.AddListener(() => gameObject.SetActive(false));
    }

    private void OnEnable() {
        if (!GameManager.i) {
            gameObject.SetActive(false);
            return;
        }

        DeleteButtons();
        SpawnButtons();
        UpdateButtonVisuals();
    }

    public void toggleButtonSelection(int index) {
        buttons[index].ToggleButton();
        UpdateButtonVisuals();
        UpdateSelectedIngrdsList();
    }

    void UpdateSelectedIngrdsList() {
        currentlySelected.Clear();
        foreach (var b in buttons) if (b.selected) currentlySelected.Add(b.ingrd);
    }

    void DeleteButtons() {
        for (int i = 0; i < buttonParent.childCount; i++) {
            Destroy(buttonParent.GetChild(i).gameObject);
        }
    }

    void UpdateButtonVisuals() {
        int selectedCount = 0;
        foreach (var b in buttons) {
            b.img.color = b.selected ? selectedColor : unSelectedColor;
            if (b.selected) selectedCount += 1;
        }
        combineButton.GetComponent<Image>().color = selectedCount > 0 ? selectedColor : unSelectedColor;
        combineButton.enabled = selectedCount > 0;
    }

    void SpawnButtons() {
        buttons.Clear();
        var ingrds = GameManager.i.labCon.GetIngredients();

        foreach (var entry in ingrds) {
            if (entry.Value <= 0) continue;

            var newButton = Instantiate(buttonPrefab, buttonParent);
            newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = entry.Key.name;
            int index = buttons.Count;
            newButton.GetComponent<Button>().onClick.AddListener(delegate {toggleButtonSelection(index); });

            var newWrapper = new ButtonWrapper {
                img = newButton.GetComponent<Image>(),
                ingrd = entry.Key,
                amount = newButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>()
            };
            newWrapper.amount.text = entry.Value.ToString();

            buttons.Add(newWrapper);
        }
    }

    public void Combine() {
        GameManager.i.labCon.AttemptCraft(currentlySelected);
    }
}
