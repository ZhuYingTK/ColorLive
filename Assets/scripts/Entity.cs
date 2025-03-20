
public enum eEntityType
{
    Cell,
    Building
}

public abstract class Entity 
{
    public virtual void OnUpdate(BoardTile node, int deltaTurn)
    {
        
    }
}
