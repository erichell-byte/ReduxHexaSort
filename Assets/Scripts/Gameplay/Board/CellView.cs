using UnityEngine;

namespace Gameplay.Board
{
	public class CellView : MonoBehaviour
	{
		[SerializeField] private MeshRenderer[] _renderers;
		
		public MeshRenderer[] Renderers => _renderers;
	}
}