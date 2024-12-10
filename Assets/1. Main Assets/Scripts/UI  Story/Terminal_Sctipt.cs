using UnityEngine;

public class Terminal_Sctipt : MonoBehaviour
{
    public int terminalNum;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Storyline_Script.current.get_story_prompts(terminalNum - 1);
            Destroy(gameObject);
        }
    }
}
