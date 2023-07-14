using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LIftCamera : MonoBehaviour
{
    [Tooltip("Ø‚è‘Ö‚¦Œã‚ÌƒJƒƒ‰")]
    public CinemachineVirtualCamera _virtualCamera;

    public CinemachineVirtualCamera GetVirtualCamera {
        get { return _virtualCamera; }
    }

}
