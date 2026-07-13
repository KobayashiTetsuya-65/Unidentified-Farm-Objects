using UnityEngine;

public class Monument : CharacterBase
{
    public override void PickUped()
    {
        base.PickUped();
        gameObject.SetActive(false);
    }
}
