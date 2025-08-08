using System.Collections;
using UnityEngine;

public class MultistageJump_Skill : Skill
{
    public int jumpCounts = 1;
    public int jumpCounter;
    protected override void Start()
    {
        base.Start();
        player.ResetMultistageJumpCounter += ResetJumpCounter;
        jumpCounter = 0;
    }
    public override void UseSkill()
    {
        if (jumpCounter >= jumpCounts)
        {
            return;
        }
        base.UseSkill();
        player.SetVelocity(player.rb.velocity.x, player.jumpForce);
        AudioManager.instance.PlaySFX(17);
        jumpCounter++;
    }

    public void ResetJumpCounter()
    {
        jumpCounter = 0;
    }
    private void OnDestroy()
    {
        player.ResetMultistageJumpCounter -= ResetJumpCounter;
    }
}
