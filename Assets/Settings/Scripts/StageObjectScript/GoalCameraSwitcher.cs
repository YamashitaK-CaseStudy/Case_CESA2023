using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCameraSwitcher : MonoBehaviour
{

    public void Deactivation()
    {
        _nearGoalCamera.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.root.tag != "Player")
        {
            return;
        }

        _nearGoalCamera.gameObject.SetActive(true);
        gameObject.SetActive(false);

        _otherObject.Deactivation();
    }

    [SerializeField] private NearGoalCamera _nearGoalCamera;
    [SerializeField] private GoalCameraSwitcher _otherObject;
}
