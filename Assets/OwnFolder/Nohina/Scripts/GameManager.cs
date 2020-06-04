using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

	/// <summary>
	/// 変数群
	/// </summary>

	//パッドモード( falseならマウス )
	public bool padMode;

	//マウスロックをするかどうか
	[SerializeField] bool mouseLock;

	//プレイヤーの敵との距離
	List<double> distance = new List<double>();

	//カーソルロック
	bool cursorLockBool = false;
	public bool cursorLock
	{
		set {	this.cursorLockBool = value;	}
		get {	return this.cursorLockBool;		}
	}

	//プレイヤーディスタンスのゲッター
	double _playerDistance = 0;
	public double PlayerDistance
	{
		get {	return _playerDistance;		}
	}

	//プレイヤーオブジェクトのゲッター
	[SerializeField] Transform playerObj;
	public Transform getplayerObj
	{
		get {	return playerObj;	}
	}

	[SerializeField] Transform enemyObj;
	public Transform getenemyObj
	{
		get {
			return enemyObj;
		}
	}

	bool _playerSitdown = false;
	public bool playerSitdown
	{
		set {
			_playerSitdown = value;
		}
		get {
			return _playerSitdown;
		}
	}

	[SerializeField] int _playerStairState = 0;
	public int playerStairState
	{
		set {
			_playerStairState = value;
		}

		get {
			return _playerStairState;
		}
	}

	[SerializeField] GameObject _AnimationController;
	public GameObject AnimationController
	{
		get {
			return _AnimationController;
		}
	}

	/// <summary>
	/// プレイヤーが変化の術を使うときのフラグ
	/// </summary>
	bool _playerDisguiceMode = false;
	public bool playerDisguiceMode
	{
		set {
			_playerDisguiceMode = value;
		}
		get {
			return _playerDisguiceMode;
		}
	}

	/// <summary>
	/// プレイヤーに攻撃が当たった時のフラグ
	/// </summary>
	public bool playerDeathFlg { set; get; } = false;

	/// <summary>
	/// プレイヤーの残機数
	/// </summary>
	public int playerDeathNum { set; get; } = 0;

	/// <summary>
	/// プレイヤーが隠れ状態かどうか
	/// </summary>
	public bool playerHideFlg { set; get; } = false;

	/// <summary>
	/// 関数群
	/// </summary>

	//マウスロック
	void MouseLock()
	{
		cursorLock = true;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Locked;
	}

	//マウスロック解除
	void MouseUnlock()
	{
		cursorLock = false;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.None;
	}

	//マウスロック判定,MouseLockがtrueなら処理しない
	public void MouseLockComp()
	{
		if( mouseLock == false )
		{
			if( cursorLockBool == false && ( Input.GetMouseButtonDown( 0 ) == true ) )
			{
				MouseLock();
			}
			else if( cursorLockBool == true && ( Input.GetKeyDown( KeyCode.Escape ) ) )
			{
				MouseUnlock();
			}
		}
	}

	//プレイヤーと敵の差分を計算 *distanceは2乗していない*
	public bool Distance( Transform enemyObj )
	{
		//オフセット計算
		Vector3 offset = enemyObj.position - playerObj.position;
		_playerDistance = offset.sqrMagnitude;

		if( _playerDistance <= 25 )
		{
			return true;
		}

		return false;
	}

	private void Update()
	{
		
	}

}