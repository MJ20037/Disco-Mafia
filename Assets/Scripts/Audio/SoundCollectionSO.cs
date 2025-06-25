using UnityEngine;

[CreateAssetMenu()]
public class SoundCollectionSO : ScriptableObject
{
    [Header("Music")]
    public SoundSO[] FightMusic;
    public SoundSO[] DiscoBallMusic;

    [Header("SFX")]
    public SoundSO[] GunShoot;
    public SoundSO[] Jump;
    public SoundSO[] Splat;
    public SoundSO[] Jetpack;
    public SoundSO[] GrenadeBeep;
    public SoundSO[] GrenadeShoot;
    public SoundSO[] GrenadeExplode;
    public SoundSO[] PlayerHit;
    public SoundSO[] MegaKill;
}
