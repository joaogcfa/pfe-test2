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


// public class Laputa : WebSocketBehavior {
    

//     // Transforma objetos da Unity em Bytes para envio
//     byte[] ObjectToByteArray(object obj)
//     {
//         if(obj == null)
//             return null;
//         BinaryFormatter bf = new BinaryFormatter();
//         using (MemoryStream ms = new MemoryStream()) {
//             bf.Serialize(ms, obj);
//             return ms.ToArray();
//         }
//     }

//     protected override void OnMessage (MessageEventArgs e) {

//         byte[][] requestArray = new byte[4][];
//         int subMeshesLength = GetMeshVertexWS.subMeshesArray.Count;
        

//         if(subMeshesLength > 0) {   
//             try {
//                 requestArray[0] = ObjectToByteArray(GetMeshVertexWS.subMeshesArray[0].nameSubMesh); 
//                 requestArray[1] = ObjectToByteArray(GetMeshVertexWS.subMeshesArray[0].verticesSubMesh); 
//                 // requestArray[2] = ObjectToByteArray(GetMeshVertexWS.subMeshesArray[0].uvSubMesh); 
//                 requestArray[2] = ObjectToByteArray(GetMeshVertexWS.subMeshesArray[0].trianglesSubMesh); 
//                 requestArray[3] = ObjectToByteArray(GetMeshVertexWS.subMeshesArray[0].task);
                

//                 // SendAsync(ObjectToByteArray(requestArray), null);
//                 // Sessions.Broadcast(ObjectToByteArray(requestArray));

//                 // IEnumerable<byte> resultRequest = requestArray[0].Concat(requestArray[1]).Concat(requestArray[2].Concat(requestArray[3]).Concat(requestArray[4]));
//                 // byte[] result = resultRequest.SelectMany(a => a).ToArray();
//                 // SendAsync(result, null);

//                 // while(!(GetMeshVertexWS.subMeshesArray[0].verticesSubMesh.Count() == GetMeshVertexWS.subMeshesArray[0].uvSubMesh.Count())) {
//                 //     GetMeshVertexWS.subMeshesArray.RemoveAt(0);
//                 //     if (subMeshesLength <= 0) {
//                 //         return;
//                 //     }
//                 // }

//                 // if (nameList.Contains(GetMeshVertexWS.subMeshesArray[0].nameSubMesh)) {
//                 //     GetMeshVertexWS.subMeshesArray.RemoveAt(0);
//                 //     return;
//                 // }

//                 // SendAsync(ObjectToByteArray(GetMeshVertexWS.subMeshesArray[0].nameSubMesh), null);
//                 // SendAsync(ObjectToByteArray(GetMeshVertexWS.subMeshesArray[0].verticesSubMesh), null);
//                 // // SendAsync(ObjectToByteArray(GetMeshVertexWS.subMeshesArray[0].uvSubMesh), null);
//                 // SendAsync(ObjectToByteArray(GetMeshVertexWS.subMeshesArray[0].trianglesSubMesh), null);
//                 // SendAsync(ObjectToByteArray(GetMeshVertexWS.subMeshesArray[0].task), null);
                

//                 // Send(ObjectToByteArray(GetMeshVertexWS.subMeshesArray[subMeshesLength-1].nameSubMesh));
//                 // Send(ObjectToByteArray(GetMeshVertexWS.subMeshesArray[subMeshesLength-1].verticesSubMesh));
//                 // Send(ObjectToByteArray(GetMeshVertexWS.subMeshesArray[subMeshesLength-1].uvSubMesh));
//                 // Send(ObjectToByteArray(GetMeshVertexWS.subMeshesArray[subMeshesLength-1].trianglesSubMesh));
//                 // Send(ObjectToByteArray(GetMeshVertexWS.subMeshesArray[subMeshesLength-1].task));
//                 Debug.Log("Malha enviada!");

//                 GetMeshVertexWS.subMeshesArray.RemoveAt(0);

//             } catch(Exception error) {
//                 String errorMsg = "Erro: " + error;
//                 Send(errorMsg);
//             }
//         }
//     }

//     protected override void OnOpen () {
//         Debug.Log("Server started!");
//     }
// }

// public class GetMeshVertexWS : MonoBehaviour {
//     [Serializable]
//     public struct subMeshes {
//         public string nameSubMesh;
//         public SerializableVector3[] verticesSubMesh;
//         public SerializableVector2[] uvSubMesh;
//         public int[] trianglesSubMesh;
//         // 0 - Create
//         // 1 - Update
//         // 2 - Delete
//         public int task;
//     }

//     // public Laputa laputaService;

//     public static List<subMeshes> subMeshesArray;
//     // public List<subMeshes> subMeshesArrayUpdate;

//     public Mesh actualMesh;
    
//     public SerializableVector3[] verticesArray;
//     public Vector3[] tempverticesArray;
//     public SerializableVector2[] UvArray;   
//     public int[] TrianglesArray;
//     public string meshName;
//     public int task;

//     public GameObject MeshingNodes;

//     public int meshQuantity;

//     public GameObject meshObj;

//     public Material new_material;

//     public Material red_material;

//     public PhotonView view;

//     public bool canSend;

//     public int currentMeshIndex;

//     public MLSpatialMapper Mapper;

//     public string deleteMeshName;

//     public subMeshes subMesh; 

//     public int subMeshesLength;

//     public int subMeshesLengthUpdate;

//     public WebSocket ws;

//     public Thread thread;

//     public Thread threadMeshAdded;

//     public Thread threadMeshUpdated;
    
//     private static Mutex mut = new Mutex();

//     public int complete;

//     private List<String> nameList = new List<String>(); 

//     int vCount;

//     public WebSocketServer wssv;

//     public WebSocketSessionManager SessionManager;

//     // Transforma objetos da Unity em Bytes para envio
//     byte[] ObjectToByteArray(object obj)
//     {
//         if(obj == null)
//             return null;
//         BinaryFormatter bf = new BinaryFormatter();
//         using (MemoryStream ms = new MemoryStream()) {
//             bf.Serialize(ms, obj);
//             return ms.ToArray();
//         }
//     }

//     // Transforma os Bytes recebidos em objetos da Unity
//     public object ByteArrayToObject(byte[] arrBytes) {
//         MemoryStream memStream = new MemoryStream();
//         BinaryFormatter binForm = new BinaryFormatter();
//         memStream.Write(arrBytes, 0, arrBytes.Length);
//         memStream.Seek(0, SeekOrigin.Begin);
//         object obj = (object) binForm.Deserialize(memStream);
//         return obj;
//     }

//     void Start() {



//         String serverAddress = "ws://192.168.43.34:8080";

        

//         complete = 0;

//         view = this.GetComponent<PhotonView>();

//         subMeshesArray = new List<subMeshes>();

//         // subMeshesArrayUpdate = new List<subMeshes>();

//         // Server
//         #if UNITY_LUMIN 
//             try {
//                 wssv = new WebSocketServer (serverAddress);

//                 wssv.AddWebSocketService<Laputa>("/Laputa");
//                 wssv.Start ();

//                 SessionManager = wssv.WebSocketServices["/Laputa"].Sessions;
                
//                 // Console.ReadKey (true);
//                 // wssv.Stop ();
//             } catch(Exception e) {
//                 print(e.Message);
//             }

//             Mapper.meshAdded += delegate(MeshId meshId) {

//                 byte[][] requestArray = new byte[4][];
//                 // WebSocketSessionManager Server;
            
//                 meshName = "Mesh " + meshId.ToString();

//                 //lista de vertex
//                 MeshingNodes = GameObject.Find(meshName);

//                 // getting mesh
//                 actualMesh = MeshingNodes.GetComponent<MeshFilter>().mesh;


//                 // getting vertices from mesh
//                 tempverticesArray = actualMesh.vertices;

//                 // getting uv from mesh

//                 // getting triangles
//                 TrianglesArray = actualMesh.GetTriangles(0);

//                 threadMeshAdded = new Thread(() => {
//                         verticesArray = new  SerializableVector3[tempverticesArray.Length];
//                         for (int i = 0; i < verticesArray.Length; i++) {
//                             verticesArray[i] = new SerializableVector3(tempverticesArray[i].x, tempverticesArray[i].y, tempverticesArray[i].z);
//                         }

//                         // UvArray = new  SerializableVector2[tempverticesArray.Length];
//                         // for (int i = 0; i < UvArray.Length; i++) {
//                         //     UvArray[i] = new SerializableVector2(tempverticesArray[i].x, tempverticesArray[i].z);
//                         // }

                        

//                         // subMesh.nameSubMesh = meshName;
//                         // subMesh.verticesSubMesh = verticesArray;
//                         // subMesh.uvSubMesh = UvArray;
//                         // subMesh.trianglesSubMesh = TrianglesArray;
//                         // subMesh.task = 0;
//                         if (!nameList.Contains(meshName)) {
//                             nameList.Add(meshName);
            

//                             requestArray[0] = ObjectToByteArray(meshName); 
//                             requestArray[1] = ObjectToByteArray(verticesArray); 
//                             requestArray[2] = ObjectToByteArray(TrianglesArray); 
//                             requestArray[3] = ObjectToByteArray(0);
                            

//                             // SendAsync(ObjectToByteArray(requestArray), null);
//                             // laputaService.sendData(ObjectToByteArray(requestArray));
//                             SessionManager.Broadcast(ObjectToByteArray(requestArray));
                            
//                             // mut.WaitOne();
//                             // subMeshesArray.Add(subMesh);
//                             // mut.ReleaseMutex();
//                         }
                        
//                 });

//                 threadMeshAdded.Start();
            
                

//                 // #if UNITY_LUMIN 
//                 //     PhotonNetwork.CleanRpcBufferIfMine(view);
//                 // #endif
//             };

//             Mapper.meshUpdated += delegate(MeshId meshId) {

//                 byte[][] requestArray = new byte[4][];
//                 // WebSocketSessionManager Server;

//                 meshName = "Mesh " + meshId.ToString();

//                 //lista de vertex
//                 MeshingNodes = GameObject.Find(meshName);

//                 // getting mesh
//                 actualMesh = MeshingNodes.GetComponent<MeshFilter>().mesh;

//                 // getting vertices from mesh
//                 // verticesArray = actualMesh.vertices;
                
//                 tempverticesArray = actualMesh.vertices;

//                 // verticesArray = new  SerializableVector3[actualMesh.vertices.Length];
//                 // for (int i = 0; i < verticesArray.Length; i++)
//                 // {
//                 //     verticesArray[i] = new SerializableVector3(actualMesh.vertices[i].x, actualMesh.vertices[i].y, actualMesh.vertices[i].z);
//                 // }

//                 // // getting uv from mesh
//                 // UvArray = new  SerializableVector2 [actualMesh.vertices.Length];
//                 // for (int i = 0; i < UvArray.Length; i++)
//                 // {
//                 //     UvArray[i] = new SerializableVector2(actualMesh.vertices[i].x, actualMesh.vertices[i].z);
//                 // }

//                 // getting triangles
//                 TrianglesArray = actualMesh.GetTriangles(0);

//                 threadMeshUpdated = new Thread(() => {
//                         verticesArray = new  SerializableVector3[tempverticesArray.Length];
//                         for (int i = 0; i < verticesArray.Length; i++) {
//                             verticesArray[i] = new SerializableVector3(tempverticesArray[i].x, tempverticesArray[i].y, tempverticesArray[i].z);
//                         }

//                         // UvArray = new  SerializableVector2[tempverticesArray.Length];
//                         // for (int i = 0; i < UvArray.Length; i++) {
//                         //     UvArray[i] = new SerializableVector2(tempverticesArray[i].x, tempverticesArray[i].z);
//                         // }

                        

//                         // subMesh.nameSubMesh = meshName;
//                         // subMesh.verticesSubMesh = verticesArray;
//                         // subMesh.uvSubMesh = UvArray;
//                         // subMesh.trianglesSubMesh = TrianglesArray;
//                         // subMesh.task = 1;




//                         requestArray[0] = ObjectToByteArray(meshName); 
//                         requestArray[1] = ObjectToByteArray(verticesArray); 
//                         requestArray[2] = ObjectToByteArray(TrianglesArray); 
//                         requestArray[3] = ObjectToByteArray(1);

//                         // laputaService.sendData(ObjectToByteArray(requestArray));

//                         SessionManager.Broadcast(ObjectToByteArray(requestArray));

//                         // mut.WaitOne();
//                         // subMeshesArray.Add(subMesh);
//                         // mut.ReleaseMutex();
                    
//                 });
                
//                 threadMeshUpdated.Start();

//                 // #if UNITY_LUMIN 
//                 //     PhotonNetwork.CleanRpcBufferIfMine(view);
//                 // #endif
//             };

//             Mapper.meshRemoved += delegate(MeshId meshId) {
//                 meshName = "Mesh " + meshId.ToString();

//                 subMesh.nameSubMesh = meshName;
//                 subMesh.verticesSubMesh = new SerializableVector3[0];
//                 subMesh.uvSubMesh = new SerializableVector2[0];
//                 subMesh.trianglesSubMesh = new int[0];
//                 subMesh.task = 2;

//                 subMeshesArray.Add(subMesh);

//                 //rpc
//                 // view.RPC("send_delete_mesh", RpcTarget.AllBuffered, meshName);

//                 // #if UNITY_LUMIN 
//                 //     PhotonNetwork.CleanRpcBufferIfMine(view);
//                 // #endif
//             };
//         #endif 

//          // Client
//         #if UNITY_STANDALONE_WIN

//             InvokeRepeating("MeshCreation", 5.0f, 0.01f);

//             thread = new Thread(() => {
//                 print("Thread Entered");
//                 ws = new WebSocket(serverAddress + "/Laputa");

//                 // ws.OnOpen += (sender, e) =>
//                 // {
//                 //     Debug.Log("connected");
//                 // };
//                 while(ws.IsAlive == false) {
//                     ws.Connect();
//                     print("Trying Connection...");
//                     Thread.Sleep(2000);
//                 }
//                 print("Connected");

//                 // ws.Send("Initial Client Request");
//                 ws.OnMessage += (sender, e) => {
//                     try {
//                         byte[][] receivedArray = new byte[4][];
//                         object obj = ByteArrayToObject(e.RawData);
//                         receivedArray = obj as byte[][];

//                         meshName = ByteArrayToObject(receivedArray[0]) as string;
//                         verticesArray = ByteArrayToObject(receivedArray[1]) as SerializableVector3[];
//                         UvArray = new SerializableVector2[verticesArray.Length];
//                         vCount = 0;
//                         while (vCount < verticesArray.Length) {
//                             UvArray[vCount] = new SerializableVector2(verticesArray[vCount].x, verticesArray[vCount].z);
//                             vCount++;
//                         }
//                         TrianglesArray = ByteArrayToObject(receivedArray[2]) as System.Int32[];
//                         task = (System.Int32)ByteArrayToObject(receivedArray[3]);
//                         // complete = 4;


//                         // if (obj.GetType().Equals(typeof(System.String))) { 
//                         //     // Debug.Log("Recebi nameSubMesh: " + (string)obj);
//                         //     meshName = obj as string;
//                         //     complete++;
//                         // }
//                         // else if (obj.GetType().Equals(typeof(SerializableVector3[]))){
//                         //     // Debug.Log("Recebi verticesSubMesh: ");
//                         //     int contadorDebug = 0;
//                         //     foreach (SerializableVector3 i in (SerializableVector3[])obj) {
//                         //         contadorDebug++;
//                         //     //    Debug.Log(i);
//                         //     }
//                         //     // Debug.Log(contadorDebug);
//                         //     verticesArray = obj as SerializableVector3[];
//                         //     UvArray = new SerializableVector2[verticesArray.Length];
//                         //     vCount = 0;
//                         //     while (vCount < verticesArray.Length) {
//                         //         UvArray[vCount] = new SerializableVector2(verticesArray[vCount].x, verticesArray[vCount].z);
//                         //         vCount++;
//                         //     }

//                         //     // Debug.Log("Received Mesh");
//                         //     // Debug.Log(verticesArray.Length);
//                         //     complete++;
//                         // }
//                         // // else if (obj.GetType().Equals(typeof(SerializableVector2[]))){
//                         // //     // Debug.Log("Recebi uvSubMesh: ");
//                         // //     // foreach (SerializableVector2 i in (SerializableVector2[])obj ) {
//                         // //         // Debug.Log(i);
//                         // //     // }
//                         // //     UvArray = obj as SerializableVector2[];
//                         // //     complete++;
//                         // // }
//                         // else if (obj.GetType().Equals(typeof(System.Int32[]))){
//                         //     // WS_server.subMesh.trianglesSubMesh = (System.Int32[])obj;
//                         //     // Debug.Log("Recebi trianglesSubMesh: ");
//                         //     // foreach (int i in (int[])obj) {
//                         //         // Debug.Log(i);
//                         //     // }
//                         //     TrianglesArray = obj as System.Int32[];
//                         //     // foreach (int i in TrianglesArray) {
//                         //     //     print(i);
//                         //     // }
//                         //     complete++;
//                         // }
//                         // else if (obj.GetType().Equals(typeof(System.Int32))){
//                         //     // Debug.Log("Recebi task: " + (System.Int32)obj);
//                         //     task = (System.Int32)obj;
//                         //     complete++;
//                         // }

//                         // // var msg = e.Data == "BALUS"
//                         // //         ? "Are you kidding?"
//                         // //         : "I'm not available now.";

//                     } catch(Exception error) {
//                         Debug.Log(error);
//                     }
//                 };
//             });

//             thread.Start();

//             void OnDestroy() {
//                 thread.Abort();
//             }
//         #endif


//         // InvokeRepeating("sendServer", 2.0f, 1.0f);

//         // #if UNITY_LUMIN 
//         //     InvokeRepeating("callRPC", 5.0f, 0.3f);
//         //     InvokeRepeating("callRPCUpdate", 5.0f, 0.3f);
//         //     InvokeRepeating("MeshCreation", 5.0f, 0.3f);
//         // #endif

//     }

//     void sendServer() {
//         if(ws == null) {
//             return;
//         }
        
//         ws.Send("BALUS");
//         // print("Hello sent");
//     }

//     void Update() {
    
//     }

//     void MeshCreation() {
//         if (true) {
//             print("bababa" + task);
//             if(task == 0) {
//                 CreateMesh(false);
//             } else if(task == 1) {
//                 DeleteMesh(meshName);
//                 CreateMesh(true);
//             } else if(task == 2) {
//                 DeleteMesh(meshName);
//             }
//             // complete = 0;
//             //ws.Send("manda +");
            
//             // print("La puta esta completa!");
//         } 
//     }   

//     // public void callRPC() {
//     //     subMeshesLength = subMeshesArray.Count;
//     //     if(subMeshesLength > 0) {
//     //         //rpc
//     //         view.RPC(
//     //             "send_all_arrays", 
//     //             RpcTarget.AllBuffered,
//     //             subMeshesArray[subMeshesLength-1].verticesSubMesh,
//     //             subMeshesArray[subMeshesLength-1].uvSubMesh,
//     //             subMeshesArray[subMeshesLength-1].trianglesSubMesh,
//     //             subMeshesArray[subMeshesLength-1].nameSubMesh
//     //         );

//     //         subMeshesArray.RemoveAt(subMeshesLength-1);
//     //     }
//     // }

//     // public void callRPCUpdate() {
//     //     subMeshesLengthUpdate = subMeshesArrayUpdate.Count;
//     //     if(subMeshesLengthUpdate > 0) {
//     //         //rpc
//     //         view.RPC("send_delete_mesh", RpcTarget.AllBuffered, subMeshesArrayUpdate[subMeshesLengthUpdate-1].nameSubMesh);

//     //         view.RPC(
//     //             "update_arrays", 
//     //             RpcTarget.AllBuffered,
//     //             subMeshesArrayUpdate[subMeshesLengthUpdate-1].verticesSubMesh,
//     //             subMeshesArrayUpdate[subMeshesLengthUpdate-1].uvSubMesh,
//     //             subMeshesArrayUpdate[subMeshesLengthUpdate-1].trianglesSubMesh,
//     //             subMeshesArrayUpdate[subMeshesLengthUpdate-1].nameSubMesh
//     //         );

//     //         subMeshesArrayUpdate.RemoveAt(subMeshesLengthUpdate-1);
//     //     }
//     // }

//     // [PunRPC]
//     // void send_all_arrays(Vector3[] verticesUpdated, Vector2[] uvUpdated, int[] trianglesUpdated,string name)
//     // {
//     //     print("send_all_arrays");
//     //     #if UNITY_STANDALONE_WIN
//     //         verticesArray = verticesUpdated;
//     //         UvArray = uvUpdated;
//     //         TrianglesArray = trianglesUpdated;
//     //         meshName = name;
//     //         // print("Actual vertices count: " + verticesArray.Count() + "\n");
//     //         CreateMesh(false);
//     //     #endif
//     // }

//     // [PunRPC]
//     // void update_arrays(Vector3[] verticesUpdated, Vector2[] uvUpdated, int[] trianglesUpdated,string name)
//     // {
//     //     print("update_arrays");
//     //     #if UNITY_STANDALONE_WIN
//     //         verticesArray = verticesUpdated;
//     //         UvArray = uvUpdated;
//     //         TrianglesArray = trianglesUpdated;
//     //         meshName = name;
//     //         // print("Actual vertices count: " + verticesArray.Count() + "\n");
//     //         CreateMesh(true);
//     //     #endif
//     // }

//     // [PunRPC]
//     // void send_delete_mesh(string name)
//     // {
//     //     print("send_delete_mesh");
//     //     #if UNITY_STANDALONE_WIN
//     //         deleteMeshName = name;
//     //         // print("Actual vertices count: " + verticesArray.Count() + "\n");
//     //         DeleteMesh();
//     //     #endif
//     // }

//     public void CreateMesh(bool red) {
//         //criando obj vazio com mesh filter

//         string name = meshName;
//         meshObj = new GameObject(name);
//         MeshFilter meshfilter = meshObj.AddComponent<MeshFilter>();
//         meshfilter.gameObject.AddComponent<MeshRenderer>();
//         Mesh newMesh = new Mesh();
//         meshObj.GetComponent<MeshFilter>().mesh = newMesh;
        
//         // newMesh.vertices = verticesArray;
//         // if (verticesArray.Length > 0) {
//             // Debug.Log("CreateMesh");
//             // newMesh.vertices = new  Vector3[verticesArray.Length];
//             Vector3[] TempVert = new  Vector3[verticesArray.Length];
//             // print(newMesh.vertices.Length);
//             for (int i = 0; i < TempVert.Length; i++)
//             {
//                 TempVert[i] = new Vector3(verticesArray[i].x, verticesArray[i].y, verticesArray[i].z);
//                 // newMesh.vertices[i].x = verticesArray[i].x;
//                 // newMesh.vertices[i].y = verticesArray[i].y;
//                 // newMesh.vertices[i].z = verticesArray[i].z;
                
//             }
//             newMesh.vertices = TempVert;
//         // }
        
        
//         // newMesh.uv = UvArray;
//         // if(UvArray.Length > 0) {
//             Vector2[] TempUV = new  Vector2[UvArray.Length];
//             for (int i = 0; i < TempUV.Length; i++)
//             {
//                 TempUV[i] = new Vector2(UvArray[i].x, UvArray[i].y);
//                 // newMesh.uv[i] = new Vector2(UvArray[i].x, UvArray[i].y);
//                 // print(newMesh.uv[i]);
//             }
//             newMesh.uv = TempUV;
//         // }
    
//         newMesh.triangles = TrianglesArray;
//         if(red) {
//             meshObj.GetComponent<Renderer>().material = red_material;
//         }
//         else {
//             meshObj.GetComponent<Renderer>().material = new_material;
//         }
        
//         MeshingNodes = GameObject.Find("MeshingNodes");
//         meshObj.transform.parent = MeshingNodes.transform;
//         meshObj.GetComponent<MeshFilter>().mesh.RecalculateNormals();
//     }

//     public void DeleteMesh(String deleteMeshName) {
//         Destroy(GameObject.Find(deleteMeshName)); 
//     }
// }

public class Backup_code : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

