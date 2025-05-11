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
        // ������Ʈ, ��ǥ ��ġ ����ȭ
        transform.localPosition = new Vector3(Random.Range(0, 10), 0.5f, Random.Range(0, 10));
        goal.localPosition = new Vector3(Random.Range(0, 10), 0.5f, Random.Range(0, 10));
        rb.velocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 1) ���־� ���� (CameraSensorComponent�� �ڵ� ����)
        // 2) ���� ����: ��ǥ���� ����
        Vector3 toGoal = (goal.localPosition - transform.localPosition) / 10f;
        sensor.AddObservation(toGoal);
        sensor.AddObservation(rb.velocity.magnitude / 5f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Discrete: [0] ����/����/����, [1] ��ȸ��/��ȸ��/����
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
        AddReward(-0.001f);  // �ð� �г�Ƽ
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var d = actionsOut.DiscreteActions;
        d[0] = Input.GetAxisRaw("Vertical") > 0 ? 2 : (Input.GetAxisRaw("Vertical") < 0 ? 0 : 1);
        d[1] = Input.GetAxisRaw("Horizontal") > 0 ? 2 : (Input.GetAxisRaw("Horizontal") < 0 ? 0 : 1);
    }
}
