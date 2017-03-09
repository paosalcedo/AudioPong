using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioDirector : MonoBehaviour {
   public AudioMixer Mixer;

   public AudioClip ballHitSound;
   public AudioSource ballHitSource;
   public AudioClip ballMoveLoop;
   public AudioSource ballMoveSource;
   public AudioClip resetSound;
   public AudioSource gameEventSounds;
   public AudioSource backgroundMusicSource;
	public AudioClip ouchSound;
	public AudioSource ouchSource;

   

   public float farLeftBallPosition = -25f;
   public float farRightBallPosition = 25f;
   public float topBallPosition = 16f;
   public float bottomBallPosition = -16f;

   public float ballMovePitchLow;
   public float ballMovePitchHigh;

   private AudioClip[] backgroundMusic;

   // Use this for initialization
   void Start () {
      ballMoveSource.clip = ballMoveLoop;
      backgroundMusic = (AudioClip[]) Resources.LoadAll<AudioClip>("Music");
	}

   public void StopAllSounds()
   {
      ballMoveSource.Stop();
      ballHitSource.Stop();
      gameEventSounds.Stop();
      backgroundMusicSource.Stop();
   }

   public void PlayBackgroundMusic()
   {
      backgroundMusicSource.clip = backgroundMusic[Random.Range(0, backgroundMusic.Length)];
      backgroundMusicSource.Play();
   }

   public void PlayResetSound() {
      gameEventSounds.clip = resetSound;
      gameEventSounds.Play();
   }

   public void PlayBallMoveSound()
   {
      ballMoveSource.Play();
   }

   public void AdjustBallMoveSound(Vector2 pos, float velo)
   {  
      ballMoveSource.panStereo = remapRange(pos.x, farLeftBallPosition, farRightBallPosition, -1, 1);
      ballMoveSource.pitch = remapRange(pos.y, bottomBallPosition, topBallPosition, ballMovePitchLow, ballMovePitchHigh);
      Mixer.SetFloat("BallFlange", (remapRange(velo, 0, 50, 0, 1)));
   }
   
   public void PlayBallHitSound(Vector2 pos)
   {
      float panValue = 0f;

      panValue = remapRange(pos.x, farLeftBallPosition, farRightBallPosition, -1, 1);
      ballHitSource.panStereo = panValue;
      ballHitSource.clip = ballHitSound;
      ballHitSource.Play();
      //Debug.Log("PanValue: " + panValue);

   }

    public void PlayRacketHitSound(Vector2 pos, int player)
    {
        ballHitSource.clip = ballHitSound;
        ballHitSource.Play();
    }

    float remapRange(float oldValue, float oldMin, float oldMax, float newMin, float newMax )
   {
      float newValue = 0;
      float oldRange = (oldMax - oldMin);
      float newRange = (newMax - newMin);
      newValue = (((oldValue - oldMin) * newRange) / oldRange) + newMin;
      return newValue;
   }
}
