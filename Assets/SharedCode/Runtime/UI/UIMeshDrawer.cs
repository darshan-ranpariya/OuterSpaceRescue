using UnityEngine;
using System.Collections;
using UnityEngine.UI;    
using UnityEngine.UI.Extensions;

[AddComponentMenu("UI/Effects/Extensions/Mesh Drawer")]  
public class UIMeshDrawer : MaskableGraphic 
{  
    public Mesh refMesh;
    public float scale = 1;

    [SerializeField] private Sprite m_Sprite;
    //public Sprite sprite { get { return m_Sprite; } set { if (SetPropertyUtility.SetClass(ref m_Sprite, value)) SetAllDirty(); } } 

    public override Texture mainTexture{
        get
        {
            return m_Sprite == null ? s_WhiteTexture : m_Sprite.texture;
        }
    } 

    protected override void OnPopulateMesh(VertexHelper vh)
    {   
        if (refMesh == null) return;

        vh.Clear();

        UIVertex vert = UIVertex.simpleVert;

        for (int i = 0; i < refMesh.vertexCount; i++)
        {
            vert.position = refMesh.vertices[i]*scale;
            vert.color = color;
            vert.uv0 = refMesh.uv[i];
            vert.normal = refMesh.normals[i];
            vh.AddVert(vert); 
//            Debug.LogFormat("v: ({0:0.0000}, {1:0.0000}, {2:0.0000})",vert.position.x,vert.position.y,vert.position.z); 

        }

        int[] ttt = refMesh.triangles;
        for (int i = 0; i < refMesh.triangles.Length/3; i++)
        {
            vh.AddTriangle(ttt[i*3], ttt[i*3+1], ttt[i*3+2]);
//            Debug.LogFormat("t: ({0}, {1}, {2})",ttt[i*3], ttt[i*3+1], ttt[i*3+2]);
        } 
    }   
     

//    public Sprite[] sprites;
//    void OnEnable()
//    {
//        for (int i = 0; i < sprites.Length; i++)
//        {
//            new Delayed.Action<int>((ii) =>
//                {
//                    sprite = sprites[ii];
//                }, i, i);
//        }
//    }
}
