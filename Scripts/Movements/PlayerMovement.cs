using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Tooltip("Vitesse de deplacement")]
	public float vitesse = 1;
	[Tooltip("Les axes X, Y et Z du vecteur de deplacement.")]
	public float X = 1;
	public float Y = 0;
	public float Z = 1;
	private Vector3 deplacement;

	private void Start()
	{
		InitVector();
	}

	private void InitVector()
	{
		X += this.transform.position.x;
		Y += this.transform.position.y;
		Z += this.transform.position.z;
		deplacement = new Vector3(X, Y, Z);
	}
	void Update()
	{
		deplacement = deplacement - this.transform.position;
		deplacement = deplacement.normalized * vitesse * Time.deltaTime;
		this.transform.position += deplacement;
		deplacement += this.transform.position;
	}
}
