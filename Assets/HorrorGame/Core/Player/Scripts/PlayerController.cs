using Assets;
using Core.Common;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Gameplay
{
    [Order(0)]
    public class PlayerController : MonoBehaviour, IControllerEntity
    {
        private const string m_PlayerName = "Player";

        [Inject] private EventManager m_EventManager;
        [Inject] private AssetLoader m_AssetLoader;
        [Inject] private InputController m_InputController;

        [SerializeField] private Transform m_SpawnPoint;

        private TaskCompletionSource<Player> m_PlayerTask;
        private Player m_Player;

        public async Task<Player> GetPlayer()
        {
            return await m_PlayerTask.Task;
        }

        public async void PreInit()
        {
            m_PlayerTask = new TaskCompletionSource<Player>();

            var playerAsset = await m_AssetLoader.LoadAsync<Player>(m_PlayerName);
            m_Player = m_AssetLoader.InstantiateSync<Player>(playerAsset.gameObject, m_SpawnPoint.position, Quaternion.identity);

            m_PlayerTask.SetResult(m_Player);
        }

        public void Init()
        {
            SubscribeOnInput();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Application.targetFrameRate = 60;
            }
            if (Input.GetKeyUp(KeyCode.K))
            {
                Application.targetFrameRate = 120;
            }
        }

        private void Move(Vector3 direction)
        {
            if (m_Player == null)
            {
                return;
            }

            Vector3 forward = m_Player.CameraRoot.transform.forward;
            Vector3 right = m_Player.CameraRoot.transform.right;

            Vector3 endDir = new Vector3(forward.x, 0, forward.z) * direction.z + right * direction.x;
            Vector3 targetVelocity = endDir.normalized * m_Player.Config.Speed;

            m_Player.CharacterController.Move(targetVelocity * Time.deltaTime);
        }

        private void MouseRotate(Vector2 direction)
        {
            if (m_Player == null)
            {
                return;
            }

            Transform camera = m_Player.CameraRoot;
            Transform playerTransform = m_Player.transform;

            float rotateSpeed = m_Player.Config.RotationSpeed;

            float cameraRotationX = direction.y * rotateSpeed * Time.deltaTime;
            float bodyRotationY = direction.x * rotateSpeed * Time.deltaTime;

            float xClamped = Mathf.Clamp(cameraRotationX, -90f, 90f);

            camera.localRotation *= Quaternion.Euler(xClamped, 0f, 0f);
            playerTransform.Rotate(Vector3.up * bodyRotationY);
        }

        private void SubscribeOnInput()
        {
            m_InputController.OnKeyboardControl += Move;
            m_InputController.OnMouseMove += MouseRotate;
        }

        private void UnsubscribeOnInput()
        {
            m_InputController.OnKeyboardControl -= Move;
            m_InputController.OnMouseMove -= MouseRotate;
        }

        private void OnDestroy()
        {
            UnsubscribeOnInput();
        }
    }
}
