using UnityEngine;

public class Coin : Collectible
{
    [SerializeField] private int value = 1;

    protected override bool OnCollected(GameObject player)
    {
        GameManager.Instance.AddScore(value);
        AudioManager.Play(Sfx.Coin);
        return true;
    }
}