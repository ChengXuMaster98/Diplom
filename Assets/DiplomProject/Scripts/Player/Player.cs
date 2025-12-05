using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _modelTransform;

    public Animator Animator;
    public Collider AttackCollider;

    public Cinemachine.CinemachineVirtualCamera VirtualCamera;

    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Transform BodyTransform { get; private set; }
    [field: SerializeField] public Transform CameraTransform { get; private set; }
}