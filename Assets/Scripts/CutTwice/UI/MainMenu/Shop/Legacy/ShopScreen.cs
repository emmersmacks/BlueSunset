using UnityEngine;
using YG;
using YG.Insides;

namespace CutTwice.UI.MainMenu
{
    public class ShopScreen : MonoBehaviour
    {
#if UNITY_EDITOR
        [Tooltip(Langs.t_rootSpawnPurchases)]
#endif
        public Transform rootSpawnPurchases;
#if UNITY_EDITOR
        [Tooltip(Langs.t_purchasePrefab)]
#endif
        public GameObject purchasePrefab;

        private void OnEnable() => YG2.onGetPayments += UpdatePurchasesList;
        private void OnDisable() => YG2.onGetPayments -= UpdatePurchasesList;
        private void Start()
        {
            UpdatePurchasesList();
            YG2.onPurchaseSuccess += OnPurchaseSuccess;
        }

        private void OnPurchaseSuccess(string id)
        {
            UpdatePurchasesList();
        }

        public void UpdatePurchasesList()
        {
            // Clear catalog
            int childCount = rootSpawnPurchases.childCount;
            for (int i = childCount - 1; i >= 0; i--)
                Destroy(rootSpawnPurchases.GetChild(i).gameObject);

            // Spawn catalog
            for (int i = 0; i < YG2.purchases.Length; i++)
            {
                GameObject purchaseObj = Instantiate(purchasePrefab, rootSpawnPurchases);
                var panel = purchaseObj.GetComponent<PurchasePanel>();
                panel.id = YG2.purchases[i].id;
                panel.OnSelect.RemoveAllListeners();
                panel.OnSelect.AddListener(UpdatePurchasesList);
            }
        }

        private void OnDestroy()
        {
            YG2.onPurchaseSuccess -= OnPurchaseSuccess;
        }
    }
}