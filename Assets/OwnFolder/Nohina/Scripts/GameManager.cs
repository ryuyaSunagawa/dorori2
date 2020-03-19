using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

	//パッドモード( falseならマウス )
	public bool padMode;

	public void Test()
	{
		Debug.Log( "This is Singleton!" );
	}

}
