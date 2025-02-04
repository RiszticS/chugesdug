using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private enum LadderPart { complete,bottom,top};
    [SerializeField] private LadderPart part = LadderPart.complete;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            switch (part)
            {
                case LadderPart.complete:
                    player.canclimb = true;
                    player.ladder = this;
                    break;
                case LadderPart.bottom:
                    player.bottomLadder = true;
                    break;
                case LadderPart.top:
                    player.topLadder = true;
                    break;
                default:
                    break;
            }
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            switch (part)
            {
                case LadderPart.complete:
                    player.canclimb = false;
                    player.climbing = false;
                    break;
                case LadderPart.bottom:
                    player.bottomLadder = false;
                    player.climbing = false;
                    break;
                case LadderPart.top:
                    player.topLadder = false;
                    player.climbing = false;
                    break;
                default:
                    break;
            }
        }
    }
}
