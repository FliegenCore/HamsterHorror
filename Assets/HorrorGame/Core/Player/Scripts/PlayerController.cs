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
            m_Player = m_AssetLoader.InstantiateSync<Player>(playerAsset.gameObject, null);

            m_PlayerTask.SetResult(m_Player);
        }

        public void Init()
        {
            SubscribeOnInput();
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

            m_Player.Rigidbody.AddForce(endDir * Time.fixedDeltaTime * m_Player.Config.Speed);
        }

        private void MouseRotate(Vector2 direction)
        {
            if (m_Player == null)
            {
                return;
            }

            Transform camera = m_Player.CameraRoot;
            camera.rotation = Quaternion.Lerp(Quaternion.Euler(camera.eulerAngles), Quaternion.Euler(camera.localEulerAngles.x + -direction.y,
                camera.localEulerAngles.y + direction.x, 0), m_Player.Config.RotationSpeed * Time.deltaTime);
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