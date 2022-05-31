namespace Services.Scene
{
    public interface ISceneService
    {
        void LoadLevel(string id, SceneService.LoadMode loadMode = SceneService.LoadMode.Unirx);
    }
}