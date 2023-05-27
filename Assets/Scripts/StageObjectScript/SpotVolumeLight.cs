using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotVolumeLight : MonoBehaviour
{
    private void Start()
    {
        var coneRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();

        coneRenderer.material.SetVector("_lightPos", transform.position);

        var scale = transform.lossyScale;
        if (scale.z == 0)
        {
            coneRenderer.material.SetFloat("_rootSize", float.Epsilon);
        }
        else
        {
            coneRenderer.material.SetFloat("_rootSize", scale.z);
        }

        coneRenderer.material.SetColor("_Color", GetComponent<Light>().color);

        var localZ_OnWorld = transform.forward;
        coneRenderer.material.SetVector("_direction", new Vector3(-localZ_OnWorld.x, -localZ_OnWorld.y, -localZ_OnWorld.z));

    }
    private void OnValidate()
    {
        var coneRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();

        coneRenderer.material.SetVector("_lightPos", transform.position);

        var scale = transform.lossyScale;
        if (scale.z == 0)
        {
            coneRenderer.material.SetFloat("_rootSize", float.Epsilon);
        }
        else
        {
            coneRenderer.material.SetFloat("_rootSize", scale.z);
        }

        coneRenderer.material.SetColor("_Color", GetComponent<Light>().color);

        var localZ_OnWorld = transform.forward;
        coneRenderer.material.SetVector("_direction", new Vector3(-localZ_OnWorld.x, -localZ_OnWorld.y, -localZ_OnWorld.z));

    }

}
