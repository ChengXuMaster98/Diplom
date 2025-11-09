using UnityEngine;

public class CameraTargetFollower : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.7f, 0);

    private void LateUpdate()
    {
        if (playerBody == null)
            return;

        // Следуем за позицией игрока
        transform.position = playerBody.position + offset;

        // Не вращаемся вместе с игроком
        // Можно добавить плавность при желании
    }
}
