using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBoxScript : MonoBehaviour
{

	ParticleSystem myParticle = null;
	Material myMaterial;
	Color myColor;

	//ボタンが押されているか
	[HideInInspector] public bool buttonDown = false;
	public float deathFrame = 0f;

	[SerializeField, Range( 0f, 1f )] float alpha = 0f;

	// Start is called before the first frame update
	void Start()
    {
		myParticle = GetComponent<ParticleSystem>();
		myMaterial = myParticle.GetComponent<ParticleSystemRenderer>().material;
		myColor = myMaterial.color;
	}

    // Update is called once per frame
    void Update()
    {
		if( buttonDown == true )
		{
			myMaterial.SetColor( "_Color", new Color( myColor.r, myColor.g, myColor.b, alpha ) );
		}
	}

	public bool deathParticle( bool buttonDown )
	{

		if( ( alpha -= ( Time.deltaTime * 2 ) ) > 0 )
		{
			myMaterial.SetColor( "_Color", new Color( myColor.r, myColor.g, myColor.b, alpha ) );
		}
		else if( alpha <= 0 )
		{
			Destroy( this.gameObject );
			return true;
		}

		return false;
	}
}
