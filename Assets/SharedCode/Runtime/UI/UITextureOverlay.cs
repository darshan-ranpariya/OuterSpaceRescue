using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
    [AddComponentMenu("UI/Effects/Extensions/TextureOverlay")]
    public class UITextureOverlay : BaseMeshEffect
    { 
        public override void ModifyMesh(VertexHelper helper)
        { 
            if (!IsActive() || helper.currentVertCount == 0)
                return;

            List<UIVertex> _vertexList = new List<UIVertex>();

            helper.GetUIVertexStream(_vertexList);

            int nCount = _vertexList.Count;
            float bottom = _vertexList[0].position.y;
            float top = _vertexList[0].position.y;
            float y = 0f;

            for (int i = nCount - 1; i >= 1; --i)
            {
                y = _vertexList[i].position.y;

                if (y > top) top = y;
                else if (y < bottom) bottom = y;
            }

            float height =  (top - bottom);
            UIVertex vertex = new UIVertex();

            for (int i = 0; i < helper.currentVertCount; i++)
            {
                helper.PopulateUIVertex(ref vertex, i);

                //vertex.color = BlendColor(vertex.color, EffectGradient.Evaluate((vertex.position.y - bottom) / height));
                vertex.color = graphic.color;
                vertex.uv1.y = (vertex.position.y - bottom) / height;
                //if (vertex.position.y > 0)
                //{
                //    vertex.uv1.y = 1;
                //}
                //else vertex.uv1.y = 0;
                helper.SetUIVertex(vertex, i);
            }
        } 
    }
}