using SquareDino;
using UnityEngine;
public class Fighting : MonoBehaviour
{
    public RaycastHit Enemyhit;
    public void Attack()
    {
        if (Enemyhit.collider == null) return;
        MyVibration.Haptic(MyHapticTypes.LightImpact);
        SoundManager.Instance.PlaySound(SoundTypes.SwordAttack);
        Enemyhit.collider.GetComponent<UnitHealth>().TakeDamage();
    }
}
