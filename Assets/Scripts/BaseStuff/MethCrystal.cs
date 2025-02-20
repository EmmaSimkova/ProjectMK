using UnityEngine;

namespace BaseStuff
{
    public class MethCrystal : MonoBehaviour
    {
        [SerializeField] private int totalMethAmount;
        [SerializeField] private int currentMethAmount;
        [SerializeField] private int methPerHit;
        [SerializeField] private GameObject methShard;
        
        AudioManagerScript audioManagerScript;

        private void Awake()
        {
            audioManagerScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
        }
        
        //Start is called before the first frame update
        void Start()
        {
            currentMethAmount = totalMethAmount;
        }
        
        public void Collect()
        {
            currentMethAmount -= methPerHit;
            //create a meth shard for each in methPerHit
            for (int i = 0; i < methPerHit + (currentMethAmount < 0? currentMethAmount : 0); i++)
            {
                GameObject methShardInstance = Instantiate(methShard, transform.position, Quaternion.identity);
                //add velocity to the meth shard
                methShardInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            }
            if (currentMethAmount <= 0)
            {
                Destroy(this.gameObject);
                audioManagerScript.PlaySFX(audioManagerScript.crystalBreak);
            }
        }
    }
}
