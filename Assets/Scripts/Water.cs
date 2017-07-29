using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	public Color color;
	public int points = 100;

	private float width = 0.5f;
	private float height = 0.5f;

	private float offset;

	private Mesh mesh;
	private Vector3[] originalVertices, displacedVertices;

	public float waveHeight = 1f;

	// Use this for initialization
	void Awake () {

		offset = Random.value * 1000f;

		// Create Vector2 vertices

		List<Vector2> vertices2D = new List<Vector2> ();

		vertices2D.Add (new Vector2 (-width, -height));

		for (int i = 0; i < points + 1; i++) {
			vertices2D.Add (new Vector2 (-width + i * width / 50, height));
		}

		vertices2D.Add (new Vector2 (width, -height));

		Vector2[] vertArray = vertices2D.ToArray();

		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator(vertArray);
		int[] indices = tr.Triangulate();

		// Create the Vector3 vertices
		Vector3[] vertices = new Vector3[vertArray.Length];
		for (int i = 0; i < vertices.Length; i++) {
			vertices[i] = new Vector3(vertArray[i].x, vertArray[i].y, 0);
		}

		mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = indices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		GetComponent<MeshFilter>().mesh = mesh;

		originalVertices = mesh.vertices;

		MeshRenderer mr = GetComponent<MeshRenderer> ();
		mr.material.color = color;
	}

	void Update() {

		displacedVertices = new Vector3[originalVertices.Length];

		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices[i] = originalVertices[i] + Vector3.up * (float)Mathf.PerlinNoise (i/10f + offset, Time.time + offset) * waveHeight;
		}

		mesh.vertices = displacedVertices;
		mesh.RecalculateNormals();
	}
}
