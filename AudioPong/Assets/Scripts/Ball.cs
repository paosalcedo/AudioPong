using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public float speed = 30;
    private AudioDirector _audioDirector;
    private ScoreDirector _scoreDirectorP1;
    private ScoreDirector _scoreDirectorP2;
    private int scoreP1;
    private int scoreP2;

    void Start()
    {
        _audioDirector = GameObject.Find("AudioDirector").GetComponent<AudioDirector>();
        _scoreDirectorP1 = GameObject.Find("p1Score").GetComponent<ScoreDirector>();
        _scoreDirectorP2 = GameObject.Find("p2Score").GetComponent<ScoreDirector>();


        _scoreDirectorP1.CurrentScore = 0; //reset scores
        _scoreDirectorP2.CurrentScore = 0;

        StartCoroutine(Reset(Random.Range(0, 1))); //serve ball randomly
    }

    void Update()
    {
        _audioDirector.AdjustBallMoveSound(this.transform.position, this.GetComponent<Rigidbody2D>().velocity.magnitude); //changing the sound of the ball move loop based on ball's position
    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos,
                    float racketHeight)
    {
        // ||  1 <- top racket
        // ||
        // ||  0 <- middle racket
        // ||
        // || -1 <- bottom racket
        return (ballPos.y - racketPos.y) / racketHeight;
    }

    public IEnumerator Reset(int servingPlayer)
    {
        _audioDirector.StopAllSounds();

        transform.position = new Vector3(0, 0, 0);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        _audioDirector.PlayResetSound();
        yield return new WaitForSeconds(_audioDirector.resetSound.length);
        serveBall(servingPlayer);
        yield return null;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "RacketLeft")
        {

            float y = hitFactor(transform.position,
                                col.transform.position,
                                col.collider.bounds.size.y);

            Vector2 dir = new Vector2(1, y).normalized;

            Vector2 velo = col.gameObject.GetComponent<Rigidbody2D>().velocity;
            GetComponent<Rigidbody2D>().velocity = (dir * speed) + velo;
            Vector2 pointOfContact = col.contacts[0].point;
            _audioDirector.PlayRacketHitSound(pointOfContact, 0);
        }
        else if (col.gameObject.name == "RacketRight")
        {
            float y = hitFactor(transform.position,
                                col.transform.position,
                                col.collider.bounds.size.y);
            Vector2 dir = new Vector2(-1, y).normalized;
            Vector2 velo = col.gameObject.GetComponent<Rigidbody2D>().velocity;
            GetComponent<Rigidbody2D>().velocity = (dir * speed) + velo;
            Vector2 pointOfContact = col.contacts[0].point;
            _audioDirector.PlayRacketHitSound(pointOfContact, 1);
        }
        else if (col.gameObject.name == "WallLeft")
        {
            updateScore(1);
        }
        else if (col.gameObject.name == "WallRight")
        {
            updateScore(0);
        }
		
		if (col.gameObject.name == "WallTop" || col.gameObject.name == "WallBottom")
        {
            Vector2 pointOfContact = col.contacts[0].point;
            _audioDirector.PlayOuchSound(pointOfContact);
        }
    }

    void updateScore(int playerWhoScored)
    {
        switch (playerWhoScored)
        {
            case 0:
                scoreP1++;
                break;
            case 1:
                scoreP2++;
                break;
            default:
                Debug.LogError("Attempted to Increment Score for non existant player");
                break;
        }
        _scoreDirectorP1.CurrentScore = scoreP1;
        _scoreDirectorP2.CurrentScore = scoreP2;
        StartCoroutine(Reset(playerWhoScored));
    }

    void serveBall(int playerServing)
    {
        switch (playerServing)
        {
            case 0: GetComponent<Rigidbody2D>().velocity = new Vector2(1f, 0.5f) * speed; ; break;
            case 1: GetComponent<Rigidbody2D>().velocity = new Vector2(-1f, 0.5f) * speed; ; break;
            default: Debug.LogError("Invalid serving player!"); break;
        }
        _audioDirector.PlayBackgroundMusic();
        _audioDirector.PlayBallMoveSound(); //start ball moving sound
    }
}
