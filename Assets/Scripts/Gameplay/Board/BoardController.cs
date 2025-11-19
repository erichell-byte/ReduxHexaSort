using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Installers;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Towers;
using Hexa.Domain.Board;
using Hexa.Domain.Player;
using Hexa.Domain.Rules;
using Hexa.Domain.State;
using Presentation.UI;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Board
{
    public class BoardController : MonoBehaviour, IBoardBoosterContext
    {
        [SerializeField] private GameObject _cellBackgroundPrefab;
        [SerializeField] private BoardTower _boardTowerPrefab;
        [SerializeField] private PlayerTower _playerTowerPrefab;
        [SerializeField] private Transform _boardParent;
        [SerializeField] private Transform _playerTowersParent;
        [SerializeField] private Vector3[] _playerTowerSlots = { };
        
        private IGameplayRuleEvaluator _ruleEvaluator;

        private IBoardGeneratorService _boardGenerator;
        private ITowerPlacementService _towerPlacement;
        private IChainReactionService _chainReaction;
        private IScoreTracker _scoreTracker;
        private IUndoHistoryService _undoHistory;
        private IPlayerDeckService _playerDeckService;
        private PlayerTowerFactory _playerTowerFactory;
        private BoosterPresenter _boosterPresenter;
        private LevelConfig _levelConfig;
        
        private IHexGrid _grid;
        private List<PlayerTower> _playerTowerViews = new();
        private Dictionary<Vector2Int, BoardTower> _boardTowers = new();
        private bool _isShuttingDown;
        public bool CanUndo => _undoHistory.CanUndo;

        [Inject]
        private void Construct(
            IBoardGeneratorService boardGenerator,
            ITowerPlacementService towerPlacement,
            IChainReactionService chainReaction,
            IPlayerDeckService playerDeckService,
            PlayerTowerFactory playerTowerFactory,
            LevelConfig levelConfig,
            IScoreTracker scoreTracker,
            IUndoHistoryService undoHistory,
            IGameplayRuleEvaluator ruleEvaluator,
            BoosterPresenter boosterPresenter)
        {
            _boardGenerator = boardGenerator;
            _towerPlacement = towerPlacement;
            _chainReaction = chainReaction;
            _playerDeckService = playerDeckService;
            _playerTowerFactory = playerTowerFactory;
            _levelConfig = levelConfig;
            _scoreTracker = scoreTracker;
            _undoHistory = undoHistory;
            _ruleEvaluator = ruleEvaluator;
            _boosterPresenter = boosterPresenter;
            
            _chainReaction.OnScoreAdded += OnScoreAdded;
        }

        private void OnEnable()
        {
            _isShuttingDown = false;
        }

        private async void Start()
        {
            await InitializeBoardAsync();
            await GeneratePlayerTowersAsync();
        }
        private void OnDestroy()
        {
            _isShuttingDown = true;

            if (_chainReaction != null)
                _chainReaction.OnScoreAdded -= OnScoreAdded;
        }
        
        
        private void OnScoreAdded(int score)
        {
            _scoreTracker.AddScore(score);
            _ruleEvaluator.EvaluateScore(_scoreTracker.Score, _scoreTracker.TargetScore);
        }
        
        private void CheckForGameOver()
        {
            _ruleEvaluator.EvaluateBoard(_grid);
        }

        private async UniTask InitializeBoardAsync()
        {
            if (_isShuttingDown) return;

            _grid = _boardGenerator.GenerateBoard(_levelConfig);
            _undoHistory.Clear();
            _boosterPresenter?.SetUndoAvailability(false);
            _boardTowers = new Dictionary<Vector2Int, BoardTower>();
    
            foreach (var pos in _grid.Cells.Keys)
            {
                if (_isShuttingDown) return;

                CreateCellBackground(pos);
    
                var boardTower = Instantiate(_boardTowerPrefab, _boardParent);
                boardTower.Initialize(_grid.GetCell(pos), _levelConfig.ColorPalette);
                boardTower.BindSelection(OnBoardTowerSelected);
                _boardTowers[pos] = boardTower;
            }
    
            _towerPlacement.Initialize(_grid, _boardTowers);
            _chainReaction.Initialize(_grid, _boardTowers);
            
            await UniTask.CompletedTask;
        }
        
        private void CreateCellBackground(Vector2Int gridPosition)
        {
            if (_cellBackgroundPrefab != null)
            {
                Vector3 position = _boardGenerator.CalculateWorldPosition(gridPosition);
                var background = Instantiate(_cellBackgroundPrefab, _boardParent);
                background.transform.position = position;
            
                var renderer = background.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.gray;
                }
            }
        }
        
        private async UniTask AnimateNewTowersAppearanceAsync()
        {
            if (_isShuttingDown) return;
            await UniTask.NextFrame();
            if (_isShuttingDown) return;
        
            foreach (var tower in _playerTowerViews)
            {
                if (tower == null) continue;

                tower.transform.localScale = Vector3.zero;
                tower.transform.DOScale(Vector3.one, 0.3f)
                    .SetEase(Ease.OutBack);
                
                if (_isShuttingDown) return;
                await UniTask.Delay(100);
            }
        }
        
        public async void TryPlacePlayerTower(PlayerTower playerTower, IHexCell towerModel, Vector3 dropPosition)
        {
            var closestCell = _towerPlacement.FindClosestEmptyCell(dropPosition);
            if (closestCell != null)
            {
                await PlaceTowerOnCell(playerTower, towerModel, closestCell);
            }
            else
            {
                await playerTower.ReturnToStartPosition();
            }
        }

        private async Task PlaceTowerOnCell(PlayerTower playerTower, IHexCell playerTowerModel, BoardTower targetTower)
        {
            var cellPosition = _towerPlacement.FindCellPositionByTower(targetTower);

            if (cellPosition != null && 
                _grid.Contains(cellPosition.Value) && 
                _grid.GetCell(cellPosition.Value).IsEmpty)
            {
                bool snapshotSaved = TrySaveSnapshot();
                await playerTower.AnimateToPosition(targetTower.transform.position);
        
                if (_towerPlacement.TryPlaceTower(playerTower.transform.position, playerTowerModel, out var placedPosition))
                {
                    targetTower.RefreshVisual();
                    _playerTowerViews.Remove(playerTower);
                    Destroy(playerTower.gameObject);
        
                    await _chainReaction.CheckChains(placedPosition.Value);
                    CheckIfNeedNewTowers();
                    CheckForGameOver();
                }
                else
                {
                    if (snapshotSaved && _undoHistory.CanUndo)
                    {
                        _undoHistory.Pop();
                        _boosterPresenter?.SetUndoAvailability(_undoHistory.CanUndo);
                    }
                    await playerTower.ReturnToStartPosition();
                }
            }
            else
            {
                await playerTower.ReturnToStartPosition();
            }
        }
        
        private async void CheckIfNeedNewTowers()
        {
            if (_playerTowerViews.Count == 0)
            {
                await UniTask.Delay(500);
                if (_isShuttingDown) return;

                if (_playerTowerViews.Count == 0)
                {
                    await GeneratePlayerTowersAsync();
                }
            }
        }
        
        private async UniTask GeneratePlayerTowersAsync()
        {
            if (_isShuttingDown) return;

            _playerTowerViews.Clear();
    
            foreach (Transform child in _playerTowersParent)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < 3; i++)
            {
                var cell = _playerDeckService.CreateRandomTower();
        
                var towerView = _playerTowerFactory.Create();
                towerView.transform.SetParent(_playerTowersParent);
                towerView.Initialize(cell, _playerTowerSlots[i], _levelConfig.ColorPalette, i);
                _playerTowerViews.Add(towerView);
            }
    
            await AnimateNewTowersAppearanceAsync();
        }
        
        public bool TryDestroyTower(BoardTower targetTower)
        {
            if (targetTower == null) return false;
            
            var cellPosition = _towerPlacement.FindCellPositionByTower(targetTower);
            if (!cellPosition.HasValue) return false;

            return TryDestroyTower(cellPosition.Value);
        }

        public bool TryDestroyTower(Vector2Int gridPosition)
        {
            if (!_grid.TryGetCell(gridPosition, out var cell) || cell.IsEmpty) return false;
            if (!_boardTowers.TryGetValue(gridPosition, out var tower)) return false;

            TrySaveSnapshot();

            cell.Clear();
            tower.RefreshVisual();
            CheckForGameOver();
            return true;
        }

        public bool TryUndoLastAction()
        {
            var snapshot = _undoHistory.Pop();
            if (snapshot == null) return false;

            RestoreSnapshot(snapshot);
            _boosterPresenter?.SetUndoAvailability(_undoHistory.CanUndo);
            return true;
        }

        public bool TryShuffleBoard()
        {
            if (_grid == null || _grid.Cells.Count == 0) return false;

            var heights = new Dictionary<Vector2Int, int>();
            var allPieces = new List<int>();

            foreach (var kvp in _grid.Cells)
            {
                var cell = kvp.Value;
                heights[kvp.Key] = cell.Stack.Count;
                allPieces.AddRange(cell.Stack.Colors);
            }

            if (allPieces.Count <= 1) return false;

            TrySaveSnapshot();
            ShuffleList(allPieces);

            int pieceIndex = 0;
            foreach (var kvp in _grid.Cells)
            {
                var cell = kvp.Value;
                cell.Clear();

                int height = heights[kvp.Key];
                for (int i = 0; i < height; i++)
                {
                    cell.AddColor(allPieces[pieceIndex++]);
                }

                if (_boardTowers.TryGetValue(kvp.Key, out var tower))
                {
                    tower.RefreshVisual();
                }
            }

            CheckForGameOver();
            return true;
        }

        private bool TrySaveSnapshot()
        {
            if (_grid == null || _grid.Cells.Count == 0) return false;

            var snapshot = CaptureSnapshot();
            _undoHistory.Push(snapshot);
            _boosterPresenter?.SetUndoAvailability(true);
            return true;
        }

        private BoardSnapshot CaptureSnapshot()
        {
            var boardState = new Dictionary<Vector2Int, List<int>>();

            foreach (var kvp in _grid.Cells)
            {
                boardState[kvp.Key] = new List<int>(kvp.Value.Stack.Colors);
            }

            var towerSnapshots = new List<PlayerTowerSnapshot>();

            foreach (var tower in _playerTowerViews)
            {
                if (tower == null || tower.CellModel == null) continue;

                towerSnapshots.Add(new PlayerTowerSnapshot(
                    new List<int>(tower.CellModel.Stack.Colors),
                    tower.SlotIndex,
                    tower.StartPosition));
            }

            return new BoardSnapshot(boardState, towerSnapshots, _scoreTracker.Score);
        }

        private void RestoreSnapshot(BoardSnapshot snapshot)
        {
            foreach (var kvp in snapshot.BoardState)
            {
                if (_grid.TryGetCell(kvp.Key, out var cell))
                {
                    cell.Clear();
                    foreach (var color in kvp.Value)
                    {
                        cell.AddColor(color);
                    }

                    if (_boardTowers.TryGetValue(kvp.Key, out var tower))
                    {
                        tower.RefreshVisual();
                    }
                }
            }

            RestorePlayerTowers(snapshot.PlayerTowers);
            RestoreGameplayValues(snapshot.Score);
            CheckForGameOver();
        }

        private void RestorePlayerTowers(List<PlayerTowerSnapshot> towerSnapshots)
        {
            foreach (var tower in _playerTowerViews)
            {
                if (tower != null)
                {
                    Destroy(tower.gameObject);
                }
            }

            _playerTowerViews.Clear();

            if (towerSnapshots == null) return;

            foreach (var snapshot in towerSnapshots)
            {
                var cell = new HexCell(new Vector2Int(-1, -1));
                foreach (var color in snapshot.Colors)
                {
                    cell.AddColor(color);
                }

                var towerView = _playerTowerFactory.Create();
                towerView.transform.SetParent(_playerTowersParent);
                var slotPosition = GetSlotPosition(snapshot);
                towerView.Initialize(cell, slotPosition, _levelConfig.ColorPalette, snapshot.SlotIndex);
                _playerTowerViews.Add(towerView);
            }
        }

        private Vector3 GetSlotPosition(PlayerTowerSnapshot snapshot)
        {
            if (snapshot.SlotIndex >= 0 && snapshot.SlotIndex < _playerTowerSlots.Length)
            {
                return _playerTowerSlots[snapshot.SlotIndex];
            }

            return snapshot.Position;
        }

        private void OnBoardTowerSelected(BoardTower tower)
        {
            _boosterPresenter?.HandleBoardTowerSelection(tower);
        }

        private void RestoreGameplayValues(int snapshotScore)
        {
            if (_scoreTracker.Score == snapshotScore) return;
            _scoreTracker.ResetScore();
            _scoreTracker.AddScore(snapshotScore);
            _ruleEvaluator.EvaluateScore(_scoreTracker.Score, _scoreTracker.TargetScore);
        }

        private void ShuffleList<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
