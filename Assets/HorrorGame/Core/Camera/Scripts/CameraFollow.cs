using UnityEngine;

namespace Core.Gameplay
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 m_Offset;
        [SerializeField] private Transform m_CameraPos;
        [SerializeField] private Transform m_TargetPos;

        private void FixedUpdate()
        {
            m_CameraPos.position = m_TargetPos.position + m_Offset;
        }
    }

}
