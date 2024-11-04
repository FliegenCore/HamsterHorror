using UnityEngine;

namespace Core.Gameplay
{
    public class PlayerConfig : MonoBehaviour
    {
        [SerializeField] private float m_Speed;
        [SerializeField] private float m_SprintSpeed;
        [SerializeField] private float m_RotateSpeed;

        public float Speed => m_Speed;
        public float SprintSpeed => m_SprintSpeed;
        public float RotationSpeed => m_RotateSpeed;    
    }
}
