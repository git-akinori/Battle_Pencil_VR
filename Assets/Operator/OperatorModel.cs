﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OperatorModel {

	public enum EOperator {
		Player, Computer
	}

	public EOperator eOperator;
	public Pencil pencil;
	public BaseMonsterBehaviour monsterBehaviour;
    public MonsterUIController monsterUI;
}
