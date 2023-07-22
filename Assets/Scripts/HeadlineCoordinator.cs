using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeadlineCoordinator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI headlineText;

    public void DisplayHeadline(string headline) {
        headlineText.text = headline;
        headlineText.gameObject.SetActive(true);
    }
}
