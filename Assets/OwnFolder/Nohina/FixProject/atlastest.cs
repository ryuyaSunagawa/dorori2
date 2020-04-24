using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class atlastest : MonoBehaviour
{

	[SerializeField]
	SpriteAtlas doronAtlas = null;

	[SerializeField]
	Sprite nowTexture;

	SpriteRenderer myRenderer;

	int texValue;
	float changeCount = 0f;

	[SerializeField]
	string size;

	[SerializeField, Range( 1, 10 )]
	float mySizeX;

	[SerializeField, Range( 1, 10 )]
	float mySizeY;

    // Start is called before the first frame update
    void Start()
    {
		myRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		changeCount += Time.deltaTime;

		if( changeCount >= 0.1 )
		{
			if( ++texValue == 11 )
			{
				texValue = 1;
			}

			myRenderer.sprite = doronAtlas.GetSprite( "f" + texValue.ToString() );

			changeCount = 0;
		}
    }
}
