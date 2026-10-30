using UnityEditor;
using UnityEngine;

namespace ITKamianets.Engine.Graybox.EditorTools
{
	/// <summary>
	/// The few helpers every scene builder wants: a primitive in a given place and size, a box that keeps its
	/// collider, and a plain coloured URP material saved as an asset (made once, updated on the next run).
	/// </summary>
	public static class GrayboxBuilder
	{
		public const string DefaultMaterialDirectory = "Assets/Materials";

		/// <summary>
		/// A primitive under <paramref name="parent"/> at a local position and scale. It has no collider unless
		/// <paramref name="keepCollider"/>, so decoration never gets in the way.
		/// </summary>
		public static GameObject Prim(PrimitiveType type, string name, Transform parent, Vector3 position, Vector3 scale, Material material, bool keepCollider = false)
		{
			var primitive = GameObject.CreatePrimitive(type);
			primitive.name = name;
			primitive.transform.SetParent(parent, false);
			primitive.transform.localPosition = position;
			primitive.transform.localScale = scale;
			primitive.GetComponent<Renderer>().sharedMaterial = material;
			if (!keepCollider) Object.DestroyImmediate(primitive.GetComponent<Collider>());
			return primitive;
		}

		/// <summary>A cube that keeps its collider: floors, walls, anything to stand on or bump into.</summary>
		public static GameObject Box(Transform parent, string name, Vector3 position, Vector3 scale, Material material)
		{
			return Prim(PrimitiveType.Cube, name, parent, position, scale, material, true);
		}

		/// <summary>
		/// A plain URP Lit material of one colour, saved as <c>&lt;directory&gt;/&lt;name&gt;.mat</c>. Running again
		/// with the same name updates the colour of the same asset.
		/// </summary>
		public static Material Mat(string name, Color colour, string directory = DefaultMaterialDirectory, float smoothness = 0.12f)
		{
			if (!AssetDatabase.IsValidFolder(directory))
			{
				var parent = System.IO.Path.GetDirectoryName(directory).Replace('\\', '/');
				AssetDatabase.CreateFolder(parent, System.IO.Path.GetFileName(directory));
			}
			var path = directory + "/" + name + ".mat";
			var material = AssetDatabase.LoadAssetAtPath<Material>(path);
			if (material == null)
			{
				material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
				AssetDatabase.CreateAsset(material, path);
			}
			material.SetColor("_BaseColor", colour);
			material.color = colour;
			material.SetFloat("_Smoothness", smoothness);
			material.SetFloat("_Metallic", 0f);
			EditorUtility.SetDirty(material);
			return material;
		}

		static Material _worldText;

		/// <summary>
		/// The material world text (TextMesh) is drawn with: the built-in font shader ignores depth, so text shows through walls;
		/// <c>Temnorid/WorldText</c> (in this package) is depth-tested. Saved as <c>&lt;directory&gt;/WorldText.mat</c>.
		/// </summary>
		public static Material WorldTextMaterial(Font font, string directory = DefaultMaterialDirectory)
		{
			if (_worldText == null)
			{
				var path = directory + "/WorldText.mat";
				_worldText = AssetDatabase.LoadAssetAtPath<Material>(path);
				if (_worldText == null)
				{
					System.IO.Directory.CreateDirectory(directory);
					_worldText = new Material(font.material);
					AssetDatabase.CreateAsset(_worldText, path);
				}
				_worldText.shader = Shader.Find("Temnorid/WorldText");
				_worldText.mainTexture = font.material.mainTexture;
				EditorUtility.SetDirty(_worldText);
			}
			return _worldText;
		}

		/// <summary>
		/// A line of text in the world (a sign, a plaque). Its height is about size x fontSize / 10 metres, and it is readable from its own
		/// -z side. <paramref name="localPosition"/> and <paramref name="rotation"/> are local to <paramref name="parent"/>.
		/// </summary>
		public static TextMesh Text(Transform parent, string name, string text, Vector3 localPosition, Quaternion rotation, float size, Color colour, string materialDirectory = DefaultMaterialDirectory)
		{
			var go = new GameObject(name);
			go.transform.SetParent(parent, false);
			go.transform.localPosition = localPosition;
			go.transform.localRotation = rotation;
			var mesh = go.AddComponent<TextMesh>();
			mesh.text = text;
			mesh.characterSize = size;
			mesh.fontSize = 64;
			mesh.anchor = TextAnchor.MiddleCenter;
			mesh.alignment = TextAlignment.Center;
			mesh.color = colour;
			go.GetComponent<MeshRenderer>().sharedMaterial = WorldTextMaterial(mesh.font, materialDirectory);
			return mesh;
		}
	}
}
