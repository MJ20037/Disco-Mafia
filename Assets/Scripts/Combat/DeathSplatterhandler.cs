using UnityEngine;

public class DeathSplatterhandler : MonoBehaviour
{
    
    private void OnEnable()
    {
        Health.OnDeath += SpawnDeathSplatterPrefab;
        Health.OnDeath += SpawnDeathVFX;
    }

    private void OnDisable()
    {
        Health.OnDeath -= SpawnDeathSplatterPrefab;
        Health.OnDeath -= SpawnDeathVFX;
    }

    private void SpawnDeathSplatterPrefab(Health sender)
    {
        GameObject newSpaltterPrefab = Instantiate(sender.SplatterPrefab, sender.transform.position, transform.rotation);
        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();

        if (colorChanger)
        {
           newSpaltterPrefab.GetComponent<SpriteRenderer>().color = colorChanger.DefaultColor; 
        }
        
        newSpaltterPrefab.transform.SetParent(this.transform);
    }

    private void SpawnDeathVFX(Health sender)
    {
        GameObject deathVFX = Instantiate(sender.DeathVFX, sender.transform.position, transform.rotation);
        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();

        if (colorChanger)
        {
            ParticleSystem.MainModule ps = deathVFX.GetComponent<ParticleSystem>().main;
            ps.startColor = colorChanger.DefaultColor;
        }
        deathVFX.transform.SetParent(this.transform);       
    }
}
