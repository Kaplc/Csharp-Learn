namespace Greedy_Snake.game
{
    public interface I_UpdateGameImage
    { 
        void UpdateGameImage(int w, int y, ref E_SceneType currSceneType);
    }

    public interface I_DrawObject
    {
        void Draw();
    }
}