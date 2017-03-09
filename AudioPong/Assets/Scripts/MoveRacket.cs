using UnityEngine;
using System.Collections;

public class MoveRacket : MonoBehaviour {

	public AudioSource creatureSource;
//	public AudioClip creatureClip;    
	public float speed = 30;
    public string axis = "Vertical";

	void Start(){
		creatureSource = GetComponent<AudioSource>();
	}

    void FixedUpdate ()
	{
		float v = Input.GetAxisRaw (axis);
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, v) * speed;

		if (Input.GetAxisRaw (axis) != 0 && !creatureSource.isPlaying) {
			creatureSource.Play();
		}
    }
}
