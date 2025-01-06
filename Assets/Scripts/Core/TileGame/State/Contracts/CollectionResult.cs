using System;

public class CollectionResult {
    // WILO: This Should Have a Commit Action that has the action transformation.
    // The UI can peek and grab all of the collection results from the stack
    // and act them out async if it wants, but this is how we report it.
    // FUTURE: headless, we'd need an auto-commit layer.

    public Action Commit;
    public int Collected;
    public int GameTotalCollected;
    public int Earned;
    public int BonusRollCount;
    public int SuccessfulBonusRollCount;
    public int BonusGained;
}