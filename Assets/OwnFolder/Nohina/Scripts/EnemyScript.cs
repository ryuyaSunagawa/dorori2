using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Text;
using System.Linq;

public class EnemyScript : MoveActorClass
{

	[Range( 0, 1 )]
	[SerializeField] float actorSpeed;

	//自分のTextMeshPro描画
	[SerializeField] TextMeshPro myTmp;

	//distanceがtrueの時ここにtrueが入る
	bool nearPlayerFlg = false;

	void Update()
	{
		nearPlayerFlg = GameManager.Instance.Distance( this.gameObject.transform );

		DrawInformation();
	}

	private void FixedUpdate()
	{
		//動く処理
		MoveProcessor();
	}

	//動く処理
	void MoveProcessor()
	{
		float horizon = ReturnDirectFloat( "Horizontal", 0 );
		float vertical = ReturnDirectFloat( "Vertical", KeyCode.W );

		MoveActor( horizon, vertical, actorSpeed );
	}

	void DrawInformation()
	{
		myTmp.GetComponent<TextMeshPro>().text = "Position = " + transform.position + "\n"
											+ "normalized = " + transform.forward.normalized + "\n"
											+ "distance = " + GameManager.Instance.PlayerDistance + "\n"
											+ "distanceFlg = " + nearPlayerFlg;
	}

	//終了時
	private void OnApplicationQuit()
	{
		//Debug.Log( tickList[ 0 ] );
		//tickList.Remove( 0 );
		//Debug.Log( tickList.Average() );
	}

	public void Death()
	{
		gameObject.SetActive( false );
	}

}
