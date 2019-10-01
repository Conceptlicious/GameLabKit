using UnityEngine;
using GameLab;


[CreateAssetMenu(fileName = "New Room 1 Character", menuName = "Room 1/Character")]
public class Character : ScriptableObject
{
	[Header("Male")]
	public Sprite Male;
	public Sprite MBlind;
	public Sprite MBlindDeaf;
	public Sprite MBlindDeafDepressed;
	public Sprite MBlindDepressed;
	public Sprite MBlindDisabled;
	public Sprite MBlindDisabledDeaf;
	public Sprite MBlindDisabledDeafDepressed;
	public Sprite MDeaf;
	public Sprite MDeafDepressed;
	public Sprite MDepressed;
	public Sprite MDisabled;
	public Sprite MDisabledDeaf;
	public Sprite MDisabledDeafDepressed;
	public Sprite MDisabledBlindDepressed;
	public Sprite MDisabledDepressed;

	[Header("Female")]
	public Sprite Female;
	public Sprite FBlind;
	public Sprite FBlindDeaf;
	public Sprite FBlindDeafDepressed;
	public Sprite FBlindDepressed;
	public Sprite FBlindDisabled;
	public Sprite FBlindDisabledDeaf;
	public Sprite FBlindDisabledDeafDepressed;
	public Sprite FDeaf;
	public Sprite FDeafDepressed;
	public Sprite FDepressed;
	public Sprite FDisabled;
	public Sprite FDisabledDeaf;
	public Sprite FDisabledDeafDepressed;
	public Sprite FDisabledBlindDepressed;
	public Sprite FDisabledDepressed;
}
