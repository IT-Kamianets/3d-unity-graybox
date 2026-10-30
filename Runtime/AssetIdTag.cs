using UnityEngine;

namespace ITKamianets.Engine.Graybox
{
	/// <summary>
	/// The stable id of what an object stands for (<c>monsters/goblin</c>, <c>props/chest</c>), whatever shape
	/// draws it today. Real models later replace the primitive by this id, and gameplay stays as it is.
	/// </summary>
	public sealed class AssetIdTag : MonoBehaviour
	{
		public string Id;
	}
}
