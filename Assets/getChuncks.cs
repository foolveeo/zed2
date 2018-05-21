using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getChuncks : MonoBehaviour {
    
    private sl.ZEDCamera zed;
    public ZEDSpatialMappingManager spatialMapping;

    // Use this for initialization
    void Start () {

      
        zed = sl.ZEDCamera.GetInstance();
        //spatialMapping.StartSpatialMapping();
    }

    // Update is called once per frame
    void Update () {

        if(!spatialMapping.IsUpdateThreadRunning)
        {
            zed = sl.ZEDCamera.GetInstance();

            this.GetComponent<MeshFilter>().mesh.Clear();
            List<ZEDSpatialMapping.Chunk> chunks = spatialMapping.ChunkList;

            int nbr_chunks = chunks.Count;
            List<int> nbr_vertices = new List<int>();
            List<int> nbr_triangles = new List<int>();
            List<int> cumulative_nbr_vertices = new List<int>();
            List<int> cumulative_nbr_triangles = new List<int>();

            int totVertices = 0;
            int totTriangles = 0;


            for (int i = 0; i < nbr_chunks; i++)
            {

                nbr_vertices.Add(chunks[i].proceduralMesh.vertices.Length);
                totTriangles += chunks[i].proceduralMesh.triangles.Length;
                totVertices += nbr_vertices[i];
                totTriangles += nbr_triangles[i];

                if (i == 0)
                {
                    cumulative_nbr_triangles.Add(0);
                    cumulative_nbr_vertices.Add(0);
                }
                else
                {
                    cumulative_nbr_vertices.Add(cumulative_nbr_vertices[i - 1] + nbr_vertices[i]);
                    cumulative_nbr_triangles.Add(cumulative_nbr_triangles[i - 1] + nbr_triangles[i]);
                }
            }

            Vector3[] vertices = new Vector3[totVertices];
            int[] triangles = new int[totVertices];

            for (int i = 0; i < nbr_chunks; i++)
            {
                chunks[i].proceduralMesh.vertices.CopyTo(vertices, cumulative_nbr_vertices[i]);
                chunks[i].proceduralMesh.triangles.CopyTo(triangles, cumulative_nbr_triangles[i]);
            }

            this.GetComponent<MeshFilter>().mesh.vertices = vertices;
            this.GetComponent<MeshFilter>().mesh.triangles = triangles;




        }



    }
}
