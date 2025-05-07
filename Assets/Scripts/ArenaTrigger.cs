using UnityEngine;

public class BossArenaTrigger : MonoBehaviour
{
   public BossController[] bosses;
    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            foreach (var boss in bosses)
            {
                if (boss != null)
                    boss.Engage();
            }
        }
    }
}

