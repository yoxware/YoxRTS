using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingPlacement
{
    VALID,
    INVALID,
    FIXED
};

public class Building
{
    private BuildingData _data;
    private Transform _transform;
    private int _currentHealth;
    private BuildingPlacement _placement;
    private List<Material> _materials;

    private BuildingManager _buildingManager;

    public Building(BuildingData data)
    {
        _data = data;
        _currentHealth = data.HP;

        GameObject gameobj = GameObject.Instantiate(Resources.Load($"Prefabs/Buildings/{_data.BuildingCode}")) as GameObject;
        _transform = gameobj.transform;
        _buildingManager = gameobj.GetComponent<BuildingManager>();


        _placement = BuildingPlacement.VALID;

        _materials = new List<Material>();
        foreach (Material material in _transform.Find("Mesh").GetComponent<Renderer>().materials)
        {
            _materials.Add(new Material(material));
        }
        SetMaterials();
    }

    public void SetPosition(Vector3 position)
    {
        _transform.position = position;
    }

    public void SetMaterials() { SetMaterials(_placement); }
    public void SetMaterials(BuildingPlacement placement)
    {
        List<Material> materials;
        if (placement == BuildingPlacement.VALID)
        {
            Material refMaterial = Resources.Load("Materials/Valid") as Material;
            materials = new List<Material>();
            for (int i = 0; i < _materials.Count; i++)
            {
                materials.Add(refMaterial);
            }
        }
        else if (placement == BuildingPlacement.INVALID)
        {
            Material refMaterial = Resources.Load("Materials/Invalid") as Material;
            materials = new List<Material>();
            for (int i = 0; i < _materials.Count; i++)
            {
                materials.Add(refMaterial);
            }
        }
        else if (placement == BuildingPlacement.FIXED)
        {
            materials = _materials;
        }
        else
        {
            return;
        }
        _transform.Find("Mesh").GetComponent<Renderer>().materials = materials.ToArray();
    }

    public void Place()
    {
        // set placement state to FIXED
        _placement = BuildingPlacement.FIXED;

        // change building materials
        SetMaterials();

        // remove box collider trigger from box collider to enable collisions
        _transform.GetComponent<BoxCollider>().isTrigger = false;
    }

    public void CheckValidPlacement()
    {
        if (_placement == BuildingPlacement.FIXED) return;
        _placement = _buildingManager.CheckPlacement()
            ? BuildingPlacement.VALID
            : BuildingPlacement.INVALID;
    }

    public string BuildingCode { get => _data.BuildingCode; }
    public Transform Transform { get => _transform; }
    public int HP { get => _currentHealth; set => _currentHealth = value; }
    public int MaxHP { get => _data.HP; }
    public bool IsFixed { get => _placement == BuildingPlacement.FIXED; }
    public bool HasValidPlacement { get => _placement == BuildingPlacement.VALID; }

    public int BuildingDataIndex
    {
        get
        {
            for (int i = 0; i < Globals.BUILDING_DATA.Length; i++)
            {
                if (Globals.BUILDING_DATA[i].BuildingCode == _data.BuildingCode)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
