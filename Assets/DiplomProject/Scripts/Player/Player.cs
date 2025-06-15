using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController Controller; // Инкапсулировать объект
    public Animator Animator;
    public Collider AttackCollider;
    public Transform ModelTransform => _modelTransform;

    [SerializeField] private Transform _modelTransform;
}