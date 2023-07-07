using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightTarget : MonoBehaviour {

    private Renderer[] renderers;
    private Color color = new Color(1, 1, 1, 0);
    private List<Material> materials;

    //Gets all the materials from each renderer
    private void Start() {
        renderers = GetComponentsInChildren<Renderer>();
        materials = new List<Material>();
        foreach (var renderer in renderers) {
            //A single child-object might have mutliple materials on it
            //that is why we need to all materials with "s"
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }

    public void ToggleHighlight(bool val) {
        if (val) {
            foreach (var material in materials) {
                //We need to enable the EMISSION
                material.EnableKeyword("_EMISSION");
                //before we can set the color
                material.SetColor("_EmissionColor", color);
            }
        } else {
            foreach (var material in materials) {
                //we can just disable the EMISSION
                //if we don't use emission color anywhere else
                material.DisableKeyword("_EMISSION");
            }
        }
    }
}
