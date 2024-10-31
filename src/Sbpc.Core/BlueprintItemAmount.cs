namespace Sbpc.Core;

public class BlueprintItemAmount
{
    public BlueprintItemAmount(ObjectReference itemClass, int amount)
    {
        ItemClass = itemClass;
        Amount = amount;
    }

    public ObjectReference ItemClass { get; init; }
    public int Amount { get; set; }
}