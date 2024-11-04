using UnityEngine;

namespace Core.Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerConfig m_Config;
        [SerializeField] private Rigidbody m_Rigidbody;
        [SerializeField] private Transform m_CameraRoot;

        public Transform CameraRoot => m_CameraRoot;
        public PlayerConfig Config => m_Config;
        public Rigidbody Rigidbody => m_Rigidbody;
    }
}
