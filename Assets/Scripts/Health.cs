using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    public RectTransform HealthBar;

    public const int MaxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int CurrentHealth = MaxHealth;

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Debug.Log("Dead!");
        }
    }

    void OnChangeHealth(int currentHealth)
    {
        HealthBar.sizeDelta = new Vector2(currentHealth, HealthBar.sizeDelta.y);
    }
}