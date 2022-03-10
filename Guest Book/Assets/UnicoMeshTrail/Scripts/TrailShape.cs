using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class TrailShape : MonoBehaviour 
{
	public Vector3[] pathPoints;
	public bool seal;
	private int pathPointCount;
	private bool lastSeal;
	private int lastChildCount;
	private Vector3 lastScale;
	private Vector3 lastChildCenterPos;
	private Vector3 childCenterPos;
	
	void Update() 
	{
		transform.position = Vector3.zero;
	}

	void Refresh ()
	{
		Vector3 childPosSum = Vector3.zero;
		for (int i=0;i<transform.childCount;i++)
		{
			childPosSum = childPosSum + transform.GetChild(i).position;
		}
		childCenterPos = childPosSum/transform.childCount;
		
		if (lastChildCount!=transform.childCount || lastSeal!=seal || lastScale!=transform.localScale || lastChildCenterPos!=childCenterPos)
		{
			pathPointCount = transform.childCount;
			if (seal) pathPointCount = pathPointCount + 1;
			
			if (transform.childCount>0)
			{
				pathPoints = new Vector3[pathPointCount];
				for (int i=0;i<transform.childCount;i++)
				{
					pathPoints[i] = transform.GetChild(i).position;
				}
				if (seal)
				{
					pathPoints[pathPointCount-1] = transform.GetChild(0).position;
				}
			}
			lastChildCount = transform.childCount;
			lastSeal = seal;
			lastScale = transform.localScale;
			lastChildCenterPos = childCenterPos;
//			Debug.Log("Refresh");
		}
	}
	
	void OnDrawGizmos ()
	{
		Refresh();
		if (pathPoints.Length>1)
		{
			DrawPath(pathPoints);
		}
		//Show Gizmos Sphere
		if (pathPoints.Length>0)
		{
			for (int i=0;i<pathPoints.Length;i++)
			{
				Gizmos.DrawSphere (pathPoints[i], 0.2f*transform.localScale.x);
			}
		}
	}

	public static Vector3 PointOnPath(Vector3[] path, float percent){
		float length = PathLength(path);
		float targetLength = length * percent;
		int nextIndex = 1;
		float seekLength = 0;
		float lastLength = 0;
		for(int i=1;i<path.Length;i++){
			lastLength = seekLength;
			seekLength += (path[i] - path[i-1]).magnitude;
			if (seekLength>=targetLength && lastLength<targetLength){
				nextIndex = i;
				break;
			}
		}
		float t = (targetLength - lastLength)/(seekLength - lastLength);
		Vector3 lastPoint = path[nextIndex - 1];
		Vector3 nextPoint = path[nextIndex];
		return Vector3.Lerp(lastPoint, nextPoint, t);
	}

	public static float PathLength(Vector3[] path){
		float length = 0;
		for(int i=1;i<path.Length;i++){
			length += (path[i] - path[i-1]).magnitude;
		}
		return length;
	}

	public static void DrawPath(Vector3[] path) {
		if(path.Length>0){
			for(int i=1;i<path.Length;i++){
				Debug.DrawLine(path[i], path[i-1], Color.white);
			}
		}
	}

}