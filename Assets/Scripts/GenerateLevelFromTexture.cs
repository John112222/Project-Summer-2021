using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class GenerateLevelFromTexture : MonoBehaviourPun
{
    public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
		CustomTree,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
    public bool FullRandom = false;
	public int RandomSeed = 12345;

    public int Rows = 5;
	public int Columns = 5;
	[Tooltip("Size of a (square) cell."), Range(2,21)]
	public int CellSize = 3;
	public int wallStack = 3;
    public ColorToMultiPrefabs[] colorMappings;
	[SerializeField] Texture2D groundTexture;
	[SerializeField] Texture2D wallTexture;
    private BasicMazeGenerator mMazeGenerator = null;

	[SerializeField] Transform _levelHolder = null;
	[SerializeField] bool willGenerateGround = false;

	public Transform levelHolder => _levelHolder == null? this.transform: _levelHolder;

    void Start()
    {
        //RunGenerator();
    }

	public void RunGenerator()
	{
		ResetMaze();
		GenerateMaze();
        GenerateTextures();
		if(willGenerateGround) GenerateLevel(groundTexture, 0);
		for(int i = 1; i <= wallStack; i++)
		{
			GenerateLevel(wallTexture, i);
		}
		SaveTexture(wallTexture);
	}

	public void ResetMaze()
    {
		var children = new List<GameObject>();
		foreach (Transform child in levelHolder) children.Add(child.gameObject);
		children.ForEach(child => DestroyImmediate(child));
    }


    public void GenerateMaze()
    {
		if (!FullRandom)
		{
			Random.InitState(RandomSeed);
		}
		switch (Algorithm)
		{
			case MazeGenerationAlgorithm.PureRecursive:
				mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
				break;
			case MazeGenerationAlgorithm.RecursiveTree:
				mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
				break;
			case MazeGenerationAlgorithm.RandomTree:
				mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
				break;
			case MazeGenerationAlgorithm.OldestTree:
				mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
				break;
			case MazeGenerationAlgorithm.RecursiveDivision:
				mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
				break;
			case MazeGenerationAlgorithm.CustomTree:
				mMazeGenerator = new CustomTreeMazeGenerator(Rows, Columns);
				break;
		}
		mMazeGenerator.GenerateMaze();
    }

    public void GenerateTextures()
    {
        groundTexture = new Texture2D(Columns * CellSize + 1, Rows * CellSize + 1, TextureFormat.RGBA32, false);
        wallTexture = new Texture2D(Columns * CellSize + 1, Rows * CellSize + 1, TextureFormat.RGBA32, false);
        GenerateDefaultGround();
        GeneratingWall();
        groundTexture.Apply();
        wallTexture.Apply();
    }
	//private void placerandomobjects(){

	//}

    private void GeneratingWall()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                SetCellTexture(row, column);
            }
        }
    }

    private void GenerateDefaultGround()
    {
        for (int i = 0; i < groundTexture.width; i++)
        {
            for (int j = 0; j < groundTexture.height; j++)
            {
                groundTexture.SetPixel(i, j, Color.black);
            }
        }
    }

    public int cellrandom = 0;
	public int CenterSizeX = 0;
	public int CenterSizeZ = 0;

	public List<Color> index2color = new List<Color>();
	private void SetCellTexture(int row, int column)
	{
		var cell = mMazeGenerator.GetMazeCell(row, column);
		if(cell.randomint<cellrandom)return;
		
		if((row<=CenterSizeX||row>=Rows-CenterSizeX)&&column<=Columns/2+CenterSizeZ/2&&column>=Columns/2-CenterSizeZ/2){
			return;
		}
		Color wallcolor = Color.black;
		if(cell.randomint<index2color.Count){
			wallcolor = index2color[cell.randomint];
		}

		if(cell.WallLeft)
		{
			for(int lineX = 0; lineX <= CellSize; lineX++)
			{
				wallTexture.SetPixel(column*CellSize, row*CellSize + lineX, wallcolor);
			}
		}
		if(cell.WallBack)
		{
			for(int lineY = 0; lineY <= CellSize; lineY++)
			{
				wallTexture.SetPixel(column*CellSize + lineY, row*CellSize, wallcolor);
			}
		}
		if(cell.WallRight && column == Columns-1)
		{
			for(int lineX = 0; lineX <= CellSize; lineX++)
			{
				wallTexture.SetPixel(column*CellSize + CellSize, row*CellSize + lineX, wallcolor);
			}
		}
		if(cell.WallFront && row == Rows-1)
		{
			for(int lineY = 0; lineY <= CellSize; lineY++)
			{
				wallTexture.SetPixel(column*CellSize + lineY, row*CellSize + CellSize, wallcolor);
			}
		}
	}
	void GenerateLevel(Texture2D map, int y)
    {
        for (int i = 0; i < map.width; i++)
        {
            for (int j = 0; j < map.height; j++)
            {
                GenerateTile(new Vector3(i, y, j), map.GetPixel(i, j));
            }
        }
    }
	void GenerateTile(Vector3 position, Color pixelColor)
    {
        //is not transperant
        if (pixelColor.a != 0)
        {
            foreach (ColorToMultiPrefabs colorMapping in colorMappings)
            {
                if (colorMapping.color.Equals(pixelColor))
                {
                    var tile = Instantiate(colorMapping.ReturnRandomPrefab(), position, Quaternion.identity, levelHolder);
					tile.transform.localPosition = position;
                }
            }
        }
    }
	public static void SaveTexture(Texture2D texture)
     {
         byte[] bytes = texture.EncodeToPNG();
         var dirPath = Application.dataPath + "/RenderOutput";
         if (!System.IO.Directory.Exists(dirPath))
         {
             System.IO.Directory.CreateDirectory(dirPath);
         }
         System.IO.File.WriteAllBytes(dirPath + "/R_" + Random.Range(0, 100000) + ".png", bytes);
         Debug.Log(bytes.Length / 1024 + "Kb was saved as: " + dirPath);
 #if UNITY_EDITOR
         UnityEditor.AssetDatabase.Refresh();
 #endif
     }
}

[CustomEditor(typeof(GenerateLevelFromTexture))]
public class GenerateLevelFromTextureEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		GenerateLevelFromTexture spawner = (GenerateLevelFromTexture)target;
		if (GUILayout.Button("Generate"))
        {
            spawner.RunGenerator();
        }
		if (GUILayout.Button("Delete"))
        {
            spawner.ResetMaze();
        }
		
	}
}

[System.Serializable]
public class ColorToMultiPrefabs
{
	[Tooltip("The color to replace")]
    public Color color = Color.black;
    [Tooltip("The prefab to replace it with")]
    public GameObject[] prefabs;

	public ColorToMultiPrefabs(){}

    public ColorToMultiPrefabs(Color color)
    {
        this.color = color;
    }

	public GameObject ReturnRandomPrefab()
	{
		return prefabs[Random.Range(0, prefabs.Length)];
	}
}