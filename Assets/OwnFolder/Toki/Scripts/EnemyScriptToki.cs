using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyScriptToki : MonoBehaviour
{

	const float enemyRangeStand = 10;
	const float enemyRangeSit = 6;

    Ray ray;
    RaycastHit hit;
    [SerializeField] private float speed = 1.0f;
    private float range = 10.0f;

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

	//distanceがtrueの時ここにtrueが入る
	bool nearPlayerFlg = false;

	// Start is called before the first frame update
	void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // autoBraking を無効にすると、目標地点の間を継続的に移動します
        //(つまり、エージェントは目標地点に近づいても
        // 速度をおとしません)
        agent.autoBraking = false;

        GotoNextPoint();
    }

    void GotoNextPoint()
    {
            // 地点がなにも設定されていないときに返します
            if (points.Length == 0)
                return;

            // エージェントが現在設定された目標地点に行くように設定します
            agent.destination = points[destPoint].position;

            // 配列内の次の位置を目標地点に設定し、
            // 必要ならば出発地点にもどります
            destPoint = (destPoint + 1) % points.Length;
    }
	
    // Update is called once per frame
    void Update()
    {

		nearPlayerFlg = GameManager.Instance.Distance( this.gameObject.transform );

		ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));

		if( GameManager.Instance.playerSitdown == true )
		{
			range = enemyRangeSit;
		}
		else if( GameManager.Instance.playerSitdown == false )
		{
			range = enemyRangeStand;
		}

        if (Physics.Raycast(ray, out hit, range))
        {
            agent.isStopped = true;
            transform.LookAt(hit.transform);
            transform.position += transform.forward * speed * Time.deltaTime;

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
        }
        else
        {
			Debug.DrawRay( transform.position, transform.TransformDirection( Vector3.forward ) * hit.distance, Color.red );
			agent.isStopped = false;
            // エージェントが現目標地点に近づいてきたら、
            // 次の目標地点を選択します
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
              GotoNextPoint();
		}
	}

	public void Death()
	{
		gameObject.SetActive( false );
	}

	private void OnTriggerEnter( Collider other )
	{
		if( other.gameObject.name == "Player" )
		{
			Debug.Log( "死んだよ！" );
		}
	}

}
