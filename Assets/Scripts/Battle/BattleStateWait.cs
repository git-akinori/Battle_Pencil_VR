﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトルの初期化ステート
/// </summary>
public class BattleStateWait : IState<BattleContext> {

	public void ExecuteEntry(BattleContext context) {
		Debug.LogWarning("[Entry] Battle State : Wait");

		DataManager.Instance.SetPlayerPencil(0); // 仮置き
		DataManager.Instance.SetComputerPencilRandom();
	}

	public void ExecuteUpdate(BattleContext context) {

		// 初期化完了したなら
		if (context.isDone) {
			// 攻守決定ステートへ遷移
			context.ChangeState(context.stateFight);
		}
	}

	public void ExecuteExit(BattleContext context) {
		Debug.LogWarning("[Exit] Battle State : Wait");
	}

	/// <summary>
	/// 初期化処理
	/// </summary>
	bool Init() {

		if (BattleManager.Instance.ActiveController != null && BattleManager.Instance.NonActiveController != null
			&& OperatorManager.Instance.PlayerController.OperatorModel.pencil != null
			&& OperatorManager.Instance.ComputerController.OperatorModel.pencil != null) {

			OperatorManager.Instance.PlayerController.OperatorModel.pencil.SummonMonster();
			OperatorManager.Instance.ComputerController.OperatorModel.pencil.SummonMonster();

			return true;
		}
		if (OperatorManager.Instance.PlayerController != null) {
			BattleManager.Instance.ActiveController = OperatorManager.Instance.PlayerController;
		}
		if (OperatorManager.Instance.ComputerController != null) { 
			BattleManager.Instance.NonActiveController = OperatorManager.Instance.ComputerController;
        }

		return false;
	}
}