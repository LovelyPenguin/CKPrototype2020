using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AIDetect
{
    [CustomEditor(typeof(AIMaster))]
    [CanEditMultipleObjects]
    public class AIEditor : Editor
    {
        private void OnSceneGUI()
        {
            AIMaster ai = (AIMaster)target;
            Transform transform = ai.transform;

            Handles.color = new Color(0.5f, 0.5f, 1.0f, 0.2f);
            Handles.DrawSolidArc(transform.position, transform.up, transform.forward, ai.seekAngle / 2, ai.seekDistance);
            Handles.DrawSolidArc(transform.position, transform.up, transform.forward, -ai.seekAngle / 2, ai.seekDistance);

            Handles.color = new Color(1.0f, 0.5f, 0.5f, 0.2f);
            Handles.DrawSolidArc(transform.position, transform.up, transform.forward, ai.attackAngle / 2, ai.attackDistance);
            Handles.DrawSolidArc(transform.position, transform.up, transform.forward, -ai.attackAngle / 2, ai.attackDistance);

            Handles.color = new Color(0f, 0f, 0f, 0.2f);
            Handles.DrawSolidDisc(Vector3.zero, Vector3.up, ai.randomMoveRange);

            Handles.color = new Color(1f, 1f, 0f, 0.5f);
            Handles.DrawSolidDisc(ai.lightSwitchLocation, Vector3.up, 0.5f);
        }
    }
}