using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightTarget : MonoBehaviour {

    //private Renderer[] renderers;
    //private Color color = new Color(112, 207, 20, 0);
    //private List<Material> materials;
    private GameObject arrow;

    //Gets all the materials from each renderer
    private void Start() {
        arrow = gameObject.transform.Find("Arrow").gameObject;
        arrow.SetActive(false);
        //materials = new List<Material>();
        //foreach (var renderer in renderers) {
            //A single child-object might have mutliple materials on it
            //that is why we need to all materials with "s"
            //materials.AddRange(new List<Material>(renderer.materials));
        //}
    }

    public void ToggleHighlight(bool val) {
        if (val) {
            arrow.SetActive(true);
        } else {
            arrow.SetActive(false);
        }
    }
}
