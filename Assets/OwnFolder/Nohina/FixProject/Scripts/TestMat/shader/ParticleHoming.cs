using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHoming : MonoBehaviour
{
	ParticleSystem ps;
	ParticleSystem.Particle[] m_Particles;

	// ターゲットをセットする
	public Transform target;
	public float _speed = 6.0f;    // 1秒間に進む距離
	public float _rotSpeed = 180.0f;  // 1秒間に回転する角度

	[SerializeField] bool targetting = true;

	// Start is called before the first frame update
	void Start()
    {
		ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
		if( targetting == true )
		{
			m_Particles = new ParticleSystem.Particle[ ps.main.maxParticles ];
			int numParticlesAlive = ps.GetParticles( m_Particles );
			for( int i = 0; i < numParticlesAlive; i++ )
			{
			}
			ps.SetParticles( m_Particles, numParticlesAlive );
		}
	}
}
