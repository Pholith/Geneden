using UnityEngine;
using UnityEngine.UI;

public class BatViewScript : MonoBehaviour
{
    public GameObject myView;

    private void OnClickEvent() {
        if (myView != null) {
            myView.SetActive(true);
        }
    }

    private void Update() {
        if (myView != null) {
            if (myView.activeInHierarchy) {
                if (Input.GetMouseButtonDown(0)) {
                    Debug.Log(gameObject);
                    myView.SetActive(false);
                }
            }
        }
    }
}
