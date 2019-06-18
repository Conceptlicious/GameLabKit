﻿using System.Collections.Generic;
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

	public enum Genders
	{
		Unspecified,
		Female,
		Male
	};

	[Flags]	public enum Disabilities
	{
		None = 0,
		LowVision = 1,
		ScreenReaders = 2,
		Anxiety = 4,
		Dyslectia = 8,
		PhysicalDisabileties = 16,
		Autism = 32,
		Deaf = 64,
		All = ~0,
	};

[Serializable]
public struct Persona
{
	public Disabilities Disability;
	public AgeGroup Age;
	public Genders Gender;

	public bool ComparePersonas(Persona other)
	{
		return other.Disability == Disability && other.Age == Age && other.Gender == Gender;
	}

	public bool ComparePersonasAge(Persona other)
	{
		return other.Age == Age;
	}

	public bool ComparePersonasGender(Persona other)
	{
		return other.Gender == Gender;
	}

	public bool ComparePersonasDisablity(Persona other)
	{
		return other.Disability == Disability;
	}
}