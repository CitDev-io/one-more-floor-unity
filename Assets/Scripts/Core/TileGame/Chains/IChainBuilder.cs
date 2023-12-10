public interface IChainBuilder {
    void StartChain(Tile tile);
    void EndChain();
    void IndicateTile(Tile tile);
}