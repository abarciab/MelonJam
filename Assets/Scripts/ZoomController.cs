using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class ZoomController : MonoBehaviour
{
    [Header("lv1 zoom (plankton)")]
    [SerializeField] Vector2 ecoOffsetlv1;
    [SerializeField] List<GameObject> lv1Objects = new List<GameObject>();
    [SerializeField] List<SortingGroup> lv1SortGroups = new List<SortingGroup>();

    [Header("lv2 zoom (medium fish + shore)")]
    [SerializeField] Vector2 ecoOffsetlv2;
    [SerializeField] List<GameObject> lv2Objects = new List<GameObject>();
    [SerializeField] List<SortingGroup> lv2SortGroups = new List<SortingGroup>();

    [Space()]
    [SerializeField] string defaultLayer;
    [SerializeField] string hiddenLayer;

    int zoomLevel;
    Ecosystem eco;
    bool topSide;

    private void Start() {
        eco = FindObjectOfType<Ecosystem>();
        GameManager.i.OnDayEnd.AddListener(ZoomOut);
        GameManager.i.OnGoDown.AddListener(() => { topSide = false; });
    }

    void ZoomOut() {
        topSide = true;
        SetZoom(2);
    }

    private void Update() {
        UpdateZoom();
    }

    public void SetZoom(int zoom) {
        zoomLevel = zoom;
    }

    void UpdateZoom() {

        foreach (var g in lv1Objects) g.SetActive(zoomLevel == 1 && topSide);
        foreach (var g in lv1SortGroups) g.sortingLayerName = zoomLevel == 1 && topSide ? defaultLayer : hiddenLayer;

        foreach (var g in lv2Objects) g.SetActive(zoomLevel == 2 && topSide);
        foreach (var g in lv2SortGroups) g.sortingLayerName = zoomLevel == 2 && topSide ? defaultLayer : hiddenLayer;

        eco.offset = zoomLevel == 1 ? ecoOffsetlv1 : ecoOffsetlv2;
    }
}
