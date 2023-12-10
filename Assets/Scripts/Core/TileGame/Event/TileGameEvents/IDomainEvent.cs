using System.Collections.Generic;

public interface IDomainEvent {
    public List<TileGameEvent> Visit(GameState currentState);
}