using System.Collections.Generic;

public interface IEventInvoker {
    public void Invoke(TileGameEvent e);

    public void Invoke(List<TileGameEvent> events);
}