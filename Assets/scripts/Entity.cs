
public enum eEntityType
{
    Cell,
    Building
}

public abstract class Entity 
{
    public virtual void Update(BoardTile node, int deltaTurn)
    {
        
    }
}
