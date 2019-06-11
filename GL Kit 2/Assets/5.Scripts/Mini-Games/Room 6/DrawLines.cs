using System.Collections;
using GameLab;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawLines : BetterMonoBehaviour
{	
	private LineRenderer lineBetweenNodes;
	[SerializeField] private Vector3 offset;

	/*
	 * Always put a access modifiers in front of a variable. (private, public etc.)
	 * List<Vector3> positions = new List<Vector3>();
	 * List<GameObject> savedButtons = new List<GameObject>();
	*/
	private List<Vector3> positions = new List<Vector3>();
	private List<GameObject> savedButtons = new List<GameObject>();

	//Why is start public?
	private void Start()
	{
		lineBetweenNodes = GetComponent<LineRenderer>();
		lineBetweenNodes.positionCount = 0;
	}

	//Ran when correct node is pressed
	public void AddPosition(GameObject pGameObject)
	{
		positions.Add(pGameObject.transform.position + offset);
		savedButtons.Add(pGameObject);
		lineBetweenNodes.positionCount = positions.Count;
		lineBetweenNodes.SetPositions(positions.ToArray());
		pGameObject.SetActive(false);
	}

	//Ran when incorrect node is pressed
	public void ResetLine()
	{
		for (int i = 0; i < savedButtons.Count; i++)
		{
			savedButtons[i].SetActive(true);
		}
		positions.Clear();
		lineBetweenNodes.positionCount = 0;
		//lineBetweenNodes.SetPositions(positions.ToArray());
	}
}
