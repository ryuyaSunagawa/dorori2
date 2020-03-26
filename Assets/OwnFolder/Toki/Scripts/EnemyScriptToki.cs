using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyScriptToki : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float range = 10.0f;

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

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
        ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));

        if (Physics.Raycast(ray, out hit, range))
        {
            agent.isStopped = true;
            transform.LookAt(hit.transform);
            transform.position += transform.forward * speed * Time.deltaTime;

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
           
        }
        else
        {
            agent.isStopped = false;
            // エージェントが現目標地点に近づいてきたら、
            // 次の目標地点を選択します
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
              GotoNextPoint();
        }
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.name =="Player")
        {
            Debug.Log("死んだよ！");
        }
    }

}
