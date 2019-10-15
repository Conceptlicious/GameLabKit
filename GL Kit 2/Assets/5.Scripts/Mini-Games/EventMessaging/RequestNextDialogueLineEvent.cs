using GameLab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RequestNextDialogueLineEvent : GameLabEvent
{
	public string NextDialogueLine { get; set; } = null;
	public bool DialogueCompleted { get; set; } = false;
}
