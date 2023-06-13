using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField, Range(0f, 360f)] float angle;
	[SerializeField] LayerMask targetMask;
	[SerializeField] LayerMask obstacleMask;

	private void Update()
	{
		FindTarget();
	}

	// ���� ����
	// -> ����, ����
	public void FindTarget()
	{
		// 1. ���� �ȿ� �ִ���
		Collider[] colliders = Physics.OverlapSphere(transform.position, range, targetMask);
		foreach (Collider collider in colliders)
		{
			// 2. ���� �ȿ� �ִ���
			Vector3 dirTarget = (collider.transform.position - transform.position).normalized;
			if (Vector3.Dot(transform.forward, dirTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad)) // => ������ ȣ�������� ��ȯ
				continue;

			// 3. �߰��� ��ֹ��� ������ (�þ߰�)
			// Ray�� ��� Ȯ���� �� ����
			float disttoTarget = Vector3.Distance(transform.position, collider.transform.position);
			if (Physics.Raycast(transform.position, dirTarget, disttoTarget, obstacleMask))
				continue;

			Debug.DrawRay(transform.position, dirTarget * disttoTarget, Color.red);
;		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);

		Vector3 rightDir = AngleToDir(transform.eulerAngles.y + angle * 0.5f);
		Vector3 leftDir = AngleToDir(transform.eulerAngles.y - angle * 0.5f);
		Debug.DrawRay(transform.position, rightDir * range, Color.yellow);
		Debug.DrawRay(transform.position, leftDir * range, Color.yellow);
	}

	private Vector3 AngleToDir(float angle)
	{
		float radian = angle * Mathf.Deg2Rad;
		return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
	}
}