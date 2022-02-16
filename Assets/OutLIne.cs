using System.Linq;
using UnityEngine;
public class OutLIne : MonoBehaviour
{
   [SerializeField] private Material _outLineMaterial;
   private Material _outlineInstace;
   private Renderer _renderer;
   private const float _activeWide = 0.04f;
   private static readonly int Property = Shader.PropertyToID("_Outline");
   private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
   private Color _lastColor=Color.white;
   private bool _isLock;
   private void Awake()
   {
      _renderer = GetComponent<Renderer>();
      _outlineInstace = Instantiate(_outLineMaterial);
      SetActiveOutLine(false);
   }
   private void OnEnable()=>SetOrRemoveMaterial(_outlineInstace,true);
   private void OnDisable()=>SetOrRemoveMaterial(_outlineInstace,false);
   private void SetOrRemoveMaterial(Material material, bool isAdd)
   {
      var materials=_renderer.sharedMaterials.ToList();
      if(isAdd)materials.Add(_outlineInstace);
      else materials.Remove(_outlineInstace);
      _renderer.materials = materials.ToArray();
   }
   public void SetActiveOutLine(bool isActive)
   {
      if (_isLock) return;
      float wide = isActive ? _activeWide : 0; 
      if(isActive==false)_outlineInstace.SetColor(OutlineColor,_lastColor);
      _outlineInstace.SetFloat(Property,wide);
   }
   public void SetOutlineColor(Color color)
   {
      _isLock = true;
      _lastColor = color;
      _outlineInstace.SetColor(OutlineColor,_lastColor);
      _outlineInstace.SetFloat(Property,_activeWide);
   }
}
