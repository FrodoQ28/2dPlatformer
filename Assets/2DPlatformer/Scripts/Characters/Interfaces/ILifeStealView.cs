public interface ILifeStealView
{
    public void OnAbilityStarted(float duration, float radius);
    public void OnAbilityTick(float normalizedTime);
    public void OnAbilityEnded();

    public void OnCooldownStarted(float cooldown);
    public void OnCooldownTick(float normalizedTime);
    public void OnCooldownEnded();
}
