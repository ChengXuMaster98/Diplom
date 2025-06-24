using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _modelTransform;

    public CharacterController Controller; // ��������������� ������
    public Animator Animator;
    public Collider AttackCollider;
    public Transform ModelTransform => _modelTransform;

}