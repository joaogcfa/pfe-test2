using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.XR.MagicLeap;

using System;
using UnityEngine.Lumin;
using UnityEngine.Serialization;
using UnityEngine.XR;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Management;
using UnityEngine.XR.MagicLeap.Meshing;

using WebSocketSharp;
using System.Threading;
using WebSocketSharp.Server;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

// Servico teste chamado Laputa
public class Laputa : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        Debug.Log("Msg received!");
    }

    protected override void OnOpen()
    {
        Debug.Log("Server started!");
    }
}

public class GetMeshVertexWS : MonoBehaviour
{
    // Struct que contem os dados de cada submesh
    [Serializable]
    public struct subMeshes
    {
        public string nameSubMesh;
        public SerializableVector3[] verticesSubMesh;
        public SerializableVector2[] uvSubMesh;
        public int[] trianglesSubMesh;

        // 0 - Create
        // 1 - Update
        // 2 - Delete
        public int task;
    }

    List<subMeshes> receivedSubmeshes;

    public int task;

    public Material new_material;

    public Material red_material;

    public PhotonView view;

    public MLSpatialMapper Mapper;

    public string deleteMeshName;

    public subMeshes subMesh;

    public WebSocket ws;

    public Thread thread;

    public Thread threadMeshAdded;

    public Thread threadMeshUpdated;

    public bool meshBuildingReady;

    private List<String> nameList = new List<String>();

    public WebSocketServer wssv;

    public WebSocketSessionManager SessionManager;

    public int totalTriangles;

    // Transforma objetos da Unity em Bytes para envio
    byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    // Transforma os Bytes recebidos em objetos da Unity
    public object ByteArrayToObject(byte[] arrBytes)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        object obj = (object)binForm.Deserialize(memStream);
        return obj;
    }

    void Start()
    {
        // ip ML1 = ws://192.168.43.34:8080
        //ip pc = ws://192.168.43.150:8080
        String serverAddress = "ws://192.168.43.150:8080";

        meshBuildingReady = false;

        view = this.GetComponent<PhotonView>();

        // SERVER
#if UNITY_LUMIN
        try
        {
            wssv = new WebSocketServer(serverAddress);

            wssv.AddWebSocketService<Laputa>("/Laputa");
            wssv.Start();

            SessionManager = wssv.WebSocketServices["/Laputa"].Sessions;
        }
        catch (Exception e)
        {
            print(e.Message);
        }

        // Evento do Magic Leap que cria as malhas
        Mapper.meshAdded += delegate(MeshId meshId)
        {
            string meshName;
            GameObject MeshingNodes;
            Mesh actualMesh;
            Vector3[] tempverticesArray;
            int[] TrianglesArray;
            SerializableVector3[] verticesArray;

            meshName = "Mesh " + meshId.ToString();

            //lista de vertex
            MeshingNodes = GameObject.Find(meshName);

            // getting mesh
            actualMesh = MeshingNodes.GetComponent<MeshFilter>().mesh;

            // getting vertices from mesh
            tempverticesArray = actualMesh.vertices;

            // getting triangles
            TrianglesArray = actualMesh.GetTriangles(0);

            threadMeshAdded = new Thread(() =>
            {
                // Array de bytes a ser enviada com os dados das malhas
                byte[][] requestArray = new byte[4][];

                verticesArray = new SerializableVector3[tempverticesArray.Length];

                // Converte os vertices de vector3 para serializableVector3
                for (int i = 0; i < verticesArray.Length; i++)
                {
                    verticesArray[i] = new SerializableVector3(
                        tempverticesArray[i].x,
                        tempverticesArray[i].y,
                        tempverticesArray[i].z
                    );
                }

                // Impede que varias malhas de mesmo nome sejam criadas
                if (!nameList.Contains(meshName))
                {
                    nameList.Add(meshName);

                    requestArray[0] = ObjectToByteArray(meshName);
                    requestArray[1] = ObjectToByteArray(verticesArray);
                    requestArray[2] = ObjectToByteArray(TrianglesArray);
                    // O numero 0 indica que eh pra acontecer criacao de malhas
                    requestArray[3] = ObjectToByteArray(0);

                    print(
                        meshName
                            + " /tamanho vertices "
                            + verticesArray.Count()
                            + " /tamanho triangulos "
                            + TrianglesArray.Count()
                            + "/criacao"
                    );

                    SessionManager.Broadcast(ObjectToByteArray(requestArray));
                }
            });

            threadMeshAdded.Start();
        };

        // Evento do Magic Leap que edita as malhas
        Mapper.meshUpdated += delegate(MeshId meshId)
        {
            string meshName;
            GameObject MeshingNodes;
            Mesh actualMesh;
            Vector3[] tempverticesArray;
            int[] TrianglesArray;
            SerializableVector3[] verticesArray;

            meshName = "Mesh " + meshId.ToString();

            //lista de vertex
            MeshingNodes = GameObject.Find(meshName);

            // getting mesh
            actualMesh = MeshingNodes.GetComponent<MeshFilter>().mesh;

            // getting vertices from mesh
            tempverticesArray = actualMesh.vertices;

            // getting triangles
            TrianglesArray = actualMesh.GetTriangles(0);

            threadMeshUpdated = new Thread(() =>
            {
                // Array de bytes a ser enviada com os dados das malhas
                byte[][] requestArray = new byte[4][];

                verticesArray = new SerializableVector3[tempverticesArray.Length];

                // Converte os vertices de vector3 para serializableVector3
                for (int i = 0; i < verticesArray.Length; i++)
                {
                    verticesArray[i] = new SerializableVector3(
                        tempverticesArray[i].x,
                        tempverticesArray[i].y,
                        tempverticesArray[i].z
                    );
                }

                requestArray[0] = ObjectToByteArray(meshName);
                requestArray[1] = ObjectToByteArray(verticesArray);
                requestArray[2] = ObjectToByteArray(TrianglesArray);
                // O numero 1 indica que eh pra acontecer criacao de malhas
                requestArray[3] = ObjectToByteArray(1);

                print(
                    meshName
                        + " /tamanho vertices "
                        + verticesArray.Count()
                        + " /tamanho triangulos "
                        + TrianglesArray.Count()
                        + "/atualizacao"
                );

                SessionManager.Broadcast(ObjectToByteArray(requestArray));
            });

            threadMeshUpdated.Start();
        };

        // Evento do Magic Leap que deleta as malhas
        // (POR ENQUANTO ESTA DESABILITADO)
        Mapper.meshRemoved += delegate(MeshId meshId)
        {
            string meshName = "Mesh " + meshId.ToString();
            SerializableVector3[] verticesArray = new SerializableVector3[0];
            int[] TrianglesArray = new int[0];
            int task = 2;

            // Array de bytes a ser enviada com os dados das malhas
            byte[][] requestArray = new byte[4][];

            requestArray[0] = ObjectToByteArray(meshName);
            requestArray[1] = ObjectToByteArray(verticesArray);
            requestArray[2] = ObjectToByteArray(TrianglesArray);
            requestArray[3] = ObjectToByteArray(task);

            print(
                meshName
                    + " /tamanho vertices "
                    + verticesArray.Count()
                    + " /tamanho triangulos "
                    + TrianglesArray.Count()
                    + "/destruicao"
            );

            SessionManager.Broadcast(ObjectToByteArray(requestArray));
        };
#endif

        // CLIENT
#if UNITY_STANDALONE_WIN

        // Funcao que checa se há malhas a serem criadas
        InvokeRepeating("MeshCreation", 5.0f, 0.01f);

        receivedSubmeshes = new List<subMeshes>();

        thread = new Thread(() =>
        {
            print("Thread Entered");

            // Cria o client
            ws = new WebSocket(serverAddress + "/Laputa");

            while (ws.IsAlive == false)
            {
                ws.Connect();
                print("Trying Connection...");
                Thread.Sleep(2000);
            }
            print("Connected");

            // Assim que o client recebe uma mensagem executa:
            ws.OnMessage += (sender, e) =>
            {
                try
                {
                    print("recebendo");
                    byte[][] receivedArray = new byte[4][];
                    subMeshes receivedMesh;
                    object obj = ByteArrayToObject(e.RawData);
                    receivedArray = obj as byte[][];

                    string meshName = ByteArrayToObject(receivedArray[0]) as string;
                    SerializableVector3[] verticesArray =
                        ByteArrayToObject(receivedArray[1]) as SerializableVector3[];
                    SerializableVector2[] UvArray = new SerializableVector2[verticesArray.Length];
                    int vCount = 0;
                    while (vCount < verticesArray.Length)
                    {
                        UvArray[vCount] = new SerializableVector2(
                            verticesArray[vCount].x,
                            verticesArray[vCount].z
                        );
                        vCount++;
                    }
                    int[] TrianglesArray = ByteArrayToObject(receivedArray[2]) as System.Int32[];
                    int task = (System.Int32)ByteArrayToObject(receivedArray[3]);

                    receivedMesh.nameSubMesh = meshName;
                    receivedMesh.uvSubMesh = UvArray;
                    receivedMesh.verticesSubMesh = verticesArray;
                    receivedMesh.trianglesSubMesh = TrianglesArray;
                    receivedMesh.task = task;

                    receivedSubmeshes.Add(receivedMesh);

                    meshBuildingReady = true;
                }
                catch (Exception error)
                {
                    Debug.Log(error);
                }
            };
        });

        thread.Start();

        void OnDestroy()
        {
            thread.Abort();
        }
#endif
    }

    void Update() { }

    void MeshCreation()
    {
        if (receivedSubmeshes.Count() > 0)
        {
            CreateMesh();
        }
        // if (meshBuildingReady) {
        //     print("Task being done: " + task);
        //     if(task == 0) {
        //         CreateMesh(false);
        //     } else if(task == 1) {
        //         DeleteMesh(meshName);
        //         CreateMesh(true);
        //     } else if(task == 2) {
        //         DeleteMesh(meshName);
        //     }
        //     meshBuildingReady = false;
        // }
    }

    public void CreateMesh()
    {
        //criando obj vazio com mesh filter

        GameObject MeshingNodes;
        GameObject meshObj;

        meshObj = new GameObject(receivedSubmeshes[0].nameSubMesh);
        MeshFilter meshfilter = meshObj.AddComponent<MeshFilter>();
        meshfilter.gameObject.AddComponent<MeshRenderer>();
        Mesh newMesh = new Mesh();
        meshObj.GetComponent<MeshFilter>().mesh = newMesh;

        Vector3[] TempVert = new Vector3[receivedSubmeshes[0].verticesSubMesh.Length];

        for (int i = 0; i < TempVert.Length; i++)
        {
            TempVert[i] = new Vector3(
                receivedSubmeshes[0].verticesSubMesh[i].x,
                receivedSubmeshes[0].verticesSubMesh[i].y,
                receivedSubmeshes[0].verticesSubMesh[i].z
            );
        }
        newMesh.vertices = TempVert;

        Vector2[] TempUV = new Vector2[receivedSubmeshes[0].uvSubMesh.Length];
        for (int i = 0; i < TempUV.Length; i++)
        {
            TempUV[i] = new Vector2(
                receivedSubmeshes[0].uvSubMesh[i].x,
                receivedSubmeshes[0].uvSubMesh[i].y
            );
        }
        newMesh.uv = TempUV;

        newMesh.triangles = receivedSubmeshes[0].trianglesSubMesh;
        if (receivedSubmeshes[0].task == 1)
        {
            DeleteMesh(receivedSubmeshes[0].nameSubMesh);
            meshObj.GetComponent<Renderer>().material = red_material;
        }
        else if(receivedSubmeshes[0].task == 0)
        {
            meshObj.GetComponent<Renderer>().material = new_material;
        }
        else{
            DeleteMesh(receivedSubmeshes[0].nameSubMesh);
        }

        MeshingNodes = GameObject.Find("MeshingNodes");
        meshObj.transform.parent = MeshingNodes.transform;
        meshObj.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        receivedSubmeshes.RemoveAt(0);
    }

    public void DeleteMesh(String deleteMeshName)
    {
        Destroy(GameObject.Find(deleteMeshName));
    }
}
