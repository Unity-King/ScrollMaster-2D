using ScrollMaster2D.Controllers;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button collectButton;

    void OnEnable()
    {
        TreeController.OnTreeInRange += HandleTreeInRange;
        TreeController.OnTreeOutOfRange += HandleTreeOutOfRange;
    }

    void OnDisable()
    {
        TreeController.OnTreeInRange -= HandleTreeInRange;
        TreeController.OnTreeOutOfRange -= HandleTreeOutOfRange;
    }

    private void HandleTreeInRange(TreeController tree)
    {
        collectButton.gameObject.SetActive(true);
        collectButton.onClick.RemoveAllListeners();
        collectButton.onClick.AddListener(tree.CollectResources);
    }

    private void HandleTreeOutOfRange()
    {
        collectButton.gameObject.SetActive(false);
        collectButton.onClick.RemoveAllListeners();
    }
}
