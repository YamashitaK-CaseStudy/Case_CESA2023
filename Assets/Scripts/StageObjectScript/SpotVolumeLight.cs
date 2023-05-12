using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotVolumeLight : MonoBehaviour
{
    private void Start()
    {
        var coneRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();

        coneRenderer.sharedMaterial.SetVector("_lightPos", transform.position);

        var scale = transform.lossyScale;
        if (scale.z == 0)
        {
            coneRenderer.sharedMaterial.SetFloat("_rootSize", float.Epsilon);
        }
        else
        {
            coneRenderer.sharedMaterial.SetFloat("_rootSize", scale.z);
        }

        coneRenderer.sharedMaterial.SetColor("_Color", GetComponent<Light>().color);

        var localZ_OnWorld = transform.forward;
        coneRenderer.sharedMaterial.SetVector("_direction", new Vector3(-localZ_OnWorld.x, -localZ_OnWorld.y, -localZ_OnWorld.z));

    }
    private void OnValidate()
    {
        var coneRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();

        coneRenderer.sharedMaterial.SetVector("_lightPos", transform.position);

        var scale = transform.lossyScale;
        if (scale.z == 0)
        {
            coneRenderer.sharedMaterial.SetFloat("_rootSize", float.Epsilon);
        }
        else
        {
            coneRenderer.sharedMaterial.SetFloat("_rootSize", scale.z);
        }

        coneRenderer.sharedMaterial.SetColor("_Color", GetComponent<Light>().color);

        var localZ_OnWorld = transform.forward;
        coneRenderer.sharedMaterial.SetVector("_direction", new Vector3(-localZ_OnWorld.x, -localZ_OnWorld.y, -localZ_OnWorld.z));

    }

}
