using System;

public interface ILootable
{
    public event Action<ILootable> LootTaked;

    public void Taked();
}