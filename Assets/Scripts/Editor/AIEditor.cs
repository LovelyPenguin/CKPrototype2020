using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AIDetect
{
    [CustomEditor(typeof(SeekPlayer))]
    [CanEditMultipleObjects]
    public class AIEditor : Editor
    {
        private void OnSceneGUI()
        {
            SeekPlayer ai = (SeekPlayer)target;
            Transform transform = ai.transform;

            Handles.color = new Color(0.5f, 0.5f, 1.0f, 0.1f);
            Handles.DrawSolidArc(transform.position, transform.up, transform.forward, ai.seekAngle / 2, ai.seekDistance);
            Handles.DrawSolidArc(transform.position, transform.up, transform.forward, -ai.seekAngle / 2, ai.seekDistance);

            Handles.color = new Color(1.0f, 0.5f, 0.5f, 0.1f);
            Handles.DrawSolidArc(transform.position, transform.up, transform.forward, ai.attackAngle / 2, ai.attackDistance);
            Handles.DrawSolidArc(transform.position, transform.up, transform.forward, -ai.attackAngle / 2, ai.attackDistance);

            //Handles.DrawSolidDisc(transform.position, Vector3.up, 10);
        }
    }
}