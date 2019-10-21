using GameLab;
using Ink.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RequestNextDialogueLineEvent : GameLabEvent
{
	public string NextDialogueLine { get; set; } = null;
	public bool KnotCompleted { get; set; } = false;
	public List<Choice> Choices { get; set; } = new List<Choice>();
	public bool HasChoices => Choices.Count > 0;
}
