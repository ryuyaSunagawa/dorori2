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

	//List<long> tickList = new List<long>();

	//現在の処理のティック
	//int nowTick = 0;

	void Update()
	{

		//処理時間計測
		Ray ray = new Ray( transform.position, transform.forward.normalized );
		RaycastHit hit;

		Debug.DrawRay( ray.origin, ray.direction * 10, Color.red );

		//System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		//sw.Start();

		bool raybool = Physics.Raycast( ray, out hit, 10.0f );

		//sw.Stop();
		//nowTick = ( int )sw.ElapsedTicks;
		//tickList.Add( sw.ElapsedTicks );
		//Debug.Log( sw.Elapsed + ", " + sw.ElapsedTicks );

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
											+ "normalized = " + transform.forward.normalized + "\n";
	}

	//終了時
	private void OnApplicationQuit()
	{
		//Debug.Log( tickList[ 0 ] );
		//tickList.Remove( 0 );
		//Debug.Log( tickList.Average() );
	}

}
