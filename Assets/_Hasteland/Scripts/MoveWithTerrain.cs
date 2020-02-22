using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithTerrain : MonoBehaviour
{
    public LayerMask m_terrainLayer;
    private Vector3 m_startingPos;
    private Transform m_targetTransform;
    private void OnEnable()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100,m_terrainLayer))
        {
            m_targetTransform = hit.transform;
            m_startingPos = m_targetTransform.InverseTransformPoint(transform.position);
        }
    }
    private void Update()
    {
        if (m_targetTransform != null)
        {
            Vector3 newPos = m_targetTransform.TransformPoint(m_startingPos);
            transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
        }
    }
}
