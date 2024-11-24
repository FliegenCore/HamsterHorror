using System;
using UnityEngine;

namespace Core.Gameplay
{
    [Order(1)]
    public class InputController : MonoBehaviour, IControllerEntity
    {
        public event Action<Vector3> OnKeyboardControl;
        public event Action<Vector2> OnMouseMove;

        private KeyCode m_Forward;
        private KeyCode m_Backward;
        private KeyCode m_Right;
        private KeyCode m_Left;

        private Vector3 m_KeyboadDirection;
        private Vector3 m_MouseDirection;

        private int m_KeyboardX;
        private int m_KeyboardZ;

        [Inject] private PlayerController m_PlayerController;

        public void PreInit()
        {
            //TODO: Load from json
            m_Forward = KeyCode.W;
            m_Backward = KeyCode.S;
            m_Left = KeyCode.A;
            m_Right = KeyCode.D;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public async void Init()
        {
            Player player = await m_PlayerController.GetPlayer();
        }

        private void Update()
        {
            OnMouseMove?.Invoke(MouseInput());
            OnKeyboardControl?.Invoke(KeyboardInput());
        }

        private Vector2 MouseInput()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");

            return new Vector2 (mouseX, mouseY);
        }

        private Vector3 KeyboardInput()
        {
            if (Input.GetKey(m_Forward))
            {
                m_KeyboardZ = 1;
            }
            else if (Input.GetKey(m_Backward))
            {
                m_KeyboardZ = -1;
            }
            else
            {
                if (m_KeyboardZ != 0)
                { 
                    m_KeyboardZ = 0;
                }
            }


            if (Input.GetKey(m_Left))
            {
                m_KeyboardX = -1;
            }
            else if (Input.GetKey(m_Right))
            {
                m_KeyboardX = 1;
            }
            else
            {
                if (m_KeyboardX != 0)
                {
                    m_KeyboardX = 0;
                }
            }

            return new Vector3(m_KeyboardX, 0, m_KeyboardZ);
        }
    }
}
