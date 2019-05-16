using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawLines : MonoBehaviour
{	
	private LineRenderer lineBetweenNodes;
	[SerializeField] private Vector3 offset;
	List<Vector3> positions = new List<Vector3>();
	List<GameObject> savedButtons = new List<GameObject>();
	

	public void Start()
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
		lineBetweenNodes.positionCount = positions.Count;
		lineBetweenNodes.SetPositions(positions.ToArray());
	}
}
