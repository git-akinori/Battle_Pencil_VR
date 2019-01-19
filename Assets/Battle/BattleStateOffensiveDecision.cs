﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// バトルの攻守決定ステート
/// </summary>
public class BattleStateOffensiveDecision : IState<BattleContext> {

	const int waitSeconds = 3;
	bool isReset = false;

	public void ExecuteEntry(BattleContext context) {
		Debug.Log("[Entry] Battle State : Offensive Decision");
		BattleManager.Instance.StartOutcomeDetection();
	}

	public void ExecuteUpdate(BattleContext context) {

		// 全ての処理が終わっていないなら
		if (!context.isDone) {

			// どちらの出目も０でないなら
			if (!BattleManager.Instance.CheckOutcomesContainZero()) {
			
				// 互いの出目が異なる値なら
				if (BattleManager.Instance.CheckEachOutcomeDifferent()) {

					// 両者のモンスターを召喚
					SummonMonsters();

					// 攻守決定
					OffensiveDecision();

					// n秒後にステート遷移
					Observable.Timer(TimeSpan.FromSeconds(waitSeconds)).Subscribe(_ =>
						context.ChangeState(context.stateFight)
					);

					context.isDone = true;
				}
				// 互いの出目が同値なら
				else if (BattleManager.Instance.CheckEachOutcomeSame()) {
					BattleManager.Instance.StartOutcomeDetection();
				}
			}
		}

	}

	public void ExecuteExit(BattleContext context) {
		Debug.Log("[Exit] Battle State : Offensive Decision");
	}
	
	// 両者のモンスターを召喚
	public void SummonMonsters() {
		OperatorManager.Instance.PlayerController.OperatorModel.pencil.SummonMonster();
		OperatorManager.Instance.ComputerController.OperatorModel.pencil.SummonMonster();
	}

	// 攻守決定
	public void OffensiveDecision() {
		if (OperatorManager.Instance.PlayerController.OperatorModel.pencil.Outcome
			> OperatorManager.Instance.ComputerController.OperatorModel.pencil.Outcome)
			BattleManager.Instance.ActiveController = OperatorManager.Instance.PlayerController;
		else
			BattleManager.Instance.ActiveController = OperatorManager.Instance.ComputerController;
	}
}
