using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public CharacterMovement charMove;

    public void PlayerAttack()
    {
        charMove.DoAttack();
    }

    public void DamagePlayer()
    {
        transform.GetComponentInParent<EnemyController>().DamagePlayer();
    }

    public void MoveSound()
    {
        LevelManager.instance.PlaySound(LevelManager.instance.levelSounds[0], LevelManager.instance.Player.position);
    }
}

