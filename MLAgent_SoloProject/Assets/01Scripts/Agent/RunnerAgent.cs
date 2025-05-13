// MazeAgent.cs
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class RunnerAgent : Agent
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float diveForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Target")]
    [SerializeField] private Transform _goal;

    private Rigidbody _rigid;
    private bool _isGrounded;
    private bool _hasDived = false;

    public override void Initialize()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    

    public override void OnEpisodeBegin()
    {
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
        transform.localPosition = new Vector3(0, 1, 0);
        transform.localRotation = Quaternion.identity;

        _goal.localPosition = new Vector3(Random.Range(8f, 12f), 0.5f, Random.Range(-2f, 2f));
        _hasDived = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(_rigid.velocity);
        sensor.AddObservation(_goal.localPosition);
        sensor.AddObservation(_isGrounded ? 1 : 0);
        sensor.AddObservation(_hasDived ? 1 : 0);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        float jump = actions.ContinuousActions[2];
        float dive = actions.ContinuousActions[3];

        Vector3 move = new Vector3(moveX, 0, moveZ);
        Vector3 worldMove = transform.TransformDirection(move);

        if (worldMove.magnitude > 0.1f)
        {
            _rigid.velocity = worldMove * moveSpeed * Time.fixedDeltaTime;

            Quaternion targetRot = Quaternion.LookRotation(worldMove);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed);
        }

        _isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 1f, groundLayer);

        // 땅에 닿았을 때 Dive 초기화
        if (_isGrounded)
        {
            _hasDived = false;
        }

        // Jump
        if (_isGrounded && jump > 0.5f)
        {
            _rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _hasDived = false; // 공중 상태 시작 → Dive 가능해짐
        }

        // Dive (공중에서만 가능, 한 번만)
        if (!_isGrounded && dive > 0.5f && !_hasDived)
        {
            Vector3 diveDir = transform.forward + Vector3.up * 0.2f;
            _rigid.AddForce(diveDir.normalized * diveForce, ForceMode.Impulse);
            _hasDived = true;
        }

        float distToGoal = Vector3.Distance(transform.localPosition, _goal.localPosition);
        AddReward(-0.001f); // 시간 패널티

        if (distToGoal < 1.5f)
        {
            SetReward(+1f);
            EndEpisode();
        }

        if (transform.localPosition.y < -3f)
        {
            SetReward(-1f);
            EndEpisode();
        }

        

    }


}
