public class Globals
{
    // used for raycasting mouse onto terrain location
    public static int TERRAIN_LAYER_MASK = 1 << 8;

    // array of building data for each type in the game
    public static BuildingData[] BUILDING_DATA = new BuildingData[]
    {
        new BuildingData("Building", 100)
    };
}