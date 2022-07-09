using System.Collections.Generic;

public class BuildingData
{
    private string _buildingCode;
    private int _healthpoints;

    public BuildingData(string buildingCode, int healthpoints)
    {
        _buildingCode = buildingCode;
        _healthpoints = healthpoints;
    }

    public string BuildingCode { get => _buildingCode; }
    public int HP { get => _healthpoints; }
}