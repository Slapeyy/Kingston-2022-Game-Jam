using UnityEngine;

namespace GameJam.Krzysztof
{
    public class Test : MonoBehaviour
    {
        public GameObject speechBubble;
        public float knockFor;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                GameOverControler.Instance.GameOver(true);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                string text = "";
                int randomDialogue = Random.Range(0, 5);
                if (randomDialogue == 0)
                {
                    text = "Hippyty Hippety, your W is my property!";
                }
                else if (randomDialogue == 1) text = "Debate ME!";
                else if (randomDialogue == 2) text = "VERRRRRYYY LONG STRING!!!";
                else if (randomDialogue == 3) text = "Short answer";
                else if (randomDialogue == 4) text = "nOnononONo";
                else if (randomDialogue == 5) text = "Something is wrong with me keys...";
                Instantiate(speechBubble).GetComponent<DialogueControl>().Init(text, new Vector3(0.2f, 1.1f, 0f), gameObject);
            }
        }
    }
}