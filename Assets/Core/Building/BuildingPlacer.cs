using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    private Building _placedBuilding = null;
    private Ray _ray;
    private RaycastHit _raycastHit;
    private Vector3 _lastPlacementPosition;

    private void Start()
    {
        _PreparePlacedBuilding(0);
    }

    private void Update()
    {
        if (_placedBuilding != null)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                _CancelPlacedBuilding();
                return;
            }

            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _raycastHit, 1000f, Globals.TERRAIN_LAYER_MASK))
            {
                _placedBuilding.SetPosition(_raycastHit.point);
                if (_lastPlacementPosition != _raycastHit.point)
                {
                    _placedBuilding.CheckValidPlacement();
                }
                _lastPlacementPosition = _raycastHit.point;
            }

            if (_placedBuilding.HasValidPlacement && Input.GetMouseButtonDown(0))
            {
                _PlaceBuilding();
            }
        }
    }

    private void _PreparePlacedBuilding(int buildingDataIndex)
    {
        // destroy previous phantom if one exists
        if (_placedBuilding != null && !_placedBuilding.IsFixed)
        {
            Destroy(_placedBuilding.Transform.gameObject);
        }
        Building building = new Building(Globals.BUILDING_DATA[buildingDataIndex]);

        // link data with BuildingManager
        building.Transform.GetComponent<BuildingManager>().Initialize(building);

        _placedBuilding = building;
        _lastPlacementPosition = Vector3.zero;
    }

    private void _CancelPlacedBuilding()
    {
        // destroy phantom building
        Destroy(_placedBuilding.Transform.gameObject);
        _placedBuilding = null;
    }

    private void _PlaceBuilding()
    {
        _placedBuilding.Place();
        _PreparePlacedBuilding(_placedBuilding.BuildingDataIndex);
    }
}