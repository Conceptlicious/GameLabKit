using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using GameLab;
using System;

	public enum AgeGroup
	{
		Child,
		Teenager,
		Adult,
		Elderly
	};

	public enum Gender
	{
		Unspecified,
		Female,
		Male
	};

	[Flags]	public enum Disabilities
	{
		LowVision,
		ScreenReaders,
		Anxiety,
		Dyslectia,
		PhysicalDisabileties,
		Autism,
		Deaf
	};
public struct Persona
{
	public Disabilities Disability;
	public AgeGroup Age;
	public Gender Gender;

}
