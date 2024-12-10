using System.Collections;
using UnityEngine;
using TMPro;

public class Storyline_Script : MonoBehaviour
{
    public static Storyline_Script current;
    public TMP_Text terminalTMP;
    public Canvas terminal_Canvas;
    public int promptSpeed;

    private void Awake()
    {
        current = this;
    }

    private string[,] penis = {
        // Number 1
        {"** Crew Log : Entry 1 **",
        "| Maintenance Chief | : Captain, I've got important news.",
        "| Ship Captain | : Yes? What is it.",
        "| Maintenance Chief | : External sensors have discovered an unknown species of creatures attaching themselves on the outer hull.",
        "| Ship Captain | : Unknown creatures? You're telling me that we have aliens gathering on the outside of our ship?",
        "| Ship Captain | : Send a maintenance crew to find out what's going on with them, I want to know what they're doing out there.",
        null
        },
        // Number 2
        {"** Crew Log : Entry 2 **",
        "| Maintenance Worker 1 | : It's not time for routine inspections yet, do you know what's going on?",
        "| Maintenance Worker 2 | : The chief said there are creatures on the outer hull, he wants us to go out and deal with it.",
        "| Maintenance Worker 1 | : Creatures!? are you serious? like, aliens? I don't think i want to go out there man...",
        "| Maintenance Worker 2 | : C'mon man, you think i want to either? lets just get this over with. Chief said they've stopped moving once they attach, maybe they're already dead?",
        "* Both Crew members exit through the airlock. *",
        null
        },
        // Number 3
        {"** Crew Log : Entry 3 **",
        "* Maintenance Worker 1 Re-Enters Airlock *",
        "| Maintenance Worker 1 | : Holy shit! Oh my god! *heavy breathing*",
        "...",
        "| Maintenance Worker 1 | : No... I'm sorry... ",
        "...",
        "| Maintenance Worker 1 | : I've got to get to the chief, ASAP!",
        },
        // Number 4
        {"** Crew Log : Entry 4 **",
        "| Maintenance worker 1 | : CHIEF!!",
        "| Maintenance Chief | : Yes, what is it? how was the pa-",
        "| Maintenance worker 1 | : WERE ALL DEAD MAN, WERE ALL GONNA DIE... HE.. HE'S GONE..",
        "| Maintenance Chief | : HEY! Calm down! And keep it down! What are you talking about? where's-",
        "| Maintenance Worker 1 | : The aliens! They're tearing up the haul! He tried to stop one but.. but..",
        "| Maintenance Chief | : I see... I'm sorry. I will speak with the captain.",
        },
        // Number 5
        {"** Crew Log : Entry 5 **",
        "| Maintenance Chief | : Hey, Captain, Important news...",
        "| Ship Captain | : Not now, ill be attending a meeting shortly, ill meet back up-",
        "| Maintenance Chief | : Sorry to interrupt but if we don't act now our ship wont be here much longer.",
        "| Ship Captain | : Wait, what? What happened out there?",
        "| Maintenance Chief | : We've lost one of our maintenance crew. these aliens are hostile, and they're currently ripping off the outer shell of the hull.",
        "| Ship Captain | : Understood, ill get in touch with the defense department right away and see what we can do. thanks.",
        },
        // Number 6
        {"** Crew Log : Entry 6 **",
        "| Ship Captain | : What do you mean it wont work? What's the point of having drones if they cant protect the ship?",
        "| Chief of Defense | : Sir, we cannot fire at our own ship, its suicide. We're going to have to find another way.",
        "| Ship Captain | : But there really aren't any other options. Either way were going down, we should at least try for the sake of the mission.",
        "| Chief of Defense | : I cannot send these drones and willingly put everyone's life in danger. We need to redirect our course immediately and deal with the remainders once we've gotten out of their territory.",
        "| Ship Captain | : This is insane... Why the hell were there no precautions for an event like this. A mission on this scale cannot fail.",
        null
        },
        // Number 7
        {"** Crew Log : Entry 7 **",
        "| Chief of Personnel | : So what are we supposed to tell the people?",
        "| Ship Captain | : We won't say anything. It won't do any good. Were just going to have to wait it out.",
        "| Chief of personnel | : Are you serious? They deserve to know... It's just not right.",
        "| Ship Captain | : You wont say a damn thing you hear me? goodbye.",
        null,
        null
        },
        // Number 8
        {"** Crew Log : Entry 8 **",
        "** WARNING!! WARNING!! WARNING!! **",
        "** PUNCTURE IN SHIP HULL SUIT UP IMMEDIATELY **",
        "** PLEASE MAKE YOUR WAY TO THE ESCAPE PODS **",
        "** WARNING!! WARNING!! WARNING!! **",
        "KABLAMO E*everyone dies*",
        null
        }
    };

    public void get_story_prompts(int terminalNum)
    {
        StartCoroutine(enum_story_prompts(terminalNum));
    }

    private IEnumerator enum_story_prompts(int terminalNum)
    {
        terminal_Canvas.gameObject.SetActive(true);
        for (int message = 0; message < 7; message++)
        {
            if (penis[terminalNum, message] != null)
            {
                terminalTMP.SetText($"{terminalTMP.text + penis[terminalNum, message] + "\n\n"}");
                yield return new WaitForSecondsRealtime(promptSpeed);
            }
        }
        yield return new WaitForSecondsRealtime(promptSpeed*4f);
        terminal_Canvas.gameObject.SetActive(false);
    }
}
