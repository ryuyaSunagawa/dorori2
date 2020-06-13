using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHoming : MonoBehaviour
{

	ParticleSystem myParticleSystem;
	ParticleSystem.Particle[] myParticle;

	//ターゲットするポジションを決める
	[SerializeField] Transform targetPosition;

	[SerializeField] float threshold = 100f;
	[SerializeField] float intensity = 1f;
	[SerializeField] bool targetting = false;

	// Start is called before the first frame update
	void Start()
    {
		myParticleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
		if( targetting == true )
		{
			myParticle = new ParticleSystem.Particle[ myParticleSystem.main.maxParticles ];
			int numParticlesAlive = myParticleSystem.GetParticles( myParticle );
			for( int i = 0; i < numParticlesAlive; i++ )
			{
				var velocity = myParticleSystem.transform.TransformPoint( myParticle[ i ].velocity );
				var position = myParticleSystem.transform.TransformPoint( myParticle[ i ].position );

				//var period = myParticle[ i ].remainingLifetime * 0.9f;

				//////ターゲットと自分自身の差
				//var diff = targetPosition.TransformPoint( targetPosition.position ) - position;
				//Vector3 accel = ( diff - velocity * period ) * 2f / ( period * period );

				////加速度が一定以上だと追尾を弱くする
				//if( accel.magnitude > threshold )
				//{
				//	accel = accel.normalized * threshold;
				//}

				//// 速度の計算
				//velocity += accel * Time.deltaTime * intensity;
				//myParticle[ i ].velocity = myParticleSystem.transform.InverseTransformPoint( velocity );
			}
			myParticleSystem.SetParticles( myParticle, numParticlesAlive );
		}
	}
}
