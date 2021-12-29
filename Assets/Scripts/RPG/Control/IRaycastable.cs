namespace RPG.Control {
    public interface IRaycastable {
        bool HandleRaycast(PlayerController callerController);
        CursorType GetCursorType();
    }
}