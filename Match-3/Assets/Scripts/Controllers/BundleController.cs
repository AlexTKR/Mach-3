using Level;
using Settings;
using UnityEngine;
using UnityEngine.U2D;
using View;

namespace Controllers
{
    public interface IGetCellAtlas
    {
        SpriteAtlas GetCellAtlas();
    }

    public interface IGetCell
    {
        CellView GetCellPrefab();
    }

    public interface iGetGameSettings
    {
        GameSettings GetGameSettings();
    }

    public class BundleController: IGetLevel, IGetCellAtlas, IGetCell, iGetGameSettings
    {
        private IGetLevel _getLevel;
        private SpriteAtlas _spriteAtlas;
        private CellView _cellView;
        private GameSettings _gameSettings;
        
        public CellType[,] GetLevel(int number)
        {
            _getLevel ??= Resources.Load<LevelHolder>("Level/LevelHolder");

            return _getLevel.GetLevel(number);
        }

        public SpriteAtlas GetCellAtlas()
        {
            return  _spriteAtlas ??= Resources.Load<SpriteAtlas>("Atlases/CellAtlas");;
        }

        public CellView GetCellPrefab()
        {
            return _cellView ??= Resources.Load<CellView>("Cells/Cell");
        }

        public GameSettings GetGameSettings()
        {
            return _gameSettings??= Resources.Load<GameSettings>("Settings/GameSettings");
        }
    }
}
