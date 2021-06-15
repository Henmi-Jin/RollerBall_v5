using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

// RollerAgent
public class RollerAgent : Agent
{
    public Transform target;
    Rigidbody rBody;

    // ���������ɌĂ΂��
    public override void Initialize()
    {
        this.rBody = GetComponent<Rigidbody>();
    }

    // �G�s�\�[�h�J�n���ɌĂ΂��
    public override void OnEpisodeBegin()
    {
        // RollerAgent�������痎�����Ă��鎞
        if (this.transform.localPosition.y < 0)
        {
            // RollerAgent�̈ʒu�Ƒ��x�����Z�b�g
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
        }

        // Target�̈ʒu�̃��Z�b�g
        target.localPosition = new Vector3(
            Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    // �ώ@�擾���ɌĂ΂��
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(target.localPosition); //Target��XYZ���W
        sensor.AddObservation(this.transform.localPosition); //RollerAgent��XYZ���W
        sensor.AddObservation(rBody.velocity.x); // RollerAgent��X���x
        sensor.AddObservation(rBody.velocity.z); // RollerAgent��Z���x
    }

    // �s�����s���ɌĂ΂��
    public override void OnActionReceived(float[] vectorAction)
    {
        // RollerAgent�ɗ͂�������
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * 10);

        // RollerAgent��Target�̈ʒu�ɓ���������
        float distanceToTarget = Vector3.Distance(
            this.transform.localPosition, target.localPosition);
        if (distanceToTarget < 1.42f)
        {
            AddReward(1.0f);
            EndEpisode();
        }

        // RollerAgent�������痎��������
        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    // �q���[���X�e�B�b�N���[�h�̍s�����莞�ɌĂ΂��
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }
}