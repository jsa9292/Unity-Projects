using UnityEngine;
using System.Collections;

public class MeshTrailDrawer : MonoBehaviour {
	
	public TrailShape shape;
	public int maxVertexNum = 300;  //for dynamic batching
	public float intervalDistance = 0.5f;
	private int vertexNum;
	private Transform myTransform;
	private Vector3 lastPos;
	
	private Vector3[] shapeVertexs;
	private int shapeVertexNum;
	
	private Vector3[] vertices;
	private Vector2[] uvs;
	// private Color[] colors;
	private int[] triangles;
	
	private int vertIndex;
	private int triIndex;
	
	private int p0;
	private int p1;
	
	private Vector3[] lastShapeVectors;
	private Vector3[] thisShapeVectors;
	
	public Material material;
	private Mesh mesh;
	
	private GameObject subMesh;
	private Transform createdMesh;
	private int count;
	
	private int lastShapeVertexNum;
	private Vector3 initPos;
	private ColorFade meshFade;
	public bool isFade = false;
	public float protectTime = 0.02f;
	public float fadeTime = 1f;
	public bool isCap = false;
	public bool autoStart = true;
	public bool isGenerateCollider = false;

	void Awake ()
	{
		myTransform = transform;
		lastPos = myTransform.position;
		initPos = myTransform.position;
	}
	
	void OnDrawGizmos ()
	{
		//maxVertexNum must greater than pathPoints's length
		if (shape!=null)
		{
			if (maxVertexNum<shape.pathPoints.Length)
			{
				maxVertexNum = shape.pathPoints.Length;
			}
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		if (!autoStart) return;

		shapeVertexNum = shape.pathPoints.Length;
		shapeVertexs = new Vector3[shapeVertexNum];
		lastShapeVertexNum = shapeVertexNum;
		
//		Debug.Log("shapeVertexNum = "+shapeVertexNum);
		
		for (int i=0;i<shapeVertexNum;i++)
		{
//			shapeVertexs[i] = shape.transform.InverseTransformPoint(shape.smoothPoints[i]);
			shapeVertexs[i] = shape.pathPoints[i];
		}
		
		vertexNum = Mathf.FloorToInt((float)maxVertexNum/(float)shapeVertexNum) * shapeVertexNum;
		vertices = new Vector3[vertexNum];
		uvs = new Vector2[vertexNum];
		// colors = new Color[vertexNum];
		triangles = new int[vertexNum/2*3];
//		Debug.Log("vertexNum = "+vertexNum);
		//Last Shape Vectors
		lastShapeVectors = new Vector3[shapeVertexNum];
		for (int i=0;i<shapeVertexNum;i++)
		{
			lastShapeVectors[i] = myTransform.TransformPoint(shapeVertexs[i]);
		}
		//This Shape Vectors
		thisShapeVectors = new Vector3[shapeVertexNum];
		//Mesh Init
		createdMesh = (new GameObject("createdMesh")).transform;
		createdMesh.position = initPos;
		subMesh  = new GameObject("subMesh"+count);
		subMesh.AddComponent<MeshFilter>();
		subMesh.AddComponent<MeshRenderer>();
		if (isFade){
			ColorFade cf = subMesh.AddComponent<ColorFade>();
			cf.protectTime = protectTime;
			cf.fadeTime = fadeTime;
		}
		subMesh.GetComponent<Renderer>().sharedMaterial = material;
		mesh = subMesh.GetComponent<MeshFilter>().mesh;
		subMesh.transform.parent = createdMesh;
		subMesh.transform.localPosition = Vector3.zero;
		count++;
		//Cap
		if (isCap)
        {
			StartCap();
		}
	}
	public void StartDrawing()
    {
		autoStart = true;
		Start();
	}

	public void EndDrawing()
	{
		autoStart = false;
		//Cap
		if (isCap)
		{
			EndCap();
		}
	}

	private void StartCap()
    {
		//Create Movement Mesh
		CreateNewMesh();
		//Center Point
		Vector3 center = Vector3.zero;
		for (int i = 0; i < lastShapeVectors.Length; i++)
		{
			center += lastShapeVectors[i];
		}
		center = center / shapeVertexNum;
		//Make Cap Plane
		for (int i = 0; i < lastShapeVectors.Length - 1; i++)
		{
			//Vertices
			vertices[vertIndex] = createdMesh.InverseTransformPoint(center);
			vertices[vertIndex + 1] = createdMesh.InverseTransformPoint(lastShapeVectors[i]);
			vertices[vertIndex + 2] = createdMesh.InverseTransformPoint(lastShapeVectors[i + 1]);
			//Set UVs
			uvs[vertIndex] = new Vector2(0, 0);
			uvs[vertIndex + 1] = new Vector2(0, 0);
			uvs[vertIndex + 2] = new Vector2(0, 0);
			//Set Triangles
			triangles[triIndex] = vertIndex;
			triangles[triIndex + 1] = vertIndex + 1;
			triangles[triIndex + 2] = vertIndex + 2;
			//Index++
			vertIndex = vertIndex + 3;
			triIndex = triIndex + 3;
		}
		//Create Cap Mesh
		CreateNewMesh();
	}

	private void EndCap()
	{
		//Create Movement Mesh
		CreateNewMesh();
		//Center Point
		Vector3 center = Vector3.zero;
		for (int i = 0; i < lastShapeVectors.Length; i++)
		{
			center += lastShapeVectors[i];
		}
		center = center / shapeVertexNum;
		//Make Cap Plane
		for (int i = 0; i < lastShapeVectors.Length - 1; i++)
		{
			//Vertices
			vertices[vertIndex] = createdMesh.InverseTransformPoint(center);
			vertices[vertIndex + 1] = createdMesh.InverseTransformPoint(lastShapeVectors[i+1]);
			vertices[vertIndex + 2] = createdMesh.InverseTransformPoint(lastShapeVectors[i]);
			//Set UVs
			uvs[vertIndex] = new Vector2(0, 0);
			uvs[vertIndex + 1] = new Vector2(0, 0);
			uvs[vertIndex + 2] = new Vector2(0, 0);
			//Set Triangles
			triangles[triIndex] = vertIndex;
			triangles[triIndex + 1] = vertIndex + 1;
			triangles[triIndex + 2] = vertIndex + 2;
			//Index++
			vertIndex = vertIndex + 3;
			triIndex = triIndex + 3;
		}
		//Create Cap Mesh
		CreateNewMesh();
	}

	public void RefreshShapeVectors()
	{
		//Refresh shape Vertexs
		shapeVertexNum = shape.pathPoints.Length;
		shapeVertexs = new Vector3[shapeVertexNum];
		for (int i=0;i<shapeVertexNum;i++)
		{
//			shapeVertexs[i] = shape.transform.InverseTransformPoint(shape.smoothPoints[i]);
			shapeVertexs[i] = shape.pathPoints[i];
		}
		//Last Shape Vectors
		lastShapeVectors = new Vector3[shapeVertexNum];
		for (int i=0;i<shapeVertexNum;i++)
		{
			lastShapeVectors[i] = myTransform.TransformPoint(shapeVertexs[i]);
		}
		//This Shape Vectors
		thisShapeVectors = new Vector3[shapeVertexNum];
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!autoStart) return;

		//Refresh shape Vertex's count
		if (lastShapeVertexNum!=shapeVertexNum)
		{
			RefreshShapeVectors();
			CreateNewMesh();
			lastShapeVertexNum = shapeVertexNum;
		}
		//Refresh shape Vertex's position
		shapeVertexNum = shape.pathPoints.Length;
		shapeVertexs = new Vector3[shapeVertexNum];
		for (int i=0;i<shapeVertexNum;i++)
		{
//			shapeVertexs[i] = shape.transform.InverseTransformPoint(shape.smoothPoints[i]);
			shapeVertexs[i] = shape.pathPoints[i];
		}
		
		//Create Procedure Mesh when moving
		if ((lastPos-myTransform.position).magnitude>intervalDistance)
//		if (Input.GetKeyDown(KeyCode.Space))
		{
			//New Shape Vectors
			for (int i=0;i<shapeVertexNum;i++)
			{
				thisShapeVectors[i] = myTransform.TransformPoint(shapeVertexs[i]);
			}

			//Create UVs and Triangles
			for (int j=0;j<shapeVertexNum-1;j++)
			{
				p0 = j+1;
				p1 = j;
				//Add Vertices
//				Debug.Log("vertIndex = "+vertIndex +" p0 = "+p0+" p1 = "+p1);
				vertices[vertIndex] = createdMesh.InverseTransformPoint(lastShapeVectors[p0]);
				vertices[vertIndex+1] = createdMesh.InverseTransformPoint(lastShapeVectors[p1]);
				vertices[vertIndex+2] = createdMesh.InverseTransformPoint(thisShapeVectors[p0]);
				vertices[vertIndex+3] = createdMesh.InverseTransformPoint(thisShapeVectors[p1]);
				//Set UVs
				uvs[vertIndex] = new Vector2(0,0);
				uvs[vertIndex+1] = new Vector2(0,1);
				uvs[vertIndex+2] = new Vector2(1,0);
				uvs[vertIndex+3] = new Vector2(1,1);
				//Set Triangles
				triangles[triIndex] = vertIndex;
				triangles[triIndex+1] = vertIndex+1;
				triangles[triIndex+2] = vertIndex+2;
				triangles[triIndex+3] = vertIndex+2;
				triangles[triIndex+4] = vertIndex+1;
				triangles[triIndex+5] = vertIndex+3;
				//Index++
				vertIndex = vertIndex + 4;
				triIndex = triIndex + 6;
				//Vertex Array is Full, Create New Mesh
				if (vertIndex>=vertexNum-3)
				{
					CreateNewMesh();
				}
			}
			//Save thisShapeVectors to lastShapeVectors
			for (int k=0;k<shapeVertexNum;k++)
			{
				lastShapeVectors[k] = thisShapeVectors[k];
			}
			//Create Mesh
			mesh.Clear();
			mesh.vertices = vertices;
			mesh.uv = uvs;
			// mesh.colors = colors;
			mesh.triangles = triangles;
			//RecalculateNormals
			mesh.RecalculateNormals();
            if (isGenerateCollider)
            {
                StartCoroutine(GenerateCollider(subMesh, mesh));
            }
            //lastPos
            lastPos = myTransform.position;
		}
	}
	
	public void CreateNewMesh()
	{
		//Finish Create Last Mesh
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.uv = uvs;
		// mesh.colors = colors;
		mesh.triangles = triangles;
		//RecalculateNormals
		mesh.RecalculateNormals();
		//Create New Mesh
		subMesh = new GameObject("subMesh"+count);
		subMesh.AddComponent<MeshFilter>();
		subMesh.AddComponent<MeshRenderer>();
		if (isFade){
			ColorFade cf = subMesh.AddComponent<ColorFade>();
			cf.protectTime = protectTime;
			cf.fadeTime = fadeTime;
		}
		subMesh.GetComponent<Renderer>().sharedMaterial = material;
		mesh = subMesh.GetComponent<MeshFilter>().mesh;
		subMesh.transform.parent = createdMesh;
		subMesh.transform.localPosition = Vector3.zero;
		count++;
		for (int i=0;i<vertices.Length;i++)
		{
			vertices[i] = Vector3.zero;
			uvs[i] = Vector2.zero;
		}
		for (int j=0;j<triangles.Length;j++)
		{
			triangles[j] = 0;
		}
		vertIndex = 0;
		triIndex = 0;
	}
	
	IEnumerator GenerateCollider(GameObject go, Mesh newMesh)
	{
		if (newMesh.vertexCount == 0) yield break; // Fix Physics PhyX Error
		yield return new WaitForSeconds(0.2f);
		MeshCollider myMC = go.GetComponent<MeshCollider>();
		if (myMC==null){
			myMC = go.AddComponent<MeshCollider>();
		}
		newMesh.RecalculateBounds();
		myMC.sharedMesh = newMesh;
	}

}
