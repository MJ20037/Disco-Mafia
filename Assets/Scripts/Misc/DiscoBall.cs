using UnityEngine;

public class DiscoBall : MonoBehaviour, IHitable
{
    private Flash _flash;
    private DiscoBallManager _discoBallManager;

    private void Awake()
    {
        _flash = GetComponent<Flash>();
        _discoBallManager = FindFirstObjectByType<DiscoBallManager>();
    }

    public void TakeHit()
    {
        _discoBallManager.DicoBallParty();
        _flash.StartFlash();
    }
}
