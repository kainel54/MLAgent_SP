// MazeAgent.cs
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class MazeAgent : Agent
{
    Rigidbody rb;
    public Transform goal;
    float moveSpeed = 2f, turnSpeed = 200f;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // 에이전트, 목표 위치 랜덤화
        transform.localPosition = new Vector3(Random.Range(0, 10), 0.5f, Random.Range(0, 10));
        goal.localPosition = new Vector3(Random.Range(0, 10), 0.5f, Random.Range(0, 10));
        rb.velocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 1) 비주얼 관측 (CameraSensorComponent로 자동 수집)
        // 2) 벡터 관측: 목표까지 방향
        Vector3 toGoal = (goal.localPosition - transform.localPosition) / 10f;
        sensor.AddObservation(toGoal);
        sensor.AddObservation(rb.velocity.magnitude / 5f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Discrete: [0] 전진/후진/정지, [1] 좌회전/우회전/정지
        int move = actions.DiscreteActions[0] - 1;
        int turn = actions.DiscreteActions[1] - 1;

        rb.MovePosition(transform.position + transform.forward * move * Time.deltaTime * moveSpeed);
        transform.Rotate(Vector3.up, turn * turnSpeed * Time.deltaTime);

        float dist = Vector3.Distance(transform.localPosition, goal.localPosition);
        if (dist < 1f)
        {
            SetReward(+1f);
            EndEpisode();
        }
        AddReward(-0.001f);  // 시간 패널티
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var d = actionsOut.DiscreteActions;
        d[0] = Input.GetAxisRaw("Vertical") > 0 ? 2 : (Input.GetAxisRaw("Vertical") < 0 ? 0 : 1);
        d[1] = Input.GetAxisRaw("Horizontal") > 0 ? 2 : (Input.GetAxisRaw("Horizontal") < 0 ? 0 : 1);
    }
}
