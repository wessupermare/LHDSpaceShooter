using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public RectTransform HealthBar;

    public const int MaxHealth = 100;
    public int CurrentHealth = MaxHealth;

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Debug.Log("Dead!");
        }

        HealthBar.sizeDelta = new Vector2(CurrentHealth, HealthBar.sizeDelta.y);
    }
}