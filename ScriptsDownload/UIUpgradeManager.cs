using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeManager : MonoBehaviour
{
    public Button healthButton;
    public Button damageButton;
    public PlayerStateSaver playerState;

    private bool hasChosenUpgrade = false;

    private void Start()
    {
        healthButton.onClick.AddListener(() =>
        {
            if (hasChosenUpgrade) return;
            playerState.CurrentHealth += 10f;
            hasChosenUpgrade = true;
        });

        damageButton.onClick.AddListener(() =>
        {
            if (hasChosenUpgrade) return;
            playerState.CurrentDamage += 10f;
            hasChosenUpgrade = true;
        });
    }
}