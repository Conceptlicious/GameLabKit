using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
public class ActivePatternManager : Manager<ActivePatternManager>
{
	[SerializeField] private List<NodePattern> patterns;
	[SerializeField] private NodePattern activePattern;


	public void NextActivePattern(NodePattern newPattern)
	{
		if (!patterns.Contains(newPattern))
		{
			return;
		}
		activePattern = newPattern;
	}

	public bool IsActivePattern(NodePattern pattern)
	{
		return pattern == activePattern;
	}
}
