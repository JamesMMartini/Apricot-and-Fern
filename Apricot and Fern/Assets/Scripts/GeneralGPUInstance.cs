using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This class holds the data for each object that will be batched
/// </summary>
public class ObjDataGen
{
    //location data
    public Vector3 pos;
    public Vector3 scale;
    public Quaternion rot;

    //matrix conversion
    public Matrix4x4 matrix
    {
        get
        {
            return Matrix4x4.TRS(pos, rot, scale);

        }
    }

    //constructor
    public ObjDataGen(Vector3 pos, Vector3 scale, Quaternion rot)
    {
        this.pos = pos;
        this.scale = scale;
        this.rot = rot;
    }
}



/// <summary>
/// This class handles the batching of the models
/// </summary>
public class GeneralGPUInstance : MonoBehaviour
{
    [SerializeField] posData[] objects;
    [SerializeField] GameObject[] gameObjects;
    //[SerializeField] int minTrees = 0, maxTrees = 100;
    //[SerializeField] Vector2 xBounds, yBounds, zBounds;
    //[SerializeField] GameObject tree;

    struct posData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public posData(Vector3 pos, Quaternion rot, Vector3 scl)
        {
            this.position = pos;
            this.rotation = rot;
            this.scale = scl;
        }

    }

    //These arrays hold the location data in vector3 and quaternion form for all of the models that will be batched
    public Vector3[] positions;
    public Vector3[] scales;
    public Quaternion[] rotations;
    //public GameObject[] gameObjects;
    public Mesh objMesh; //the mesh to instance at the location of the object batching
    public Material objMat; //the material to apply to the instanced mesh
    public int batchIndexMax = 942; //the number of instances in each batch, max is 1023

    private List<List<ObjDataGen>> batches = new List<List<ObjDataGen>>(); //the list to keep track of all the batches
    void Start()
    {
        //objects = new posData[Random.Range(minTrees, maxTrees)];
        //gameObjects = new GameObject[Random.Range(minTrees, maxTrees)];
        //GenerateObjects();
        gameObjects = new GameObject[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject currBuilding = this.transform.GetChild(i).gameObject;
            gameObjects[i] = currBuilding;
            //currBuilding.SetActive(false);

        }

        InitializeObjectPositions();

        int batchIndexNum = 0;
        List<ObjDataGen> currBatch = new List<ObjDataGen>(); //the current batch
        for (int i = 0; i < positions.Length; i++) //for every position coordinate in the array
        {
            //add an object at that position to the batch
            AddObj(currBatch, i);
            batchIndexNum++;

            if (batchIndexNum >= batchIndexMax) //once enough objects have been added to the batch, build the next batch
            {
                batches.Add(currBatch);
                currBatch = BuildNewBatch();
                batchIndexNum = 0;
            }
        }
    }

    /*
    void GenerateObjects()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i] = Instantiate(tree, new Vector3(Random.Range(xBounds.x, xBounds.y), yBounds.x, Random.Range(zBounds.x, zBounds.y)), Quaternion.identity,
            this.transform);
            //tree.transform.localPosition = new Vector3(Random.Range(xBounds.x, xBounds.y), Random.Range(yBounds.x, yBounds.y), Random.Range(zBounds.x, zBounds.y));

            //objects[i] = new posData(new Vector3(Random.Range(xBounds.x, xBounds.y),
            //    Random.Range(yBounds.x, yBounds.y), Random.Range(zBounds.x, zBounds.y)),
            //    new Quaternion(), new Vector3(1, 1, 1));
        }
    }
    */
    void InitializeObjectPositions()
    {
        positions = new Vector3[gameObjects.Length];
        rotations = new Quaternion[gameObjects.Length];
        scales = new Vector3[gameObjects.Length];
        for (int i = 0; i < gameObjects.Length; i++)
        {
            //Debug.Log(blocks[i].transform.position);
            positions[i] = gameObjects[i].transform.position;
            rotations[i] = gameObjects[i].transform.rotation;
            scales[i] = gameObjects[i].transform.lossyScale;
            gameObjects[i].SetActive(false);
        }

    }


    // Update is called once per frame

    void Update()
    {
        RenderBatches();
    }

    /// <summary>
    /// This method adds a new object to a match given the index of the location data to use
    /// </summary>
    /// <param name="currBatch"></param> The current batch to add to
    /// <param name="i"></param> The index of the position, rotation, and scale in the storage arrays
    private void AddObj(List<ObjDataGen> currBatch, int i)
    {
        currBatch.Add(new ObjDataGen(positions[i], scales[i], rotations[i]));
        // Debug.Log("Added Mesh");
    }
    private List<ObjDataGen> BuildNewBatch()
    {
        return new List<ObjDataGen>();
    }
    /// <summary>
    /// This method renders all of the batches in the list using the mesh instancing method provided in the graphics library
    /// </summary>
    private void RenderBatches()
    {
        foreach (var batch in batches)
        {
            Graphics.DrawMeshInstanced(objMesh, 0, objMat, batch.Select((a) => a.matrix).ToList());
        }
    }
}

